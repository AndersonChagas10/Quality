using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    [Table("LogRequest")]
    public partial class LogRequest
    {
        public long Id { get; set; }
        public string Params { get; set; }
        public string Path { get; set; }
        public string Request { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int UserId { get; set; }
    }
}
