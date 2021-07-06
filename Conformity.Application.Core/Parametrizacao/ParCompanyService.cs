using Conformity.Application.Core.Log;
using Conformity.Domain.Core.Entities.Parametrizacao;
using Conformity.Domain.Core.Interfaces;

namespace Conformity.Application.Core.Parametrizacao
{
    public class ParCompanyService : BaseServiceWithLog<ParCompany>
    {
        public ParCompanyService(IRepositoryNoLazyLoad<ParCompany> repository
            , EntityTrackService historicoAlteracaoService) 
            : base(repository
                  , historicoAlteracaoService)
        {
        }
    }
}
