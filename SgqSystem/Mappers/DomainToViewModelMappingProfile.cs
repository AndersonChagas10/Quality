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
            CreateMap<GenericReturn<User>, GenericReturnViewModel<UserViewModel>>();
            CreateMap<User, UserViewModel>();
            CreateMap<ResultOld, ResultOldViewModel>();
        }
    }
}