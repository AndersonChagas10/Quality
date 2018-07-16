namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LogAlteracoes
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Tabela { get; set; }

        public int Registro { get; set; }

        [Required]
        [StringLength(100)]
        public string Campo { get; set; }

        public string Valor { get; set; }

        public int UsuarioAlteracao { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}
