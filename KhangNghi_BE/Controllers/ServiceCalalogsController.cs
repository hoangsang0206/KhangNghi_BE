using KhangNghi_BE.Contants;
using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Filters;
using KhangNghi_BE.Services;
using Microsoft.AspNetCore.Mvc;

namespace KhangNghi_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceCalalogsController : ControllerBase
    {
        private readonly IServiceCatalogService _serviceCatalogService;

        public ServiceCalalogsController(IServiceCatalogService serviceCatalogService)
        {
            _serviceCatalogService = serviceCatalogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCatalogs()
        {
            IEnumerable<ServiceCatalog> catalogs = await _serviceCatalogService.GetAllAsync();
            return Ok(new ApiResponse
            {
                Success = true,
                Data = catalogs
            });
        }

        #region Authorized

        [HttpPost("create")]
        [AdminAuthorize(Code = Functions.CreateServiceCatalog)]
        public async Task<IActionResult> CreateCatalog(CatalogVM catalog)
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

            ServiceCatalog? sCatalog = await _serviceCatalogService.GetByIdAsync(catalog.CatalogId);
            if (sCatalog != null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Danh mục đã tồn tại"
                });
            }

            bool result = await _serviceCatalogService.CreateAsync(catalog);
            ApiResponse response = new ApiResponse
            {
                Success = result,
                Data = result ? await _serviceCatalogService.GetByIdAsync(catalog.CatalogId) : null
            };

            return result ? Ok(response) : BadRequest(response);
        }

        [HttpPut("update")]
        [AdminAuthorize(Code = Functions.UpdateServiceCatalog)]
        public async Task<IActionResult> UpdateCatalog(CatalogVM catalog)
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

            bool result = await _serviceCatalogService.UpdateAsync(catalog);

            ApiResponse response = new ApiResponse
            {
                Success = result,
                Data = result ? await _serviceCatalogService.GetByIdAsync(catalog.CatalogId) : null
            };

            return result ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("delete/{catalogId}")]
        [AdminAuthorize(Code = Functions.DeleteServiceCatalog)]
        public async Task<IActionResult> DeleteCatalog(string catalogId)
        {
            ServiceCatalog? sCatalog = await _serviceCatalogService.GetByIdAsync(catalogId);
            if (sCatalog == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Danh mục không tồn tại"
                });
            }

            bool result = await _serviceCatalogService.DeleteAsync(catalogId);

            ApiResponse response = new ApiResponse
            {
                Success = result,
                Message = result ? "Xóa danh mục thành công" : "Xóa danh mục thất bại"
            };

            return result ? Ok(response) : BadRequest(response);
        }

        #endregion
    }
}
