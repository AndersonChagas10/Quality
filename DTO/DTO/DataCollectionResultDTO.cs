using DTO.BaseEntity;
using DTO.Helpers;
using System;
using System.Runtime.Serialization;

namespace DTO.DTO
{
    [DataContract]
    [Serializable]
    public class DataCollectionResultDTO : EntityBase
    {
        [DataMember]
        public int DataCollectionId { get; set; }
        [DataMember]
        public Nullable<int> Level03Id { get; set; }
        [DataMember]
        public int TotalEvaluated { get; set; }
        [DataMember]
        public bool Conformed { get; set; }
        [DataMember]
        public bool Repeated { get; set; }
        [DataMember]
        public decimal DataCollectionValue { get; set; }
        [DataMember]
        public string DataCollectionValueText { get; set; }
        [DataMember]
        public decimal Weight { get; set; }

        [DataMember]
        public DataCollectionDTO DataCollection { get; set; }
        [DataMember]
        public Level03DTO Level03 { get; set; }

        public void ValidaDataCollectionResultDTO()
        {

            ValidaBaseEntity();

            //#region DataCollectionId: (FK) Não pdoe ser Zero, não pode ser negativo.
            //Guard.ForNegative(DataCollectionId, "DataCollectionId");
            //Guard.forValueZero(DataCollectionId, "DataCollectionId");
            //#endregion

            //#region Level03Id: (FK) Se existir, não pdoe ser Zero, não pode ser negativo.
            //if (Level03Id.IsNotNull())
            //{
            //    Guard.ForNegative(Level03Id.Value, "Level03Id");
            //    Guard.forValueZero(Level03Id.Value, "Level03Id");
            //}
            //#endregion

            //#region TotalEvaluated: Não pode ser negativo.
            //Guard.ForNegative(TotalEvaluated, "TotalEvaluated");
            //#endregion

            //#region Repeated

            //#endregion

            //#region DataCollectionValue

            //#endregion

            //#region DataCollectionValueText

            //#endregion

            //#region Weight: Não pode ser negativo.
            //Guard.ForNegative(TotalEvaluated, "TotalEvaluated");
            //#endregion


        }

    }
}