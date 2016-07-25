using System;
using System.Globalization;
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
        [DataMember]
        public string NameTechinical { get; set; }
        [DataMember]
        public string NameSlaughter { get; set; }


        [DataMember]
        public string DateExecuteFarmatado
        {
            get { return DateExecute.ToString("dd/MM/yyyy hh:mm:ss"); }
            set
            {
                DateExecute = DateTime.ParseExact(value.ToString(), "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
            }
        }
        [DataMember]
        public string StartTimeFarmatado
        {
            get { return StartTime.ToString("dd/MM/yyyy hh:mm:ss"); }
            set
            {
                StartTime = DateTime.ParseExact(value.ToString(), "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
            }
        }
        [DataMember]
        public string DateTimeSlaughterFarmatado
        {
            get { return DateTimeSlaughter != null ? DateTimeSlaughter.Value.ToString("dd/MM/yyyy hh:mm:ss") : string.Empty; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    DateTimeSlaughter = DateTime.ParseExact(value.ToString(), "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                }
            }
        }
        [DataMember]
        public string DateTimeTechinicalFarmatado
        {
            get { return DateTimeTechinical != null ? DateTimeTechinical.Value.ToString("dd/MM/yyyy hh:mm:ss") : string.Empty; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    DateTimeTechinical = DateTime.ParseExact(value.ToString(), "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                }
            }
        }
    }
}
