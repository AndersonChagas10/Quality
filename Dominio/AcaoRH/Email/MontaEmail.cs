using Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.AcaoRH
{
    public class MontaEmail
    {
        private readonly IEmail _email;

        public MontaEmail(IEmail email)
        {
            _email = email;
        }

        public IEmail GetEmail()
        {
            return _email;
        }
    }
}
