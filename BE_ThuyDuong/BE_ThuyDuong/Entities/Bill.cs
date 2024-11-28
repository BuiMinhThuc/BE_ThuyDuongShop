using BE_ThuyDuong.Entities;

namespace BE_ThuyDuong.Entities
{
    public class Bill:BaseEntities
    {
        public int UserId { get; set; }
        public DateTime? CreateTime { get;set; } = DateTime.Now;
        public decimal? TotalPrice {  get; set; }
       
        public ICollection<HistorryPay>? HistorryPays { get; set;}
        public User? User { get; set; }
    }
}
