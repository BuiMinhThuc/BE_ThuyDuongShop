namespace BE_ThuyDuong.Entities
{
    public class Trademark:BaseEntities
    {
        public string TradamarkName {  get; set; }
        public string Address {  get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
