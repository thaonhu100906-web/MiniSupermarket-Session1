
// Họ tên: Trần Thị Thảo Như
// MSSV: 2124110075
// Lớp: CCQ2411C
// Môn: lập trình ứng dụng

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TranThiThaoNhu_2124110075.API.Data;
using TranThiThaoNhu_2124110075.API.DTOs.Users;
using TranThiThaoNhu_2124110075.API.Models;

namespace TranThiThaoNhu_2124110075.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly SupermarketDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public UsersController(SupermarketDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users
                .AsNoTracking()
                .Select(e => new
                {
                    e.UserId,
                    e.Username,
                    e.FullName,
                    e.Role
                })
                .ToListAsync();

            return Ok(users);
        }

        // Phân trang tài khoản; chỉ trả thông tin công khai, không trả PasswordHash.
        // GET api/Users/paged?pageNumber=1&pageSize=10
        [HttpGet("paged")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsersPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyword = null)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                return BadRequest(new { message = "pageNumber phải >= 1 và pageSize từ 1 đến 100." });

            var query = _context.Users.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                query = query.Where(u => u.Username.Contains(keyword) || u.FullName.Contains(keyword));
            }

            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(u => u.UserId)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Select(u => new { u.UserId, u.Username, u.FullName, u.Role }).ToListAsync();
            return Ok(new TranThiThaoNhu_2124110075.API.DTOs.Products.PagedResultDto<object>
            {
                Items = items.Cast<object>().ToList(), PageNumber = pageNumber, PageSize = pageSize,
                TotalCount = totalCount, TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
                return NotFound(new { message = "Không tìm thấy tài khoản." });

            return Ok(new
            {
                user.UserId,
                user.Username,
                user.FullName,
                user.Role
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser(
            [FromBody] CreateUserDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = request.Username.Trim();
            var fullName = request.FullName.Trim();
            var role = request.Role.Trim();

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(fullName))
            {
                return BadRequest(new
                {
                    message = "Tài khoản và họ tên không được để trống."
                });
            }

            if (!IsValidRole(role))
                return BadRequest(new { message = "Vai trò không hợp lệ." });

            bool usernameExists = await _context.Users
                .AnyAsync(e => e.Username == username);

            if (usernameExists)
                return BadRequest(new { message = "Tên tài khoản đã tồn tại." });

            var user = new User
            {
                Username = username,
                FullName = fullName,
                Role = role,
                PasswordHash = string.Empty
            };

            user.PasswordHash = _passwordHasher.HashPassword(
                user, request.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetUser),
                new { id = user.UserId },
                new
                {
                    user.UserId,
                    user.Username,
                    user.FullName,
                    user.Role
                });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(
            int id,
            [FromBody] UpdateUserDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound(new { message = "Không tìm thấy tài khoản." });

            var username = request.Username.Trim();
            var fullName = request.FullName.Trim();
            var role = request.Role.Trim();

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(fullName))
            {
                return BadRequest(new
                {
                    message = "Tài khoản và họ tên không được để trống."
                });
            }

            if (!IsValidRole(role))
                return BadRequest(new { message = "Vai trò không hợp lệ." });

            bool usernameExists = await _context.Users.AnyAsync(
                e => e.Username == username && e.UserId != id);

            if (usernameExists)
                return BadRequest(new { message = "Tên tài khoản đã tồn tại." });

            user.Username = username;
            user.FullName = fullName;
            user.Role = role;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Cập nhật tài khoản thành công.",
                user.UserId,
                user.Username,
                user.FullName,
                user.Role
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound(new { message = "Không tìm thấy tài khoản." });

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa tài khoản thành công." });
        }

        private static bool IsValidRole(string role)
        {
            return role == "Admin" ||
                   role == "Warehouse" ||
                   role == "Cashier";
        }
    }
}
