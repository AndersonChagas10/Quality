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
            MailSenderBusiness.SendMail(GlobalConfig.emailFrom, 
            GlobalConfig.emailPass,
            GlobalConfig.emailSmtp,
            GlobalConfig.emailPort, 
            GlobalConfig.emailSSL, 
            string.Join(",", email.GetEmail().To), 
            email.GetEmail().Subject, 
            email.GetEmail().Body));
        }
    }
}
