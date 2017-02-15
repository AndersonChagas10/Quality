using System;
using System.Runtime.Serialization;

namespace PA.DTO
{
    [DataContract]
    [Serializable]
    public class LogOperacaoDTO : BaseDTO
    {
        [DataMember]
        public string MensagemOperacao { get; set; }
        [DataMember]
        public Nullable<int> Linha { get; set; }
        [DataMember]
        public string NomeUsuario { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DataOcorrencia { get; set; }
        [DataMember]
        public Nullable<int> TipoRegistro { get; set; }
        [DataMember]
        public string TextoExcecao { get; set; }
        [DataMember]
        public string TextoPilhaExcecao { get; set; }
        [DataMember]
        public string DescricaoLanIp { get; set; }
        [DataMember]
        public string DescricaoInternetIp { get; set; }
        [DataMember]
        public string NomeMetodo { get; set; }
        [DataMember]
        public string UrlTela { get; set; }
    }
}
