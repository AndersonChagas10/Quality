using AutoMapper;

namespace SgqSystem.Mappers
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {

            Mapper.Initialize(x =>
            {
                x.AddProfile<CorrectiveActionMapperProfile>();
                x.AddProfile<UserMapperProfile>();
                x.AddProfile<ConsolidationLevel01MapperProfile>();
                x.AddProfile<ConsolidationLevel02MapperProfile>();
                x.AddProfile<CollectionLevel02MapperProfile>();
                x.AddProfile<CollectionLevel03MapperProfile>();
                x.AddProfile<Level03MapperProfile>();
                x.AddProfile<Level01MapperProfile>();
                x.AddProfile<Level02MapperProfile>();
                x.AddProfile<Level03MapperProfile>();
                x.AddProfile<PeriodMapperProfile>();
                //x.AddProfile<ShiftMapperProfile>(); PROBLEMA NA TABELA, FORA DE PADRão, MOCKADO.
                x.AddProfile<DepartmenMapperProfile>();
                x.AddProfile<UnitMapperProfile>();
                x.AddProfile<UnitUserMapperProfile>();
                

                /*
                 * Par Level1 Mapping
                 */
                x.AddProfile<ParLevel1MapperProfile>();
                x.AddProfile<ParConsolidationTypeMapperProfile>();
                x.AddProfile<ParFrequencyMapperProfile>();
                x.AddProfile<ParClusterMapperProfile>();
                x.AddProfile<ParLevel1XClusterMapperProfile>();
                x.AddProfile<ParHeaderFieldMapperProfile>();
                x.AddProfile<ParLevelDefinitonMapperProfile>();
                x.AddProfile<ParFieldTypeMapperProfile>();
                x.AddProfile<ParDepartmentMapperProfile>();
                x.AddProfile<ParLevelDefinitonMapperProfile>();
                x.AddProfile<ParMultipleValuesMapperProfile>();

                /*
                * Par Level2 Mapping
                */
                x.AddProfile<ParLevel2MapperProfile>();
                x.AddProfile<ParLevel3GroupMapperProfile>();
                x.AddProfile<ParLocalMapperProfile>();
                x.AddProfile<ParCounterMapperProfile>();
                x.AddProfile<ParCounterLocalMapperProfile>();
                x.AddProfile<ParRelapseMapperProfile>();
                x.AddProfile<ParNotConformityRuleMapperProfile>();
                x.AddProfile<ParNotConformityRuleXLevelMapperProfile>();
                x.AddProfile<ParCompanyMapperProfile>();
                x.AddProfile<ParLevel3MapperProfile>();
                x.AddProfile<ParEvaluationMapperProfile>();
                x.AddProfile<ParSampleMapperProfile>();

                /*
                 Exemplo para configuração do Auto Mapper.
                 */
                x.AddProfile<ExampleMapperProfile>();
            });

        }

    }
}