namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MonitoramentoEquipamentos
    {
        public int Id { get; set; }

        public int Monitoramento { get; set; }

        [Required]
        [StringLength(100)]
        public string Subtipo { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public virtual Monitoramentos Monitoramentos { get; set; }
    }
}
