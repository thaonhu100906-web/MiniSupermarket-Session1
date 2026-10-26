//họ tên: Trần Thị Thảo Như
//Mssv: 2124110075
//Lớp: CCQ2411C
//Môn: lập trình ứng dụng

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TranThiThaoNhu_2124110075.API.Data;
using TranThiThaoNhu_2124110075.API.Models;
using TranThiThaoNhu_2124110075.API.Services;

namespace TranThiThaoNhu_2124110075.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly SupermarketDbContext _context;
        private readonly ProductAdoSearchService _adoSearchService;

        public ProductsController(SupermarketDbContext context, ProductAdoSearchService adoSearchService)
        {
            _context = context;
            _adoSearchService = adoSearchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.Products.Include(p => p.Category).AsNoTracking().ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _context.Products.Include(p => p.Category).AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm trong CSDL!" });
            }
            return Ok(product);
        }

        // ADO.NET demo: gọi Stored Procedure bằng SqlParameter và đọc kết quả bằng SqlDataReader.
        // Cần chạy Scripts/Products_SearchByKeyword.sql một lần trên database hiện có.
        [HttpGet("ado-search")]
        public async Task<IActionResult> SearchWithAdo([FromQuery] string keyword, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest(new { message = "Vui lòng nhập từ khóa tìm kiếm!" });
            if (keyword.Length > 150)
                return BadRequest(new { message = "Từ khóa không được vượt quá 150 ký tự." });

            var result = await _adoSearchService.SearchAsync(keyword, cancellationToken);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest(new { message = "Vui lòng nhập từ khóa tìm kiếm!" });
            }

            var result = await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .Where(p => p.ProductName.Contains(keyword) || p.Barcode.Contains(keyword))
                .ToListAsync();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] Product newProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (string.IsNullOrWhiteSpace(newProduct.Barcode) ||
                string.IsNullOrWhiteSpace(newProduct.ProductName))
                return BadRequest(new { message = "Ma vach va ten san pham khong duoc de trong." });

            if (newProduct.Price <= 0)
                return BadRequest(new { message = "Gia san pham phai lon hon 0." });

            if (newProduct.StockQuantity < 0)
                return BadRequest(new { message = "So luong ton kho khong duoc am." });

            if (!await _context.Categories.AnyAsync(c => c.CategoryId == newProduct.CategoryId))
                return BadRequest(new { message = "Nhom hang khong ton tai." });

            if (await _context.Products.AnyAsync(p => p.Barcode == newProduct.Barcode))
                return BadRequest(new { message = "Ma vach san pham da ton tai." });

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newProduct.ProductId }, newProduct);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] Product updateProduct)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm cần sửa!" });
            }
            if (string.IsNullOrWhiteSpace(updateProduct.Barcode) ||
                string.IsNullOrWhiteSpace(updateProduct.ProductName))
                return BadRequest(new { message = "Ma vach va ten san pham khong duoc de trong." });

            if (updateProduct.Price <= 0)
                return BadRequest(new { message = "Gia san pham phai lon hon 0." });

            if (updateProduct.StockQuantity < 0)
                return BadRequest(new { message = "So luong ton kho khong duoc am." });

            if (!await _context.Categories.AnyAsync(c => c.CategoryId == updateProduct.CategoryId))
                return BadRequest(new { message = "Nhom hang khong ton tai." });

            if (await _context.Products.AnyAsync(p => p.Barcode == updateProduct.Barcode && p.ProductId != id))
                return BadRequest(new { message = "Ma vach san pham da ton tai." });

            product.Barcode = updateProduct.Barcode;
            product.ProductName = updateProduct.ProductName;
            product.Price = updateProduct.Price;
            product.StockQuantity = updateProduct.StockQuantity;
            product.CategoryId = updateProduct.CategoryId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm cần xóa!" });
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? keyword = null)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest(new
                {
                    message = "pageNumber phai >= 1 va pageSize phai tu 1 den 100."
                });
            }

            var query = _context.Products.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                query = query.Where(p =>
                    p.ProductName.Contains(keyword) ||
                    p.Barcode.Contains(keyword));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(p => p.ProductId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new TranThiThaoNhu_2124110075.API.DTOs.Products.ProductListItemDto
                {
                    ProductId = p.ProductId,
                    Barcode = p.Barcode,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.CategoryName
                })
                .ToListAsync();

            var result = new TranThiThaoNhu_2124110075.API.DTOs.Products.PagedResultDto<TranThiThaoNhu_2124110075.API.DTOs.Products.ProductListItemDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            return Ok(result);
        }
    }
}

