using PA.DTO;
using System;
using System.Runtime.Serialization;

namespace QualidadeTotal.PlanoAcao.DTO
{
    [DataContract]
    [Serializable]
    public class VinculoCampoCabecalhoDTO
    {
        [DataMember]
        public Nullable<int> IdMultiplaEscolha { get; set; }
        [DataMember]
        public int IdCampo { get; set; }
        [DataMember]
        public int IdCabecalho { get; set; }
        [DataMember]
        public string Valor { get; set; }
        [DataMember]
        public Nullable<int> IdParticipante { get; set; }
        [DataMember]
        public Nullable<int> IdGrupoCabecalho { get; set; }

        //[DataMember]
        //public CabecalhoDTO Cabecalho { get; set; }
        [DataMember]
        public CampoDTO Campo { get; set; }
        //[DataMember]
        //public MultiplaEscolhaDTO MultiplaEscolha { get; set; }
        //[DataMember]
        //public Usuarios Usuarios { get; set; }
        //[DataMember]
        //public GrupoCabecalhoDTO GrupoCabecalho { get; set; }
    }
}