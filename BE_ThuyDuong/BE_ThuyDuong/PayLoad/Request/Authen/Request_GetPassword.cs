namespace BE_ThuyDuong.PayLoad.Request.Authen
{
    public class Request_GetPassword
    {
        public string otp {  get; set; }
        public string passwordnew { get; set; }
        public string passwordnewComfirm { get; set; }
    }
}
