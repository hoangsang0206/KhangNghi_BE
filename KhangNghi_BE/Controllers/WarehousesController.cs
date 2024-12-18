﻿using KhangNghi_BE.Contants;
using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Filters;
using KhangNghi_BE.Services;
using KhangNghi_BE.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KhangNghi_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;
        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly IContractService _contractService;

        private readonly int _pageSize = 30;

        public WarehousesController(IWarehouseService warehouseService,
            IProductService productService, ISupplierService supplierService, IContractService contractService)
        {
            _warehouseService = warehouseService;
            _productService = productService;
            _supplierService = supplierService;
            _contractService = contractService;
        }

        #region CRUD Warehouse

        [HttpGet("all")]
        [AdminAuthorize(Code = Functions.ViewWarehouses)]
        public async Task<IActionResult> GetAsync(string? sortBy)
        {
            IEnumerable<Warehouse> warehouses = await _warehouseService.GetAsync(sortBy);
            return Ok(new ApiResponse
            {
                Success = true,
                Data = warehouses
            });
        }

        [HttpGet("1/{id}")]
        [AdminAuthorize(Code = Functions.ViewWarehouses)]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            Warehouse? wh = await _warehouseService.GetByIdAsync(id);
            if (wh == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy kho hàng này"
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Data = wh
            });
        }

        [HttpPost("create")]
        [AdminAuthorize(Code = Functions.CreateWarehouse)]
        public async Task<IActionResult> CreateAsync([FromBody] WarehouseVM warehouse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Dữ liệu không hợp lệ"
                });
            }

            Warehouse? wh = await _warehouseService.GetByIdAsync(warehouse.WarehouseId);
            if (wh != null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Kho hàng đã tồn tại"
                });
            }

            bool result = await _warehouseService.CreateAsync(warehouse);
            if (!result)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Tạo kho hàng không thành công"
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Tạo kho hàng thành công",
                Data = await _warehouseService.GetByIdAsync(warehouse.WarehouseId)
            });
        }

        [HttpPut("update")]
        [AdminAuthorize(Code = Functions.UpdateWarehouse)]
        public async Task<IActionResult> UpdateAsync([FromBody] WarehouseVM warehouse)
        {
            bool result = await _warehouseService.UpdateAsync(warehouse);
            if (!result)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Cập nhật kho hàng không thành công"
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Cập nhật kho hàng thành công",
                Data = await _warehouseService.GetByIdAsync(warehouse.WarehouseId)
            });
        }

        [HttpDelete("{id}")]
        [AdminAuthorize(Code = Functions.DeleteWarehouse)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            Warehouse? wh = await _warehouseService.GetByIdAsync(id);
            if (wh == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy kho hàng này"
                });
            }

            bool result = await _warehouseService.DeleteAsync(id);
            if (!result)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Xóa kho hàng không thành công"
                });
            }

            return Ok(new ApiResponse 
            { 
                Success = true,
                Message = "Xóa kho hàng thành công"
            });
        }

        #endregion

        #region Warehouse Stock

        [HttpGet("instock-qty")]
        public async Task<IActionResult> GetInStockQuantity(string productId, string? warehouseId)
        {
            int quantity = string.IsNullOrEmpty(warehouseId) 
                ? await _warehouseService.GetInStockQuantity(productId)
                : await _warehouseService.GetInStockQuantity(productId, warehouseId);

            return Ok(new ApiResponse
            {
                Success = true,
                Message = quantity <= 0 ? "Sản phẩm đã hết hàng" : null,
                Data = quantity
            });
        }

        #endregion

        #region Warehouse Import

        [HttpGet("import/all")]
        [AdminAuthorize(Code = Functions.WarehouseImport)]
        public async Task<IActionResult> GetImportSlips(int page = 1)
        {
            PagedList<StockEntry> slips = await _warehouseService.GetImportSlipsAsync(null, null, page, _pageSize);
            return Ok(new ApiResponse
            {
                Success = true,
                Data = slips
            });
        }

        [HttpPost("import/create")]
        [AdminAuthorize(Code = Functions.WarehouseImport)]
        public async Task<IActionResult> CreateImportSlip([FromBody] StockEntryVM entry)
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

            Warehouse? warehouse = await _warehouseService.GetByIdAsync(entry.WarehouseId);
            if (warehouse == null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Kho hàng không tồn tại"
                });
            }

            Supplier? supplier = await _supplierService.GetByIdAsync(entry.SupplierId);
            if (supplier == null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Nhà cung cấp không tồn tại"
                });
            }

            foreach(var product in entry.StockEntryDetails)
            {
                Product? p = await _productService.GetByIdAsync(product.ProductId);
                if (p == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = $"Sản phẩm với mã {product.ProductId} không tồn tại"
                    });
                }
            }

            StockEntry slip = new StockEntry
            {
                EntryId = DateTime.Now.ToString("yyyyMMdd") + RandomUtils.GenerateRandomString(8).ToUpper(),
                EntryDate = DateTime.Now,
                WarehouseId = entry.WarehouseId,
                SupplierId = entry.SupplierId,
                Note = entry.Note,
                TotalAmout = entry.StockEntryDetails.Sum(d => d.Quantity * d.UnitPrice),
                StockEntryDetails = entry.StockEntryDetails.Select(d => new StockEntryDetail
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                }).ToList()
            };

            bool result = await _warehouseService.CreateImportSlipAsync(slip);

            return Ok(new ApiResponse
            {
                Success = result,
                Message = result ? "Nhập kho hàng thành công" : "Nhập kho hàng không thành công",
            });
        }

        #endregion

        #region Warehouse Export

        [HttpGet("export/all")]
        [AdminAuthorize(Code = Functions.WarehouseExport)]
        public async Task<IActionResult> GetExportSlips(int page = 1)
        {
            PagedList<StockExit> slips = await _warehouseService.GetExportSlipsAsync(null, null, page, _pageSize);
            return Ok(new ApiResponse
            {
                Success = true,
                Data = slips
            });
        }

        [HttpPost("export/create")]
        [AdminAuthorize(Code = Functions.WarehouseExport)]
        public async Task<IActionResult> CreateExportSlip([FromBody] StockExitVM exit)
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

            Warehouse? warehouse = await _warehouseService.GetByIdAsync(exit.WarehouseId);
            if (warehouse == null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Kho hàng không tồn tại"
                });
            }

            if(exit.ContractId != null)
            {
                Contract? contract = await _contractService.GetByIdAsync(exit.ContractId);
                if (contract == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Hợp đồng không tồn tại"
                    });
                }
            }

            foreach (var product in exit.StockExitDetails)
            {
                Product? p = await _productService.GetByIdAsync(product.ProductId);
                if (p == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = $"Sản phẩm với mã {product.ProductId} không tồn tại"
                    });
                }
                else
                {
                    int inStock = p.ProductsInWarehouses
                        .FirstOrDefault(w => w.WarehouseId == exit.WarehouseId)?.Quantity ?? 0;

                    if (inStock < product.Quantity)
                    {
                        return BadRequest(new ApiResponse
                        {
                            Success = false,
                            Message = $"Sản phẩm {p.ProductName} không đủ hàng trong kho"
                        });
                    }
                }
            }

            StockExit slip = new StockExit
            {
                ExitId = DateTime.Now.ToString("yyyyMMdd") + RandomUtils.GenerateRandomString(8).ToUpper(),
                ExitDate = DateTime.Now,
                WarehouseId = exit.WarehouseId,
                ContractId = exit.ContractId,
                Note = exit.Note,
                TotalAmout = exit.StockExitDetails.Sum(d => d.Quantity * d.UnitPrice),
                StockExitDetails = exit.StockExitDetails.Select(d => new StockExitDetail
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                }).ToList()
            };

            bool result = await _warehouseService.CreateExportSlipAsync(slip);

            return Ok(new ApiResponse
            {
                Success = result,
                Message = result ? "Xuất kho hàng thành công" : "Xuất kho hàng không thành công",
            });
        }

        #endregion
    }
}
