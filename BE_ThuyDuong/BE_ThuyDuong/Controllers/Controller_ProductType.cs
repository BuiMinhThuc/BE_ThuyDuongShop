using BE_ThuyDuong.PayLoad.Request.ProductType;
using BE_ThuyDuong.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_ThuyDuong.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Controller_ProductType : ControllerBase
    {
        private readonly IService_ProductType service_ProductType;

        public Controller_ProductType(IService_ProductType service_ProductType)
        {
            this.service_ProductType = service_ProductType;
        }

        [HttpPost("CreateProductType")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateProductType(Request_CreateProductType request)
        {
            // Kiểm tra nếu chưa đăng nhập
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("Vui lòng đăng nhập để thực hiện hành động này." );
            }

            // Kiểm tra nếu không phải admin
            if (!User.IsInRole("Admin"))
            {
                return Forbid( "Bạn không có quyền thực hiện hành động này." );
            }

            // Nếu người dùng đã đăng nhập và có quyền admin
            return Ok(await service_ProductType.CreateProductType(request));
        }


        [HttpPut("UpdateProductType")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateProductType(Request_UpdateProductType request)
        {
            // Kiểm tra nếu chưa đăng nhập
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("Vui lòng đăng nhập để thực hiện hành động này.");
            }

            // Kiểm tra nếu không phải admin
            if (!User.IsInRole("Admin"))
            {
                return Forbid("Bạn không có quyền thực hiện hành động này.");
            }
            return Ok(await service_ProductType.UpdateProductType(request));
        }
        [HttpDelete("DeleteProductType")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteProductType(int ProductTypeID)
        {
            // Kiểm tra nếu chưa đăng nhập
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("Vui lòng đăng nhập để thực hiện hành động này.");
            }

            // Kiểm tra nếu không phải admin
            if (!User.IsInRole("Admin"))
            {
                return Forbid("Bạn không có quyền thực hiện hành động này.");
            }
            return Ok(await service_ProductType.DeleteProductType(ProductTypeID));
        }
        [HttpGet("GetListProductType")]
        public async Task<IActionResult> GetListProductType(int pageSize=10, int pageNumber =1)
        {
            return Ok(await service_ProductType.GetListProductType(pageSize,pageNumber));
        }
    }
}
