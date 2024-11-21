namespace BE_ThuyDuong.PayLoad.Request.Authen
{
    public class Request_Register
    {
        public string Username {  get; set; }
        public string Password {  get; set; }
        public string Email {  get; set; }
        public string? FullName {  get; set; }
        public string? PhoneNumber {  get; set; }
        public IFormFile? UrlAvt {  get; set; }
    }
}
