using AutoMapper;
using PA.DTO;
using SgqSystem.PlanoAcao.Model;

namespace PA.Domain.AutoMapper.Profiles
{
    public class VinculoParticipanteProjetoProfile : Profile
    {
        public VinculoParticipanteProjetoProfile()
        {
            CreateMap<VinculoParticipanteProjeto, VinculoParticipanteProjetoDTO>();
            CreateMap<VinculoParticipanteProjetoDTO, VinculoParticipanteProjeto>();
        }

        //protected override void Configure()
        //{
        //    AutoMapperNHibernateUtil.CreateMap<VinculoParticipanteProjeto, VinculoParticipanteProjetoDTO>();
        //    CreateMap<VinculoParticipanteProjetoDTO, VinculoParticipanteProjeto>();
        //}
    }
}
