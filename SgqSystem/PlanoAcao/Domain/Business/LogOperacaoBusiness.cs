using PA.Data;
using SgqSystem.PlanoAcao.Model;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace PA.Domain.Business
{
    public class LogOperacaoBusiness
    {

        #region PROPRIEDADES
        //private readonly LogOperacaoData _logOperacaoData;
        //#endregion

        //#region CONSTRUTOR
        //public LogOperacaoBusiness(LogOperacaoData logOperacaoData)
        //{
        //    _logOperacaoData = logOperacaoData;
        //}
        #endregion

        #region LOG OPERAÇÃO

        public void SalvarLogOperacao(int IdUsuario, string nomeMetodo, Exception e)
        {
            LogOperacaoData _logOperacaoData = new LogOperacaoData();
            try
            {
                #region LAN IP ADRESS
                IPHostEntry host;
                string localIP = "";
                host = Dns.GetHostEntry(Dns.GetHostName());

                foreach (IPAddress ip in host.AddressList)
                {
                    string[] temp = ip.ToString().Split('.');

                    if (ip.AddressFamily == AddressFamily.InterNetwork && temp[0] == "192")
                    {
                        localIP = ip.ToString();
                    }
                    else
                    {
                        localIP = "";
                    }
                }

                #endregion

                #region Internet IP Adress

                string ipAddress = System.Web.HttpContext.Current.Request.UserHostAddress;

                #endregion

                #region Url Tela

                string url = System.Web.HttpContext.Current.Request.Url.ToString();

                #endregion

                #region Linha
                int sLinha;


                var st = new StackTrace(e, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                sLinha = frame.GetFileLineNumber();

                #endregion

                string complemento = "Class:" + e.TargetSite.ReflectedType.Name.ToString() + "     Method:" + e.TargetSite.Name.ToString();

                string MensagemExcecao = ("Mensagem:" + e.Message + "   " + complemento + "   InnerException:" + (e.InnerException == null ? string.Empty : e.InnerException.ToString()));

                LogOperacaoPA log = new LogOperacaoPA()
                {
                    Id = 0,
                    DescricaoInternetIp = ipAddress,
                    DescricaoLanIp = localIP,
                    NomeUsuario = "",
                    MensagemOperacao = e.Message,
                    TextoExcecao = MensagemExcecao,
                    TextoPilhaExcecao = "",
                    UrlTela = url,
                    DataOcorrencia = DateTime.Now,
                    Linha = sLinha,
                    NomeMetodo = string.IsNullOrEmpty(complemento) ? nomeMetodo : complemento,
                    TipoRegistro = 0,
                };

                _logOperacaoData.SalvarLogOperacao(log);

                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        #endregion
    }
}
