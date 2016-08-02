using DTO.BaseEntity;
using System;
using System.Runtime.Serialization;

namespace DTO.DTO
{
    [DataContract]
    [Serializable]
    public class CorrectiveActionLevelsDTO : EntityBase
    {
        [DataMember]
        public int CorrectiveActionId { get; set; }
        [DataMember]
        public int AuditLevel01Id { get; set; }
        [DataMember]
        public int AuditLevel02Id { get; set; }
        [DataMember]
        public int AuditLevel03Id { get; set; }
        [DataMember]
        public Nullable<int> Defects { get; set; }
        //[DataMember]
        //public CorrectiveActionDTO CorrectiveAction { get; set; }

        public void ValidaCoccectiveAction()
        {
            ValidaBaseEntity();



        }
    }
}
