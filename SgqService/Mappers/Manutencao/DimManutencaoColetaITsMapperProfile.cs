using AutoMapper;
using Dominio;
using DTO.DTO.Manutencao;

namespace SgqSystem.Mappers.Manutencao
{
    public class DimManutencaoColetaITsMapperProfile : Profile
    {
        public DimManutencaoColetaITsMapperProfile()
        {
            CreateMap<DimManutencaoColetaITs, DimManutencaoColetaITsDTO>();
            CreateMap<DimManutencaoColetaITsDTO, DimManutencaoColetaITs>();
        }
    }
}