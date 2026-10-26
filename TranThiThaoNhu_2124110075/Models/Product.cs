//họ tên: Trần Thị Thảo Như
//Mssv: 2124110075
//Lớp: CCQ2411C
//Môn: lập trình ứng dụng

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranThiThaoNhu_2124110075.API.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Mã vạch sản phẩm không được để trống!")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Mã vạch phải gồm đúng 13 chữ số.")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "Mã vạch EAN-13 chỉ được chứa 13 chữ số.")]
        public string Barcode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên sản phẩm không được để trống!")]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [Range(typeof(decimal), "0.01", "9999999999999999", ErrorMessage = "Giá sản phẩm phải lớn hơn 0.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không được âm.")]
        public int StockQuantity { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn danh mục hợp lệ.")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
    }
}