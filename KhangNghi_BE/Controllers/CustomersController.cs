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
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly int _pageSize = 30;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("types")]
        [AdminAuthorize(Code = Functions.ViewClients)]
        public async Task<IActionResult> GetCustomerTypes()
        {
            return Ok(new ApiResponse 
            { 
                Success = true,
                Data = await _customerService.GetCustomerTypesAsync()
            });
        }

        [HttpGet]
       // [AdminAuthorize(Code = Functions.ViewClients)]
        public async Task<IActionResult> GetCustomers(string? sortBy, string? filterBy, int page = 1)
        {
            PagedList<Customer> customers = await _customerService.GetAsync(sortBy, filterBy, page, _pageSize);
            return Ok(new ApiResponse 
            { 
                Success = true,
                Data = customers
            });
        }

        [HttpGet("by-id")]
        [AdminAuthorize(Code = Functions.ViewClients)]
        public async Task<IActionResult> GetCustomerById(string id)
        {
            Customer? customer = await _customerService.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound(new ApiResponse 
                { 
                    Success = false,
                    Message = "Không tìm thấy khách hàng"
                });
            }

            return Ok(new ApiResponse 
            { 
                Success = true,
                Data = customer
            });
        }

        [HttpPost]
        [AdminAuthorize(Code = Functions.CreateClient)]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerVM customer)
        {
            string customerId = "KH" + DateTime.Now.ToString("yyyyMMddHHmmss");
            bool result = await _customerService.CreateAsync(customer, customerId);

            if (!result)
            {
                return BadRequest(new ApiResponse 
                { 
                    Success = false,
                    Message = "Tạo khách hàng thất bại"
                });
            }

            return Ok(new ApiResponse 
            { 
                Success = true,
                Data = await _customerService.GetByIdAsync(customerId)
            });
        }

        [HttpPut]
        [AdminAuthorize(Code = Functions.UpdateClient)]
        public async Task<IActionResult> UpdateCustomer([FromQuery] string id, [FromBody] CustomerVM customer)
        {
            Customer? customerToUpdate = await _customerService.GetByIdAsync(id);
            if (customerToUpdate == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy khách hàng"
                });
            }

            bool result = await _customerService.UpdateAsync(id, customer);

            if (!result)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Cập nhật khách hàng thất bại"

                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Cập nhật khách hàng thành công",
                Data = await _customerService.GetByIdAsync(id)
            });
        }

        [HttpDelete]
        [AdminAuthorize(Code = Functions.DeleteClient)]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            Customer? customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy khách hàng"
                });
            }

            bool result = await _customerService.DeleteAsync(id);

            if (!result)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Xóa khách hàng thất bại"
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Xóa khách hàng thành công"
            });
        }
    }
}
