using AutoMapper;
using PA.DTO;
using SgqSystem.PlanoAcao.Model;

namespace PA.Domain.AutoMapper.Profiles
{
    public class ProjetoProfile : Profile
    {
        public ProjetoProfile()
        {
            CreateMap<Projeto, ProjetoDTO>();
            CreateMap<ProjetoDTO, Projeto>();
        }

        //protected override void Configure()
        //{
        //    AutoMapperNHibernateUtil.CreateMap<Projeto, ProjetoDTO>();
        //    CreateMap<ProjetoDTO, Projeto>();
        //}
    }
}
