using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class BaseModel
    {

        [DisplayName("Data de Inserção")]
        public DateTime? AddDate { get; set; }

        [DisplayName("Data de Alteração")]
        public DateTime? AlterDate { get; set; }
    }
}
