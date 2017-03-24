using AutoMapper;
using PlanoAcaoCore;

namespace SgqSystem.MapperConfig
{
    public class AcaoMapper : Profile
    {
        public AcaoMapper()
        {
            CreateMap<Pa_Acao, PlanoAcaoEF.Pa_Acao>();
            CreateMap<PlanoAcaoEF.Pa_Acao, Pa_Acao>();
        }
    }
}