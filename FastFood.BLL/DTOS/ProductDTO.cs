using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.BLL.DTOS
{
    public class ProductDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Slug { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Image { get; set; }   // tên file lưu DB
        public byte[]? ImageBytes { get; set; } // dữ liệu ảnh upload
        public string? ImageFileName { get; set; } // tên file gốc
    }
}
