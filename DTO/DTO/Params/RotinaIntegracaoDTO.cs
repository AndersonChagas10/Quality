using DTO.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO
{
    public class RotinaIntegracaoDTO : EntityBase
    {
        //public string DataSource { get; set; }
        //public string InitialCatalog { get; set; }
        //public string User { get; set; }
        //public string Password { get; set; }
        //public string query { get; set; }
        //public string Parametro { get; set; }
        [Column("Nome")]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
