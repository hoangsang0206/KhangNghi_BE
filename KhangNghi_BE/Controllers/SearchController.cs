using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KhangNghi_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IServiceService _serviceService;

        private readonly int _pageSize = 20;

        public SearchController(IProductService productService, IServiceService serviceService)
        {
            _productService = productService;
            _serviceService = serviceService;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query, int productsPage, int servicesPage)
        {
            PagedList<Product> products = await _productService.GetByNameAsync(null, query, productsPage,  _pageSize);
            PagedList<Service> services = await _serviceService.SearchByNameAsync(query, servicesPage, _pageSize);

            return Ok(new ApiResponse
            {
                Success = true,
                Data = new
                {
                    Products = products,
                    Services = services
                }
            });
        }
    }
}
