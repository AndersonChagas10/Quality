using DTO.BaseEntity;
using DTO.Helpers;

namespace DTO.DTO
{
    public class Level2DTO : EntityBase
    {
        public string Name { get; set; }

        public void ValidaLevel2DTO(bool isAlter)
        {

            #region Name
            string NameValue;
            Guard.NullOrEmptyValuesCheck(retorno: out NameValue, propName: "Name", value: Name, requerido: true);
            Name = NameValue;
            #endregion

        }
    }
}
