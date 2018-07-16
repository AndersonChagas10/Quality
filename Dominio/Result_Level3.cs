namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Result_Level3
    {
        public int Id { get; set; }

        public int CollectionLevel2_Id { get; set; }

        public int ParLevel3_Id { get; set; }

        public string ParLevel3_Name { get; set; }

        public decimal? Weight { get; set; }

        public string IntervalMin { get; set; }

        public string IntervalMax { get; set; }

        public string Value { get; set; }

        public string ValueText { get; set; }

        public bool? IsConform { get; set; }

        public bool? IsNotEvaluate { get; set; }

        public decimal? Defects { get; set; }

        public decimal? PunishmentValue { get; set; }

        public decimal? WeiEvaluation { get; set; }

        public decimal? Evaluation { get; set; }

        public decimal? WeiDefects { get; set; }

        public decimal? CT4Eva3 { get; set; }

        public decimal? Sampling { get; set; }

        public bool HasPhoto { get; set; }

        public virtual CollectionLevel2 CollectionLevel2 { get; set; }

        public virtual ParLevel3 ParLevel3 { get; set; }
    }
}
