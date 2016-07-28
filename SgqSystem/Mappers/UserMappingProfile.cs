using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.Helpers;
using SgqSystem.ViewModels;

namespace SgqSystem.Mappers
{
    public class UserMappingProfile : Profile
    {

        public UserMappingProfile()
        {
            CreateMap<GenericReturn<UserSgq>, GenericReturn<UserViewModel>>();
            CreateMap<UserSgq, UserViewModel>();
            CreateMap<UserSgq, UserDTO>();
            CreateMap<GenericReturn<UserDTO>, GenericReturn<UserViewModel>>();
            CreateMap<UserDTO, UserViewModel>();
            CreateMap<UserViewModel, UserDTO>();
            CreateMap<UserDTO, UserSgq>();

        }

    }
}