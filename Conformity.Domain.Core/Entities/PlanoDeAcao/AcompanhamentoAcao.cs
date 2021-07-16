﻿using Conformity.Domain.Core.DTOs;
using Conformity.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conformity.Domain.Core.Entities.PlanoDeAcao
{
    [Table("PA.AcompanhamentoAcao")]
    public partial class AcompanhamentoAcao : BaseModel, IEntity
    {
        public int Acao_Id { get; set; }
        public int UserSgq_Id { get; set; }
        public string Observacao { get; set; }
        public DateTime DataRegistro { get; set; } = DateTime.Now;
        public ICollection<AcompanhamentoAcaoXNotificar> ListaNotificar { get; set; }
        public int Status { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<AcompanhamentoAcaoXAttributes> AcompanhamentoAcaoXAttributes { get; set; }

        [ForeignKey("UserSgq_Id")]
        public virtual UserSgq UserSgq { get; set; }
    }
}
