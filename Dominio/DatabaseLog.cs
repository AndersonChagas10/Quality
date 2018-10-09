using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    [Table("DatabaseLog")]
    public partial class DatabaseLog
    {
        public DatabaseLog()
        {
        }

        public long Id { get; set; }

        public string Tabela { get; set; }

        public string Json { get; set; }

        public int Operacao { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? AddDate { get; set; }
    }
}