namespace PlanoAcaoCore
{
    public static class Conn
    {
        //CONFIG COMMUN PARA TODOS
        public static int sessionTimer = 60;
        public static bool isSgqIntegrado = true;


        //Config para dev SRT
        //Local Db
        //public static string catalog { get { return "PlanoDeAcao"; } }
        //public static string dataSource { get { return @"SERVERGRT\MSSQLSERVER2014"; } }
        //public static string user { get { return "sa"; } }
        //public static string pass { get { return "1qazmko0"; } }

        //Remoto SGQ
        //public static string catalog2 { get { return "dbGQualidade_JBS"; } }
        //public static string dataSource2 { get { return @"SERVERGRT\MSSQLSERVER2014"; } }
        //public static string user2 { get { return "sa"; } }
        //public static string pass2 { get { return "1qazmko0"; } }

        //public static string selfRoot { get { return "http://192.168.25.200/PlanoAcao/"; } }
        //public static string SgqHost { get { return "http://192.168.25.200/sgqbr/api/User/AuthenticationLogin"; } }

        //public static string emailFrom = "celsogea@hotmail.com";
        //public static string emailPass = "Thebost1";
        //public static string emailSmtp = "smtp.live.com";
        //public static int emailPort = 587;
        //public static bool emailSSL = true;


        ////Azure
        //Local DB
        //public static string catalog { get { return "PlanoDeAcao"; } }
        //public static string dataSource { get { return @"servergrtpa.database.windows.net"; } }
        //public static string user { get { return "grt"; } }
        //public static string pass { get { return "1qazmko0#"; } }

        //Remoto SGQ
        //public static string catalog2 { get { return "SGQ_YTOARA"; } }
        //public static string dataSource2 { get { return @"servergrt.database.windows.net"; } }
        //public static string user2 { get { return "grt"; } }
        //public static string pass2 { get { return "1qazmko0#"; } }

        //public static string selfRoot { get { return "http://server20129141.cloudapp.net/PlanoDeAcao"; } }
        //public static string SgqHost { get { return "http://server20129141.cloudapp.net"; } }

        //public static string emailFrom = "celsogea@hotmail.com";
        //public static string emailPass = "Thebost1";
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

        //Utilizar para JBS
        //DB Local
        public static string catalog { get { return "dbGQualidadeTeste"; } }
        public static string dataSource { get { return @"10.255.0.41"; } }
        public static string user { get { return "UserGQualidade"; } }
        public static string pass { get { return "grJsoluco3s"; } }
        //Remoto SGQ
        public static string catalog2 { get { return "dbGQualidade"; } }
        public static string dataSource2 { get { return @"10.255.0.16"; } }
        public static string user2 { get { return "UserGQualidade"; } }
        public static string pass2 { get { return "grJsoluco3s"; } }

        public static string selfRoot { get { return "http://mtzsvmqsc/PlanoDeAcao/"; } }
        public static string SgqHost { get { return "http://mtzsvmqsc/sgq/api/User/AuthenticationLogin"; } }


        public static string emailFrom = "sgq@jbs.com.br";
        public static string emailPass = "B7pwGJD44SbY";
        public static string emailSmtp = "correio.jbs.com.br";
        public static int emailPort = 587;
        public static bool emailSSL = false;
    }
}
