using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces
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
