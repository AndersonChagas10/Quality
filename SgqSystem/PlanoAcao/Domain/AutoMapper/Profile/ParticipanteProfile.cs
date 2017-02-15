using AutoMapper;
using PA.DTO;
using SgqSystem.PlanoAcao.Model;

namespace PA.Domain.AutoMapper.Profiles
{
    public class ParticipanteProfile : Profile
    {
        public ParticipanteProfile()
        {
            CreateMap<Usuarios, ParticipanteDTO>();
            CreateMap<ParticipanteDTO, Usuarios>();
        }

        //protected override void Configure()
        //{
        //    AutoMapperNHibernateUtil.CreateMap<Usuarios, ParticipanteDTO>();
        //    CreateMap<ParticipanteDTO, Usuarios>();
        //}
    }
}
