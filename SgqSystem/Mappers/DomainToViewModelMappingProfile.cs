using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.Helpers;
using SgqSystem.ViewModels;
using System.Collections.Generic;

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
            #region Coleta

            /**
             * Para evitar ausencia de auto mapper.
             * Como este processo não tem custo computacional significante, 
             * podemos criar o auto mapper profile para todas as possiblidades disponíveis, 
             * independente de usarmos ela neste momento ou não.
             * */
            //Coleta para >>> ColetaViewModel, ColetaDTO
            CreateMap<GenericReturn<Coleta>, GenericReturnViewModel<ColetaViewModel>>();
            CreateMap<Coleta, ColetaViewModel>();
            CreateMap<GenericReturn<Coleta>, GenericReturnViewModel<ColetaDTO>>();
            CreateMap<Coleta, ColetaDTO>(); 

            //ColetaViewModel para >>> Coleta, ColetaDTO
            CreateMap<GenericReturn<ColetaViewModel>, GenericReturnViewModel<Coleta>>();
            CreateMap<ColetaViewModel, Coleta>(); 
            CreateMap<GenericReturn<ColetaViewModel>, GenericReturnViewModel<ColetaDTO>>();
            CreateMap<ColetaViewModel, ColetaDTO>(); 

            //ColetaDTO para >>> ColetaViewModel, Coleta
            CreateMap<GenericReturn<ColetaDTO>, GenericReturnViewModel<ColetaViewModel>>();
            CreateMap<GenericReturn<List<ColetaDTO>>, GenericReturnViewModel<List<ColetaViewModel>>>();
            CreateMap<ColetaDTO, ColetaViewModel>(); 
            CreateMap<GenericReturn<ColetaDTO>, GenericReturnViewModel<Coleta>>();
            CreateMap<ColetaDTO, Coleta>(); 

            #endregion
        }
    }
}