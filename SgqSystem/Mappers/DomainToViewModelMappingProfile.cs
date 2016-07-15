using AutoMapper;
using Dominio.Entities;
using DTO.DTO;
using DTO.Helpers;
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

            CreateMap<GenericReturn<UserDTO>, GenericReturnViewModel<UserViewModel>>();

            CreateMap<UserSgq, UserViewModel>();
            CreateMap<UserSgq, UserDTO>();

            CreateMap<UserDTO, UserViewModel>();

            CreateMap<ResultOld, ResultOldViewModel>();
        }
    }
}