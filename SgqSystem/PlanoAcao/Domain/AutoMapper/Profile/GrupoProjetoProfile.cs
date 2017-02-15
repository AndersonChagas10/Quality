using AutoMapper;
using PA.DTO;
using SgqSystem.PlanoAcao.Model;

namespace PA.Domain.AutoMapper.Profiles
{
    public class GrupoProjetoProfile : Profile
    {
        public GrupoProjetoProfile()
        {
            CreateMap<GrupoProjeto, GrupoProjetoDTO>();
            CreateMap<GrupoProjetoDTO, GrupoProjeto>();
        }
        //public override string ProfileName
        //{
        //    get { return "GrupoProjetoMapping"; }
        //}

        //protected override void Configure()
        //{
        //    AutoMapperNHibernateUtil.CreateMap<GrupoProjeto, GrupoProjetoDTO>();
        //    CreateMap<GrupoProjetoDTO, GrupoProjeto>();
        //}
    }
}
