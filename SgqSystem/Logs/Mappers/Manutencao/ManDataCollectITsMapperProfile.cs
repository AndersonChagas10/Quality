using AutoMapper;
using Dominio;
using DTO.DTO.Manutencao;

namespace SgqSystem.Mappers.Manutencao
{
    public class ManDataCollectITsMapperProfile : Profile
    {
        public ManDataCollectITsMapperProfile()
        {
            CreateMap<ManDataCollectIT, DimManutencaoColetaITsDTO>();
            CreateMap<DimManutencaoColetaITsDTO, ManDataCollectIT>();
        }
    }

}