using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FastFood.DAL.Models
{
    public class ProductModel
    {
        [Key]
        public long Id { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tên Sản phẩm")]
        public string Name { get; set; }
        public string? Slug { get; set; }
        [Required, MinLength(4, ErrorMessage = "Yêu cầu nhập mô tả Sản phẩm")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập giá Sản phẩm")]
        [Range(0.0, double.MaxValue)]
        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Yêu cầu chọn danh mục")]

        public int CategoryId { get; set; }
        public CategoryModel? Category { get; set; }
        public string? Image { get; set; }
       

    }
}
