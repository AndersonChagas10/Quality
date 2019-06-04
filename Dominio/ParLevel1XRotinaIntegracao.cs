﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    [Table("ParLevel1XRotinaIntegracao")]
    public class ParLevel1XRotinaIntegracao : BaseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParLevel1_Id { get; set; }

        public int? RotinaIntegracao_Id { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("ParLevel1_Id")]
        public virtual ParLevel1 ParLevel1 { get; set; }

        [ForeignKey("RotinaIntegracao_Id")]
        public virtual RotinaIntegracao RotinaIntegracao { get; set; }
    }
}
