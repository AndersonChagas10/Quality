using System;
using System.Collections.Generic;
using System.Linq;

namespace DTO
{
    public class SgqConfig
    {

        public int Id { get; set; } = 0;
        public DateTime AddDate { get; set; } = DateTime.Now;
        public DateTime? AlterDate { get; set; } = null;
        public int ActiveIn { get; set; }
        public bool recoveryPassAvaliable { get; set; }
        public string urlPreffixAppColleta { get; set; }
        public string urlAppColleta { get; set; }
        public bool mockLoginEUA { get; set; }


        /*Emial*/
        public string MailFrom { get; set; }
        public string MailPass { get; set; }
        public bool MailSSL { get; set; }
        public int MailPort { get; set; }
        public string MailSmtp { get; set; }
        public bool MockEmail { get; set; }
    }


    public static class GlobalConfig
    {

        public static Dictionary<int, string> PaginaDoTablet { get; set; }
        public static string UrlUpdateTelaTablet { get; set; }
        public static string ParamsDisponiveis { get; set; }
        public static bool MockOn { get; set; }

        /*Sistema real time*/
        public static bool Brasil { get; set; } //UTILIZADO PARA SABER SE é JBS BRASIL
        public static bool Eua { get; set; }
        public static bool Canada { get; set; }
        public static bool Ytoara { get; set; }
        public static bool Guarani { get; set; }

        /*Resources manager*/
        public static bool LanguageBrasil { get; set; }
        public static bool LanguageEUA { get; set; }

        /*DataMenber*/
        public static bool mockLoginEUA { get; set; }
        public static int Id { get; set; } = 0;
        public static DateTime AddDate { get; set; } = DateTime.Now;
        public static DateTime? AlterDate { get; set; } = null;
        public static int ActiveIn { get; set; }
        public static bool recoveryPassAvaliable { get; set; }
        public static string urlPreffixAppColleta { get; set; }
        public static string urlAppColleta { get; set; }

        /*Mail*/
        public static string emailPass { get; set; }
        public static bool emailSSL { get; set; }
        public static string emailFrom { get; set; }
        public static int emailPort { get; set; }
        public static string emailSmtp { get; set; }
        public static bool mockEmail { get; set; }

        //public static List<EmailContent> EmailList { get; set; }

        public static string PrimeiraOption
        {
            get
            {
                var primeiraOption = string.Empty;
                if (Eua)
                    primeiraOption = "Select...";
                else if (Brasil)
                    primeiraOption = "Selecione...";
                return primeiraOption;
            }
        }

        public static string Vinculado
        {
            get
            {
                var primeiraOption = string.Empty;
                if (Eua)
                    primeiraOption = "Linked";
                else if (Brasil)
                    primeiraOption = "Vinculado";
                return primeiraOption;
            }
        }


        public static string NaoVinculado
        {
            get
            {
                var primeiraOption = string.Empty;
                if (Eua)
                    primeiraOption = "Unlinked";
                else if (Brasil)
                    primeiraOption = "Não Vinculados";
                return primeiraOption;
            }
        }

        /// <summary>
        /// Se existe config: true.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static bool VerifyConfig(string connectionString)
        {
            if (ActiveIn > 0)/*Se ja configurado*/
                return true;

            using (var db = new ADOFactory.Factory(connectionString))/*Caso nao configurado, procura config no DB*/
            {
                var cfg = db.SearchQuery<SgqConfig>("SELECT * FROM SgqConfig").LastOrDefault();
                if (cfg != null)/*Se existe config, pega a ultima existente e configura*/
                {
                    ConfigWebSystem(cfg);
                    if (ActiveIn > 0)
                        return true;
                }
            }


            return false;/*Se não existe config retorna falso*/
        }

        public static string Verifica { get; set; }
        /// <summary>
        /// Recebe parametros do DB e Configura arquivo de config do web site.
        /// </summary>
        /// <param name="dto"></param>
        public static void ConfigWebSystem(SgqConfig dto)
        {

            Verifica = string.Empty;
            SetAllFalse();
            switch (dto.ActiveIn)
            {
                case 1:
                    LanguageBrasil = true;
                    Brasil = true;
                    Verifica += "Ambiente:  Brasil\n";
                    break;
                case 2:
                    LanguageEUA = true;
                    Eua = true;
                    Verifica += "Ambiente:  Eua\n";
                    break;
                case 3:
                    LanguageEUA = true;
                    Canada = true;
                    Verifica += "Ambiente:  Canada\n";
                    break;
                case 4:
                    Ytoara = true;
                    //Brasil = true;
                    LanguageBrasil = true;
                    //Brasil = true;
                    Verifica += "Ambiente:  Ytoara\n";
                    break;
                case 5:
                    Guarani = true;
                    LanguageBrasil = true;
                    Verifica += "Ambiente:  Guarani\n";
                    break;
                default:
                    break;
            }

            Id = dto.Id;
            AddDate = dto.AddDate;
            AlterDate = dto.AlterDate;
            ActiveIn = dto.ActiveIn;
            recoveryPassAvaliable = dto.recoveryPassAvaliable;
            urlPreffixAppColleta = dto.urlPreffixAppColleta;
            urlAppColleta = dto.urlAppColleta;
            mockLoginEUA = dto.mockLoginEUA;
            emailFrom = dto.MailFrom;
            emailPass = dto.MailPass;
            emailSSL = dto.MailSSL;
            emailSmtp = dto.MailSmtp;
            emailPort = dto.MailPort;
            mockEmail = dto.MockEmail;

            Verifica += "recoveryPassAvaliable:  " + recoveryPassAvaliable.ToString() + "\n";
            Verifica += "urlPreffixAppColleta:  " + urlPreffixAppColleta + "\n";
            Verifica += "urlAppColleta:  " + urlAppColleta + "\n";
            Verifica += "mockLoginEUA:  " + mockLoginEUA + "\n";
            Verifica += "emailFrom:  " + emailFrom + "\n";
            Verifica += "emailPass:  " + emailPass + "\n";
            Verifica += "emailSSL:  " + emailSSL.ToString() + "\n";
            Verifica += "emailSmtp:  " + emailSmtp + "\n";
            Verifica += "emailPort:  " + emailPort.ToString() + "\n";
            Verifica += "mockEmail:  " + mockEmail.ToString() + "\n";

            Verifica += "AddDate:  " + AddDate.ToString() + "\n";
            Verifica += "AlterDate:  " + AlterDate.ToString() + "\n";
            Verifica += "Id:  " + Id.ToString() + "\n";

            MockOn = true;
        }

        /// <summary>
        /// Zera variavel de config GlobalConfig
        /// </summary>
        private static void SetAllFalse()
        {

            Id = 0;
            AddDate = DateTime.Now;
            AlterDate = null;
            Brasil = false;
            Eua = false;
            Canada = false;
            Ytoara = false;
            LanguageBrasil = false;
            LanguageEUA = false;
            Guarani = false;
            ActiveIn = 0;
            recoveryPassAvaliable = false;
            urlPreffixAppColleta = string.Empty;
            urlAppColleta = string.Empty;
            //JBS = false;

        }


    }

}
