using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PA.DTO
{
    [DataContract]
    [Serializable]
    public class CampoDTO : BaseDTO
    {
        [DataMember]
        public string Nome { get; set; }
        [DataMember]
        public string Tipo { get; set; }
        [DataMember]
        public bool Agrupador { get; set; }
        [DataMember]
        public Nullable<int> Sequencia { get; set; }
        [DataMember]
        public bool Obrigatorio { get; set; }
        [DataMember]
        public bool Ativo { get; set; }
        [DataMember]
        public int IdProjeto { get; set; }
        [DataMember]
        public bool Predefinido { get; set; }
        [DataMember]
        public bool Modificavel { get; set; }
        [DataMember]
        public System.DateTime DataCriacao { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DataAlteracao { get; set; }
        [DataMember]
        public Nullable<int> IdCampoPai { get; set; }
        [DataMember]
        public Nullable<bool> FixadoEsquerda { get; set; }
        [DataMember]
        public Nullable<bool> ExibirTabela { get; set; }
        [DataMember]
        public Nullable<bool> Cabecalho { get; set; }
        [DataMember]
        public Nullable<int> IdGrupoCabecalho { get; set; }

        //[DataMember]
        //public GrupoCabecalhoDTO GrupoCabecalho { get; set; }
        //[DataMember]
        //public ProjetoDTO Projeto { get; set; }
        [DataMember]
        public List<MultiplaEscolhaDTO> MultiplaEscolha { get; set; }
        //[DataMember]
        //public List<VinculoCampoCabecalhoDTO> VinculoCampoCabecalho { get; set; }
        //[DataMember]
        //public List<VinculoCampoTarefaDTO> VinculoCampoTarefa { get; set; }
    }
}
