namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AcoesPreventivas
    {
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime Data { get; set; }

        public int Departamento { get; set; }

        public int Operacao { get; set; }

        public int Tarefa { get; set; }

        public int Avaliacao { get; set; }

        public int Amostra { get; set; }

        [Required]
        [StringLength(20)]
        public string Motivo { get; set; }

        [Required]
        public string AcaoPreventiva { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }
    }
}
