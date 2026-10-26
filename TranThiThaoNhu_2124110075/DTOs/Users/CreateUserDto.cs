
using System.ComponentModel.DataAnnotations;

namespace TranThiThaoNhu_2124110075.API.DTOs.Users
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Tên tài khoản không được để trống!")]
        [StringLength(50, ErrorMessage = "Tên tài khoản không vượt quá 50 ký tự!")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống!")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự!")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ tên không được để trống!")]
        [StringLength(100, ErrorMessage = "Họ tên không vượt quá 100 ký tự!")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn vai trò!")]
        public string Role { get; set; } = "Cashier";
    }
}
