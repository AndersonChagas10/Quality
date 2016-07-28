using DTO.BaseEntity;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DTO.DTO
{
    [DataContract]
    [Serializable]
    public class DataCollectionDTO : EntityBase
    {
        [DataMember]
        public int Level02ConsolidationId { get; set; }
        [DataMember]
        public int EvaluationNumber { get; set; }
        [DataMember]
        public int SampleNumber { get; set; }
        [DataMember]
        public int MonitorId { get; set; }
        [DataMember]
        public System.DateTime CollectionDate { get; set; }
        [DataMember]
        public Nullable<int> ProductId { get; set; }
        [DataMember]
        public Nullable<int> Access { get; set; }
        [DataMember]
        public Nullable<int> Sets { get; set; }
        [DataMember]
        public Nullable<int> Side { get; set; }
        [DataMember]
        public bool Shared { get; set; }
        [DataMember]
        public int Plataform { get; set; }
        [DataMember]
        public bool Processed { get; set; }
        [DataMember]
        public int Control { get; set; }

        [DataMember]
        public Level02ConsolidationDTO Level02Consolidation { get; set; }
        [DataMember]
        public List<DataCollectionResultDTO> DataCollectionResult { get; set; }

        public void ConsolidationDataCollectionDTO()
        {

        }

        public void ValidaDataCollectionDTO()
        {

            ValidaBaseEntity();

            #region Level02ConsolidationId: (FK)
            //Guard.ForNegative(Level02ConsolidationId, "Level02ConsolidationId");
            //Guard.forValueZero(Level02ConsolidationId, "Level02ConsolidationId");
            #endregion

            #region EvaluationNumber: Não pode ser negativo.
            Guard.ForNegative(EvaluationNumber, "EvaluationNumber");
            #endregion

            #region SampleNumber:  Não pode ser negativo. NAO IMPLEMENTADO
            //Guard.ForNegative(SampleNumber, "SampleNumber");
            #endregion

            #region MonitorId: Não pdoe ser Zero, não pode ser negativo.
            Guard.ForNegative(MonitorId, "Monitor Identification");
            Guard.forValueZero(MonitorId, "Monitor Identification");
            #endregion

            #region CollectionDate:  Se nulo, utiliza data de hoje.
            if (CollectionDate.IsNull() || CollectionDate == DateTime.MinValue)
                CollectionDate = DateTime.Now;
            #endregion

            #region ProductId: Se existir, não pode ser negativo. NAO IMPLEMENTADO
            //if(ProductId.IsNotNull())
            //    Guard.ForNegative(ProductId.Value, "ProductId");
            #endregion

            #region Access:
            #endregion

            #region Sets: Se existir, não pode ser negativo.  NAO IMPLEMENTADO
            //if (Sets.IsNotNull())
            //    Guard.ForNegative(Sets.Value, "Sets");
            #endregion

            #region Side: Se existir, não pode ser negativo.  NAO IMPLEMENTADO
            //if (Side.IsNotNull())
            //    Guard.ForNegative(Side.Value, "Side");
            #endregion

            #region Shared:  NAO IMPLEMENTADO
            #endregion

            #region Plataform:  NAO IMPLEMENTADO
            #endregion

            #region Processed:  NAO IMPLEMENTADO
            #endregion

        }

    }
}
