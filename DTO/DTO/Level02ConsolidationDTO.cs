using DTO.BaseEntity;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DTO.DTO
{
    [DataContract]
    [Serializable]
    public class Level02ConsolidationDTO : EntityBase
    {
        [DataMember]
        public int Level02Id { get; set; }
        [DataMember]
        public int Level01ConsolidationId { get; set; }
        [DataMember]
        public System.DateTime DateConsolidation { get; set; }
        [DataMember]
        public decimal TotalLevel03 { get; set; }
        [DataMember]
        public decimal TotalLevel03Weight { get; set; }
        [DataMember]
        public decimal TotalEvaluated { get; set; }
        [DataMember]
        public decimal TotalEvaluatedWeight { get; set; }
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
        public List<DataCollectionDTO> DataCollection { get; set; }
        [DataMember]
        public Level01ConsolidationDTO Level01Consolidation { get; set; }
        [DataMember]
        public Level02DTO Level02 { get; set; }
        [DataMember]
        public List<Level03ConsolidationDTO> Level03Consolidation { get; set; }

        public void ValidaLevel02ConsolidationDTO(bool isAlter = false)
        {
            ValidaBaseEntity(isAlter);
        }

    }
}