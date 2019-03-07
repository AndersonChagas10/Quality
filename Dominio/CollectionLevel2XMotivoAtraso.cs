﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class CollectionLevel2XMotivoAtraso : BaseModel
    {
        [Key]
        public int Id { get; set; }

        public int CollectionLevel2_Id { get; set; }

        public int MotivoAtraso_Id { get; set; }

        public bool IsValid()
        {
            if (CollectionLevel2_Id <= 0 || MotivoAtraso_Id <= 0)
            {
                return false;
            }

            return true;
        }

    }
}
