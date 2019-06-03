using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    [Table("RotinaIntegracao")]
    public class RotinaIntegracao : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public string DataSource { get; set; }
        public string InitialCatalog { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string query { get; set; }
        public string Parametro { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Retornos { get; set; }
    }
}
