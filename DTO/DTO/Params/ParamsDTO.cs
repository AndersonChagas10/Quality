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
        public ParLevel2DTO parLevel2Dto { get; set; }

        public List<ParLevel1XClusterDTO> parLevel1XClusterDto { get; set; }
        
        public ParHeaderFieldDTO parHeaderFieldDto { get; set; }

        public List<ParHeaderFieldDTO> listParHeaderFieldDto { get; set; }
        public List<ParDepartmentDTO> listParDepartmentdDto { get; set; }
        public List<ParFrequencyDTO> listParFrequencydDto { get; set; }

        public ParLevel1XHeaderFieldDTO parLevel1HeaderFieldDto { get; set; }
        
        /*Fim Ja implementados */



        //public ParClusterDTO parClusterDto { get; set; }
        ////ParClusterDTO

        //public ParGoalDTO parGoalDto { get; set; }
        ////ParGoalDTO

        //public ParLevel1XClusterDTO parLevel1ClusterDto { get; set; }
        ////ParLevel1ClusterDTO

        //public ParClusterGroupDTO parClusterGroupDto { get; set; }
        ////ParClusterGroupDTO


        //public ParFieldTypeDTO parFieldTypeDto { get; set; }




        #endregion

    }
}
