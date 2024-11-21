﻿using BE_ThuyDuong.PayLoad.DTO;
using BE_ThuyDuong.PayLoad.Request.Authen;
using BE_ThuyDuong.PayLoad.Response;

namespace BE_ThuyDuong.Service.Interface
{
    public interface IService_Authen
    {
        Task<ResponseBase> Register(Request_Register request);
        Task<ResponseBase> ChangePassword(int UserId, Request_ChangePassWord request);
  /*      Task<ResponseBase> SendOtpForEmail(string email);*/
        Task<ResponseObject<DTO_Token>> Login(Request_Login request);
        Task<ResponseObject<DTO_User>> GetUserById(int UserId);
    }
}