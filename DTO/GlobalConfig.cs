using System.Collections.Generic;

namespace DTO
{
    public static class GlobalConfig
    {

        public static bool Brasil { get; set; } = true;
        public static bool Eua { get; set; } = false;

        public static string linkDataCollect
        {
            get
            {
                var retorno = string.Empty;
                if (Brasil)
                    retorno =  "http://mtzsvmqsc/AppColeta/";
                else if (Eua)
                    retorno =  "http://10.190.2.34/AppSgqHml/";

                return retorno;
            }
            set { linkDataCollect = value; }
        }

        public static bool corrigeLinkDataCollect { get; set; } = false;
        public static bool linkDataCollectJaConfigurado { get; set; } = false;

        public static List<string> listLinkDataCollect
        {
            get
            {
                var listLinksDataCollect = new List<string>();
                listLinksDataCollect.Add("http://192.168.25.200/AppColeta/");
                listLinksDataCollect.Add("http://mtzsvmqsc/AppColeta/");
                //listLinksDataCollect.Add("http://mtzsvmqsc/AppColeta/");
                //listLinksDataCollect.Add("http://mtzsvmqsc/AppColeta/");
                //listLinksDataCollect.Add("http://mtzsvmqsc/AppColeta/");

                return listLinksDataCollect;
            }
            set
            {
                listLinkDataCollect = value;
            }
        }

        public static string AlteraGc(int seletor)
        {
            string retorno = "";
            switch (seletor)
            {
                case 1:
                    Brasil = true;
                    Eua = false;
                    retorno = "Global config alterada para Brasil";
                    break;
                case 2:
                    Brasil = false;
                    Eua = true;
                    retorno = "Global config alterada para EUA";
                    break;
            }

            return retorno;
        }

        public static string CheckGC()
        {
            var retorno = "";

            if (Brasil)
                retorno = "Config atual: Brasil";
            else if (Eua)
                retorno = "Config atual: Eua";
            else
                retorno = "Não definida";

            return retorno;
        }
    }
}
