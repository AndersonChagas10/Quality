using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    [Table("ParLevelHeaderField")]
    public class ParLevelHeaderField : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TableName { get; set; }
        public bool IsActive { get; set; }
    }
}