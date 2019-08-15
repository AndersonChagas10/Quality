using System;

namespace SgqServiceBusiness.Api.Login
{
    public class LoginController
    {
        public string Logado(DateTime? dataApp = null)
        {
            if (dataApp != null)
            {
                var dataServer = DateTime.Now;

                if (dataApp < dataServer.AddHours(-30))
                {
                    return "dataInvalida";
                }
            }

            return "onLine";
        }
    }
}
