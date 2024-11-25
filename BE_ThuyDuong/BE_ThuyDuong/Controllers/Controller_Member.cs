using BE_ThuyDuong.Interfaces;
using BE_ThuyDuong.PayLoad.Request.Card;
using BE_ThuyDuong.PayLoad.Request.Product;
using BE_ThuyDuong.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLKS_v1.Implements;

namespace BE_ThuyDuong.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Controller_Member : ControllerBase
    {
        private readonly IService_Product service_Product;
        private readonly IService_Card service_Card;
        private readonly IService_HistotyPay service_HistotyPay;
        private readonly BE_ThuyDuong.Interfaces.IVNPayService vNPayService;

        public Controller_Member(IService_Product service_Product, IService_Card service_Card, IService_HistotyPay service_HistotyPay, IVNPayService vNPayService)
        {
            this.service_Product = service_Product;
            this.service_Card = service_Card;
            this.service_HistotyPay = service_HistotyPay;
            this.vNPayService = vNPayService;
        }

        [HttpPost("GetLinkVnPay")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetLinkVnPay([FromRoute] int hoaDonId )
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Ok("Vui lòng đăng nhập !");
            }
            int id = int.Parse(HttpContext.User.FindFirst("Id").Value);
            return Ok(await vNPayService.TaoDuongDanThanhToan(hoaDonId, HttpContext,id));
        }
        [HttpGet("Return")]
        public async Task<IActionResult> Return()
        {
            var vnpayData = HttpContext.Request.Query;

            return Redirect(await vNPayService.VNPayReturn(vnpayData));
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct(Request_CreateProduct request)
        {
            return Ok(await  service_Product.CreateProduct(request));
        }
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(Request_UpdateProduct request)
        {
            return Ok(await service_Product.UpdateProduct(request));
        }
        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            return Ok(await service_Product.DeleteProduct(productId));
        }

        [HttpGet("GestListProduct")]
        public async Task<IActionResult> GestListProduct(int pageSize =7, int pageNumber =1) {
            return Ok(await service_Product.GestListProducts(pageSize, pageNumber));
        }
        [HttpGet("SearchProduct")]
        public async Task<IActionResult> SearchProduct(string keyword,int pageSize = 7, int pageNumber = 1)
        {
            return Ok(await service_Product.SearchProducts(keyword,pageSize, pageNumber));
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
