using AutoMapper;
using QualidadeTotal.PlanoAcao.DTO;
using SgqSystem.PlanoAcao.Model;

namespace PA.Domain.AutoMapper.Profiles
{
    public class GrupoCabecalhoProfile : Profile
    {
        public GrupoCabecalhoProfile()
        {
            CreateMap<GrupoCabecalho, GrupoCabecalhoDTO>();
            CreateMap<GrupoCabecalhoDTO, GrupoCabecalho>();
        }
        //public override string ProfileName
        //{
        //    get { return " GrupoCabecalhoMapping"; }
        //}
        //protected override void Configure()
        //{
        //    AutoMapperNHibernateUtil.CreateMap<GrupoCabecalho, GrupoCabecalhoDTO>();
        //    CreateMap<GrupoCabecalhoDTO, GrupoCabecalho>();
        //}
    }
}