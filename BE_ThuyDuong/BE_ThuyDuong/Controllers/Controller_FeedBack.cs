using BE_ThuyDuong.PayLoad.Request.FeedBack;
using BE_ThuyDuong.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_ThuyDuong.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Controller_FeedBack : ControllerBase
    {
        private readonly IService_FeedBack service_FeedBack;

        public Controller_FeedBack(IService_FeedBack service_FeedBack)
        {
            this.service_FeedBack = service_FeedBack;
        }


        [HttpGet("GetFullListFeedBack")]
        public async Task<IActionResult> GetFullListFeedBack(int pageSize=10, int pageNumber =1)
        {
            return Ok(await service_FeedBack.GetFullListFeedBack(pageSize, pageNumber));
        }
        [HttpPost("CreateFeedBack")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateFeedBack( Request_CreateFeedBack request)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Ok("Vui lòng đăng nhập !");
            }
            int id = int.Parse(HttpContext.User.FindFirst("Id").Value);
            return Ok(await service_FeedBack.CreateFeedback(id, request));
        }
    }
}
