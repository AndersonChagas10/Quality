using AutoMapper;
using DTO.DTO;
using PlanoAcaoCore;
using PlanoAcaoCore.Acao;

namespace SgqSystem.MapperConfig
{
    public class MapperUtil : Profile
    {
        public MapperUtil()
        {
            CreateMap<Pa_Acao, PlanoAcaoEF.Pa_Acao>();
            CreateMap<PlanoAcaoEF.Pa_Acao, Pa_Acao>();

            CreateMap<Pa_Acompanhamento, PlanoAcaoEF.Pa_Acompanhamento>();
            CreateMap<PlanoAcaoEF.Pa_Acompanhamento, Pa_Acompanhamento>();

            CreateMap<Pa_Planejamento, PlanoAcaoEF.Pa_Planejamento>();
            CreateMap<PlanoAcaoEF.Pa_Planejamento, Pa_Planejamento>();

            CreateMap<FTA, PlanoAcaoEF.Pa_FTA>();
            CreateMap<PlanoAcaoEF.Pa_FTA, FTA>();

            CreateMap<Pa_AcompanhamentoXQuemVM, PlanoAcaoEF.Pa_AcompanhamentoXQuem>();
            CreateMap<PlanoAcaoEF.Pa_AcompanhamentoXQuem, Pa_AcompanhamentoXQuemVM>();

            CreateMap<PlanoAcaoEF.EmailContent, EmailContentDTO>();
            CreateMap<EmailContentDTO, PlanoAcaoEF.EmailContent>();
        }
    }
}