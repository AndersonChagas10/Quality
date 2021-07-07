using Conformity.Domain.Core.Interfaces;

namespace Conformity.Domain.Core.Entities.PlanoDeAcao
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
