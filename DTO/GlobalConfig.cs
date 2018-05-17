﻿using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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


        /*Email*/
        public string MailFrom { get; set; }
        public string MailPass { get; set; }
        public bool MailSSL { get; set; }
        public int MailPort { get; set; }
        public string MailSmtp { get; set; }
        public bool MockEmail { get; set; }
    }
    public class Mandala
    {
        public int ParLevel1_id { get; set; }
        public string ParLevel1_name { get; set; }
        public int ParCompany_id { get; set; }
        public string ParCompany_Name { get; set; }
        public string Coletado { get; set; }
        public string Cor { get; set; }
        public string Avaliacoes_Planejadas { get; set; }
        public string Avaliacoes_Realizadas { get; set; }
        public string Amostras_Planejadas { get; set; }
        public string Amostras_Realizadas { get; set; }
    }


    public class CorrectiveAct
    {
        public int Id { get; set; }
        public string UnitId { get; set; }
        public string Unidade { get; set; }
        public string ParLevel1_Id { get; set; }
        public string Indicador { get; set; }
        public string ParLevel2_Id { get; set; }
        public string Monitoramento { get; set; }
        public string PreventativeMeasure { get; set; }
    }


    public class HtmlDoTablet
    {
        public string Html { get; set; }
        public DateTime? DataInicio { get; set; }
        public string DataInicioStr { get { return DataInicio == null ? null : DataInicio.Value.ToShortDateString() + " " + DataInicio.Value.ToShortTimeString(); } }
        public DateTime? DataFim { get; set; }
        public string DataFimStr { get { return DataFim == null ? null : DataFim.Value.ToShortDateString() + " " + DataFim.Value.ToShortTimeString(); } }
        public enum StatusType { ERROR, SUCESSO, PROCESSANDO, PENDENTE };
        public string StackTrace { get; set; }
        public StatusType Status { get; set; } = StatusType.PENDENTE;
        public string StatusStr
        {
            get
            {
                //Gambeta para funcionar o resources no controller
                var vtnc = CommonDataLocal.getResource("waiting_upper");
                var str = Resources.Resource.waiting_upper; //CommonDataLocal.getResource("waiting_upper");//
                switch (Status)
                {
                    case StatusType.ERROR:
                        str = Resources.Resource.error_upper; //CommonDataLocal.getResource("error_upper"); //
                        break;
                    case StatusType.SUCESSO:
                        str = Resources.Resource.success_upper; //CommonDataLocal.getResource("success_upper"); //
                        break;
                    case StatusType.PROCESSANDO:
                        str = Resources.Resource.processing_upper; //CommonDataLocal.getResource("processing_upper"); //
                        break;
                }
                return str.ToString();
            }
        }
    }


    public static class GlobalConfig
    {
        private static Semaphore _poolSemaphore;
        public static Semaphore PoolSemaphore { get { if (_poolSemaphore == null) _poolSemaphore = new Semaphore(5, 5); return _poolSemaphore; } }
        public static Dictionary<int, HtmlDoTablet> PaginaDoTablet { get; set; }
        public static string UrlUpdateTelaTablet { get; set; }
        public static string ParamsDisponiveis { get; set; }
        public static bool MockOn { get; set; }
        public static List<Mandala> MandalaUnidade { get; set; }
        public static List<Mandala> MandalaIndicador { get; set; }
        public static List<Mandala> MandalaMonitoramento { get; set; }
        public static List<CorrectiveAct> CorrectiveAct { get; set; }
        public static CorrectiveAct GetCorrectiveAct {get; set;}

        /*Sistema real time*/
        public static bool Brasil { get; set; } //UTILIZADO PARA SABER SE é JBS BRASIL
        public static bool Eua { get; set; }
        public static bool Canada { get; set; }
        public static bool Ytoara { get; set; }
        public static bool Guarani { get; set; }
        public static bool Santander { get; set; }


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
        public static string pathFTA { get; set; }

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

            using (var factory = new ADOFactory.Factory(connectionString))/*Caso nao configurado, procura config no DB*/
            {
                var cfg = factory.SearchQuery<SgqConfig>("SELECT * FROM SgqConfig").LastOrDefault();
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
        public static Dictionary<int, string> UsuariosUnidades { get; set; }

        public static string Ambient { get; set; }
        public enum Ambiets
        {
            Homologacao,
            Producao,
            Desenvolvimento,
            DesenvolvimentoDeployServidorGrtParaTeste
        }
        public static string UrlEmailAlertas { get; set; }

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
                case 6:
                    Santander = true;
                    //Brasil = true;
                    LanguageBrasil = true;
                    //Brasil = true;
                    Verifica += "Ambiente:  Santander\n";
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

            //pathFTA = "http://mtzsvmqsc/PlanoDeAcao/Pa_Acao/NewFTA?";
            //pathFTA = "http://localhost:59907/Pa_Acao/NewFTA?";
            pathFTA = "http://192.168.25.200/PlanoAcao/Pa_Acao/NewFTA?";
            //pathFTA = "http://192.168.25.200/PlanoAcaoUSA/Pa_Acao/NewFTA?";
            //pathFTA = "http://10.190.2.34/ActionPlanHML/Pa_Acao/NewFTA?";
            //pathFTA = "http://sgqtest.jbssa.com/actionPlanHML/Pa_Acao/NewFTA?";
            //pathFTA = "http://sgq.jbssa.com/ActionPlan/Pa_Acao/NewFTA?";

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

            //MockOn = true; // campo precisa ser adicionado no banco de dados
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
