using PA.DTO;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace QualidadeTotal.PlanoAcao.DTO
{
    [DataContract]
    [Serializable]
    public class CabecalhoDTO : BaseDTO
    {
        [DataMember]
        public int IdProjeto { get; set; }
        [DataMember]
        public Nullable<int> IdParticipanteCriador { get; set; }
        [DataMember]
        public System.DateTime DataCriacao { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DataAlteracao { get; set; }
        [DataMember]
        public bool Ativo { get; set; }

        //[DataMember]
        //public  Usuarios Usuarios { get; set; }
        //[DataMember]
        //public  ProjetoDTO Projeto { get; set; }
        //[DataMember]
        //public List<TarefaDTO> TarefaPA { get; set; }
        [DataMember]
        public List<VinculoCampoCabecalhoDTO> VinculoCampoCabecalho { get; set; }
    }
}