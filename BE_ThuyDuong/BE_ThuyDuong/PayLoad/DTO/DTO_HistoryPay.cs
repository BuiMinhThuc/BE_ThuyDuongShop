namespace BE_ThuyDuong.PayLoad.DTO
{
    public class DTO_HistoryPay
    {
        public int Id { get; set; }
       public int UserId {  get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime? CreateTime {  get; set; }
    }
}
