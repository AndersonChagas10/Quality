using AutoMapper;
using PA.DTO;
using SgqSystem.PlanoAcao.Model;

namespace PA.Domain.AutoMapper.Profiles
{

    public class CampoProfile : Profile
    {
        public CampoProfile()
        {
            CreateMap<Campo, CampoDTO>();
            CreateMap<CampoDTO, Campo>();
        }
        //public override string ProfileName
        //{
        //    get { return "CampoMapping"; }
        //}

        //protected override void Configure()
        //{
        //    AutoMapperNHibernateUtil.CreateMap<Campo, CampoDTO>();
        //    CreateMap<CampoDTO, Campo>();
        //}
    }

}
