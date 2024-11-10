using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Services;
using Microsoft.AspNetCore.Mvc;

namespace KhangNghi_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly int _pageSize = 30;

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
    }
}
