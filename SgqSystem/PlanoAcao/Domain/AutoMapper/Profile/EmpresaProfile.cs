using AutoMapper;
using PA.DTO;
using SgqSystem.PlanoAcao.Model;

namespace PA.Domain.AutoMapper.Profiles
{
    public class EmpresaProfile : Profile
    {
        public EmpresaProfile()
        {
            CreateMap<EmpresaDTO, Unidades>();
            CreateMap<Unidades, EmpresaDTO>();
        }
        //public override string ProfileName
        //{
        //    get { return "EmpresaMapping"; }
        //}

        //protected override void Configure()
        //{
        //    CreateMap<EmpresaDTO, Unidades>();
        //    CreateMap<Unidades, EmpresaDTO>();

        //    //AutoMapperNHibernateUtil.CreateMap<Unidades, EmpresaDTO>();
        //    //CreateMap<EmpresaDTO, Unidades>();
        //}
    }
}
