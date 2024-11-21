using BE_ThuyDuong.DataContext;
using BE_ThuyDuong.PayLoad.DTO;
using BE_ThuyDuong.PayLoad.Response;
using BE_ThuyDuong.Service.Interface;
using BE_ThuyDuong.PayLoad.Converter;

namespace BE_ThuyDuong.Service.Implement
{
    public class Service_Product : IService_Product
    {
        private readonly AppDbContext dbContext;
        private readonly ResponseBase responseBase;
        private readonly BE_ThuyDuong.PayLoad.Converter.Coverter_Product coverter_Product;

        public Service_Product(AppDbContext dbContext, ResponseBase responseBase, Coverter_Product coverter_Product)
        {
            this.dbContext = dbContext;
            this.responseBase = responseBase;
            this.coverter_Product = coverter_Product;
        }

        public async Task<IQueryable<DTO_Product>> GestListProducts(int pageSize, int pageNumbeer)
        {
            return await Task.FromResult( dbContext.products.Skip((pageNumbeer-1)*pageSize).Take(pageSize).Select( x=> coverter_Product.EntityToDTO(x)));
        }
    }
}
