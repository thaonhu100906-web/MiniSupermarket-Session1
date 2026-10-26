
using System.ComponentModel.DataAnnotations;

namespace TranThiThaoNhu_2124110075.API.DTOs.Customers
{
    public class CustomerCreateDto
    {
        [Required(ErrorMessage = "Tên khách hàng không được để trống!")]
        [StringLength(100, ErrorMessage = "Tên khách hàng không vượt quá 100 ký tự!")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại không được để trống!")]
        [StringLength(15, ErrorMessage = "Số điện thoại không vượt quá 15 ký tự!")]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Địa chỉ không vượt quá 200 ký tự!")]
        public string? Address { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Điểm thưởng không được âm!")]
        public int RewardPoints { get; set; } = 0;

        [StringLength(50, ErrorMessage = "Hạng thành viên không vượt quá 50 ký tự!")]
        public string MembershipRank { get; set; } = "Chuẩn";
    }
}
