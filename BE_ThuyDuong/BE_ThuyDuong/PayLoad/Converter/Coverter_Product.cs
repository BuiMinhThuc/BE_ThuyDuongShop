using BE_ThuyDuong.DataContext;
using BE_ThuyDuong.Entities;
using BE_ThuyDuong.PayLoad.DTO;
using Microsoft.EntityFrameworkCore;

namespace BE_ThuyDuong.PayLoad.Converter
{
    public class Coverter_Product
    {
        private readonly AppDbContext dbContext;

        public Coverter_Product(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public DTO_Product EntityToDTO(Product product)
        {
            return new DTO_Product
            {
                Id = product.Id,
                UrlImg = product.UrlImg,
                NameProduct=product.NameProduct,
                Description=product.Description,
                Price=product.Price,
                ProductTypeName=  dbContext.productTypes.Include(x=>x.Products).Where(x=>x.Id==product.ProductTypeId).Select(x=>x.TypeName).FirstOrDefault(),
                Quantity = product.Quantity,
                TrademarkName=  dbContext.trademarks.Include(x=>x.Products).Where(x=>x.Id==product.TrademarkId).Select(x=>x.TradamarkName).FirstOrDefault(),
            };
        }


    }
}
