using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Collection
    {
        public long? Id { get; set; }
        public DateTime? CollectionDate { get; set; }
        public DateTime? AddDate { get; set; }
        public int? UserSgq_Id { get; set; }
        public int? Shift_Id { get; set; }
        public int? Period_Id { get; set; }
        public int? ParCargo_Id { get; set; }
        public int? ParCompany_Id { get; set; }
        public int? ParDepartment_Id { get; set; }
        public int? ParCluster_Id { get; set; }
        public int? ParLevel1_Id { get; set; }
        public int? ParLevel2_Id { get; set; }
        public int? ParLevel3_Id { get; set; }
        public int? CollectionType { get; set; }

        //Result_Level3
        public double? Weigth { get; set; }
        public string IntervalMin { get; set; }
        public string IntervalMax { get; set; }
        public string Value { get; set; }
        public string ValueText { get; set; }
        public bool? IsNotEvaluate { get; set; }
        public bool? IsConform { get; set; }
        public double? Defects { get; set; }
        public double? PunishimentValue { get; set; }
        public double? WeiEvaluation { get; set; }
        public double? Evaluation { get; set; }
        public double? WeiDefects { get; set; }
        public bool? HasPhoto { get; set; }

        //CollectionLevel2
        public int? Sample { get; set; }
        public bool? HaveCorrectiveAction { get; set; }
        public int? Parfrequency_Id { get; set; }
        public int? AlertLevel { get; set; }

        //HeaderField
        public int? ParHeaderField_Id { get; set; }
        public string ParHeaderField_Value { get; set; }

        //outros
        public bool? IsProcessed { get; set; }

        [NotMapped]
        public bool? HasError { get; set; }
    }
}
