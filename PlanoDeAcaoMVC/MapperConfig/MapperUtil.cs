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
            CreateMap<Pa_Acao, Dominio.Pa_Acao>();
            CreateMap<Dominio.Pa_Acao, Pa_Acao>();

            CreateMap<Pa_Acompanhamento, Dominio.Pa_Acompanhamento>();
            CreateMap<Dominio.Pa_Acompanhamento, Pa_Acompanhamento>();

            CreateMap<Pa_Planejamento, Dominio.Pa_Planejamento>();
            CreateMap<Dominio.Pa_Planejamento, Pa_Planejamento>();

            CreateMap<FTA, Dominio.Pa_FTA>();
            CreateMap<Dominio.Pa_FTA, FTA>();

            CreateMap<Pa_AcompanhamentoXQuemVM, Dominio.Pa_AcompanhamentoXQuem>();
            CreateMap<Dominio.Pa_AcompanhamentoXQuem, Pa_AcompanhamentoXQuemVM>();

            CreateMap<Dominio.EmailContent, EmailContentDTO>();
            CreateMap<EmailContentDTO, Dominio.EmailContent>();
        }
    }
}