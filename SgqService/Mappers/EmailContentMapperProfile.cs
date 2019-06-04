using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqService.Mappers
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