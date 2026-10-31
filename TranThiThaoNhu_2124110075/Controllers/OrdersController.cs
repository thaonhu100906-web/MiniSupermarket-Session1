
// Họ tên: Trần Thị Thảo Như
// MSSV: 2124110075
// Lớp: CCQ2411C
// Môn: lập trình ứng dụng

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using TranThiThaoNhu_2124110075.API.Data;
using TranThiThaoNhu_2124110075.API.DTOs.Orders;
using TranThiThaoNhu_2124110075.API.Models;

namespace TranThiThaoNhu_2124110075.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly SupermarketDbContext _context;

        public OrdersController(SupermarketDbContext context)
        {
            _context = context;
        }

        // Lấy danh sách hóa đơn để báo cáo doanh thu
        [HttpGet]
        public async Task<IActionResult> GetOrders(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            if (from.HasValue && to.HasValue && from.Value.Date > to.Value.Date)
            {
                return BadRequest(new
                {
                    message = "Ngày bắt đầu không được sau ngày kết thúc!"
                });
            }

            var query = _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderDetails)
                .AsQueryable();

            if (from.HasValue)
            {
                query = query.Where(o => o.OrderDate >= from.Value.Date);
            }

            if (to.HasValue)
            {
                var endDate = to.Value.Date.AddDays(1);
                query = query.Where(o => o.OrderDate < endDate);
            }

            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderListItemDto
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    ItemCount = o.OrderDetails.Sum(d => d.Quantity),
                    TotalAmount = o.TotalAmount
                })
                .ToListAsync();

            return Ok(orders);
        }


        // Phân trang hóa đơn phía máy chủ: GET api/orders/paged?pageNumber=1&pageSize=10
        [HttpGet("paged")]
        public async Task<IActionResult> GetOrdersPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10,
            [FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                return BadRequest(new { message = "pageNumber phải >= 1 và pageSize từ 1 đến 100." });
            if (from.HasValue && to.HasValue && from.Value.Date > to.Value.Date)
                return BadRequest(new { message = "Ngày bắt đầu không được sau ngày kết thúc." });

            var query = _context.Orders.AsNoTracking().Include(o => o.OrderDetails).AsQueryable();
            if (from.HasValue) query = query.Where(o => o.OrderDate >= from.Value.Date);
            if (to.HasValue) query = query.Where(o => o.OrderDate < to.Value.Date.AddDays(1));
            var totalCount = await query.CountAsync();
            var items = await query.OrderByDescending(o => o.OrderDate).ThenByDescending(o => o.OrderId)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Select(o => new OrderListItemDto { OrderId = o.OrderId, OrderDate = o.OrderDate,
                    ItemCount = o.OrderDetails.Sum(d => d.Quantity), TotalAmount = o.TotalAmount }).ToListAsync();
            return Ok(new TranThiThaoNhu_2124110075.API.DTOs.Products.PagedResultDto<OrderListItemDto>
            {
                Items = items, PageNumber = pageNumber, PageSize = pageSize,
                TotalCount = totalCount, TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            });
        }

        // Bao cao doanh thu theo ca/khoang thoi gian bang Stored Procedure ADO.NET.
        [HttpGet("shift-revenue")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetShiftRevenue(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to,
            CancellationToken cancellationToken)
        {
            if (from >= to)
                return BadRequest(new { message = "Thoi gian bat dau phai nho hon thoi gian ket thuc." });

            var connection = _context.Database.GetDbConnection();
            bool openedHere = connection.State != ConnectionState.Open;
            try
            {
                if (openedHere) await connection.OpenAsync(cancellationToken);
                await using var command = connection.CreateCommand();
                command.CommandText = "dbo.GetShiftRevenue";
                command.CommandType = CommandType.StoredProcedure;

                var startParameter = command.CreateParameter();
                startParameter.ParameterName = "@StartDate";
                startParameter.DbType = DbType.DateTime2;
                startParameter.Value = from;
                command.Parameters.Add(startParameter);

                var endParameter = command.CreateParameter();
                endParameter.ParameterName = "@EndDate";
                endParameter.DbType = DbType.DateTime2;
                endParameter.Value = to;
                command.Parameters.Add(endParameter);

                await using var reader = await command.ExecuteReaderAsync(cancellationToken);
                if (!await reader.ReadAsync(cancellationToken))
                    return Ok(new { orderCount = 0, totalRevenue = 0m, from, to });

                return Ok(new
                {
                    orderCount = reader.GetInt64(reader.GetOrdinal("OrderCount")),
                    totalRevenue = reader.GetDecimal(reader.GetOrdinal("TotalRevenue")),
                    startDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                    endDate = reader.GetDateTime(reader.GetOrdinal("EndDate"))
                });
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new { message = "Khong the chay bao cao doanh thu. Hay chay Scripts/ShiftRevenueReport.sql truoc.", detail = ex.Message });
            }
            finally
            {
                if (openedHere) await connection.CloseAsync();
            }
        }

        // Thanh toán và lưu hóa đơn
        [HttpPost("checkout")]
        [Authorize(Roles = "Admin,Cashier")]
        public async Task<IActionResult> Checkout(
            [FromBody] CheckoutRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request.Items == null || request.Items.Count == 0)
            {
                return BadRequest(new
                {
                    message = "Giỏ hàng đang trống!"
                });
            }
            //================================================================================================47
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    TotalAmount = 0
                };

                foreach (var item in request.Items)
                {
                    var product = await _context.Products
                        .FirstOrDefaultAsync(
                            p => p.ProductId == item.ProductId);

                    if (product == null)
                    {
                        await transaction.RollbackAsync();

                        return BadRequest(new
                        {
                            message = $"Không tìm thấy sản phẩm ID {item.ProductId}."
                        });
                    }

                    if (product.StockQuantity < item.Quantity)
                    {
                        await transaction.RollbackAsync();

                        return BadRequest(new
                        {
                            message = $"Sản phẩm {product.ProductName} không đủ tồn kho."
                        });
                    }

                    decimal unitPrice = product.Price;

                    order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = product.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = unitPrice
                    });

                    order.TotalAmount += unitPrice * item.Quantity;
                    product.StockQuantity -= item.Quantity;
                }

                // Tiêu chí 47: đổi điểm và lưu hóa đơn trong cùng transaction.
                if (request.RedeemPoints > 0)
                {
                    if (string.IsNullOrWhiteSpace(request.CustomerPhone))
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(new { message = "Vui lòng nhập số điện thoại khách hàng để đổi điểm." });
                    }

                    var customer = await _context.Customers.FirstOrDefaultAsync(c => c.PhoneNumber == request.CustomerPhone.Trim());
                    if (customer == null)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(new { message = "Không tìm thấy khách hàng theo số điện thoại đã nhập." });
                    }
                    if (customer.RewardPoints < request.RedeemPoints)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(new { message = $"Khách hàng chỉ có {customer.RewardPoints} điểm, không đủ để đổi." });
                    }

                    decimal discount = request.RedeemPoints * 1000m;
                    if (discount > order.TotalAmount)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(new { message = "Số điểm đổi vượt quá tổng tiền hóa đơn." });
                    }
                    customer.RewardPoints -= request.RedeemPoints;
                    order.TotalAmount -= discount;
                }

                if (request.CashReceived < order.TotalAmount)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new
                    {
                        message = $"Tiền khách đưa chưa đủ. Khách còn thiếu {order.TotalAmount - request.CashReceived:N0} đ."
                    });
                }

                _context.Orders.Add(order);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    message = "Thanh toán thành công!",
                    orderId = order.OrderId,
                    orderDate = order.OrderDate,
                    totalAmount = order.TotalAmount,
                    cashReceived = request.CashReceived,
                    changeAmount = request.CashReceived - order.TotalAmount,
                    redeemedPoints = request.RedeemPoints,
                    discountAmount = request.RedeemPoints * 1000m
                });
            }
            catch
            {
                await transaction.RollbackAsync();

                return StatusCode(500, new
                {
                    message = "Thanh toán thất bại, không thể lưu hóa đơn."
                });
            }
        }
    }
}
