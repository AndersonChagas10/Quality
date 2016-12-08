namespace Dominio
{
    public static class GlobalConfig
    {

        public static bool Brasil { get; set; } = true;
        public static bool Eua { get; set; } = false;

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
