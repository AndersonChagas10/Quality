using DTO.BaseEntity;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace DTO.DTO
{
    public class CorrectiveActionDTO : EntityBase
    {
        public int idcorrectiveaction { get; set; }
        public string PeriodName { get; set; }
        public string ShiftName { get; set; }
        public string AuditorName { get; set; }
        public string level02Name { get; set; }
        public string level03Name { get; set; }
        public string level01Name { get; set; }
        public int Level01Id { get; set; }
        public int Level02Id { get; set; }
        public UnitDTO Unit { get; set; }

        public int AuditorId { get; set; }
        public int CollectionLevel02Id { get; set; }
        public Nullable<int> SlaughterId { get; set; }
        public Nullable<int> TechinicalId { get; set; }
        public Nullable<System.DateTime> DateTimeSlaughter { get; set; }
        public Nullable<System.DateTime> DateTimeTechinical { get; set; }
        public System.DateTime DateCorrectiveAction { get; set; }
        public System.DateTime AuditStartTime { get; set; }
        public string DescriptionFailure { get; set; }
        public string ImmediateCorrectiveAction { get; set; }
        public string ProductDisposition { get; set; }
        public string PreventativeMeasure { get; set; }

        public System.DateTime DateExecute { get; set; }
        public int ShiftId { get; set; }


        //public virtual UserSgq UserSgq { get; set; }
        //public virtual UserSgq UserSgq1 { get; set; }
        //public virtual UserSgq UserSgq2 { get; set; }
        //public virtual CollectionLevel02 CollectionLevel02 { get; set; }

        public System.DateTime StartTime { get; set; }
        public int PeriodId { get; set; }
        public string NameTechinical { get; set; }
        public string NameSlaughter { get; set; }
        public Nullable<int> AuditLevel01Id { get; set; }

        public List<CorrectiveActionLevelsDTO> CorrectiveActionLevels { get; set; }


        //public string DateExecuteFarmatado
        //{
        //    get { return DateExecute.ToString("MM/dd/yyyy hh:mm:ss"); }
        //    set
        //    {
        //        //DateExecute = DateTime.ParseExact(value.ToString(), "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
        //        DateExecute = Convert.ToDateTime(value.ToString(), CultureInfo.InvariantCulture);
        //    }
        //}
        public string DateCorrectiveActionFormatado
        {
            get { return DateCorrectiveAction.ToString("MM/dd/yyyy hh:mm:ss"); }
            set
            {
                // StartTime = DateTime.ParseExact(value.ToString(), "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                DateCorrectiveAction = Convert.ToDateTime(value.ToString(), CultureInfo.InvariantCulture);
            }
        }

        public string DateCorrectiveActionFormatadoShort
        {
            get { return DateCorrectiveAction.ToString("MM/dd/yyyy"); }
            set
            {
                // StartTime = DateTime.ParseExact(value.ToString(), "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                DateCorrectiveAction = Convert.ToDateTime(value.ToString(), CultureInfo.InvariantCulture);
            }
        }

        public string StartTimeFormatado
        {
            get { return AuditStartTime.ToString("MM/dd/yyyy hh:mm:ss"); }
            set
            {
                // StartTime = DateTime.ParseExact(value.ToString(), "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                AuditStartTime = Convert.ToDateTime(value.ToString(), CultureInfo.InvariantCulture);
            }
        }

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

        public string NameSlaugtherAndDate
        {
            get { return NameSlaughter != null ? NameSlaughter + " (" + DateTimeSlaughter.Value.ToString("MM/dd/yyyy hh:mm:ss") + ")" : string.Empty; }
        }

        public string NameTechinicalAndDate
        {
            get { return NameTechinical != null ? NameTechinical + " (" + DateTimeTechinical.Value.ToString("MM/dd/yyyy hh:mm:ss") + ")" : string.Empty; }
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


        public string SendMeByMail
        {
            get
            {

                if (Id <= 0)
                    return string.Empty;

                var body = "<div class='header' style='font-size:14px;font-weight:bold'>" +
                "    <div class='unitCode'>JBS <span id='estName'>" + Unit.Name + "</span> Beef, Est. <span id='estCode'>" + Unit.Code + "</span></div>" +
                "    <div>Safe Program Audit Form</div>" +
                "    <div>Corrective Action</div>" +
                "</div>" +
                "<div style='border: 1px solid #ddd; padding: 8px; width:100%;height:auto; margin-top: 8px;'>" +
                "    <div class='row'>" +
                "        <div class='col-xs-6' id='CorrectiveActionTaken'>" +
                "            <b class='font16'>Corrective Action Taken:<br></b>" +
                "            <b>Date/Time:</b> <span id='datetime'>" + DateCorrectiveActionFormatado + "</span><br>" +
                "            <b>Auditor: </b><span id='auditor'>" + AuditorName + "</span><br>" +
                "            <b>Shift: </b><span id='shift'>" + ShiftName + "</span><br>" +
                "        </div>" +
                "        <div class='col-xs-6' id='AuditInformation'>" +
                "            <b class='font16'>Audit Information:<br></b>" +
                "            <b>Audit Audit: </b><span id='auditText'>" + level01Name + "</span><br>" +
                "            <b>Start Time: </b><span id='starttime'>" + StartTimeFormatado + "</span><br>" +
                "            <b>Period: </b><span id='correctivePeriod'>" + PeriodName + "</span>" +
                "        </div>" +
                "    </div>" +
                "</div>" +
                "<div>" +
                "    <div style='border: 1px solid #ddd; padding: 8px; width:100%;height:auto;margin-top: 8px;'>" +
                "        <h4 style='margin-top:0'>Description Failure</h4>" +
                "        <div id='DescriptionFailure'>" +
                "         " + DescriptionFailure +
                "        </div>" +
                "    </div>" +
                "    <div style='border: 1px solid #ddd; padding: 8px; width:100%;height:auto; margin-top: 8px;'>" +
                "        <h4 style='margin-top:0'>Immediate Corrective Action</h4>" +
                "        <div id='ImmediateCorrectiveAction'>" + ImmediateCorrectiveAction + "</div>" +
                "    </div>" +
                "    <div style='border: 1px solid #ddd; padding: 8px; width:100%;height:auto; margin-top: 8px;'>" +
                "        <h4 style='margin-top:0'>Product Disposition</h4>" +
                "        <div id='ProductDisposition'>" + ProductDisposition + "</div>" +
                "    </div>" +
                "    <div style='border: 1px solid #ddd; padding: 8px; width:100%;height:auto; margin-top: 8px;'>" +
                "        <h4 style='margin-top:0'>Preventative Measure</h4>" +
                "        <div id='PreventativeMeasure'>" + PreventativeMeasure + "</div>" +
                "    </div>" +
                "    <div style='border: 1px solid #ddd; padding: 8px; width:100%;height:auto; margin-top: 8px; height: 90px;margin-bottom:15px;'>" +
                "        <div class='SlaugtherSignature ' style='width:50%; float: left;' id='Slaugther' userid=''>" +
                "            <h5 style='margin-top:0;font-weight:bold'>Slaughter signature</h5>" +
                "            <div class='name'>" + NameSlaughter + "</div>" +
                "            <div class='date'>" + DateTimeSlaughterFarmatado + "</div>" +
                "        </div>" +
                "        <div class='TechnicaSignature' style='width:49%; float: right;' id='Technical' userid=''>" +
                "            <h5 style='margin-top:0;font-weight:bold'>Technical signature</h5>" +
                "            <div class='name'>" + NameTechinical + "</div>" +
                "            <div class='date'>" + DateTimeTechinicalFarmatado + "</div>" +
                "        </div>" +
                "    </div>" +
                "</div>"; /*+
                "    <div class='col-xs-4'>" +
                "        <div>Origination Date: January 30, 2014</div> <!--O que fazer com a Data?-->" +
                "<br>" +
                "        <div>Revision Date:</div>" +
                "<br>" +
                "        <div>Supersedes Date:</div>" +
                "    </div>" +
                "    <div class='col-xs-4'>" +
                "<br>" +
                "        <div>This Document Contains Confidential</div>" +
                "<br>" +
                "        <div>Commercial Information Pursuant to</div>" +
                "<br>" +
                "        <div>5 U.S.C Sec. 552(b)(4).</div>" +
                "    </div>";*/

                return body;
            }
        }

    }
}
