using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqSystem.Mappers
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