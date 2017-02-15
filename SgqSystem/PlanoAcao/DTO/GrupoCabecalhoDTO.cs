using PA.DTO;
using System;
using System.Runtime.Serialization;

namespace QualidadeTotal.PlanoAcao.DTO
{
    [DataContract]
    [Serializable]
    public class GrupoCabecalhoDTO : BaseDTO
    {
        [DataMember]
        public string Nome { get; set; }
        [DataMember]
        public Nullable<int> IdGrupoCabecalhoPai { get; set; }
        [DataMember]
        public Nullable<int> IdProjeto { get; set; }
        [DataMember]
        public System.DateTime DataCriacao { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DataAlteracao { get; set; }
        [DataMember]
        public bool Ativo { get; set; }

        //[DataMember]
        //public List<CampoDTO> Campo { get; set; }
        //[DataMember]
        //public ProjetoDTO Projeto { get; set; }
        //[DataMember]
        //public List<VinculoCampoCabecalhoDTO> VinculoCampoCabecalho { get; set; }
    }
}