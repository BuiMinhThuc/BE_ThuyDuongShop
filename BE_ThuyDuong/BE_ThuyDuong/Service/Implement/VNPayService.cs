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




namespace QLKS_v1.Implements
{
    public class VNPayService : BE_ThuyDuong.Interfaces.IVNPayService
    {

        private readonly IConfiguration _configuration;
        private readonly IService_Authen _authService;
        private readonly AppDbContext dbContext;

        public VNPayService()
        {
        }

        public VNPayService(IConfiguration configuration, IService_Authen authService, AppDbContext dbContext)
        {
            _configuration = configuration;
            _authService = authService;
            this.dbContext = dbContext;
        }

        public async Task<string> TaoDuongDanThanhToan(int BillId, HttpContext httpContext, int Customerid)
        {
            

            VnPayLibrary vnpay = new VnPayLibrary();

            double vnp_Amount = 0; // Chuyển đổi giá trị TotalPrice

            // Thêm các tham số vào requestData
            vnpay.AddRequestData("vnp_Version", _configuration["VnPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _configuration["VnPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _configuration["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", vnp_Amount.ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _configuration["VnPay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(httpContext));
            vnpay.AddRequestData("vnp_Locale", _configuration["VnPay:Locale"]);
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán hóa đơn: " + BillId);
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", _configuration["VnPay:ReturnUrl"]);
            vnpay.AddRequestData("vnp_TxnRef", BillId.ToString());

            string baseUrl = _configuration["VnPay:BaseUrl"];
            string hashSecret = _configuration["VnPay:HashSecret"];

            return vnpay.CreateRequestUrl(baseUrl, hashSecret);
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

            string hoaDonId = vnPayLibrary.GetResponseData("vnp_TxnRef");
            string vnp_ResponseCode = vnPayLibrary.GetResponseData("vnp_ResponseCode");
            string vnp_TransactionStatus = vnPayLibrary.GetResponseData("vnp_TransactionStatus");
            string vnp_SecureHash = vnPayLibrary.GetResponseData("vnp_SecureHash");
            double vnp_Amount = Convert.ToDouble(vnPayLibrary.GetResponseData("vnp_Amount"));
            bool check = vnPayLibrary.ValidateSignature(vnp_SecureHash, vnp_HashSecret);

            if (check)
            {
                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                {
                    
                    return "http://127.0.0.1:5501/FE/Member/HTML/Home.html?checkPay=True";
                }
                else
                {
                    
                    dbContext.SaveChanges();
                    return "http://127.0.0.1:5501/FE/Member/HTML/Home.html";
                }
            }
            else
            {
                return "Có lỗi trong quá trình xử lý";
            }
        }
    }
}
