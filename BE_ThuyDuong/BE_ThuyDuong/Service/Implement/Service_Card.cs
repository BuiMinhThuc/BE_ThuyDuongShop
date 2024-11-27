using BE_ThuyDuong.DataContext;
using BE_ThuyDuong.Entities;
using BE_ThuyDuong.PayLoad.DTO;
using BE_ThuyDuong.PayLoad.Request.Card;
using BE_ThuyDuong.PayLoad.Response;
using BE_ThuyDuong.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace BE_ThuyDuong.Service.Implement
{
    public class Service_Card : IService_Card
    {

        private readonly AppDbContext dbContext;
        private readonly ResponseBase responseBase;

        public Service_Card(AppDbContext dbContext, ResponseBase responseBase)
        {
            this.dbContext = dbContext;
            this.responseBase = responseBase;
        }

        public async Task<ResponseBase> AddCard(int UserId,Request_AddCard request)
        {
            var product = await dbContext.products.FirstOrDefaultAsync(x => x.Id == request.ProductId);
            if (product == null) {
                return responseBase.ResponseBaseError(StatusCodes.Status404NotFound, "Sản phẩm không hợp lệ !");
            }

            var card = new Card();
            card.ProductId=request.ProductId;
            card.Quantity = request.Quantity;
            card.UserId = UserId;
            dbContext.cards.Add(card);
            await dbContext.SaveChangesAsync();

            return responseBase.ResponseBaseSucces("Thêm thành công !");
        }

        public async Task<IQueryable<DTO_Card>> GestListCardForUserId(int userId, int pageSize, int pageNumbeer)
        {
            return await Task.FromResult(dbContext.cards.Where(x=>x.UserId==userId)
                .Skip((pageNumbeer-1)*pageSize)
                .Take(pageSize)
                .Select( x=>new DTO_Card
                {   UrlImg= dbContext.products.Where(z=>z.Id==x.ProductId).Select(x=>x.UrlImg).FirstOrDefault(),
                    ProductName= dbContext.products.Where(y=>y.Id==x.ProductId).Select(y=>y.NameProduct).FirstOrDefault(),
                    Quantity= x.Quantity

                })
                );
        }
    }
}
