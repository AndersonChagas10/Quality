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
                x.AddProfile<UserMappingProfile>();
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
                x.AddProfile<UnitMappingProfile>();
                x.AddProfile<UnitUserMapperProfile>();
                x.AddProfile<ParLevel1MapperProfile>();
                x.AddProfile<ParConsolidationTypeMapperProfile>();
                x.AddProfile<ParFrequencyMapperProfile>();



                /*
                 Exemplo para configuração do Auto Mapper.
                 */
                x.AddProfile<ExampleMapperProfile>();
            });

        }

    }
}