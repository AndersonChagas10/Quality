using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    [Table("Integ.CollectionData")]
    public class IntegCollectionData
    {
        public int? Table_Id { get; set; }

        [Key]
        public string Key_Integ { get; set; }

        public int? ParLevel1_id { get; set; }

        public int? ParLevel2_id { get; set; }

        public int? ParLevel3_id { get; set; }

        public int? ParCompany_id { get; set; }

        [NotMapped]
        public int? ParCluster_id { get; set; } = 3;

        public decimal? Weight { get; set; }

        public decimal? Value { get; set; }

        public string ValueText { get; set; }

        public decimal? MinInterval { get; set; }

        public decimal? MaxInterval { get; set; }

        public bool? IsConform { get; set; }

        public bool? IsNotEvaluate { get; set; }

        public decimal? Evaluation { get; set; }

        public int? Sample { get; set; }

        public decimal? WeiEvaluation { get; set; }

        public decimal? Defects { get; set; }

        public decimal? WeiDefects { get; set; }

        public int? Coletado { get; set; }
    }
}
