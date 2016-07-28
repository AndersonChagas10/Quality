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
          
            #region Level1
            CreateMap<Level1, Level1DTO>();
            #endregion

            #region Level1
            CreateMap<Level2, Level2DTO>();
            #endregion

            #region Level3
            CreateMap<Level3, Level3DTO>();
            #endregion

            #region Sync
            CreateMap<SyncDTO, SyncViewModel>();
            #endregion

            #region Sync
            CreateMap<GenericReturn<SyncDTO>, GenericReturn<SyncViewModel>>();
            #endregion
        }
    }
}