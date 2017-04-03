using AutoMapper;
using Dominio;
using DTO.DTO;
using Helper;

namespace SgqSystem.Mappers
{
    public class EmailContentMapperProfile : Profile
    {
        public EmailContentMapperProfile()
        {
            CreateMap<EmailContent, EmailContentDTO>();
            CreateMap<EmailContentDTO, EmailContent>();
        }
    }
}