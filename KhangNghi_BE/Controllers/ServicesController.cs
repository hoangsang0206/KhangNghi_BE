using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Services;
using Microsoft.AspNetCore.Mvc;

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
    }
}
