using KhangNghi_BE.Contants;
using KhangNghi_BE.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KhangNghi_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AdminAuthorize(Code = Functions.ManageUser)]
    public class AuthorizationController : ControllerBase
    {
    }
}
