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

        public SearchController(IProductService productService, IServiceService serviceService)
        {
            _productService = productService;
            _serviceService = serviceService;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            return Ok();
        }
    }
}
