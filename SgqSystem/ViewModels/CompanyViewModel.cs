using DTO.DTO.Params;
using System.Collections.Generic;

namespace SgqSystem.ViewModels
{
    public class CompanyViewModel
    {
       
        public ParCompanyDTO parCompanyDTO { get; set; }
        public ParStructureDTO parStructureDTO { get; set; }
        public ParStructureGroupDTO parStructureGroupDTO { get; set; }
        public ParCompanyXStructureDTO parCompanyXStructureDTO { get; set; }

    }
}
