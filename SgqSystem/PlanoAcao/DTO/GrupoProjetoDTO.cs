using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace PA.DTO
{
    [DataContract]
    [Serializable]
    public class GrupoProjetoDTO : BaseDTO
    {
        [DataMember]
        public string Nome { get; set; }
        [DataMember]
        public Nullable<int> Sequencia { get; set; }
        [DataMember]
        public Nullable<int> IdEmpresa { get; set; }
        [DataMember]
        public System.DateTime DataCriacao { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DataAlteracao { get; set; }
        [DataMember]
        public bool Ativo { get; set; }

        [DataMember]
        public EmpresaDTO Empresa { get; set; }
        [DataMember]
        public List<ProjetoDTO> Projeto { get; set; }
    }
}
