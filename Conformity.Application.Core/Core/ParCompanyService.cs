using Conformity.Domain.Core.Entities;
using Conformity.Domain.Core.Interfaces;
using Conformity.Infra.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conformity.Application.Core.Core
{
    public class ParCompanyService : BaseServiceWithLog<ParCompany>
    {
        public ParCompanyService(IRepositoryNoLazyLoad<ParCompany> repository
            , HistoricoAlteracaoService historicoAlteracaoService) 
            : base(repository
                  , historicoAlteracaoService)
        {
        }
    }
}
