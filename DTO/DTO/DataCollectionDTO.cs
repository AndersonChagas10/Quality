using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DTO.DTO
{
    [DataContract]
    [Serializable]
    public class DataCollectionDTO
    {
        [DataMember]
        public int Id { get; set; }
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
        public System.DateTime AddData { get; set; }
        [DataMember]
        public Nullable<System.DateTime> AlterDate { get; set; }
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
        public Level02ConsolidationDTO Level02Consolidation { get; set; }
        [DataMember]
        public List<DataCollectionResultDTO> DataCollectionResult { get; set; }
    }
}
