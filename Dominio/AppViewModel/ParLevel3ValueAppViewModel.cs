using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.AppViewModel
{
    public class ParLevel3ValueAppViewModel
    {
        public int Id { get; set; }

        public int ParLevel3_Id { get; set; }

        public int ParLevel3InputType_Id { get; set; }

        public int? ParLevel3BoolFalse_Id { get; set; }

        public int? ParLevel3BoolTrue_Id { get; set; }

        public int? ParCompany_Id { get; set; }

        public int? ParMeasurementUnit_Id { get; set; }

        public bool? AcceptableValueBetween { get; set; }

        public decimal? IntervalMin { get; set; }

        public decimal? IntervalMax { get; set; }

        public bool IsActive { get; set; }

        public int? ParLevel1_Id { get; set; }

        public int? ParLevel2_Id { get; set; }

        public string DynamicValue { get; set; }

        public bool ShowLevel3Limits { get; set; }
    }
}
