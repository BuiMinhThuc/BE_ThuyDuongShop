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

        [HttpGet("CheckRole")]
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
        }
        [HttpPut("ChangePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword(Request_ChangePassWord request)
        {

            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Ok("Vui lòng đăng nhập !");
            }
            int id = int.Parse(HttpContext.User.FindFirst("Id").Value);
            return Ok(service_Authen.ChangePassword(id, request));
        }




    }
}
