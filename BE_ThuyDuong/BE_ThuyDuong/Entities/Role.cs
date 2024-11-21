namespace BE_ThuyDuong.Entities
{
    public class Role:BaseEntities
    {
        public string KeyRole { get; set; }
        public ICollection<User>? Users { get; set; }
    }
}
