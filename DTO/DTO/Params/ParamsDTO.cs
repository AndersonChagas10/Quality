using DTO.BaseEntity;
using System.Collections.Generic;

namespace DTO.DTO.Params
{
    public class ParamsDTO : EntityBase
    {

        #region Param Level - 1

        public int parLevel1Selected { get; set; }

        public int parLevel2Selected { get; set; }

        public int parLevel3Selected { get; set; }

        /*Ja implementados*/

        public ParLevel1DTO parLevel1Dto { get; set; }
        public List<ParLevel1XClusterDTO> parLevel1XClusterDto { get; set; }
        public List<ParHeaderFieldDTO> listParHeaderFieldDto { get; set; }
        public List<ParFrequencyDTO> listParFrequencydDto { get; set; }

        public ParHeaderFieldDTO parHeaderFieldDto { get; set; }
        public ParLevel1XHeaderFieldDTO parLevel1HeaderFieldDto { get; set; }
        
        /*Fim Ja implementados */

        #endregion

        public List<ParDepartmentDTO> listParDepartmentdDto { get; set; }

        public ParLevel2DTO parLevel2Dto { get; set; }

        public List<ParLevel3GroupDTO> listParLevel3GroupDto { get; set; }

        public ParLocalDTO parLocalDto { get; set; }
        public ParCounterDTO parCounterDto { get; set; }
        public ParCounterXLocalDTO parCounterXLocalDto { get; set; }
        public ParRelapseDTO parRelapseDto { get; set; }
        public ParNotConformityRuleDTO parNotConformityRuleDto { get; set; }
        public ParNotConformityRuleXLevelDTO parNotConformityRuleXLevelDto { get; set; }

        public ParCompanyDTO parCompanyDto { get; set; }

    }
}
