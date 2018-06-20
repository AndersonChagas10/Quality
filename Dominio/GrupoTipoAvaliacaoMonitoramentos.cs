namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GrupoTipoAvaliacaoMonitoramentos
    {
        public int Id { get; set; }

        public int GrupoTipoAvaliacao { get; set; }

        public int Monitoramento { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public virtual GrupoTipoAvaliacoes GrupoTipoAvaliacoes { get; set; }

        public virtual Monitoramentos Monitoramentos { get; set; }
    }
}
