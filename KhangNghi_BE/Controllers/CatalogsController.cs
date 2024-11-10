using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Services;
using Microsoft.AspNetCore.Mvc;

namespace KhangNghi_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogsController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogsController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCatalogs()
        {
            IEnumerable<ProductCatalog> catalogs = await _catalogService.GetCatalogsAsync();
            return Ok(new ApiResponse
            {
                Success = true,
                Data = catalogs
            });
        }

        [HttpGet("product-catalogs/{pId}")]
        public async Task<IActionResult> GetProductCatalogs(string pId)
        {
            IEnumerable<ProductCatalog> catalogs = await _catalogService.GetProductCatalogAsync(pId);
            return Ok(new ApiResponse
            {
                Success = true,
                Data = catalogs
            });
        }
    }
}
