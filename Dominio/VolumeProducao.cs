namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VolumeProducao")]
    public partial class VolumeProducao
    {
        public int Id { get; set; }

        public int Operacao { get; set; }

        public int Unidade { get; set; }

        [Column(TypeName = "date")]
        public DateTime Data { get; set; }

        public int Departamento { get; set; }

        public int? Volume { get; set; }

        public int? Quartos { get; set; }

        public decimal? Meta { get; set; }

        public decimal? ToleranciaDia { get; set; }

        public decimal? Nivel1 { get; set; }

        public decimal? Nivel2 { get; set; }

        public decimal? Nivel3 { get; set; }

        public int? Quantidade { get; set; }

        public int? HorasTrabalho { get; set; }

        public int? QuantidadeMediaHora { get; set; }

        public int? NivelGeralInspecao { get; set; }

        public int? AvaliacaoHora { get; set; }

        public int? QtdColabOuEsteiras { get; set; }

        public int? QuantidadeAvaliacao { get; set; }

        public int? TamanhoAmostra { get; set; }

        public int? AmostraAvaliacao { get; set; }

        public int? AmostraDia { get; set; }

        public int? QtdFamiliaProdutos { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public virtual Departamentos Departamentos { get; set; }

        public virtual Operacoes Operacoes { get; set; }

        public virtual Unidades Unidades { get; set; }
    }
}
