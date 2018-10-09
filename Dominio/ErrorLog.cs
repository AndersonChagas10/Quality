using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    [Table("ErrorLog")]
    public partial class ErrorLog
    {
        public ErrorLog()
        {
        }

        public long Id { get; set; }

        public string StackTrace { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? AddDate { get; set; }
    }
}