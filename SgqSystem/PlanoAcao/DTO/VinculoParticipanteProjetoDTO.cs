using System;
using System.Runtime.Serialization;
namespace PA.DTO
{
    [DataContract]
    [Serializable]
    public class VinculoParticipanteProjetoDTO : BaseDTO
    {
        [DataMember]
        public int IdProjeto { get; set; }
        [DataMember]
        public int IdParticipante { get; set; }

        [DataMember]
        public ParticipanteDTO Usuarios { get; set; }
        [DataMember]
        public ProjetoDTO Projeto { get; set; }
    }
}
