using QualidadeTotal.PlanoAcao.DTO;
using SgqSystem.PlanoAcao.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace PA.Data
{
    public class PlanoAcaoData
    {

        #region ACOES

        public TarefaPA AlterarTarefa(TarefaPA tarefa)
        {
            using (var db = new Entities())
            {

                var result = db.TarefaPA
                  .Where(p => p.Ativo && p.Id == tarefa.Id)
                  .AsNoTracking().FirstOrDefault();

                result = new TarefaPA()
                {
                    Id = result.Id,
                    IdProjeto = result.IdProjeto,
                    DataCriacao = result.DataCriacao,
                    Ativo = result.Ativo,
                    IdParticipanteCriador = result.IdParticipanteCriador,
                    DataAlteracao = DateTime.Now,
                    VinculoCampoTarefa = null,
                    AcompanhamentoTarefa = null,
                    Projeto = null,
                    Usuarios = null,
                };

                db.TarefaPA.Attach(result);
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();

                db.Dispose();

                return result;
            }
        }

        public TarefaPA SalvarTarefa(TarefaPA tarefa)
        {
            using (var db = new Entities())
            {
                db.TarefaPA.Add(tarefa);
                db.Entry(tarefa).State = EntityState.Added;
                db.SaveChanges();

                db.Dispose();

                return tarefa;
            }
        }

        public Campo SalvarCabecalho(Campo cabecalho)
        {
            using (var db = new Entities())
            {
                db.Cabecalho.Add(cabecalho);

                if (cabecalho.Id != 0)
                {
                    db.Entry(cabecalho).State = EntityState.Modified;
                }
                else
                {
                    db.Entry(cabecalho).State = EntityState.Added;
                }
                db.SaveChanges();

                db.Dispose();

                return cabecalho;
            }
        }

        public List<VinculoCampoCabecalho> SalvarVinculoCampoCabecalho(List<VinculoCampoCabecalho> vinculoCampoCabecalho)
        {
            using (var db = new Entities())
            {
                foreach (var i in vinculoCampoCabecalho)
                {
                    db.VinculoCampoCabecalho.Add(i);
                    db.Entry(i).State = EntityState.Added;
                }

                db.SaveChanges();

                db.Dispose();

                return vinculoCampoCabecalho;
            }
        }

        public List<VinculoCampoTarefa> AlterarVinculoCampoTarefa(List<VinculoCampoTarefa> vinculoCampoTarefa)
        {
            using (var db = new Entities())
            {
                foreach (var i in vinculoCampoTarefa)
                {
                    db.VinculoCampoTarefa.Add(i);
                    if (i.Id != 0)
                    {
                        db.Entry(i).State = EntityState.Modified;
                    }
                    else
                    {
                        db.Entry(i).State = EntityState.Added;
                    }
                }

                db.SaveChanges();

                db.Dispose();

                return vinculoCampoTarefa;
            }
        }

        public List<VinculoCampoTarefa> SalvarVinculoCampoTarefa(List<VinculoCampoTarefa> vinculoCampoTarefa)
        {
            using (var db = new Entities())
            {
                foreach (var i in vinculoCampoTarefa)
                {
                    db.VinculoCampoTarefa.Add(i);
                    db.Entry(i).State = EntityState.Added;
                }

                db.SaveChanges();

                db.Dispose();

                return vinculoCampoTarefa;
            }
        }

        public List<AcompanhamentoTarefa> SalvarAcompanhamentoTarefa(List<AcompanhamentoTarefa> acompanhamentoTarefa)
        {
            using (var db = new Entities())
            {
                foreach (var i in acompanhamentoTarefa)
                {
                    db.AcompanhamentoTarefa.Add(i);
                    db.Entry(i).State = EntityState.Added;
                }

                db.SaveChanges();

                db.Dispose();

                return acompanhamentoTarefa;
            }
        }

        public TarefaPA DesativarTarefaPorId(int idTarefa)
        {
            using (var db = new Entities())
            {

                var result = db.TarefaPA
                  .Where(p => p.Ativo && p.Id == idTarefa)
                  .AsNoTracking().FirstOrDefault();

                result = new TarefaPA()
                {
                    Id = result.Id,
                    IdProjeto = result.IdProjeto,
                    DataCriacao = result.DataCriacao,
                    Ativo = false,
                    IdParticipanteCriador = result.IdParticipanteCriador,
                    DataAlteracao = DateTime.Now,
                    VinculoCampoTarefa = null,
                    AcompanhamentoTarefa = null,
                    Usuarios = null,
                    Projeto = null
                };

                db.TarefaPA.Attach(result);
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();

                db.Dispose();

                return result;
            }
        }

        public List<MultiplaEscolha> SalvarMultiplaEscolha(List<MultiplaEscolha> multiplaEscolha)
        {
            using (var db = new Entities())
            {
                foreach (var i in multiplaEscolha)
                {
                    db.MultiplaEscolha.Add(i);
                    db.Entry(i).State = EntityState.Added;
                }

                db.SaveChanges();

                db.Dispose();

                return multiplaEscolha;
            }
        }


        #endregion

        #region CONSULTA

        public List<object> BuscarCamposJbs(string query)
        {
            using (var db = new Dominio.SgqDbDevEntities())
            {
                List<object> items = new List<object>();

                db.Database.Connection.Open();

                var cmd = db.Database.Connection.CreateCommand();

                cmd.CommandText = query.ToString();

                var reader = cmd.ExecuteReader();
                //while (reader.Read())
                //{
                //    Dictionary<string, object> dict = new Dictionary<string, object>();

                //    for (int i = 0; i < reader.FieldCount; i++)
                //    {

                //        var _temp = reader[i];

                //        if (_temp.GetType() == typeof(string))
                //        {
                //            _temp = _temp.ToString().TrimEnd();
                //        }

                //        dict.Add(reader.GetName(i).ToString(), _temp);
                //    }

                //    items.Add(dict);
                //}

                while (reader.Read())
                {

                    object empData = new { Id = reader[0], Nome = reader[1] };

                    items.Add(empData);
                }

                db.Dispose();

                return items;
            }
        }

        public List<MultiplaEscolha> BuscarListaMultiplaEscolhaFilho(int idMult, int idCampo)
        {
            using (var db = new Entities())
            {
                var result = db.MultiplaEscolha
                 .Where(p => p.Ativo && p.IdMultiplaEscolhaPai == idMult && p.IdCampo == idCampo)
                 .AsNoTracking().ToList();

                result = result.Select(y => new MultiplaEscolha()
                {
                    Ativo = y.Ativo,
                    Cor = y.Cor,
                    DataCriacao = y.DataCriacao,
                    Id = y.Id,
                    IdCampo = y.IdCampo,
                    Nome = y.Nome,
                    DataAlteracao = y.DataAlteracao,
                    IdMultiplaEscolhaPai = y.IdMultiplaEscolhaPai
                }).ToList();

                db.Dispose();

                return result;
            }
        }

        public List<MultiplaEscolha> BuscarListaMultiplaEscolhaPorCampoId(int id, string nomeTabela)
        {
            using (var db = new Entities())
            {

                var result = db.MultiplaEscolha
                 .Where(p => p.Ativo && p.IdCampo == id && p.NomeTabelaExterna.Equals(nomeTabela))
                 .AsNoTracking().ToList();

                result = result.Select(y => new MultiplaEscolha()
                {
                    Ativo = y.Ativo,
                    Cor = y.Cor,
                    DataCriacao = y.DataCriacao,
                    Id = y.Id,
                    IdCampo = y.IdCampo,
                    Nome = y.Nome,
                    DataAlteracao = y.DataAlteracao,
                    IdTabelaExterna = y.IdTabelaExterna,
                    NomeTabelaExterna = y.NomeTabelaExterna,
                    IdMultiplaEscolhaPai = y.IdMultiplaEscolhaPai
                }).ToList();

                db.Dispose();

                return result;

            }
        }

        public Campo BuscarCampo(string nome, int id)
        {
            using (var db = new Entities())
            {
                var result = new Campo();

                if (nome.Equals(string.Empty) || id != 0)
                {
                    result = db.Campo
                                    .Where(p => p.Ativo && p.Id == id)
                                    .AsNoTracking().FirstOrDefault();
                }
                else
                {
                    result = db.Campo
                                     .Where(p => p.Ativo && p.Nome.Equals(nome))
                                     .AsNoTracking().FirstOrDefault();
                }

                if (result != null)
                {
                    result = new Campo()
                    {
                        Ativo = result.Ativo,
                        DataCriacao = result.DataCriacao,
                        IdProjeto = result.IdProjeto,
                        Modificavel = result.Modificavel,
                        Agrupador = result.Agrupador,
                        Id = result.Id,
                        Nome = result.Nome,
                        Obrigatorio = result.Obrigatorio,
                        Predefinido = result.Predefinido,
                        Sequencia = result.Sequencia != null ? result.Sequencia.Value : 0,
                        Tipo = result.Tipo,
                        DataAlteracao = result.DataAlteracao,
                        FixadoEsquerda = result.FixadoEsquerda == null ? false : result.FixadoEsquerda,
                        ExibirTabela = result.ExibirTabela == null ? false : result.ExibirTabela,
                        IdCampoPai = result.IdCampoPai == null ? 0 : result.IdCampoPai
                    };
                }

                db.Dispose();

                return result;
            }
        }

        public List<TarefaPA> BuscarTodasTarefas()
        {
            using (var db = new Entities())
            {


                var list = db.TarefaPA
                  .Where(p => p.Ativo)
                  .AsNoTracking().ToList();

                list = list.Select(tar => new TarefaPA()
                {
                    Id = tar.Id,
                    IdProjeto = tar.IdProjeto,
                    DataCriacao = tar.DataCriacao,
                    Ativo = tar.Ativo,
                    IdParticipanteCriador = tar.IdParticipanteCriador,
                    VinculoCampoTarefa = tar.VinculoCampoTarefa.Select(vinc => new VinculoCampoTarefa()
                    {
                        Valor = vinc.Valor,
                        Id = vinc.Id,
                        IdMultiplaEscolha = vinc.IdMultiplaEscolha,
                        IdCampo = vinc.IdCampo,
                        IdParticipante = vinc.IdParticipante,
                        IdTarefa = vinc.IdTarefa,
                        Campo = vinc.Campo == null ? null : !vinc.Campo.Ativo ? null : new Campo()
                        {
                            Ativo = vinc.Campo.Ativo,
                            DataCriacao = vinc.Campo.DataCriacao,
                            IdProjeto = vinc.Campo.IdProjeto,
                            Modificavel = vinc.Campo.Modificavel,
                            Agrupador = vinc.Campo.Agrupador,
                            Id = vinc.Campo.Id,
                            Nome = vinc.Campo.Nome,
                            Obrigatorio = vinc.Campo.Obrigatorio,
                            Predefinido = vinc.Campo.Predefinido,
                            Sequencia = vinc.Campo.Sequencia != null ? vinc.Campo.Sequencia.Value : 0,
                            Tipo = vinc.Campo.Tipo,
                            DataAlteracao = vinc.Campo.DataAlteracao,
                            IdCampoPai = vinc.Campo.IdCampoPai == null ? 0 : vinc.Campo.IdCampoPai,
                            FixadoEsquerda = vinc.Campo.FixadoEsquerda == null ? false : vinc.Campo.FixadoEsquerda,
                            ExibirTabela = vinc.Campo.ExibirTabela == null ? false : vinc.Campo.ExibirTabela,
                        },
                        MultiplaEscolha = vinc.MultiplaEscolha == null ? null : !vinc.MultiplaEscolha.Ativo ? null : new MultiplaEscolha()
                        {
                            Ativo = vinc.MultiplaEscolha.Ativo,
                            Cor = vinc.MultiplaEscolha.Cor,
                            DataCriacao = vinc.MultiplaEscolha.DataCriacao,
                            Id = vinc.MultiplaEscolha.Id,
                            IdCampo = vinc.MultiplaEscolha.IdCampo,
                            Nome = vinc.MultiplaEscolha.Nome,
                            DataAlteracao = vinc.MultiplaEscolha.DataAlteracao,
                            IdTabelaExterna = vinc.MultiplaEscolha.IdTabelaExterna,
                            NomeTabelaExterna = vinc.MultiplaEscolha.NomeTabelaExterna,
                            IdMultiplaEscolhaPai = vinc.MultiplaEscolha.IdMultiplaEscolhaPai
                        },
                        Usuarios = vinc.Usuarios == null ? null : new Usuarios()
                        {
                            Email = vinc.Usuarios.Email,
                            Nome = vinc.Usuarios.Nome,
                            Id = vinc.Usuarios.Id,
                            Unidade = vinc.Usuarios.Unidade,
                        }
                    }).GroupBy(r => r.Id).Select(group => group.First()).ToList(),/**/
                }).ToList();



                //var View_Status_PA = db.VIEW_STATUS_PA.ToList();

                //var NovoResultado = from tarefas in list
                //                    join viewStatusPa in View_Status_PA
                //                    on tarefas.Id equals viewStatusPa.IdTarefa
                //                    select new { tarefas, viewStatusPa };

                //List<TarefaPA> novo = new List<TarefaPA>();

                //list = NovoResultado.Select(tar => new TarefaPA()
                //{
                //    Id = tar.tarefas.Id,
                //    IdProjeto = tar.tarefas.IdProjeto,
                //    DataCriacao = tar.tarefas.DataCriacao,
                //    Ativo = tar.tarefas.Ativo,
                //    IdParticipanteCriador = tar.tarefas.IdParticipanteCriador,
                //    VinculoCampoTarefa = tar.tarefas.VinculoCampoTarefa,
                //    VinculoCampoTarefa = tar.viewStatusPa,

                //}).ToList();


                db.Dispose();


                return list;


                #region Testes
                //var list = (from tar in db.Tarefa
                //            join vin in db.VinculoCampoTarefa on tar.Id equals vin.IdTarefa
                //            join camp in db.Campo on vin.IdCampo equals camp.Id
                //            join mult in db.MultiplaEscolha on vin.IdMultiplaEscolha equals mult.Id
                //            select tar);

                //var aaa = list.ToList();




                //var sqlQuery = "SELECT [Id] " +
                //                " ,[IdProjeto] " +
                //                " ,[IdParticipanteCriador] " +
                //                " ,[DataCriacao] " +
                //                " ,[DataAlteracao] " +
                //                " ,[Ativo] " +
                //                "FROM [dbo].[Tarefa]";

                //var listaBanco = db.Database.SqlQuery<Tarefa>(sqlQuery).ToList();





                //var list65 = (from tar in db.Tarefa.AsNoTracking()
                //              select new Tarefa()
                //              {
                //                  Id = tar.Id,
                //                  IdProjeto = tar.IdProjeto,
                //                  IdParticipanteCriador = tar.IdParticipanteCriador,
                //                  DataCriacao = tar.DataCriacao,
                //                  DataAlteracao = tar.DataAlteracao,
                //                  Ativo = tar.Ativo,
                //                  VinculoCampoTarefa = new List<VinculoCampoTarefa>(),
                //                  AcompanhamentoTarefa = new List<AcompanhamentoTarefa>(),
                //                  Participante = null,
                //                  Projeto = null,
                //              }).ToList();




                //var list = (from tar in db.Tarefa.AsNoTracking()
                //            where tar.Ativo == true
                //            select new Tarefa()
                //            {
                //                Id = tar.Id,
                //                IdProjeto = tar.IdProjeto,
                //                DataCriacao = tar.DataCriacao,
                //                Ativo = tar.Ativo,
                //                VinculoCampoTarefa = new List<VinculoCampoTarefa>()
                //            }).ToList();

                //list.ForEach(x => x.VinculoCampoTarefa = (from vinc in db.VinculoCampoTarefa.AsNoTracking()
                //                                          select new VinculoCampoTarefa()
                //                                          {
                //                                              Campo = new Campo(),
                //                                              MultiplaEscolha = new MultiplaEscolha(),
                //                                              Valor = vinc.Valor,
                //                                              Id = vinc.Id,
                //                                              IdMultiplaEscolha = vinc.IdMultiplaEscolha,
                //                                              IdCampo = vinc.IdCampo,
                //                                              IdParticipante = vinc.IdParticipante,
                //                                              IdTarefa = vinc.IdTarefa,
                //                                          }).ToList());

                //list.ForEach(x => x.VinculoCampoTarefa.ToList().ForEach(y => y.Campo = (from camp in db.Campo.AsNoTracking()
                //                                                                        where camp.Id == y.IdCampo && camp.Ativo == true
                //                                                                        select camp).FirstOrDefault()));


                //list.ForEach(x => x.VinculoCampoTarefa.ToList().ForEach(y => y.MultiplaEscolha = (from mu in db.MultiplaEscolha.AsNoTracking()
                //                                                                                  where mu.Id == y.IdMultiplaEscolha && mu.Ativo == true
                //                                                                                  select mu).FirstOrDefault()));



                //var hujhkhj = (from tar in db.Tarefa
                //               join vin in db.VinculoCampoTarefa on tar.Id equals vin.IdTarefa
                //               join camp in db.Campo on vin.IdCampo equals camp.Id
                //               join mult in db.MultiplaEscolha on vin.IdMultiplaEscolha equals mult.Id
                //               select tar);



                //var list2 = (from tar in db.Tarefa
                //             join vin in db.VinculoCampoTarefa on tar.Id equals vin.IdTarefa into gvin
                //             select new Tarefa()
                //             {
                //                 Id = tar.Id,
                //                 VinculoCampoTarefa = (from vinc in gvin
                //                                       join camp in db.Campo on vinc.IdCampo equals camp.Id
                //                                       join mult in db.MultiplaEscolha on vinc.IdMultiplaEscolha equals mult.Id
                //                                       where vinc.IdTarefa == tar.Id
                //                                       select new VinculoCampoTarefa()
                //                                       {
                //                                           Id = vinc.Id,
                //                                           Valor = vinc.Valor,
                //                                           Campo = new Campo()
                //                                           {
                //                                               Modificavel = camp.Modificavel,
                //                                               Agrupador = camp.Agrupador,
                //                                               Ativo = camp.Ativo,
                //                                               Id = camp.Id,
                //                                               Nome = camp.Nome,
                //                                               Obrigatorio = camp.Obrigatorio,
                //                                               Predefinido = camp.Predefinido,
                //                                               Sequencia = camp.Sequencia != null ? camp.Sequencia.Value : 0,
                //                                               Tipo = camp.Tipo,
                //                                           },
                //                                           MultiplaEscolha = new MultiplaEscolha()
                //                                           {
                //                                               Id = mult.Id,
                //                                               Nome = mult.Nome,
                //                                               Cor = mult.Cor,
                //                                           }
                //                                       }).ToList()
                //             }).ToList();








                //var completo = (from tar in db.Tarefa.AsNoTracking()
                //                    //   join vin in db.VinculoCampoTarefa on tar.Id equals vin.IdTarefa into vinct
                //                select new Tarefa()
                //                {
                //                    Id = tar.Id,
                //                    VinculoCampoTarefa = (from vinc in db.VinculoCampoTarefa.AsNoTracking()
                //                                          where vinc.IdTarefa == tar.Id
                //                                          //  join camp in db.Campo on vinc.IdCampo equals camp.Id
                //                                          select new VinculoCampoTarefa()
                //                                          {
                //                                              Id = vinc.Id,
                //                                              Valor = vinc.Valor,

                //                                              Campo = (from camp in db.Campo.AsNoTracking()
                //                                                       where camp.Id == vinc.IdCampo
                //                                                       select new Campo()
                //                                                       {
                //                                                           Modificavel = camp.Modificavel,
                //                                                           Agrupador = camp.Agrupador,
                //                                                           Ativo = camp.Ativo,
                //                                                           Id = camp.Id,
                //                                                           Nome = camp.Nome,
                //                                                           Obrigatorio = camp.Obrigatorio,
                //                                                           Predefinido = camp.Predefinido,
                //                                                           Sequencia = camp.Sequencia != null ? camp.Sequencia.Value : 0,
                //                                                           Tipo = camp.Tipo,
                //                                                       }).FirstOrDefault(),

                //                                              MultiplaEscolha = (from mu in db.MultiplaEscolha.AsNoTracking()
                //                                                                 where mu.Id == vinc.IdMultiplaEscolha
                //                                                                 select new MultiplaEscolha()
                //                                                                 {
                //                                                                     Id = mu.Id,
                //                                                                     Nome = mu.Nome,
                //                                                                     Cor = mu.Cor,
                //                                                                     //Campo = new PlanoAcaoCampoDTO()
                //                                                                     //{
                //                                                                     //    Id = mu.id_campo
                //                                                                     //}
                //                                                                 }).FirstOrDefault()
                //                                          }).ToList()

                //                });


                #endregion
            }
        }

        public TarefaPA BuscarTarefaPorId(int id)
        {
            using (var db = new Entities())
            {

                var result = db.TarefaPA
                 .Where(p => p.Ativo && p.Id == id)
                 .AsNoTracking().FirstOrDefault();

                result = new TarefaPA()
                {
                    Id = result.Id,
                    IdProjeto = result.IdProjeto,
                    DataCriacao = result.DataCriacao,
                    Ativo = result.Ativo,
                    IdParticipanteCriador = result.IdParticipanteCriador,
                    VinculoCampoTarefa = result.VinculoCampoTarefa.Select(vinc => new VinculoCampoTarefa()
                    {
                        Valor = vinc.Valor,
                        Id = vinc.Id,
                        IdMultiplaEscolha = vinc.IdMultiplaEscolha,
                        IdCampo = vinc.IdCampo,
                        IdParticipante = vinc.IdParticipante,
                        IdTarefa = vinc.IdTarefa,
                        Campo = vinc.Campo == null ? null : !vinc.Campo.Ativo ? null : new Campo()
                        {
                            Ativo = vinc.Campo.Ativo,
                            DataCriacao = vinc.Campo.DataCriacao,
                            IdProjeto = vinc.Campo.IdProjeto,
                            Modificavel = vinc.Campo.Modificavel,
                            Agrupador = vinc.Campo.Agrupador,
                            Id = vinc.Campo.Id,
                            Nome = vinc.Campo.Nome,
                            Obrigatorio = vinc.Campo.Obrigatorio,
                            Predefinido = vinc.Campo.Predefinido,
                            Sequencia = vinc.Campo.Sequencia != null ? vinc.Campo.Sequencia.Value : 0,
                            Tipo = vinc.Campo.Tipo,
                            DataAlteracao = vinc.Campo.DataAlteracao,
                            IdCampoPai = vinc.Campo.IdCampoPai == null ? 0 : vinc.Campo.IdCampoPai,
                            FixadoEsquerda = vinc.Campo.FixadoEsquerda == null ? false : vinc.Campo.FixadoEsquerda,
                            ExibirTabela = vinc.Campo.ExibirTabela == null ? false : vinc.Campo.ExibirTabela,
                        },
                        MultiplaEscolha = vinc.MultiplaEscolha == null ? null : !vinc.MultiplaEscolha.Ativo ? null : new MultiplaEscolha()
                        {
                            Ativo = vinc.MultiplaEscolha.Ativo,
                            Cor = vinc.MultiplaEscolha.Cor,
                            DataCriacao = vinc.MultiplaEscolha.DataCriacao,
                            Id = vinc.MultiplaEscolha.Id,
                            IdCampo = vinc.MultiplaEscolha.IdCampo,
                            Nome = vinc.MultiplaEscolha.Nome,
                            DataAlteracao = vinc.MultiplaEscolha.DataAlteracao,
                            IdTabelaExterna = vinc.MultiplaEscolha.IdTabelaExterna,
                            NomeTabelaExterna = vinc.MultiplaEscolha.NomeTabelaExterna,
                            IdMultiplaEscolhaPai = vinc.MultiplaEscolha.IdMultiplaEscolhaPai
                        },
                        Usuarios = vinc.Usuarios == null ? null : new Usuarios()
                        {
                            Email = vinc.Usuarios.Email,
                            Nome = vinc.Usuarios.Nome,
                            Id = vinc.Usuarios.Id,
                            Unidade = vinc.Usuarios.Unidade,
                        }
                    }).ToList()
                };

                db.Dispose();

                return result;
            }
        }

        public List<Campo> BuscarTodosCamposProjetoPorId(int id)
        {
            using (var db = new Entities())
            {

                var result = db.Campo
                 .Where(p => p.Ativo && p.Projeto.Id == id)
                 .AsNoTracking().ToList();

                result = result.Select(x => new Campo()
                {
                    Agrupador = x.Agrupador,
                    Ativo = x.Ativo,
                    DataCriacao = x.DataCriacao,
                    Id = x.Id,
                    IdProjeto = x.IdProjeto,
                    Modificavel = x.Modificavel,
                    Nome = x.Nome,
                    Obrigatorio = x.Obrigatorio,
                    Predefinido = x.Predefinido,
                    Sequencia = x.Sequencia,
                    Tipo = x.Tipo,
                    IdCampoPai = x.IdCampoPai == null ? 0 : x.IdCampoPai,
                    FixadoEsquerda = x.FixadoEsquerda == null ? false : x.FixadoEsquerda,
                    ExibirTabela = x.ExibirTabela == null ? false : x.ExibirTabela,
                    Cabecalho = x.Cabecalho == null ? false : x.Cabecalho,
                    IdGrupoCabecalho = x.IdGrupoCabecalho == null ? 0 : x.IdGrupoCabecalho,
                    // MultiplaEscolha = x.IdCampoPai != null ? null : x.MultiplaEscolha.Select(y => y == null ? null : !y.Ativo ? null : new MultiplaEscolha()
                    MultiplaEscolha = x.MultiplaEscolha.Select(y => y == null ? null : !y.Ativo ? null : new MultiplaEscolha()
                    {
                        Ativo = y.Ativo,
                        Cor = y.Cor,
                        DataCriacao = y.DataCriacao,
                        Id = y.Id,
                        IdCampo = y.IdCampo,
                        Nome = y.Nome,
                        DataAlteracao = y.DataAlteracao,
                        IdTabelaExterna = y.IdTabelaExterna,
                        NomeTabelaExterna = y.NomeTabelaExterna,
                        IdMultiplaEscolhaPai = y.IdMultiplaEscolhaPai
                    }).ToList()
                }).ToList();

                db.Dispose();

                return result;

            }

        }

        public List<Usuarios> BuscarParticipantesCampoTipoPerson()
        {
            using (var db = new Entities())
            {
                var result = db.Usuarios
                    //  .Where(p => p.Ativo)
                 .AsNoTracking().ToList();

                result = result.Select(x => new Usuarios()
                {
                    Nome = x.Nome,
                    Id = x.Id,
                    Unidade = x.Unidade,
                    Email = x.Email,
                }).ToList();

                db.Dispose();

                return result;

            }

        }

        public List<AcompanhamentoTarefa> BuscarAcompanhamentosTarefaId(int Id)
        {
            using (var db = new Entities())
            {

                var result = db.AcompanhamentoTarefa
                .Where(p => p.Ativo && p.IdTarefa == Id)
                .AsNoTracking().ToList();

                result = result.Select(a => new AcompanhamentoTarefa()
                {
                    Comentario = string.IsNullOrEmpty(a.Comentario) ? string.Empty : a.Comentario,
                    DataEnvio = a.DataEnvio,
                    Status = string.IsNullOrEmpty(a.Status) ? string.Empty : a.Status,
                    Enviado = string.IsNullOrEmpty(a.Enviado) ? string.Empty : a.Enviado,
                    Id = a.Id,
                    NomeParticipanteEnvio = string.IsNullOrEmpty(a.NomeParticipanteEnvio) ? string.Empty : a.NomeParticipanteEnvio,
                    IdTarefa = a.IdTarefa,
                    Ativo = a.Ativo,
                    DataCriacao = a.DataCriacao,
                    IdParticipanteEnvio = a.IdParticipanteEnvio,
                    TarefaPA = null,
                    Usuarios = null
                }).ToList();

                db.Dispose();

                return result;

            }
        }

        public Usuarios BuscarParticipantePorId(int id, bool senha = false)
        {
            using (var db = new Entities())
            {

                var result = db.Usuarios
                 .Where(p => p.Id == id)
                 .AsNoTracking().FirstOrDefault();

                result = new Usuarios()
                {
                    Email = result.Email,
                    Nome = result.Nome,
                    Id = result.Id,
                    Unidade = result.Unidade,
                    DataAlteracao = result.DataAlteracao,
                    Senha = senha ? result.Senha : string.Empty
                };

                db.Dispose();

                return result;
            }
        }

        public ConfiguracaoEmailPA BuscarConfiguracaoEmail()
        {
            using (var db = new Entities())
            {

                var result = db.ConfiguracaoEmailPA
                 .Where(p => p.Ativo)
                 .AsNoTracking().FirstOrDefault();

                result = new ConfiguracaoEmailPA()
                {
                    Ativo = result.Ativo,
                    DataAlteracao = result.DataAlteracao,
                    DataCriacao = result.DataCriacao,
                    Email = result.Email,
                    Host = result.Host,
                    Id = result.Id,
                    Port = result.Port,
                    Senha = result.Senha
                };

                db.Dispose();

                return result;
            }
        }

        public List<GrupoCabecalho> BuscarListaGrupoCabecalhoProjetoPorId(int id)
        {
            using (var db = new Entities())
            {
                var result = db.GrupoCabecalho
                 .Where(p => p.Ativo && p.IdProjeto == id)
                 .AsNoTracking().ToList();

                result = result.Select(y => new GrupoCabecalho()
                {
                    Ativo = y.Ativo,
                    Id = y.Id,
                    DataCriacao = y.DataCriacao,
                    IdGrupoCabecalhoPai = y.IdGrupoCabecalhoPai == null ? 0 : y.IdGrupoCabecalhoPai,
                    IdProjeto = y.IdProjeto == null ? 0 : y.IdProjeto,
                    Nome = y.Nome
                }).ToList();

                db.Dispose();

                return result;

            }
        }

        public List<NumeroMesesAcoes> BuscarNroAcoesEstabelecidas()
        {
            using (var db = new Dominio.SgqDbDevEntities())
            {
                List<NumeroMesesAcoes> items = new List<NumeroMesesAcoes>();

                db.Database.Connection.Open();

                var cmd = db.Database.Connection.CreateCommand();

                //cmd.CommandText = "SELECT count(FORMAT(convert(date, b.valor, 103), 'yyyy-MM')) qtde \n" +
                //                  "        ,FORMAT(convert(date, b.valor, 103), 'yyyy-MM') data      \n" +
                //                  "   FROM TarefaPA a,                                               \n" +
                //                  "        VinculoCampoTarefa b                                      \n" +
                //                  "  WHERE b.IdTarefa = a.Id                                         \n" +
                //                  "    AND a.Ativo = 1                                               \n" +
                //                  "    AND b.IdCampo = 28--status                                    \n" +
                //                  "   GROUP BY FORMAT(convert(date, b.valor, 103), 'yyyy-MM')";

                cmd.CommandText = "SELECT                                                                                                     \n " +
                                    "count(FORMAT(convert(date, b.valor, 103), 'yyyy-MM')) qtde                                                 \n " +
                                    ",FORMAT(convert(date, b.valor, 103), 'yyyy-MM') data                                                       \n " +
                                    "FROM TarefaPA a,                                                                                           \n " +
                                    "VinculoCampoTarefa b                                                                                       \n " +
                                    "WHERE b.IdTarefa = a.Id                                                                                    \n " +
                                    "AND a.Ativo = 1                                                                                            \n " +
                                    "AND b.IdCampo = 28                                                                                         \n " +
                                    "AND b.IdTarefa not in (                                                                                    \n " +
                                        "select                                                                                                 \n " +
                                        "SS.idTarefa                                                                                            \n " +
                                        "from(                                                                                                  \n " +
                                            "Select                                                                                             \n " +
                                            "*                                                                                                  \n " +
                                            ", CASE                                                                                             \n " +
                                            "WHEN S.StatusAtual = 'Cancelado' then 1                                                            \n " +
                                            "WHEN S.StatusAtual = 'Concluido (Prazo)' then 1                                                    \n " +
                                            "WHEN S.StatusAtual = 'Replanejado' then 1                                                          \n " +
                                            "WHEN S.StatusAtual = 'Concluido (Atrasado)' then 1                                                 \n " +
                                            "ELSE 0                                                                                             \n " +
                                            "END AS Concluido                                                                                   \n " +
                                            ", FORMAT(convert(date, S.DataEnvio, 103), 'yyyy-MM') data                                          \n " +
                                            "from                                                                                               \n " +
                                            "(                                                                                                  \n " +
                                                "SELECT                                                                                         \n " +
                                                "DataEnvio                                                                                      \n " +
                                                ", idTarefa                                                                                     \n " +
                                                ", SUBSTRING(STATUS, LEN(left(Status, CHARINDEX('para ', Status) + 5)), 50) as StatusAtual      \n " +
                                                "from AcompanhamentoTarefa                                                                      \n " +
                                                "where id in(                                                                                   \n " +
                                                "SELECT max(Id) as id                                                                           \n " +
                                                "FROM [AcompanhamentoTarefa]                                             \n " +
                                                "group by idTarefa                                                                              \n " +
                                                ")                                                                                              \n " +
                                            ") as S                                                                                             \n " +
                                        ") as SS                                                                                                \n " +
                                        "where StatusAtual = 'Cancelado'                                                                        \n " +
                                    ")                                                                                                          \n " +
                                    "GROUP BY FORMAT(convert(date, b.valor, 103), 'yyyy-MM')";

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    NumeroMesesAcoes empData = new NumeroMesesAcoes { Quantidade = reader[0] == System.DBNull.Value ? 0 : Convert.ToInt32(reader[0].ToString()), Mes = reader[1].ToString() };
                    items.Add(empData);
                }

                db.Dispose();

                return items;
            }
        }

        public List<NumeroMesesAcoes> BuscarNroAcoesEstabelecidasConcluidas()
        {
            using (var db = new Dominio.SgqDbDevEntities())
            {
                List<NumeroMesesAcoes> items = new List<NumeroMesesAcoes>();

                db.Database.Connection.Open();

                var cmd = db.Database.Connection.CreateCommand();

                //cmd.CommandText = "SELECT count(FORMAT(convert(date, b.valor, 103), 'yyyy-MM')) qtde                                                 \n" +
                //                  "     ,FORMAT(convert(date, b.valor, 103), 'yyyy-MM') data                                                         \n" +
                //                  " FROM TarefaPA a,                                                                                                 \n" +
                //                  "      VinculoCampoTarefa b                                                                                        \n" +
                //                  "WHERE b.IdTarefa = a.Id                                                                                           \n" +
                //                  "  AND a.Ativo = 1                                                                                                 \n" +
                //                  "  AND b.IdCampo = 30--status                                                                                      \n" +
                //                  "  AND(SELECT x.valor FROM VinculoCampoTarefa x WHERE x.IdTarefa = a.Id AND x.IdMultiplaEscolha = 16) IS not NULL  \n" +
                //                  " GROUP BY FORMAT(convert(date, b.valor, 103), 'yyyy-MM')";

                cmd.CommandText = "select                                                                                                 \n" +
                                    "count(1) as qtde                                                                                       \n" +
                                    ",data                                                                                                  \n" +
                                    "from(                                                                                                  \n" +
                                        "Select                                                                                             \n" +
                                        "*                                                                                                  \n" +
                                        ", CASE                                                                                             \n" +
                                        "WHEN S.StatusAtual = 'Cancelado' then 0                                                            \n" +
                                        "WHEN S.StatusAtual = 'Concluido (Prazo)' then 1                                                    \n" +
                                        "WHEN S.StatusAtual = 'Replanejado' then 0                                                          \n" +
                                        "WHEN S.StatusAtual = 'Concluido (Atrasado)' then 1                                                 \n" +
                                        "ELSE 0                                                                                             \n" +
                                        "END AS Concluido                                                                                   \n" +
                                        ", FORMAT(convert(date, S.DataEnvio, 103), 'yyyy-MM') data                                          \n" +
                                        "from                                                                                               \n" +
                                        "(                                                                                                  \n" +
                                            "SELECT                                                                                         \n" +
                                            "DataEnvio                                                                                      \n" +
                                            ", idTarefa                                                                                     \n" +
                                            ", SUBSTRING(STATUS, LEN(left(Status, CHARINDEX('para ', Status) + 5)), 50) as StatusAtual      \n" +
                                            "from AcompanhamentoTarefa                                                                      \n" +
                                            "where id in(                                                                                   \n" +
                                            "SELECT max(Id) as id                                                                           \n" +
                                            "FROM [AcompanhamentoTarefa]                                             \n" +
                                            "group by idTarefa                                                                              \n" +
                                            ")                                                                                              \n" +
                                        ") as S                                                                                             \n" +
                                    ") as SS                                                                                                \n" +
                                    "where Concluido = 1                                                                                    \n" +
                                    "group by data";


                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    NumeroMesesAcoes empData = new NumeroMesesAcoes { Quantidade = reader[0] == System.DBNull.Value ? 0 : Convert.ToInt32(reader[0].ToString()), Mes = reader[1].ToString() };
                    items.Add(empData);
                }

                db.Dispose();

                return items;
            }
        }





        public List<TarefaPA> BuscarTarefaPA()
        {
            using (var db = new Dominio.SgqDbDevEntities())
            {
                List<TarefaPA> items = new List<TarefaPA>();

                db.Database.Connection.Open();

                var cmd = db.Database.Connection.CreateCommand();

                cmd.CommandText = "SELECT a.Id                   \n" +
                                  "      ,a.IdProjeto            \n" +
                                  "      ,a.DataCriacao          \n" +
                                  "      ,a.Ativo                \n" +
                                  "      ,a.IdParticipanteCriador\n" +
                                  " FROM TarefaPA a";

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    TarefaPA empData = new TarefaPA
                    {
                        Id = reader[0] == System.DBNull.Value ? 0 : Convert.ToInt32(reader[0].ToString()),
                        IdProjeto = reader[1] == System.DBNull.Value ? 0 : Convert.ToInt32(reader[1].ToString()),
                        DataCriacao = reader[2] == System.DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader[2].ToString()),
                        Ativo = reader[3] == System.DBNull.Value ? false : Convert.ToBoolean(reader[3].ToString()),
                        IdParticipanteCriador = reader[4] == System.DBNull.Value ? null : (int?)Convert.ToInt32(reader[4].ToString())
                    };
                    items.Add(empData);
                }

                db.Dispose();

                return items;
            }
        }

        public List<VinculoCampoTarefa> BuscarVinculoCampoTarefa()
        {
            using (var db = new Dominio.SgqDbDevEntities())
            {
                List<VinculoCampoTarefa> items = new List<VinculoCampoTarefa>();

                db.Database.Connection.Open();

                var cmd = db.Database.Connection.CreateCommand();

                cmd.CommandText = "SELECT Id,                \n" +
                                  "       IdMultiplaEscolha, \n" +
                                  "       IdCampo,           \n" +
                                  "       IdTarefa,          \n" +
                                  "       Valor,             \n" +
                                  "       IdParticipante     \n" +
                                  "  FROM VinculoCampoTarefa";

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    VinculoCampoTarefa empData = new VinculoCampoTarefa
                    {
                        Id = reader[0] == System.DBNull.Value ? 0 : Convert.ToInt32(reader[0].ToString()),
                        IdMultiplaEscolha = reader[1] == System.DBNull.Value ? null : (int?)Convert.ToInt32(reader[1].ToString()),
                        IdCampo = reader[2] == System.DBNull.Value ? 0 : Convert.ToInt32(reader[2].ToString()),
                        IdTarefa = reader[3] == System.DBNull.Value ? 0 : Convert.ToInt32(reader[3].ToString()),
                        Valor = reader[4].ToString(),
                        IdParticipante = reader[5] == System.DBNull.Value ? 0 : Convert.ToInt32(reader[5].ToString())
                    };
                    items.Add(empData);
                }

                db.Dispose();

                return items;
            }
        }

        public List<Campo> BuscarVinculoCampo()
        {
            using (var db = new Dominio.SgqDbDevEntities())
            {
                List<Campo> items = new List<Campo>();

                db.Database.Connection.Open();

                var cmd = db.Database.Connection.CreateCommand();

                cmd.CommandText = "SELECT Id,              \n" +
                                  "       Nome,            \n" +
                                  "       Tipo,            \n" +
                                  "       Agrupador,       \n" +
                                  "       Sequencia,       \n" +
                                  "       Obrigatorio,     \n" +
                                  "       Ativo,           \n" +
                                  "       IdProjeto,       \n" +
                                  "       Predefinido,     \n" +
                                  "       Modificavel,     \n" +
                                  "       DataCriacao,     \n" +
                                  "       DataAlteracao,   \n" +
                                  "       IdCampoPai,      \n" +
                                  "       FixadoEsquerda,  \n" +
                                  "       ExibirTabela,    \n" +
                                  "       Cabecalho,       \n" +
                                  "       IdGrupoCabecalho \n" +
                                  "  FROM Campo";

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Campo empData = new Campo
                    {
                        Id = reader[0] == System.DBNull.Value ? 0 : Convert.ToInt32(reader[0].ToString()),
                        Nome = reader[1].ToString(),
                        Tipo = reader[2].ToString(),
                        Agrupador = reader[3] == System.DBNull.Value ? false : Convert.ToBoolean(reader[3].ToString()),
                        Sequencia = reader[4] == System.DBNull.Value ? null : (int?)Convert.ToInt32(reader[4].ToString()),
                        Obrigatorio = reader[5] == System.DBNull.Value ? false : Convert.ToBoolean(reader[5].ToString()),
                        Ativo = reader[6] == System.DBNull.Value ? false : Convert.ToBoolean(reader[6].ToString()),
                        IdProjeto = reader[7] == System.DBNull.Value ? 0 : Convert.ToInt32(reader[7].ToString()),
                        Predefinido = reader[8] == System.DBNull.Value ? false : Convert.ToBoolean(reader[8].ToString()),
                        Modificavel = reader[9] == System.DBNull.Value ? false : Convert.ToBoolean(reader[9].ToString()),
                        DataCriacao = reader[10] == System.DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader[10].ToString()),
                        DataAlteracao = reader[11] == System.DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader[11].ToString()),
                        IdCampoPai = reader[12] == System.DBNull.Value ? 0 : Convert.ToInt32(reader[12].ToString()),
                        FixadoEsquerda = reader[13] == System.DBNull.Value ? null : (bool?)Convert.ToBoolean(reader[13].ToString()),
                        ExibirTabela = reader[14] == System.DBNull.Value ? null : (bool?)Convert.ToBoolean(reader[14].ToString()),
                        Cabecalho = reader[15] == System.DBNull.Value ? null : (bool?)Convert.ToBoolean(reader[15].ToString()),
                        IdGrupoCabecalho = reader[16] == System.DBNull.Value ? 0 : Convert.ToInt32(reader[16].ToString())
                    };
                    items.Add(empData);
                }

                db.Dispose();

                return items;
            }
        }

        public List<MultiplaEscolha> BuscarMultiplaEscolha()
        {
            using (var db = new Dominio.SgqDbDevEntities())
            {
                List<MultiplaEscolha> items = new List<MultiplaEscolha>();

                db.Database.Connection.Open();

                var cmd = db.Database.Connection.CreateCommand();

                cmd.CommandText = "SELECT Id,                   \n" +
                                  "       IdCampo,              \n" +
                                  "       Nome,                 \n" +
                                  "       IdTabelaExterna,      \n" +
                                  "       Cor,                  \n" +
                                  "       NomeTabelaExterna,    \n" +
                                  "       DataCriacao,          \n" +
                                  "       DataAlteracao,        \n" +
                                  "       Ativo,                \n" +
                                  "       IdMultiplaEscolhaPai  \n" +
                                  "   FROM MultiplaEscolha";

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    MultiplaEscolha empData = new MultiplaEscolha
                    {
                        Id = reader[0] == System.DBNull.Value ? 0 : Convert.ToInt32(reader[0].ToString()),
                        IdCampo = reader[1] == System.DBNull.Value ? 0 : Convert.ToInt32(reader[1].ToString()),
                        Nome = reader[2].ToString(),
                        IdTabelaExterna = reader[3] == System.DBNull.Value ? null : (int?)Convert.ToInt32(reader[3].ToString()),
                        Cor = reader[4].ToString(),
                        NomeTabelaExterna = reader[5].ToString(),
                        DataCriacao = reader[6] == System.DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader[6].ToString()),
                        DataAlteracao = reader[7] == System.DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader[7].ToString()),
                        Ativo = reader[8] == System.DBNull.Value ? false : Convert.ToBoolean(reader[8].ToString()),
                        IdMultiplaEscolhaPai = reader[9] == System.DBNull.Value ? null : (int?)Convert.ToInt32(reader[9].ToString())
                    };
                    items.Add(empData);
                }

                db.Dispose();

                return items;
            }
        }

        public List<TarefaPA> BuscarTodasTarefasNOVO()
        {
            List<TarefaPA> result = new List<TarefaPA>();
            result = BuscarTarefaPA();


            return result;

        }

        #endregion

        #region MOCK

        public List<ResultCabecalhoMock> BuscarCabecalhoMOCK(string query)
        {
            using (var db = new Dominio.SgqDbDevEntities())
            {
                List<ResultCabecalhoMock> items = new List<ResultCabecalhoMock>();

                db.Database.Connection.Open();

                var cmd = db.Database.Connection.CreateCommand();

                cmd.CommandText = query;

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {


                    List<string> dict = new List<string>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {

                        var _temp = reader[i];

                        if (_temp.GetType() == typeof(string))
                        {
                            _temp = _temp.ToString().TrimEnd();
                        }

                        items.Add(new ResultCabecalhoMock()
                        {
                            NomeCampo = reader.GetName(i).ToString(),
                            Valor = _temp.ToString()
                        });
                    }
                }

                db.Dispose();

                return items;
            }
        }

        public MultiplaEscolha SalvarAddCampoDropDown(MultiplaEscolha mult)
        {
            using (var db = new Entities())
            {
                db.MultiplaEscolha.Add(mult);

                if (mult.Id != 0)
                {
                    db.Entry(mult).State = EntityState.Modified;
                }
                else
                {
                    db.Entry(mult).State = EntityState.Added;
                }
                db.SaveChanges();

                db.Dispose();

                return mult;
            }
        }


        public Campo BuscarCabecalhoPorId(int id)
        {
            using (var db = new Entities())
            {
                var result = db.Cabecalho
                .Where(p => p.Ativo && p.Id == id)
                .AsNoTracking().FirstOrDefault();

                result = new Campo()
                {
                    Ativo = result.Ativo,
                    Id = result.Id,
                    DataCriacao = result.DataCriacao,
                    IdProjeto = result.IdProjeto == null ? 0 : result.IdProjeto,
                    IdParticipanteCriador = result.IdParticipanteCriador == null ? 0 : result.IdParticipanteCriador,
                    VinculoCampoCabecalho = result.VinculoCampoCabecalho.Select(z => new VinculoCampoCabecalho()
                    {
                        Id = z.Id,
                        IdCabecalho = z.IdCabecalho,
                        IdCampo = z.IdCampo,
                        IdGrupoCabecalho = z.IdGrupoCabecalho,
                        IdMultiplaEscolha = z.IdMultiplaEscolha,
                        IdParticipante = z.IdParticipante,
                        Valor = z.Valor,
                        Campo = new Campo()
                        {
                            Nome = z.Campo.Nome,
                            Tipo = z.Campo.Tipo,
                        }
                    }).ToList(),
                };
                db.Dispose();

                return result;
            }
        }

        public List<Campo> BuscarTodosCabecalhos()
        {
            using (var db = new Entities())
            {
                var result = db.Cabecalho
                .Where(p => p.Ativo)
                .AsNoTracking().ToList();

                result = result.Select(y => new Campo()
                {
                    Ativo = y.Ativo,
                    Id = y.Id,
                    DataCriacao = y.DataCriacao,
                    IdProjeto = y.IdProjeto == null ? 0 : y.IdProjeto,
                    IdParticipanteCriador = y.IdParticipanteCriador == null ? 0 : y.IdParticipanteCriador,
                    VinculoCampoCabecalho = y.VinculoCampoCabecalho.Select(z => new VinculoCampoCabecalho()
                    {
                        Id = z.Id,
                        IdCabecalho = z.IdCabecalho,
                        IdCampo = z.IdCampo,
                        IdGrupoCabecalho = z.IdGrupoCabecalho,
                        IdMultiplaEscolha = z.IdMultiplaEscolha,
                        IdParticipante = z.IdParticipante,
                        Valor = z.Valor,
                        Campo = new Campo()
                        {
                            Nome = z.Campo.Nome
                        }
                    }).ToList(),
                }).ToList();

                db.Dispose();

                return result;
            }
        }

        public List<ResultCabecalhoMock> BuscarCabecalhoFiltradoMOCK(string query)
        {
            using (var db = new Dominio.SgqDbDevEntities())
            {
                List<ResultCabecalhoMock> items = new List<ResultCabecalhoMock>();

                db.Database.Connection.Open();

                var cmd = db.Database.Connection.CreateCommand();

                cmd.CommandText = query;

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {


                    List<string> dict = new List<string>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {

                        var _temp = reader[i];

                        if (_temp.GetType() == typeof(string))
                        {
                            _temp = _temp.ToString().TrimEnd();
                        }

                        items.Add(new ResultCabecalhoMock()
                        {
                            NomeCampo = reader.GetName(i).ToString(),
                            Valor = _temp.ToString()
                        });
                    }
                }

                db.Dispose();

                return items;
            }
        }


        #endregion


        #region Relatorios

        public List<VinculoCampoTarefa> PopularGraficosComFiltroBusca(int idOque, int filtro, List<string> valoresFiltro)
        {
            using (var db = new Entities())
            {
                var list = new List<VinculoCampoTarefa>();

                if (valoresFiltro != null)
                {
                    list = db.VinculoCampoTarefa
                        .Where(p => p.IdCampo == idOque || (p.IdCampo == filtro && valoresFiltro.Contains(p.Valor)))
                        .ToList();
                }
                else
                {
                    list = db.VinculoCampoTarefa
                      .Where(p => p.IdCampo == idOque || p.IdCampo == filtro)
                      .ToList();
                }

                foreach (var i in list)
                {
                    if (i.IdCampo == 34 && i.Valor == "Ativo")
                    {
                        var quando = db.VinculoCampoTarefa
                            .Where(p => p.IdTarefa == i.IdTarefa && p.IdCampo == 14)
                            .FirstOrDefault();

                        if (quando == null)
                            continue;

                        var teste = DateTime.Now;

                        DateTime.TryParseExact(quando.Valor, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out teste);

                        if (teste < DateTime.Now)
                        {
                            i.Valor = "Ativo (Atrasado)";
                        }
                    }

                }

                list = list.Select(vinc => new VinculoCampoTarefa()
                {
                    Valor = vinc.Valor,
                    Id = vinc.Id,
                    IdMultiplaEscolha = vinc.IdMultiplaEscolha,
                    IdCampo = vinc.IdCampo,
                    IdParticipante = vinc.IdParticipante,
                    IdTarefa = vinc.IdTarefa,
                    Campo = new Campo()
                    {
                        Nome = vinc.Campo.Nome
                    }


                }).ToList();

                db.Dispose();

                return list;
            }
        }

        public List<TarefaPA> BuscarQuantidadeTodosStatus()
        {
            using (var db = new Entities())
            {

                var list = db.TarefaPA
                  .Where(p => p.Ativo)
                  .ToList();

                list = list.Select(tar => new TarefaPA()
                {
                    Id = tar.Id,
                    IdProjeto = tar.IdProjeto,
                    DataCriacao = tar.DataCriacao,
                    Ativo = tar.Ativo,
                    IdParticipanteCriador = tar.IdParticipanteCriador,
                    VinculoCampoTarefa = tar.VinculoCampoTarefa.Select(vinc => new VinculoCampoTarefa()
                    {
                        Valor = vinc.Valor,
                        Id = vinc.Id,
                        IdMultiplaEscolha = vinc.IdMultiplaEscolha,
                        IdCampo = vinc.IdCampo,
                        IdParticipante = vinc.IdParticipante,
                        IdTarefa = vinc.IdTarefa,
                        Campo = vinc.Campo == null ? null : !vinc.Campo.Ativo ? null : new Campo()
                        {
                            Ativo = vinc.Campo.Ativo,
                            DataCriacao = vinc.Campo.DataCriacao,
                            IdProjeto = vinc.Campo.IdProjeto,
                            Modificavel = vinc.Campo.Modificavel,
                            Agrupador = vinc.Campo.Agrupador,
                            Id = vinc.Campo.Id,
                            Nome = vinc.Campo.Nome,
                            Obrigatorio = vinc.Campo.Obrigatorio,
                            Predefinido = vinc.Campo.Predefinido,
                            Sequencia = vinc.Campo.Sequencia != null ? vinc.Campo.Sequencia.Value : 0,
                            Tipo = vinc.Campo.Tipo,
                            DataAlteracao = vinc.Campo.DataAlteracao,
                            IdCampoPai = vinc.Campo.IdCampoPai == null ? 0 : vinc.Campo.IdCampoPai,
                            FixadoEsquerda = vinc.Campo.FixadoEsquerda == null ? false : vinc.Campo.FixadoEsquerda,
                            ExibirTabela = vinc.Campo.ExibirTabela == null ? false : vinc.Campo.ExibirTabela,
                        },
                        MultiplaEscolha = vinc.MultiplaEscolha == null ? null : !vinc.MultiplaEscolha.Ativo ? null : new MultiplaEscolha()
                        {
                            Ativo = vinc.MultiplaEscolha.Ativo,
                            Cor = vinc.MultiplaEscolha.Cor,
                            DataCriacao = vinc.MultiplaEscolha.DataCriacao,
                            Id = vinc.MultiplaEscolha.Id,
                            IdCampo = vinc.MultiplaEscolha.IdCampo,
                            Nome = vinc.MultiplaEscolha.Nome,
                            DataAlteracao = vinc.MultiplaEscolha.DataAlteracao,
                            IdTabelaExterna = vinc.MultiplaEscolha.IdTabelaExterna,
                            NomeTabelaExterna = vinc.MultiplaEscolha.NomeTabelaExterna,
                            IdMultiplaEscolhaPai = vinc.MultiplaEscolha.IdMultiplaEscolhaPai
                        },
                        Usuarios = vinc.Usuarios == null ? null : new Usuarios()
                        {
                            Email = vinc.Usuarios.Email,
                            Nome = vinc.Usuarios.Nome,
                            Id = vinc.Usuarios.Id,
                            Unidade = vinc.Usuarios.Unidade,
                        }
                    }).ToList()
                }).ToList();

                db.Dispose();

                return list;
            }
        }

        public List<SelectListItem> PopularDropDownValorFiltro(int id)
        {
            var selectItem = new List<SelectListItem>();

            using (var db = new Entities())
            {

                var list = db.VinculoCampoTarefa
                  .Where(p => p.IdCampo == id)
                  .ToList();

                list = list
                    .GroupBy(p => p.Valor)
                    .Select(g => g.First())
                    .ToList();

                selectItem = list.Select(vinc => new SelectListItem()
                {
                    Text = vinc.Valor,
                    Value = vinc.Valor
                }).ToList();

                db.Dispose();

                return selectItem;
            }

        }



        #endregion







    }
}
