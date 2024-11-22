using BE_ThuyDuong.PayLoad.DTO;
using BE_ThuyDuong.PayLoad.Request.HistoryPay;
using BE_ThuyDuong.PayLoad.Request.Product;
using BE_ThuyDuong.PayLoad.Response;

namespace BE_ThuyDuong.Service.Interface
{
    public interface IService_Product
    {
        Task<IQueryable<DTO_Product>> GestListProducts(int pageSize, int pageNumbeer);
        Task<ResponseObject<DTO_Product>> CreateProduct(Request_CreateProduct request);
        Task<ResponseObject<DTO_Product>> UpdateProduct(Request_UpdateProduct request);
        Task<ResponseBase> DeleteProduct(int productId);
    }
}
