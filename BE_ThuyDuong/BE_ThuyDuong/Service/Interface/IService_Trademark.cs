using BE_ThuyDuong.PayLoad.DTO;
using BE_ThuyDuong.PayLoad.Request.Trademark;
using BE_ThuyDuong.PayLoad.Response;
using BE_ThuyDuong.PayLoad.Request.Trademark;

namespace BE_ThuyDuong.Service.Interface
{
    public interface IService_Trademark
    {
        public Task<IQueryable<DTO_Trademark>> GetFullList(int pageSize, int pageNumber);
       /* public Task<IQueryable<DTO_Trademark>> GetListForID(int Id,int pageSize, int pageNumber);*/
        public Task<ResponseObject<DTO_Trademark>> CreateTrademark(Request_CreateTrademark request);
        public Task<ResponseObject<DTO_Trademark>> UpdateTrademark(Request_UpdateTrademark request);
        public Task<ResponseBase> DeleteTrademark(int TrademarkId);
       
    }
}
