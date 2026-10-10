
// Họ tên: Trần Thị Thảo Như
// MSSV: 2124110075
// Lớp: CCQ2411C
// Môn: lập trình ứng dụng

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TranThiThaoNhu_2124110075.API.Data;
using TranThiThaoNhu_2124110075.API.Models;

namespace TranThiThaoNhu_2124110075.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly SupermarketDbContext _context;

        public CategoriesController(SupermarketDbContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .ToListAsync();

            return Ok(categories);
        }

        // GET: api/Categories/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy danh mục!"
                });
            }

            return Ok(category);
        }

        // GET: api/Categories/search?keyword=banh
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest(new
                {
                    message = "Vui lòng nhập từ khóa!"
                });
            }

            var result = await _context.Categories
                .AsNoTracking()
                .Where(c => c.CategoryName.Contains(keyword.Trim()))
                .ToListAsync();

            return Ok(result);
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category newCat)
        {
            if (string.IsNullOrWhiteSpace(newCat.CategoryName))
            {
                return BadRequest(new
                {
                    message = "Tên danh mục không được để trống!"
                });
            }

            string categoryName = newCat.CategoryName.Trim();

            bool nameExists = await _context.Categories
                .AnyAsync(c => c.CategoryName == categoryName);

            if (nameExists)
            {
                return BadRequest(new
                {
                    message = "Tên danh mục đã tồn tại!"
                });
            }

            newCat.CategoryName = categoryName;

            _context.Categories.Add(newCat);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = newCat.CategoryId },
                newCat
            );
        }

        // PUT: api/Categories/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] Category updateCat)
        {
            if (string.IsNullOrWhiteSpace(updateCat.CategoryName))
            {
                return BadRequest(new
                {
                    message = "Tên danh mục không được để trống!"
                });
            }

            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy danh mục cần sửa!"
                });
            }

            string categoryName = updateCat.CategoryName.Trim();

            bool nameExists = await _context.Categories.AnyAsync(
                c => c.CategoryName == categoryName &&
                     c.CategoryId != id);

            if (nameExists)
            {
                return BadRequest(new
                {
                    message = "Tên danh mục đã được sử dụng!"
                });
            }

            category.CategoryName = categoryName;
            category.Description = updateCat.Description;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Cập nhật danh mục thành công!",
                category
            });
        }

        // DELETE: api/Categories/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy danh mục cần xóa!"
                });
            }

            // Kiểm tra sản phẩm thuộc danh mục trước khi xóa
            bool hasProducts = await _context.Products
                .AnyAsync(p => p.CategoryId == id);

            if (hasProducts)
            {
                return BadRequest(new
                {
                    message = "Không thể xóa danh mục vì vẫn còn sản phẩm thuộc danh mục này. Vui lòng chuyển sản phẩm sang danh mục khác trước."
                });
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Xóa danh mục thành công!"
            });
        }

        // GET: api/Categories/admin-dashboard
        [HttpGet("admin-dashboard")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdminDashboard()
        {
            return Ok(new
            {
                message = "Chào mừng Admin! Bạn có toàn quyền quản trị hệ thống siêu thị mini."
            });
        }

        // GET: api/Categories/staff-pos
        [HttpGet("staff-pos")]
        [Authorize(Roles = "Admin,Cashier")]
        public IActionResult GetStaffPos()
        {
            return Ok(new
            {
                message = "Màn hình POS Thu ngân sẵn sàng phục vụ bán hàng."
            });
        }
    }
}