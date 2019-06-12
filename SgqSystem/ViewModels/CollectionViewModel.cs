using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqSystem.ViewModels
{
    public class CollectionViewModel
    {
        public int Id { get; set; }

        public int UnitId { get; set; }

        public int Shift { get; set; }

        public int Period { get; set; }

        public int ParLevel1_Id { get; set; }

        public int ParLevel2_Id { get; set; }

        public int Sample { get; set; }

        public int? EvaluationNumber { get; set; }

        public int? ParDepartment_Id { get; set; }

        public int? ParCargo_Id { get; set; }

        public int? ParCluster_Id { get; set; }

        public int AuditorId { get; set; }

        public DateTime CollectionDate { get; set; }

        public DateTime StartPhaseDate { get; set; }
        public decimal Defects { get; internal set; }
        public decimal WeiDefects { get; internal set; }
        public int TotalLevel3Evaluation { get; internal set; }
        public decimal WeiEvaluation { get; internal set; }
        public object AlterDate { get; internal set; }
        public DateTime AddDate { get; internal set; }
        public string Key { get; internal set; }
    }
}