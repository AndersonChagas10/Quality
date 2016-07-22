using System;
using System.Runtime.Serialization;

namespace DTO.DTO
{
    [DataContract]
    [Serializable]
    public class CorrectiveActionDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public System.DateTime DateExecute { get; set; }
        [DataMember]
        public int Auditor { get; set; }
        [DataMember]
        public int Shift { get; set; }
        [DataMember]
        public int AuditLevel1 { get; set; }
        [DataMember]
        public int AuditLevel2 { get; set; }
        [DataMember]
        public int AuditLevel3 { get; set; }
        [DataMember]
        public System.DateTime StartTime { get; set; }
        [DataMember]
        public int Period { get; set; }
        [DataMember]
        public string DescriptionFailure { get; set; }
        [DataMember]
        public string ImmediateCorrectiveAction { get; set; }
        [DataMember]
        public string ProductDisposition { get; set; }
        [DataMember]
        public string PreventativeMeasure { get; set; }
        [DataMember]
        public Nullable<int> Slaughter { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DateTimeSlaughter { get; set; }
        [DataMember]
        public Nullable<int> Techinical { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DateTimeTechinical { get; set; }
    }
}
