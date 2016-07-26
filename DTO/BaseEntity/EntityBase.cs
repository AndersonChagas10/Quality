using DTO.Helpers;
using System;
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

        public void ValidaBaseEntity(bool isAlter)
        {
            #region Id: Não pode ser negativo, se alteração não pdoe ser Zero.
            Guard.ForValidId(Id, "Primary Key", "LEvel01", isAlter);
            #endregion

            #region AddData: Se nulo utilizar a data atual.
            Guard.AutoFillDateWithDateNow(AddDate);
            #endregion

            #region AlterDate: Se for alteração, e for nulo, utilizar a data atual.
            if (isAlter)
                Guard.AutoFillDateWithDateNow(AlterDate);
            #endregion 
        }
    }
}
