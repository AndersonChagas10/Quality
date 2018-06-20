namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DepartamentoProdutos
    {
        public int Id { get; set; }

        public int Departamento { get; set; }

        public int Produto { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public virtual Departamentos Departamentos { get; set; }

        public virtual Produtos Produtos { get; set; }
    }
}
