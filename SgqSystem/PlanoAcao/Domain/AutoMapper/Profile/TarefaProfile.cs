using AutoMapper;
using PA.DTO;
using SgqSystem.PlanoAcao.Model;

namespace PA.Domain.AutoMapper.Profiles
{
    public class TarefaProfile : Profile
    {
        public TarefaProfile()
        {
            CreateMap<TarefaPA, TarefaDTO>();
            CreateMap<TarefaDTO, TarefaPA>();
        }

        //protected override void Configure()
        //{
        //    AutoMapperNHibernateUtil.CreateMap<TarefaPA, TarefaDTO>();
        //    CreateMap<TarefaDTO, TarefaPA>();
        //}
    }
}
