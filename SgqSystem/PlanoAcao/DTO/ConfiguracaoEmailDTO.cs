using System;
using System.Runtime.Serialization;

namespace PA.DTO
{
    [DataContract]
    [Serializable]
    public class ConfiguracaoEmailDTO : BaseDTO
    {
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Senha { get; set; }
        [DataMember]
        public System.DateTime DataCriacao { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DataAlteracao { get; set; }
        [DataMember]
        public string Host { get; set; }
        [DataMember]
        public int Port { get; set; }
        [DataMember]
        public bool Ativo { get; set; }

    }
}
