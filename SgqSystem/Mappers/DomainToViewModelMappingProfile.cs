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
            //Modo 1
            #region UserSgq

            CreateMap<GenericReturn<UserSgq>, GenericReturnViewModel<UserViewModel>>();
            CreateMap<UserSgq, UserViewModel>();
            CreateMap<UserSgq, UserDTO>();

            #endregion

            //Modo 2
            #region UserDTO

            CreateMap<GenericReturn<UserDTO>, GenericReturnViewModel<UserViewModel>>();
            CreateMap<UserDTO, UserViewModel>();

            #endregion

            //Modo 3 (preferencial)
            #region ResultOld

            /**
             * Para evitar ausencia de auto mapper.
             * Como este processo não tem custo computacional significante, 
             * podemos criar o auto mapper profile para todas as possiblidades disponíveis, 
             * independente de usarmos ela neste momento ou não.
             * */
            //ResultOld para >>> ResultOldViewModel, ResultOldDTO
            CreateMap<GenericReturn<ResultOld>, GenericReturnViewModel<ResultOldViewModel>>();
            CreateMap<ResultOld, ResultOldViewModel>();
            CreateMap<GenericReturn<ResultOld>, GenericReturnViewModel<ResultOldDTO>>();
            CreateMap<ResultOld, ResultOldDTO>(); 

            //ResultOldViewModel para >>> ResultOld, ResultOldDTO
            CreateMap<GenericReturn<ResultOldViewModel>, GenericReturnViewModel<ResultOld>>();
            CreateMap<ResultOldViewModel, ResultOld>(); 
            CreateMap<GenericReturn<ResultOldViewModel>, GenericReturnViewModel<ResultOldDTO>>();
            CreateMap<ResultOldViewModel, ResultOldDTO>(); 

            //ResultOldDTO para >>> ResultOldViewModel, ResultOld
            CreateMap<GenericReturn<ResultOldDTO>, GenericReturnViewModel<ResultOldViewModel>>();
            CreateMap<ResultOldDTO, ResultOldViewModel>(); 
            CreateMap<GenericReturn<ResultOldDTO>, GenericReturnViewModel<ResultOld>>();
            CreateMap<ResultOldDTO, ResultOld>(); 

            #endregion
        }
    }
}