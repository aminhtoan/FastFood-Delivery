using FastFood.BLL.DTOS;
using FastFood.BLL.Product;

using FastFood.UI.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FastFood.UI.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    public class ProductController : Controller
    {
        private readonly ProductBLL _productBLL;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ProductBLL productBLL, IWebHostEnvironment webHostEnvironment)
        {
            _productBLL = productBLL;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _productBLL.GetAllProductsAsync(); // Trả về List<ProductModel> từ DAL

            // Map sang ProductViewModel
            var viewModels = products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryId = p.CategoryId,
                CategoryName = p.CategoryName ?? string.Empty,
                Image = p.Image
            }).ToList();

            return View(viewModels);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _productBLL.GetAllCategoriesAsync(), "Id", "Name");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel viewModel)
        {
            // Load dropdown list
            ViewBag.Categories = new SelectList(await _productBLL.GetAllCategoriesAsync(), "Id", "Name", viewModel.CategoryId);


            if (ModelState.IsValid)
            {
                try
                {
                   // upload ảnh 
                    if (viewModel.ImageUpload != null)
                    {
                        string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                        if (!Directory.Exists(uploadsDir))
                            Directory.CreateDirectory(uploadsDir);

                        string imageName = Guid.NewGuid().ToString() + "_" + viewModel.ImageUpload.FileName;
                        string filePath = Path.Combine(uploadsDir, imageName);

                        using (var fs = new FileStream(filePath, FileMode.Create))
                        {
                            await viewModel.ImageUpload.CopyToAsync(fs);
                        }

                        viewModel.Image = imageName;
                    }

                    // ✅ Map ViewModel → DTO
                    var productDto = new ProductDTO
                    {
                        Name = viewModel.Name,
                        Description = viewModel.Description,
                        Price = viewModel.Price,
                        CategoryId = viewModel.CategoryId,
                       
                        Image = viewModel.Image
                    };

                    var result = await _productBLL.CreateProductAsync(productDto);

                    if (result.IsSuccess)
                    {
                        TempData["success"] = result.Message;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["error"] = result.Message;
                        ModelState.AddModelError("", result.Message);
                    }
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Lỗi hệ thống: " + ex.Message;
                }
            }
            else
            {
                TempData["error"] = "Dữ liệu có vài thứ đang bị lỗi";

                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }

            return View(viewModel);
        }

        // GET: Product/Edit/5

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            // Lấy Product DTO từ Business Logic Layer (BLL)
            var productDto = await _productBLL.GetProductByIdAsync(id);

            if (productDto == null)
            {
                TempData["error"] = "Không tìm thấy sản phẩm.";
                return RedirectToAction("Index");
            }

            // ✅ Ánh xạ DTO → ViewModel để điền vào form
            var viewModel = new ProductViewModel
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                CategoryId = productDto.CategoryId,
                Image = productDto.Image, 
            };

            // Load dropdown list cho Categories
            ViewBag.Categories = new SelectList(await _productBLL.GetAllCategoriesAsync(), "Id", "Name", viewModel.CategoryId);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel viewModel)
        {
            // Load dropdown list (trong trường hợp ModelState không hợp lệ)
            ViewBag.Categories = new SelectList(await _productBLL.GetAllCategoriesAsync(), "Id", "Name", viewModel.CategoryId);

            if (ModelState.IsValid)
            {
                try
                {
                    // 🔹 Lấy sản phẩm hiện có trong DB (DTO)
                    var existingProduct = await _productBLL.GetProductByIdAsync(viewModel.Id);
                    if (existingProduct == null)
                    {
                        TempData["error"] = "Không tìm thấy sản phẩm cần chỉnh sửa.";
                        return RedirectToAction("Index");
                    }

                    string oldImageName = existingProduct.Image;

                    // 🔹 Nếu người dùng upload ảnh mới
                    if (viewModel.ImageUpload != null)
                    {
                        string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                        if (!Directory.Exists(uploadsDir))
                            Directory.CreateDirectory(uploadsDir);

                        string newImageName = Guid.NewGuid().ToString() + "_" + viewModel.ImageUpload.FileName;
                        string newFilePath = Path.Combine(uploadsDir, newImageName);

                        using (var fs = new FileStream(newFilePath, FileMode.Create))
                        {
                            await viewModel.ImageUpload.CopyToAsync(fs);
                        }

                        viewModel.Image = newImageName;

                        // Xóa ảnh cũ nếu có
                        if (!string.IsNullOrEmpty(oldImageName) && oldImageName != "no-image.png")
                        {
                            string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "media/products", oldImageName);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }
                    }
                    else
                    {
                        // Giữ nguyên ảnh cũ nếu không upload mới
                        viewModel.Image = oldImageName;
                    }

                    // 🔹 Map ViewModel → DTO để update
                    var productDto = new ProductDTO
                    {
                        Id = viewModel.Id,
                        Name = viewModel.Name,
                        Description = viewModel.Description,
                        Price = viewModel.Price,
                        CategoryId = viewModel.CategoryId,
                        Image = viewModel.Image
                    };

                    // 🔹 Gọi BLL cập nhật
                    var result = await _productBLL.UpdateProductAsync(productDto);

                    if (result.IsSuccess)
                    {
                        TempData["success"] = result.Message;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["error"] = result.Message;
                        ModelState.AddModelError("", result.Message);
                    }
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Lỗi hệ thống: " + ex.Message;
                }
            }
            else
            {
                TempData["error"] = "Dữ liệu có vài thứ đang bị lỗi.";

                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }

            // Trả về lại view nếu có lỗi
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                // 1️⃣ Lấy sản phẩm để biết tên file ảnh
                var product = await _productBLL.GetProductByIdAsync(id);
                if (product == null)
                {
                    TempData["error"] = "Không tìm thấy sản phẩm.";
                    return RedirectToAction("Index");
                }

                // 2️⃣ Xóa ảnh vật lý (nếu có)
                if (!string.IsNullOrEmpty(product.Image))
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imagePath = Path.Combine(uploadsDir, product.Image);
                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);
                }

                // 3️⃣ Gọi BLL để xóa sản phẩm trong DB
                var result = await _productBLL.DeleteProductAsync(id);

                if (result.IsSuccess)
                    TempData["success"] = result.Message;
                else
                    TempData["error"] = result.Message;
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi hệ thống: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
