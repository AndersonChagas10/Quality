using DTO.BaseEntity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace DTO.DTO
{
    [DataContract]
    [Serializable]
    public class CorrectiveActionDTO : EntityBase
    {
        public int AuditorId { get; set; }
        public string DescriptionFailure { get; set; }
        public string ImmediateCorrectiveAction { get; set; }
        public string ProductDisposition { get; set; }
        public string PreventativeMeasure { get; set; }
        public Nullable<int> SlaughterId { get; set; }
        public Nullable<System.DateTime> DateTimeSlaughter { get; set; }
        public Nullable<int> TechinicalId { get; set; }
        public Nullable<System.DateTime> DateTimeTechinical { get; set; }
        public System.DateTime AuditStartTime { get; set; }
        public System.DateTime DateCorrectiveAction { get; set; }
        public Nullable<int> CollectionLevel03Id { get; set; }

        public CollectionLevel03DTO CollectionLevel03 { get; set; }
        public UserDTO UserSgq { get; set; }
        public UserDTO UserSgq1 { get; set; }
        public UserDTO UserSgq2 { get; set; }
        public List<CorrectiveActionLevelsDTO> CorrectiveActionLevels { get; set; }


        //[DataMember]
        //public string DateExecuteFarmatado
        //{
        //    get { return DateExecute.ToString("MM/dd/yyyy hh:mm:ss"); }
        //    set
        //    {
        //        //DateExecute = DateTime.ParseExact(value.ToString(), "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
        //        DateExecute = Convert.ToDateTime(value.ToString(), CultureInfo.InvariantCulture);
        //    }
        //}
        //[DataMember]
        //public string StartTimeFarmatado
        //{
        //    get { return StartTime.ToString("MM/dd/yyyy hh:mm:ss"); }
        //    set
        //    {
        //        // StartTime = DateTime.ParseExact(value.ToString(), "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
        //        StartTime = Convert.ToDateTime(value.ToString(), CultureInfo.InvariantCulture);
        //    }
        //}
        [DataMember]
        public string DateTimeSlaughterFarmatado
        {
            get { return DateTimeSlaughter != null ? DateTimeSlaughter.Value.ToString("MM/dd/yyyy hh:mm:ss") : string.Empty; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    //DateTimeSlaughter = DateTime.ParseExact(value.ToString(), "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                    DateTimeSlaughter = Convert.ToDateTime(value.ToString(), CultureInfo.InvariantCulture);
                }
            }
        }
        [DataMember]
        public string DateTimeTechinicalFarmatado
        {
            get { return DateTimeTechinical != null ? DateTimeTechinical.Value.ToString("MM/dd/yyyy hh:mm:ss") : string.Empty; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    //DateTimeTechinical = DateTime.ParseExact(value.ToString(), "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                    DateTimeTechinical = Convert.ToDateTime(value.ToString(), CultureInfo.InvariantCulture);
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

            #region Auditor (FK) 
            #endregion

            #region Shift (FK)
            #endregion

            #region AuditLevel1 (FK) 
            #endregion

            #region AuditLevel2 (FK) 
            #endregion

            #region AuditLevel3 (FK) 
            #endregion

            #region StartTime
            #endregion

            #region Period (FK)
            #endregion

            #region DescriptionFailure
            #endregion

            #region ImmediateCorrectiveAction
            #endregion

            #region ProductDisposition
            #endregion

            #region PreventativeMeasure
            #endregion

            #region Slaughter (FK) 
            #endregion

            #region DateTimeSlaughter
            #endregion

            #region Techinical (FK) 
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
