using AutoMapper;
using Dominio;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqServiceBusiness.Mappers
{
    public class RotinaIntegracaoMapperProfile : Profile
    {
        public RotinaIntegracaoMapperProfile()
        {
            CreateMap<RotinaIntegracao, RotinaIntegracaoDTO>();
            CreateMap<RotinaIntegracaoDTO, RotinaIntegracao>();
        }
    }
}