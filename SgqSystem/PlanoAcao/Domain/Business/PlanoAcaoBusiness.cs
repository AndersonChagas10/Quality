using PA.Data;
using PA.Domain.AutoMapper;
using PA.Domain.Business;
using PA.DTO;
using QualidadeTotal.PlanoAcao.DTO;
using SgqSystem.PlanoAcao.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace PA.Domain
{
    public class PlanoAcaoBusiness
    {

        #region Contrutor
        //private readonly PlanoAcaoData _PlanoAcaoData;
        //private readonly LogOperacaoBusiness _logOperacaoBusiness;

        //public PlanoAcaoBusiness(PlanoAcaoData PlanoAcaoData, LogOperacaoBusiness logOperacaoBusiness)
        //{
        //    _PlanoAcaoData = PlanoAcaoData;
        //    _logOperacaoBusiness = logOperacaoBusiness;
        //}
        #endregion

        #region CONSULTAR

        public List<MultiplaEscolhaDTO> BuscarCamposJbs(RequestJbs request)
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();
            try
            {
                string query = string.Empty;

                if (request.Tabela.ToUpper().StartsWith("INDICADOR"))
                {
                    query = " SELECT CodigoIndicador, Indicador FROM " +
                            " (" +
                            "	SELECT " +
                            "		   I.ID		AS CodigoIndicador " +
                            "		  ,I.Nome	AS Indicador " +
                            "		  ,M.ID		AS CodigoMonitoramento " +
                            "		  ,M.Nome	AS Monitoramento " +
                            "		  ,T.ID		AS CodigoTarefa " +
                            "		  ,T.Nome	AS Tarefa " +
                            "	FROM      TarefaMonitoramentos TM " +
                            "	LEFT JOIN Tarefas M " +
                            "		   ON M.ID = TM.Tarefa " +
                            "	LEFT JOIN Monitoramentos T " +
                            "		   ON T.ID = TM.Monitoramento " +
                            "	LEFT JOIN OPERACOES I " +
                            "		   ON I.ID = M.Operacao " +
                            " ) IND" +
                            " GROUP BY CodigoIndicador, Indicador " +
                            " ORDER BY 2 ";
                }
                else if (request.Tabela.ToUpper().StartsWith("MONITORAMENTO"))
                {
                    query = " SELECT CodigoMonitoramento, Monitoramento  FROM " +
                            " (" +
                            "	SELECT" +
                            "		   I.ID		AS CodigoIndicador " +
                            "		  ,I.Nome	AS Indicador " +
                            "		  ,M.ID		AS CodigoMonitoramento " +
                            "		  ,M.Nome	AS Monitoramento " +
                            "		  ,T.ID		AS CodigoTarefa " +
                            "		  ,T.Nome	AS Tarefa " +
                            "	FROM      TarefaMonitoramentos TM " +
                            "	LEFT JOIN Tarefas M " +
                            "		   ON M.ID = TM.Tarefa " +
                            "	LEFT JOIN Monitoramentos T " +
                            "		   ON T.ID = TM.Monitoramento " +
                            "	LEFT JOIN OPERACOES I " +
                            "		   ON I.ID = M.Operacao " +
                            " ) MON " +
                            " WHERE CodigoIndicador = " + request.IdBusca.ToString() +
                            " GROUP BY CodigoMonitoramento, Monitoramento " +
                            " ORDER BY 2 ";
                }
                else if (request.Tabela.ToUpper().StartsWith("TAREFA"))
                {
                    query = " SELECT CodigoTarefa, Tarefa  FROM " +
                            " ( " +
                            "	SELECT " +
                            "		   I.ID		AS CodigoIndicador " +
                            "		  ,I.Nome	AS Indicador " +
                            "		  ,M.ID		AS CodigoMonitoramento " +
                            "		  ,M.Nome	AS Monitoramento " +
                            "		  ,T.ID		AS CodigoTarefa " +
                            "		  ,T.Nome	AS Tarefa " +
                            "	FROM      TarefaMonitoramentos TM " +
                            "	LEFT JOIN Tarefas M " +
                            "		   ON M.ID = TM.Tarefa " +
                            "	LEFT JOIN Monitoramentos T " +
                            "		   ON T.ID = TM.Monitoramento " +
                            "	LEFT JOIN OPERACOES I " +
                            "		   ON I.ID = M.Operacao " +
                            " ) TAR" +
                            " WHERE CodigoIndicador = " + request.IdBusca.ToString() +
                            "  AND CodigoMonitoramento = " + request.IdBusca2.ToString() +
                            " GROUP BY CodigoTarefa, Tarefa " +
                            " ORDER BY 2 ";
                }
                else if (request.Tabela.ToUpper().Equals("CAUSA GENÉRICA"))
                {
                    query = " SELECT codigoCausa, CausaGenerica FROM" +
                            " (" +
                            " SELECT" +
                            "	   CG.ID					AS codigoCausa" +
                            "	  ,CG.CausaGenerica			AS CausaGenerica" +
                            "	  ,G.ID						AS codigoGrupo" +
                            "	  ,G.GrupoCausa				AS GrupoCausa" +
                            "	  ,CMG.ID					AS codigoContramedida" +
                            "	  ,CMG.ContramedidaGenerica	AS ContramedidaGenerica" +
                            " FROM CausaGenerica CG" +
                            " LEFT JOIN GrupoCausa G" +
                            " ON G.ID = CG.GrupoCausa" +
                            " LEFT JOIN ContramedidaGenerica CMG" +
                            " ON CG.ID = CMG.CausaGenerica" +
                            " ) CAUSA" +
                            " GROUP BY codigoCausa, CausaGenerica" +
                            " ORDER BY 2";
                }
                else if (request.Tabela.ToUpper().Equals("GRUPO CAUSA"))
                {
                    query = " SELECT codigoGrupo, GrupoCausa FROM" +
                            " (" +
                            " SELECT" +
                            "	   CG.ID					AS codigoCausa" +
                            "	  ,CG.CausaGenerica			AS CausaGenerica" +
                            "	  ,G.ID						AS codigoGrupo" +
                            "	  ,G.GrupoCausa				AS GrupoCausa" +
                            "	  ,CMG.ID					AS codigoContramedida" +
                            "	  ,CMG.ContramedidaGenerica	AS ContramedidaGenerica" +
                            " FROM CausaGenerica CG" +
                            " LEFT JOIN GrupoCausa G" +
                            " ON G.ID = CG.GrupoCausa" +
                            " LEFT JOIN ContramedidaGenerica CMG" +
                            " ON CG.ID = CMG.CausaGenerica" +
                            " ) CAUSA" +
                            " WHERE codigoCausa = " + request.IdBusca.ToString() +
                            " GROUP BY codigoGrupo, GrupoCausa" +
                            " ORDER BY 2";
                }
                else if (request.Tabela.ToUpper().Equals("CONTRAMEDIDA GENÉRICA"))
                {
                    query = " SELECT codigoContramedida, ContramedidaGenerica FROM " +
                            " (" +
                            " SELECT" +
                            "	   CG.ID					AS codigoCausa " +
                            "	  ,CG.CausaGenerica			AS CausaGenerica " +
                            "	  ,G.ID						AS codigoGrupo " +
                            "	  ,G.GrupoCausa				AS GrupoCausa " +
                            "	  ,CMG.ID					AS codigoContramedida " +
                            "	  ,CMG.ContramedidaGenerica	AS ContramedidaGenerica " +
                            " FROM CausaGenerica CG " +
                            " LEFT JOIN GrupoCausa G " +
                            " ON G.ID = CG.GrupoCausa " +
                            " LEFT JOIN ContramedidaGenerica CMG " +
                            " ON CG.ID = CMG.CausaGenerica " +
                            " ) CAUSA " +
                            " WHERE codigoCausa = " + request.IdBusca.ToString() +
                            " GROUP BY codigoContramedida, ContramedidaGenerica " +
                            " ORDER BY 2 ";
                }
                else if (request.Tabela.ToUpper().Equals("UNIDADE"))
                {
                    query = " SELECT Id, Nome FROM Unidades ";
                }
                else if (request.Tabela.ToUpper().Equals("DEPARTAMENTO"))
                {
                    query = " SELECT Id, Nome FROM Departamentos ";
                }

                var result = _PlanoAcaoData.BuscarCamposJbs(query);

                var campo = AutoMapperUtil.Map<CampoDTO>(_PlanoAcaoData.BuscarCampo(request.Tabela, 0));

                var multEscolha = _PlanoAcaoData.BuscarListaMultiplaEscolhaPorCampoId(campo.Id, request.Tabela);

                var listaRetorno = new List<MultiplaEscolhaDTO>();


                #region Camparar se campos JBS estao no multiplaEscolha

                var listaNaoContem = new List<MultiplaEscolha>();

                foreach (var m in result)
                {
                    var achou = false;
                    var tempObjeto = new MultiplaEscolha();

                    int Id = (int)m.GetType().GetProperty("Id").GetValue(m, null);
                    string Nome = (string)m.GetType().GetProperty("Nome").GetValue(m, null);

                    //int Id = (int)m?.GetType().GetProperty("Id")?.GetValue(m, null);
                    //string Nome = (string)m?.GetType().GetProperty("Nome")?.GetValue(m, null);

                    foreach (var n in multEscolha)
                    {
                        if (n.IdTabelaExterna == Id)
                        {
                            achou = true;
                            tempObjeto = n;

                            listaRetorno.Add(new MultiplaEscolhaDTO()
                            {
                                Id = n.Id,
                                IdTabelaExterna = Id,
                                Nome = Nome,
                                Cor = n.Cor
                            });

                            break;
                        }
                    }

                    multEscolha.Remove(tempObjeto);

                    if (!achou)
                    {
                        listaNaoContem.Add(new MultiplaEscolha()
                        {
                            Ativo = true,
                            Cor = string.Empty,
                            DataAlteracao = null,
                            DataCriacao = DateTime.Now,
                            Id = 0,
                            IdCampo = campo.Id,
                            IdTabelaExterna = Id,
                            Nome = Nome,
                            NomeTabelaExterna = campo.Nome
                        });
                    }
                }

                if (listaNaoContem.Count > 0)
                {
                    listaRetorno.AddRange(AutoMapperUtil.Map<List<MultiplaEscolhaDTO>>(_PlanoAcaoData.SalvarMultiplaEscolha(listaNaoContem)));
                }

                #endregion

                return listaRetorno;

            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "BuscarCamposJbs=" + request.Tabela, ex);
                throw ex;
            }

        }

        public List<MultiplaEscolhaDTO> BuscarListaMultiplaEscolhaFilho(MultiplaEscolhaDTO mult)
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();
            try
            {
                var result = AutoMapperUtil.Map<List<MultiplaEscolhaDTO>>(_PlanoAcaoData.BuscarListaMultiplaEscolhaFilho(mult.Id, mult.IdCampo));

                return result;
            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "BuscarListaMultiplaEscolhaFilho", ex);
                throw ex;
            }
        }

        public List<TarefaDTO> BuscarTodasTarefas()
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();

            try
            {

                var result = AutoMapperUtil.Map<List<TarefaDTO>>(_PlanoAcaoData.BuscarTodasTarefas());


                if (result != null)
                {
                    //Remove os vinculos das tarefas onde o campo esta desativado
                    var listRemover = new List<VinculoCampoTarefaDTO>();
                    foreach (var a in result)
                    {
                        foreach (var b in a.VinculoCampoTarefa)
                        {
                            if (b.Campo == null)
                            {
                                listRemover.Add(b);
                            }
                        }
                    }
                    foreach (var a in listRemover)
                    {
                        result.ForEach(x => x.VinculoCampoTarefa.Remove(a));
                    }

                    //Organizar por sequencia os campos
                    result.ForEach(x => x.VinculoCampoTarefa = x.VinculoCampoTarefa.OrderBy(y => y.Campo != null ? y.Campo.Sequencia.Value : 0).ToList());
                }

                return result;

            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "BuscarTodasTarefas", ex);
                throw ex;
            }
        }

        public TarefaDTO BuscarTarefaPorId(int id)
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();
            try
            {

                var result = AutoMapperUtil.Map<TarefaDTO>(_PlanoAcaoData.BuscarTarefaPorId(id));


                if (result != null)
                {
                    //Remove os vinculos das tarefas onde o campo esta desativado
                    var listRemover = new List<VinculoCampoTarefaDTO>();
                    foreach (var b in result.VinculoCampoTarefa)
                    {
                        if (b.Campo == null)
                        {
                            listRemover.Add(b);
                        }
                    }

                    foreach (var a in listRemover)
                    {
                        result.VinculoCampoTarefa.Remove(a);
                    }

                    //Organizar por sequencia os campos
                    result.VinculoCampoTarefa = result.VinculoCampoTarefa.OrderBy(y => y.Campo != null ? y.Campo.Sequencia.Value : 0).ToList();
                }

                return result;

            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "BuscarTarefaPorId", ex);
                throw ex;
            }
        }

        public List<CampoDTO> BuscarTodosCamposProjetoPorId(int id)
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();
            try
            {

                var result = AutoMapperUtil.Map<List<CampoDTO>>(_PlanoAcaoData.BuscarTodosCamposProjetoPorId(id));

                //Organizar por sequencia os campos
                result = result.OrderBy(x => x.Sequencia.Value != null ? x.Sequencia.Value : 0).ToList();

                //Organizar por se fixado a esquerda
                result = result.OrderBy(x => x.FixadoEsquerda.Value == false).ToList();

                return result;
            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "BuscarTodosCamposProjetoPorId", ex);
                throw ex;
            }
        }

        public List<ParticipanteDTO> BuscarParticipantesCampoTipoPerson()
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();
            try
            {
                return AutoMapperUtil.Map<List<ParticipanteDTO>>(_PlanoAcaoData.BuscarParticipantesCampoTipoPerson());
            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "BuscarParticipantesCampoTipoPerson", ex);
                throw ex;
            }
        }

        public List<AcompanhamentoTarefaDTO> BuscarAcompanhamentosTarefaId(int Id)
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData(); LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness(); try
            {
                var result = AutoMapperUtil.Map<List<AcompanhamentoTarefaDTO>>(_PlanoAcaoData.BuscarAcompanhamentosTarefaId(Id));

                result.ForEach(x => x.DataEnvioString = x.DataEnvio.ToString());

                result = result.OrderByDescending(x => x.DataEnvio).ToList();

                return result;
            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "BuscarAcompanhamentosTarefaId", ex);
                throw ex;
            }
        }

        public List<GrupoCabecalhoDTO> BuscarListaGrupoCabecalhoProjetoPorId(int id)
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData(); LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness(); try
            {
                var result = AutoMapperUtil.Map<List<GrupoCabecalhoDTO>>(_PlanoAcaoData.BuscarListaGrupoCabecalhoProjetoPorId(id));

                return result;
            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "BuscarListaGrupoCabecalhoProjetoPorId", ex);
                throw ex;
            }
        }

        #endregion

        #region AÇÕES


        public CabecalhoDTO SalvarCabecalho(CabecalhoDTO cabecalho)
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();
            try
            {
                var cab = new Campo()
                {
                    Id = cabecalho.Id,
                    Ativo = true,
                    DataCriacao = DateTime.Now,
                    DataAlteracao = null,
                    IdParticipanteCriador = cabecalho.IdParticipanteCriador,
                    IdProjeto = cabecalho.IdProjeto,
                };

                cab = _PlanoAcaoData.SalvarCabecalho(cab);

                var listVinc = new List<VinculoCampoCabecalho>();

                foreach (var i in cabecalho.VinculoCampoCabecalho)
                {
                    listVinc.Add(new VinculoCampoCabecalho()
                    {
                        Id = 0,
                        IdCampo = i.IdCampo,
                        IdMultiplaEscolha = i.IdMultiplaEscolha == 0 ? null : i.IdMultiplaEscolha,
                        IdParticipante = i.IdParticipante == 0 ? null : i.IdParticipante,
                        IdCabecalho = cab.Id,
                        Valor = i.Valor,
                        IdGrupoCabecalho = i.IdGrupoCabecalho
                    });
                }

                cabecalho.VinculoCampoCabecalho = AutoMapperUtil.Map<List<VinculoCampoCabecalhoDTO>>(_PlanoAcaoData.SalvarVinculoCampoCabecalho(listVinc));

                cabecalho.Id = cab.Id;

                return cabecalho;
            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "SalvarCabecalho", ex);
                throw ex;
            }
        }

        public TarefaDTO SalvarTarefa(TarefaDTO tarefa)
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();
            try
            {
                var tar = new TarefaPA()
                {
                    Id = tarefa.Id,
                    Ativo = true,
                    DataCriacao = DateTime.Now,
                    DataAlteracao = null,
                    IdParticipanteCriador = tarefa.IdParticipanteCriador,
                    IdProjeto = tarefa.IdProjeto,
                };

                tar = _PlanoAcaoData.SalvarTarefa(tar);

                var listVinc = new List<VinculoCampoTarefa>();

                foreach (var i in tarefa.VinculoCampoTarefa)
                {
                    listVinc.Add(new VinculoCampoTarefa()
                    {
                        Id = i.Id,
                        IdCampo = i.IdCampo,
                        IdMultiplaEscolha = i.IdMultiplaEscolha == 0 ? null : i.IdMultiplaEscolha,
                        IdParticipante = i.IdParticipante == 0 ? null : i.IdParticipante,
                        IdTarefa = tar.Id,
                        Valor = i.Valor
                    });
                }

                tarefa.VinculoCampoTarefa = AutoMapperUtil.Map<List<VinculoCampoTarefaDTO>>(_PlanoAcaoData.SalvarVinculoCampoTarefa(listVinc));

                return tarefa;
            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "SalvarTarefa", ex);
                throw ex;
            }
        }

        public TarefaDTO AlterarTarefa(TarefaDTO tarefa)
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();
            try
            {

                var tar = _PlanoAcaoData.AlterarTarefa(AutoMapperUtil.Map<TarefaPA>(tarefa));

                var listVinc = new List<VinculoCampoTarefa>();

                foreach (var i in tarefa.VinculoCampoTarefa)
                {
                    listVinc.Add(new VinculoCampoTarefa()
                    {
                        Id = i.Id,
                        IdCampo = i.IdCampo,
                        IdMultiplaEscolha = i.IdMultiplaEscolha == 0 ? null : i.IdMultiplaEscolha,
                        IdParticipante = i.IdParticipante == 0 ? null : i.IdParticipante,
                        IdTarefa = tar.Id,
                        Valor = i.Valor
                    });
                }

                tarefa.VinculoCampoTarefa = AutoMapperUtil.Map<List<VinculoCampoTarefaDTO>>(_PlanoAcaoData.AlterarVinculoCampoTarefa(listVinc));

                return tarefa;
            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "AlterarTarefa", ex);
                throw ex;
            }
        }

        public TarefaDTO SalvarAcompanhamentoTarefa(TarefaDTO tarefa)
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();
            try
            {
                var emails = new List<string>();

                var acompanhamentoTarefa = new AcompanhamentoTarefaDTO();

                var listVinc = new List<VinculoCampoTarefa>();

                foreach (var i in tarefa.VinculoCampoTarefa)
                {
                    listVinc.Add(new VinculoCampoTarefa()
                    {
                        Id = i.Id,
                        IdCampo = i.IdCampo,
                        IdMultiplaEscolha = i.IdMultiplaEscolha == 0 ? null : i.IdMultiplaEscolha,
                        IdParticipante = i.IdParticipante == 0 ? null : i.IdParticipante,
                        IdTarefa = tarefa.Id,
                        Valor = i.Valor
                    });
                }


                var listAcom = new List<AcompanhamentoTarefa>();

                foreach (var i in tarefa.AcompanhamentoTarefa)
                {

                    acompanhamentoTarefa = i;

                    if (!string.IsNullOrEmpty(i.Enviado))
                    {
                        emails = i.Enviado.Split(';').ToList();

                        emails = emails.Select(x => x.Trim()).ToList();

                        emails.RemoveAll(x => x.Trim() == string.Empty);

                    }

                    var participanteEnvio = _PlanoAcaoData.BuscarParticipantePorId(i.IdParticipanteEnvio);

                    listAcom.Add(new AcompanhamentoTarefa()
                    {
                        Id = i.Id,
                        IdTarefa = tarefa.Id,
                        Ativo = true,
                        Comentario = i.Comentario,
                        DataCriacao = DateTime.Now,
                        DataEnvio = DateTime.Now,
                        Enviado = i.Enviado,
                        IdParticipanteEnvio = i.IdParticipanteEnvio,
                        NomeParticipanteEnvio = participanteEnvio.Nome,
                        Status = i.Status
                    });
                }

                tarefa.AcompanhamentoTarefa = AutoMapperUtil.Map<List<AcompanhamentoTarefaDTO>>(_PlanoAcaoData.SalvarAcompanhamentoTarefa(listAcom));

                tarefa.VinculoCampoTarefa = AutoMapperUtil.Map<List<VinculoCampoTarefaDTO>>(_PlanoAcaoData.AlterarVinculoCampoTarefa(listVinc));

                if (emails.Count > 0)
                {
                    EnviarEmailAcompanhamentoTarefa(emails, acompanhamentoTarefa);
                }

                return tarefa;
            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "SalvarAcompanhamentoTarefa", ex);
                throw ex;
            }
        }

        public TarefaDTO DesativarTarefaPorId(int idTarefa)
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();
            try
            {
                return AutoMapperUtil.Map<TarefaDTO>(_PlanoAcaoData.DesativarTarefaPorId(idTarefa));
            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "DesativarTarefaPorId", ex);
                throw ex;
            }
        }

        #endregion

        #region AUXILIARES

        private void EnviarEmailAcompanhamentoTarefa(List<string> emails, AcompanhamentoTarefaDTO acompanhamentoTarefa)
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();

            var config = _PlanoAcaoData.BuscarConfiguracaoEmail();

            var mail = new MailMessage();
            mail.Subject = "Plano de Ação";
            mail.From = new MailAddress(config.Email);
            mail.Body = acompanhamentoTarefa.Comentario != null ? acompanhamentoTarefa.Comentario : string.Empty;
            foreach (var i in emails)
            {
                mail.To.Add(i);
            }

            using (var client = new SmtpClient(config.Host, config.Port)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = false,
                Credentials = new NetworkCredential(config.Email, config.Senha)
            })
            {
                try
                {
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    client.Dispose();
                    //  _logOperacaoBusiness.SalvarLogOperacao(0, "EnviarEmailAcompanhamentoTarefa", ex);
                    throw ex;
                }
                finally
                {
                    client.Dispose();
                }
            }
        }


        #endregion

        #region MOCK

        public List<ResultCabecalhoMock> BuscarCabecalhoMOCK(RequestCabecalhoMock request)
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();
            try
            {
                var query = " SELECT  " +
                            " * " +
                            " FROM  " +
                            " ( " +
                            " SELECT " +
                            " MAX(\"Diretoria\")  AS \"Diretoria\", " +
                            " MAX(\"Gerência\")  AS \"Gerência\", " +
                            " MAX(\"Coordenação\")  AS \"Coordenação\", " +
                            " MAX(\"Missão\")  AS \"Missão\", " +
                            " MAX(\"Visão\")  AS \"Visão\", " +
                            " MAX(\"Dimenção\")  AS \"Dimenção\", " +
                            " MAX(\"Tema / Assunto\")  AS \"Tema / Assunto\", " +
                            " MAX(\"Objetivo\")  AS \"Objetivo\", " +
                            " MAX(\"Indicadores\")  AS \"Indicadores\", " +
                            " MAX(\"Objetivo Gerencial\")  AS \"Objetivo Gerencial\", " +
                            " MAX(\"Valor de\")  AS \"Valor de\", " +
                            " MAX(\"Valor para\")  AS \"Valor para\", " +
                            " MAX(\"Data (Início)\")  AS \"Data (Início)\", " +
                            " MAX(\"Data (Fim)\")  AS \"Data (Fim)\" " +
                            " FROM " +
                            " ( " +
                            " SELECT " +
                            " CB.ID, " +
                            " C.ID AS IDCAMPO, " +
                            " CASE WHEN C.Nome =  'Diretoria'  then  VCC.Valor  else null end as  'Diretoria' , " +
                            " CASE WHEN C.Nome =  'Gerência'  then  VCC.Valor  else null end as  'Gerência' , " +
                            " CASE WHEN C.Nome =  'Coordenação'  then  VCC.Valor  else null end as  'Coordenação' , " +
                            " CASE WHEN C.Nome =  'Missão'  then  VCC.Valor  else null end as  'Missão' , " +
                            " CASE WHEN C.Nome =  'Visão'  then  VCC.Valor  else null end as  'Visão' , " +
                            " CASE WHEN C.Nome =  'Dimenção'  then  VCC.Valor  else null end as  'Dimenção' , " +
                            " CASE WHEN C.Nome =  'Tema / Assunto'  then  VCC.Valor  else null end as  'Tema / Assunto' , " +
                            " CASE WHEN C.Nome =  'Objetivo'  then  VCC.Valor  else null end as  'Objetivo' , " +
                            " CASE WHEN C.Nome =  'Indicadores'  then  VCC.Valor  else null end as  'Indicadores' , " +
                            " CASE WHEN C.Nome =  'Objetivo Gerencial'  then  VCC.Valor  else null end as  'Objetivo Gerencial' , " +
                            " CASE WHEN C.Nome =  'Valor de'  then  VCC.Valor  else null end as  'Valor de' , " +
                            " CASE WHEN C.Nome =  'Valor para'  then  VCC.Valor  else null end as  'Valor para' , " +
                            " CASE WHEN C.Nome =  'Data (Início)'  then  VCC.Valor  else null end as  'Data (Início)' , " +
                            " CASE WHEN C.Nome =  'Data (Fim)'  then  VCC.Valor  else null end as  'Data (Fim)'  " +
                            " FROM CABECALHO CB " +
                            " LEFT JOIN VINCULOCAMPOCABECALHO VCC " +
                            " ON VCC.IDCABECALHO = CB.ID " +
                            " LEFT JOIN CAMPO C " +
                            " ON C.ID = VCC.IDCAMPO " +
                            " LEFT JOIN grupocabecalho GC " +
                            " ON GC.ID = C.idGrupoCabecalho " +
                            " LEFT JOIN grupocabecalho GC_PAI " +
                            " ON GC_PAI.ID = GC.idgrupocabecalhopai " +
                            " ) vetor " +
                            " GROUP BY ID " +
                            " ) VAL_CAB " +
                            " WHERE 1=1 " +
                           (!string.IsNullOrEmpty(request.Diretoria) ? " and \"Diretoria\" = '" + request.Diretoria + "' " : "").ToString() +
                           (!string.IsNullOrEmpty(request.Gerencia) ? " and \"Gerência\" = '" + request.Gerencia + "' " : "").ToString() +
                           (!string.IsNullOrEmpty(request.Coordenacao) ? " and \"Coordenação\" = '" + request.Coordenacao + "' " : "").ToString() +
                           (!string.IsNullOrEmpty(request.TemaAssunto) ? " and \"Tema / Assunto\" = '" + request.TemaAssunto + "' " : "").ToString() +
                           (!string.IsNullOrEmpty(request.Objetivo) ? " and \"Objetivo\" = '" + request.Objetivo + "' " : "").ToString() +
                           (!string.IsNullOrEmpty(request.Indicadores) ? " and \"Indicadores\" = '" + request.Indicadores + "' " : "").ToString();


                var result = _PlanoAcaoData.BuscarCabecalhoMOCK(query);


                if (!string.IsNullOrEmpty(request.Diretoria))
                {
                    var listRemove = result.Select(x => x).Where(x => x.NomeCampo.Equals("Diretoria")).ToList();
                    foreach (var i in listRemove)
                    {
                        result.Remove(i);
                    }
                }
                if (!string.IsNullOrEmpty(request.Gerencia))
                {
                    var listRemove = result.Select(x => x).Where(x => x.NomeCampo.Equals("Gerência")).ToList();
                    foreach (var i in listRemove)
                    {
                        result.Remove(i);
                    }
                }
                if (!string.IsNullOrEmpty(request.Coordenacao))
                {
                    var listRemove = result.Select(x => x).Where(x => x.NomeCampo.Equals("Coordenação")).ToList();
                    foreach (var i in listRemove)
                    {
                        result.Remove(i);
                    }
                }
                if (!string.IsNullOrEmpty(request.TemaAssunto))
                {
                    var listRemove = result.Select(x => x).Where(x => x.NomeCampo.Equals("Tema / Assunto")).ToList();
                    foreach (var i in listRemove)
                    {
                        result.Remove(i);
                    }
                }
                if (!string.IsNullOrEmpty(request.Objetivo))
                {
                    var listRemove = result.Select(x => x).Where(x => x.NomeCampo.Equals("Objetivo")).ToList();
                    foreach (var i in listRemove)
                    {
                        result.Remove(i);
                    }
                }
                else if (!string.IsNullOrEmpty(request.Indicadores))
                {
                    var listRemove = result.Select(x => x).Where(x => x.NomeCampo.Equals("Indicadores")).ToList();
                    foreach (var i in listRemove)
                    {
                        result.Remove(i);
                    }
                }



                return result;
            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "BuscarCabecalhoMOCK", ex);
                throw ex;
            }
        }

        public MultiplaEscolhaDTO SalvarAddCampoDropDown(MultiplaEscolhaDTO multiplaEscolha)
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();
            try
            {
                multiplaEscolha.DataCriacao = DateTime.Now;

                var result = _PlanoAcaoData.SalvarAddCampoDropDown(AutoMapperUtil.Map<MultiplaEscolha>(multiplaEscolha));

                return AutoMapperUtil.Map<MultiplaEscolhaDTO>(result);

            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "SalvarAddCampoDropDown", ex);
                throw ex;
            }
        }

        public CabecalhoDTO BuscarCabecalhoPorId(int id)
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();
            try
            {
                var result = AutoMapperUtil.Map<CabecalhoDTO>(_PlanoAcaoData.BuscarCabecalhoPorId(id));
                return result;
            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "BuscarCabecalhoPorId", ex);
                throw ex;
            }
        }

        public List<ResultCabecalhoMock> BuscarCabecalhoPorIdConcatenado(string ids)
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();
            try
            {
                if (!string.IsNullOrEmpty(ids))
                {
                    var idStirng = ids.Split(',');

                    var retorno = new List<ResultCabecalhoMock>();

                    for (var j = 0; j < idStirng.Length; j++)
                    {
                        var result = AutoMapperUtil.Map<CabecalhoDTO>(_PlanoAcaoData.BuscarCabecalhoPorId(Convert.ToInt32(idStirng[j].Trim())));

                        var temp = new ResultCabecalhoMock();
                        temp.Valor = result.Id.ToString();

                        if(j == 4)
                        {

                        }

                        foreach (var i in result.VinculoCampoCabecalho)
                        {

                            if (!string.IsNullOrEmpty(i.Valor))
                            {
                                temp.NomeCampo += i.Campo.Nome + ": " + i.Valor.Substring(0, (i.Valor.Length > 15 ? 15 : i.Valor.Length)) + (i.Valor.Length > 15 ? "..." : "");
                                temp.NomeCampo += "\n";
                            }            
                        }

                        retorno.Add(temp);

                    }


                    return retorno;
                }

                return new List<ResultCabecalhoMock>();
            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "BuscarCabecalhoPorId", ex);
                throw ex;
            }
        }

        public List<CabecalhoDTO> BuscarTodosCabecalhos()
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();
            try
            {
                var result = AutoMapperUtil.Map<List<CabecalhoDTO>>(_PlanoAcaoData.BuscarTodosCabecalhos());
                return result;
            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "BuscarTodosCabecalhos", ex);
                throw ex;
            }
        }

        public List<ResultCabecalhoMock> BuscarCabecalhoFiltradoMOCK(RequestCabecalhoMock request)
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();
            try
            {
                var query = " SELECT  " +
                            " ID " +
                            " FROM  " +
                            " ( " +
                            " SELECT " +
                            " ID , " +
                            " MAX(\"Diretoria\")  AS \"Diretoria\", " +
                            " MAX(\"Gerência\")  AS \"Gerência\", " +
                            " MAX(\"Coordenação\")  AS \"Coordenação\", " +
                            " MAX(\"Missão\")  AS \"Missão\", " +
                            " MAX(\"Visão\")  AS \"Visão\", " +
                            " MAX(\"Dimenção\")  AS \"Dimenção\", " +
                            " MAX(\"Tema / Assunto\")  AS \"Tema / Assunto\", " +
                            " MAX(\"Objetivo\")  AS \"Objetivo\", " +
                            " MAX(\"Indicadores\")  AS \"Indicadores\", " +
                            " MAX(\"Objetivo Gerencial\")  AS \"Objetivo Gerencial\", " +
                            " MAX(\"Valor de\")  AS \"Valor de\", " +
                            " MAX(\"Valor para\")  AS \"Valor para\", " +
                            " MAX(\"Data (Início)\")  AS \"Data (Início)\", " +
                            " MAX(\"Data (Fim)\")  AS \"Data (Fim)\" " +
                            " FROM " +
                            " ( " +
                            " SELECT " +
                            " CB.ID, " +
                            " C.ID AS IDCAMPO, " +
                            " CASE WHEN C.Nome =  'Diretoria'  then  VCC.Valor  else null end as  'Diretoria' , " +
                            " CASE WHEN C.Nome =  'Gerência'  then  VCC.Valor  else null end as  'Gerência' , " +
                            " CASE WHEN C.Nome =  'Coordenação'  then  VCC.Valor  else null end as  'Coordenação' , " +
                            " CASE WHEN C.Nome =  'Missão'  then  VCC.Valor  else null end as  'Missão' , " +
                            " CASE WHEN C.Nome =  'Visão'  then  VCC.Valor  else null end as  'Visão' , " +
                            " CASE WHEN C.Nome =  'Dimenção'  then  VCC.Valor  else null end as  'Dimenção' , " +
                            " CASE WHEN C.Nome =  'Tema / Assunto'  then  VCC.Valor  else null end as  'Tema / Assunto' , " +
                            " CASE WHEN C.Nome =  'Objetivo'  then  VCC.Valor  else null end as  'Objetivo' , " +
                            " CASE WHEN C.Nome =  'Indicadores'  then  VCC.Valor  else null end as  'Indicadores' , " +
                            " CASE WHEN C.Nome =  'Objetivo Gerencial'  then  VCC.Valor  else null end as  'Objetivo Gerencial' , " +
                            " CASE WHEN C.Nome =  'Valor de'  then  VCC.Valor  else null end as  'Valor de' , " +
                            " CASE WHEN C.Nome =  'Valor para'  then  VCC.Valor  else null end as  'Valor para' , " +
                            " CASE WHEN C.Nome =  'Data (Início)'  then  VCC.Valor  else null end as  'Data (Início)' , " +
                            " CASE WHEN C.Nome =  'Data (Fim)'  then  VCC.Valor  else null end as  'Data (Fim)'  " +
                            " FROM CABECALHO CB " +
                            " LEFT JOIN VINCULOCAMPOCABECALHO VCC " +
                            " ON VCC.IDCABECALHO = CB.ID " +
                            " LEFT JOIN CAMPO C " +
                            " ON C.ID = VCC.IDCAMPO " +
                            " LEFT JOIN grupocabecalho GC " +
                            " ON GC.ID = C.idGrupoCabecalho " +
                            " LEFT JOIN grupocabecalho GC_PAI " +
                            " ON GC_PAI.ID = GC.idgrupocabecalhopai " +
                            " ) vetor " +
                            " GROUP BY ID " +
                            " ) VAL_CAB " +
                            " WHERE 1=1 " +
                            (request.IdCabecalho == null ? "" : request.IdCabecalho == 0 ? "" : " and ID = " + request.IdCabecalho.ToString()).ToString() +
                            (!string.IsNullOrEmpty(request.Diretoria) ? " and \"Diretoria\" = '" + request.Diretoria + "' " : "").ToString() +
                            (!string.IsNullOrEmpty(request.Gerencia) ? " and \"Gerência\" = '" + request.Gerencia + "' " : "").ToString() +
                            (!string.IsNullOrEmpty(request.Coordenacao) ? " and \"Coordenação\" = '" + request.Coordenacao + "' " : "").ToString() +
                            (!string.IsNullOrEmpty(request.Missao) ? " and \"Missão\" = '" + request.Missao + "' " : "").ToString() +
                            (!string.IsNullOrEmpty(request.Visao) ? " and \"Visão\" = '" + request.Visao + "' " : "").ToString() +
                            (!string.IsNullOrEmpty(request.Dimencao) ? " and \"Dimenção\" = '" + request.Dimencao + "' " : "").ToString() +
                            (!string.IsNullOrEmpty(request.TemaAssunto) ? " and \"Tema / Assunto\" = '" + request.TemaAssunto + "' " : "").ToString() +
                            (!string.IsNullOrEmpty(request.Objetivo) ? " and \"Objetivo\" = '" + request.Objetivo + "' " : "").ToString() +
                            (!string.IsNullOrEmpty(request.Indicadores) ? " and \"Indicadores\" = '" + request.Indicadores + "' " : "").ToString() +
                            (!string.IsNullOrEmpty(request.MetaObjetivoGerencial) ? " and \"Objetivo Gerencial\" = '" + request.MetaObjetivoGerencial + "' " : "").ToString() +
                            (!string.IsNullOrEmpty(request.MetaValorde) ? " and \"Valor de\" = '" + request.MetaValorde + "' " : "").ToString() +
                            (!string.IsNullOrEmpty(request.MetaValorpara) ? " and \"Valor para\" = '" + request.MetaValorpara + "' " : "").ToString() +
                            (!string.IsNullOrEmpty(request.MetaInicio) ? " and \"Data (Início)\" = '" + request.MetaInicio + "' " : "").ToString() +
                            (!string.IsNullOrEmpty(request.MetaFim) ? " and \"Data (Fim)\" = '" + request.MetaFim + "' " : "").ToString();

                var result = _PlanoAcaoData.BuscarCabecalhoFiltradoMOCK(query);

                return result;
            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "BuscarCabecalhoFiltradoMOCK", ex);
                throw ex;
            }
        }


        #endregion








        #region Relatorios


        public ObjetoGrafico PopularGraficosComFiltroBusca(int idOque, int idfiltro, List<string> valoresFiltro)
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();

            try
            {
                var resultVinculo = _PlanoAcaoData.PopularGraficosComFiltroBusca(idOque, idfiltro, valoresFiltro);


                if (valoresFiltro != null)
                {
                    List<int> ids = resultVinculo.Where(p => valoresFiltro.Contains(p.Valor)).Select(x => x.IdTarefa).ToList();

                    var listRemove = resultVinculo.Where(x => !ids.Contains(x.IdTarefa)).ToList();

                    foreach (var r in listRemove)
                    {
                        resultVinculo.Remove(r);
                    }
                }



                //var resultDistinct = resultVinculo.GroupBy(p => new { p.IdTarefa, p.IdCampo })
                //                          .Select(g => g.First())
                //                          .ToList();


                var seriesValor = resultVinculo.GroupBy(p => p.Valor)
                                           .Select(g => g.FirstOrDefault())
                                           .Where(x => x.IdCampo == idOque)
                                           .ToList();

                var series = seriesValor.Select(x => new Series()
                {
                    Descricao = x.Valor,
                    Quantidade = 0
                }).ToList();


                var categoriasValor = resultVinculo.GroupBy(p => p.Valor)
                                            .Select(g => g.FirstOrDefault())
                                            .Where(x => x.IdCampo == idfiltro)
                                            .ToList();

                var categoriasFiltro = resultVinculo.Select(g => g)
                                            .Where(x => x.IdCampo == idfiltro)
                                            .ToList();


                foreach (var a in series)
                {
                    foreach (var b in resultVinculo)
                    {
                        if (b.Valor.Equals(a.Descricao) && b.IdCampo == idOque)
                        {
                            a.Quantidade++;
                        }
                    }
                }







                var categorias = new List<string>();

                for (var k = 0; k < categoriasValor.Count; k++)
                {

                }




                var seriesCategoria = new List<Series>();




                for (var i = 0; i < categoriasValor.Count; i++)
                {

                    categorias.Add(categoriasValor[i].Valor);

                    var Data = new List<int>();

                    for (var k = 0; k < categoriasValor.Count; k++)
                    {
                        Data.Add(0);
                    }


                    foreach (var a in categoriasFiltro)
                    {

                        if (a.Valor.Equals(categoriasValor[i].Valor))
                        {

                            foreach (var b in resultVinculo)
                            {
                                if (a.IdTarefa == b.IdTarefa && b.IdCampo == idOque)
                                {
                                    if (seriesCategoria.Where(x => x.Descricao.Equals(b.Valor)).Count() > 0)
                                    {
                                        foreach (var u in seriesCategoria.Where(x => x.Descricao.Equals(b.Valor)))
                                        {
                                            u.Data[i]++;
                                        }
                                    }
                                    else
                                    {
                                        var temp = new Series()
                                        {
                                            Descricao = b.Valor,
                                            Quantidade = 0,
                                            Data = Data
                                        };

                                        temp.Data[i]++;

                                        seriesCategoria.Add(temp);
                                    }

                                }
                            }


                        }


                    }


                }

                var objetoGrafico = new ObjetoGrafico()
                {
                    Descricao = seriesValor.FirstOrDefault().Campo.Nome,
                    Series = series,
                    Categorias = categorias,
                    SeriesCategoria = seriesCategoria
                };

                return objetoGrafico;
            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "PopularGraficosComFiltroBusca", ex);
                throw ex;
            }
        }

        public List<QuantidadeStatus> BuscarStatusPorTemaAssunto()
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();
            try
            {
                var temasAssuntos = new List<QuantidadeStatus>();

                var result = _PlanoAcaoData.BuscarQuantidadeTodosStatus();


                var achouTemaAssunto = false;
                var achouStatus = false;

                for (var i = 0; i < result.Count;)
                {

                    var temp = new QuantidadeStatus();

                    foreach (var j in result[i].VinculoCampoTarefa)
                    {
                        if (j.IdCampo == 7 && !achouTemaAssunto) //campo tema assunto
                        {
                            if (temasAssuntos.Where(x => x.TemaAssunto.Equals(j.Valor)).FirstOrDefault() == null)
                            {
                                achouTemaAssunto = true;
                                temp.TemaAssunto = j.Valor;
                            }
                            else
                            {
                                var vartempRemover = temasAssuntos.Where(x => x.TemaAssunto.Equals(j.Valor)).FirstOrDefault();
                                temasAssuntos.Remove(vartempRemover);

                                temp.Ativo += vartempRemover.Ativo;
                                temp.Cancelado += vartempRemover.Cancelado;
                                temp.ConcluidoAtrasado += vartempRemover.ConcluidoAtrasado;
                                temp.ConcluidoPrazo += vartempRemover.ConcluidoPrazo;
                                temp.Replanejado += vartempRemover.Replanejado;
                                temp.TemaAssunto = vartempRemover.TemaAssunto;

                                achouTemaAssunto = true;
                            }
                        }
                        else if (j.IdCampo == 34 && !achouStatus) // campo status
                        {
                            achouStatus = true;

                            if (j.Valor.Equals("Ativo"))
                            {
                                temp.Ativo++;
                            }
                            else if (j.Valor.Equals("Cancelado"))
                            {
                                temp.Cancelado++;
                            }
                            else if (j.Valor.Equals("Concluido (Prazo)"))
                            {
                                temp.ConcluidoPrazo++;
                            }
                            else if (j.Valor.Equals("Concluido (Atrasado)"))
                            {
                                temp.ConcluidoAtrasado++;
                            }
                            else if (j.Valor.Equals("Replanejado"))
                            {
                                temp.Replanejado++;
                            }
                        }

                        if (achouTemaAssunto && achouStatus)
                        {
                            break;
                        }
                    }

                    if (achouTemaAssunto && achouStatus)
                    {
                        temasAssuntos.Add(temp);
                        achouTemaAssunto = false;
                        achouStatus = false;
                        i++;
                    }

                }

                //temasAssuntos = temasAssuntos
                //               .GroupBy(p => p.TemaAssunto)
                //               .Select(g => g.First())
                //               .ToList();

                return temasAssuntos;
            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "BuscarStatusPorTemaAssunto", ex);
                throw ex;
            }
        }

        public List<QuantidadeStatus> BuscarStatusPorUsuario()
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();
            try
            {
                var usuarios = new List<QuantidadeStatus>();

                var result = _PlanoAcaoData.BuscarQuantidadeTodosStatus();


                var achouUsuario = false;
                var achouStatus = false;

                for (var i = 0; i < result.Count;)
                {

                    var temp = new QuantidadeStatus();

                    foreach (var j in result[i].VinculoCampoTarefa)
                    {
                        if (j.IdCampo == 26 && !achouUsuario) //campo quem
                        {
                            if (j.Usuarios != null)
                            {
                                if (usuarios.Where(x => x.NomeUsuario.Equals(j.Usuarios.Nome)).FirstOrDefault() == null)
                                {
                                    achouUsuario = true;
                                    temp.NomeUsuario = j.Usuarios.Nome;
                                }
                                else
                                {
                                    var vartempRemover = usuarios.Where(x => x.NomeUsuario.Equals(j.Usuarios.Nome)).FirstOrDefault();
                                    usuarios.Remove(vartempRemover);

                                    temp.Ativo += vartempRemover.Ativo;
                                    temp.Cancelado += vartempRemover.Cancelado;
                                    temp.ConcluidoAtrasado += vartempRemover.ConcluidoAtrasado;
                                    temp.ConcluidoPrazo += vartempRemover.ConcluidoPrazo;
                                    temp.Replanejado += vartempRemover.Replanejado;
                                    temp.NomeUsuario = vartempRemover.NomeUsuario;

                                    achouUsuario = true;
                                }
                            }
                            else
                            {
                                achouUsuario = true;
                                temp.NomeUsuario = "";
                            }
                        }
                        else if (j.IdCampo == 34 && !achouStatus) // campo status
                        {
                            achouStatus = true;

                            if (j.Valor.Equals("Ativo"))
                            {
                                temp.Ativo++;
                            }
                            else if (j.Valor.Equals("Cancelado"))
                            {
                                temp.Cancelado++;
                            }
                            else if (j.Valor.Equals("Concluido (Prazo)"))
                            {
                                temp.ConcluidoPrazo++;
                            }
                            else if (j.Valor.Equals("Concluido (Atrasado)"))
                            {
                                temp.ConcluidoAtrasado++;
                            }
                            else if (j.Valor.Equals("Replanejado"))
                            {
                                temp.Replanejado++;
                            }
                        }

                        if (achouUsuario && achouStatus)
                        {
                            break;
                        }
                    }

                    if (achouUsuario && achouStatus)
                    {
                        usuarios.Add(temp);
                        achouUsuario = false;
                        achouStatus = false;
                        i++;
                    }

                }

                return usuarios;
            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "BuscarStatusPorUsuario", ex);
                throw ex;
            }
        }

        public List<SelectListItem> PopularDropDownValorFiltro(int id)
        {
            PlanoAcaoData _planoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();
            try
            {
                var result = _planoAcaoData.PopularDropDownValorFiltro(id);

                return result;
            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "PopularDropDownValorFiltro", ex);
                throw ex;
            }
        }

        public ObjetoGrafico PopularGraficoAcoesEstabelecidas()
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();

            try
            {
                var data = _PlanoAcaoData.BuscarNroAcoesEstabelecidas();

                var series = data.Select(x => new Series()
                {
                    Descricao = x.Mes,
                    Quantidade = x.Quantidade,
                    Data = (from p in data where p.Mes==x.Mes select p.Quantidade).ToList<int>()
                    
                }).ToList();

                List<string> CategoriasValues = data.Select(x => x.Mes).ToList();
                
                var objetoGrafico = new ObjetoGrafico()
                {
                    Descricao = "NÚMERO DE AÇÕES ESTABELECIDAS",
                    Series = series,
                    Categorias = CategoriasValues,
                    SeriesCategoria = series
                };

                return objetoGrafico;
            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "PopularGraficosComFiltroBusca", ex);
                throw ex;
            }
        }

        public ObjetoGrafico PopularGraficoAcoesEstabelecidasConcluidas()
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();

            try
            {
                var data = _PlanoAcaoData.BuscarNroAcoesEstabelecidasConcluidas();

                var series = data.Select(x => new Series()
                {
                    Descricao = x.Mes,
                    Quantidade = x.Quantidade,
                    Data = (from p in data where p.Mes == x.Mes select p.Quantidade).ToList<int>()

                }).ToList();

                List<string> CategoriasValues = data.Select(x => x.Mes).ToList();

                var objetoGrafico = new ObjetoGrafico()
                {
                    Descricao = "NÚMERO DE AÇÕES CONCLUÍDAS",
                    Series = series,
                    Categorias = CategoriasValues,
                    SeriesCategoria = series
                };

                return objetoGrafico;
            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "PopularGraficosComFiltroBusca", ex);
                throw ex;
            }
        }

        public object PopularGraficoEstoqueAcoes()
        {
            PlanoAcaoData _PlanoAcaoData = new PlanoAcaoData();
            LogOperacaoBusiness _logOperacaoBusiness = new LogOperacaoBusiness();

            try
            {
                var estabelecidas = PopularGraficoAcoesEstabelecidas();
                var concluidas = PopularGraficoAcoesEstabelecidasConcluidas();

                object[] array = new object[] { estabelecidas, concluidas };
                return array;

                
            }
            catch (Exception ex)
            {
                _logOperacaoBusiness.SalvarLogOperacao(0, "PopularGraficosComFiltroBusca", ex);
                throw ex;
            }
        }

        #endregion






    }
}
