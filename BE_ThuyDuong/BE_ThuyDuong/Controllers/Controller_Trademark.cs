using BE_ThuyDuong.PayLoad.Request.Trademark;
using BE_ThuyDuong.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SammiShop.PayLoad.Request.Trademark;

namespace SammiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Controller_Trademark : ControllerBase
    {
        private readonly IService_Trademark service_Trademark;

        public Controller_Trademark(IService_Trademark service_Trademark)
        {
            this.service_Trademark = service_Trademark;
        }

        [HttpPost("CreateTrademark")]
        public async Task<IActionResult> CreateTrademark(Request_CreateTrademark request) {
            return Ok(await service_Trademark.CreateTrademark(request));
        
        }

        [HttpPut("UpdateTrademark")]
        public async Task<IActionResult> UpdateTrademark(Request_UpdateTrademark request)
        {
            return Ok(await service_Trademark.UpdateTrademark(request));
        }

        [HttpDelete("DeleteTrademark")]
        public async Task<IActionResult> DeleteTrademark(int Id)
        {
            return Ok(await service_Trademark.DeleteTrademark(Id));
        }



        [HttpGet("GetFullListTrademark")]
        public async Task<IActionResult> GetFullListTrademark(int pageSize=10, int pageNumber=1)
        {
            return Ok(await service_Trademark.GetFullList(pageSize,pageNumber));
        }
      





    }
}
