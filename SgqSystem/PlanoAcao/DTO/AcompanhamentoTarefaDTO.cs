using System;
using System.Runtime.Serialization;

namespace PA.DTO
{
    [DataContract]
    [Serializable]
    public class AcompanhamentoTarefaDTO : BaseDTO
    {
        [DataMember]
        public int IdTarefa { get; set; }
        [DataMember]
        public System.DateTime DataEnvio { get; set; }
        [DataMember]
        public string Comentario { get; set; }
        [DataMember]
        public string Enviado { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string NomeParticipanteEnvio { get; set; }
        [DataMember]
        public int IdParticipanteEnvio { get; set; }
        [DataMember]
        public System.DateTime DataCriacao { get; set; }
        [DataMember]
        public bool Ativo { get; set; }

        //[DataMember]
        //public ParticipanteDTO Usuarios { get; set; }

        #region EXTRA

        [DataMember]
        public string DataEnvioString { get; set; }

        #endregion

    }
}
