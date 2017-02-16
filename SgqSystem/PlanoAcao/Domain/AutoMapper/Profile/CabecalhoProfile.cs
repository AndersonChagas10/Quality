using AutoMapper;
using QualidadeTotal.PlanoAcao.DTO;
using SgqSystem.PlanoAcao.Model;

namespace PA.Domain.AutoMapper.Profiles
{
    public class CabecalhoProfile : Profile
    {
        public CabecalhoProfile()
        {
            CreateMap<Campo, CabecalhoDTO>();
            CreateMap<CabecalhoDTO, Campo>();
    }

        //public override string ProfileName
        //{
        //    get { return " CabecalhoMapping"; }
        //}
        //protected override void Configure()
        //{
        //    AutoMapperNHibernateUtil.CreateMap<Cabecalho, CabecalhoDTO>();
        //    CreateMap<CabecalhoDTO, Cabecalho>();
        //}
    }
}