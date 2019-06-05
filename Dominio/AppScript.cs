using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    [Table("AppScript")]
    public class AppScript
    {
        public int Id { get; set; }

        public string Version { get; set; }

        [DisplayName("Nome do Arquivo")]
        public string ArchiveName { get; set; }

        public string Script { get; set; }
    }
}
