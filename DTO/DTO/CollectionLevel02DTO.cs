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

        public CollectionLevel02DTO(NextRoot nextRoot)
        {
            ValidaBaseEntity();

            //FK
            //Auditor_Id = int.Parse(nextRoot.auditorid);
            //Guard.ForValidFk(Auditor_Id, "Auditor_Id  is not valid.");

            CattleType_Id = int.Parse(nextRoot.cattletype);
            Guard.ForValidFk(CattleType_Id, "CattleType Id is not valid.");

            //Level01_Id = int.Parse(nextRoot.); COLOCAR NO DOMAIn

            Level02_Id = int.Parse(nextRoot.level02id);
            Guard.ForValidFk(Level02_Id, "Level02 Id is not valid.");

            Unit_Id = int.Parse(nextRoot.unidadeid);
            Guard.ForValidFk(Unit_Id, "Unit id is not valid.");

            Chainspeed = decimal.Parse(nextRoot.chainspeed);
            Guard.ForNegative(Chainspeed, "Chainspeed");
            //ConsecutiveFailureIs = bool.Parse(nextRoot.Con)

            ConsecutiveFailureTotal = int.Parse(nextRoot.consecutivefailuretotal);
            Guard.ForNegative(ConsecutiveFailureTotal, "Chainspeed");

            LotNumber = decimal.Parse(nextRoot.lotnumber);
            Guard.ForNegative(LotNumber, "Chainspeed");

            Mudscore = decimal.Parse(nextRoot.mudscore);
            Guard.ForNegative(Mudscore, "Chainspeed");

            Period = int.Parse(nextRoot.period);
            Guard.ForNegative(Period, "Chainspeed");

            if (Guard.VerifyStringNullValue(nextRoot.phase))
            {
                Phase = int.Parse(nextRoot.phase);
                Guard.ForNegative(Phase, "Phase number");
            }

            ReauditIs = bool.Parse(nextRoot.reaudit);
            Guard.VerifyIfIsBool(ReauditIs, "Chainspeed");

            ReauditNumber = int.Parse(nextRoot.reauditnumber);
            Guard.ForNegative(ReauditNumber, "Chainspeed");

            NotEvaluatedIs = bool.Parse(nextRoot.notavaliable);
            Guard.VerifyIfIsBool(NotEvaluatedIs, "Not Evalueated");

            nextRoot.sample = "1";
            Sample = int.Parse(nextRoot.sample);
            Guard.ForNegative(Sample, "Chainspeed");

            Shift = int.Parse(nextRoot.shift);
            Guard.ForNegative(Shift, "Chainspeed");

            if (Phase > 1)
                StartPhaseDate = DateTime.Parse(nextRoot.startphasedate);

        }

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
