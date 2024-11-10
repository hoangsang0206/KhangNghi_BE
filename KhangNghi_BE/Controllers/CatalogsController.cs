using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Services;
using KhangNghi_BE.Filters;
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



        #region Authorized

        [HttpPost("create")]
        [AdminAuthorize(Code = "create-catalog")]
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

            ProductCatalog newCatalog = await _catalogService.CreateAsync(catalog);
            return Ok(new ApiResponse
            {
                Success = true,
                Data = newCatalog
            });
        }

        [HttpPut("update")]
        [AdminAuthorize(Code = "update-catalog")]
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

            ProductCatalog? updatedCatalog = await _catalogService.UpdateAsync(catalog);
            if(updatedCatalog == null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Danh mục không tồn tại"
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Cập nhật thành công",
                Data = updatedCatalog
            });
        }

        [HttpDelete("delete/{catalogId}")]
        [AdminAuthorize(Code = "delete-catalog")]
        public async Task<IActionResult> DeleteCatalog(string catalogId)
        {
            ProductCatalog? catalog = await _catalogService.GetByIdAsync(catalogId);
            if (catalog == null)
            {
                return BadRequest(new ApiResponse
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
