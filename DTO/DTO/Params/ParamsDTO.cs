using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParamsDTO : EntityBase
    {

        #region Param Level - 1

        public int parLevel1Selected { get; set; }

        public int parLevel2Selected { get; set; }

        public int parLevel3Selected { get; set; }

        public ParLevel1DTO parLevel1Dto { get; set; }
        //ParLevel1DTO

        public ParClusterDTO parClusterDto { get; set; }
        //ParClusterDTO

        public ParGoalDTO parGoalDto { get; set; }
        //ParGoalDTO

        public ParLevel1ClusterDTO parLevel1ClusterDto { get; set; }
        //ParLevel1ClusterDTO

        public ParClusterGroupDTO parClusterGroupDto { get; set; }
        //ParClusterGroupDTO


        public ParLevel1HeaderFieldDTO parLevel1HeaderFieldDto { get; set; }
        //ParLevel1HeaderField

        //ParFieldTypeDTO
        //ParMultipleValuesDTO
        //ParConsolidationTypeDTO

        public ParHeaderFieldDTO parHeaderFieldDto { get; set; }
        //ParHeaderFieldDTO

        #endregion

    }
}
