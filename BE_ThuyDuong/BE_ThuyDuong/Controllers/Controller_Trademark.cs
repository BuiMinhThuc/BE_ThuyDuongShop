﻿using BE_ThuyDuong.PayLoad.Request.Trademark;
using BE_ThuyDuong.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BE_ThuyDuong.PayLoad.Request.Trademark;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace BE_ThuyDuong.Controllers
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateTrademark(Request_CreateTrademark request) {
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
            return Ok(await service_Trademark.CreateTrademark(request));
        
        }

        [HttpPut("UpdateTrademark")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateTrademark(Request_UpdateTrademark request)
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
            return Ok(await service_Trademark.UpdateTrademark(request));
        }

        [HttpDelete("DeleteTrademark")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteTrademark(int Id)
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
            return Ok(await service_Trademark.DeleteTrademark(Id));
        }



        [HttpGet("GetFullListTrademark")]
        public async Task<IActionResult> GetFullListTrademark(int pageSize=10, int pageNumber=1)
        {
            return Ok(await service_Trademark.GetFullList(pageSize,pageNumber));
        }
      





    }
}
