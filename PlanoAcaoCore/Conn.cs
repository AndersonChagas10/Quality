namespace PlanoAcaoCore
{
    public static class Conn
    {
        //CONFIG COMMUN PARA TODOS
        public static int sessionTimer = 60;
        public static bool isSgqIntegrado = true;
        public static bool visaoOperacional = false;
        public static string nameCausaEspecifica { get { return !visaoOperacional ? "Causa Especifica" : "Assunto"; } }
        public static string nameComoPontosImportantes { get { return !visaoOperacional ? "Como Pontos Importantes" : "Como"; } }
        public static string nameContramedidaEspecifica { get { return !visaoOperacional ? "Ação Específica" : "O que"; } }
        public static string TitileMailAcompanhamento = "Plano de Ação - Atualização de Acompanhamento.";
        public static string TitileMailNovaAcao = "Plano de Ação - Nova Ação.";
        public static string TitileMailNovoFTA = "Plano de Ação - Novo Relatório de Análise de Desvio criado.";

        ////////Config para dev GRT
        ////Local Db
        public static string catalog { get { return "PlanoDeAcao"; } }
        public static string dataSource { get { return @"SERVERGRT\MSSQLSERVER2014"; } }
        public static string user { get { return "sa"; } }
        public static string pass { get { return "1qazmko0"; } }

        //Remoto SGQ
        public static string catalog2 { get { return "dbGQualidade"; } }
        public static string dataSource2 { get { return @"SERVERGRT\MSSQLSERVER2014"; } }
        public static string user2 { get { return "sa"; } }
        public static string pass2 { get { return "1qazmko0"; } }

        public static string selfRoot { get { return "http://192.168.25.200/PlanoAcao/"; } }
        public static string SgqHost { get { return "http://192.168.25.200/sgqbr/api/User/AuthenticationLogin"; } }

        //Config para localhost
        //Local Db
        //public static string catalog { get { return "PlanoDeAcao"; } }
        //public static string dataSource { get { return @"SERVERGRT\MSSQLSERVER2014"; } }
        //public static string user { get { return "sa"; } }
        //public static string pass { get { return "1qazmko0"; } }
        //public static string catalog { get { return "PlanoDeAcao"; } }
        //public static string dataSource { get { return @"DESKTOP-6M17EOF"; } }
        //public static string user { get { return "sa"; } }
        //public static string pass { get { return "betsy1"; } }

        ////Remoto SGQ
        //public static string catalog2 { get { return "dbGQualidade"; } }
        //public static string dataSource2 { get { return @"SERVERGRT\MSSQLSERVER2014"; } }
        //public static string user2 { get { return "sa"; } }
        //public static string pass2 { get { return "1qazmko0"; } }

        //public static string selfRoot { get { return "http://192.168.25.200/PlanoAcao/"; } }
        //public static string SgqHost { get { return "http://192.168.25.200/sgqbr/api/User/AuthenticationLogin"; } }

        ////Config para localhost
        ////Local Db
        //public static string catalog { get { return "PlanoDeAcao"; } }
        //public static string dataSource { get { return @"DELLGABRIEL\MSSQL2014"; } }
        //public static string user { get { return "sa"; } }
        //public static string pass { get { return "betsy1"; } }

        ////Remoto SGQ
        //public static string catalog2 { get { return "dbGQualidade_JBS"; } }
        //public static string dataSource2 { get { return @"DELLGABRIEL\MSSQL2014"; } }
        //public static string user2 { get { return "sa"; } }
        //public static string pass2 { get { return "betsy1"; } }
        //Remoto SGQ
        //public static string catalog2 { get { return "dbGQualidade"; } }
        //public static string dataSource2 { get { return @"DESKTOP-6M17EOF"; } }
        //public static string user2 { get { return "sa"; } }
        //public static string pass2 { get { return "betsy1"; } }

        //public static string selfRoot { get { return "http://localhost/PlanoAcao/"; } }
        //public static string SgqHost { get { return "http://localhost/SgqSystem/api/User/AuthenticationLogin"; } }

        public static string emailFrom = "celsogea@hotmail.com";
        public static string emailPass = "tR48MJsfaz1Rf+dT+Ag8dQ==";
        public static string emailSmtp = "smtp.live.com";
        public static int emailPort = 587;
        public static bool emailSSL = true;

        //Config para dev GRT UTILIZAÇÃO LOCAL
        //Local Db GRT UTILIZAÇÃO LOCAL
        //public static string catalog { get { return "PlanoDeAcao_GRT"; } }
        //public static string dataSource { get { return @"SERVERGRT\MSSQLSERVER2014"; } }
        //public static string user { get { return "sa"; } }
        //public static string pass { get { return "1qazmko0"; } }

        ////Remoto SGQ GRT UTILIZAÇÃO LOCAL
        //public static string catalog2 { get { return "dbGQualidade_GRT"; } }
        //public static string dataSource2 { get { return @"SERVERGRT\MSSQLSERVER2014"; } }
        //public static string user2 { get { return "sa"; } }
        //public static string pass2 { get { return "1qazmko0"; } }

        //public static string selfRoot { get { return "http://192.168.25.200/PlanoAcaoGRT/"; } }
        //public static string SgqHost { get { return "http://192.168.25.200/sgqgrt/api/User/AuthenticationLogin"; } }

        //public static string emailFrom = "celsogea@hotmail.com";
        //public static string emailPass = "tR48MJsfaz1Rf+dT+Ag8dQ==";
        //public static string emailSmtp = "smtp.live.com";
        //public static int emailPort = 587;
        //public static bool emailSSL = true;


        ////Azure
        ////Local DB
        //public static string catalog { get { return "PlanoDeAcao"; } }
        //public static string dataSource { get { return @"servergrtpa.database.windows.net"; } }
        //public static string user { get { return "grt"; } }
        //public static string pass { get { return "1qazmko0#"; } }

        ////Remoto SGQ
        //public static string catalog2 { get { return "SGQ_YTOARA"; } }
        //public static string dataSource2 { get { return @"servergrt.database.windows.net"; } }
        //public static string user2 { get { return "grt"; } }
        //public static string pass2 { get { return "1qazmko0#"; } }

        //public static string selfRoot { get { return "http://server20129141.cloudapp.net/PlanoDeAcao"; } }
        //public static string SgqHost { get { return "http://server20129141.cloudapp.net/SGQYtoara/api/User/AuthenticationLogin"; } }

        //public static string emailFrom = "celsogea@hotmail.com";
        //public static string emailPass = "tR48MJsfaz1Rf+dT+Ag8dQ==";
        //public static string emailSmtp = "smtp.live.com";
        //public static int emailPort = 587;
        //public static bool emailSSL = true;

        ////Azure GLOBAL
        ////Local DB
        //public static string catalog { get { return "PlanoDeAcao"; } }
        //public static string dataSource { get { return @"servergrtpa.database.windows.net"; } }
        //public static string user { get { return "grt"; } }
        //public static string pass { get { return "1qazmko0#"; } }

        ////Remoto SGQ
        //public static string catalog2 { get { return "SGQ_YTOARA"; } }
        //public static string dataSource2 { get { return @"servergrt.database.windows.net"; } }
        //public static string user2 { get { return "grt"; } }
        //public static string pass2 { get { return "1qazmko0#"; } }

        //public static string selfRoot { get { return "http://server20129141.cloudapp.net/PlanoDeAcao"; } }
        //public static string SgqHost { get { return "http://server20129141.cloudapp.net/SGQGlobal/api/User/AuthenticationLogin"; } }

        //public static string emailFrom = "celsogea@hotmail.com";
        //public static string emailPass = "tR48MJsfaz1Rf+dT+Ag8dQ==";
        //public static string emailSmtp = "smtp.live.com";
        //public static int emailPort = 587;
        //public static bool emailSSL = true;

        ////grj
        //public static string catalog { get { return "grjqualidadedev"; } }
        //public static string dataSource { get { return @"mssql1.gear.host"; } }
        //public static string user { get { return "grjqualidadedev"; } }
        //public static string pass { get { return "Mi3UpU0J35<_"; } }
        ////grj
        //public static string catalog2 { get { return "grjqualidadedev"; } }
        //public static string dataSource2 { get { return @"mssql1.gear.host"; } }
        //public static string user2 { get { return "grjqualidadedev"; } }
        //public static string pass2 { get { return "Mi3UpU0J35<_"; } }

        ////Utilizar para JBS
        ////DB Local
        //public static string catalog { get { return "dbGQualidadeTeste"; } }
        //public static string dataSource { get { return @"10.255.0.41"; } }
        //public static string user { get { return "UserGQualidade"; } }
        //public static string pass { get { return "grJsoluco3s"; } }
        //////Remoto SGQ
        //public static string catalog2 { get { return "dbGQualidade"; } }
        //public static string dataSource2 { get { return @"10.255.5.93"; } }
        //public static string user2 { get { return "UserGQualidade"; } }
        //public static string pass2 { get { return "grJsoluco3s"; } }

        //public static string selfRoot { get { return "http://mtzsvmqsc/PlanoDeAcao/"; } }
        //public static string SgqHost { get { return "http://mtzsvmqsc/sgq/api/User/AuthenticationLogin"; } }

        //public static string emailFrom = "sgq@jbs.com.br";
        //public static string emailPass = "Bvkw+iUcPJGZQe0tPxQDjg==";
        //public static string emailSmtp = "correio.jbs.com.br";
        //public static int emailPort = 587;
        //public static bool emailSSL = false;
    }
}
