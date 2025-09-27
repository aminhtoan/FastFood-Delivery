using FoodFast.DAL.Models;
using FoodFast.DAL.Repository.Product;

namespace FoodFast.BLL.Product
{
    public class ProductBLL
    {
        private readonly ProductDAL _productDAL;

        public ProductBLL(ProductDAL productDAL)
        {
            _productDAL = productDAL;
        }

        public IEnumerable<ProductModel> GetAllProducts()
        {
            // Có thể xử lý thêm nghiệp vụ ở đây (lọc, sắp xếp,…)
            return _productDAL.GetAllProducts();
        }
    }
}
