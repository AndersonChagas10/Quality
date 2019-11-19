using AutoMapper;
using Dominio;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgqServiceBusiness.Mappers
{
    public class ParHeaderFieldXComponenteGenericoMapperProfile : Profile
    {
        public ParHeaderFieldXComponenteGenericoMapperProfile()
        {
            CreateMap<ParHeaderFieldXComponenteGenerico, ParHeaderFieldXComponenteGenericoDTO>();
            CreateMap<ParHeaderFieldXComponenteGenericoDTO, ParHeaderFieldXComponenteGenerico>();
        }
    }
}
