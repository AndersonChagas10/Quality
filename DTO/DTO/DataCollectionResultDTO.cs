using System;
using System.Runtime.Serialization;

namespace DTO.DTO
{
    [DataContract]
    [Serializable]
    public class DataCollectionResultDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int DataCollectionId { get; set; }
        [DataMember]
        public System.DateTime AddDate { get; set; }
        [DataMember]
        public Nullable<System.DateTime> AlterDate { get; set; }
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
    }
}