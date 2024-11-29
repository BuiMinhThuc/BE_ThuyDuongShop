using BE_ThuyDuong.PayLoad.DTO;
using BE_ThuyDuong.PayLoad.Request.Card;
using BE_ThuyDuong.PayLoad.Response;

namespace BE_ThuyDuong.Service.Interface
{
    public interface IService_Card
    {
        Task<IQueryable<DTO_Card>> GestListCardForUserId(int userId,int pageSize, int pageNumbeer);
        Task<ResponseBase> AddCard(int UserId, Request_AddCard request);
        Task<ResponseBase> DeleteCard(int CardId);
    }
}
