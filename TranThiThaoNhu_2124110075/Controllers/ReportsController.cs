using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;
using TranThiThaoNhu_2124110075.API.Data;

namespace TranThiThaoNhu_2124110075.API.Controllers
{
    [ApiController]
    [Route("api/reports")]
    [Authorize(Roles = "Admin,Warehouse")]
    public class ReportsController : ControllerBase
    {
        private readonly SupermarketDbContext _db;
        public ReportsController(SupermarketDbContext db) => _db = db;

        [HttpGet("dashboard-summary")]
        public async Task<IActionResult> DashboardSummary()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var revenue = await _db.Orders.Where(o => o.OrderDate >= today && o.OrderDate < tomorrow)
                .Select(o => (decimal?)o.TotalAmount).SumAsync() ?? 0m;
            var orders = await _db.Orders.CountAsync();
            var newCustomers = await _db.Customers.CountAsync(c => c.CreatedAt.HasValue && c.CreatedAt.Value >= today && c.CreatedAt.Value < tomorrow);
            var lowStock = await _db.Products.CountAsync(p => p.StockQuantity <= 10);
            return Ok(new { revenueToday = revenue, orderCount = orders, newCustomersToday = newCustomers, lowStockCount = lowStock });
        }

        [HttpGet("revenue-by-day")]
        public async Task<IActionResult> RevenueByDay([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            if (from.Date > to.Date)
                return BadRequest(new { message = "Ngày bắt đầu không được lớn hơn ngày kết thúc." });

            // Stored Procedure gom nhóm doanh thu theo CAST(OrderDate AS DATE).
            // @EndDate là mốc kết thúc loại trừ để lấy trọn ngày được chọn.
            var startDate = from.Date;
            var endDate = to.Date.AddDays(1);
            var rows = new List<RevenueByDayRow>();
            var connection = _db.Database.GetDbConnection();
            var shouldCloseConnection = connection.State != ConnectionState.Open;

            try
            {
                if (shouldCloseConnection)
                    await connection.OpenAsync();

                await using var command = connection.CreateCommand();
                command.CommandText = "dbo.GetRevenueByDay";
                command.CommandType = CommandType.StoredProcedure;

                var startParameter = command.CreateParameter();
                startParameter.ParameterName = "@StartDate";
                startParameter.DbType = DbType.DateTime2;
                startParameter.Value = startDate;
                command.Parameters.Add(startParameter);

                var endParameter = command.CreateParameter();
                endParameter.ParameterName = "@EndDate";
                endParameter.DbType = DbType.DateTime2;
                endParameter.Value = endDate;
                command.Parameters.Add(endParameter);

                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    rows.Add(new RevenueByDayRow
                    {
                        Date = reader.GetDateTime(0).Date,
                        Revenue = reader.GetDecimal(1),
                        OrderCount = reader.GetInt32(2)
                    });
                }
            }
            finally
            {
                if (shouldCloseConnection && connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }

            // Bổ sung ngày không có hóa đơn để biểu đồ luôn hiển thị liên tục theo khoảng lọc.
            var rowsByDate = rows.ToDictionary(x => x.Date.Date);
            var result = Enumerable.Range(0, (to.Date - from.Date).Days + 1).Select(i =>
            {
                var date = from.Date.AddDays(i);
                rowsByDate.TryGetValue(date, out var row);
                return new
                {
                    date,
                    revenue = row?.Revenue ?? 0m,
                    orderCount = row?.OrderCount ?? 0
                };
            });

            return Ok(result);
        }

        private sealed class RevenueByDayRow
        {
            public DateTime Date { get; set; }
            public decimal Revenue { get; set; }
            public int OrderCount { get; set; }
        }


        [HttpGet("export/products.csv")]
        public async Task<IActionResult> ExportProductsCsv()
        {
            var products = await _db.Products.AsNoTracking().Include(p => p.Category)
                .OrderBy(p => p.ProductName).ToListAsync();
            static string Csv(string? value) => "\"" + (value ?? "").Replace("\"", "\"\"") + "\"";
            var sb = new StringBuilder("MaSP,MaVach,TenSanPham,DonGia,TonKho,DanhMuc\r\n");
            foreach (var p in products)
                sb.AppendLine($"{p.ProductId},{Csv(p.Barcode)},{Csv(p.ProductName)},{p.Price},{p.StockQuantity},{Csv(p.Category?.CategoryName)}");
            return File(Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray(), "text/csv; charset=utf-8", "DanhMucSanPham.csv");
        }

        [HttpGet("export/orders.csv")]
        public async Task<IActionResult> ExportOrdersCsv([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var query = _db.Orders.AsNoTracking().Include(o => o.OrderDetails).AsQueryable();
            if (from.HasValue) query = query.Where(o => o.OrderDate >= from.Value.Date);
            if (to.HasValue) query = query.Where(o => o.OrderDate < to.Value.Date.AddDays(1));
            var orders = await query.OrderByDescending(o => o.OrderDate).ToListAsync();
            var sb = new StringBuilder("MaHoaDon,NgayLap,SoLuongSanPham,TongTien\r\n");
            foreach (var o in orders) sb.AppendLine($"{o.OrderId},{o.OrderDate:yyyy-MM-dd HH:mm:ss},{o.OrderDetails.Sum(d => d.Quantity)},{o.TotalAmount}");
            return File(Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray(), "text/csv; charset=utf-8", "BaoCaoHoaDon.csv");
        }

        [HttpGet("export/revenue.csv")]
        public async Task<IActionResult> ExportRevenueCsv([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            if (from.Date > to.Date)
                return BadRequest(new { message = "Ngày bắt đầu không được lớn hơn ngày kết thúc." });

            var end = to.Date.AddDays(1);
            var rows = await _db.Orders.AsNoTracking()
                .Where(o => o.OrderDate >= from.Date && o.OrderDate < end)
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new { Date = g.Key, OrderCount = g.Count(), Revenue = g.Sum(x => x.TotalAmount) })
                .OrderBy(x => x.Date)
                .ToListAsync();

            var sb = new StringBuilder("Ngay,SoHoaDon,DoanhThu\r\n");
            foreach (var row in rows)
                sb.AppendLine($"{row.Date:yyyy-MM-dd},{row.OrderCount},{row.Revenue.ToString(System.Globalization.CultureInfo.InvariantCulture)}");

            var bytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
            return File(bytes, "text/csv; charset=utf-8", "BaoCaoDoanhThu.csv");
        }
        //=================================================================================46
        [HttpGet("best-sellers")]
        public async Task<IActionResult> BestSellers([FromQuery] int take = 5)
        {
            take = Math.Clamp(take, 1, 20);
            var result = await _db.OrderDetails.AsNoTracking()
                .GroupBy(d => new { d.ProductId, d.Product!.ProductName })
                .Select(g => new { productId = g.Key.ProductId, productName = g.Key.ProductName,
                    quantitySold = g.Sum(x => x.Quantity), revenue = g.Sum(x => x.Quantity * x.UnitPrice) })
                .OrderByDescending(x => x.quantitySold).ThenBy(x => x.productName).Take(take).ToListAsync();
            return Ok(result);
        }
    }
}
