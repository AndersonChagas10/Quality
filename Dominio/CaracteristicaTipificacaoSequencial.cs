namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CaracteristicaTipificacaoSequencial")]
    public partial class CaracteristicaTipificacaoSequencial
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int Sequencial { get; set; }

        public decimal? nCdCaracteristica_Id { get; set; }

        public virtual CaracteristicaTipificacao CaracteristicaTipificacao { get; set; }
    }
}
