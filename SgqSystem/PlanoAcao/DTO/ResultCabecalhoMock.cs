using System;
using System.Runtime.Serialization;

namespace QualidadeTotal.PlanoAcao.DTO
{
    [DataContract]
    [Serializable]
    public class ResultCabecalhoMock
    {
        [DataMember]
        public string NomeCampo { get; set; }
        [DataMember]
        public string Valor { get; set; }
    }

    [DataContract]
    [Serializable]
    public class RequestCabecalhoMock
    {
        [DataMember]
        public int? IdCabecalho { get; set; }

        [DataMember]
        public string Diretoria { get; set; }
        [DataMember]
        public string Gerencia { get; set; }
        [DataMember]
        public string Coordenacao { get; set; }
        [DataMember]
        public string Missao { get; set; }
        [DataMember]
        public string Visao { get; set; }
        [DataMember]
        public string Dimencao { get; set; }
        [DataMember]
        public string TemaAssunto { get; set; }
        [DataMember]
        public string Objetivo { get; set; }
        [DataMember]
        public string Indicadores { get; set; }
        [DataMember]
        public string MetaObjetivoGerencial { get; set; }
        [DataMember]
        public string MetaValorde { get; set; }
        [DataMember]
        public string MetaValorpara { get; set; }
        [DataMember]
        public string MetaInicio { get; set; }
        [DataMember]
        public string MetaFim { get; set; }

    }


}