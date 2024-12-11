using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KhangNghi_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly IWarehouseService _warehouseService;

        public SuppliersController(ISupplierService supplierService, IWarehouseService warehouseService)
        {
            _supplierService = supplierService;
            _warehouseService = warehouseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSuppliers()
        {
            var suppliers = await _supplierService.GetAllAsync();
            return Ok(new ApiResponse 
            {
                Success = true,
                Data = suppliers
            });
        }

        [HttpGet("by-id")]
        public async Task<IActionResult> GetSupplierById(string id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);

            if(supplier == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy nhà cung cấp"
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Data = supplier
            });
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSupplier([FromBody] SupplierVM supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Dữ liệu không hợp lệ"
                });
            }

            var existedSupplier = await _supplierService.GetByIdAsync(supplier.SupplierId);
            if (existedSupplier != null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Nhà cung cấp đã tồn tại"
                });
            }

            var result = await _supplierService.CreateAsync(supplier);
            if (!result)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Tạo nhà cung cấp thất bại"
                });
            }
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Tạo nhà cung cấp thành công",
                Data = await _supplierService.GetByIdAsync(supplier.SupplierId)
            });
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateSupplier([FromBody] SupplierVM supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Dữ liệu không hợp lệ"
                });
            }

            var existedSupplier = await _supplierService.GetByIdAsync(supplier.SupplierId);
            if (existedSupplier == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Nhà cung cấp không tồn tại"
                });
            }

            var result = await _supplierService.UpdateAsync(supplier);
            if (!result)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Cập nhật nhà cung cấp thất bại"
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Cập nhật nhà cung cấp thành công",
                Data = await _supplierService.GetByIdAsync(supplier.SupplierId)
            });
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteSupplier(string id)
        {
            var existedSupplier = await _supplierService.GetByIdAsync(id);
            if (existedSupplier == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Nhà cung cấp không tồn tại"
                });
            }

            var stockEntries = await _warehouseService.GetStockEntriesBySupplierIdAsync(id);
            if (stockEntries.Any())
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Nhà cung cấp này đã cung cấp sản phẩm cho kho, không thể xóa"
                });
            }

            var result = await _supplierService.DeleteAsync(id);
            if (!result)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Xóa nhà cung cấp thất bại"
                });
            }
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Xóa nhà cung cấp thành công"
            });
        }
    }
}
