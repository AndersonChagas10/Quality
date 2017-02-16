using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace QualidadeTotal.PlanoAcao.DTO
{
    [DataContract]
    [Serializable]
    public class QuantidadeStatus
    {
        [DataMember]
        public string TemaAssunto { get; set; }
        [DataMember]
        public string NomeUsuario { get; set; }
        [DataMember]
        public int Ativo { get; set; }
        [DataMember]
        public int Cancelado { get; set; }
        [DataMember]
        public int ConcluidoPrazo { get; set; }
        [DataMember]
        public int ConcluidoAtrasado { get; set; }
        [DataMember]
        public int Replanejado { get; set; }
    }

    [DataContract]
    [Serializable]
    public class ObjetoGrafico
    {
        [DataMember]
        public string Descricao { get; set; }
        [DataMember]
        public List<Series> Series { get; set; }
        [DataMember]
        public List<string> Categorias { get; set; }
        [DataMember]
        public List<Series> SeriesCategoria { get; set; }
    }

    [DataContract]
    [Serializable]
    public class Series
    {
        [DataMember]
        public string Descricao { get; set; }
        [DataMember]
        public int Quantidade { get; set; }
        [DataMember]
        public List<int> Data { get; set; }
    }

    [DataContract]
    [Serializable]
    public class ObjtoBusca
    {
        [DataMember]
        public int IdOque { get; set; }
        [DataMember]
        public int IdFiltro { get; set; }
        [DataMember]
        public List<string> ValoresFiltro { get; set; }
    }


    [DataContract]
    [Serializable]
    public class NumeroMesesAcoes
    {
        [DataMember]
        public int Quantidade { get; set; }
        [DataMember]
        public string Mes { get; set; }
    }

}