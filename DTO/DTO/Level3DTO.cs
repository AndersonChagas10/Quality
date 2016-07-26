using DTO.BaseEntity;
using DTO.Helpers;

namespace DTO.DTO
{
    public class Level3DTO : EntityBase
    {
        public string Name { get; set; }
        public void ValidaLeve3DTO(bool isAlter)
        {

            #region Name
            string NameValue;
            Guard.NullOrEmptyValuesCheck(retorno: out NameValue, propName: "Name", value: Name, requerido: true);
            Name = NameValue;
            #endregion
        }
    }
}
