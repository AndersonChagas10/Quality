using DTO.BaseEntity;
using DTO.Helpers;
using System;
using System.Runtime.Serialization;

namespace DTO.DTO
{
    [DataContract]
    [Serializable]
    public class Level03ConsolidationDTO : EntityBase
    {
        [DataMember]
        public int Level02ConsolidationId { get; set; }
        [DataMember]
        public int Level03Id { get; set; }
        [DataMember]
        public System.DateTime DateConsolidation { get; set; }
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

        public void ValidaLevel03ConsolidationDTO()
        {
            ValidaBaseEntity();

            #region Level02ConsolidationId: (FK) Não pode ser Zero, não pode ser negativo.
            Guard.ForNegative(Level02ConsolidationId, "Level02ConsolidationId");
            Guard.forValueZero(Level02ConsolidationId, "Level02ConsolidationId");
            #endregion

            #region Level03Id: (FK) Não pode ser Zero, não pode ser negativo.
            Guard.ForNegative(Level03Id, "Level03Id");
            Guard.forValueZero(Level03Id, "Level03Id");
            #endregion

        }
    }
}