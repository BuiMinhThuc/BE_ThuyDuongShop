using BE_ThuyDuong.PayLoad.DTO;

namespace BE_ThuyDuong.Service.Interface
{
    public interface IService_HistotyPay
    {
        Task<IQueryable<DTO_HistoryPay>> GetListHistoryByUserId(int userId, int pageSize, int pageNumber);


    }
}
