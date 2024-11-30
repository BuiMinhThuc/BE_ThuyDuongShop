namespace BE_ThuyDuong.Entities
{
    public class Feedback: BaseEntities
    {
        public int UserId { set; get; }
        public string Content {  set; get; }
        public int Star {  set; get; }
        public User? User { set; get; }


    }
}
