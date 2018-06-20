namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParCalendar")]
    public partial class ParCalendar
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Data { get; set; }

        public bool? Feriado { get; set; }

        public bool? DiaUtil { get; set; }

        public int? NrDiaSemana { get; set; }

        public int? NrSemanaMes { get; set; }

        public int? NrSemanaAno { get; set; }

        public int? Sabado { get; set; }

        public int? Domingo { get; set; }
    }
}
