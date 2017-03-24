using AutoMapper;
using PlanoAcaoCore;

namespace SgqSystem.MapperConfig
{
    public class PlanejamentoMapper : Profile
    {
        public PlanejamentoMapper()
        {
            CreateMap<Pa_Planejamento, PlanoAcaoEF.Pa_Planejamento>();
            CreateMap<PlanoAcaoEF.Pa_Planejamento, Pa_Planejamento>();
        }
    }
}