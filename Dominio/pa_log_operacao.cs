namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pa_log_operacao
    {
        public int id { get; set; }

        [Required]
        public string mensagem_operacao { get; set; }

        public int? linha { get; set; }

        [StringLength(150)]
        public string nm_usuario { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dt_ocorrencia { get; set; }

        public TimeSpan? hr_ocorrencia { get; set; }

        public int? tp_registro { get; set; }

        public string tx_excecao { get; set; }

        [StringLength(50)]
        public string dc_lan_ip { get; set; }

        public string tx_pilha_excecao { get; set; }

        [StringLength(50)]
        public string dc_internet_ip { get; set; }
    }
}
