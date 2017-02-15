using AutoMapper;
using QualidadeTotal.PlanoAcao.DTO;
using SgqSystem.PlanoAcao.Model;

namespace PA.Domain.AutoMapper.Profiles
{
    public class VinculoCampoCabecalhoProfile : Profile
    {
        public VinculoCampoCabecalhoProfile()
        {
            CreateMap<VinculoCampoCabecalho, VinculoCampoCabecalhoDTO>();
            CreateMap<VinculoCampoCabecalhoDTO, VinculoCampoCabecalho>();
        }
        //protected override void Configure()
        //{
        //    AutoMapperNHibernateUtil.CreateMap<VinculoCampoCabecalho, VinculoCampoCabecalhoDTO>();
        //    CreateMap<VinculoCampoCabecalhoDTO, VinculoCampoCabecalho>();
        //}
    }
}