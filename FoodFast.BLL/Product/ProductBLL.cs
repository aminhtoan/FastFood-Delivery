using FoodFast.DAL.Models;
using FoodFast.DAL.Repository.Product;

namespace FoodFast.BLL.Product
{
    public class ProductBLL
    {
        private readonly ProductDAL _productDAL;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductBLL(ProductDAL productDAL, IWebHostEnvironment webHostEnvironment)
        {
            _productDAL = productDAL;
            _webHostEnvironment = webHostEnvironment;
        }

        public IEnumerable<ProductModel> GetAllProducts()
        {
            // Có thể xử lý thêm nghiệp vụ ở đây (lọc, sắp xếp,…)
            return _productDAL.GetAllProducts();
        }
        // Lấy tất cả danh mục
        public async Task<List<CategoryModel>> GetAllCategoriesAsync()
        {
            try
            {
                return await _productDAL.GetAllCategoriesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách danh mục: " + ex.Message);
            }
        }
        public async Task<(bool IsSuccess, string Message)> CreateProductAsync(ProductModel product)
        {
            try
            {
                // Tạo slug từ tên sản phẩm
                product.Slug = GenerateSlug(product.Name);

                // Kiểm tra slug đã tồn tại chưa
                var existingProduct = await _productDAL.GetProductBySlugAsync(product.Slug);
                if (existingProduct != null)
                {
                    return (false, "Sản phẩm đã tồn tại trong hệ thống");
                }

                // Xử lý upload ảnh
                if (product.ImageUpload != null)
                {
                    var imageResult = await SaveImageAsync(product.ImageUpload);
                    if (!imageResult.IsSuccess)
                    {
                        return imageResult;
                    }
                    product.Image = imageResult.Message; // Tên file ảnh
                }

                // Thêm sản phẩm vào database
                await _productDAL.AddProductAsync(product);

                return (true, "Thêm sản phẩm thành công!");
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi thêm sản phẩm: " + ex.Message);
            }
        }
        private async Task<(bool IsSuccess, string Message)> SaveImageAsync(IFormFile imageFile)
        {
            try
            {
                // Kiểm tra file
                if (imageFile == null || imageFile.Length == 0)
                {
                    return (false, "File ảnh không hợp lệ");
                }

                // Kiểm tra định dạng
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extension = Path.GetExtension(imageFile.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    return (false, "Chỉ chấp nhận file ảnh: jpg, jpeg, png, gif, webp");
                }

                // Kiểm tra kích thước (max 5MB)
                if (imageFile.Length > 5 * 1024 * 1024)
                {
                    return (false, "Kích thước file không được vượt quá 5MB");
                }

                // Tạo thư mục nếu chưa có
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }

                // Tạo tên file unique
                string imageName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadsDir, imageName);

                // Lưu file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                return (true, imageName);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi lưu ảnh: " + ex.Message);
            }
        }
        private string GenerateSlug(string name)
        {
            return name.ToLower()
                       .Replace(" ", "-")
                       .Replace("đ", "d")
                       .Replace("á", "a").Replace("à", "a").Replace("ả", "a").Replace("ã", "a").Replace("ạ", "a")
                       .Replace("ă", "a").Replace("ắ", "a").Replace("ằ", "a").Replace("ẳ", "a").Replace("ẵ", "a").Replace("ặ", "a")
                       .Replace("â", "a").Replace("ấ", "a").Replace("ầ", "a").Replace("ẩ", "a").Replace("ẫ", "a").Replace("ậ", "a")
                       .Replace("é", "e").Replace("è", "e").Replace("ẻ", "e").Replace("ẽ", "e").Replace("ẹ", "e")
                       .Replace("ê", "e").Replace("ế", "e").Replace("ề", "e").Replace("ể", "e").Replace("ễ", "e").Replace("ệ", "e")
                       .Replace("í", "i").Replace("ì", "i").Replace("ỉ", "i").Replace("ĩ", "i").Replace("ị", "i")
                       .Replace("ó", "o").Replace("ò", "o").Replace("ỏ", "o").Replace("õ", "o").Replace("ọ", "o")
                       .Replace("ô", "o").Replace("ố", "o").Replace("ồ", "o").Replace("ổ", "o").Replace("ỗ", "o").Replace("ộ", "o")
                       .Replace("ơ", "o").Replace("ớ", "o").Replace("ờ", "o").Replace("ở", "o").Replace("ỡ", "o").Replace("ợ", "o")
                       .Replace("ú", "u").Replace("ù", "u").Replace("ủ", "u").Replace("ũ", "u").Replace("ụ", "u")
                       .Replace("ư", "u").Replace("ứ", "u").Replace("ừ", "u").Replace("ử", "u").Replace("ữ", "u").Replace("ự", "u")
                       .Replace("ý", "y").Replace("ỳ", "y").Replace("ỷ", "y").Replace("ỹ", "y").Replace("ỵ", "y");
        }
    }
}
