﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    [Table("CollectionLevel2XParHeaderFieldGeral")]
    public class CollectionLevel2XParHeaderFieldGeral
    {
        public int Id { get; set; }

        public int CollectionLevel2_Id { get; set; }

        public int ParHeaderFieldGeral_Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string ParHeaderField_Name { get; set; }

        public int ParFieldType_Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Value { get; set; }

        public int? Evaluation { get; set; }

        public int? Sample { get; set; }

        [ForeignKey("CollectionLevel2_Id")]
        public virtual CollectionLevel2 CollectionLevel2 { get; set; }

        [ForeignKey("ParFieldType_Id")]
        public virtual ParFieldType ParFieldType { get; set; }

        //[ForeignKey("[ParHeaderFieldGeral_Id]")]
        //public virtual ParHeaderFieldGeral ParHeaderFieldGeral { get; set; }
    }
}
