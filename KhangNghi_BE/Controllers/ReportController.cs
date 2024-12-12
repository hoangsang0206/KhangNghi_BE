using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Filters;
using KhangNghi_BE.Services;
using Microsoft.AspNetCore.Mvc;

namespace KhangNghi_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController, AdminAuthorize]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("all-summary")]
        public async Task<IActionResult> GetAllSummaryAsync()
        {
            return Ok(new ApiResponse
            {
                Success = true,
                Data = await _reportService.GetAllSummaryAsync()
            });
        }
    }
}
