using KhangNghi_BE.Contants;
using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Filters;
using KhangNghi_BE.Services;
using KhangNghi_BE.Utils;
using Microsoft.AspNetCore.Mvc;

namespace KhangNghi_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly int _pageSize = 30;
        private readonly string[] _allowedExtension = { ".jpg", ".jpeg", ".png", ".webp" };
        private readonly string _rootFolder = "Files";
        private readonly string _imageFolder = "Images";

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetProducts(string? sortBy)
        {
            IEnumerable<Product> products = await _productService.GetAsync(sortBy);
            return Ok(new ApiResponse
            {
                Success = true,
                Data = products
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(string? sortBy, int page = 1)
        {
            PagedList<Product> products = await _productService.GetAsync(sortBy, page, _pageSize);

            return Ok(new ApiResponse
            {
                Success = true,
                Data = products
            });
        }

        [HttpGet("catalog/{catalogId}")]
        public async Task<IActionResult> GetProductsByCatalog(string? sortBy, string catalogId, int page = 1)
        {
            PagedList<Product> products = await _productService.GetByCatalogAsync(sortBy, catalogId, page, _pageSize);

            return Ok(new ApiResponse
            {
                Success = true,
                Data = products
            });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts(string? sortBy, string query, int page = 1)
        {
            PagedList<Product> products = await _productService.GetByNameAsync(sortBy, query, page, _pageSize);

            return Ok(new ApiResponse
            {
                Success = true,
                Data = products
            });
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            Product? product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy sản phẩm"
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Data = product
            });
        }

        #region Authorized

        [HttpPost("create")]
        [AdminAuthorize(Code = Functions.CreateProduct)]
        public async Task<IActionResult> CreateProduct([FromForm] ProductVM product, List<IFormFile>? images)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Dữ liệu không hợp lệ",
                    Data = ModelState
                });
            }

            Product? existedProduct = await _productService.GetByIdAsync(product.ProductId);
            if(existedProduct != null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Sản phẩm đã tồn tại"
                });
            }

            if (images != null)
            {
                string uploadPath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), _rootFolder, _imageFolder));
                foreach (IFormFile image in images)
                {
                    string fileName = product.ProductId + "-" 
                        + Guid.NewGuid().ToString() 
                        + Path.GetExtension(image.FileName);

                    bool fileResult = await FileUtils.UploadFileAsync(image, uploadPath, fileName);
                    if(fileResult)
                    {
                        product.Images.Add(new ProductVM.ProductImage
                        {
                            ImageUrl = $"/Files/Images/{fileName}"
                        });
                    }
                }
            }

            bool result = await _productService.CreateAsync(product);

            ApiResponse response = new ApiResponse
            {
                Success = result,
                Message = result ? "Tạo sản phẩm thành công" : "Tạo sản phẩm thất bại",
                Data = result ? await _productService.GetByIdAsync(product.ProductId) : null
            };

            if(result)
            {
                return Ok(response);
            }

            foreach(var image in product.Images)
            {
                if(image.ImageUrl != null)
                {
                    string fileName = image.ImageUrl.Replace("/Files/Images/", string.Empty);
                    string path = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), _rootFolder, _imageFolder, fileName));
                    FileUtils.DeleteFile(path);
                }
            }
            return BadRequest(response);
        }

        [HttpPut("update")]
        [AdminAuthorize(Code = Functions.UpdateProduct)]
        public async Task<IActionResult> UpdateProduct(ProductVM product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Dữ liệu không hợp lệ",
                    Data = ModelState
                });
            }


            return Ok();
        }

        [HttpDelete("delete/{productId}")]
        [AdminAuthorize(Code = Functions.DeleteProduct)]
        public async Task<IActionResult> DeleteProduct(string productId)
        {
            Product? product = await _productService.GetByIdAsync(productId);
            if(product == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Sản phẩm không tồn tại"
                });
            }

            bool result = await _productService.DeleteAsync(productId);

            ApiResponse response = new ApiResponse
            {
                Success = result,
                Message = result ? "Xóa sản phẩm thành công" : "Xóa sản phẩm thất bại"
            };

            if(result)
            {
                foreach (var image in product.ProductImages)
                {
                    if(image.ImageUrl != null)
                    {
                        string fileName = image.ImageUrl.Replace("/Files/Images/", string.Empty);
                        string path = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), _rootFolder, _imageFolder, fileName));
                        FileUtils.DeleteFile(path);
                    }
                }
                return Ok(response);
            }

            return BadRequest(response);
        }

        #endregion
    }
}
