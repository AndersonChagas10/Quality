namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NQA")]
    public partial class NQA
    {
        public int Id { get; set; }

        public int NivelGeralInspecao { get; set; }

        public int TamanhoLoteMin { get; set; }

        public int TamanhoLoteMax { get; set; }

        public int Amostra { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }
    }
}
