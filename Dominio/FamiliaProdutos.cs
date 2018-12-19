namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FamiliaProdutos
    {
        public int Id { get; set; }

        public int? Operacao { get; set; }

        public int Ano { get; set; }

        public int Mes { get; set; }

        public int? Unidade { get; set; }

        public int Posicao { get; set; }

        public int Produto { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        [ForeignKey("Operacao")]
        public virtual Operacoes Operacoes { get; set; }

        [ForeignKey("Produto")]
        public virtual Produtos Produtos { get; set; }

        [ForeignKey("Unidade")]
        public virtual Unidades Unidades { get; set; }
    }
}
