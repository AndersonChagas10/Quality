using AutoMapper;
using Dominio.Entities;
using SgqSystem.ViewModels;

namespace SgqSystem.Mappers
{
    public class DomainToViewModelMappingProfile : Profile
    {

        //protected override void Configure()
        //{
        //    //CreateMap<DateTime, String>().ConvertUsing<StringFromDateTimeTypeConverter>();
        //}

        public DomainToViewModelMappingProfile()
        {
            CreateMap<GenericReturn<UserSgq>, GenericReturnViewModel<UserViewModel>>();
            CreateMap<UserSgq, UserViewModel>();
            CreateMap<ResultOld, ResultOldViewModel>();
        }
    }
}