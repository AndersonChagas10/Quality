using AutoMapper;

namespace SgqSystem.Mappers
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {

            Mapper.Initialize(x =>
            {
                //    //x.CreateMap<GenericReturn<T>, GenericReturnViewModel<T>>();
                x.AddProfile<DomainToViewModelMappingProfile>();
                x.AddProfile<ViewModelToDomainMappingProfile>();
                x.AddProfile<CorrectiveActionMappingProfile>();
            });


        }

    }
}