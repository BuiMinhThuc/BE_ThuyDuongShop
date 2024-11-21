namespace BE_ThuyDuong.PayLoad.Request.Authen
{
    public class Request_ChangePassWord
    {
        public string PassWordOld {  get; set; }
        public string PassWordNew { get; set;}
        public string PasswordComfirm { get; set;}
    }
}
