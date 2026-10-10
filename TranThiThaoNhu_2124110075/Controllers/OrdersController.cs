
// Họ tên: Trần Thị Thảo Như
// MSSV: 2124110075
// Lớp: CCQ2411C
// Môn: lập trình ứng dụng

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TranThiThaoNhu_2124110075.API.Data;
using TranThiThaoNhu_2124110075.API.Models;

namespace TranThiThaoNhu_2124110075.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
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
                .Select(o => new
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    ItemCount = o.OrderDetails.Sum(d => d.Quantity),
                    TotalAmount = o.TotalAmount
                })
                .ToListAsync();

            return Ok(orders);
        }

        // Thanh toán và lưu hóa đơn
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(
            [FromBody] CheckoutRequest request)
        {
            if (request.Items == null || request.Items.Count == 0)
            {
                return BadRequest(new
                {
                    message = "Giỏ hàng đang trống!"
                });
            }

            if (request.Items.Any(x => x.Quantity <= 0))
            {
                return BadRequest(new
                {
                    message = "Số lượng sản phẩm phải lớn hơn 0!"
                });
            }

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
                            message =
                                $"Không tìm thấy sản phẩm ID {item.ProductId}."
                        });
                    }

                    if (product.StockQuantity < item.Quantity)
                    {
                        await transaction.RollbackAsync();

                        return BadRequest(new
                        {
                            message =
                                $"Sản phẩm {product.ProductName} không đủ tồn kho."
                        });
                    }

                    // Lấy giá từ database để tính tiền
                    decimal unitPrice = product.Price;

                    order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = product.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = unitPrice
                    });

                    order.TotalAmount += unitPrice * item.Quantity;

                    // Trừ số lượng tồn kho
                    product.StockQuantity -= item.Quantity;
                }

                _context.Orders.Add(order);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    message = "Thanh toán thành công!",
                    orderId = order.OrderId,
                    orderDate = order.OrderDate,
                    totalAmount = order.TotalAmount
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return StatusCode(500, new
                {
                    message = "Thanh toán thất bại, không thể lưu hóa đơn.",
                    error = ex.Message
                });
            }
        }
    }

    public class CheckoutRequest
    {
        public string? CashierUsername { get; set; }
        public string? CustomerPhone { get; set; }
        public List<CheckoutItemRequest> Items { get; set; } = new();
    }

    public class CheckoutItemRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
