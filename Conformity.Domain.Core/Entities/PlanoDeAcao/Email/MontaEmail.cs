using Conformity.Domain.Core.Interfaces;

namespace Conformity.Domain.Core.Entities.PlanoDeAcao
{
    public class MontaEmail
    {
        public IEmail Email { private set; get; }

        public MontaEmail(IEmail email)
        {
            Email = email;
        }
    }
}
