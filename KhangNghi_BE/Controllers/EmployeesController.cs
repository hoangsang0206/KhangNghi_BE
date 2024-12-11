using KhangNghi_BE.Contants;
using KhangNghi_BE.Data;
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
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly IEmployeePositionService _employeePositionService;
        private readonly IInvoiceService _invoiceService;

        private readonly int _pageSize = 30;

        public EmployeesController(IEmployeeService employeeService, IInvoiceService invoiceService,
            IDepartmentService departmentService, IEmployeePositionService employeePositionService)
        {
            _employeeService = employeeService;
            _invoiceService = invoiceService;
            _departmentService = departmentService;
            _employeePositionService = employeePositionService;
        }

        [HttpGet]
        [AdminAuthorize(Code = Functions.ViewEmployees)]
        public async Task<IActionResult> GetEmployees(string? sortBy, int page = 1)
        {
            var employees = await _employeeService.GetAsync(sortBy, page, _pageSize);

            return Ok(new ApiResponse
            {
                Success = true,
                Data = employees
            });

        }

        [HttpGet("by-id")]
        [AdminAuthorize(Code = Functions.ViewEmployees)]
        public async Task<IActionResult> GetEmployeeById(string id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy nhân viên"
                });
            }
            return Ok(new ApiResponse
            {
                Success = true,
                Data = employee
            });
        }

        [HttpPost("create")]
        [AdminAuthorize(Code = Functions.CreateEmployee)]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeVM employee)
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

            var department = await _departmentService.GetByIdAsync(employee.DepartmentId);
            if (department == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Phòng ban không tồn tại"
                });
            }

            var position = await _employeePositionService.GetByIdAsync(employee.PositionId);
            if (position == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Vị trí công việc không tồn tại"
                });
            }

            string id = DateTime.Now.ToString() + RandomUtils.GenerateRandomString(8).ToUpper();

            var result = await _employeeService.CreateAsync(employee, id);

            if (!result)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Tạo nhân viên thất bại"
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Tạo nhân viên thành công",
                Data = await _employeeService.GetByIdAsync(id)
            });
        }

        [HttpPut("update")]
        [AdminAuthorize(Code = Functions.UpdateEmployee)]
        public async Task<IActionResult> Update([FromQuery] string employeeId, [FromBody] EmployeeVM employeeVM)
        {
            var employee = await _employeeService.GetByIdAsync(employeeId);
            if (employee == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Nhân viên không tồn tại"
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Dữ liệu không hợp lệ",
                    Data = ModelState
                });
            }

            var department = await _departmentService.GetByIdAsync(employeeVM.DepartmentId);
            if (department == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Phòng ban không tồn tại"
                });
            }

            var position = await _employeePositionService.GetByIdAsync(employeeVM.PositionId);
            if (position == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Vị trí công việc không tồn tại"
                });
            }

            bool result = await _employeeService.UpdateAsync(employeeVM, employeeId);

            if(!result)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Cập nhật nhân viên thất bại"
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Cập nhật nhân viên thành công",
                Data = await _employeeService.GetByIdAsync(employeeId)
            });
        }
    }
}
