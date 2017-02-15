using System;
using System.Runtime.Serialization;

namespace PA.DTO
{
    [DataContract]
    [Serializable]
    public class ParticipanteDTO : BaseDTO
    {
        [DataMember]
        public Nullable<int> Identificador { get; set; }
        [DataMember]
        public int Unidade { get; set; }
        [DataMember]
        public string Usuario { get; set; }
        [DataMember]
        public string Senha { get; set; }
        [DataMember]
        public string Nome { get; set; }
        [DataMember]
        public int UsuarioInsercao { get; set; }
        [DataMember]
        public System.DateTime DataInsercao { get; set; }
        [DataMember]
        public Nullable<int> UsuarioAlteracao { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DataAlteracao { get; set; }
        [DataMember]
        public string Funcao { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public Nullable<bool> DefineFamilia { get; set; }
        [DataMember]
        public Nullable<bool> DefineParametros { get; set; }
        [DataMember]
        public Nullable<bool> EditaUsuarios { get; set; }
        [DataMember]
        public Nullable<bool> ReceberAlerta { get; set; }
        [DataMember]
        public Nullable<int> Regional { get; set; }
        [DataMember]
        public System.DateTime DataAlteracaoSenha { get; set; }
        [DataMember]
        public Nullable<bool> ConfiguraSistema { get; set; }

        //[DataMember]
        //public List<AcompanhamentoTarefaDTO> AcompanhamentoTarefa { get; set; }
        //[DataMember]
        //public EmpresaDTO Empresa { get; set; }
        //[DataMember]
        //public List<TarefaDTO> TarefaPA { get; set; }
        //[DataMember]
        //public List<VinculoCampoTarefaDTO> VinculoCampoTarefa { get; set; }
        //[DataMember]
        //public List<VinculoParticipanteMultiplaEscolhaDTO> VinculoParticipanteMultiplaEscolha { get; set; }
        //[DataMember]
        //public List<VinculoParticipanteProjetoDTO> VinculoParticipanteProjeto { get; set; }

    }
}
