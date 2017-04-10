using System;
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
        public string emailFrom { get; set; }
        public string emailPass { get; set; }
        public string emailSSL { get; set; }

    }

    public static class GlobalConfig
    {


        /*Sistema real time*/
        public static bool Brasil { get; set; }
        public static bool Eua { get; set; }
        public static bool Canada { get; set; }
        public static bool JBS { get; set; }
        public static bool Ytoara { get; set; }
        public static bool Guarani { get; set; }

        /*DataMenber*/
        public static int Id { get; set; } = 0;
        public static DateTime AddDate { get; set; } = DateTime.Now;
        public static DateTime? AlterDate { get; set; } = null;
        public static int ActiveIn { get; set; }
        public static bool recoveryPassAvaliable { get; set; }
        public static string urlPreffixAppColleta { get; set; }
        public static string urlAppColleta { get; set; }

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

        /// <summary>
        /// Recebe parametros do DB e Configura arquivo de config do web site.
        /// </summary>
        /// <param name="dto"></param>
        public static void ConfigWebSystem(SgqConfig dto)
        {

            SetAllFalse();
            switch (dto.ActiveIn)
            {
                case 1:
                    JBS = true;
                    Brasil = true;
                    break;
                case 2:
                    JBS = true;
                    Eua = true;
                    break;
                case 3:
                    JBS = true;
                    Canada = true;
                    break;
                case 4:
                    Ytoara = true;
                    break;
                case 5:
                    Guarani = true;
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
            JBS = false;
            Ytoara = false;
            Guarani = false;
            ActiveIn = 0;
            recoveryPassAvaliable = false;
            urlPreffixAppColleta = string.Empty;
            urlAppColleta = string.Empty;

        }

      
    }

}
