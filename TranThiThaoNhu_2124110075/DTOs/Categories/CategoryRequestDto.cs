
using System.ComponentModel.DataAnnotations;

namespace TranThiThaoNhu_2124110075.API.DTOs.Categories
{
    public class CategoryRequestDto
    {
        [Required(ErrorMessage = "Tên danh mục không được để trống!")]
        [StringLength(100, ErrorMessage = "Tên danh mục không vượt quá 100 ký tự!")]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Mô tả không vượt quá 255 ký tự!")]
        public string? Description { get; set; }
    }
}
