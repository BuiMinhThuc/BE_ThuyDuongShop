using BE_ThuyDuong.PayLoad.DTO;

namespace BE_ThuyDuong.Service.Interface
{
    public interface IService_Product
    {
        Task<IQueryable<DTO_Product>> GestListProducts(int pageSize, int pageNumbeer);
    }
}
