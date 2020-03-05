using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Result_Level3XParHeaderFieldGeral : BaseModel
    {
        public int Id { get; set; }

        public int ResultLevel3_Id { get; set; }

        public int ParHeaderFieldGeral_Id { get; set; }

        public int ParFieldType_Id { get; set; }

        [NotMapped]
        public int Collection_Id { get; set; }

        public string Value { get; set; }

        public int? Evaluation { get; set; }

        public int? Sample { get; set; }

        public bool IsActive { get; set; }
    }
}
