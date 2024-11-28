using BE_ThuyDuong.Entities;

namespace BE_ThuyDuong.Entities
{
    public class HistorryPay:BaseEntities
    {
        public int BillId { get; set; }

        public int ProductId {  get; set; }
        public int Quantity {  get; set; }

        public Product? Product { get; set; }
        public Bill? Bill { get; set; }

    }
}
