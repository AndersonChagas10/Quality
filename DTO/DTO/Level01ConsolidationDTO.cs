using DTO.BaseEntity;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DTO.DTO
{
    [DataContract]
    [Serializable]
    public class Level01ConsolidationDTO : EntityBase
    {
        [DataMember]
        public int CenterResultId { get; set; }
        [DataMember]
        public int Level01Id { get; set; }
        [DataMember]
        public System.DateTime DateConsolidation { get; set; } = DateTime.Now;
        [DataMember]
        public int TotalLevel02 { get; set; }
        [DataMember]
        public decimal TotalLevel02Weight { get; set; }
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
        public decimal TotalNotConformShared_Weight { get; set; }
        [DataMember]
        public bool Shared { get; set; }

        [DataMember]
        public Level01DTO Level01 { get; set; }
        [DataMember]
        public List<Level02ConsolidationDTO> Level02Consolidation { get; set; }

        public void ValidaLevel01ConsolidationDTO()
        {

            ValidaBaseEntity();

            #region CenterResultId (FK) 
            #endregion

            #region Level01Id: (FK) 
            #endregion

            #region DateConsolidation 
            #endregion

            #region TotalLevel02 
            #endregion

            #region TotalLevel02Weight 
            #endregion 

            #region TotalEvaluated 
            #endregion 

            #region TotalEvaluatedWeight 
            #endregion 

            #region TotalEvaluatedShared 
            #endregion 

            #region TotalEvaluatedSharedWeight 
            #endregion 

            #region TotalNotConform 
            #endregion 

            #region TotalNotConformWeight 
            #endregion 

            #region TotalNotConformShared 
            #endregion 

            #region TotalNotConformShared_Weight 
            #endregion 

            #region Shared
            #endregion 
        }

    }
}