using DTO.BaseEntity;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace DTO.DTO
{
    public class CollectionLevel02DTO : EntityBase
    {

        public CollectionLevel02DTO() { }

        public CollectionLevel02DTO(NextRoot nextRoot, int unitId)
        {
            if (nextRoot.id != null)
                if (nextRoot.id.Length > 0)
                    Id = int.Parse(nextRoot.id);

            if (nextRoot.remove != null)
                if (nextRoot.remove.Length > 0)
                    Remove = bool.Parse(nextRoot.remove);
            Guard.VerifyIfIsBool(Remove, "problema no remove.");

            try
            {
                ValidaBaseEntity();

                //FK
                //Auditor_Id = int.Parse(nextRoot.auditorid);
                //Guard.ForValidFk(Auditor_Id, "Auditor_Id  is not valid.");

                #region Caso for HTP

                if (nextRoot.level01id.Equals("1"))
                {
                }

                #endregion

                #region Caso for CFF

                if (nextRoot.level01id.Equals("3"))
                {
                }

                #endregion

                #region Caso for  CCA

                if (nextRoot.level01id.Equals("2"))
                {
                    CattleTypeId = int.Parse(nextRoot.cattletype);
                    Guard.ForValidFk(CattleTypeId, "CattleType Id is not valid.");

                    Chainspeed = decimal.Parse(nextRoot.chainspeed);
                    Guard.ForNegative(Chainspeed, "Chainspeed");
                    //ConsecutiveFailureIs = bool.Parse(nextRoot.Con)

                    LotNumber = decimal.Parse(nextRoot.lotnumber);
                    Guard.ForNegative(LotNumber, "LotNumber");

                    Mudscore = decimal.Parse(nextRoot.mudscore);
                    Guard.ForNegative(Mudscore, "Mudscore");

                    if (nextRoot.consecutivefailuretotal != null)
                        ConsecutiveFailureTotal = int.Parse(nextRoot.consecutivefailuretotal);
                    Guard.ForNegative(ConsecutiveFailureTotal, "ConsecutiveFailureTotal");
                }

                #endregion

                #region Valores Default Para Todos

                Level02Id = int.Parse(nextRoot.level02id);
                Guard.ForValidFk(Level02Id, "Level02 Id is not valid.");

                UnitId = unitId;
                Guard.ForValidFk(UnitId, "Unit id is not valid.");

                Period = int.Parse(nextRoot.period);
                Guard.ForNegative(Period, "Period");

                if (nextRoot.phase.IsNotNull())
                {
                    Phase = int.Parse(nextRoot.phase);
                    Guard.ForNegative(Phase, "Phase number");
                }

                ReauditIs = bool.Parse(nextRoot.reaudit);
                Guard.VerifyIfIsBool(ReauditIs, "ReauditIs");

                ReauditNumber = int.Parse(nextRoot.reauditnumber);
                Guard.ForNegative(ReauditNumber, "ReauditNumber");

                NotEvaluatedIs = bool.Parse(nextRoot.notavaliable);
                Guard.VerifyIfIsBool(NotEvaluatedIs, "NotEvaluatedIs");

                //nextRoot.sample = "1";
                if (nextRoot.sample != null)
                    Sample = int.Parse(nextRoot.sample);
                else
                    Sample = 1;
                Guard.ForNegative(Sample, "Sample");

                Shift = int.Parse(nextRoot.shift);
                Guard.ForNegative(Shift, "Shift");

                if (Phase > 1)
                    StartPhaseDate = DateTime.Parse(nextRoot.startphasedate);

                #endregion

                if (nextRoot.idcorrectiveaction != null)
                    CorrectiveActionId = int.Parse(nextRoot.idcorrectiveaction);
               

            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar CollectionLevel02DTO" + e.Message, e);
            }

            #region Cria Level03 Collection

            if (nextRoot.nextnextRoot == null)
                throw new Exception("Lista de CollectionLevel03DTO vazia.");

            if (nextRoot.nextnextRoot.Count == 0)
                throw new Exception("Lista de CollectionLevel03DTO vazia.");

            collectionLevel03DTO = new List<CollectionLevel03DTO>();
            foreach (var x in nextRoot.nextnextRoot)
                collectionLevel03DTO.Add(new CollectionLevel03DTO(x, Level01Id, Level02Id));

            //MOCK
            AuditorId = 1;
            #endregion
            //08 / 30 / 2016 10:38
            if(nextRoot.datetime != null)
                CollectionDate = DateTime.ParseExact(nextRoot.datetime, "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);

            if (nextRoot.evaluate != null)
                EvaluationNumber = int.Parse(nextRoot.evaluate);
            else
                EvaluationNumber = 1;
            
        }

        public int EvaluationNumber { get; set; }
        public int CorrectiveActionId { get; set; }
        public int ConsolidationLevel02Id { get; set; }
        public int AuditorId { get; set; }
        public int CattleTypeId { get; set; }
        public int Level01Id { get; set; }
        public int Level02Id { get; set; }
        public int UnitId { get; set; }
        public decimal Chainspeed { get; set; }
        public bool ConsecutiveFailureIs { get; set; }
        public int ConsecutiveFailureTotal { get; set; }
        public decimal LotNumber { get; set; }
        public decimal Mudscore { get; set; }
        public int Period { get; set; }
        public int Phase { get; set; }
        public bool ReauditIs { get; set; }
        public int ReauditNumber { get; set; }
        public bool NotEvaluatedIs { get; set; }
        public int Sample { get; set; }
        public int Shift { get; set; }
        public System.DateTime StartPhaseDate { get; set; }
        public System.DateTime CollectionDate { get; set; }

        public List<CollectionLevel03DTO> collectionLevel03DTO { get; set; }
        public ConsolidationLevel01DTO consolidationLevel01DTO { get; set; }
        public bool Remove { get; set; }
    }
}
