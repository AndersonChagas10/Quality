using DTO.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO.Params
{
    public class ParLevel1XRotinaIntegracaoDTO : EntityBase
    {

        public int? ParLevel1_Id { get; set; }
        public int? RotinaIntegracao_Id { get; set; }
        public int ParLevelDefinition_Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public ParLevelDefinitonDTO ParLevelDefinition { get; set; }
        public RotinaIntegracaoDTO RotinaIntegracao { get; set; }
        public ParLevel1DTO ParLevel1 { get; set; }
    }
}
