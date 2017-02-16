using AutoMapper;
using Dominio;
using PA.DTO;

namespace PA.Domain.AutoMapper.Profiles
{
    public class MultiplaEscolhaProfile : Profile
    {
        public MultiplaEscolhaProfile()
        {
            CreateMap<MultiplaEscolha, MultiplaEscolhaDTO>();
            CreateMap<MultiplaEscolhaDTO, MultiplaEscolha>();
        }

        //public override string ProfileName
        //{
        //    get { return "MultiplaEscolhaMapping"; }
        //}

        //protected override void Configure()
        //{
        //    AutoMapperNHibernateUtil.CreateMap<MultiplaEscolha, MultiplaEscolhaDTO>();
        //    CreateMap<MultiplaEscolhaDTO, MultiplaEscolha>();
        //}
    }
}
