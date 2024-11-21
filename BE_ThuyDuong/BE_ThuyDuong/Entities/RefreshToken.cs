namespace BE_ThuyDuong.Entities
{
    public class RefreshToken:BaseEntities
    {
        public string Token {  get; set; }
        public int UserId {  get; set; }
        public DateTime Exprited {  get; set; }
        public User? User { get; set; }

    }
}
