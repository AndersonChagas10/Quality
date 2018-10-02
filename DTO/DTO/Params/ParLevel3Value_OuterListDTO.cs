using DTO.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO.Params
{
    public class ParLevel3Value_OuterListDTO : EntityBase
    {
        public int OuterEmpresa_Id { get; set; }
        public string OuterEmpresa_Text { get; set; }
        public int OuterLevel3_Id { get; set; }
        public string OuterLevel3_Text { get; set; }
        public int OuterLevel3Value_Id { get; set; }
        public string OuterLevel3Value_Text { get; set; }
        public int OuterLevel3ValueIntervalMinValue { get; set; }
        public int OuterLevel3ValueIntervalMaxValue { get; set; }
        public string Operator { get; set; }
        public int Order { get; set; }

        public int ParLevel3_Id { get; set; }
        public string ParLevel3_Name { get; set; }
        public int ParLevel3InputType_Id { get; set; }
        public string ParLevel3InputType_Name { get; set; }
        public int ParCompany_Id { get; set; }
        public string ParCompany_Name { get; set; }
        public int ParMeasurementUnit_Id { get; set; }
        public string ParMeasurementUnit_Name { get; set; }

        public int UnidadeMedida_Id { get; set; }
        public string UnidadeMedidaText { get; set; }
        public string AceitavelEntreText { get; set; }
        public int AceitavelEntre_Id { get; set; }
        public decimal  LimInferior { get; set; }
        public decimal LimSuperior { get; set; }

        public bool IsActive { get; set; }

    }
}
