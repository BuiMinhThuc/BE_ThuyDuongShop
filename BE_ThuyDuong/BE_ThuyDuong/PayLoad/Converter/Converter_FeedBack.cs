using BE_ThuyDuong.DataContext;
using BE_ThuyDuong.Entities;
using BE_ThuyDuong.PayLoad.DTO;

namespace BE_ThuyDuong.PayLoad.Converter
{
    public class Converter_FeedBack
    {
        private readonly AppDbContext dbContext;

        public Converter_FeedBack(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public DTO_FeedBack EntitiToDTO(Feedback feedback)
        {
            var user = dbContext.users.FirstOrDefault(x => x.Id == feedback.UserId);
            return new DTO_FeedBack
            {
                Id = feedback.Id,
                UserId = feedback.UserId,
                UserName= user.UserName,
                UrlAvt= user.UrlAvt,
                Content = feedback.Content,
                Star = feedback.Star,
            };
        }
    }
}
