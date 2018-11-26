using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ParInputTypeValues : BaseModel
    {
        public int Id { get; set; }

        public int Valor { get; set; }

        public int Peso { get; set; }

        public string Cor { get; set; }

        public bool IsActive { get; set; }

        public int? ParLevel3Value_Id { get; set; }

        [ForeignKey("ParLevel3Value_Id")]
        public virtual ParLevel3Value ParLevel3Value { get; set; }
    }
}
