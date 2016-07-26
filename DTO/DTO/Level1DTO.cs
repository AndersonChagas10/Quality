using DTO.BaseEntity;
using DTO.Helpers;

namespace DTO.DTO
{
    public class Level1DTO : EntityBase
    {
        public string Name { get; set; }

        public void ValidaLevel1DTO()
        {
            ValidaBaseEntity();

            #region Name
            string NameValue;
            Guard.NullOrEmptyValuesCheck(retorno: out NameValue, propName: "Name", value: Name, requerido: true);
            Name = NameValue;
            #endregion
        }
    }
}
