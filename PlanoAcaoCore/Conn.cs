namespace PlanoAcaoCore
{
    public static class Conn
    {
        ////CONFIG COMMUN PARA TODOS
        public static int sessionTimer = 1800;
        public static bool isSgqIntegrado = true;
        public static bool visaoOperacional = false;
        public static string nameCausaEspecifica { get { return !visaoOperacional ? "Causa Específica" : "Assunto"; } }
        public static string nameComoPontosImportantes { get { return !visaoOperacional ? "Como Pontos Importantes" : "Como"; } }
        public static string nameContramedidaEspecifica { get { return !visaoOperacional ? "Ação Específica" : "O que"; } }
        public static string TitileMailAcompanhamento = "Plano de Ação - Atualização de Acompanhamento.";
        public static string TitileMailNovaAcao = "Plano de Ação - Nova Ação.";
        public static string TitileMailNovoFTA = "Plano de Ação - Novo Relatório de Análise de Desvio criado.";

        //////Config para dev GRT
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

        //________________________________________________________________________
        //Configurações de Email de envio

        public static string emailFrom = "testedealertagrt@hotmail.com";
        public static string emailPass = "L7e9HaN6UAsTeTxI3vtsoA==";
        public static string emailSmtp = "smtp.live.com";
        public static int emailPort = 587;
        public static bool emailSSL = true;

        //________________________________________________________________________

        //Config para GABRIEL NUNES 1
        //Local Db
        //public static string catalog { get { return "PlanoDeAcao"; } }
        //public static string dataSource { get { return @"PCRICARDOGRT"; } }
        //public static string user { get { return "sa"; } }
        //public static string pass { get { return "1qazmko0"; } }

        //////Remoto SGQ
        //public static string catalog2 { get { return "dbGQualidade"; } }
        //public static string dataSource2 { get { return @"PCRICARDOGRT"; } }
        //public static string user2 { get { return "sa"; } }
        //public static string pass2 { get { return "1qazmko0"; } }

        //public static string selfRoot { get { return "http://localhost/PlanoAcao/"; } }
        //public static string SgqHost { get { return "http://localhost/SgqSystem/api/User/AuthenticationLogin"; } }

        //________________________________________________________________________

        ////Config para GABRIEL NUNES 2
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

        //________________________________________________________________________

        //Config para GRT UTILIZAÇÃO INTERNA
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

        //________________________________________________________________________

        //RENAN SANTINI
        //Local DB
        //public static string catalog { get { return "PlanoDeAcao"; } }
        //public static string dataSource { get { return "DESKTOP-0KN2R6G"; } }
        //public static string user { get { return null; } }
        //public static string pass { get { return null; } }

        ////Remoto SGQ
        //public static string catalog2 { get { return "dbGQualidade"; } }
        //public static string dataSource2 { get { return @"SERVERGRT\MSSQLSERVER2014"; } }
        //public static string user2 { get { return "sa"; } }
        //public static string pass2 { get { return "1qazmko0"; } }

        //public static string selfRoot { get { return "http://192.168.25.200/PlanoAcao/"; } }
        //public static string SgqHost { get { return "http://192.168.25.200/sgqbr/api/User/AuthenticationLogin"; } }

        //________________________________________________________________________

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

        //________________________________________________________________________

        //Utilizar para JBS
        //DB Local
        //public static string catalog { get { return "dbGQualidadeTeste"; } }
        //public static string dataSource { get { return @"10.255.5.33"; } }
        //public static string user { get { return "UserGQualidade"; } }
        //public static string pass { get { return "grJsoluco3s"; } }
        ////Remoto SGQ
        //public static string catalog2 { get { return "dbGQualidade"; } }
        //public static string dataSource2 { get { return @"10.255.5.93"; } }
        //public static string user2 { get { return "UserGQualidade"; } }
        //public static string pass2 { get { return "grJsoluco3s"; } }

        //public static string selfRoot { get { return "http://mtzsvmqsc/PlanoDeAcao/"; } }
        //public static string selfRoot { get { return "http://mtzsvmqsc/PlanoDeAcaoNovo/"; } }
        //public static string SgqHost { get { return "http://mtzsvmqsc/sgq/api/User/AuthenticationLogin"; } }

        //public static string emailFrom = "sgq@jbs.com.br";
        //public static string emailPass = "cLospk3FD8TBBL1er9GJJg==";
        //public static string emailSmtp = "correio.jbs.com.br";
        //public static int emailPort = 587;
        //public static bool emailSSL = false;
    }
}
