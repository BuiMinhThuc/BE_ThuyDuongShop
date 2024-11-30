using BE_ThuyDuong.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_ThuyDuong.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Controller_HistoryPay : ControllerBase
    {
        private readonly IService_HistotyPay service_HistotyPay;

        public Controller_HistoryPay(IService_HistotyPay service_HistotyPay)
        {
            this.service_HistotyPay = service_HistotyPay;
        }


        [HttpGet("GetFullListHistoryPay")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetFullListHistoryPay(int pageSize=10, int pageNumber=1) 
        {
            return Ok(await service_HistotyPay.GetFullListHistory(pageSize, pageNumber));
        }
        [HttpGet("GestListHistoryPayByUserId")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GestListHistoryPayByUserId(int pageSize = 50, int pageNumber = 1)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return BadRequest("Vui lòng đăng nhập !");
            }
            int id = int.Parse(HttpContext.User.FindFirst("Id").Value);
            return Ok(await service_HistotyPay.GetListHistoryByUserId(id, pageSize, pageNumber));
        }

    }
}
