using BE_ThuyDuong.DataContext;
using BE_ThuyDuong.PayLoad.DTO;
using BE_ThuyDuong.PayLoad.Response;
using BE_ThuyDuong.Service.Interface;
using BE_ThuyDuong.PayLoad.Converter;
using BE_ThuyDuong.PayLoad.Request.HistoryPay;
using BE_ThuyDuong.PayLoad.Request.Product;
using BE_ThuyDuong.Entities;
using BE_ThuyDuong.Handle;
using Microsoft.EntityFrameworkCore;

namespace BE_ThuyDuong.Service.Implement
{
    public class Service_Product : IService_Product
    {
        private readonly AppDbContext dbContext;
        private readonly ResponseBase responseBase;
        private readonly ResponseObject<DTO_Product> responseObject;
        private readonly BE_ThuyDuong.PayLoad.Converter.Coverter_Product coverter_Product;

        public Service_Product(AppDbContext dbContext, ResponseBase responseBase, ResponseObject<DTO_Product> responseObject, Coverter_Product coverter_Product)
        {
            this.dbContext = dbContext;
            this.responseBase = responseBase;
            this.responseObject = responseObject;
            this.coverter_Product = coverter_Product;
        }

        public async Task<ResponseObject<DTO_Product>> CreateProduct(Request_CreateProduct request)
        {
            if (!CheckInput.IsImage(request.UrlImg))
            {
                return responseObject.ResponseObjectError(StatusCodes.Status404NotFound, "Ảnh không hợp lệ !", null);
            }
            var productType = await dbContext.productTypes.FirstOrDefaultAsync(x => x.Id == request.ProductTypeId);
            if(productType == null)
            {
                return responseObject.ResponseObjectError(StatusCodes.Status404NotFound, "Loại sản phẩm không tồn tại !", null);
            }
            var trademark = await dbContext.trademarks.FirstOrDefaultAsync(x=>x.Id==request.TrademarkId);
            if( trademark == null)
            {
                return responseObject.ResponseObjectError(StatusCodes.Status404NotFound, "Thương hiệu không tồn tại !", null);
            }    
            if(request.UrlImg== null)
            {
                return responseObject.ResponseObjectError(StatusCodes.Status404NotFound, "Vui lòng chọn ảnh !", null);
            }

            
            CloudinaryService cloudinaryService = new CloudinaryService();
           
            var product = new Product();
            product.NameProduct = request.NameProduct;
            product.Description= request.Description;
            product.Price=request.Price;
            product.Quantity=request.Quantity;
            product.UrlImg = await cloudinaryService.UploadImage(request.UrlImg);
            product.ProductTypeId=request.ProductTypeId;
            product.TrademarkId=request.TrademarkId;
            dbContext.products.Add(product);
            await dbContext.SaveChangesAsync();
            return responseObject.ResponseObjectSucces("Thêm sản phẩm thành công !",coverter_Product.EntityToDTO(product));
        }

        public async Task<ResponseBase> DeleteProduct(int productId)
        {
            var product = await dbContext.products.FirstOrDefaultAsync(x => x.Id == productId);
            if(product == null)
            {
                return responseBase.ResponseBaseError(StatusCodes.Status404NotFound,"Sản phẩm không tồn tại !");
            }
            dbContext.products.Remove(product); await dbContext.SaveChangesAsync();
            await dbContext.SaveChangesAsync();
            return responseBase.ResponseBaseSucces("Xóa sản phẩm thành công !");


        }

        public async Task<IQueryable<DTO_Product>> GestListProducts(int pageSize, int pageNumbeer)
        {
            return await Task.FromResult( dbContext.products.Skip((pageNumbeer-1)*pageSize).Take(pageSize).Select( x=> coverter_Product.EntityToDTO(x)));
        }

        public async Task<IQueryable<DTO_Product>> SearchProducts(string keyword, int pageSize, int pageNumbeer)
        {
            return await Task.FromResult(dbContext.products.Where(x=>x.NameProduct.Contains(keyword)).Skip((pageNumbeer - 1) * pageSize).Take(pageSize).Select(x => coverter_Product.EntityToDTO(x)));
        }

        public async Task<ResponseObject<DTO_Product>> UpdateProduct(Request_UpdateProduct request)
        {
            if (!CheckInput.IsImage(request.UrlImg))
            {
                return responseObject.ResponseObjectError(StatusCodes.Status404NotFound, "Ảnh không hợp lệ !", null);
            }
            var productType = await dbContext.productTypes.FirstOrDefaultAsync(x => x.Id == request.ProductTypeId);
            if (productType == null)
            {
                return responseObject.ResponseObjectError(StatusCodes.Status404NotFound, "Loại sản phẩm không tồn tại !", null);
            }
            var trademark = await dbContext.trademarks.FirstOrDefaultAsync(x => x.Id == request.TrademarkId);
            if (trademark == null)
            {
                return responseObject.ResponseObjectError(StatusCodes.Status404NotFound, "Thương hiệu không tồn tại !", null);
            }
            if (request.UrlImg == null)
            {
                return responseObject.ResponseObjectError(StatusCodes.Status404NotFound, "Vui lòng chọn ảnh !", null);
            }


            CloudinaryService cloudinaryService = new CloudinaryService();

            var product = await dbContext.products.FirstOrDefaultAsync(x => x.Id == request.Id);
            product.NameProduct = request.NameProduct;
            product.Description = request.Description;
            product.Price = request.Price;
            product.Quantity = request.Quantity;
           // product.UrlImg = await cloudinaryService.ReplaceImage(product.UrlImg,request.UrlImg);
            product.UrlImg = await cloudinaryService.UploadImage(request.UrlImg);
            product.ProductTypeId = request.ProductTypeId;
            product.TrademarkId = request.TrademarkId;
            dbContext.products.Update(product);
            await dbContext.SaveChangesAsync();
            return responseObject.ResponseObjectSucces("Sửa sản phẩm thành công !", coverter_Product.EntityToDTO(product));
        }
    }
}
