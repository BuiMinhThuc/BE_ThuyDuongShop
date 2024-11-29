using BE_ThuyDuong.DataContext;
using BE_ThuyDuong.Entities;
using BE_ThuyDuong.PayLoad.Converter;
using BE_ThuyDuong.PayLoad.DTO;
using BE_ThuyDuong.PayLoad.Request.Trademark;
using BE_ThuyDuong.PayLoad.Response;
using Microsoft.EntityFrameworkCore;
using BE_ThuyDuong.PayLoad.Request.Trademark;

namespace BE_ThuyDuong.Service.Implement
{
    public class Service_Trademark : BE_ThuyDuong.Service.Interface.IService_Trademark
    {
        private readonly AppDbContext dbContext;
        private readonly Converter_Trademark converter_Trademark;
        private readonly ResponseBase responseBase;
        private readonly ResponseObject<DTO_Trademark> responseObject;

        public Service_Trademark(AppDbContext dbContext, Converter_Trademark converter_Trademark, ResponseBase responseBase, ResponseObject<DTO_Trademark> responseObject)
        {
            this.dbContext = dbContext;
            this.converter_Trademark = converter_Trademark;
            this.responseBase = responseBase;
            this.responseObject = responseObject;
        }

        public async Task<ResponseObject<DTO_Trademark>> CreateTrademark(Request_CreateTrademark request)
        {
            var trademark = new Trademark
            {
                TradamarkName=request.TradamarkName,
                Address=request.Address,
            };
            dbContext.trademarks.Add(trademark);
            await dbContext.SaveChangesAsync();
            return responseObject.ResponseObjectSucces("Thêm thành công !", converter_Trademark.EntityToDTO(trademark));



        }

        public async Task<ResponseBase> DeleteTrademark(int TrademarkId)
        {
            var trademark = await dbContext.trademarks.FirstOrDefaultAsync(x => x.Id == TrademarkId);
            if (trademark == null)
            {
                return responseBase.ResponseBaseError(StatusCodes.Status400BadRequest, "Không tìm thấy thương hiệu này !");
            }
         

            dbContext.trademarks.Remove(trademark);
            await dbContext.SaveChangesAsync();
            return responseBase.ResponseBaseSucces("Xóa thành công !");
        }

        public async Task<IQueryable<DTO_Trademark>> GetFullList(int pageSize, int pageNumber)
        {
            return await Task.FromResult(dbContext.trademarks.Skip((pageNumber-1)*pageSize).Take(pageSize).Select(x=>converter_Trademark.EntityToDTO(x)));
        }

        /*public async Task<IQueryable<DTO_Trademark>> GetListForKey(string Key, int pageSize, int pageNumber)
        {
            return await Task.FromResult(dbContext.trademarks.Where(x=>x.Id==(int)Key || x.TradamarkName.Contains(Key) ||).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(x => converter_Trademark.EntityToDTO(x)));
        }*/

        public async Task<ResponseObject<DTO_Trademark>> UpdateTrademark(Request_UpdateTrademark request)
        {
            var trademark = await dbContext.trademarks.FirstOrDefaultAsync(x => x.Id == request.Id);
            if(trademark == null)
            {
                return responseObject.ResponseObjectError(StatusCodes.Status400BadRequest,"Không tìm thấy thương hiệu này !", null);
            }
                trademark.TradamarkName = request.TradamarkName;
                trademark.Address = request.Address;
         
            dbContext.trademarks.Update(trademark);
            await dbContext.SaveChangesAsync();
            return responseObject.ResponseObjectSucces("Sửa thành công !", converter_Trademark.EntityToDTO(trademark));
        }
    }
}
