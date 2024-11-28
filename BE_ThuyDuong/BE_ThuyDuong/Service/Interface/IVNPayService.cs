using BE_ThuyDuong.PayLoad.DTO;
using BE_ThuyDuong.PayLoad.Request.HistoryPay;
using BE_ThuyDuong.PayLoad.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_ThuyDuong.Interfaces
{
    public interface IVNPayService
    {
        Task<ResponseObject<DTO_Bill>> TaoDuongDanThanhToan(List<Request_ListProductPay> request, HttpContext httpContext, int id);
        Task<string> VNPayReturn(IQueryCollection vnpayData);
    }
}
