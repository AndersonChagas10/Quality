using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParamsDTO : EntityBase
    {

        #region Param Level - 1

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


        //ParLevel1HeaderFieldDTO
        //ParFieldTypeDTO
        //ParMultipleValuesDTO
        //ParConsolidationTypeDTO
        //ParHeaderFieldDTO

        #endregion

    }
}
