using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TranThiThaoNhu_2124110075.API.Data;
using TranThiThaoNhu_2124110075.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SupermarketDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public AuthController(IConfiguration configuration, SupermarketDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        // Endpoint Đăng nhập: POST /api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            // Tài khoản demo được kiểm tra trước khi truy vấn database.
            // Nhờ đó admin/123456 vẫn đăng nhập được ngay cả khi bảng Users
            // chưa được migration hoặc database chưa sẵn sàng.
            if (request.Username == "admin" && request.Password == "123456")
            {
                var adminToken = GenerateJwtToken("admin", "Admin");
                return Ok(new { success = true, token = adminToken, role = "Admin" });
            }

            if (request.Username == "cashier" && request.Password == "123456")
            {
                var cashierToken = GenerateJwtToken("cashier", "Cashier");
                return Ok(new { success = true, token = cashierToken, role = "Cashier" });
            }

            // Sau đó mới xác thực tài khoản được quản trị trong cơ sở dữ liệu.
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user != null)
            {
                try
                {
                    var verification = _passwordHasher.VerifyHashedPassword(
                        user, user.PasswordHash, request.Password);
                    if (verification != PasswordVerificationResult.Failed)
                    {
                        var dbToken = GenerateJwtToken(user.Username, user.Role);
                        return Ok(new { success = true, token = dbToken, role = user.Role });
                    }
                }
                catch (FormatException)
                {
                    // Dữ liệu seed cũ có chuỗi giữ chỗ thay vì password hash.
                }
            }

            return Unauthorized(new { success = false, message = "Sai tài khoản hoặc mật khẩu!" });
        }

        private string GenerateJwtToken(string username, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            // Lấy khóa bí mật từ appsettings.json
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"] ?? "SupermarketSecretKeyDoAnMonHoc2026SecureString!!");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(2), // Thời hạn token là 2 tiếng
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class LoginRequestDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
