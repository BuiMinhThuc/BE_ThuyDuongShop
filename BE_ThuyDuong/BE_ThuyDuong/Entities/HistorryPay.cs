namespace BE_ThuyDuong.Entities
{
    public class HistorryPay:BaseEntities
    {
        public int UserId {  get; set; }
        public int ProductId {  get; set; }
        public int Quantity {  get; set; }
        public DateTime? CreatedDate { get; set; }=DateTime.Now;
        public User? User { get; set; }
        public Product? Product { get; set; }

    }
}
