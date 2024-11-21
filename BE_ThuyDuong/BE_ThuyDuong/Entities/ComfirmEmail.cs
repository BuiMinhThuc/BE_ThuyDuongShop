namespace BE_ThuyDuong.Entities
{
    public class ComfirmEmail:BaseEntities
    {
        public string Otp {  get; set; }
        public string Email { get; set; }
        public int UserId {  get; set; }
        public DateTime Exprited {  get; set; }=DateTime.Now.AddMinutes(5);
        public bool Status {  get; set; }=false;
        public User? User { get; set; }

    }
}
