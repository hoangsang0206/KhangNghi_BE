using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Filters;
using KhangNghi_BE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KhangNghi_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly IEmployeePositionService _employeePositionService;

        public PositionsController(IEmployeePositionService employeePositionService)
        {
            _employeePositionService = employeePositionService;
        }

        [HttpGet]
        [AdminAuthorize]
        public async Task <IActionResult> Get()
        {
            var result = await _employeePositionService.GetAsync();
            return Ok(new ApiResponse
            {
                Success = true,
                Data = result
            });

        }
    }
}
