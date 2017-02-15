using System;
using System.Runtime.Serialization;

namespace PA.DTO
{
    [DataContract]
    [Serializable]
    public class VinculoParticipanteMultiplaEscolhaDTO : BaseDTO
    {
        [DataMember]
        public int IdParticipante { get; set; }
        [DataMember]
        public int IdMultiplaEscolha { get; set; }

        [DataMember]
        public MultiplaEscolhaDTO MultiplaEscolha { get; set; }
        [DataMember]
        public ParticipanteDTO Usuarios { get; set; }
    }
}
