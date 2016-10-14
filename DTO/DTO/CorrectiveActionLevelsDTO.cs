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
        public int Level01Id { get; set; }
        [DataMember]
        public int Level02Id { get; set; }
        [DataMember]
        public int Level03Id { get; set; }
        [DataMember]
        public Nullable<int> Defects { get; set; }
        //[DataMember]
        //public CorrectiveActionDTO CorrectiveAction { get; set; }

        public void ValidaCorrectiveActionLevels()
        {
            ValidaBaseEntity();



        }
    }
}
