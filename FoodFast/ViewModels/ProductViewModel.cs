
using FastFood.DAL.Repository.Validation;
using System.ComponentModel.DataAnnotations;

namespace FastFood.UI.ViewModels
{
    public class ProductViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập tên Sản phẩm")]
        public string Name { get; set; }

        public string? Slug { get; set; }

        [Required, MinLength(4, ErrorMessage = "Yêu cầu nhập mô tả Sản phẩm")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập giá Sản phẩm")]
        [Range(0.0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Yêu cầu chọn danh mục")]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Image { get; set; }

      
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }
    }
}
