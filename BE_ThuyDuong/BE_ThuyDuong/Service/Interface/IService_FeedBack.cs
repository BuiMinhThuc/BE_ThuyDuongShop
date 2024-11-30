using BE_ThuyDuong.PayLoad.DTO;
using BE_ThuyDuong.PayLoad.Request.FeedBack;
using BE_ThuyDuong.PayLoad.Response;

namespace BE_ThuyDuong.Service.Interface
{
    public interface IService_FeedBack
    {
        Task<IQueryable<DTO_FeedBack>> GetFullListFeedBack(int pageSize, int pageNumber);
        Task<ResponseObject<DTO_FeedBack>> CreateFeedback(int UserId,Request_CreateFeedBack request);
    }
}
