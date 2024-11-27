using BE_ThuyDuong.Entities;
using BE_ThuyDuong.PayLoad.DTO;

namespace BE_ThuyDuong.PayLoad.Converter
{
    public class Converter_Trademark
    {
        public DTO_Trademark EntityToDTO(Trademark trademark)
        {
            return new DTO_Trademark
            {
                Id = trademark.Id,
                TradamarkName=trademark.TradamarkName,
                Address=trademark.Address,
            };
        }
    }
}
