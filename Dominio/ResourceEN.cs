using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    [Table("ResourceEN")]
    public class ResourceEN 
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
