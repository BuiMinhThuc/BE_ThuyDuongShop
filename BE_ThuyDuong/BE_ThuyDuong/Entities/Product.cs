namespace BE_ThuyDuong.Entities
{
    public class Product:BaseEntities
    {
        public string NameProduct {  get; set; }
        public string Description {  get; set; }
        public string UrlImg {  get; set; }
        public decimal Price {  get; set; }
        public int ProductTypeId {  get; set; }
        public int Quantity {  get; set; }
        public int TrademarkId {  get; set; }
        public ProductType? ProductType { get; set; }
        public Trademark? Trademark { get; set; }
        public ICollection<Card>? Cards { get; set; }
        public ICollection<HistorryPay>? HistorryPays { get; set; }

    }
}
