using AutoMapper;
using PA.DTO;
using SgqSystem.PlanoAcao.Model;

namespace PA.Domain.AutoMapper.Profiles
{
    public class AcompanhamentoTarefaProfile : Profile
    {
        public AcompanhamentoTarefaProfile()
        {
            CreateMap<AcompanhamentoTarefa, AcompanhamentoTarefaDTO>();
            CreateMap<AcompanhamentoTarefaDTO, AcompanhamentoTarefa>();
        }

        //public override string ProfileName
        //{
        //    get { return " AcompanhamentoTarefaMapping"; }
        //}

        //protected override void Configure()
        //{
        //    AutoMapperNHibernateUtil.CreateMap<AcompanhamentoTarefa, AcompanhamentoTarefaDTO>();
        //    CreateMap<AcompanhamentoTarefaDTO, AcompanhamentoTarefa>();
        //}
    }
}
