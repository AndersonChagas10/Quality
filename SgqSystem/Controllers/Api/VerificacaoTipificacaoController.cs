using Dominio;
using DTO.Helpers;
using SgqSystem.Handlres;
using SgqSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Api
{
    public enum alertaTipo
    {
        success = 1,
        info = 2,
        warning = 3,
        danger = 4
    }


    [AllowAnonymous]
    [RoutePrefix("api/VerificacaoTipificacao")]
    public class VerificacaoTipificacaoController : Controller
    {

        public string connectionString(Unidades unidades, string url = null)
        {
            try
            {
                string urlHost = url;
                if (Request != null)
                {
                    urlHost = Request.Url.Host.ToString();
                }
                
                string _user = "UserGQualidade";
                string _password = "grJsoluco3s";

                if (urlHost.Contains("grj") || urlHost.Contains("localhost:") || urlHost.Contains("localhost"))
                {
                    unidades.EnderecoIP = "mssql1.gear.host";
                    unidades.NomeDatabase = "GRJQualidadeDev";
                    _user = "androidapp";
                    _password = "developer!";
                }
                string porta = null;

                string conexao = "data source=" + unidades.EnderecoIP + porta + ";initial catalog=" + unidades.NomeDatabase + ";persist security info=True;user id=" + _user + ";password=" + _password + ";";

                return conexao;
            }
            catch (Exception ex)
            {
                string mensagem = ex.Message;
                return mensagem;
            }
        }

        public string mensagem(string mensagem, alertaTipo tipoAlerta, bool json = true, bool reenviarRequisicao = false)
        {
            string alertaTipoLabel = "danger";
            switch (tipoAlerta)
            {
                case alertaTipo.success:
                    alertaTipoLabel = "success";
                    break;
                case alertaTipo.info:
                    alertaTipoLabel = "info";
                    break;
                case alertaTipo.warning:
                    alertaTipoLabel = "warning";
                    break;
                case alertaTipo.danger:
                    alertaTipoLabel = "danger";
                    break;
            }
            string requestResend = "requestresend";
            if (reenviarRequisicao == true)
            {
                requestResend = requestResend + "=true";
            }
            else
            {
                requestResend = requestResend + "=false";
            }

            mensagem = "#mensagem&" + alertaTipoLabel + "&" + mensagem + "&" + requestResend;
            if (json == true)
            {
                mensagem = "{" + mensagem + "}";
            }
            return mensagem;
        }
        //[SkipPasswordExpirationCheck]
        /// <summary>
        /// WebService para gravar a tipificação
        /// </summary>
        /// <param name="token">Autenticador</param>
        /// <param name="verificacaoTipificacaoChave">Chanve Verificação de Tipificação composta do código da unidade, código da operação, sequencial, banda e data. </param>
        /// <param name="sequencial">Sequencial do Boi</param>
        /// <param name="iUnidade">Id Unidade</param>
        /// <param name="data">Data da Verificação</param>
        /// <param name="empresaId">Id da Empresa</param>
        /// <param name="departamentoId">Id do Departameto</param>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <param name="codigo">Campo de teste</param>
        /// <param name="teste">Sim para executar teste na base </param>
        /// <param name="porta">porta do banco de dados</param>
        /// <returns>Json</returns>
        public JsonResult GetDadosGet(string token = "", string verificacaoTipificacaoChave = "", string sequencial = "", string idUnidade = "",
                                        string data = "", string empresaId = "", string departamentoId = "", string tarefaId = "",
                                        string codigo = "", string teste = "", string porta = "", string versaoApp = "", string url = "", string usuarioId = "")//codigo só pra teste
        {

            //problemas que podem ocorrer

            //se a tabela resultados já tiver resultados, vai duplicar na hora de trazer a comparacao
            //o idel é migrar os valores antigos da tabela de verficacaotipificacaocomparacao NumCaracteristica
            //verificar se o registro foi sincronizado e nao deu time out



            //Verifica se o sequêncial foi informado
            if (string.IsNullOrEmpty(sequencial))
            {
                return Json(mensagem("Informe o número sequencial!", alertaTipo.warning), JsonRequestBehavior.AllowGet);
                // return Json("{" + mensagem("Informe o número sequencial!", alertaTipo.warning) + "}", JsonRequestBehavior.AllowGet);
            }
            //Verifica se o Id da Unidade foi Informado
            if (string.IsNullOrEmpty(idUnidade))
            {
                return Json(mensagem("Informe o Id da Unidade", alertaTipo.warning), JsonRequestBehavior.AllowGet);
            }
            //Verifica se a Data da Coleta foi Informada
            if (string.IsNullOrEmpty(data))
            {
                return Json(mensagem("Informe a Data da Coleta", alertaTipo.warning), JsonRequestBehavior.AllowGet);
            }

            //somente exemplo de como deve ser testado via url
            //http://localhost:50267/VerificacaoTipificacao/GetDadosGet?token=testedotoken&codigo=202&data=20160318&sequencial=666
            //http://localhost:50267/VerificacaoTipificacao/GetDadosGet?token=testedotoken&idUnidade=1&data=20160502&sequencial=20&teste=sim&verificacaoTipificacaoChave=12730120160502
            //WebService para teste via browser sem a necessidade de carregar a página. Os parâmetros devem alterados para os dados dos dias
            //http://localhost:50267/VerificacaoTipificacao/GetDadosGet?token=testedotoken&idUnidade=1&data=20160502&sequencial=20&teste=sim&verificacaoTipificacaoChave=12730120160502&empresaId=1&departamentoId=2&tarefaId=183&operacaoId=27

            using (var db = new SGQ_GlobalEntities())
            {
                //teste do token é a validação para entrar na verificação de tipificação
                if (token.Equals("testedotoken"))//Token(token))
                {
                    //se teste for sim, vai testar no ambiente de teste

                    int id = Convert.ToInt32(idUnidade);

                    var unidades = (from p in db.Unidades
                                    where p.Id == id
                                    select p).FirstOrDefault();


                    string conexao = connectionString(unidades, url);


                    //Verifica se existe na VerificacaoTipificacao uma verificação com a chave informada no parametro do WebService.

                    //confirmar se a funcionalidade do statuus da Verificacao é para informar se tem ou nao verificacao.
                    var verificacaoTipificacao = (from p in db.VerificacaoTipificacao
                                                  where p.Chave == verificacaoTipificacaoChave
                                                  select p).FirstOrDefault();

                    if (verificacaoTipificacao == null)
                    {
                        return Json(mensagem("Não foi encontrada tipificação para ser validada!", alertaTipo.warning), JsonRequestBehavior.AllowGet);

                    }

                    // var VerificacaoTipificacao = db.VerificacaoTipificacao.Where(v => v.Chave == verificacaoTipificacaoChave).FirstOrDefault();
                    verificacaoTipificacao.Status = false;

                    //Instanciamos a variável existeComparacao para verificar se existe comparação no no sistema.
                    bool existeComparacao = false;

                    //Instanciamos a variável excluirVerificacaoAntiga para verificar se existe alguma verificação na tabela VerificacaoTipificacaoValidacao.
                    bool excluirVerificacaoAntiga = false;

                    // Query String para verificação das Caracteristicas da tipificação
                    string queryString = "exec FBED_GRTTipificacaoCaracteristica " + unidades.Codigo + ", '" + data + "', " + sequencial;
                    int iSequencial = 0;
                    int iBanda = 0;
                    DateTime dataHoraMonitor = DateTime.Now;

                    using (SqlConnection connection = new SqlConnection(conexao))
                    {
                        try
                        {
                            SqlCommand command = new SqlCommand(queryString, connection);
                            connection.Open();
                            using (SqlDataReader reader = command.ExecuteReader())
                            {

                                string queryZeroToNull = "UPDATE VerificacaoTipificacaoResultados SET AreasParticipantesId = NULL WHERE AreasParticipantesId='0'";
                                int noOfRowDeleted0 = db.Database.ExecuteSqlCommand(queryZeroToNull);

                                queryZeroToNull = "UPDATE VerificacaoTipificacaoResultados SET CaracteristicaTipificacaoId = NULL WHERE CaracteristicaTipificacaoId='0'";
                                noOfRowDeleted0 = db.Database.ExecuteSqlCommand(queryZeroToNull);



                                //A comparação pode vir a partir do banco de dados (Identificadores).
                                //São os dados que serão comparadors para incluir na tabela de tipificação.

                                //string[] comparacao = { "<IDADE>", "<SEXO>", "<GORDURA>", "<CONTUSAO>", "<FALHAOP>" };

                                string[] comparacao = { "<IDADE>", "<SEXO>", "<GORDURA>", "<CONTUSAO>", "<FALHAOP>" };

                                //Intanciamos iSequencial e iBanda fora do do while pois utilizamos a informação em outros métodos.



                                while (reader.Read())
                                {

                                    if (Convert.ToInt32(reader[4].ToString()) == verificacaoTipificacao.Banda)
                                    {
                                        int nCdEmpresa = Convert.ToInt32(reader[0].ToString());
                                        DateTime dMovimento = Convert.ToDateTime(reader[1].ToString());
                                        dataHoraMonitor = dMovimento;
                                        iSequencial = Convert.ToInt32(reader[2].ToString());
                                        int iSequencialTipificacao = Convert.ToInt32(reader[3].ToString());
                                        iBanda = Convert.ToInt32(reader[4].ToString());
                                        string cIdentificadorTipificacao = reader[5].ToString();
                                        int nCdCaracteristicaTipificacao = Convert.ToInt32(reader[6].ToString());

                                        //Entramos apenas uma vez no bloco de código abaixo para verificação de exclusão.
                                        if (excluirVerificacaoAntiga == false)
                                        {
                                            //Exclui qualquer Verificação Tipificação registrado para a empresa informada na data informado e com o sequencial informado.
                                            //por padrão o sequencial deve ser um campo unico para a empresa na data informada.
                                            //Chamamos o método ExcluiVerificacaoTipificacaoValidacao dentro do while, pois o valor iSequencial é recuperado somente no DataReader
                                            ///podemos informar o iSequencial no parametro e não seria necessário entrar na estrutuda de decisão para excluir a verificação
                                            ///a mesma poderia ser excluida no inicio do WebService
                                            ///



                                            //var VerificacaoTipificacaoValidacaoExiste = (from p in db.VerificacaoTipificacaoValidacao
                                            //                                             where p.nCdEmpresa == nCdEmpresa && p.dMovimento == dMovimento.Date && p.iSequencial == iSequencial
                                            //                                             select p);

                                            //if (VerificacaoTipificacaoValidacaoExiste != null)
                                            //{
                                            //    db.VerificacaoTipificacaoValidacao.Remove(VerificacaoTipificacaoValidacaoExiste);
                                            //    db.SaveChanges();
                                            //}
                                            //podemos excluir resultados e tbm
                                            //deleta validacao
                                            string queryValidacaoVDelete = "DELETE FROM VerificacaoTipificacaoValidacao WHERE nCdEmpresa='" + nCdEmpresa + "' AND CAST(dMovimento AS DATE) ='" + dMovimento.ToString("yyyy-MM-dd 00:00:00") + "' AND iSequencial='" + iSequencial + "' AND iBanda='" + iBanda + "'";
                                            int noOfRowDeleted = db.Database.ExecuteSqlCommand(queryValidacaoVDelete);


                                            //deleta validacao de comparacao
                                            string queryValidacaoCDelete = "DELETE FROM VerificacaoTipificacaoComparacao WHERE nCdEmpresa='" + nCdEmpresa + "' AND CAST(DataHora AS DATE) = '" + dMovimento.ToString("yyyy-MM-dd 00:00:00") + "' AND Sequencial='" + iSequencial + "' AND Banda='" + iBanda + "'";
                                            noOfRowDeleted = db.Database.ExecuteSqlCommand(queryValidacaoCDelete);
                                            //if (ExcluiVerificacaoTipificacaoValidacao(nCdEmpresa, dMovimento, iSequencial) == false)
                                            //{
                                            //    return Json("{mensagem:'Existe uma verificação para este sequencial!'}", JsonRequestBehavior.AllowGet);
                                            //}
                                            excluirVerificacaoAntiga = true;
                                            //Após a primeira verificação definimos o valor da variável excluirVerificacaoAntiga para true, pois sua utilização é necessária apenas uma vez.

                                        }

                                        //Se existir o valor da comparacao na matriz de comparação então iremos adicionar um registro na tabela VerificacaoTipificacaoValidacao
                                        if (MatrizStrinComparacao(comparacao, cIdentificadorTipificacao) == true)
                                        {
                                            //se existir comparação incluiremos na tabela VerificacaoTipificacaoValidacao os valores que serão comparados a partir da tabela do cliente
                                            existeComparacao = true;
                                            //Informamos na tabela verificação que o status da verificação é verdadeiro
                                            verificacaoTipificacao.Status = true;
                                            //Instanciamos um novo objeto da tabela VerificacaoTipificacaoValidacao
                                            var VerificacaoTipificacaoValidacao = new VerificacaoTipificacaoValidacao();
                                            //Incluimos o registro na tabela com 
                                            VerificacaoTipificacaoValidacao.nCdEmpresa = nCdEmpresa;
                                            VerificacaoTipificacaoValidacao.dMovimento = dMovimento;
                                            VerificacaoTipificacaoValidacao.iSequencial = iSequencial;
                                            VerificacaoTipificacaoValidacao.iSequencialTipificacao = iSequencialTipificacao;
                                            VerificacaoTipificacaoValidacao.iBanda = iBanda;
                                            VerificacaoTipificacaoValidacao.cIdentificadorTipificacao = cIdentificadorTipificacao;
                                            VerificacaoTipificacaoValidacao.nCdCaracteristicaTipificacao = nCdCaracteristicaTipificacao;
                                            db.VerificacaoTipificacaoValidacao.Add(VerificacaoTipificacaoValidacao);
                                        }
                                    }
                                }

                                iBanda = verificacaoTipificacao.Banda;
                                if (existeComparacao == false)
                                {
                                    return Json(mensagem("A Tipificação gravada não foi validada!", alertaTipo.info), JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    db.SaveChanges();
                                    //tratamento de erro, como fica?
                                    //Verifica se a comparação entre a tabela Verificação Comparação Resultados está conforme a tabela Verificação Tipificação Validacao
                                    //
                                    try
                                    {

                                        string[] ArrayComparacao = { "<GORDURA>", "<CONTUSAO>" };

                                        for (var i = 0; i < ArrayComparacao.Length; i++)
                                        {

                                            string[] varComparacao = new string[1];

                                            varComparacao[0] = ArrayComparacao[i];

                                            string comparacaoToString = ArrayComparacao[i];

                                            var resultIdTarefa = (from x in db.VerificacaoTipificacaoTarefaIntegracao
                                                                  //join y in db.CaracteristicaTipificacao
                                                                  //on x.CaracteristicaTipificacaoId equals y.nCdCaracteristica
                                                                  //where y.cIdentificador.Equals(comparacaoToString)
                                                                  select x).FirstOrDefault().TarefaId;

                                            bool conforme = verificacaoTipificaoComparacao(unidades.Codigo.ToString(), data, sequencial, iBanda.ToString(), unidades, empresaId, departamentoId, tarefaId, conexao, varComparacao);

                                            //instancia um resultado
                                            Resultados resultadoObj = new Resultados();
                                            //busca o id da operacao
                                            //estamos buscando o id da operação pelo id pois é a unica alternativa no momento para não gerar requisição no bando de dados simplimente para buscar um id
                                            //mas essa informacao deve vir na class
                                            //não localizados onde o id da operação é utilizado
                                            // int idOperacao = 27;

                                            //cria uma variavel com o id da empresa
                                            int _empresaId = Convert.ToInt32(empresaId);

                                            // instancia a empresa
                                            var empresa = db.Empresas.Where(d => d.Id == _empresaId).FirstOrDefault();

                                            //var unidade = db.Unidades.Where(u => u.Codigo == vtv.nCdEmpresa).FirstOrDefault();
                                            //cria uma variavel para o id do departamento
                                            int _departamentoId = Convert.ToInt32(departamentoId);

                                            //instancia o departaento
                                            var departamento = db.Departamentos.Where(d => d.Id == _departamentoId).FirstOrDefault();

                                            //cria uma variavel para o id da tarefa
                                            int _tarefaId = Convert.ToInt32(tarefaId);
                                            //instancia a tarefa

                                            var tarefa = db.Tarefas.Where(t => t.Id == _tarefaId).FirstOrDefault();

                                            //cria uma variavel para o id do monitoramento
                                            int _monitoramentoId = resultIdTarefa;//db.TarefaMonitoramentos.Where(tm => tm.Tarefa == tarefa.Id).FirstOrDefault().Id;
                                            //instancia o monitoramento
                                            var monitoramento = db.Monitoramentos.Where(t => t.Id == _monitoramentoId).FirstOrDefault();

                                            //var operacao = db.Operacoes.Where(o => o.Id == idOperacao).FirstOrDefault();
                                            //instancia uma variavel para operacsao
                                            var operacao = db.Operacoes.Where(o => o.Id == tarefa.Operacao).FirstOrDefault();
                                            //cria uma variavel do produtoid
                                            int _produtoId = tarefa.Operacao;//Convert.ToInt32(produtoId);
                                            //instancia uo produto id
                                            var produto = db.Produtos.Where(p => p.Id == _produtoId).FirstOrDefault();

                                            //OPERAÇÃO E PRODUTO PEGA DA TAREFA
                                            //intancia a meta
                                            Metas meta = db.Metas.Where(m => m.Operacao == operacao.Id).FirstOrDefault();

                                            //pesquisa na tabela resultados um resultado com a chave
                                            Resultados res = db.Resultados.Where(r => r.EmpresaId == 1
                                                && r.UnidadeId == id
                                                && r.OperacaoId == tarefa.Operacao
                                                && r.TarefaId == _tarefaId
                                                && r.MonitoramentoId == _monitoramentoId
                                                && DbFunctions.TruncateTime(r.DataHora) == DbFunctions.TruncateTime(dataHoraMonitor)
                                                && r.Sequencial.Value == iSequencial
                                                && r.Banda.Value == iBanda).FirstOrDefault();
                                            if (res != null)
                                            {
                                                //se exiistir um resultado;
                                                //joga o resultado da tabela no objetivo resultado criado acima
                                                versaoApp = res.VersaoAPP;
                                                resultadoObj = res;
                                            }
                                            else
                                            {
                                                //comeca salvando as metas no resultado objeto
                                                resultadoObj.Meta = meta.Meta;
                                                resultadoObj.Status = "Avaliado";
                                                resultadoObj.AvaliacaoAvulsa = false;
                                                resultadoObj.Amostragem = tarefa.Amostragem;
                                                resultadoObj.FormaAmostragem = tarefa.FormaAmostragem;

                                                resultadoObj.Sequencial = iSequencial;
                                                resultadoObj.Banda = iBanda;

                                                string chave = unidades.Id + "-" + operacao.Id + "-" + tarefa.Id + "-" + resultadoObj.Sequencial + "-" + resultadoObj.Banda + "-" + dataHoraMonitor.ToString("yyyMMdd");

                                                resultadoObj.Chave = chave;


                                                int numeroAvaliacao = CommonData.GetNumeroAvaliacaoAtual(unidades.Id, departamento.Id, operacao.Id, tarefa.Id, true);

                                                //cria um numeoro de amostra
                                                //diminuo em 1 o valor retornado, pois neste ponto a tabela VerificacaoTipificacao ja recebeu os dados da amostra corrente
                                                int numeroAmostra = CommonData.GetProximoNumeroAmostra(unidades.Id, departamento.Id, operacao.Id, tarefa.Id, numeroAvaliacao, true);

                                                //se numeroAmostra for = menor a 1
                                                if (numeroAmostra <= 1)
                                                {
                                                    numeroAvaliacao = CommonData.GetProximoNumeroAvaliacao(unidades.Id, departamento.Id, operacao.Id, tarefa.Id, true);
                                                    numeroAmostra = 1;
                                                }

                                                //cria um numero de avaliacao

                                                //preenche o numero da amostra
                                                resultadoObj.NumeroAvaliacao = numeroAvaliacao;
                                                resultadoObj.NumeroAmostra = numeroAmostra;
                                                string identificador = unidades.Id + "-" + operacao.Id + "-" + tarefa.Id + "-" + monitoramento.Id + "-" + resultadoObj.Sequencial + "-" + resultadoObj.Banda + "-" + dataHoraMonitor.ToString("yyyMMdd");
                                                resultadoObj.Identificador = identificador;

                                                resultadoObj.EmpresaId = empresa.Id;
                                                resultadoObj.Empresa = empresa.Nome;
                                                resultadoObj.UnidadeId = unidades.Id;
                                                resultadoObj.Unidade = unidades.Nome;
                                                resultadoObj.DepartamentoId = departamento.Id;
                                                resultadoObj.Departamento = departamento.Nome;
                                                resultadoObj.OperacaoId = operacao.Id;
                                                resultadoObj.Operacao = operacao.Nome;
                                                resultadoObj.TarefaId = tarefa.Id;
                                                resultadoObj.Tarefa = tarefa.Nome;
                                                //resultadoObj.PecasAvaliadas = !string.IsNullOrWhiteSpace(resultado.TotalAvaliadoStr) ? int.Parse(resultado.TotalAvaliadoStr) : pecasAvaliadas;
                                                resultadoObj.MonitoramentoId = monitoramento.Id;
                                                resultadoObj.Monitoramento = monitoramento.Nome;
                                                resultadoObj.ProdutoId = produto.Id;
                                                resultadoObj.Produto = produto.Nome;
                                                //resultadoObj.Monitor = 63;
                                                resultadoObj.Monitor = Convert.ToInt32(usuarioId);
                                                resultadoObj.Minimo = "Conforme";
                                                resultadoObj.Maximo = "Conforme";
                                                resultadoObj.Acesso = "IN LOCO";

                                                if (!string.IsNullOrEmpty(versaoApp))
                                                {
                                                    resultadoObj.VersaoAPP = versaoApp;
                                                    resultadoObj.Mobile = true;
                                                }
                                                else
                                                {
                                                    resultadoObj.Mobile = false;
                                                }

                                                resultadoObj.Peso = 1;
                                                resultadoObj.Data = verificacaoTipificacao.DataHora;
                                                resultadoObj.DataHora = verificacaoTipificacao.DataHora;
                                                resultadoObj.DataTipificacao = verificacaoTipificacao.DataHora;
                                                resultadoObj.DataHoraMonitor = verificacaoTipificacao.DataHora;
                                            }

                                            if (conforme == true)
                                            {
                                                resultadoObj.Avaliacao_2 = 0;
                                            }
                                            else
                                            {
                                                resultadoObj.Avaliacao_2 = 1;
                                            }
                                            //resultadoObj.Avaliacao_2 = Convert.ToInt32(!ValidaConformidade(verificacaoTipificacaoValidacaoList, reader));
                                            resultadoObj.Avaliacao_1 = (resultadoObj.Avaliacao_2 == 0) ? "Conforme" : "Não Conforme";

                                            //se não existe um resultado para essa Verificação de Tipificação
                                            //adicionamos o resultado na tabela
                                            //se existir apenas atualizamos

                                            if (res == null)
                                            {

                                                db.Resultados.Add(resultadoObj);
                                            }
                                            db.SaveChanges();


                                            //comparacao da tabelas
                                            //se tiver divergente grana na comparacao
                                            //gr

                                            //  DbDataReader verificacaoReader = VerificacaoTipificacaoResultadosLista(Convert.ToInt32(unidades.Codigo), data, Convert.ToInt32(sequencial));

                                            //if (verificacaoReader != null)
                                            //{

                                            //}
                                            //else
                                            //{
                                            //    return Json("{mensagem:'Não foi feita a validação para os dados informados!'}", JsonRequestBehavior.AllowGet);
                                            //}
                                        }

                                        return Json(mensagem("Tipificação registrada com sucesso!", alertaTipo.info), JsonRequestBehavior.AllowGet);

                                    }
                                    catch (Exception ex)
                                    {
                                        string t = ex.ToString();
                                        var inner = ex.InnerException.IsNotNull() ? ex.InnerException.Message : "Não consta.";
                                        return Json(mensagem("Não foi possível registrar os dados de comparação. Tente novamente! EXCEPTION" + t + ", INNER: " + inner + ". CONNECTION: " + conexao + ".", alertaTipo.warning, reenviarRequisicao: true));

                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string t = ex.ToString();
                            var inner = ex.InnerException.IsNotNull() ? ex.InnerException.Message : "Não consta.";
                            //pegar a excption e salvar em um banco de dados
                            return Json(mensagem("Não foi possível verificar a tipificação. Tente novamente em alguns instantes. Se o problema persistir, entre em contato com o suporte! EXCEPTION" + t + ", INNER: " + inner + ". CONNECTION: " + conexao + ".", alertaTipo.warning, reenviarRequisicao: true), JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }

            return Json(mensagem("Não foi possível executar a verificação de tipificação. Aguarde alguns instantes e tente novamente. Se o problema persistir entre em contato com o suporte!", alertaTipo.warning, reenviarRequisicao: true), JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [Route("buscarTipificacaoSelectList")]
        public JsonResult buscarTipificacaoSelectList(string data, string unidade)
        {

            try
            {
                var retorno = new List<VerificacaoTipificacao>();
                int unidadeId = Convert.ToInt32(unidade);
                var queryString = "";
                var dataDate = DateTime.ParseExact(data, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                data = DateTime.ParseExact(data, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

                string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["SGQ"].ConnectionString;
                queryString = "SELECT * FROM VerificacaoTipificacao WHERE CAST(DataHora AS DATE) = '" + data + "' AND UnidadeId='" + unidadeId + "'";


                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<object> resultado = new List<object>();
                        while (reader.Read())
                        {
                            retorno.Add(new VerificacaoTipificacao
                            {
                                Id = (int)reader[0],
                                Sequencial = (int)reader[1],
                                Banda = (byte)reader[2],
                                DataHora = (DateTime)reader[3],
                                UnidadeId = (int)reader[4],
                                Chave = reader[5].ToString(),
                                Status = (bool)reader[6]
                            });

                        }
                        if (retorno.Count > 0)
                        {
                            return Json(new { data = retorno }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { mensagemSemDados = "Não existe Verificação da Tipificação para a data selecionada." }, JsonRequestBehavior.AllowGet);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException.IsNotNull() ? ex.InnerException.Message : "Não consta.";
                return Json(new { mensagemException = ex.Message + " Inner:" + inner }, JsonRequestBehavior.AllowGet);
            }
            //return Json(new { mensagemSemDados = "Não existe Verificação da Tipificação para a data selecionada." }, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        [Route("buscarTipificacao")]
        public JsonResult buscarTipificacao(string sequencial, string banda, string data, string unidade)
        {

            try
            {
                using (var db = new SGQ_GlobalEntities())
                {
                    //data = Convert.ToDateTime(data).ToString("yyyy-MM-dd")
                    data = DateTime.ParseExact(data, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

                    int id = Convert.ToInt32(unidade);

                    var unidades = (from p in db.Unidades
                                    where p.Id == id
                                    select p).FirstOrDefault();


                    string queryString = "";
                    string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["SGQ"].ConnectionString;

                    queryString = "SELECT vt.Id, vt.Sequencial, vt.banda, vt.unidadeId, vt.DataHora, vtr.id, vtr.TarefaId, vtr.CaracteristicaTipificacaoId, c.cIdentificador, vtr.AreasParticipantesId,  r.NumeroAvaliacao, r.NumeroAmostra " +
                                    "FROM VerificacaoTipificacaoResultados vtr " +
                                    "INNER JOIN VerificacaoTipificacao vt ON vtr.chave = vt.chave " +
                                    "LEFT JOIN CaracteristicaTipificacao c ON vtr.CaracteristicaTipificacaoId=c.nCdCaracteristica " +
                                    "LEFT JOIN Resultados r ON r.TarefaId=183 AND r.Sequencial='" + sequencial + "' AND r.Banda='" + banda + "' AND CAST(r.DataHoraMonitor AS DATE) = '" + data + "' AND r.UnidadeId='" + unidade + "'" +
                                    "WHERE vt.Sequencial='" + sequencial + "' AND vt.Banda='" + banda + "' AND CAST(vt.DataHora AS DATE) = '" + data + "' AND vt.UnidadeId='" + unidade + "' ORDER BY c.nCdCaracteristica ASC";

                    //utiliza transacao para excluir e incluir os itens
                    using (SqlConnection connection = new SqlConnection(conexao))
                    {
                        SqlCommand command = new SqlCommand(queryString, connection);

                        connection.Open();
                        data = DateTime.ParseExact(data, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                        //data = Convert.ToDateTime(data).ToString("dd/MM/yyyy");
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //Instanciamos uma classe do Objeto de Comparacao
                            List<object> resultado = new List<object>();
                            while (reader.Read())
                            {
                                string verificacaoId = reader[0].ToString();
                                string resultadoId = reader[5].ToString();
                                string tarefaId = reader[6].ToString();
                                string caracteristicaId = reader[7].ToString();
                                string caracateristicaIdentificador = reader[8].ToString();
                                string AreasParticipantesId = reader[9].ToString();
                                string NumeroAvaliacao = reader[10].ToString();
                                string NumeroAmostra = reader[11].ToString();
                                resultado.Add(new
                                {
                                    tipificacaoId = verificacaoId,
                                    sequencial = sequencial,
                                    banda = banda,
                                    data = data,
                                    unidade = unidade,
                                    resultadoId = resultadoId,
                                    tarefaId = tarefaId,
                                    caracteristicaId = caracteristicaId,
                                    caracateristicaIdentificador = caracateristicaIdentificador,
                                    AreasParticipantesId = AreasParticipantesId,
                                    NumeroAvaliacao = NumeroAvaliacao,
                                    NumeroAmostra = NumeroAmostra
                                });

                            }
                            if (resultado.Count > 0)
                            { return Json(new { resultado = resultado, IsSucesso = true }, JsonRequestBehavior.AllowGet); }
                            else
                            { return Json(new { mensagem = "Tipificação não encontrada. Verifique se existe a Tipificação.", IsSucesso = true }, JsonRequestBehavior.AllowGet); }
                        }
                    }
                    //catch (Exception ex)
                    //{
                    //    return Json(new { mensagem = "Não foi possível localizar a verificacao", IsSucesso = true, exMessage = ex.Message + "Inner" + ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
                    //}

                }
            }
            catch (Exception ex)
            {
                return Json(new { mensagem = ex.Message + " Inner " + ex.InnerException.Message, IsSucesso = true, exMessage = ex.Message + "Inner" + ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }


        }

        public bool verificacaoTipificaoComparacao(string unidadeCodigo, string data, string sequencial, string banda, Unidades unidades, string empresaId, string departamentoId, string tarefaIdm, string conexao, string[] varComparacao)
        {

            //melhorar tabela de comparacao
            //criar valorSGQ, valorJBS, nCdEmpresa

            //Instanciamos uma variável conforme partindo do principicio que a verificação está conforme
            bool conforme = true;

            //Criamos uma variável de comparação unica para comparar os parametros unicos da tabela
            string[] comparacaoUnica = { "<IDADE>", "<SEXO>", "<GORDURA>" };

            //string[] comparacaoRESULTADO = { "<IDADE>", "<SEXO>", "<GORDURA>", "<CONTUSAO>" };

            string[] comparacaoRESULTADO = varComparacao;


            string queryString = "(SELECT 'VTR', U.CODIGO nCdEmpresa, VT.DATAHORA dMovimento, VT.SEQUENCIAL iSequencial, VT.BANDA iBanda, VTR.CARACTERISTICATIPIFICACAOID nCdCaracteristicaTipificacao, CT.cIdentificador, VTV.nCdCaracteristicaTipificacao " +
                                "FROM VERIFICACAOTIPIFICACAO VT " +
                                "INNER JOIN UNIDADES U ON U.ID = VT.UNIDADEID " +
                                "INNER JOIN VERIFICACAOTIPIFICACAOresultados VTR ON VTR.CHAVE = VT.CHAVE " +
                                "INNER JOIN CaracteristicaTipificacao CT ON VTR.CARACTERISTICATIPIFICACAOID=CT.nCdCaracteristica " +
                                "LEFT JOIN VerificacaoTipificacaoValidacao VTV ON VTV.nCdEmpresa=U.CODIGO AND CAST(VTV.dMovimento AS DATE) = CAST(VT.datahora AS DATE) AND VTV.iSequencial=VT.SEQUENCIAL AND VTV.IBANDA=VT.BANDA AND VTV.nCdCaracteristicaTipificacao=VTR.CARACTERISTICATIPIFICACAOID " +
                                "WHERE U.CODIGO='" + unidadeCodigo + "' AND CAST(VT.datahora AS DATE) = CAST('" + data + "' AS DATE) AND VT.sequencial='" + sequencial + "' AND VT.Banda='" + banda + "')" +
                                "UNION ALL " +
                                "(SELECT 'VTV', VTV.nCdEmpresa, VTV.dMovimento, VTV.iSequencial, VTV.iBanda, VTR.CaracteristicaTipificacaoId, VTV.cIdentificadorTipificacao, VTV.nCdCaracteristicaTipificacao " +
                                "FROM VerificacaoTipificacaoValidacao VTV " +
                                "INNER JOIN UNIDADES U ON U.CODIGO = VTV.nCdEmpresa " +
                                "INNER JOIN VERIFICACAOTIPIFICACAO VT ON VT.UnidadeId=U.ID AND VT.Sequencial=VTV.iSequencial AND VT.Banda=VTV.iBanda AND CAST(VT.DataHora AS DATE) = CAST(VTV.dMovimento AS DATE) " +
                                "LEFT JOIN VERIFICACAOTIPIFICACAOresultados VTR ON VTR.CHAVE = VT.CHAVE AND VTR.CaracteristicaTipificacaoId=VTV.nCdCaracteristicaTipificacao " +
                                "WHERE VTV.nCdEmpresa='" + unidadeCodigo + "' AND CAST(VTV.dMovimento AS DATE) = CAST('" + data + "' AS DATE) AND VTV.iSequencial='" + sequencial + "' AND VTV.iBanda='" + banda + "') ";

            //utiliza transacao para excluir e incluir os itens
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SGQ"].ConnectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //Instanciamos uma classe do Objeto de Comparacao
                        List<ResultadoComparacaoValidacao> comparacao = new List<ResultadoComparacaoValidacao>();

                        while (reader.Read())
                        {
                            //Comparamos a caracteristica das duas tabelasd
                            string idCaracteristicaTipificacaoVTResultados = reader[5].ToString();
                            string labelCaracteristica = reader[6].ToString();
                            string idCaracteristicaTipificacaoVTValidacao = reader[7].ToString();

                            //Se as caracteristicas forem diferentes o resultado da verificação é não conforme
                            if (idCaracteristicaTipificacaoVTResultados != idCaracteristicaTipificacaoVTValidacao && MatrizStrinComparacao(comparacaoRESULTADO, labelCaracteristica))
                            {
                                conforme = false;
                                //Incluimos o resultado na tabela de comparação
                                comparacao.Add(
                                          new ResultadoComparacaoValidacao()
                                          {

                                              localTabela = reader[0].ToString(),
                                              nCadEmpresa = reader[1].ToString(),
                                              dataMovimento = Convert.ToDateTime(reader[2].ToString()),
                                              iSequencial = Convert.ToInt32(reader[3].ToString()),
                                              iBanda = Convert.ToInt32(reader[4].ToString()),
                                              idCaracteristicaVTR = reader[5].ToString(),
                                              identificadorLabel = reader[6].ToString(),
                                              idCaracteristicaVTV = reader[7].ToString()

                                          });
                            }

                        }
                        //Se existe resultados não conformes
                        if (conforme == false)
                        {
                            //Separamos os resultados que estão na tabela VERIFICACAOTIPIFICACAOresultados
                            var valoresVTR = (from p in comparacao
                                              where p.localTabela == "VTR"
                                              select p).ToList();

                            //separamos os resultados que estão na tabela VerificacaoTipificacaoValidacao
                            var valoresVTV = (from p in comparacao
                                              where p.localTabela == "VTV"
                                              select p).ToList();

                            //Varremos a tabela de resultados para comparar os valores da Matriz comparacaoUnica com a tabela de validação
                            foreach (var p in valoresVTR)
                            {
                                //Se o resultado da for uma comparação Única
                                if (MatrizStrinComparacao(comparacaoUnica, p.identificadorLabel) == true)
                                {
                                    //Verificamos se existe a caracteristica na tabela de validação
                                    var valorComparacaoVTV = (from pV in valoresVTV
                                                              where pV.identificadorLabel == p.identificadorLabel
                                                              select pV).FirstOrDefault();

                                    //Se a caracteristica existir
                                    if (valorComparacaoVTV != null)
                                    {
                                        //Atualizamos o valor do valor da verificação validação na tabela resultados com o valor da verificação na tabela caracteristica
                                        p.idCaracteristicaVTV = valorComparacaoVTV.idCaracteristicaVTV;
                                        //reovemos o valor da tabela de validação
                                        valoresVTV.Remove(valorComparacaoVTV);

                                        //teste de desempenho para verificamos se podemos deixar as informações na tabela de comparação ao inves de dividir em 2 objetos
                                        //comparacao.Remove(valorComparacaoVTV);
                                    }
                                }
                                //Incluimos o Valor dos resultados na tabela VerificacaoTipificacaoComparacao
                                VerificacaoTipificacaoComparacaoAdicionar(unidadeCodigo, sequencial, banda, p.identificadorLabel, p.dataMovimento.ToString(), p.idCaracteristicaVTR, p.idCaracteristicaVTV);
                            }

                            //Verificamos se ainda existem valores da tabela VerificacaoTipificacaoValidacao que não foram comparados
                            foreach (var p in valoresVTV)
                            {
                                //Incluimos os Valores da Tipificação na tabela VerificacaoTipificacaoComparacao 
                                VerificacaoTipificacaoComparacaoAdicionar(unidadeCodigo, sequencial, banda, p.identificadorLabel, p.dataMovimento.ToString(), p.idCaracteristicaVTR, p.idCaracteristicaVTV);
                            }
                        }
                        //retornamos o valor para tabela de resultados
                        return conforme;
                    }
                }
                catch (Exception ex)
                {
                    //string t = ex.ToString();
                    //mensagemErro = ex.Message;
                    //return false;
                    //RETORNAR O ERRO
                    //mensagemErro = Json(t, JsonRequestBehavior.AllowGet);
                    throw ex;
                }
            }


        }
        protected void VerificacaoTipificacaoComparacaoAdicionar(string uCodigo, string sequencial, string banda, string labelCaracteristica, string dataRegistro,
                                                                string idCaracteristicaTipificacaoVTR, string idCaracteristicaTipificacaoVTV)
        {

            using (var db = new SGQ_GlobalEntities())
            {
                try
                {
                    var VerificacaoTipificacaoComparacao = new VerificacaoTipificacaoComparacao();
                    VerificacaoTipificacaoComparacao.nCdEmpresa = Convert.ToInt32(uCodigo);
                    VerificacaoTipificacaoComparacao.Sequencial = Convert.ToInt32(sequencial);
                    VerificacaoTipificacaoComparacao.Banda = Convert.ToInt32(banda);
                    VerificacaoTipificacaoComparacao.Identificador = labelCaracteristica;
                    VerificacaoTipificacaoComparacao.NumCaracteristica = 0;

                    VerificacaoTipificacaoComparacao.DataHora = Convert.ToDateTime(dataRegistro);
                    if (!string.IsNullOrEmpty(idCaracteristicaTipificacaoVTR))
                    {
                        VerificacaoTipificacaoComparacao.valorSGQ = Convert.ToInt32(idCaracteristicaTipificacaoVTR);
                    }
                    if (!string.IsNullOrEmpty(idCaracteristicaTipificacaoVTV))
                    {
                        VerificacaoTipificacaoComparacao.valorJBS = Convert.ToInt32(idCaracteristicaTipificacaoVTV);
                    }
                    db.VerificacaoTipificacaoComparacao.Add(VerificacaoTipificacaoComparacao);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string erro = ex.Message;

                }
            }
        }
        //metodo utilizado inicialmente para deletar a VerificacaoTipificacaoValidacao
        //protected bool ExcluiVerificacaoTipificacaoValidacao(int nCdEmpresa, DateTime dMovimento, int iSequencial)
        //{
        //    string sql = "DELETE FROM VerificacaoTipificacaoValidacao WHERE nCdEmpresa='" + nCdEmpresa + "' AND dMovimento='" + dMovimento.ToString("yyyy-MM-dd 00:00:00") + "' AND iSequencial='" + iSequencial + "'";
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    using (SqlCommand command = new SqlCommand(sql, connection))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            int i = command.ExecuteNonQuery();
        //            //se existir ou não os dados retorna true;
        //            return true;

        //        }
        //        catch (Exception ex)
        //        {
        //            //caso exista os dados e não conseguir excluir, retorna a exception  e não continua a verificação;
        //            throw ex;
        //        }
        //    }
        //}
        public string mensagemErro { get; set; }

        /// <summary>
        /// Comparação de Matriz de String de uma dimenção
        /// </summary>
        /// <param name="matriz">Informa a Matriz</param>
        /// <param name="valorComparar">Informa o Valor a comparar</param>
        /// <returns>true para existe, false para não existe</returns>
        public bool MatrizStrinComparacao(string[] matriz, string valorComparar)
        {
            //retornar uma mensatgem para a equipe de desenvolvimento
            if (matriz == null)
            {
                mensagemErro = "Informe a Matriz";
                return false;
            }
            if (string.IsNullOrEmpty(valorComparar))
            {
                mensagemErro = "Informe o Valor para Comparar com os dados da matriz";
                return false;
            }

            int strNumber = 0;
            int strIndex = 0;
            for (strNumber = 0; strNumber < matriz.Length; strNumber++)
            {
                strIndex = matriz[strNumber].IndexOf(valorComparar);
                if (strIndex >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void AddToDictionary(DbDataReader reader, Dictionary<string, object> dict)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {

                var _temp = reader[i];

                if (_temp.GetType() == typeof(string))
                {
                    _temp = _temp.ToString().TrimEnd();
                }

                dict.Add(reader.GetName(i).ToString(), _temp);
            }
        }

        private void AddToDictionary(SqlDataReader reader, Dictionary<string, object> dict)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {

                var _temp = reader[i];

                if (_temp.GetType() == typeof(string))
                {
                    _temp = _temp.ToString().TrimEnd();
                }

                dict.Add(reader.GetName(i).ToString(), _temp);
            }
        }


    }
    public class ResultadoComparacaoValidacao
    {
        public string localTabela { get; set; }
        public DateTime dataMovimento { get; set; }
        public string nCadEmpresa { get; set; }
        public int iSequencial { get; set; }
        public int iBanda { get; set; }
        public string idCaracteristicaVTR { get; set; }
        public string identificadorLabel { get; set; }
        public string idCaracteristicaVTV { get; set; }
    }

}
