using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BE_ThuyDuong.ConfigModels.VnPayPayment;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;


using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using BE_ThuyDuong.Service.Interface;
using BE_ThuyDuong.DataContext;
using BE_ThuyDuong.PayLoad.Request.HistoryPay;
using BE_ThuyDuong.PayLoad.Response;
using BE_ThuyDuong.Entities;
using BE_ThuyDuong.PayLoad.DTO;




namespace BE_ThuyDuong.Implements
{
    public class VNPayService : BE_ThuyDuong.Interfaces.IVNPayService
    {

        private readonly IConfiguration _configuration;
        private readonly IService_Authen _authService;
        private readonly ResponseObject<DTO_Bill> responseObject;
        private readonly AppDbContext dbContext;

        public VNPayService()
        {
        }

        public VNPayService(IConfiguration configuration, IService_Authen authService, ResponseObject<DTO_Bill> responseObject, AppDbContext dbContext)
        {
            _configuration = configuration;
            _authService = authService;
            this.responseObject = responseObject;
            this.dbContext = dbContext;
        }

        public async Task<ResponseObject<DTO_Bill>> TaoDuongDanThanhToan(List<Request_ListProductPay> request, HttpContext httpContext, int id)
        {
            decimal TotalPrice = 0;

            var bill = new BE_ThuyDuong.Entities.Bill();
            bill.UserId = id;
            bill.TotalPrice = TotalPrice;
            dbContext.bills.Add(bill);
            await dbContext.SaveChangesAsync();
            foreach (var item in request)
            {
               /* var product = await dbContext.products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
                if (product == null)
                {
                    return responseObject.ResponseObjectError(404, $"Sản phẩm có id : {item.ProductId} không tồn tại !",null);
                }
                if (product.Quantity < item.Quantity)
                {
                    return responseObject.ResponseObjectError(404, $"Sản phẩm có id : {item.ProductId}, tên {product.NameProduct} số lượng chỉ còn {product.Quantity} !",null);
                }*/
                var historyPay = new HistorryPay();
                historyPay.Quantity = item.Quantity;
                historyPay.BillId = bill.Id;
                historyPay.ProductId= item.ProductId;
                dbContext.historryPays.Add(historyPay);
                var product = await dbContext.products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
                TotalPrice += product.Price * (decimal)item.Quantity;
               
            }
            bill.TotalPrice = TotalPrice;
            dbContext.bills.Update(bill);
            await dbContext.SaveChangesAsync();
            VnPayLibrary vnpay = new VnPayLibrary();
            var user = await dbContext.users.FirstOrDefaultAsync(x => x.Id == id);

            double Total = (double)TotalPrice*100;
            // Thêm các tham số vào requestData
            vnpay.AddRequestData("vnp_Version", _configuration["VnPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _configuration["VnPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _configuration["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", Total.ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _configuration["VnPay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(httpContext));
            vnpay.AddRequestData("vnp_Locale", _configuration["VnPay:Locale"]);
            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toán hóa đơn {bill.Id} cho khách hàng: {user.UserName} ");
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", _configuration["VnPay:ReturnUrl"]);
            vnpay.AddRequestData("vnp_TxnRef", bill.Id.ToString());

            string baseUrl = _configuration["VnPay:BaseUrl"];
            string hashSecret = _configuration["VnPay:HashSecret"];

            DTO_Bill dTO_Bill = new DTO_Bill();
            dTO_Bill.listProductPay = request;
            dTO_Bill.TotalPrice = TotalPrice;
            dTO_Bill.Address = user.Address;
            dTO_Bill.Id = bill.Id;
            return responseObject.ResponseObjectSucces(vnpay.CreateRequestUrl(baseUrl, hashSecret), dTO_Bill);
        }



        public async Task<string> VNPayReturn(IQueryCollection vnpayData)
        {
            string vnp_TmnCode = _configuration.GetSection("VnPay:TmnCode").Value;
            string vnp_HashSecret = _configuration.GetSection("VnPay:HashSecret").Value;

            var vnPayLibrary = new VnPayLibrary();
            foreach (var (key, value) in vnpayData)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnPayLibrary.AddResponseData(key, value);
                }
            }

            string billId = vnPayLibrary.GetResponseData("vnp_TxnRef");
            string vnp_ResponseCode = vnPayLibrary.GetResponseData("vnp_ResponseCode");
            string vnp_TransactionStatus = vnPayLibrary.GetResponseData("vnp_TransactionStatus");
            string vnp_SecureHash = vnPayLibrary.GetResponseData("vnp_SecureHash");
            double vnp_Amount = Convert.ToDouble(vnPayLibrary.GetResponseData("vnp_Amount"));
            bool check = vnPayLibrary.ValidateSignature(vnp_SecureHash, vnp_HashSecret);

            if (check)
            {
                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                {
                    var Bill = await dbContext.bills.FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(billId));
                    var removeListProductCart = await dbContext.cards.Where(x => x.UserId == Bill.UserId).ToListAsync();
                    dbContext.cards.RemoveRange(removeListProductCart);
                    await dbContext.SaveChangesAsync();

                    foreach(var item in removeListProductCart)
                    {
                        var historyPay = new HistorryPay();
                        historyPay.BillId = Bill.Id;
                        historyPay.ProductId=item.ProductId;
                        historyPay.Quantity = item.Quantity;
                        dbContext.historryPays.Add(historyPay);
                        await dbContext.SaveChangesAsync();
                    }
                    return "http://127.0.0.1:5500/home_page.html?checkPay=True";
                }
                else
                {
                    var bill = await dbContext.bills.FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(billId));

                    var listHistorypay = await dbContext.historryPays.Where(x => x.BillId == bill.Id).ToListAsync();

                    dbContext.historryPays.RemoveRange(listHistorypay);

                    dbContext.bills.Remove(bill);
                    await dbContext.SaveChangesAsync();
                    return "http://127.0.0.1:5500/home_page.html";
                }
            }
            else
            {
                return "Có lỗi trong quá trình xử lý";
            }
        }
    }
}
