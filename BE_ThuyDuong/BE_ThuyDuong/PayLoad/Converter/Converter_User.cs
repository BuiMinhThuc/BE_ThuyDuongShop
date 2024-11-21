using BE_ThuyDuong.Entities;
using BE_ThuyDuong.PayLoad.DTO;

namespace BE_ThuyDuong.PayLoad.Converter
{
    public class Converter_User
    {
        public DTO_User EntityToDTO(User user)
            => new DTO_User
            {
                UserName = user.UserName,
                PassWord = user.PassWord,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RoleId = user.RoleId,
                UrlAvt = user.UrlAvt,
            };
    }
}
