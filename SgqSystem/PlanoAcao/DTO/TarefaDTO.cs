using QualidadeTotal.PlanoAcao.DTO;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PA.DTO
{
    [DataContract]
    [Serializable]
    public class TarefaDTO : BaseDTO
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
        [DataMember]
        public Nullable<int> IdCabecalho { get; set; }



        //[DataMember]
        //public ParticipanteDTO Usuarios { get; set; }
        //[DataMember]
        //public ProjetoDTO Projeto { get; set; }
        [DataMember]
        public List<AcompanhamentoTarefaDTO> AcompanhamentoTarefa { get; set; }
        [DataMember]
        public CabecalhoDTO Cabecalho { get; set; }
        [DataMember]
        public List<VinculoCampoTarefaDTO> VinculoCampoTarefa { get; set; }
    }
}
