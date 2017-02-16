using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PA.DTO
{
    [DataContract]
    [Serializable]
    public class EmpresaDTO : BaseDTO
    {
        [DataMember]
        public Nullable<int> Identificador { get; set; }
        [DataMember]
        public string Sigla { get; set; }
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
        public Nullable<int> SIF { get; set; }
        [DataMember]
        public Nullable<int> Estado { get; set; }
        [DataMember]
        public Nullable<int> Regional { get; set; }
        [DataMember]
        public Nullable<int> Cluster { get; set; }
        [DataMember]
        public Nullable<int> Codigo { get; set; }
        [DataMember]
        public string EnderecoIP { get; set; }
        [DataMember]
        public string NomeDatabase { get; set; }
        [DataMember]
        public Nullable<bool> Ativa { get; set; }

        [DataMember]
        public List<GrupoProjetoDTO> GrupoProjeto { get; set; }
    }
}
