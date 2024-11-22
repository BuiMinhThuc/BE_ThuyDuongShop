namespace BE_ThuyDuong.PayLoad.Request.Product
{
    public class Request_UpdateProduct
    {
        public int Id {  get; set; }
        public string NameProduct { get; set; }
        public string Description { get; set; }
        public FormFile UrlImg { get; set; }
        public decimal Price { get; set; }
        public int ProductTypeId { get; set; }
        public int Quantity { get; set; }
        public int TrademarkId { get; set; }
    }
}
