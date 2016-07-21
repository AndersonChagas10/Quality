using System;
using System.Runtime.Serialization;

namespace DTO.DTO
{
    [DataContract]
    [Serializable]
    public class Level03ConsolidationDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Level02ConsolidationId { get; set; }
        [DataMember]
        public int Level03Id { get; set; }
        [DataMember]
        public System.DateTime DateConsolidation { get; set; }
        [DataMember]
        public System.DateTime AddDate { get; set; }
        [DataMember]
        public Nullable<System.DateTime> AlterDate { get; set; }
        [DataMember]
        public decimal TotalLevel03 { get; set; }
        [DataMember]
        public decimal TotalLevel03Weight { get; set; }
        [DataMember]
        public decimal TotalEvaluated { get; set; }
        [DataMember]
        public decimal totalEvaluatedWeight { get; set; }
        [DataMember]
        public decimal TotalEvaluatedShared { get; set; }
        [DataMember]
        public decimal TotalEvaluatedSharedWeight { get; set; }
        [DataMember]
        public decimal TotalNotConform { get; set; }
        [DataMember]
        public decimal TotalNotConformWeight { get; set; }
        [DataMember]
        public decimal TotalNotConformShared { get; set; }
        [DataMember]
        public decimal TotalNotConformSharedWeight { get; set; }
        [DataMember]
        public bool Shared { get; set; }

        [DataMember]
        public Level02ConsolidationDTO Level02Consolidation { get; set; }
        [DataMember]
        public Level03DTO Level03 { get; set; }
    }
}