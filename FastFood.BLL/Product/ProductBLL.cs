using FastFood.BLL.DTOS;
using FastFood.DAL.Models;
using FastFood.DAL.Repository.Product;


namespace FastFood.BLL.Product
{
    public class ProductBLL
    {
        private readonly ProductDAL _productDAL;
      
        public ProductBLL(ProductDAL productDAL)
        {
            _productDAL = productDAL;
          
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _productDAL.GetAllAsync();
            return products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? string.Empty,
                Image = p.Image
            }).ToList();
        }
        // Lấy sản phẩm theo ID
        public async Task<ProductDTO> GetProductByIdAsync(long id)
        {
            // 1. Lấy Product Entity từ Data Access Layer (DAL)
            var product = await _productDAL.GetProductByIdAsync(id);

            // 2. Kiểm tra nếu không tìm thấy sản phẩm
            if (product == null)
            {
                // Có thể ném ra ngoại lệ (ví dụ: NotFoundException) hoặc trả về null tùy thuộc vào quy ước của bạn.
                return null;
                // throw new NotFoundException($"Product with ID {id} not found.");
            }

            // 3. Ánh xạ (map) từ Product Entity sang ProductDTO
            var productDto = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CategoryId = product.CategoryId,
                // Giả định rằng Entity Product có thuộc tính Category.
                // Dùng toán tử ?? để đảm bảo CategoryName không bị null nếu Category là null.
                CategoryName = product.Category?.Name ?? string.Empty,
                Image = product.Image
            };

            return productDto;
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
        // Thêm sản phẩm
        public async Task<(bool IsSuccess, string Message)> CreateProductAsync(ProductDTO productDto)
        {
            try
            {
                // Tạo slug từ tên
                productDto.Slug = productDto.Name.Replace(" ", "-");

                // Kiểm tra trùng slug
                var existingProduct = await _productDAL.GetProductBySlugAsync(productDto.Slug);
                if (existingProduct != null)
                {
                    return (false, "Sản phẩm đã có trong database");
                }
                //Map từ DTO sang Model 
                var productModel = new ProductModel
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    CategoryId = productDto.CategoryId,
                   
                    Slug = productDto.Slug,
                    Image = productDto.Image
                };
                // Gọi DAL để lưu
                await _productDAL.AddProductAsync(productModel);

                return (true, "Thêm sản phẩm thành công");
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi thêm sản phẩm: " + ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string Message)> UpdateProductAsync(ProductDTO dto)
        {
            try
            {
                var existing = await _productDAL.GetProductByIdAsync(dto.Id);
                if (existing == null)
                    return (false, "Không tìm thấy sản phẩm để cập nhật");

                existing.Name = dto.Name;
                existing.Description = dto.Description;
                existing.Price = dto.Price;
                existing.CategoryId = dto.CategoryId;
                existing.Image = dto.Image;
                existing.Slug = dto.Name.Replace(" ", "-");

                await _productDAL.UpdateProductAsync(existing);
                return (true, "Cập nhật sản phẩm thành công");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi khi cập nhật sản phẩm: {ex.Message}");
            }
        }

        // Xóa sản phẩm
        public async Task<(bool IsSuccess, string Message)> DeleteProductAsync(long id)
        {
            try
            {
                var product = await _productDAL.GetProductByIdAsync(id);
                if (product == null)
                {
                    return (false, "Không tìm thấy sản phẩm");
                }

              

                // Xóa sản phẩm
                await _productDAL.DeleteProductAsync(id);

                return (true, "Xóa sản phẩm thành công!");
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi xóa sản phẩm: " + ex.Message);
            }
        }
       
       
    }
}
