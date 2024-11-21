namespace BE_ThuyDuong.Entities
{
    public class ProductType:BaseEntities
    {
        public string TypeName {  get; set; }
        public ICollection<Product>?  Products { get; set;}
        
    }
}
