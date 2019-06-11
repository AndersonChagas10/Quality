using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqService.Mappers
{
    public class ExampleMapperProfile : Profile
    {
        public ExampleMapperProfile()
        {
            CreateMap<ExampleDTO, Example>();
            CreateMap<Example, ExampleDTO>();
        }
    }
}