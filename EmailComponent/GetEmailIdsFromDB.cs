using Dominio.ADO;
using System.Collections.Generic;
using System.Linq;

namespace EmailComponent
{
    public class GetEmailIdsFromDB
    {

        public string connectionString { get; set; }

        public string getConfig
        {
            get
            {
                //MOCK
                var query = "SELECT '' AS emailLogin    " +
                            ", 'celsogea' AS emailLoginName                 " +
                            ", 'smtp.live.com' AS host                      " +
                            ", '587' AS port                                " +
                            ", 'thebost1' AS pass";

                return query;
            }
        }

        public Email GetEmailConfig()
        {
            //using (var db = new FactoryADO(connectionString))
            //{
            //    var result = db.SearchQuery<Email>(getConfig).FirstOrDefault();
            //    return result;
            //}
            var teste = new Email()
            {
                emailLogin = "celsogea@hotmail.com",
                emailLoginName = "celsogea",
                host = "smtp.live.com",
                port = 587,
                pass = "thebost1",
                ssl = false
            };

            return teste;
        }

        public List<Email> GetListMail()
        {
            var mock = MockEmailTesteAlerta();

            using (var db = new FactoryADO(connectionString))
            {
                //var result = db.SearchQuery<Email>(getConfig).FirstOrDefault();
            }

            return mock;
        }

        private List<Email> MockEmailTesteAlerta()
        {
            //USAR GUARD SE NECESsARIO PARA MONTAR LISTA DEFINITIVA.
            var _list = new List<Email>();

            _list.Add(new Email()
            {
                subject = "TESTE Subject 1",
                messageBody = "TESTE messageBody 1",
                toEmail = "celso.bernar@grtsolucoes.com.br",
                toName = "CelsoGea"
            });

            _list.Add(new Email()
            {
                subject = "TESTE Subject 2",
                messageBody = "TESTE messageBody 2",
                toEmail = "celso.bernar@grtsolucoes.com.br",
                toName = "CelsoGea"
            });

            return _list;

        }

    }
}
