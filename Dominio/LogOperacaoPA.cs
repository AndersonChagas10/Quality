namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LogOperacaoPA")]
    public partial class LogOperacaoPA
    {
        public int Id { get; set; }

        [Required]
        public string MensagemOperacao { get; set; }

        public int? Linha { get; set; }

        public string NomeUsuario { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DataOcorrencia { get; set; }

        public int? TipoRegistro { get; set; }

        public string TextoExcecao { get; set; }

        public string TextoPilhaExcecao { get; set; }

        public string DescricaoLanIp { get; set; }

        public string DescricaoInternetIp { get; set; }

        public string NomeMetodo { get; set; }

        public string UrlTela { get; set; }
    }
}
