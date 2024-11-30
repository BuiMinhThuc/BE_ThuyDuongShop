using BE_ThuyDuong.PayLoad.DTO;
using BE_ThuyDuong.PayLoad.Request.Authen;
using BE_ThuyDuong.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_ThuyDuong.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Controller_Authen : ControllerBase
    {
        private readonly IService_Authen service_Authen;

        public Controller_Authen(IService_Authen service_Authen)
        {
            this.service_Authen = service_Authen;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(Request_Register request)
        {
            return Ok(await service_Authen.Register(request));
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(Request_Login request)
        {
            return Ok(await service_Authen.Login(request));
        }
        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(DTO_Token request)
        {
            return Ok(await service_Authen.RenewAccessToken(request));
        }
        [HttpPost("GetOtpforGetPassword")]
        public async Task<IActionResult> GetOtpforGetPassword(string email)
        {
            return Ok(await service_Authen.SendOtpForEmail(email));
        }
        [HttpPut("getPassword")]
        public async Task<IActionResult> getPassword(Request_GetPassword request)
        {
            return Ok(await service_Authen.GetPassword(request));
        }
        [HttpGet("GetUserbyLogin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserbyLogin()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Ok("Vui lòng đăng nhập !");
            }
            int id = int.Parse(HttpContext.User.FindFirst("Id").Value);
            return Ok(await service_Authen.GetUserById(id));
        }

       /* [HttpGet("CheckRole")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CheckRole()
        {

            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Ok("Vui lòng đăng nhập !");
            }
            int id = int.Parse(HttpContext.User.FindFirst("Id").Value);
            string Role = HttpContext.User.FindFirst("RoleId").Value;

            return Ok($"{id} đăng nhập với quyền là  {Role}!");
        }*/
        [HttpPut("ChangePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword(Request_ChangePassWord request)
        {

            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Ok("Vui lòng đăng nhập !");
            }
            int id = int.Parse(HttpContext.User.FindFirst("Id").Value);
            return Ok(await service_Authen.ChangePassword(id, request));
        }
        [HttpPut("EditRoleUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> EditRoleUser(Request_EditRoleUser request)
        {

            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Ok("Vui lòng đăng nhập !");
            }
            // Kiểm tra nếu không phải admin
            if (!User.IsInRole("Admin"))
            {
                return Ok("Bạn không có quyền thực hiện hành động này.");
            }
            return Ok(await service_Authen.EditRoletUser(request));
        }

        [HttpGet("SearchUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SearchUser(string Key, int pageSize =10, int pageNumber =1)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Ok("Vui lòng đăng nhập !");
            }
            if (!User.IsInRole("Admin"))
            {
                return Ok("Bạn không có quyền thực hiện hành động này.");
            }
          
            return Ok(await service_Authen.SearchUser(Key, pageSize,pageNumber));
        }

        [HttpGet("SearchUsGetFullListUserer")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetFullListUser( int pageSize = 10, int pageNumber = 1)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Ok("Vui lòng đăng nhập !");
            }
            if (!User.IsInRole("Admin"))
            {
                return Ok("Bạn không có quyền thực hiện hành động này.");
            }

            return Ok(await service_Authen.GetFullListUser( pageSize, pageNumber));
        }
        [HttpGet("GetFullListRole")]
 /*       [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]*/
        public async Task<IActionResult> GetFullListRole()
        {
            /*if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Ok("Vui lòng đăng nhập !");
            }
            if (!User.IsInRole("Admin"))
            {
                return Ok("Bạn không có quyền thực hiện hành động này.");
            }*/

            return Ok(await service_Authen.GetFullListRole());
        }

    }
}
