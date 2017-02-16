using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PA.DTO
{
    [DataContract]
    [Serializable]
    public class ProjetoDTO : BaseDTO
    {
        [DataMember]
        public string Nome { get; set; }
        [DataMember]
        public Nullable<int> IdGrupoProjeto { get; set; }
        [DataMember]
        public System.DateTime DataCriacao { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DataAlteracao { get; set; }
        [DataMember]
        public bool Ativo { get; set; }

        //[DataMember]
        //public List<CabecalhoDTO> Cabecalho { get; set; }
        [DataMember]
        public List<CampoDTO> Campo { get; set; }
        //[DataMember]
        //public List<GrupoCabecalhoDTO> GrupoCabecalho { get; set; }
        [DataMember]
        public GrupoProjetoDTO GrupoProjeto { get; set; }
        [DataMember]
        public List<TarefaDTO> TarefaPA { get; set; }
        [DataMember]
        public List<VinculoParticipanteProjetoDTO> VinculoParticipanteProjeto { get; set; }
    }
}
