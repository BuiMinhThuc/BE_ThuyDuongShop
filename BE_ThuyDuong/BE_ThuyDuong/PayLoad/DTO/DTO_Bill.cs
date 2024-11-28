using BE_ThuyDuong.PayLoad.Request.HistoryPay;

namespace BE_ThuyDuong.PayLoad.DTO
{
    public class DTO_Bill
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public string Address { get; set; }
        public List<Request_ListProductPay> listProductPay { get; set; }
    }
}
