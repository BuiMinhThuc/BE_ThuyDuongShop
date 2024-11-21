namespace BE_ThuyDuong.Entities
{
    public class Card:BaseEntities
    {
        public int ProductId {  get; set; }
        public int UserId {  get; set; }
        public int Quantity {  get; set; }

        public Product? Product { get; set; }
        public User? User { get; set; }
    }
}
