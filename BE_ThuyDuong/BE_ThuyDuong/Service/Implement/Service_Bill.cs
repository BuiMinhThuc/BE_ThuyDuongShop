using BE_ThuyDuong.DataContext;
using BE_ThuyDuong.Entities;
using BE_ThuyDuong.PayLoad.Request.HistoryPay;
using BE_ThuyDuong.PayLoad.Response;
using Microsoft.EntityFrameworkCore;
using BE_ThuyDuong.Entities;
using BE_ThuyDuong.Service.Interface;
using SammiShop.Service.Interface;


namespace BE_ThuyDuong.Service.Implement
{
    public class Service_Bill : IService_Bill
    {
        private readonly AppDbContext dbContext;
        private readonly ResponseObject<Bill> responseObject;

        public Service_Bill(AppDbContext dbContext, ResponseObject<Bill> responseObject)
        {
            this.dbContext = dbContext;
            this.responseObject = responseObject;
        }

       /* public async Task<ResponseObject<Bill>> CreateBill(List<Request_ListProductPay> request, int UserId)
        {
           
        }*/
    }
}
