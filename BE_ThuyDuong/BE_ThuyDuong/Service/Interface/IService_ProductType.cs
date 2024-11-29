using BE_ThuyDuong.PayLoad.DTO;
using BE_ThuyDuong.PayLoad.Request.ProductType;
using BE_ThuyDuong.PayLoad.Response;

namespace BE_ThuyDuong.Service.Interface
{
    public interface IService_ProductType
    {
        Task<ResponseObject<DTO_ProductType>> CreateProductType(Request_CreateProductType request);
        Task<ResponseObject<DTO_ProductType>> UpdateProductType(Request_UpdateProductType request);

        Task<ResponseObject<DTO_ProductType>> DeleteProductType(int Id);
        Task<IQueryable<DTO_ProductType>> GetListProductType(int pageSize, int pageNumber);
    }
}
