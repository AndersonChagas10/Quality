using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.AppViewModel
{
    public class ParVinculoPesoAppViewModel
    {
        public int Id { get; set; }

        public int? Peso { get; set; }

        public bool IsActive { get; set; }

        public string Name { get; set; }

        public int? ParLevel3_Id { get; set; }

        public int? ParCompany_Id { get; set; }

        public int? ParGroupParLevel1_Id { get; set; }

        public int? ParDepartment_Id { get; set; }

        public int? ParLevel1_Id { get; set; }

        public int? ParLevel2_Id { get; set; }

        public int? ParCargo_Id { get; set; }

        public int? ParFrequency_Id { get; set; }

        public int? Evaluation { get; set; }

        public int? Sample { get; set; }
    }
}
