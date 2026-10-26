
using System.ComponentModel.DataAnnotations;

namespace TranThiThaoNhu_2124110075.API.DTOs.Orders
{
    public class CheckoutRequestDto
    {
        public string? CashierUsername { get; set; }

        public string? CustomerPhone { get; set; }

        [Required(ErrorMessage = "Giỏ hàng không được để trống!")]
        [MinLength(1, ErrorMessage = "Giỏ hàng phải có ít nhất một sản phẩm!")]
        public List<CheckoutItemRequestDto> Items { get; set; } = new();
    }

    public class CheckoutItemRequestDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Sản phẩm không hợp lệ!")]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0!")]
        public int Quantity { get; set; }
    }
}
