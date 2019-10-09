using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class CollectionLevel2XParCargo : BaseModel
    {
        [Key]
        public long Id { get; set; }

        public long CollectionLevel2_Id { get; set; }

        public int ParCargo_Id { get; set; }
    }
}
