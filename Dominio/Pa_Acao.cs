namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pa_Acao : BaseModel
    {
        public int Id { get; set; }

        public int? Unidade_Id { get; set; }

        public int? Departamento_Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? QuandoInicio { get; set; }

        public int? DuracaoDias { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? QuandoFim { get; set; }

        public string ComoPontosimportantes { get; set; }

        public int? Predecessora_Id { get; set; }

        public string PraQue { get; set; }

        public decimal? QuantoCusta { get; set; }

        public int? Status { get; set; }

        public int? Panejamento_Id { get; set; }

        public int? Pa_IndicadorSgqAcao_Id { get; set; }

        public int? Pa_Problema_Desvio_Id { get; set; }

        public int? Level1Id { get; set; }

        public int? Level2Id { get; set; }

        public int? Level3Id { get; set; }

        public int? Quem_Id { get; set; }

        public int? CausaGenerica_Id { get; set; }

        public int? ContramedidaGenerica_Id { get; set; }

        public int? GrupoCausa_Id { get; set; }

        public string CausaEspecifica { get; set; }

        public string ContramedidaEspecifica { get; set; }

        public int? TipoIndicador { get; set; }

        public int? Fta_Id { get; set; }

        [StringLength(255)]
        public string Observacao { get; set; }

        [StringLength(500)]
        public string Level1Name { get; set; }

        [StringLength(500)]
        public string Level2Name { get; set; }

        [StringLength(500)]
        public string Level3Name { get; set; }

        [StringLength(500)]
        public string Regional { get; set; }

        [StringLength(500)]
        public string UnidadeName { get; set; }

        public int? UnidadeDeMedida_Id { get; set; }
    }
}
