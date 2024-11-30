using BE_ThuyDuong.PayLoad.DTO;
using BE_ThuyDuong.PayLoad.Request.HistoryPay;
using BE_ThuyDuong.PayLoad.Response;

namespace BE_ThuyDuong.Service.Interface
{
    public interface IService_HistotyPay
    {
        Task<IQueryable<DTO_HistoryPay>> GetListHistoryByUserId(int userId, int pageSize, int pageNumber);
        Task<IQueryable<DTO_HistoryPay>> GetFullListHistory( int pageSize, int pageNumber);
       /* Task<ResponseBase> CreateHistoryPay(int UserId,List<Request_ListProductPay> request);*/

    }
}
