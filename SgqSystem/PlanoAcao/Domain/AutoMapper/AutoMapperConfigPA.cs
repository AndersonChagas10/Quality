using AutoMapper;

namespace SgqSystem.PlanoAcao.Domain.AutoMapper
{
    public class AutoMapperConfigPA
    {
        public static void RegisterMappings()
        {

            Mapper.Initialize(x =>
            {
                //x.AddProfile<CorrectiveActionMapperProfile>();
            });
        }
    }
}