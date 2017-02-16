using AutoMapper;
using PA.DTO;
using SgqSystem.PlanoAcao.Model;

namespace PA.Domain.AutoMapper.Profiles
{
    public class LogOperacaoProfile : Profile
    {
        public LogOperacaoProfile()
        {
            CreateMap<LogOperacaoPA, LogOperacaoDTO>();
            CreateMap<LogOperacaoPA, LogOperacaoDTO>();
        }

        //public override string ProfileName
        //{
        //    get { return "LogOperacaoMapping"; }
        //}

        //protected override void Configure()
        //{
        //    AutoMapperNHibernateUtil.CreateMap<LogOperacaoPA, LogOperacaoDTO>();
        //    CreateMap<LogOperacaoDTO, LogOperacaoPA>();
        //}
    }
}
