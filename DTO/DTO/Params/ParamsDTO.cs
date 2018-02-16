﻿using DTO.BaseEntity;
using System.Collections.Generic;

namespace DTO.DTO.Params
{
    public class ParamsDTO : EntityBase
    {

        public int level1Selected { get; set; }

        public int level2Selected { get; set; }

        public int level3Selected { get; set; }

        #region Param Level - 1

        public int parLevel1Selected { get; set; }

        public int parLevel2Selected { get; set; }

        public int parLevel3Selected { get; set; }


        /*Ja implementados*/

        public ParLevel1DTO parLevel1Dto { get; set; }
        public List<ParLevel1XClusterDTO> parLevel1XClusterDto { get; set; }
        public List<ParHeaderFieldDTO> listParHeaderFieldDto { get; set; }
        public List<ParFrequencyDTO> listParFrequencydDto { get; set; }
        public List<ParCounterXLocalDTO> listParCounterXLocal { get; set; }
        public ParHeaderFieldDTO parHeaderFieldDto { get; set; }
        public ParLevel1XHeaderFieldDTO parLevel1HeaderFieldDto { get; set; }
        public ParLevel2XHeaderFieldDTO parLevel2HeaderFieldDto { get; set; }

        /*Fim Ja implementados */


        #endregion

        #region Organizar.
        public List<ParDepartmentDTO> listParDepartmentdDto { get; set; }

        public ParLevel2DTO parLevel2Dto { get; set; }

        public List<ParLevel3GroupDTO> listParLevel3GroupDto { get; set; }

        public List<ParLocalDTO> listParLocalDto { get; set; }
        public List<ParCounterDTO> listParCounterDto { get; set; }
        public List<ParCounterXLocalDTO> listParCounterXLocalDto { get; set; }
        public List<ParRelapseDTO> listParRelapseDto { get; set; }
        public ParNotConformityRuleDTO parNotConformityRuleDto { get; set; }
        public ParNotConformityRuleXLevelDTO parNotConformityRuleXLevelDto { get; set; }

        public List<ParNotConformityRuleDTO> listParNotConformitRule { get; set; }
        

        public ParCompanyDTO parCompanyDto { get; set; }

        
        
        public ParEvaluationDTO parEvaluationDto { get; set; }
        public ParSampleDTO parSampleDto { get; set; }

        public ParLevel3DTO parLevel3Dto { get; set; }
        public ParLevel3ValueDTO parLevel3Value { get; set; }

        public ParLevel3Level2DTO parLevel3Level2 { get; set; }

        #endregion

        #region Collection

        public List<ParLevel1DTO> collectionObject { get; set; }

        #endregion

    }
}
