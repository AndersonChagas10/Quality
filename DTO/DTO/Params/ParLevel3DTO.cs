using DTO.BaseEntity;
using System.Collections.Generic;

namespace DTO.DTO.Params
{
    public class ParLevel3DTO : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;

        public decimal pesoDoVinculo { get; set; }
        public virtual List<ParLevel3ValueDTO> ParLevel3Value { get; set; }

        public List<ParRelapseDTO> listParRelapseDto { get; set; }
        public List<int> removeReincidencia { get; set; }
    }
}