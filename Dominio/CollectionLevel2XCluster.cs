using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class CollectionLevel2XCluster
    {
        [Key]
        public long Id { get; set; }

        public long CollectionLevel2_Id { get; set; }

        public int ParCluster_Id { get; set; }
    }
}
