namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EquipamentosAvaliados
    {
        public int Id { get; set; }

        public string Chave { get; set; }

        public int EquipamentoId { get; set; }

        [Required]
        [StringLength(100)]
        public string Equipamento { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }
    }
}
