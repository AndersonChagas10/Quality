using Conformity.Domain.Core.Entities.PlanoDeAcao;
using System.Collections.Generic;

namespace Conformity.Domain.Core.Interfaces
{
    public interface IEmail
    {
        string Body { get; set; }
        string Subject { get; set; }
        IEnumerable<string> To { get; set; }
        void MontarBody(Acao acao);
        void MontarSybject(Acao acao);
        void MontarTo(Acao acao);
    }
}
