namespace BE_ThuyDuong.PayLoad.DTO
{
    public class DTO_User
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UrlAvt { get; set; }
        public string Email { get; set; }
        public string? FullName { get; set; }
        public int? RoleId { get; set; }
    }
}
