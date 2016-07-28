using DTO.Helpers;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace DTO.BaseEntity
{
    public class EntityBase
    {
        [DataMember]
        public int Id { get; set; } = 0;
        [DataMember]
        public DateTime AddDate { get; set; } = DateTime.Now;
        [DataMember]
        public DateTime? AlterDate { get; set; } = null;

        public void ValidaBaseEntity([CallerMemberName] string callerName = "")
        {
            #region Id: Não pode ser negativo
            Guard.ForValidId(Id, callerName);
            #endregion
        }
    }
}
