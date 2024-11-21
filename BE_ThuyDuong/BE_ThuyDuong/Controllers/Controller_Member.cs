using BE_ThuyDuong.PayLoad.Request.Card;
using BE_ThuyDuong.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_ThuyDuong.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Controller_Member : ControllerBase
    {
        private readonly IService_Product service_Product;
        private readonly IService_Card service_Card;
        private readonly IService_HistotyPay service_HistotyPay;

        public Controller_Member(IService_Product service_Product, IService_Card service_Card, IService_HistotyPay service_HistotyPay)
        {
            this.service_Product = service_Product;
            this.service_Card = service_Card;
            this.service_HistotyPay = service_HistotyPay;
        }

        [HttpGet("GestListProduct")]
        public async Task<IActionResult> GestListProduct(int pageSize =7, int pageNumber =1) {
            return Ok(await service_Product.GestListProducts(pageSize, pageNumber));
        }
        [HttpGet("GestListCardForUserId")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GestListCardForUserId(int pageSize = 7, int pageNumber = 1)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return BadRequest("Vui lòng đăng nhập !");
            }
            int id = int.Parse(HttpContext.User.FindFirst("Id").Value);
            return Ok(await service_Card.GestListCardForUserId(id, pageSize, pageNumber));
        }
        [HttpPost("AddCard")]
        public async Task<IActionResult> AddCard(Request_AddCard request)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return BadRequest("Vui lòng đăng nhập !");
            }
            int id = int.Parse(HttpContext.User.FindFirst("Id").Value);
            return Ok(await service_Card.AddCard(id,request));
        }

        [HttpGet("GestListHistoryPayByUserId")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GestListHistoryPayByUserId(int pageSize = 7, int pageNumber = 1)
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
