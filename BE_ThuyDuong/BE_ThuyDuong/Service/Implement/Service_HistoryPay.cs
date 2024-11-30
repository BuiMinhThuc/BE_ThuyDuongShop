using BE_ThuyDuong.DataContext;
using BE_ThuyDuong.PayLoad.Converter;
using BE_ThuyDuong.PayLoad.DTO;
using BE_ThuyDuong.PayLoad.Request.HistoryPay;
using BE_ThuyDuong.PayLoad.Response;
using BE_ThuyDuong.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace BE_ThuyDuong.Service.Implement
{
    public class Service_HistoryPay:IService_HistotyPay
    {
        private readonly AppDbContext dbContext;
        private readonly Converter_Historypay converter_Historypay;
        private readonly ResponseBase responseBase;

        public Service_HistoryPay(AppDbContext dbContext, Converter_Historypay converter_Historypay, ResponseBase responseBase)
        {
            this.dbContext = dbContext;
            this.converter_Historypay = converter_Historypay;
            this.responseBase = responseBase;
        }

        public async Task<IQueryable<DTO_HistoryPay>> GetFullListHistory( int pageSize, int pageNumber)
        {
            return await Task.FromResult(dbContext.historryPays.OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(x => converter_Historypay.EntityToDTO(x)));
        }

        /*public async Task<ResponseBase> CreateHistoryPay(int UserId,List<Request_ListProductPay> request)
        {
           foreach (var item in request)
            {
                var product = await dbContext.products.FirstOrDefaultAsync(x=>x.Id== item.ProductId);
                if(product == null)
                {
                    return responseBase.ResponseBaseError(404, $"Sản phẩm có id : {item.ProductId} không tồn tại !");
                }
                if (product.Quantity < item.Quantity)
                {
                    return responseBase.ResponseBaseError(404, $"Sản phẩm có id : {item.ProductId}, tên {product.NameProduct} số lượng chỉ còn {product.Quantity} !");
                }
                var historyPay = new BE_ThuyDuong.Entities.HistorryPay();
                historyPay.UserId=UserId;
                historyPay.ProductId=item.ProductId;
                historyPay.Quantity=item.Quantity;
                historyPay.CreatedDate=DateTime.Now;
                dbContext.historryPays.Add(historyPay);
            }
           await dbContext.SaveChangesAsync();
            return 








        }*/

        public async Task<IQueryable<DTO_HistoryPay>> GetListHistoryByUserId(int userId,int pageSize, int pageNumber)
        {
           return await Task.FromResult(dbContext.historryPays.Include(x=>x.Bill).AsNoTracking().Where(x=>x.Bill.UserId==userId).Skip((pageNumber-1)*pageSize)
               .Take(pageSize).Select(x=>converter_Historypay.EntityToDTO(x)));
        }
    }
}
