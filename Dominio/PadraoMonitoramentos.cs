namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PadraoMonitoramentos
    {
        public int Id { get; set; }

        public int Padrao { get; set; }

        public int Monitoramento { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public int? Unidade { get; set; }

        public int? UnidadeMedida { get; set; }

        public int? UnidadeMedidaLegenda { get; set; }

        [StringLength(100)]
        public string Tipo { get; set; }

        public virtual Monitoramentos Monitoramentos { get; set; }

        public virtual Padroes Padroes { get; set; }

        public virtual UnidadesMedidas UnidadesMedidas { get; set; }

        public virtual Unidades Unidades { get; set; }

        public virtual UnidadesMedidas UnidadesMedidas1 { get; set; }
    }
}
