namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Equipamentos
    {
        public int Id { get; set; }

        public int Unidade { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(100)]
        public string Nome { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        [StringLength(100)]
        public string Tipo { get; set; }

        [StringLength(100)]
        public string Subtipo { get; set; }

        public int? ParCompany_Id { get; set; }

        public string ParCompanyName { get; set; }

        public virtual Unidades Unidades { get; set; }
    }
}
