using AutoMapper;

namespace SgqSystem.Mappers
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {

            Mapper.Initialize(x =>
            {
                //x.AddProfile<DomainToViewModelMappingProfile>();
                x.AddProfile<CorrectiveActionMappingProfile>();
                x.AddProfile<UserMappingProfile>();
                x.AddProfile<ColetaMappingProfile>();
                x.AddProfile<CollectionLevel02MapperProfile>();
                x.AddProfile<CollectionLevel03MapperProfile>();
                x.AddProfile<ConsolidationLevel02MapperProfile>();
                x.AddProfile<ConsolidationLevel01MapperProfile>();
                x.AddProfile<Level03ConsolidationMapperProfile>();
                x.AddProfile<Level01MappingProfile>();
                x.AddProfile<DepartmenMappingProfile>();
                x.AddProfile<UnitMappingProfile>();
                x.AddProfile<UnitUserMappingProfile>();
            });

        }

    }
}