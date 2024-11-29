using BE_ThuyDuong.Entities;
using BE_ThuyDuong.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_ThuyDuong.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Controller_Card : ControllerBase
    {
        private readonly IService_Card service_Card;

        public Controller_Card(IService_Card service_Card)
        {
            this.service_Card = service_Card;
        }

        [HttpDelete("DeleteProductInCard")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int CardId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("Vui lòng đăng nhập để thực hiện hành động này.");
            }

           
            return Ok(await service_Card.DeleteCard(CardId));
        }

    }
}
