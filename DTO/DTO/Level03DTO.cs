using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DTO.DTO
{
    [DataContract]
    [Serializable]
    public class Level03DTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Alias { get; set; }
        [DataMember]
        public string ClientSpecification { get; set; }
        [DataMember]
        public System.DateTime AddedDate { get; set; }
        [DataMember]
        public Nullable<System.DateTime> AlterDate { get; set; }
        [DataMember]
        public bool Active { get; set; }
        [DataMember]
        public int PeopleCreateId { get; set; }
        [DataMember]
        public int Ordering { get; set; }

        [DataMember]
        public List<DataCollectionResultDTO> DataCollectionResult { get; set; }
        [DataMember]
        public List<Level03ConsolidationDTO> Level03Consolidation { get; set; }
    }
}