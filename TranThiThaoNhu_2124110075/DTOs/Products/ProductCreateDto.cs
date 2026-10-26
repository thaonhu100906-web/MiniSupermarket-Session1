// Họ tên: Trần Thị Thảo Như
// MSSV: 2124110075
// Lớp: CCQ2411C
// Môn: lập trình ứng dụng

using System.ComponentModel.DataAnnotations;

namespace TranThiThaoNhu_2124110075.API.DTOs.Products
{
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "Mã vạch sản phẩm không được để trống.")]
        [StringLength(50)]
        public string Barcode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên sản phẩm không được để trống.")]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "9999999999999999",
            ErrorMessage = "Giá sản phẩm phải lớn hơn 0.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue,
            ErrorMessage = "Số lượng tồn kho không được âm.")]
        public int StockQuantity { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Vui lòng chọn nhóm hàng hợp lệ.")]
        public int CategoryId { get; set; }
    }
}
