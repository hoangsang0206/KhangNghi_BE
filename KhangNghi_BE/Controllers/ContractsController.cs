using KhangNghi_BE.Contants;
using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Filters;
using KhangNghi_BE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KhangNghi_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractsController : ControllerBase
    {
        private readonly IContractService _contractService;
        private readonly int _pageSize = 20;

        public ContractsController(IContractService contractService)
        {
            _contractService = contractService;
        }

        [HttpGet]
        [AdminAuthorize(Code = Functions.ViewContracts)]
        public async Task<IActionResult> GetContracts([FromQuery] string? sortBy, string filterBy, int page)
        {
            PagedList<Contract> contracts = await _contractService.GetAsync(sortBy, filterBy, page, _pageSize);
            return Ok(new ApiResponse
            {
                Success = true,
                Data = contracts
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

        [HttpDelete("{id}")]
        [AdminAuthorize(Code = Functions.DeleteContract)]
        public async Task<IActionResult> Delete(string id)
        {
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
