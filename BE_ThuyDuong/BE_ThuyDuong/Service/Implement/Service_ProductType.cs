using BE_ThuyDuong.Controllers;
using BE_ThuyDuong.DataContext;
using BE_ThuyDuong.Entities;
using BE_ThuyDuong.PayLoad.Converter;
using BE_ThuyDuong.PayLoad.DTO;
using BE_ThuyDuong.PayLoad.Request.ProductType;
using BE_ThuyDuong.PayLoad.Response;
using BE_ThuyDuong.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace BE_ThuyDuong.Service.Implement
{
    public class Service_ProductType : IService_ProductType
    {

        private readonly AppDbContext dbContext;
        private readonly ResponseObject<DTO_ProductType> responseObject;
        private readonly Converter_ProductType converter;

        public Service_ProductType(AppDbContext dbContext, ResponseObject<DTO_ProductType> responseObject, Converter_ProductType converter)
        {
            this.dbContext = dbContext;
            this.responseObject = responseObject;
            this.converter = converter;
        }

        public async Task<ResponseObject<DTO_ProductType>> CreateProductType(Request_CreateProductType request)
        {
            var productType = new ProductType();
            productType.TypeName = request.TypeName;
            dbContext.productTypes.Add(productType);
            await dbContext.SaveChangesAsync();
            return responseObject.ResponseObjectSucces("Thêm thành công !",converter.EntityToDTO(productType));
        }

        public async Task<ResponseObject<DTO_ProductType>> DeleteProductType(int Id)
        {
            var productType = await dbContext.productTypes.FirstOrDefaultAsync(x => x.Id == Id);
            if (productType == null)
            {
                return responseObject.ResponseObjectError(StatusCodes.Status404NotFound, "Không tìm thấy loại sản phẩm này do Id không hợp lệ !", null);
            }
            dbContext.productTypes.Remove(productType);
            await dbContext.SaveChangesAsync();
            return responseObject.ResponseObjectSucces("Xóa thành công !", converter.EntityToDTO(productType));
        }

        public async Task<IQueryable<DTO_ProductType>> GetListProductType(int pageSize, int pageNumber)
        {
            return await Task.FromResult(dbContext.productTypes.OrderByDescending(x => x.Id).Skip((pageNumber-1)*pageSize).Take(pageSize).Select(x=>converter.EntityToDTO(x)));
        }

        public async Task<ResponseObject<DTO_ProductType>> UpdateProductType(Request_UpdateProductType request)
        {
            var productType = await dbContext.productTypes.FirstOrDefaultAsync(x => x.Id == request.Id);
            if(productType == null)
            {
                return responseObject.ResponseObjectError(StatusCodes.Status404NotFound, "Không tìm thấy loại sản phẩm này do Id không hợp lệ !", null);
            }
            productType.TypeName = request.ProductTypeName?? productType.TypeName;
            dbContext.productTypes.Update(productType);
            await dbContext.SaveChangesAsync();
            return responseObject.ResponseObjectSucces("Cập nhật thành công !", converter.EntityToDTO(productType));
        }
    }
}
