using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Filters;
using KhangNghi_BE.Services;
using Microsoft.AspNetCore.Mvc;

namespace KhangNghi_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        [AdminAuthorize]
        public async Task<IActionResult> Get()
        {
            var departments = await _departmentService.GetAsync();
            return Ok(new ApiResponse
            {
                Success = true,
                Data = departments
            });
        }
    }
}
