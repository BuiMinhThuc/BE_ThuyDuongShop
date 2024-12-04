using BE_ThuyDuong.DataContext;
using BE_ThuyDuong.Entities;
using BE_ThuyDuong.Handle;
using BE_ThuyDuong.PayLoad.DTO;
using BE_ThuyDuong.PayLoad.Request.Authen;
using BE_ThuyDuong.PayLoad.Response;
using BE_ThuyDuong.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SixLabors.ImageSharp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using static System.Net.WebRequestMethods;

using Microsoft.Extensions.Configuration;
using BE_ThuyDuong.PayLoad.Converter;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BE_ThuyDuong.Service.Implement
{
    public class Service_Authen : IService_Authen
    {
        private readonly AppDbContext dbContext;
        private readonly ResponseBase responseBase;
        private readonly Converter_User converter_User;
        private readonly IConfiguration configuration;
        private readonly ResponseObject<DTO_Token> responseObjectToken;
        private readonly ResponseObject<DTO_User> responseObject;

        public Service_Authen(AppDbContext dbContext, ResponseBase responseBase, Converter_User converter_User, IConfiguration configuration, ResponseObject<DTO_Token> responseObjectToken, ResponseObject<DTO_User> responseObject)
        {
            this.dbContext = dbContext;
            this.responseBase = responseBase;
            this.converter_User = converter_User;
            this.configuration = configuration;
            this.responseObjectToken = responseObjectToken;
            this.responseObject = responseObject;
        }
        public async Task<ResponseObject<DTO_Token>> RenewAccessToken(DTO_Token request)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:SecretKey").Value);

            var tokenValidation = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:SecretKey").Value))
            };

            try
            {
                var tokenAuthentication = jwtTokenHandler.ValidateToken(request.AccessToken, tokenValidation, out var validatedToken);
                if (validatedToken is not JwtSecurityToken jwtSecurityToken || jwtSecurityToken.Header.Alg != SecurityAlgorithms.HmacSha256)
                {
                    return responseObjectToken.ResponseObjectError(StatusCodes.Status400BadRequest, "Token không hợp lệ", null);
                }
                RefreshToken refreshToken = await dbContext.refreshTokens.FirstOrDefaultAsync(x => x.Token == request.RefreshToken);
                if (refreshToken == null)
                {
                    return responseObjectToken.ResponseObjectError(StatusCodes.Status404NotFound, "RefreshToken không tồn tại trong database", null);
                }
                if (refreshToken.Exprited < DateTime.Now)
                {
                    return responseObjectToken.ResponseObjectError(StatusCodes.Status401Unauthorized, "Token chưa hết hạn", null);
                }
                var user = dbContext.users.FirstOrDefault(x => x.Id == refreshToken.UserId);
                if (user == null)
                {
                    return responseObjectToken.ResponseObjectError(StatusCodes.Status404NotFound, "Người dùng không tồn tại", null);
                }
                var newToken = GenerateAccessToken(user);

                return responseObjectToken.ResponseObjectSucces("Làm mới token thành công", newToken);
            }
            catch (Exception ex)
            {
                return responseObjectToken.ResponseObjectError(StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }
        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

        private DTO_Token GenerateAccessToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:SecretKey").Value);

            var decentralization = dbContext.roles.FirstOrDefault(x => x.Id == user.RoleId);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("Username", user.UserName.ToString()),
                    new Claim("RoleId", user.RoleId.ToString()),
                    
                    //new Claim("UrlAvatar", user.UrlAvatar.ToString()),
                    new Claim("FullName", user.FullName.ToString()),
                    new Claim(ClaimTypes.Role, decentralization?.KeyRole ?? "")
                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            RefreshToken rf = new RefreshToken
            {
                Token = refreshToken,
                Exprited = DateTime.Now.AddHours(4),
                UserId = user.Id
            };

            dbContext.refreshTokens.Add(rf);
            dbContext.SaveChanges();

            DTO_Token tokenDTO = new DTO_Token
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            return tokenDTO;
        }

            public async Task<ResponseObject<DTO_Token>> Login(Request_Login request)
        {
            if (request.Username == null)
            {
                return responseObjectToken.ResponseObjectError(StatusCodes.Status400BadRequest, "Vui lòng nhập Username !", null);
            }
            if (request.Password == null)
            {
                return responseObjectToken.ResponseObjectError(StatusCodes.Status400BadRequest, "Vui lòng nhập Password !", null);
            }
            var user =  await dbContext.users.FirstOrDefaultAsync(x=>x.UserName== request.Username);
            if(user == null)
            {
                return responseObjectToken.ResponseObjectError(StatusCodes.Status400BadRequest, "Tên đăng nhập hoặc mật khẩu không chính xác !", null);
            }
            if(!BCrypt.Net.BCrypt.Verify(request.Password, user.PassWord))
            {
                return responseObjectToken.ResponseObjectError(StatusCodes.Status400BadRequest, "Tên đăng nhập hoặc mật khẩu không chính xác !", null);
            }
            return responseObjectToken.ResponseObjectSucces("Đăng nhập thành công", GenerateAccessToken(user));


        }

        public async Task<ResponseBase> Register(Request_Register request)
        {
            string UserInput = CheckInput.IsValidUsername(request.Username);
            string PassInput = CheckInput.IsPassWord(request.Password);
            string PhoneInput = CheckInput.IsValidPhoneNumber(request.PhoneNumber);
            bool EmailInput = CheckInput.IsValiEmail(request.Email);


            if (UserInput != request.Username)
            {
                return responseBase.ResponseBaseError(StatusCodes.Status400BadRequest, UserInput);
            }
            var checkUser = await dbContext.users.FirstOrDefaultAsync(x => x.UserName == request.Username);
            if (checkUser != null)
            {
                return responseBase.ResponseBaseError(StatusCodes.Status400BadRequest, "UserName đã tồn tại rồi !");
            }

            if (PassInput != request.Password)
            {
                return responseBase.ResponseBaseError(StatusCodes.Status400BadRequest, PassInput);
            }
            if (request.PhoneNumber != null)
                if (PhoneInput != request.PhoneNumber)
                {
                    return responseBase.ResponseBaseError(StatusCodes.Status400BadRequest, PhoneInput);
                }
            if (!EmailInput)
            {
                return responseBase.ResponseBaseError(StatusCodes.Status400BadRequest, "Email không hợp lệ !");
            }
            var checkemail= await dbContext.users.FirstOrDefaultAsync(x=>x.Email==request.Email);
            if(checkemail != null)
            {
                return responseBase.ResponseBaseError(StatusCodes.Status400BadRequest, "Email đã được sử dụng rồi, vui lòng chọn email khác !");
            }
            if(request.UrlAvt != null)
                if (!CheckInput.IsImage(request.UrlAvt))
                {
                    return responseBase.ResponseBaseError(StatusCodes.Status400BadRequest, "Ảnh không hợp lệ !");
                }

            CloudinaryService cloudinaryService = new CloudinaryService();
            string imgNull = "https://media.istockphoto.com/id/1300845620/vector/user-icon-flat-isolated-on-white-background-user-symbol-vector-illustration.jpg?s=612x612&w=0&k=20&c=yBeyba0hUkh14_jgv1OKqIH0CCSWU_4ckRkAoy2p73o=";
           
            EmailTo emailTo = new EmailTo();
            emailTo.Subject = "XÁC NHẬN ĐĂNG KÍ SAMMISHOP !";
            emailTo.Mail = request.Email;
            Random random = new Random();
            int otp = random.Next(1000, 9999);
            emailTo.Content = $"Đây là mã kích hoạt tài khoản của bạn, mã sẽ hết hạn sau 5 phút :\n SammiShop@{otp} ";
           var checkEmailv1= await emailTo.SendEmailAsync(emailTo);
            if( checkEmailv1 != "Gửi email thành công" ) { return responseBase.ResponseBaseError(StatusCodes.Status400BadRequest, "Email không hợp lệ !"); }
            Console.WriteLine(checkEmailv1);

            User user = new User();
            user.UserName = request.Username;
            user.Address = request.Address??null;
            user.FullName = request.FullName??null;
            user.PassWord = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.PhoneNumber= request.PhoneNumber?? null;
            user.Email = request.Email;
           
            if (request.UrlAvt == null)
               user.UrlAvt = imgNull;
            else
                user.UrlAvt =await cloudinaryService.UploadImage(request.UrlAvt);
            dbContext.users.Add(user);
            await dbContext.SaveChangesAsync();

            ComfirmEmail comfirmEmail = new ComfirmEmail();
            comfirmEmail.UserId = user.Id;
            comfirmEmail.Email = request.Email;
            comfirmEmail.Otp = $"SammiShop@{otp}";
            comfirmEmail.Exprited=DateTime.Now.AddMinutes(5);
            comfirmEmail.Status = false;
            dbContext.comfirmEmails.Add(comfirmEmail);
            await dbContext.SaveChangesAsync();

            return responseBase.ResponseBaseSucces("Đăng kí tài khoản thành công !");
        }

        public async Task<ResponseObject<DTO_User>> GetUserById(int UserId)
        {
            var user = await dbContext.users.FirstOrDefaultAsync(x => x.Id == UserId);
            if (user == null)
            {
                return responseObject.ResponseObjectError(StatusCodes.Status400BadRequest, "User không tồn tại !",null);
            }
            return responseObject.ResponseObjectSucces($"Lấy thông tin của {user.FullName} thành công !", converter_User.EntityToDTO(user));
        }

        public async Task<ResponseBase> ChangePassword(int UserId,Request_ChangePassWord request)
        {
            var user = await dbContext.users.FirstOrDefaultAsync (x => x.Id == UserId);
            if (user == null)
            {
                return responseBase.ResponseBaseError(StatusCodes.Status400BadRequest, "Tài khoản không tồn tại !");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.PassWordOld, user.PassWord))
            {
                return responseBase.ResponseBaseError(StatusCodes.Status400BadRequest, "Mặt khẩu cũ không đúng !");
            }
            if(request.PasswordComfirm!= request.PassWordNew)
            {
                return responseBase.ResponseBaseError(StatusCodes.Status400BadRequest, "Mặt khẩu xác nhận không trùng khớp !");
            }

            user.PassWord= BCrypt.Net.BCrypt.HashPassword(request.PasswordComfirm);
            dbContext.users.Update(user);
            await dbContext.SaveChangesAsync();
            return responseBase.ResponseBaseSucces("Đổi mật khẩu thành công !");
        }

        public async Task<ResponseBase> SendOtpForEmail(string email)
        {
            bool EmailInput = CheckInput.IsValiEmail(email);
            if (!EmailInput)
            {
                return responseBase.ResponseBaseError(StatusCodes.Status400BadRequest, "Email không hợp lệ !");
            }
            var checkemail= await dbContext.comfirmEmails.FirstOrDefaultAsync(x=>x.Email==email);
            if(checkemail==null)
            {
                return responseBase.ResponseBaseError(StatusCodes.Status400BadRequest, "Email này chưa đăng kí tài khoản !");

            }
            var emailTo = new EmailTo();
            emailTo.Subject = "MÃ XÁC NHẬN SAMMISHOP";
            emailTo.Mail = email;
            Random random = new Random();
            int otp = random.Next(1000, 9999);
            emailTo.Content = $"Đây là mãlấy lại mật khẩu của bạn, mã sẽ hết hạn sau 5 phút :\n SammiShop@{otp} ";
            await emailTo.SendEmailAsync(emailTo);
            checkemail.Otp = $"SammiShop@{otp}";
            checkemail.Exprited=DateTime.Now.AddMinutes(5);
            dbContext.comfirmEmails.Update(checkemail);
            await dbContext.SaveChangesAsync();
            return responseBase.ResponseBaseSucces("Gửi mã thành công !");

        }

        public async Task<ResponseBase> GetPassword(Request_GetPassword request)
        {
            var comfirmEmail = await dbContext.comfirmEmails.FirstOrDefaultAsync(x => x.Otp == request.otp);
            if(comfirmEmail==null)
            {
                return responseBase.ResponseBaseError(400, "Mã xác nhận không hợp lệ !");
            }
            if(request.passwordnew!= request.passwordnewComfirm)
            {
                return responseBase.ResponseBaseError(400, "Mật khẩu xác nhận không trùng khớp !");
            }
            string PassInput = CheckInput.IsPassWord(request.passwordnew);
            if (PassInput != request.passwordnew)
            {
                return responseBase.ResponseBaseError(StatusCodes.Status400BadRequest, PassInput);
            }
            var user = await dbContext.users.FirstOrDefaultAsync(x => x.Id == comfirmEmail.UserId);
            user.PassWord = BCrypt.Net.BCrypt.HashPassword(request.passwordnew);
            dbContext.users.Update(user);
            await dbContext.SaveChangesAsync();
            return responseBase.ResponseBaseSucces("Cập nhật mật khẩu thành công !");


        }

        public async Task<ResponseObject<DTO_User>> EditRoletUser(Request_EditRoleUser request)
        {
            var user = await dbContext.users.FirstOrDefaultAsync(x => x.Id == request.UserId);
            if(user==null)
            {
                return responseObject.ResponseObjectError(StatusCodes.Status404NotFound,"User Id không tồn tại !",null);
            }
            user.RoleId = request.RoleId;
            dbContext.users.Update(user) ;
            await dbContext.SaveChangesAsync();

            return responseObject.ResponseObjectSucces("Sửa quyền tài khoản thành công !",converter_User.EntityToDTO(user));

        }

        public async Task<IQueryable<DTO_User>> SearchUser(string Key, int pageSize, int pageNumber)
        {
            return await Task.FromResult(dbContext.users.OrderByDescending(x => x.Id).Where(x=>x.UserName.Contains(Key) || x.FullName.Contains(Key)|| x.PhoneNumber.Contains(Key) || x.Address.Contains(Key)  ) 
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(x => converter_User.EntityToDTO(x)));
        }

        public async Task<IQueryable<DTO_User>> GetFullListUser(int pageSize, int pageNumber)
        {
            return await Task.FromResult(dbContext.users.OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(x => converter_User.EntityToDTO(x)));
        }

        public async Task<IQueryable<DTO_Role>> GetFullListRole()
        {
            return await Task.FromResult(dbContext.roles
               .Select(x => new DTO_Role
               {
                   Id = x.Id,
                   NameRole = x.KeyRole,
               }));
        }
    }
}
