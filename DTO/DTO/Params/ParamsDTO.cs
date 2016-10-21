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
        
        public ParHeaderFieldDTO parHeaderFieldDto { get; set; }

        /*Fim Ja implementados */



        //public ParClusterDTO parClusterDto { get; set; }
        ////ParClusterDTO

        //public ParGoalDTO parGoalDto { get; set; }
        ////ParGoalDTO

        //public ParLevel1XClusterDTO parLevel1ClusterDto { get; set; }
        ////ParLevel1ClusterDTO

        //public ParClusterGroupDTO parClusterGroupDto { get; set; }
        ////ParClusterGroupDTO

        //public ParLevel1HeaderFieldDTO parLevel1HeaderFieldDto { get; set; }
        ////ParLevel1HeaderField

        //public ParFieldTypeDTO parFieldTypeDto { get; set; }

        //public ParMultipleValuesDTO parMultipleValuesDto { get; set; }
        ////ParMultipleValuesDTO


        //public ParHeaderFieldDTO parHeaderFieldDto { get; set; }
        //ParHeaderFieldDTO

        #endregion

    }
}
