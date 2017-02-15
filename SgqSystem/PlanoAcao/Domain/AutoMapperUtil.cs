using AutoMapper;
using PA.Domain.AutoMapper.Profiles;

namespace PA.Domain.AutoMapper
{
    public static class AutoMapperUtil
    {
        static AutoMapperUtil()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<AcompanhamentoTarefaProfile>();
                x.AddProfile<CampoProfile>();
                x.AddProfile<ConfiguracaoEmailProfile>();
                x.AddProfile<EmpresaProfile>();
                x.AddProfile<GrupoProjetoProfile>();
                x.AddProfile<MultiplaEscolhaProfile>();
                x.AddProfile<ParticipanteProfile>();
                x.AddProfile<LogOperacaoProfile>();
                x.AddProfile<ProjetoProfile>();
                x.AddProfile<TarefaProfile>();
                x.AddProfile<VinculoParticipanteMultiplaEscolhaProfile>();
                x.AddProfile<VinculoParticipanteProjetoProfile>();
                x.AddProfile<VinculoCampoTarefaProfile>();
                x.AddProfile<CabecalhoProfile>();
                x.AddProfile<GrupoCabecalhoProfile>();
                x.AddProfile<VinculoCampoCabecalhoProfile>();
            });
        }

        public static TDestination Map<TDestination>(object source)
        {
            return Mapper.Map<TDestination>(source);
        }

    }
}
