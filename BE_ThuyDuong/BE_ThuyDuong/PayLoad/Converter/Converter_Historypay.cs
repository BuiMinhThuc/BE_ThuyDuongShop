using BE_ThuyDuong.DataContext;
using BE_ThuyDuong.PayLoad.DTO;
using Microsoft.EntityFrameworkCore;

namespace BE_ThuyDuong.PayLoad.Converter
{
    public class Converter_Historypay
    {
        private readonly AppDbContext dbContext;

        public Converter_Historypay(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public DTO_HistoryPay EntityToDTO(BE_ThuyDuong.Entities.HistorryPay historryPay)
        {
            

            return new DTO_HistoryPay()
            {
                Id = historryPay.Id,
                ProductName= dbContext.historryPays.Include(x=>x.Product).Where(x => x.Id == historryPay.Id).Select(x=>x.Product.NameProduct).FirstOrDefault(),
               
                CreateTime= dbContext.historryPays.Include(x=>x.Bill).Where(x=>x.Id==historryPay.Id).Select(x=>x.Bill.CreateTime).FirstOrDefault(),
                Quantity=historryPay.Quantity,
                TotalPrice = dbContext.historryPays.Include(x => x.Product).Where(x => x.Id == historryPay.Id).Select(x => x.Product.Price).FirstOrDefault()* historryPay.Quantity,
                
            };
        }
    }
}
