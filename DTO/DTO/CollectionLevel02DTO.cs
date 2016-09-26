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
                    Id = Guard.ConverteValor<int>(nextRoot.id, "Level02.id");//int.Parse(nextRoot.id);
            ValidaBaseEntity();


            if (nextRoot.remove != null)
                if (nextRoot.remove.Length > 0)
                    Remove = Guard.ConverteValor<bool>(nextRoot.remove, "Level02.remove");//bool.Parse(nextRoot.remove);
            Guard.VerifyIfIsBool(Remove, "problema no remove.");


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
                CattleTypeId = Guard.ConverteValor<int>(nextRoot.cattletype, "Level02.cattletype");//int.Parse(nextRoot.cattletype);
                Guard.ForValidFk(CattleTypeId, "CattleType Id is not valid.");

                Chainspeed = Guard.ConverteValor<decimal>(nextRoot.chainspeed, "Level02.chainspeed"); //decimal.Parse(nextRoot.chainspeed);
                Guard.ForNegative(Chainspeed, "Chainspeed");
                //ConsecutiveFailureIs = bool.Parse(nextRoot.Con)

                LotNumber = Guard.ConverteValor<decimal>(nextRoot.lotnumber, "Level02.lotnumber");// decimal.Parse(nextRoot.lotnumber);
                Guard.ForNegative(LotNumber, "LotNumber");

                Mudscore = Guard.ConverteValor<decimal>(nextRoot.mudscore, "Level02.mudscore"); //decimal.Parse(nextRoot.mudscore);
                Guard.ForNegative(Mudscore, "Mudscore");

                if (nextRoot.consecutivefailuretotal != null)
                    ConsecutiveFailureTotal = Guard.ConverteValor<int>(nextRoot.consecutivefailuretotal, "Level02.consecutivefailuretotal"); //int.Parse(nextRoot.consecutivefailuretotal);
                Guard.ForNegative(ConsecutiveFailureTotal, "ConsecutiveFailureTotal");
            }

            #endregion

            #region Valores Default Para Todos

            Level02Id = Guard.ConverteValor<int>(nextRoot.level02id, "Level02.level02id"); // int.Parse(nextRoot.level02id);
            Guard.ForValidFk(Level02Id, "Level02 Id is not valid.");

            UnitId = unitId;
            Guard.ForValidFk(UnitId, "Unit id is not valid.");

            Period = Guard.ConverteValor<int>(nextRoot.period, "Level02.period"); //int.Parse(nextRoot.period);
            Guard.ForNegative(Period, "Period");

            if (nextRoot.phase.IsNotNull())
            {
                Phase = Guard.ConverteValor<int>(nextRoot.phase, "Level02.phase"); //int.Parse(nextRoot.phase);
                Guard.ForNegative(Phase, "Phase number");
            }

            ReauditIs = Guard.ConverteValor<bool>(nextRoot.reaudit, "Level02.reaudit"); //bool.Parse(nextRoot.reaudit);
            Guard.VerifyIfIsBool(ReauditIs, "ReauditIs");

            ReauditNumber = Guard.ConverteValor<int>(nextRoot.reauditnumber, "Level02.reauditnumber"); //int.Parse(nextRoot.reauditnumber);
            Guard.ForNegative(ReauditNumber, "ReauditNumber");
            if (nextRoot.notavaliable.IsNotNull())
                NotEvaluatedIs = Guard.ConverteValor<bool>(nextRoot.notavaliable, "Level02.notavaliable"); //bool.Parse(nextRoot.notavaliable);
            Guard.VerifyIfIsBool(NotEvaluatedIs, "NotEvaluatedIs");

            //nextRoot.sample = "1";
            if (nextRoot.sample != null)
                Sample = Guard.ConverteValor<int>(nextRoot.sample, "Level02.sample"); //int.Parse(nextRoot.sample);
            else
                Sample = 1;
            Guard.ForNegative(Sample, "Sample");

            Shift = Guard.ConverteValor<int>(nextRoot.shift, "Level02.shift"); //int.Parse(nextRoot.shift);
            Guard.ForNegative(Shift, "Shift");

            if (Phase > 1)
            {
                var partePrincipalDaData = nextRoot.startphasedate.Split(':')[0] + ":" + nextRoot.startphasedate.Split(':')[1];
                StartPhaseDate = DateTime.ParseExact(partePrincipalDaData, "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
            }

            #endregion

            if (nextRoot.idcorrectiveaction != null)
                CorrectiveActionId = Guard.ConverteValor<int>(nextRoot.idcorrectiveaction, "Level02.idcorrectiveaction"); //int.Parse(nextRoot.idcorrectiveaction);


            #region Cria Level03 Collection

            if (nextRoot.nextnextRoot == null)
                throw new ExceptionHelper("Lista de CollectionLevel03DTO vazia.");

            if (nextRoot.nextnextRoot.Count == 0)
                throw new ExceptionHelper("Lista de CollectionLevel03DTO vazia.");

            collectionLevel03DTO = new List<CollectionLevel03DTO>();
            foreach (var x in nextRoot.nextnextRoot)
                collectionLevel03DTO.Add(new CollectionLevel03DTO(x, Level01Id, Level02Id));

            //MOCK
            AuditorId = 1;
            #endregion
            //08 / 30 / 2016 10:38
            if (nextRoot.datetime != null)
                CollectionDate = DateTime.ParseExact(nextRoot.datetime.Split(':')[0] + ":" + nextRoot.datetime.Split(':')[1], "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);

            if (nextRoot.evaluate != null)
                EvaluationNumber = Guard.ConverteValor<int>(nextRoot.evaluate, "Level02.evaluate"); //int.Parse(nextRoot.evaluate);
            else
                EvaluationNumber = 1;

        }

        public int ConsolidationLevel02Id { get; set; }
        public int Level01Id { get; set; }
        public int Level02Id { get; set; }
        public int UnitId { get; set; }
        public int AuditorId { get; set; }
        public int Shift { get; set; }
        public int Period { get; set; }
        public int Phase { get; set; }
        public bool ReauditIs { get; set; }
        public int ReauditNumber { get; set; }
        public System.DateTime CollectionDate { get; set; }
        public System.DateTime StartPhaseDate { get; set; }
        public Nullable<int> EvaluationNumber { get; set; }
        public int Sample { get; set; }
        public int CattleTypeId { get; set; }
        public decimal Chainspeed { get; set; }
        public bool ConsecutiveFailureIs { get; set; }
        public int ConsecutiveFailureTotal { get; set; }
        public decimal LotNumber { get; set; }
        public decimal Mudscore { get; set; }
        public bool NotEvaluatedIs { get; set; }
        public bool Duplicated { get; set; }
        public Level01DTO Level01 { get; set; }
        public Level02DTO Level02 { get; set; }
        public UserDTO UserSgq { get; set; }
        public List<CollectionLevel03DTO> CollectionLevel03 { get; set; }
        public List<CorrectiveActionDTO> CorrectiveAction { get; set; }


        //public int EvaluationNumber { get; set; }
        public int CorrectiveActionId { get; set; }
        //public int ConsolidationLevel02Id { get; set; }
        //public int AuditorId { get; set; }
        //public int CattleTypeId { get; set; }
        //public int Level01Id { get; set; }
        //public int Level02Id { get; set; }
        //public int UnitId { get; set; }
        //public decimal Chainspeed { get; set; }
        //public bool ConsecutiveFailureIs { get; set; }
        //public int ConsecutiveFailureTotal { get; set; }
        //public decimal LotNumber { get; set; }
        //public decimal Mudscore { get; set; }
        //public int Period { get; set; }
        //public int Phase { get; set; }
        //public bool ReauditIs { get; set; }
        //public int ReauditNumber { get; set; }
        //public bool NotEvaluatedIs { get; set; }
        //public int Sample { get; set; }
        //public int Shift { get; set; }
        public string Name { get; set; }
        //public System.DateTime StartPhaseDate { get; set; }
        //public System.DateTime CollectionDate { get; set; }

        public List<CollectionLevel03DTO> collectionLevel03DTO { get; set; }
        public ConsolidationLevel01DTO consolidationLevel01DTO { get; set; }
        public bool Remove { get; set; }
        public CorrectiveActionDTO CorrectiveActionSaved { get; set; }
    }
}
