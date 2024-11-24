using KhangNghi_BE.Contants;
using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Filters;
using KhangNghi_BE.Services;
using KhangNghi_BE.Utils;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace KhangNghi_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceService _serviceService;
        private readonly int _pageSize = 30;

        public ServicesController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetServices(string? sortBy)
        {
            IEnumerable<Service> services = await _serviceService.GetAsync(sortBy);
            return Ok(new ApiResponse
            {
                Success = true,
                Data = services
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetServices(string? sortBy, int page = 1)
        {
            PagedList<Service> services = await _serviceService.GetAsync(sortBy, page, _pageSize);
            return Ok(new ApiResponse
            {
                Success = true,
                Data = services
            });
        }

        [HttpGet("detail/{sId}")]
        public async Task<IActionResult> GetServiceById(string sId)
        {
            Service? service = await _serviceService.GetByIdAsync(sId);

            if (service == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy dịch vụ"
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Data = service
            });
        }

        #region Authorized

        [HttpPost("create")]
        [AdminAuthorize(Code = Functions.CreateService)]
        public async Task<IActionResult> CreateService([FromForm] ServiceVM service, IFormFile mainImage)
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

            Service? existedService = await _serviceService.GetByIdAsync(service.ServiceId);
            if (existedService != null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Dịch vụ đã tồn tại"
                });
            }

            bool result = await _serviceService.CreateAsync(service);

            ApiResponse response = new ApiResponse
            {
                Success = result,
                Data = result ? await _serviceService.GetByIdAsync(service.ServiceId) : null
            };

            return result ? Ok(response) : BadRequest(response);
        }
        
        [HttpPut("update")]
        [AdminAuthorize(Code = Functions.UpdateService)]
        public async Task<IActionResult> UpdateService(ServiceVM service)
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

            bool result = await _serviceService.UpdateAsync(service);

            ApiResponse response = new ApiResponse
            {
                Success = result,
                Data = result ? await _serviceService.GetByIdAsync(service.ServiceId) : null
            };

            return result ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("delete/{sId}")]
        [AdminAuthorize(Code = Functions.DeleteService)]
        public async Task<IActionResult> DeleteService(string sId)
        {
            Service? service = await _serviceService.GetByIdAsync(sId);
            if(service == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy dịch vụ"
                });
            }

            bool result = await _serviceService.DeleteAsync(sId);
            ApiResponse response = new ApiResponse
            {
                Success = result,
                Message = result ? "Xóa dịch vụ thành công" : "Xóa dịch vụ thất bại"
            };

            return result ? Ok(response) : BadRequest(response);
        }

        #endregion
    }
}
