using DTO.DTO;
using DTO.DTO.Params;
using System.Collections.Generic;

namespace SgqSystem.ViewModels
{
    public class DefectViewModel
    {
        public List<DefectDTO> _defectsDto { get; set; }

        public int ParCompany_Id { get; set; }

    }
}
