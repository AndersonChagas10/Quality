using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    [Table("LogError")]
    public class LogError
    {
        public int Id { get; set; }
        public string ErrorMessage { get; set; }
        public int Line { get; set; }
        public string Method { get; set; }
        public string Controller { get; set; }
        public string Object { get; set; }
        public string StackTrace { get; set; }
        public DateTime AddDate { get; set; }
    }
}
