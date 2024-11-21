namespace BE_ThuyDuong.PayLoad.DTO
{
    public class DTO_Product
    {
        public int Id { get; set; }
        public string NameProduct { get; set; }
        public string Description { get; set; }
        public string UrlImg { get; set; }
        public decimal Price { get; set; }
        public string ProductTypeName { get; set; }
        public int Quantity { get; set; }
        public string TrademarkName { get; set; }
    }
}
