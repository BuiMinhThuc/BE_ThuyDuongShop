namespace BE_ThuyDuong.PayLoad.DTO
{
    public class DTO_FeedBack
    {
        public int Id { get; set; }
        public int UserId { set; get; }
        public string UserName { set; get; }
        public string UrlAvt { set; get; }
        public string Content { set; get; }
        public int Star { set; get; }
    }
}
