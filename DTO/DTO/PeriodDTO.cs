using DTO.BaseEntity;
using DTO.Helpers;
using System.Runtime.Serialization;

namespace DTO.DTO
{
    public class PeriodDTO : EntityBase
    {
        [DataMember]
        public string Description { get; set; }
        public string Name { get { return "Periodo" + Description; } }
        public void ValidaPeriodDTO()
        {
            ValidaBaseEntity();

            #region Description
            string DescValidated;
            Guard.CheckStringFull(retorno: out DescValidated, propName: "Description", requerido: true, value: Description);
            Description = DescValidated;
            #endregion
        }
    }
}
