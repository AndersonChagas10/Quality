using AutoMapper;
using SgqSystem.MapperConfig;

namespace PlanoDeAcaoMVC.MapperConfig
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<AcaoMapper>();
                x.AddProfile<PlanejamentoMapper>();
                x.AddProfile<AcompanhamentoMapper>();
            });
        }
    }
}