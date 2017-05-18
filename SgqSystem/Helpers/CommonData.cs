using Dominio;
using DTO;
using DTO.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Helpers
{
    public static class CommonData
    {
        public static int GetNumeroDeFamiliasPorUnidadeDoUsuario(HttpContextBase filterContext, int hashKey)
        {
            using (var db = new SgqDbDevEntities())
            {
                var id = Guard.GetUsuarioLogado_Id(filterContext);
                var user = db.UserSgq.FirstOrDefault(r => r.Id == id);
                var level1 = db.ParLevel1.FirstOrDefault(r => r.hashKey == hashKey);
                var existeAlgum = db.Database.SqlQuery<ParLevel2ControlCompany>("select * from ParLevel2ControlCompany where ParCompany_Id is not null and ParLevel1_Id = " + level1.Id).ToList();

                if (existeAlgum != null && existeAlgum.Count() > 0)
                {
                    var lastDate = existeAlgum.OrderByDescending(r => r.InitDate).FirstOrDefault().InitDate;
                    var counter = existeAlgum.Count(r => r.InitDate == lastDate);
                    return counter;
                }
                else
                {
                    return 0;
                }
            }
        }

        public static int GetNumeroDeFamiliasCorporativo(HttpContextBase filterContext, int hashKey)
        {
            using (var db = new SgqDbDevEntities())
            {
                var id = Guard.GetUsuarioLogado_Id(filterContext);
                var user = db.UserSgq.FirstOrDefault(r => r.Id == id);
                var level1 = db.ParLevel1.FirstOrDefault(r => r.hashKey == hashKey);
                var existeAlgum = db.Database.SqlQuery<ParLevel2ControlCompany>("select * from ParLevel2ControlCompany where ParCompany_Id is null and ParLevel1_Id = " + level1.Id).ToList();

                if (existeAlgum != null && existeAlgum.Count() > 0)
                {
                    var lastDate = existeAlgum.OrderByDescending(r => r.InitDate).FirstOrDefault().InitDate;
                    var counter = existeAlgum.Count(r => r.InitDate == lastDate);
                    return counter;
                }
                else
                {
                    return 0;
                }

            }
        }

        public static UserSgq GetuserSgqLogado(HttpContextBase filterContext, int hashKey)
        {
            using (var db = new SgqDbDevEntities())
            {
                var id = Guard.GetUsuarioLogado_Id(filterContext);
                return db.UserSgq.FirstOrDefault(r => r.Id == id);
            }
        }

        /// <summary>
        /// Retorna as operações por unidade conforme parametrização do cluster
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <returns></returns>
        public static IEnumerable<Tuple<int, string>> GetOperacoesPorUnidadeId(int unidadeId)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                var operacoes = (from u in db.Unidades.AsNoTracking()
                                 join c in db.Clusters.AsNoTracking() on u.Cluster equals c.Sigla
                                 join cd in db.ClusterDepartamentos.AsNoTracking() on c.Legenda equals cd.Cluster
                                 join d in db.Departamentos.AsNoTracking() on cd.Departamento equals d.Nome
                                 join depo in db.DepartamentoOperacoes.AsNoTracking() on d.Id equals depo.Departamento
                                 join ope in db.Operacoes.AsNoTracking() on depo.Operacao equals ope.Id
                                 orderby ope.Nivel
                                 where ((u.Id == unidadeId || unidadeId == 0) && u.Ativa.Value && u.Id != 54)
                                 select new { OperacaoId = ope.Id, Operacao = ope.Nome })
                                .Distinct()
                                .ToList();

                var retorno = from o in operacoes.AsEnumerable()
                              select Tuple.Create(o.OperacaoId, o.Operacao);

                return retorno;
            }
        }

        /// <summary>
        /// Retorna os IDs das unidades que o usuário tem acesso
        /// </summary>
        /// <param name="usuarioId">Id do Usuário</param>
        /// <returns></returns>
        public static IEnumerable<int> GetUnidadesIdPorUsuarioId(int usuarioId)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                var retorno = (from uu in db.UsuarioUnidades.AsNoTracking()
                               where uu.Usuario == usuarioId
                               select uu.Unidade)
                                .Distinct()
                                .ToList();

                return retorno;
            }
        }

        /// <summary>
        /// Retorna as regionais cadastradas
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Tuple<int, string>> GetRegionais()
        {
            using (var db = new SGQ_GlobalEntities())
            {
                var regionais = db.Regionais.AsNoTracking().Select(r => new { RegionalId = r.Id, Regional = r.Nome }).ToList();

                var retorno = regionais.AsEnumerable().Select(r => Tuple.Create(r.RegionalId, r.Regional));

                return retorno;
            }
        }

        /// <summary>
        /// Retorna as unidades por regional
        /// </summary>
        /// <param name="regionaoId">Id da Regional</param>
        /// <returns></returns>
        public static IEnumerable<Tuple<int, string>> GetUnidadesPorRegionalId(int regionaoId)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                var unidades = db.Unidades.AsNoTracking().Where(u => u.Regional.Value == regionaoId).Select(u => new { UnidadeId = u.Id, Unidade = u.Sigla }).ToList();

                var retorno = unidades.AsEnumerable().Select(u => Tuple.Create(u.UnidadeId, u.Unidade));

                return retorno;
            }
        }

        /// <summary>
        /// Retorna todas unidades ativas
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Tuple<int, string>> GetUnidades()
        {
            using (var db = new SGQ_GlobalEntities())
            {
                var unidades = db.Unidades.AsNoTracking()
                    .Where(u => u.Ativa.Value)
                    .Select(u => new
                    {
                        UnidadeId = u.Id,
                        Unidade = u.Sigla
                    })
                    .ToList()
                    .AsEnumerable()
                    .Select(u => Tuple.Create(u.UnidadeId, u.Unidade));

                return unidades;
            }
        }

        /// <summary>
        /// Retorna as tarefas por operação
        /// </summary>
        /// <param name="operacaoId">Id da Operação</param>
        /// <returns></returns>
        public static IEnumerable<Tuple<int, string>> GetTarefasPorOperacaoId(int operacaoId)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                var tarefas = db.Tarefas.AsNoTracking()
                    .Where(t => t.Operacao == operacaoId)
                    .Select(t => new
                    {
                        TarefaId = t.Id,
                        Tarefa = t.Nome
                    })
                    .ToList()
                    .AsEnumerable()
                    .Select(t => Tuple.Create(t.TarefaId, t.Tarefa))
                    .ToList();

                return tarefas;
            }
        }

        /// <summary>
        /// Retorna os monitoramentos por tarefa e unidade (utilizado no relatório de parametrizações)
        /// </summary>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <returns></returns>
        //public static IEnumerable<GetMonitoramentosParametrizacao_Result> GetMonitoramentoPorTarefaIdUnidadeIdParametrizacao(int tarefaId, int unidadeId)
        //{
        //    using (var db = new SGQ_GlobalEntities())
        //    {
        //        var monitoramentos = db.GetMonitoramentosParametrizacao(unidadeId, tarefaId).ToList();

        //        return monitoramentos;
        //    }
        //}

        /// <summary>
        /// Retorna os monitoramentos por tarefa e unidade
        /// </summary>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <returns></returns>
        public static IEnumerable<Tuple<int, string>> GetMonitoramentoPorTarefaIdUnidadeId(int tarefaId, int unidadeId)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                var monitoramentos = (from tm in db.TarefaMonitoramentos.AsNoTracking()
                                      join m in db.Monitoramentos.AsNoTracking() on tm.Monitoramento equals m.Id
                                      where tm.Tarefa == tarefaId && (tm.Unidade.Value == unidadeId || !tm.Unidade.HasValue)
                                      orderby m.Nome
                                      select new
                                      {
                                          MonitoramentoId = m.Id,
                                          Monitoramento = m.Nome
                                      })
                                     .Distinct()
                                     .ToList()
                                     .AsEnumerable()
                                     .Select(m => Tuple.Create(m.MonitoramentoId, m.Monitoramento))
                                     .ToList();

                return monitoramentos;
            }
        }

        /// <summary>
        /// Retorna as avaliações por unidade, operação e tarefa (utilizado no relatório de parametrizações)
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="operacaoId">Id da Operação</param>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <returns></returns>
        public static IEnumerable<Tuple<string, int, string, int?, int>> GetAvaliacoesPorUnidadeIdOperacaoIdTarefaId(int unidadeId, int operacaoId, int tarefaId)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                var avaliacoes = (from a in db.TarefaAvaliacoes.AsNoTracking()
                                  where (a.Unidade == null || a.Unidade == unidadeId) && a.Operacao == operacaoId && a.Tarefa == tarefaId
                                  orderby a.Avaliacao
                                  select new
                                  {
                                      Departamento = a.Departamentos.Nome,
                                      Avaliacao = a.Avaliacao,
                                      Acesso = a.Acesso,
                                      Unidade = a.Unidade,
                                      Id = a.Id
                                  })
                                 .ToList();

                var retorno = avaliacoes.AsEnumerable().Select(a => Tuple.Create(a.Departamento, a.Avaliacao, a.Acesso, a.Unidade, a.Id));

                return retorno;
            }
        }

        #region Especificas
        //a
        /// <summary>
        /// Retorna as avaliações por unidade, operação e tarefa (utilizado no relatório de parametrizações)  somente especificas
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Tuple<string, string, string, string, string, string, int>> GetAvaliacoesEspecificasPorRegional(int regional)
        {
            using (var db = new SGQ_GlobalEntities())
            {

                var unidadesDaRegional = db.Unidades.AsNoTracking().Where(r => r.Regional == regional).Select(r => r.Id);

                var avaliacoes = (from a in db.TarefaAvaliacoes.AsNoTracking()
                                  where (unidadesDaRegional.Any(r => r == a.Unidade) || a.Unidade == null)
                                  orderby a.Avaliacao
                                  select new
                                  {
                                      Departamento = a.Departamentos.Nome,
                                      Avaliacao = a.Avaliacao,
                                      Acesso = a.Acesso,
                                      Unidade = a.Unidades.Nome,
                                      Sigla = a.Unidades.Sigla,
                                      Operacao = a.Operacoes.Nome,
                                      Monitoramento = a.Tarefas.Nome,
                                      Id = a.Id
                                  })
                                 .Distinct()
                                 .ToList();

                var retorno = avaliacoes.AsEnumerable().Select(a => Tuple.Create(a.Unidade.IsNotNull() ? a.Sigla : "CORP", a.Unidade.IsNotNull() ? a.Unidade : "Corporativo", a.Departamento, a.Operacao, a.Monitoramento, a.Acesso, a.Avaliacao));

                return retorno;
            }
        }

        /// <summary>
        /// Retorna a lista de flags da tarefa
        /// </summary>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <returns></returns>
        public static List<Tuple<string, object, string, string, string, string, string>> GetFlagsTarefaRegional(int regional)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                var unidades = db.Unidades.AsNoTracking().Where(r => r.Regional == regional);
                //var temp1 = db.TarefaMonitoramentos.Where(r => r.Unidade != null && unidades.Any(c => c.Id == r.Unidade)).Distinct();
                var temp = db.TarefaMonitoramentos.AsNoTracking().Where(r => r.Unidade == null || unidades.Any(c => c.Id == r.Unidade)).Select(r => new { r.Unidades, r.Monitoramentos, r.Tarefa, r.Unidade, r.Tarefas }).Distinct();
                //var a = temp1.ToList();
                //var b = temp.ToList();
                var retorno = new List<Tuple<string, object, string, string, string, string, string>>();
                var flagsDb = db.Tarefas.AsNoTracking().Where(r => temp.Any(c => c.Tarefa == r.Id)).
                             //where t.Id == tarefaId
                             Select(t => new
                             {
                                 Amostragem = t.Amostragem,
                                 Departamento = t.Departamento.HasValue ? t.Departamentos.Nome : string.Empty,
                                 Frequencia = t.Frequencia,
                                 Vigencia = t.Vigencia,
                                 Produto = t.Produto.HasValue ? t.Produtos.Nome : string.Empty,
                                 EditarAcesso = t.EditarAcesso,
                                 ExibirAcesso = t.ExibirAcesso,
                                 FormaAmostragem = t.FormaAmostragem,
                                 AvaliarProdutos = t.AvaliarProdutos,
                                 InformarPesagem = t.InformarPesagem,
                                 Sequencial = t.Sequencial,
                                 Sigla = temp.Where(r => r.Tarefa == t.Id).FirstOrDefault().Unidades.Sigla,
                                 Unidade = temp.Where(r => r.Tarefa == t.Id).FirstOrDefault().Unidades.Nome,
                                 Monitoramento = temp.Where(r => r.Tarefa == t.Id).FirstOrDefault().Monitoramentos.Nome,
                                 Tarefa = temp.Where(r => r.Tarefa == t.Id).FirstOrDefault().Tarefas.Nome,
                             })
                             .Distinct()
                             .ToList();

                foreach (var flags in flagsDb)
                {
                    foreach (var prop in flags.GetType().GetProperties())
                    {
                        retorno.Add(Tuple.Create(flags.Unidade.IsNotNull() ? flags.Sigla : "CORP", prop.GetValue(flags, null), (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? Nullable.GetUnderlyingType(prop.PropertyType).Name : prop.PropertyType.Name, prop.Name, flags.Unidade.IsNotNull() ? flags.Unidade : "Corporativo", flags.Tarefa, flags.Monitoramento));
                    }
                }

                return retorno;
            }
        }

        /// <summary>
        /// Retorna a lista de flags da operação
        /// </summary>
        /// <param name="operacaoId">Id da Operação</param>
        /// <returns></returns>
        public static List<Tuple<string, string, object, string, string, string, string>> GetFlagsOperacaoRegional(int regional)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                var unidades = db.Unidades.AsNoTracking().Where(r => r.Regional == regional);
                var tarMon = db.TarefaAvaliacoes.AsNoTracking().Where(r => r.Unidade == null || unidades.Any(c => c.Id == r.Unidade)).Distinct();
                var retorno = new List<Tuple<string, string, object, string, string, string, string>>();
                var flagsDb = db.Operacoes.AsNoTracking().Where(r => tarMon.Any(c => c.Operacao == r.Id)).Select(o => new
                {

                    Sigla = tarMon.Where(r => r.Operacoes.Id == o.Id).FirstOrDefault().Unidades.Sigla,
                    Nivel = o.Nivel,
                    Frequencia = o.Frequencia,
                    FrequenciaAlerta = o.FrequenciaAlerta,
                    Vigencia = o.Vigencia,
                    ControleVP = o.ControleVP,
                    ControleFP = o.ControleFP,
                    AdCampoVazio = o.ADCAMPOVAZIO,
                    AvaliarEquipamentos = o.AvaliarEquipamentos,
                    AvaliarCamaras = o.AvaliarCamaras,
                    Especifico = o.Especifico,
                    AlterarAmostra = o.AlterarAmostra,
                    IncluirTarefa = o.IncluirTarefa,
                    IncluirAvaliacao = o.IncluirAvaliacao,
                    ExibirPColeta = o.ExibirPColeta,
                    PadraoPerc = o.PadraoPerc,
                    AvaliarSequencial = o.AvaliarSequencial,
                    Criterio = o.Criterio,
                    ExibirData = o.ExibirData,
                    EmitirAlerta = o.EmitirAlerta,
                    ControlarAvaliacoes = o.ControlarAvaliacoes,
                    Unidade = tarMon.Where(r => r.Operacoes.Id == o.Id).FirstOrDefault().Unidades.Nome,
                    Indicador = tarMon.Where(r => r.Operacoes.Id == o.Id).FirstOrDefault().Tarefas.Nome,
                    Departamento = tarMon.Where(r => r.Operacoes.Id == o.Id).FirstOrDefault().Departamentos.Nome,
                }).Distinct().ToList();

                foreach (var flags in flagsDb)
                {
                    foreach (var prop in flags.GetType().GetProperties())
                    {
                        retorno.Add(Tuple.Create(flags.Unidade.IsNotNull() ? flags.Sigla : "CORP", prop.Name, prop.GetValue(flags, null), (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? Nullable.GetUnderlyingType(prop.PropertyType).Name : prop.PropertyType.Name, flags.Unidade.IsNotNull() ? flags.Unidade : "Corporativo", flags.Indicador, flags.Departamento));
                    }
                }

                return retorno;
            }
        }

        /// <summary>
        /// Retorna os monitoramentos por tarefa e unidade (utilizado no relatório de parametrizações)
        /// </summary>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <returns></returns>
        //public static IEnumerable<Tuple<string, GetMonitoramentosParametrizacao_Result, string, string, string, string>> GetMonitoramentoPorRegional(int regional)
        //{
        //    using (var db = new SGQ_GlobalEntities())
        //    {

        //        var unidades = db.Unidades.AsNoTracking().Where(r => r.Regional == regional);
        //        var tarefas = db.TarefaAvaliacoes.AsNoTracking().Where(r => r.Unidade == null || unidades.Any(c => c.Id == r.Unidade)).Select(r => new { r.Unidades, r.Departamentos, r.Tarefa, r.Unidade, r.Operacoes, r.Tarefas }).Distinct().ToList();
        //        var ncorp = tarefas.Where(r => r.Unidade != null).ToList();
        //        var corp = tarefas.Where(r => r.Unidade == null).ToList();
        //        var monitoramentos = new List<Tuple<string, GetMonitoramentosParametrizacao_Result, string, string, string, string>>();

        //        foreach (var i in ncorp)
        //        {
        //            var result = db.GetMonitoramentosParametrizacao(i.Unidade, i.Tarefa).Select(a => Tuple.Create(i.Unidades.Sigla, a, i.Unidades.Nome, i.Operacoes.Nome, i.Departamentos.Nome, i.Tarefas.Nome));
        //            monitoramentos.AddRange(result);
        //        }
        //        foreach (var i in corp)
        //        {
        //            var result = db.GetMonitoramentosParametrizacao(0, i.Tarefa).Select(a => Tuple.Create("CORP", a, "Corporativo", i.Operacoes.Nome, i.Departamentos.Nome, i.Tarefas.Nome));
        //            monitoramentos.AddRange(result);
        //        }

        //        monitoramentos = monitoramentos.OrderBy(r => r.Item1).ToList();

        //        return monitoramentos;
        //    }
        //}

        //a

        /// <summary>
        /// Retorna as avaliações por unidade, operação e tarefa (utilizado no relatório de parametrizações)
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="operacaoId">Id da Operação</param>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <returns></returns>
        public static IEnumerable<Tuple<string, string, int, string, int?, int>> GetAvaliacoesPorUnidadeIdOperacaoId(int unidadeId, int operacaoId)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                var avaliacoes = (from a in db.TarefaAvaliacoes.AsNoTracking()
                                  where (a.Unidade != null && a.Unidade == unidadeId) && a.Operacao == operacaoId
                                  orderby a.Avaliacao
                                  select new
                                  {
                                      Tarefa = a.Tarefas.Nome,
                                      Departamento = a.Departamentos.Nome,
                                      Avaliacao = a.Avaliacao,
                                      Acesso = a.Acesso,
                                      Unidade = a.Unidade,
                                      Id = a.Id
                                  })
                                 .ToList();

                var retorno = avaliacoes.AsEnumerable().Select(a => Tuple.Create(a.Departamento, a.Tarefa, a.Avaliacao, a.Acesso, a.Unidade, a.Id));

                return retorno;
            }
        }

        /// <summary>
        /// Retorna as avaliações por unidade, operação e tarefa (utilizado no relatório de parametrizações)
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="operacaoId">Id da Operação</param>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <returns></returns>
        public static IEnumerable<Tuple<string, string, string, int, string, int?, int>> GetAvaliacoes(int regional)
        {
            using (var db = new SGQ_GlobalEntities())
            {

                var unidades = db.Unidades.Where(r => r.Regional == regional).ToList();

                var avaliacoes = (from a in db.TarefaAvaliacoes
                                  where (a.Unidade != null && unidades.Any(c => c.Id == a.Unidade))
                                  orderby a.Avaliacao
                                  select new
                                  {
                                      Tarefa = a.Tarefas.Nome,
                                      Operacao = a.Operacoes.Nome,
                                      Departamento = a.Departamentos.Nome,
                                      Avaliacao = a.Avaliacao,
                                      Acesso = a.Acesso,
                                      Unidade = a.Unidade,
                                      Id = a.Id
                                  })
                                 .ToList();

                var retorno = avaliacoes.AsEnumerable().Select(a => Tuple.Create(a.Departamento, a.Operacao, a.Tarefa, a.Avaliacao, a.Acesso, a.Unidade, a.Id));

                return retorno;
            }
        }



        /// <summary>
        /// Retorna as avaliações por unidade, operação e tarefa (utilizado no relatório de parametrizações) somente especificas
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="operacaoId">Id da Operação</param>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <returns></returns>
        public static IEnumerable<Tuple<string, int, string, int?, int>> GetAvaliacoesPorUnidadeEspecificasIdOperacaoIdTarefaId(int unidadeId, int operacaoId, int tarefaId)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                var avaliacoes = (from a in db.TarefaAvaliacoes
                                  where (a.Unidade != null || a.Unidade == unidadeId) && a.Operacao == operacaoId && a.Tarefa == tarefaId
                                  orderby a.Avaliacao
                                  select new
                                  {
                                      Departamento = a.Departamentos.Nome,
                                      Avaliacao = a.Avaliacao,
                                      Acesso = a.Acesso,
                                      Unidade = a.Unidade,
                                      Id = a.Id
                                  })
                                 .ToList();

                var retorno = avaliacoes.AsEnumerable().Select(a => Tuple.Create(a.Departamento, a.Avaliacao, a.Acesso, a.Unidade, a.Id));

                return retorno;
            }
        }


        /// <summary>
        /// Retorna as avaliações por unidade, operação e tarefa (utilizado no relatório de parametrizações) somente especificas
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="operacaoId">Id da Operação</param>
        /// <returns></returns>
        public static IEnumerable<Tuple<string, string, string, string, string, int, int>> GetAvaliacoesPorUnidadeEspecificasIdOperacaoId(int unidadeId, int operacaoId)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                var avaliacoes = (from a in db.TarefaAvaliacoes.AsNoTracking()
                                  where (a.Unidade != null || a.Unidade == unidadeId) && a.Operacao == operacaoId
                                  orderby a.Avaliacao
                                  select new
                                  {
                                      Departamento = a.Departamentos.Nome,
                                      Avaliacao = a.Avaliacao,
                                      Acesso = a.Acesso,
                                      Unidade = a.Unidades.Nome,
                                      Operacao = a.Operacoes.Nome,
                                      Monitoramento = a.Tarefas.Nome,
                                      Id = a.Id
                                  })
                                 .ToList();

                var retorno = avaliacoes.AsEnumerable().Select(a => Tuple.Create(a.Unidade, a.Departamento, a.Operacao, a.Monitoramento, a.Acesso, a.Avaliacao, a.Id));

                return retorno;
            }
        }

        /// <summary>
        /// Retorna as operações por unidade conforme parametrização do cluster somente específicas
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <returns></returns>
        public static IEnumerable<Tuple<int, string>> GetOperacoesEspecificaPorUnidadeId(int unidadeId)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                var operacoes = (from u in db.Unidades.AsNoTracking()
                                 join c in db.Clusters.AsNoTracking() on u.Cluster equals c.Sigla
                                 join cd in db.ClusterDepartamentos.AsNoTracking() on c.Legenda equals cd.Cluster
                                 join d in db.Departamentos.AsNoTracking() on cd.Departamento equals d.Nome
                                 join depo in db.DepartamentoOperacoes.AsNoTracking() on d.Id equals depo.Departamento
                                 join ope in db.Operacoes.AsNoTracking() on depo.Operacao equals ope.Id
                                 //join teste in db.TarefaAvaliacoes.AsNoTracking() on ope.Id equals teste.Unidade
                                 orderby ope.Nivel
                                 where ((u.Id == unidadeId || unidadeId == 0) && u.Ativa.Value && u.Id != 54 && (ope.Especifico == true))
                                 select new { OperacaoId = ope.Id, Operacao = ope.Nome })
                                .Distinct()
                                .ToList();

                var retorno = from o in operacoes.AsEnumerable()
                              select Tuple.Create(o.OperacaoId, o.Operacao);

                return retorno;
            }
        }

        /// <summary>
        /// Retorna as unidades por regional somente específicas
        /// </summary>
        /// <param name="regionaoId">Id da Regional</param>
        /// <returns></returns>
        public static IEnumerable<Tuple<int, string>> GetUnidadesComTarefasEspecificasPorRegionalId(int regionaoId)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                var unidades = db.Unidades.AsNoTracking().Where(u => u.Regional.Value == regionaoId).Select(u => new { UnidadeId = u.Id, Unidade = u.Sigla }).ToList();

                var filtro = db.TarefaAvaliacoes.Where(r => r.Unidade != null).Select(c => c.Unidade).Distinct();

                unidades = unidades.Where(r => filtro.Any(c => c == r.UnidadeId)).ToList();

                var retorno = unidades.AsEnumerable().Select(u => Tuple.Create(u.UnidadeId, u.Unidade));

                return retorno;
            }
        }

        /// <summary>
        /// Retorna os monitoramentos por tarefa e unidade (utilizado no relatório de parametrizações) somente específicas
        /// </summary>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <returns></returns>
        //public static IEnumerable<GetMonitoramentosParametrizacao_Result> GetMonitoramentoPorEspecificosTarefaIdUnidadeIdParametrizacao(int tarefaId, int unidadeId)
        //{
        //    using (var db = new SGQ_GlobalEntities())
        //    {
        //        var monitoramentos = db.GetMonitoramentosParametrizacao(unidadeId, tarefaId).ToList();
        //        var filtro = db.TarefaAvaliacoes.Where(r => r.Unidade != null).Select(c => c.Tarefa);
        //        monitoramentos = monitoramentos.Where(r => filtro.Any(c => c == r.Monitoramento)).ToList();
        //        return monitoramentos;
        //    }
        //}

        /// <summary>
        /// Retorna as tarefas por operação somente específicas
        /// </summary>
        /// <param name="operacaoId">Id da Operação</param>
        /// <returns></returns>
        public static IEnumerable<Tuple<int, string>> GetTarefasEspecificasPorOperacaoId(int operacaoId)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                var filtro = db.TarefaAvaliacoes.AsNoTracking().Where(r => r.Unidade != null).Select(c => c.Tarefa).Distinct();
                var tarefas = db.Tarefas.AsNoTracking()
                    .Where(t => t.Operacao == operacaoId && filtro.Any(c => c == t.Id))
                    .Select(t => new
                    {
                        TarefaId = t.Id,
                        Tarefa = t.Nome
                    })
                    .ToList()
                    .AsEnumerable()
                    .Select(t => Tuple.Create(t.TarefaId, t.Tarefa))
                    .ToList();
                return tarefas;
            }
        }

        /// <summary>
        /// Retorna a lista de flags da operação
        /// </summary>
        /// <param name="operacaoId">Id da Operação</param>
        /// <returns></returns>
        public static List<Tuple<string, object, string>> GetFlagsOperacaoEspecificas(int operacaoId)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                var filtro = db.TarefaAvaliacoes.Where(r => r.Unidade != null).Select(c => c.Operacao).Distinct();
                var flags = (from o in db.Operacoes.AsNoTracking()
                             where o.Id == operacaoId && filtro.Any(r => r == o.Id)
                             select new
                             {
                                 Nivel = o.Nivel,
                                 Frequencia = o.Frequencia,
                                 FrequenciaAlerta = o.FrequenciaAlerta,
                                 Vigencia = o.Vigencia,
                                 ControleVP = o.ControleVP,
                                 ControleFP = o.ControleFP,
                                 AdCampoVazio = o.ADCAMPOVAZIO,
                                 AvaliarEquipamentos = o.AvaliarEquipamentos,
                                 AvaliarCamaras = o.AvaliarCamaras,
                                 Especifico = o.Especifico,
                                 AlterarAmostra = o.AlterarAmostra,
                                 IncluirTarefa = o.IncluirTarefa,
                                 IncluirAvaliacao = o.IncluirAvaliacao,
                                 ExibirPColeta = o.ExibirPColeta,
                                 PadraoPerc = o.PadraoPerc,
                                 AvaliarSequencial = o.AvaliarSequencial,
                                 Criterio = o.Criterio,
                                 ExibirData = o.ExibirData,
                                 EmitirAlerta = o.EmitirAlerta,
                                 ControlarAvaliacoes = o.ControlarAvaliacoes
                             })
                            .First();

                var retorno = new List<Tuple<string, object, string>>();

                foreach (var prop in flags.GetType().GetProperties())
                {
                    retorno.Add(Tuple.Create(prop.Name, prop.GetValue(flags, null), (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? Nullable.GetUnderlyingType(prop.PropertyType).Name : prop.PropertyType.Name));
                }

                return retorno;
            }
        }

        /// <summary>
        /// Retorna a lista de flags da tarefa
        /// </summary>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <returns></returns>
        public static List<Tuple<string, object, string>> GetFlagsTarefaEspecificas(int tarefaId)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                var filtro = db.TarefaAvaliacoes.Where(r => r.Unidade != null).Select(c => c.Tarefa).Distinct();
                var flags = (from t in db.Tarefas.AsNoTracking()
                             where t.Id == tarefaId && filtro.Any(r => r == t.Id)
                             select new
                             {
                                 Amostragem = t.Amostragem,
                                 Departamento = t.Departamento.HasValue ? t.Departamentos.Nome : string.Empty,
                                 Frequencia = t.Frequencia,
                                 Vigencia = t.Vigencia,
                                 Produto = t.Produto.HasValue ? t.Produtos.Nome : string.Empty,
                                 EditarAcesso = t.EditarAcesso,
                                 ExibirAcesso = t.ExibirAcesso,
                                 FormaAmostragem = t.FormaAmostragem,
                                 AvaliarProdutos = t.AvaliarProdutos,
                                 InformarPesagem = t.InformarPesagem,
                                 Sequencial = t.Sequencial
                             })
                            .First();

                var retorno = new List<Tuple<string, object, string>>();

                foreach (var prop in flags.GetType().GetProperties())
                {
                    retorno.Add(Tuple.Create(prop.Name, prop.GetValue(flags, null), (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? Nullable.GetUnderlyingType(prop.PropertyType).Name : prop.PropertyType.Name));
                }

                return retorno;
            }
        }

        #endregion

        /// <summary>
        /// Retorna o número máximo de amostras
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="departamentoId">Id do Departamento</param>
        /// <param name="operacaoId">Id da Operação</param>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <returns></returns>
        public static int GetMaximoNumeroAmostra(int unidadeId, int departamentoId, int operacaoId, int tarefaId)
        {
            DateTime dataConsultar = GetDataSegundoTurno(unidadeId, operacaoId);

            return GetMaximoNumeroAmostra(unidadeId, departamentoId, operacaoId, tarefaId, dataConsultar);
        }

        /// <summary>
        /// Retorna o número máximo de amostras da data informada
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="departamentoId">Id do Departamento</param>
        /// <param name="operacaoId">Id da Operação</param>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <param name="dataConsultar">Data a ser consultada</param>
        /// <returns></returns>
        public static int GetMaximoNumeroAmostra(int unidadeId, int departamentoId, int operacaoId, int tarefaId, DateTime dataConsultar)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                int retorno = 0;

                var operacao = db.Operacoes.AsNoTracking().Single(o => o.Id == operacaoId);

                if (!operacao.ControleVP.Value)
                {
                    retorno = db.TarefaAmostras.AsNoTracking()
                        .Where(ta => ta.Tarefa == tarefaId && ta.Unidade.Value == unidadeId)
                        .Select(ta => ta.Amostras).FirstOrDefault();

                    if (retorno == 0)
                    {
                        retorno = db.TarefaAmostras.AsNoTracking()
                            .Where(ta => ta.Tarefa == tarefaId && !ta.Unidade.HasValue)
                            .Select(ta => ta.Amostras).FirstOrDefault();
                    }
                }
                else
                {
                    if (operacao.Nome.IndexOf("CEP") > 0)
                    {
                        if (operacao.ControleFP.Value)
                        {
                            var quantidadeFamilias = db.FamiliaProdutos.AsNoTracking()
                                .Where(fp => fp.Ano == dataConsultar.Year &&
                                    fp.Mes == dataConsultar.Month &&
                                    (!fp.Operacao.HasValue || fp.Operacao.Value == operacaoId) &&
                                    (!fp.Unidade.HasValue || fp.Unidade.Value == unidadeId))
                                    .Count();

                            retorno = db.VolumeProducao.AsNoTracking()
                               .Where(vp => vp.Unidade == unidadeId &&
                               vp.Departamento == departamentoId &&
                               vp.Operacao == operacaoId &&
                               DbFunctions.TruncateTime(vp.Data) <= DbFunctions.TruncateTime(dataConsultar)
                               ).OrderByDescending(vp => vp.Data).ThenBy(vp => vp.DataAlteracao)
                               .Select(vp => new { AmostraDia = vp.AmostraDia.Value, HorasTrabalho = vp.HorasTrabalho.Value, id = vp.Id })
                               .AsEnumerable()
                               .Select(vp => int.Parse(Math.Ceiling(double.Parse(vp.AmostraDia.ToString()) / double.Parse(vp.HorasTrabalho.ToString()) / double.Parse(quantidadeFamilias.ToString())).ToString()))
                               .FirstOrDefault();
                        }
                        else
                        {
                            retorno = db.VolumeProducao.AsNoTracking()
                                .Where(vp => vp.Unidade == unidadeId &&
                                vp.Departamento == departamentoId &&
                                vp.Operacao == operacaoId
                                && DbFunctions.TruncateTime(vp.Data) <= DbFunctions.TruncateTime(dataConsultar)
                                ).OrderByDescending(vp => vp.Data).ThenBy(vp => vp.DataAlteracao)
                                .Select(vp => vp.QtdColabOuEsteiras.Value * vp.AmostraAvaliacao.Value)
                                .FirstOrDefault();

                        }
                    }
                    else
                    {
                        if (operacao.Nome.IndexOf("PCC") > 0) //Se for PCC obriga a cadastras o VOLUME.
                        {
                            retorno = db.VolumeProducao.AsNoTracking()
                            .Where(vp => vp.Unidade == unidadeId
                                && vp.Departamento == departamentoId
                                && vp.Operacao == operacaoId
                                && DbFunctions.TruncateTime(vp.Data) == DbFunctions.TruncateTime(dataConsultar))
                            //).OrderByDescending(vp => vp.Data).ThenBy(vp => vp.DataAlteracao)
                            .Select(vp => vp.Quartos.Value / 2)
                            .FirstOrDefault();
                        }
                        else
                        {
                            retorno = db.VolumeProducao.AsNoTracking()
                                .Where(vp => vp.Unidade == unidadeId &&
                                vp.Departamento == departamentoId &&
                                vp.Operacao == operacaoId
                                && DbFunctions.TruncateTime(vp.Data) <= DbFunctions.TruncateTime(dataConsultar)
                                ).OrderByDescending(vp => vp.Data).ThenBy(vp => vp.DataAlteracao)
                                .Select(vp => vp.Quartos.Value / 2)
                                .FirstOrDefault();
                        }
                    }
                }

                return retorno;
            }
        }

        /// <summary>
        /// Retorna o número máximo de avaliações
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="departamentoId">Id do Departamento</param>
        /// <param name="operacaoId">Id da Operação</param>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <returns></returns>
        public static int GetMaximoNumeroAvaliacao(int unidadeId, int departamentoId, int operacaoId, int tarefaId)
        {
            DateTime dataConsultar = GetDataSegundoTurno(unidadeId, operacaoId);

            return GetMaximoNumeroAvaliacao(unidadeId, departamentoId, operacaoId, tarefaId, dataConsultar);
        }

        /// <summary>
        /// Retorna o número máximo de avaliações da data informada
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="departamentoId">Id do Departamento</param>
        /// <param name="operacaoId">Id da Operação</param>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <param name="dataConsultar">Data a ser consultada</param>
        /// <returns></returns>
        public static int GetMaximoNumeroAvaliacao(int unidadeId, int departamentoId, int operacaoId, int tarefaId, DateTime dataConsultar)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                int retorno;

                var operacao = db.Operacoes.AsNoTracking().Single(o => o.Id == operacaoId);

                if (!operacao.ControleVP.Value)
                {
                    retorno = db.TarefaAvaliacoes.AsNoTracking()
                         .Where(ta => ta.Departamento == departamentoId
                             && ta.Operacao == operacaoId
                             && ta.Tarefa == tarefaId
                             && (!ta.Unidade.HasValue || ta.Unidade.Value == unidadeId))
                         .Max(ta => ta.Avaliacao);
                }
                else
                {
                    if (operacao.Nome.IndexOf("CEP") > 0)
                    {
                        retorno = db.VolumeProducao.AsNoTracking()
                            .Where(vp => vp.Unidade == unidadeId
                                && vp.Departamento == departamentoId
                                && vp.Operacao == operacaoId
                                && DbFunctions.TruncateTime(vp.Data) <= DbFunctions.TruncateTime(dataConsultar)
                                ).OrderByDescending(vp => vp.Data).ThenBy(vp => vp.DataAlteracao)
                            .Select(vp => vp.HorasTrabalho.Value)
                            .FirstOrDefault();
                    }
                    else
                    {
                        if (operacao.Nome.IndexOf("PCC") > 0) //Se for PCC obriga a cadastras o VOLUME.
                        {
                            retorno = db.VolumeProducao.AsNoTracking()
                            .Where(vp => vp.Unidade == unidadeId
                                && vp.Departamento == departamentoId
                                && vp.Operacao == operacaoId
                                && DbFunctions.TruncateTime(vp.Data) == DbFunctions.TruncateTime(dataConsultar))
                            //).OrderByDescending(vp => vp.Data).ThenBy(vp => vp.DataAlteracao)
                            .Select(vp => vp.Quartos.Value / 2)
                            .FirstOrDefault();
                        }
                        else
                        {
                            retorno = db.VolumeProducao.AsNoTracking()
                                .Where(vp => vp.Unidade == unidadeId
                                    && vp.Departamento == departamentoId
                                    && vp.Operacao == operacaoId
                                    && DbFunctions.TruncateTime(vp.Data) <= DbFunctions.TruncateTime(dataConsultar)
                                    ).OrderByDescending(vp => vp.Data).ThenBy(vp => vp.DataAlteracao)
                                .Select(vp => vp.Quartos.Value / 2)
                                .FirstOrDefault();

                        }
                    }
                }

                return retorno;
            }
        }

        /// <summary>
        /// Retorna o número da última amostra realizada
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="departamentoId">Id do Departamento</param>
        /// <param name="operacaoId">Id da Operação</param>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <returns></returns>
        public static int GetNumeroAmostraRealizada(int unidadeId, int departamentoId, int operacaoId, int tarefaId, bool decrescer = false)
        {
            DateTime dataConsultar = GetDataSegundoTurno(unidadeId, operacaoId);

            return GetNumeroAmostraRealizada(unidadeId, departamentoId, operacaoId, tarefaId, 0, dataConsultar, decrescer);
        }

        /// <summary>
        /// Retorna o número da última amostra realizada da avaliação informada
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="departamentoId">Id do Departamento</param>
        /// <param name="operacaoId">Id da Operação</param>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <param name="numeroAvaliacao">Número da avaliação a ser consultada</param>
        /// <returns></returns>
        public static int GetNumeroAmostraRealizada(int unidadeId, int departamentoId, int operacaoId, int tarefaId, int numeroAvaliacao, bool decrescer = false)
        {
            DateTime dataConsultar = GetDataSegundoTurno(unidadeId, operacaoId);

            return GetNumeroAmostraRealizada(unidadeId, departamentoId, operacaoId, tarefaId, numeroAvaliacao, dataConsultar, decrescer);
        }

        /// <summary>
        /// Retorna o número da última amostra realizada da avaliação e data informadas
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="departamentoId">Id do Departamento</param>
        /// <param name="operacaoId">Id da Operação</param>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <param name="numeroAvaliacao">Número da avaliação a ser consultada</param>
        /// <param name="dataConsultar">Data a ser consultada</param>
        /// <returns></returns>
        public static int GetNumeroAmostraRealizada(int unidadeId, int departamentoId, int operacaoId, int tarefaId, int numeroAvaliacao, DateTime dataConsultar, bool decrescer = false)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                //int retorno = 0;

                if (operacaoId == 27)
                {
                    var amostrasRealizadas = GetAmostraRealizadaVerificacao(unidadeId, dataConsultar, db) - (decrescer ? 1 : 0);

                    if (amostrasRealizadas > 0)
                    {

                        //Demonstrar a quantidade de amostrasRealizadas independente se for maior que o maximoAmostras

                        var maximoAmostras = GetMaximoNumeroAmostra(unidadeId, departamentoId, operacaoId, tarefaId, dataConsultar);
                        if (amostrasRealizadas > maximoAmostras)
                        {
                            return amostrasRealizadas - (amostrasRealizadas / maximoAmostras * maximoAmostras);
                        }

                        return amostrasRealizadas;
                    }

                    return 0;
                }

                var dados = (from t in db.Tarefas.AsNoTracking()
                             where t.Id == tarefaId
                             select new { FormaAmostragem = t.FormaAmostragem, Frequencia = t.Frequencia }).FirstOrDefault();

                if (dados.FormaAmostragem.Equals("Coletiva"))
                {
                    return db.Resultados.AsNoTracking()
                                .Where(r => r.EmpresaId == 1
                                && r.UnidadeId == unidadeId
                                && r.DepartamentoId == departamentoId
                                && r.OperacaoId == operacaoId
                                && r.TarefaId == tarefaId
                                && (numeroAvaliacao == 0 || (numeroAvaliacao != 0 && r.NumeroAvaliacao == numeroAvaliacao))
                                && ((dados.Frequencia == "Diario" && DbFunctions.TruncateTime(r.DataHora) == DbFunctions.TruncateTime(dataConsultar))
                                || (dados.Frequencia == "Semanal" && r.DataHora.Year == dataConsultar.Year && SqlFunctions.DatePart("wk", r.DataHora) == SqlFunctions.DatePart("wk", dataConsultar))
                                || (dados.Frequencia == "Quinzenal" && (r.DataHora.Year == dataConsultar.Year && r.DataHora.Month == dataConsultar.Month && ((dataConsultar.Day < 16 && r.DataHora.Day < 16) || (dataConsultar.Day >= 16 && r.DataHora.Day >= 16))))
                                || (dados.Frequencia == "Mensal" && r.DataHora.Year == dataConsultar.Year && r.DataHora.Month == dataConsultar.Month)))
                                .GroupBy(r => new { r.EmpresaId, r.UnidadeId, r.DepartamentoId, r.OperacaoId, r.TarefaId })
                                .Select(rg => rg.Sum(r => r.PecasAvaliadas.HasValue ? r.PecasAvaliadas.Value : 0))
                                .FirstOrDefault();
                }
                else
                {
                    return db.Resultados.AsNoTracking()
                                .Where(r => r.EmpresaId == 1
                                && r.UnidadeId == unidadeId
                                && r.DepartamentoId == departamentoId
                                && r.OperacaoId == operacaoId
                                && r.TarefaId == tarefaId
                                && (numeroAvaliacao == 0 || (numeroAvaliacao != 0 && r.NumeroAvaliacao == numeroAvaliacao))
                                && ((dados.Frequencia == "Diario" && DbFunctions.TruncateTime(r.DataHora) == DbFunctions.TruncateTime(dataConsultar))
                                || (dados.Frequencia == "Semanal" && r.DataHora.Year == dataConsultar.Year && SqlFunctions.DatePart("wk", r.DataHora) == SqlFunctions.DatePart("wk", dataConsultar))
                                || (dados.Frequencia == "Quinzenal" && (r.DataHora.Year == dataConsultar.Year && r.DataHora.Month == dataConsultar.Month && ((dataConsultar.Day < 16 && r.DataHora.Day < 16) || (dataConsultar.Day >= 16 && r.DataHora.Day >= 16))))
                                || (dados.Frequencia == "Mensal" && r.DataHora.Year == dataConsultar.Year && r.DataHora.Month == dataConsultar.Month)))
                                .Select(r => new { NumeroAvaliacao = r.NumeroAvaliacao, NumeroAmostra = r.NumeroAmostra })
                                .AsEnumerable()
                                .Select(r => string.Format("{0}-{1}", r.NumeroAvaliacao, r.NumeroAmostra))
                                .Distinct()
                                .Count();
                }

                //return 0;
            }
        }


        public static int GetNumeroAmostraRealizadaVerificacao(int unidadeId, int departamentoId, int operacaoId, int tarefaId, int numeroAvaliacao, DateTime dataConsultar, bool decrescer = false)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                //int retorno = 0;

                if (operacaoId == 27)
                {
                    var amostrasRealizadas = GetAmostraRealizadaVerificacao(unidadeId, dataConsultar, db) - (decrescer ? 1 : 0);

                    if (amostrasRealizadas > 0)
                    {

                        //Demonstrar a quantidade de amostrasRealizadas independente se for maior que o maximoAmostras

                        //var maximoAmostras = GetMaximoNumeroAmostra(unidadeId, departamentoId, operacaoId, tarefaId, dataConsultar);
                        //if (amostrasRealizadas > maximoAmostras)
                        //{
                        //    return amostrasRealizadas - (amostrasRealizadas / maximoAmostras * maximoAmostras);
                        //}

                        return amostrasRealizadas;
                    }

                    return 0;
                }

                var dados = (from t in db.Tarefas.AsNoTracking()
                             where t.Id == tarefaId
                             select new { FormaAmostragem = t.FormaAmostragem, Frequencia = t.Frequencia }).FirstOrDefault();

                if (dados.FormaAmostragem.Equals("Coletiva"))
                {
                    return db.Resultados.AsNoTracking()
                                .Where(r => r.EmpresaId == 1
                                && r.UnidadeId == unidadeId
                                && r.DepartamentoId == departamentoId
                                && r.OperacaoId == operacaoId
                                && r.TarefaId == tarefaId
                                && (numeroAvaliacao == 0 || (numeroAvaliacao != 0 && r.NumeroAvaliacao == numeroAvaliacao))
                                && ((dados.Frequencia == "Diario" && DbFunctions.TruncateTime(r.DataHora) == DbFunctions.TruncateTime(dataConsultar))
                                || (dados.Frequencia == "Semanal" && r.DataHora.Year == dataConsultar.Year && SqlFunctions.DatePart("wk", r.DataHora) == SqlFunctions.DatePart("wk", dataConsultar))
                                || (dados.Frequencia == "Quinzenal" && (r.DataHora.Year == dataConsultar.Year && r.DataHora.Month == dataConsultar.Month && ((dataConsultar.Day < 16 && r.DataHora.Day < 16) || (dataConsultar.Day >= 16 && r.DataHora.Day >= 16))))
                                || (dados.Frequencia == "Mensal" && r.DataHora.Year == dataConsultar.Year && r.DataHora.Month == dataConsultar.Month)))
                                .GroupBy(r => new { r.EmpresaId, r.UnidadeId, r.DepartamentoId, r.OperacaoId, r.TarefaId })
                                .Select(rg => rg.Sum(r => r.PecasAvaliadas.HasValue ? r.PecasAvaliadas.Value : 0))
                                .FirstOrDefault();
                }
                else
                {
                    return db.Resultados.AsNoTracking()
                                .Where(r => r.EmpresaId == 1
                                && r.UnidadeId == unidadeId
                                && r.DepartamentoId == departamentoId
                                && r.OperacaoId == operacaoId
                                && r.TarefaId == tarefaId
                                && (numeroAvaliacao == 0 || (numeroAvaliacao != 0 && r.NumeroAvaliacao == numeroAvaliacao))
                                && ((dados.Frequencia == "Diario" && DbFunctions.TruncateTime(r.DataHora) == DbFunctions.TruncateTime(dataConsultar))
                                || (dados.Frequencia == "Semanal" && r.DataHora.Year == dataConsultar.Year && SqlFunctions.DatePart("wk", r.DataHora) == SqlFunctions.DatePart("wk", dataConsultar))
                                || (dados.Frequencia == "Quinzenal" && (r.DataHora.Year == dataConsultar.Year && r.DataHora.Month == dataConsultar.Month && ((dataConsultar.Day < 16 && r.DataHora.Day < 16) || (dataConsultar.Day >= 16 && r.DataHora.Day >= 16))))
                                || (dados.Frequencia == "Mensal" && r.DataHora.Year == dataConsultar.Year && r.DataHora.Month == dataConsultar.Month)))
                                .Select(r => new { NumeroAvaliacao = r.NumeroAvaliacao, NumeroAmostra = r.NumeroAmostra })
                                .AsEnumerable()
                                .Select(r => string.Format("{0}-{1}", r.NumeroAvaliacao, r.NumeroAmostra))
                                .Distinct()
                                .Count();
                }

                //return 0;
            }
        }

        private static int GetAmostraRealizadaVerificacao(int unidadeId, DateTime dataConsultar, SGQ_GlobalEntities db)
        {
            return (from v in db.VerificacaoTipificacao.AsNoTracking()
                    let d = DbFunctions.TruncateTime(v.DataHora)
                    where d == DbFunctions.TruncateTime(dataConsultar)
                    && v.UnidadeId == unidadeId
                    group d by d)
                .DefaultIfEmpty()
                .Select(r => r.Count())
                .FirstOrDefault();
        }

        /// <summary>
        /// Retorna o número da última amostra realizada da avaliação e data informadas
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="departamentoId">Id do Departamento</param>
        /// <param name="operacaoId">Id da Operação</param>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <param name="numeroAvaliacao">Número da avaliação a ser consultada</param>
        /// <param name="dataConsultar">Data a ser consultada</param>
        /// <returns></returns>
        public static int GetNumeroAmostraData(int unidadeId, int departamentoId, int operacaoId, int tarefaId, int numeroAvaliacao, DateTime dataConsultar)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                int retorno = 0;

                var dados = (from t in db.Tarefas.AsNoTracking()
                             where t.Id == tarefaId
                             select new { FormaAmostragem = t.FormaAmostragem, Frequencia = t.Frequencia }).FirstOrDefault();

                if (dados.FormaAmostragem.Equals("Coletiva"))
                {
                    retorno = db.Resultados.AsNoTracking()
                                .Where(r => r.EmpresaId == 1
                                && r.UnidadeId == unidadeId
                                && r.DepartamentoId == departamentoId
                                && r.OperacaoId == operacaoId
                                && r.TarefaId == tarefaId
                                && (numeroAvaliacao == 0 || (numeroAvaliacao != 0 && r.NumeroAvaliacao == numeroAvaliacao))
                                && ((dados.Frequencia == "Diario" && DbFunctions.TruncateTime(r.DataHoraMonitor.Value) == DbFunctions.TruncateTime(dataConsultar))
                                || (dados.Frequencia == "Semanal" && r.DataHora.Year == dataConsultar.Year && SqlFunctions.DatePart("wk", r.DataHoraMonitor.Value) == SqlFunctions.DatePart("wk", dataConsultar))
                                || (dados.Frequencia == "Quinzenal" && (r.DataHoraMonitor.Value.Year == dataConsultar.Year && r.DataHoraMonitor.Value.Month == dataConsultar.Month && ((dataConsultar.Day < 16 && r.DataHoraMonitor.Value.Day < 16) || (dataConsultar.Day >= 16 && r.DataHoraMonitor.Value.Day >= 16))))
                                || (dados.Frequencia == "Mensal" && r.DataHoraMonitor.Value.Year == dataConsultar.Year && r.DataHoraMonitor.Value.Month == dataConsultar.Month)))
                                .GroupBy(r => new { r.EmpresaId, r.UnidadeId, r.DepartamentoId, r.OperacaoId, r.TarefaId })
                                .Select(rg => rg.Sum(r => r.PecasAvaliadas.Value))
                                .FirstOrDefault();
                }
                else
                {
                    retorno = db.Resultados.AsNoTracking()
                                .Where(r => r.EmpresaId == 1
                                && r.UnidadeId == unidadeId
                                && r.DepartamentoId == departamentoId
                                && r.OperacaoId == operacaoId
                                && r.TarefaId == tarefaId
                                && (numeroAvaliacao == 0 || (numeroAvaliacao != 0 && r.NumeroAvaliacao == numeroAvaliacao))
                                && ((dados.Frequencia == "Diario" && DbFunctions.TruncateTime(r.DataHoraMonitor.Value) == DbFunctions.TruncateTime(dataConsultar))
                                || (dados.Frequencia == "Semanal" && r.DataHora.Year == dataConsultar.Year && SqlFunctions.DatePart("wk", r.DataHoraMonitor.Value) == SqlFunctions.DatePart("wk", dataConsultar))
                                || (dados.Frequencia == "Quinzenal" && (r.DataHoraMonitor.Value.Year == dataConsultar.Year && r.DataHoraMonitor.Value.Month == dataConsultar.Month && ((dataConsultar.Day < 16 && r.DataHoraMonitor.Value.Day < 16) || (dataConsultar.Day >= 16 && r.DataHoraMonitor.Value.Day >= 16))))
                                || (dados.Frequencia == "Mensal" && r.DataHoraMonitor.Value.Year == dataConsultar.Year && r.DataHoraMonitor.Value.Month == dataConsultar.Month)))
                                .Select(r => new { NumeroAvaliacao = r.NumeroAvaliacao, NumeroAmostra = r.NumeroAmostra })
                                .AsEnumerable()
                                .Select(r => string.Format("{0}-{1}", r.NumeroAvaliacao, r.NumeroAmostra))
                                .Distinct()
                                .Count();
                }

                return retorno;
            }
        }

        /// <summary>
        /// Retorna o número da última avaliação realizada
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="departamentoId">Id do Departamento</param>
        /// <param name="operacaoId">Id da Operação</param>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <returns></returns>
        public static int GetNumeroAvaliacaoAtual(int unidadeId, int departamentoId, int operacaoId, int tarefaId, bool decrescer = false)
        {
            DateTime dataConsultar = GetDataSegundoTurno(unidadeId, operacaoId);

            return GetNumeroAvaliacaoAtual(unidadeId, departamentoId, operacaoId, tarefaId, dataConsultar, decrescer);
        }

        /// <summary>
        /// Retorna o número da última avaliação realizada da data informada
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="departamentoId">Id do Departamento</param>
        /// <param name="operacaoId">Id da Operação</param>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <param name="dataConsultar">Data a ser consultada</param>
        /// <returns></returns>
        public static int GetNumeroAvaliacaoAtual(int unidadeId, int departamentoId, int operacaoId, int tarefaId, DateTime dataConsultar, bool decrescer = false)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                //int retorno = 0;

                if (operacaoId == 27)
                {
                    var amostrasRealizadas = GetAmostraRealizadaVerificacao(unidadeId, dataConsultar, db) - (decrescer ? 1 : 0);

                    if (amostrasRealizadas > 0)
                    {
                        var maximoAmostras = GetMaximoNumeroAmostra(unidadeId, departamentoId, operacaoId, tarefaId, dataConsultar);

                        // return (amostrasRealizadas / maximoAmostras + amostrasRealizadas == maximoAmostras ? 0 : amostrasRealizadas / maximoAmostras + 1);

                        return (amostrasRealizadas / maximoAmostras + amostrasRealizadas == maximoAmostras ? 0 : (int)Math.Ceiling((decimal)amostrasRealizadas / (decimal)maximoAmostras)/* + 1*/);
                    }

                    return 0;
                }

                var dados = (from t in db.Tarefas.AsNoTracking()
                             where t.Id == tarefaId
                             select new { Frequencia = t.Frequencia }).FirstOrDefault();

                return db.Resultados.AsNoTracking()
                            .Where(r => r.EmpresaId == 1
                            && r.UnidadeId == unidadeId
                            && r.DepartamentoId == departamentoId
                            && r.OperacaoId == operacaoId
                            && r.TarefaId == tarefaId
                            && ((dados.Frequencia == "Diario" && DbFunctions.TruncateTime(r.DataHora) == DbFunctions.TruncateTime(dataConsultar))
                            || (dados.Frequencia == "Semanal" && r.DataHora.Year == dataConsultar.Year && SqlFunctions.DatePart("wk", r.DataHora) == SqlFunctions.DatePart("wk", dataConsultar))
                            || (dados.Frequencia == "Quinzenal" && (r.DataHora.Year == dataConsultar.Year && r.DataHora.Month == dataConsultar.Month && ((dataConsultar.Day < 16 && r.DataHora.Day < 16) || (dataConsultar.Day >= 16 && r.DataHora.Day >= 16))))
                            || (dados.Frequencia == "Mensal" && r.DataHora.Year == dataConsultar.Year && r.DataHora.Month == dataConsultar.Month)))
                            .Select(r => r.NumeroAvaliacao)
                            .DefaultIfEmpty()
                            .Max();

                //return retorno;
            }
        }

        /// <summary>
        /// Retorna o número da última avaliação realizada da data informada
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="departamentoId">Id do Departamento</param>
        /// <param name="operacaoId">Id da Operação</param>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <param name="dataConsultar">Data a ser consultada</param>
        /// <returns></returns>
        public static int GetNumeroAvaliacaoData(int unidadeId, int departamentoId, int operacaoId, int tarefaId, DateTime dataConsultar)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                int retorno = 0;

                var dados = (from t in db.Tarefas.AsNoTracking()
                             where t.Id == tarefaId
                             select new { Frequencia = t.Frequencia }).FirstOrDefault();

                retorno = db.Resultados.AsNoTracking()
                            .Where(r => r.EmpresaId == 1
                            && r.UnidadeId == unidadeId
                            && r.DepartamentoId == departamentoId
                            && r.OperacaoId == operacaoId
                            && r.TarefaId == tarefaId
                            && ((dados.Frequencia == "Diario" && DbFunctions.TruncateTime(r.DataHoraMonitor.Value) == DbFunctions.TruncateTime(dataConsultar))
                            || (dados.Frequencia == "Semanal" && r.DataHora.Year == dataConsultar.Year && SqlFunctions.DatePart("wk", r.DataHoraMonitor.Value) == SqlFunctions.DatePart("wk", dataConsultar))
                            || (dados.Frequencia == "Quinzenal" && (r.DataHoraMonitor.Value.Year == dataConsultar.Year && r.DataHoraMonitor.Value.Month == dataConsultar.Month && ((dataConsultar.Day < 16 && r.DataHoraMonitor.Value.Day < 16) || (dataConsultar.Day >= 16 && r.DataHoraMonitor.Value.Day >= 16))))
                            || (dados.Frequencia == "Mensal" && r.DataHoraMonitor.Value.Year == dataConsultar.Year && r.DataHoraMonitor.Value.Month == dataConsultar.Month)))
                            .Select(r => r.NumeroAvaliacao)
                            .DefaultIfEmpty()
                            .Max();

                return retorno;
            }
        }

        /// <summary>
        /// Retorna o próximo número de avaliação
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="departamentoId">Id do Departamento</param>
        /// <param name="operacaoId">Id da Operação</param>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <returns></returns>
        public static int GetProximoNumeroAvaliacao(int unidadeId, int departamentoId, int operacaoId, int tarefaId, bool decrescer = false)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                int retorno = 0;

                bool controlarAvaliacoes = db.Operacoes.AsNoTracking().Single(o => o.Id == operacaoId).ControlarAvaliacoes.Value;

                int avaliacaoAtual = GetNumeroAvaliacaoAtual(unidadeId, departamentoId, operacaoId, tarefaId, decrescer);

                avaliacaoAtual++;

                int maximoAvaliacoes = GetMaximoNumeroAvaliacao(unidadeId, departamentoId, operacaoId, tarefaId);

                if (controlarAvaliacoes && maximoAvaliacoes == 0)
                {
                    retorno = -1;
                }
                else
                {
                    retorno = avaliacaoAtual;
                }

                return retorno;
            }
        }

        /// <summary>
        /// Retorna o próximo número de avaliação da data informada
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="departamentoId">Id do Departamento</param>
        /// <param name="operacaoId">Id da Operação</param>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <param name="dataConsultar">Data a ser consultada</param>
        /// <returns></returns>
        public static int GetProximoNumeroAvaliacao(int unidadeId, int departamentoId, int operacaoId, int tarefaId, DateTime dataConsultar)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                int retorno = 0;

                bool controlarAvaliacoes = db.Operacoes.AsNoTracking().Single(o => o.Id == operacaoId).ControlarAvaliacoes.Value;

                int avaliacao = GetNumeroAvaliacaoAtual(unidadeId, departamentoId, operacaoId, tarefaId, dataConsultar);

                avaliacao++;

                int maximoAvaliacoes = GetMaximoNumeroAvaliacao(unidadeId, departamentoId, operacaoId, tarefaId, dataConsultar);

                if (controlarAvaliacoes && maximoAvaliacoes == 0)
                {
                    retorno = -1;
                }
                else
                {
                    retorno = avaliacao;
                }

                return retorno;
            }
        }

        /// <summary>
        /// Retorna o próximo número de avaliação da data informada
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="departamentoId">Id do Departamento</param>
        /// <param name="operacaoId">Id da Operação</param>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <param name="dataConsultar">Data a ser consultada</param>
        /// <returns></returns>
        public static bool GetProximoNumeroAvaliacaoData(int unidadeId, int departamentoId, int operacaoId, int tarefaId, DateTime dataConsultar, out int numeroAvaliacao)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                bool retorno = true;

                bool controlarAvaliacoes = db.Operacoes.AsNoTracking().Single(o => o.Id == operacaoId).ControlarAvaliacoes.Value;

                int avaliacao = GetNumeroAvaliacaoData(unidadeId, departamentoId, operacaoId, tarefaId, dataConsultar);
                int amostra = GetNumeroAmostraData(unidadeId, departamentoId, operacaoId, tarefaId, avaliacao, dataConsultar);

                int maximoAmostras = GetMaximoNumeroAmostra(unidadeId, departamentoId, operacaoId, tarefaId, dataConsultar);

                if (amostra == maximoAmostras || (avaliacao == 0 && amostra == 0))
                {
                    avaliacao++;
                }

                int maximoAvaliacoes = GetMaximoNumeroAvaliacao(unidadeId, departamentoId, operacaoId, tarefaId, dataConsultar);

                if ((controlarAvaliacoes && maximoAvaliacoes == 0) || avaliacao > maximoAvaliacoes)
                {
                    retorno = false;
                }

                numeroAvaliacao = avaliacao;

                return retorno;
            }
        }

        /// <summary>
        /// Retorna o próximo número de amostra
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="departamentoId">Id do Departamento</param>
        /// <param name="operacaoId">Id da Operação</param>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <param name="numeroAvaliacao">Número da avaliação a ser consultada</param>
        /// <returns></returns>
        public static int GetProximoNumeroAmostra(int unidadeId, int departamentoId, int operacaoId, int tarefaId, int numeroAvaliacao, bool decrescer = false)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                int retorno = 0;

                int amostraAtual = GetNumeroAmostraRealizada(unidadeId, departamentoId, operacaoId, tarefaId, numeroAvaliacao, decrescer);

                amostraAtual++;

                int maximoAmostras = GetMaximoNumeroAmostra(unidadeId, departamentoId, operacaoId, tarefaId);

                if (amostraAtual <= maximoAmostras)
                {
                    retorno = amostraAtual;
                }

                return retorno;
            }
        }

        /// <summary>
        /// Retorna o próximo número de amostra da avaliação e data informadas
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="departamentoId">Id do Departamento</param>
        /// <param name="operacaoId">Id da Operação</param>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <param name="numeroAvaliacao">Número da avaliação a ser consultada</param>
        /// <param name="dataConsultar">Data a ser consultada</param>
        /// <returns></returns>
        public static int GetProximoNumeroAmostra(int unidadeId, int departamentoId, int operacaoId, int tarefaId, int numeroAvaliacao, DateTime dataConsultar)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                int retorno = 0;

                int amostraAtual = GetNumeroAmostraRealizada(unidadeId, departamentoId, operacaoId, tarefaId, numeroAvaliacao, dataConsultar);

                amostraAtual++;

                int maximoAmostras = GetMaximoNumeroAmostra(unidadeId, departamentoId, operacaoId, tarefaId, dataConsultar);

                if (amostraAtual <= maximoAmostras)
                {
                    retorno = amostraAtual;
                }

                return retorno;
            }
        }

        /// <summary>
        /// Retorna o próximo número de amostra da avaliação e data informadas
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="departamentoId">Id do Departamento</param>
        /// <param name="operacaoId">Id da Operação</param>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <param name="numeroAvaliacao">Número da avaliação a ser consultada</param>
        /// <param name="dataConsultar">Data a ser consultada</param>
        /// <returns></returns>
        public static int GetProximoNumeroAmostraData(int unidadeId, int departamentoId, int operacaoId, int tarefaId, int numeroAvaliacao, DateTime dataConsultar)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                int retorno = 0;

                int amostraAtual = GetNumeroAmostraData(unidadeId, departamentoId, operacaoId, tarefaId, numeroAvaliacao, dataConsultar);

                amostraAtual++;

                int maximoAmostras = GetMaximoNumeroAmostra(unidadeId, departamentoId, operacaoId, tarefaId, dataConsultar);

                if (amostraAtual <= maximoAmostras)
                {
                    retorno = amostraAtual;
                }

                return retorno;
            }
        }

        /// <summary>
        /// Retorna a data correta de avaliação considerando o término do segundo turno configurado
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="operacaoId">Id da Operação</param>
        /// <returns></returns>
        public static DateTime GetDataSegundoTurno(int unidadeId, int operacaoId)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                DateTime dataConsultar;

                var horario = db.Horarios.AsNoTracking()
                    .Where(h => h.OperacaoId == operacaoId && h.UnidadeId.HasValue && h.UnidadeId.Value == unidadeId)
                    .Select(h => h.Hora).FirstOrDefault();

                if (horario == TimeSpan.Zero)
                {
                    horario = db.Horarios.AsNoTracking()
                        .Where(h => h.OperacaoId == operacaoId && !h.UnidadeId.HasValue)
                        .Select(h => h.Hora).FirstOrDefault();
                }

                if (DateTime.Now.TimeOfDay > TimeSpan.Zero && horario >= DateTime.Now.TimeOfDay)
                {
                    dataConsultar = DateTime.Now.AddDays(-1);
                }
                else
                {
                    dataConsultar = DateTime.Now;
                }
                return dataConsultar;
            }
        }

        /// <summary>
        /// Retorna a lista de flags da operação
        /// </summary>
        /// <param name="operacaoId">Id da Operação</param>
        /// <returns></returns>
        public static List<Tuple<string, object, string>> GetFlagsOperacao(int operacaoId)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                var flags = (from o in db.Operacoes.AsNoTracking()
                             where o.Id == operacaoId
                             select new
                             {
                                 Nivel = o.Nivel,
                                 Frequencia = o.Frequencia,
                                 FrequenciaAlerta = o.FrequenciaAlerta,
                                 Vigencia = o.Vigencia,
                                 ControleVP = o.ControleVP,
                                 ControleFP = o.ControleFP,
                                 AdCampoVazio = o.ADCAMPOVAZIO,
                                 AvaliarEquipamentos = o.AvaliarEquipamentos,
                                 AvaliarCamaras = o.AvaliarCamaras,
                                 Especifico = o.Especifico,
                                 AlterarAmostra = o.AlterarAmostra,
                                 IncluirTarefa = o.IncluirTarefa,
                                 IncluirAvaliacao = o.IncluirAvaliacao,
                                 ExibirPColeta = o.ExibirPColeta,
                                 PadraoPerc = o.PadraoPerc,
                                 AvaliarSequencial = o.AvaliarSequencial,
                                 Criterio = o.Criterio,
                                 ExibirData = o.ExibirData,
                                 EmitirAlerta = o.EmitirAlerta,
                                 ControlarAvaliacoes = o.ControlarAvaliacoes
                             })
                            .First();

                var retorno = new List<Tuple<string, object, string>>();

                foreach (var prop in flags.GetType().GetProperties())
                {
                    retorno.Add(Tuple.Create(prop.Name, prop.GetValue(flags, null), (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? Nullable.GetUnderlyingType(prop.PropertyType).Name : prop.PropertyType.Name));
                }

                return retorno;
            }
        }

        /// <summary>
        /// Retorna a lista de flags da tarefa
        /// </summary>
        /// <param name="tarefaId">Id da Tarefa</param>
        /// <returns></returns>
        public static List<Tuple<string, object, string>> GetFlagsTarefa(int tarefaId)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                var flags = (from t in db.Tarefas.AsNoTracking()
                             where t.Id == tarefaId
                             select new
                             {
                                 Amostragem = t.Amostragem,
                                 Departamento = t.Departamento.HasValue ? t.Departamentos.Nome : string.Empty,
                                 Frequencia = t.Frequencia,
                                 Vigencia = t.Vigencia,
                                 Produto = t.Produto.HasValue ? t.Produtos.Nome : string.Empty,
                                 EditarAcesso = t.EditarAcesso,
                                 ExibirAcesso = t.ExibirAcesso,
                                 FormaAmostragem = t.FormaAmostragem,
                                 AvaliarProdutos = t.AvaliarProdutos,
                                 InformarPesagem = t.InformarPesagem,
                                 Sequencial = t.Sequencial
                             })
                            .First();

                var retorno = new List<Tuple<string, object, string>>();

                foreach (var prop in flags.GetType().GetProperties())
                {
                    retorno.Add(Tuple.Create(prop.Name, prop.GetValue(flags, null), (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? Nullable.GetUnderlyingType(prop.PropertyType).Name : prop.PropertyType.Name));
                }

                return retorno;
            }
        }

        /// <summary>
        /// Retorna as unidades que o usuário tem acesso
        /// </summary>
        /// <param name="usuarioId">Id do Usuário</param>
        /// <returns></returns>
        public static IEnumerable<Tuple<int, string>> GetUnidadesPorUsuarioId(int usuarioId)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                var retorno = (from uu in db.UsuarioUnidades.AsNoTracking()
                               join u in db.Unidades.AsNoTracking() on uu.Unidade equals u.Id
                               where uu.Usuario == usuarioId
                               && u.Ativa.Value
                               select new { UnidadeId = u.Id, Unidade = u.Nome })
                                .Distinct()
                                .AsEnumerable()
                                .Select(x => Tuple.Create(x.UnidadeId, x.Unidade))
                                .ToList();

                return retorno;
            }
        }

        public static int GetUltimoIndiceReincidencia(int unidadeId, int departamentoId, int operacaoId, int tarefaId, int monitoramentoId, DateTime dataConsultar)
        {
            var dataMesAnterior = dataConsultar.AddMonths(-1);

            using (var db = new SGQ_GlobalEntities())
            {
                int? ultimoIndice = (from i in db.PenalidadeReincidencia.AsNoTracking()
                                     where i.UnidadeId == unidadeId
                                     && i.DepartamentoId == departamentoId
                                     && i.OperacaoId == operacaoId
                                     && i.TarefaId == tarefaId
                                     && i.MonitoramentoId == monitoramentoId
                                     && (i.Data.Year == dataMesAnterior.Year && i.Data.Month == dataMesAnterior.Month)
                                     select i.Indice)
                                    .FirstOrDefault();

                return !ultimoIndice.HasValue ? 0 : ultimoIndice.Value;
            }
        }

        public static JsonResult GetNumeroColetaPorPeriodo(string dtInicio, string dtFim)
        {

            try
            {
                var result = 0;
                using (var db = new SGQ_GlobalEntities())
                {
                    var dataInicio = DateTime.ParseExact(dtInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var varDatafim = DateTime.ParseExact(dtFim, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    result = db.Resultados
                        .AsNoTracking()
                        .Where(r => DbFunctions.TruncateTime(r.Data) >= DbFunctions.TruncateTime(dataInicio) && DbFunctions.TruncateTime(r.Data) <= DbFunctions.TruncateTime(varDatafim))
                        .Count();
                }
                if (result > 0)
                {
                    return RetornoPadraoJson(result, false);
                }
                else
                {
                    return RetornoPadraoJson(result, true);
                }
            }
            catch (Exception ex)
            {
                return RetornoPadraoJsonException(ex);
            }

        }

        public static DictionaryEntry getResource(string value)
        {
            System.Resources.ResourceManager resourceManager = Resources.Resource.ResourceManager;

            if (GlobalConfig.LanguageBrasil)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Guard.LANGUAGE_PT_BR);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Guard.LANGUAGE_PT_BR);
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("");
            }

            var list = resourceManager.GetResourceSet(
                Thread.CurrentThread.CurrentUICulture, true, false);
            

            var listRes = list.Cast<DictionaryEntry>();

            foreach (var r in listRes)
            {
                if (r.Key.ToString() == value)
                    return r;
            }

            return new DictionaryEntry();
        }

        public static MvcHtmlString ParCounterList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> values, Object htmlAttributes)
        {

            List<SelectListItem> Textes = new List<SelectListItem>();
            foreach (SelectListItem item in values)
            {
                SelectListItem selItem = new SelectListItem();
                selItem.Value = item.Value.ToString();
                if (getResource(item.Text.ToString()).Value != null)
                    selItem.Text = getResource(item.Text.ToString()).Value.ToString();
                else
                    selItem.Text = item.Text.ToString();
                Textes.Add(selItem);
            }

            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
                                                                     name,
                                                                     Textes,
                                                                     htmlAttributes);
        }

        public static MvcHtmlString ParLocalList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> values, Object htmlAttributes)
        {

            List<SelectListItem> Textes = new List<SelectListItem>();
            foreach (SelectListItem item in values)
            {
                SelectListItem selItem = new SelectListItem();
                selItem.Value = item.Value.ToString();
                if (getResource(item.Text.ToString()).Value != null)
                    selItem.Text = getResource(item.Text.ToString()).Value.ToString();
                else
                    selItem.Text = item.Text.ToString();
                Textes.Add(selItem);
            }

            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
                                                                     name,
                                                                     Textes,
                                                                     htmlAttributes);
        }

        //public static JsonResult DebugAlertas()
        //{

        //    var UnidadeId = 20;
        //    var OperacaoId = 1;
        //    var DepartamentoId = 2;
        //    var NumeroAvaliacao = 1;
        //    var Operacao = "";
        //    var AV = 0;
        //    var NC = 0;
        //    var Meta = 0;
        //    var Real = 0;
        //    bool EmitirAlerta = false;
        //    bool AlertaAgruparAvaliacoes = false;
        //    var AlertaEmitido = 0;
        //    var DesvioEncontrado = 0;
        //    var MetaDia = 0;
        //    var Nivel_1 = 0;
        //    var Nivel_2 = 0;
        //    var ProximaMetaTolerancia = 0;
        //    var UltimoNumeroNC = 0;

        //    using (var db = new SGQ_GlobalEntities())
        //    {


        //        //Obtenho informações do indicador

        //        //select * from view_alertaniveis where unidade = @unidadeid and operacao = @operacaoid
        //        var r1 = db.View_AlertaNiveis.AsNoTracking().Where(r => r.Unidade == UnidadeId && r.Operacao == OperacaoId).FirstOrDefault();

        //        //  SELECT @EmitirAlerta = EmitirAlerta
        //        //        ,@AlertaAgruparAvaliacoes = AlertaAgruparAvaliacoes
        //        //    FROM Operacoes WITH (NOLOCK)
        //        //   WHERE Id = @OperacaoId
        //        var r2 = db.Operacoes.Find(OperacaoId);
        //        EmitirAlerta = r2.EmitirAlerta.IsNotNull();
        //        AlertaAgruparAvaliacoes = r2.AlertaAgruparAvaliacoes.IsNotNull();
        //        //SELECT @EmitirAlerta EmitirAlerta, @AlertaAgruparAvaliacoes AlertaAgruparAvaliacoes


        //        //  -- Obtenho os niveis de alerta para o indicador e unidade
        //        //SELECT @Operacao = Operacao, @MetaDia = Nivel_3, @Nivel_1 = Nivel_1, @Nivel_2 = Nivel_2
        //        //  FROM View_AlertaNiveis
        //        // WHERE Unidade = @UnidadeId
        //        //   AND Operacao = @OperacaoId
        //        var r3 = db.View_AlertaNiveis.Where(r => r.Unidade == UnidadeId && r.Operacao == OperacaoId).FirstOrDefault();


        //        //IF (@@ROWCOUNT = 0)
        //        //BEGIN
        //        //  SELECT @Operacao = Operacao, @MetaDia = Nivel_3, @Nivel_1 = Nivel_1, @Nivel_2 = Nivel_2
        //        //    FROM View_AlertaNiveis
        //        //   WHERE Unidade IS NULL
        //        //     AND Operacao = @OperacaoId
        //        //END

        //        return new JsonResult()
        //        {
        //            Data = new
        //            {
        //                Data = new
        //                {
        //                    rSet1 = r1,
        //                    rSet2 = new List<bool>() { EmitirAlerta, AlertaAgruparAvaliacoes },


        //                }
        //            },
        //            JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //        };


        //    }


        //    //    -- Obtenho os niveis de alerta para o indicador de forma corporativa, caso nao exista niveis para a unidade
        //    //    IF (@@ROWCOUNT = 0)
        //    //    BEGIN
        //    //      SELECT @Operacao = Operacao, @MetaDia = Nivel_3, @Nivel_1 = Nivel_1, @Nivel_2 = Nivel_2
        //    //        FROM View_AlertaNiveis
        //    //       WHERE Unidade IS NULL
        //    //         AND Operacao = @OperacaoId
        //    //    END

        //    //      SELECT @Operacao Operacao, @Nivel_1 Nivel_1, @Nivel_2 Nivel_2, @MetaDia MetaDia

        //    //      --Obtenho os dados de avaliação e não conformidade
        //    //      SELECT @Operacao = Operacao
        //    //            ,@Meta = Meta
        //    //            ,@AV = AV
        //    //            ,@NC = NC
        //    //            ,@Real = Real
        //    //        FROM dbo.fn_GetScoreCardPorOperacaoPeriodo(@UnidadeId, @OperacaoId, '20160616', '20160616')

        //    //      SELECT @Operacao Operacao, @Meta Meta, @AV AV, @NC NC, @Real Real

        //    //      --Obtenho o último alerta emitido e desvio encontrado
        //    //      SELECT @DesvioEncontrado = ISNULL(MAX(Desvio), 0)
        //    //        FROM Desvios WITH (NOLOCK)
        //    //       WHERE UnidadeId = @UnidadeId
        //    //         AND OperacaoId = @OperacaoId
        //    //         AND CONVERT(date, DataHora) = CONVERT(date, GETDATE())

        //    //      SELECT @AlertaEmitido = ISNULL(MAX(AlertaEmitido), 0)
        //    //        FROM Desvios WITH (NOLOCK)
        //    //       WHERE UnidadeId = @UnidadeId
        //    //         AND OperacaoId = @OperacaoId
        //    //         AND NumeroAvaliacao = @NumeroAvaliacao
        //    //         AND CONVERT(date, DataHora) = CONVERT(date, GETDATE())

        //    //      --Cálculo dos parametros para emissão de alerta
        //    //      SELECT @ProximaMetaTolerancia = ProximaMetaTolerancia
        //    //        FROM ControleMetaTolerancia WITH(NOLOCK)
        //    //       WHERE UnidadeId = @UnidadeId
        //    //         AND OperacaoId = @OperacaoId
        //    //         AND Data = CONVERT(date, GETDATE())

        //    //      SELECT @DesvioEncontrado DesvioEncontrado, @AlertaEmitido AlertaEmitido, @ProximaMetaTolerancia ProximaMetaTolerancia

        //    //      --Cálculo dos parametros para emissão de alerta
        //    //      SELECT @ProximaMetaTolerancia = ProximaMetaTolerancia
        //    //            ,@UltimoNumeroNC = UltimoNumeroNC
        //    //        FROM ControleMetaTolerancia WITH(NOLOCK)
        //    //       WHERE UnidadeId = @UnidadeId
        //    //         AND OperacaoId = @OperacaoId
        //    //         AND Data = CONVERT(date, GETDATE())

        //    //      SELECT @ProximaMetaTolerancia ProximaMetaTolerancia
        //    //            ,@UltimoNumeroNC UltimoNumeroNC

        //    //  --Obtenho a quantidade de monitoramentos que faltam ser avaliados
        //    //  SELECT COUNT(Id) monitoramentosfaltantes
        //    //    FROM (SELECT T.Id
        //    //            FROM Tarefas T WITH (NOLOCK)
        //    //          INNER JOIN Operacoes O WITH (NOLOCK) ON O.Id = T.Operacao
        //    //          WHERE T.Operacao = @OperacaoId
        //    //            AND ((@AlertaAgruparAvaliacoes = 0 AND T.Departamento = @DepartamentoId) OR @AlertaAgruparAvaliacoes = 1 OR T.Departamento IS NULL)
        //    //            AND ((O.ControleFP = 0)
        //    //              OR (O.ControleFP = 1
        //    //                  AND EXISTS (SELECT Id 
        //    //                                FROM FamiliaProdutos WITH (NOLOCK)
        //    //                                WHERE (Operacao = @OperacaoId OR Operacao IS NULL)
        //    //                                  AND Ano = YEAR(GETDATE()) 
        //    //                                  AND Mes = MONTH(GETDATE()) 
        //    //                                  AND Produto = T.Produto 
        //    //                                  AND (Unidade = @UnidadeId OR Unidade IS NULL))))
        //    //          ) t
        //    //    LEFT JOIN     (SELECT DISTINCT TarefaId
        //    //                      FROM Resultados WITH (NOLOCK)
        //    //                    WHERE EmpresaId = 1
        //    //                      AND UnidadeId = @UnidadeId
        //    //                      AND ((@AlertaAgruparAvaliacoes = 0 AND DepartamentoId = @DepartamentoId) OR @AlertaAgruparAvaliacoes = 1)
        //    //                      AND OperacaoId = @OperacaoId
        //    //                      AND NumeroAvaliacao = @NumeroAvaliacao
        //    //                      AND NumeroAmostra = dbo.GetMaximoNumeroAmostra(EmpresaId, UnidadeId, DepartamentoId, OperacaoId, TarefaId)
        //    //                      AND Data = CONVERT(date, GETDATE())
        //    //                      AND AvaliacaoAvulsa = 0
        //    //                  ) tt
        //    //      ON tt.TarefaId = t.Id
        //    //  WHERE tt.TarefaId IS NULL

        //    //      --Cálculo dos parametros para emissão de alerta
        //    //      SELECT totalnc, metadia, avaliacoes, tolerancia, CASE WHEN ISNULL(@ProximaMetaTolerancia, 0) = 0 THEN tolerancia ELSE @ProximaMetaTolerancia END metatolerancia, (metadia / avaliacoes) metaavaliacao from (
        //    //      SELECT (metadia / 3) tolerancia, totalnc, metadia, avaliacoes from (
        //    //      SELECT (@NC) totalnc, (@MetaDia) metadia, dbo.fn_AlertaNumeroAvaliacoes(@UnidadeId, @OperacaoId) avaliacoes) t) tt

        //}

        #region Retorno Json

        public static JsonResult RetornoPadraoJson<T>(T obj, bool semDados)
        {

            if (semDados)
            {
                return new JsonResult()
                {
                    Data = new { mensagemSemDados = "Não existem dados para o período selecionado." },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                return new JsonResult()
                {
                    Data = new { Data = new { data = obj } },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

        }

        public static JsonResult RetornoPadraoJsonException(Exception ex)
        {
            var inner = ex.InnerException.IsNotNull() ? ex.InnerException.Message : "Não consta.";
            return new JsonResult()
            {
                Data = new { mensagemException = "Ocorreu um problema ao buscar os dados.", mensagemParaDebugNavegador = ex.Message + " Inner:" + inner },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public static JsonResult RetornoTratadoJson<T>(object obj)
        {


            return new JsonResult()
            {
                Data = new { errMsg = "test" },
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }

        #endregion

    }
}