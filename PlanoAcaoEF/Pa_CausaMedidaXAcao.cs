namespace PlanoAcaoEF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pa_CausaMedidaXAcao
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

        public int? CausaGenerica_Id { get; set; }

        public int? CausaEspecifica_Id { get; set; }

        public int? ContramedidaGenerica_Id { get; set; }

        public int? ContramedidaEspecifica_Id { get; set; }

        public int? GrupoCausa_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Acao_Id { get; set; }
    }
}
