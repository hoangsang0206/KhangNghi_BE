using KhangNghi_BE.Contants;
using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Filters;
using KhangNghi_BE.Services;
using KhangNghi_BE.Utils;
using Microsoft.AspNetCore.Mvc;

namespace KhangNghi_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractsController : ControllerBase
    {
        private readonly IContractService _contractService;
        private readonly string _rootFolder = "Files";
        private readonly string _docsFolder = "Documents";
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
        public async Task<IActionResult> Create([FromForm] ContractVM contract, IFormFile file)
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

            string fileName = contract.ContractId
                        + Path.GetExtension(file.FileName);
            string uploadPath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), _rootFolder, _docsFolder));

            bool uploadResult = await FileUtils.UploadFileAsync(file, uploadPath, fileName);

            bool result = await _contractService.CreateAsync(contract, $"/{_rootFolder}/{_docsFolder}/{fileName}");
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

        [HttpDelete]
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
