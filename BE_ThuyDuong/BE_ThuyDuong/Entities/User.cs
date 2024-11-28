using BE_ThuyDuong.Entities;

namespace BE_ThuyDuong.Entities
{
    public class User:BaseEntities
    {
        public string UserName {  get; set; }
        public string PassWord {  get; set; }
        public string? PhoneNumber {  get; set; }
        public string? UrlAvt {  get; set; }

        public string Email {  get; set; }
        public string? Address { get; set; }
        public string? FullName {  get; set; }
        public int? RoleId {  get; set; }=1;
        public Role? Role { get; set; }
        public ICollection<Bill>? Bills { get; set; }
        public ICollection<ComfirmEmail>? ComfirmEmails { get; set; }
        public ICollection<RefreshToken>? RefreshTokens { get; set; }
        public ICollection<Card>? Cards { get; set; }
     


    }
}
