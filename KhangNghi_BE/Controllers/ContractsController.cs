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
    public class ContractsController : ControllerBase
    {
        private readonly IContractService _contractService;
        private readonly IInvoiceService _invoiceService;
        private readonly IWarehouseService _warehouseService;
        private readonly IProductService _productService;
        private readonly IServiceService _serviceService;

        private readonly string _rootFolder = "Files";
        private readonly string _docsFolder = "Documents";
        private readonly int _pageSize = 20;

        public ContractsController(IContractService contractService, IInvoiceService invoiceService,
            IWarehouseService warehouseService, IProductService productService, IServiceService serviceService)
        {
            _contractService = contractService;
            _invoiceService = invoiceService;
            _warehouseService = warehouseService;
            _productService = productService;
            _serviceService = serviceService;
        }

        [HttpGet]
        [AdminAuthorize(Code = Functions.ViewContracts)]
        public async Task<IActionResult> GetContracts([FromQuery] string? sortBy, string? filterBy, int page)
        {
            PagedList<Contract> contracts = await _contractService.GetAsync(sortBy, filterBy, page, _pageSize);
            return Ok(new ApiResponse
            {
                Success = true,
                Data = contracts
            });
        }

        [HttpGet("by-customer")]
        [AdminAuthorize(Code = Functions.ViewContracts)]
        public async Task<IActionResult> GetCustomerContracts(string customerId)
        {
            var result = await _contractService.GetByCustomerAsync(customerId);

            return Ok(new ApiResponse 
            {
                Success = true,
                Data= result
            });
        }

        [HttpGet("{id}")]
        [AdminAuthorize(Code = Functions.ViewContracts)]
        public async Task<IActionResult> GetById(string id)
        {
            Contract? contract = await _contractService.GetByIdAsync(id);
            if (contract == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy hợp đồng này"
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Data = contract
            });
        }

        [HttpPost("create")]
        [AdminAuthorize(Code = Functions.CreateContract)]
        public async Task<IActionResult> Create([FromBody] ContractVM contract)
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

            if (contract.CreatedDate.HasValue && contract.CreatedDate.Value > DateTime.Now)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Ngày tạo hợp đồng không được lớn hơn ngày hiện tại"
                });
            }

            if (contract.SignedAt.HasValue && contract.SignedAt.Value > DateTime.Now)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Ngày ký hợp đồng không được lớn hơn ngày hiện tại"
                });
            }

            foreach (var detail in contract.ContractDetails)
            {
                if (detail.ProductId != null)
                {
                    Product? product = await _productService.GetByIdAsync(detail.ProductId);
                    if (product == null)
                    {
                        return BadRequest(new ApiResponse
                        {
                            Success = false,
                            Message = $"Không tìm thấy sản phẩm {detail.ProductId}"
                        });
                    }
                }

                if (detail.ServiceId != null)
                {
                    Service? service = await _serviceService.GetByIdAsync(detail.ServiceId);
                    if (service == null)
                    {
                        return BadRequest(new ApiResponse
                        {
                            Success = false,
                            Message = $"Không tìm thấy dịch vụ {detail.ServiceId}"
                        });
                    }
                }
            }

            bool result = await _contractService.CreateAsync(contract);
            ApiResponse response = new ApiResponse
            {
                Success = result,
                Message = result ? "Tạo hợp đồng thành công" : "Tạo hợp đồng thất bại",
                Data = result ? await _contractService.GetByIdAsync(contract.ContractId) : null
            };

            if (!result)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut("sign")]
        [AdminAuthorize(Code = Functions.SignContract)]
        public async Task<IActionResult> SignContract(string contractId)
        {
            bool result = await _contractService.SignContractAsync(contractId);
            ApiResponse response = new ApiResponse
            {
                Success = result,
                Message = result ? "Ký hợp đồng thành công" : "Ký hợp đồng thất bại"
            };

            if (!result)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut("update")]
        [AdminAuthorize(Code = Functions.UpdateContract)]
        public async Task<IActionResult> Update([FromBody] ContractVM contract)
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

            var existedContract = await _contractService.GetByIdAsync(contract.ContractId);
            if (existedContract == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy hợp đồng này"
                });
            }

            if (existedContract.SignedAt.HasValue)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Hợp đồng đã được ký, không thể chỉnh sửa"
                });
            }

            if (contract.CreatedDate.HasValue && contract.CreatedDate.Value > DateTime.Now)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Ngày tạo hợp đồng không được lớn hơn ngày hiện tại"
                });
            }

            foreach (var detail in contract.ContractDetails)
            {
                if (detail.ProductId != null)
                {
                    Product? product = await _productService.GetByIdAsync(detail.ProductId);
                    if (product == null)
                    {
                        return BadRequest(new ApiResponse
                        {
                            Success = false,
                            Message = $"Không tìm thấy sản phẩm {detail.ProductId}"
                        });
                    }
                }

                if (detail.ServiceId != null)
                {
                    Service? service = await _serviceService.GetByIdAsync(detail.ServiceId);
                    if (service == null)
                    {
                        return BadRequest(new ApiResponse
                        {
                            Success = false,
                            Message = $"Không tìm thấy dịch vụ {detail.ServiceId}"
                        });
                    }
                }
            }

            bool result = await _contractService.UpdateAsync(contract);
            ApiResponse response = new ApiResponse
            {
                Success = result,
                Message = result ? "Cập nhật hợp đồng thành công" : "Cập nhật hợp đồng thất bại",
                Data = result ? await _contractService.GetByIdAsync(contract.ContractId) : null
            };

            return result ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        [AdminAuthorize(Code = Functions.DeleteContract)]
        public async Task<IActionResult> Delete(string id)
        {
            Contract? contract = await _contractService.GetByIdAsync(id);
            if (contract == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy hợp đồng này"
                });
            }

            if (contract.SignedAt.HasValue)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Hợp đồng đã được ký, không thể xóa"
                });
            }

            Invoice? invoice = await _invoiceService.GetByContractId(id);
            if (invoice != null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Hợp đồng này đã được tạo hóa đơn, không thể xóa"
                });
            }

            var stockExits = await _warehouseService.GetByContractAsync(id);
            if (stockExits.Any())
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Hợp đồng này đã được tạo phiếu xuất kho, không thể xóa"
                });
            }

            bool result = await _contractService.DeleteAsync(id);
            ApiResponse response = new ApiResponse
            {
                Success = result,
                Message = result ? "Xóa hợp đồng thành công" : "Xóa hợp đồng thất bại"
            };

            if (!result)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
