using BE_ThuyDuong.PayLoad.Request.HistoryPay;

namespace BE_ThuyDuong.PayLoad.Request.Product
{
    public class Request_CreateProduct
    {
        public string NameProduct { get; set; }
        public string Description { get; set; }
        public IFormFile UrlImg { get; set; }
        public decimal Price { get; set; }
        public int ProductTypeId { get; set; }
        public int Quantity { get; set; }
        public int TrademarkId { get; set; }
    }
}
