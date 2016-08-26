using DTO.BaseEntity;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO
{
    public class CollectionLevel02DTO : EntityBase
    {

        public CollectionLevel02DTO() { }

        public CollectionLevel02DTO(NextRoot nextRoot)
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

            #region Caso for CCA

            if (nextRoot.level01id.Equals("2"))
            {
            }

            #endregion

            #region Caso for CFF

            if (nextRoot.level01id.Equals("3"))
            {
                CattleType_Id = int.Parse(nextRoot.cattletype);
                Guard.ForValidFk(CattleType_Id, "CattleType Id is not valid.");

                Chainspeed = decimal.Parse(nextRoot.chainspeed);
                Guard.ForNegative(Chainspeed, "Chainspeed");
                //ConsecutiveFailureIs = bool.Parse(nextRoot.Con)

                LotNumber = decimal.Parse(nextRoot.lotnumber);
                Guard.ForNegative(LotNumber, "LotNumber");

                Mudscore = decimal.Parse(nextRoot.mudscore);
                Guard.ForNegative(Mudscore, "Mudscore");

                ConsecutiveFailureTotal = int.Parse(nextRoot.consecutivefailuretotal);
                Guard.ForNegative(ConsecutiveFailureTotal, "ConsecutiveFailureTotal");
            }

            #endregion

            #region Valores Default Para Todos

            Level02_Id = int.Parse(nextRoot.level02id);
            Guard.ForValidFk(Level02_Id, "Level02 Id is not valid.");

            Unit_Id = int.Parse(nextRoot.unidadeid);
            Guard.ForValidFk(Unit_Id, "Unit id is not valid.");

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

            nextRoot.sample = "1";
            Sample = int.Parse(nextRoot.sample);
            Guard.ForNegative(Sample, "Sample");

            Shift = int.Parse(nextRoot.shift);
            Guard.ForNegative(Shift, "Shift");

            if (Phase > 1)
                StartPhaseDate = DateTime.Parse(nextRoot.startphasedate);

            #endregion
            
            #region Cria Level03 Collection

            collectionLevel03DTO = new List<CollectionLevel03DTO>();
            foreach (var x in nextRoot.nextnextRoot)
                collectionLevel03DTO.Add(new CollectionLevel03DTO(x, Level01_Id, Level02_Id)); 

            #endregion

        }

        public List<CollectionLevel03DTO> collectionLevel03DTO { get; set; }
        public int ConsolidationLevel02_Id { get; set; }
        public int Auditor_Id { get; set; } //FK
        public int CattleType_Id { get; set; } //FK
        public int Level01_Id { get; set; } //FK
        public int Level02_Id { get; set; } //FK
        public int Unit_Id { get; set; } //FK
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

        public ConsolidationLevel01DTO consolidationLevel01DTO { get; set; }
    }
}
