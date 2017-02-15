using AutoMapper;
using PA.DTO;
using SgqSystem.PlanoAcao.Model;

namespace PA.Domain.AutoMapper.Profiles
{
    internal class VinculoParticipanteMultiplaEscolhaProfile : Profile
    {
        public VinculoParticipanteMultiplaEscolhaProfile()
        {
            CreateMap<VinculoParticipanteMultiplaEscolha, VinculoParticipanteMultiplaEscolhaDTO>();
            CreateMap<VinculoParticipanteMultiplaEscolhaDTO, VinculoParticipanteMultiplaEscolha>();
        }

        //protected override void Configure()
        //{
        //    AutoMapperNHibernateUtil.CreateMap<VinculoParticipanteMultiplaEscolha, VinculoParticipanteMultiplaEscolhaDTO>();
        //    CreateMap<VinculoParticipanteMultiplaEscolhaDTO, VinculoParticipanteMultiplaEscolha>();
        //}
    }
}
