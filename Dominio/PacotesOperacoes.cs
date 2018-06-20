namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PacotesOperacoes
    {
        public int Id { get; set; }

        public int? Pacote { get; set; }

        public int? Operacao { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public virtual Operacoes Operacoes { get; set; }

        public virtual Pacotes Pacotes { get; set; }
    }
}
