using Dominio.AcaoRH;
using DTO;
using Helper;
using System.Threading.Tasks;

namespace SgqServiceBusiness.Controllers.RH
{
    public static class EmailAcaoService
    {
        public static void Send(MontaEmail email)
        {
            Task.Run(() =>
            MailSenderBusiness.SendMail(
                (DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.emailFrom as string),
                (DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.emailPass as string),
                (DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.emailSmtp as string),
                int.Parse(DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.emailPort as string),
                bool.Parse(DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.emailSSL as string),
                string.Join(",", email.GetEmail().To),
                email.GetEmail().Subject,
                email.GetEmail().Body));
        }
    }
}
