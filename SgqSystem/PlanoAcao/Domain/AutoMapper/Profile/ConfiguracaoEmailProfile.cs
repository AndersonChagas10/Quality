using AutoMapper;
using PA.DTO;
using SgqSystem.PlanoAcao.Model;

namespace PA.Domain.AutoMapper.Profiles
{
    public class ConfiguracaoEmailProfile : Profile
    {
        public ConfiguracaoEmailProfile()
        {
            CreateMap<ConfiguracaoEmailPA, ConfiguracaoEmailDTO>();
            CreateMap<ConfiguracaoEmailDTO, ConfiguracaoEmailPA>();
        }

        //public override string ProfileName
        //{
        //    get { return "ConfiguracaoEmailMapping"; }
        //}

        //protected override void Configure()
        //{
        //    AutoMapperNHibernateUtil.CreateMap<ConfiguracaoEmailPA, ConfiguracaoEmailDTO>();
        //    CreateMap<ConfiguracaoEmailDTO, ConfiguracaoEmailPA>();
        //}
    }
}
