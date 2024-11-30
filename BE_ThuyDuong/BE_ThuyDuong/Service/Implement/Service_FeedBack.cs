using BE_ThuyDuong.DataContext;
using BE_ThuyDuong.Entities;
using BE_ThuyDuong.PayLoad.Converter;
using BE_ThuyDuong.PayLoad.DTO;
using BE_ThuyDuong.PayLoad.Request.FeedBack;
using BE_ThuyDuong.PayLoad.Response;
using BE_ThuyDuong.Service.Interface;

namespace BE_ThuyDuong.Service.Implement
{
    public class Service_FeedBack : IService_FeedBack
    {
        private readonly AppDbContext dbContext;
        private readonly Converter_FeedBack converter_FeedBack;
        private readonly ResponseObject<DTO_FeedBack> responseObject;

        public Service_FeedBack(AppDbContext dbContext, Converter_FeedBack converter_FeedBack, ResponseObject<DTO_FeedBack> responseObject)
        {
            this.dbContext = dbContext;
            this.converter_FeedBack = converter_FeedBack;
            this.responseObject = responseObject;
        }

        public async Task<ResponseObject<DTO_FeedBack>> CreateFeedback(int UserId,Request_CreateFeedBack request)
        {
            if (request.Content == null)
            {
                responseObject.ResponseObjectError(400, "Vui lòng nhập đánh giá !", null);
            }
            if(request.Star== null)
            {
                request.Star = 5;
            }
            var feedback = new Feedback()
            {
                UserId = UserId,
                Content = request.Content,
                Star = request.Star,

            };
            dbContext.feedbacks.Add(feedback);
            await dbContext.SaveChangesAsync();
            return responseObject.ResponseObjectSucces("Đánh giá thành công !", converter_FeedBack.EntitiToDTO(feedback));

        }

        public async Task<IQueryable<DTO_FeedBack>> GetFullListFeedBack(int pageSize, int pageNumber)
        {
            return await Task.FromResult(dbContext.feedbacks.OrderByDescending(x=>x.Id)
                .Skip((pageNumber-1)*pageSize).Take(pageSize).Select(x=> converter_FeedBack.EntitiToDTO(x)));
        }
    }
}
