using Conformity.Domain.Core.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conformity.Domain.Core.Entities.PlanoDeAcao
{
    [Table("EvidenciaAcaoConcluida", Schema = "PA")]
    public partial class EvidenciaConcluida : BaseModel, IEntity
    {
        public int Acao_Id { get; set; }
        public string Path { get; set; }
        public bool IsActive { get; set; }

    }
}
