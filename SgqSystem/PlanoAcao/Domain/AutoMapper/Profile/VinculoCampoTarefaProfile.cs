using AutoMapper;
using PA.DTO;
using SgqSystem.PlanoAcao.Model;

namespace PA.Domain.AutoMapper.Profiles
{
    public class VinculoCampoTarefaProfile : Profile
    {
        public VinculoCampoTarefaProfile()
        {
            CreateMap<VinculoCampoTarefa, VinculoCampoTarefaDTO>();
            CreateMap<VinculoCampoTarefaDTO, VinculoCampoTarefa>();
        }

        //protected override void Configure()
        //{
        //    AutoMapperNHibernateUtil.CreateMap<VinculoCampoTarefa, VinculoCampoTarefaDTO>();
        //    CreateMap<VinculoCampoTarefaDTO, VinculoCampoTarefa>();
        //}
    }
}
