using System;
using System.Runtime.Serialization;

namespace PA.DTO
{
    [DataContract]
    [Serializable]
    public class VinculoCampoTarefaDTO : BaseDTO
    {
        [DataMember]
        public Nullable<int> IdMultiplaEscolha { get; set; }
        [DataMember]
        public int IdCampo { get; set; }
        [DataMember]
        public int IdTarefa { get; set; }
        [DataMember]
        public string Valor { get; set; }
        [DataMember]
        public Nullable<int> IdParticipante { get; set; }

        [DataMember]
        public CampoDTO Campo { get; set; }
        [DataMember]
        public MultiplaEscolhaDTO MultiplaEscolha { get; set; }
        [DataMember]
        public ParticipanteDTO Usuarios { get; set; }
        //[DataMember]
        //public TarefaDTO Tarefa { get; set; }
    }
}
