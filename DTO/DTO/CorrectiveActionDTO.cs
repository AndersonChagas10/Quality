using DTO.BaseEntity;
using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace DTO.DTO
{
    [DataContract]
    [Serializable]
    public class CorrectiveActionDTO : EntityBase
    {
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

        /// <summary>
        /// Valida Entidade CorrectiveActionDTO para integridade do Banco de dados.
        /// </summary>
        /// <param name="isAlter"></param>
        public void ValidaDataCorrectiveActionDTO()
        {

            ValidaBaseEntity();

            #region DateExecute
            #endregion

            #region Auditor
            #endregion

            #region Shift
            #endregion

            #region AuditLevel1
            #endregion
            
            #region AuditLevel2
            #endregion
            
            #region AuditLevel3
            #endregion
            
            #region StartTime
            #endregion
            
            #region Period
            #endregion
            
            #region DescriptionFailure
            #endregion
            
            #region ImmediateCorrectiveAction
            #endregion
            
            #region ProductDisposition
            #endregion

            #region PreventativeMeasure
            #endregion

            #region Slaughter
            #endregion

            #region DateTimeSlaughter
            #endregion

            #region Techinical
            #endregion

            #region DateTimeTechinical
            #endregion

            #region NameTechinical
            #endregion

            #region NameSlaughter
            #endregion

        }

    }
}
