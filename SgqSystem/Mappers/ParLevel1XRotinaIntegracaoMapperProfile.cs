using AutoMapper;
using Dominio;
using DTO.DTO.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqSystem.Mappers
{
    public class ParLevel1XRotinaIntegracaoMapperProfile : Profile
    {
        public ParLevel1XRotinaIntegracaoMapperProfile()
        {
            CreateMap<ParLevel1XRotinaIntegracao, ParLevel1XRotinaIntegracaoDTO>();
            CreateMap<ParLevel1XRotinaIntegracaoDTO, ParLevel1XRotinaIntegracao>();
        }
    }
}