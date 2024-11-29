using BE_ThuyDuong.Entities;
using BE_ThuyDuong.PayLoad.DTO;

namespace BE_ThuyDuong.PayLoad.Converter
{
    public class Converter_ProductType
    {
        public DTO_ProductType EntityToDTO(ProductType productType)
        {
            return new DTO_ProductType
            {
                Id = productType.Id,
                TypeName=productType.TypeName,
            };
        }
    }
}
