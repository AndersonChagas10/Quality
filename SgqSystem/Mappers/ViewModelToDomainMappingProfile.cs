using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.Helpers;
using SgqSystem.ViewModels;

namespace SgqSystem.Mappers
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            //Não utiliza mais pois o model extende o DTO.
            #region UserViewModel

            //CreateMap<GenericReturnViewModel<UserViewModel>, GenericReturn<UserSgq>>();
            CreateMap<GenericReturnViewModel<UserViewModel>, GenericReturn<UserDTO>>();
            CreateMap<UserViewModel, UserDTO>();
            //CreateMap<UserViewModel, UserSgq>()
            
            #endregion

            //Geralmente Utilizado para conversões do domain, devem seguir a regra de construtor para utilizar o principio da self validation.
            #region UserDTO

            CreateMap<UserDTO, UserSgq>();

            #endregion

            //Não utiliza mais pois o model extende o DTO.
            #region ResultOldViewModel 

            CreateMap<ResultOldViewModel, ResultOld>();
           

            #endregion
        }
    }
}