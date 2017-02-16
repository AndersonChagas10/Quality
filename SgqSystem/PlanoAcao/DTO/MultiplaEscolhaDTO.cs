using System;
using System.Runtime.Serialization;

namespace PA.DTO
{
    [DataContract]
    [Serializable]
    public class MultiplaEscolhaDTO : BaseDTO
    {
        [DataMember]
        public int IdCampo { get; set; }
        [DataMember]
        public string Nome { get; set; }
        [DataMember]
        public Nullable<int> IdTabelaExterna { get; set; }
        [DataMember]
        public string Cor { get; set; }
        [DataMember]
        public string NomeTabelaExterna { get; set; }
        [DataMember]
        public System.DateTime DataCriacao { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DataAlteracao { get; set; }
        [DataMember]
        public bool Ativo { get; set; }
        [DataMember]
        public Nullable<int> IdMultiplaEscolhaPai { get; set; }

        //[DataMember]
        //public CampoDTO Campo { get; set; }
        //[DataMember]
        //public List<VinculoCampoCabecalhoDTO> VinculoCampoCabecalho { get; set; }
        //[DataMember]
        //public List<VinculoCampoTarefaDTO> VinculoCampoTarefa { get; set; }
        //[DataMember]
        //public List<VinculoParticipanteMultiplaEscolhaDTO> VinculoParticipanteMultiplaEscolha { get; set; }

    }
}
