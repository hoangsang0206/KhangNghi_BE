using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Services;
using KhangNghi_BE.Filters;
using Microsoft.AspNetCore.Mvc;
using KhangNghi_BE.Contants;

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

        [HttpGet("product-catalogs")]
        public async Task<IActionResult> GetProductCatalogs(string pId)
        {
            IEnumerable<ProductCatalog> catalogs = await _catalogService.GetProductCatalogAsync(pId);
            return Ok(new ApiResponse
            {
                Success = true,
                Data = catalogs
            });
        }



        #region Authorized

        [HttpPost("create")]
        [AdminAuthorize(Code = Functions.CreateCatalog)]
        public async Task<IActionResult> CreateCatalog([FromBody] CatalogVM catalog)
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

            ProductCatalog? oldCatalog = await _catalogService.GetByIdAsync(catalog.CatalogId);
            if (oldCatalog != null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Danh mục này đã tồn tại"
                });
            }

            bool result = await _catalogService.CreateAsync(catalog);
            ApiResponse response = new ApiResponse
            {
                Success = result,
                Message = result ? "Tạo mới thành công" : "Tạo mới thất bại",
                Data = result ? await _catalogService.GetByIdAsync(catalog.CatalogId) : null
            };

            if(result)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPut("update")]
        [AdminAuthorize(Code = Functions.UpdateCatalog)]
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

            bool result = await _catalogService.UpdateAsync(catalog);

            ApiResponse response = new ApiResponse
            {
                Success = result,
                Message = result ? "Cập nhật thành công" : "Cập nhật thất bại",
                Data = result ? await _catalogService.GetByIdAsync(catalog.CatalogId) : null
            };

            if (result)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpDelete("delete")]
        [AdminAuthorize(Code = Functions.DeleteCatalog)]
        public async Task<IActionResult> DeleteCatalog(string catalogId)
        {
            ProductCatalog? catalog = await _catalogService.GetByIdAsync(catalogId);
            if (catalog == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Danh mục không tồn tại"
                });
            }

            bool result = await _catalogService.DeleteAsync(catalogId);

            return Ok(new ApiResponse
            {
                Success = result,
                Message = result ? "Xóa thành công" : "Xóa thất bại"
            });
        }

        #endregion
    }
}
