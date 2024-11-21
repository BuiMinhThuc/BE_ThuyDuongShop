using BE_ThuyDuong.DataContext;
using BE_ThuyDuong.PayLoad.Converter;
using BE_ThuyDuong.PayLoad.DTO;
using BE_ThuyDuong.Service.Interface;

namespace BE_ThuyDuong.Service.Implement
{
    public class Service_HistoryPay:IService_HistotyPay
    {
        private readonly AppDbContext dbContext;
        private readonly Converter_Historypay converter_Historypay;

        public Service_HistoryPay(AppDbContext dbContext, Converter_Historypay converter_Historypay)
        {
            this.dbContext = dbContext;
            this.converter_Historypay = converter_Historypay;
        }

        public async Task<IQueryable<DTO_HistoryPay>> GetListHistoryByUserId(int userId,int pageSize, int pageNumber)
        {
           return await Task.FromResult(dbContext.historryPays.Where(x=>x.UserId==userId).Skip((pageNumber-1)*pageSize)
               .Take(pageSize).Select(x=>converter_Historypay.EntityToDTO(x)));
        }
    }
}
