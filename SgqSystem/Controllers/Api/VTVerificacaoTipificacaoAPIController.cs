using Dominio;
using SgqSystem.Handlres;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("api/VTVerificacaoTipificacao")]
    public class VTVerificacaoTipificacaoApiController : ApiController
    {
        [Route("Save")]
        [HttpPost]
        public void SaveVTVerificacaoTipificacao(TipificacaoViewModel model)
        {
            var _verificacao = model.VerificacaoTipificacao;

            using (var db = new SGQ_GlobalEntities())
            {
                var verificacaoTipificacao = db.VTVerificacaoTipificacao.FirstOrDefault(r => r.Chave == _verificacao.Chave);
                
                //Variavel para atribuir data correta na verificãção da tipificação
                DateTime dataHoraTipificacao = DateTime.Now;
                
                //Caso exista uma verificação da tipidicação a mesma será removida, mantando somente a data para atribuir na nova verificação que será criada
                if(verificacaoTipificacao != null)
                {
                    //Atribui a data da verificação que for encontrada
                    dataHoraTipificacao = verificacaoTipificacao.DataHora;

                    //Busca todos os resultados referente a verificação localizada
                    var resultados = db.VTVerificacaoTipificacaoResultados.Where(p => p.Chave == verificacaoTipificacao.Chave).ToList();
                    //Deleta os resultados da verificaçao
                    foreach (var r in resultados)
                    {
                        db.VTVerificacaoTipificacaoResultados.Remove(r);
                    }
                    //Deleta a Verificação
                    db.VTVerificacaoTipificacao.Remove(verificacaoTipificacao);
                }

                db.SaveChanges();

                //instanciamos um novo objeto na verificacao da tipificacao
                verificacaoTipificacao = new VTVerificacaoTipificacao();

                verificacaoTipificacao.Sequencial = _verificacao.Sequencial;
                verificacaoTipificacao.Banda = _verificacao.Banda;
                verificacaoTipificacao.DataHora = dataHoraTipificacao;
                verificacaoTipificacao.UnidadeId = _verificacao.UnidadeId;
                verificacaoTipificacao.Chave = _verificacao.Chave;
                verificacaoTipificacao.Status = false;

                //gravamos o objeto no banco
                db.VTVerificacaoTipificacao.Add(verificacaoTipificacao);

                for (var i = 0; i < model.VerificacaoTipificacaoResultados.Count; i++)
                {
                    if (model.VerificacaoTipificacaoResultados[i].CaracteristicaTipificacaoId == "null")
                    {
                        //PEGA ID DA AREA PARTICIPANTES
                       // var aa = model.VerificacaoTipificacaoResultados[i].AreasParticipantesId.GetValueOrDefault().ToString();

                        string aId = model.VerificacaoTipificacaoResultados[i].AreasParticipantesId;

                        var AreaParticObj = (from x in db.AreasParticipantes.AsNoTracking()
                                     where x.cNrCaracteristica == aId
                                     select x).FirstOrDefault();

                       
                        var numeroCaracteristica = AreaParticObj.cNrCaracteristica;

                        numeroCaracteristica = numeroCaracteristica.Substring(0, 4);

                        var codigoCaracteristica = (from x in db.AreasParticipantes.AsNoTracking()
                                                    where x.cNrCaracteristica == numeroCaracteristica
                                                    select x.nCdCaracteristica).FirstOrDefault();

                        var codigoTarefa = (from x in db.VerificacaoTipificacaoTarefaIntegracao.AsNoTracking()
                                            where x.CaracteristicaTipificacaoId == codigoCaracteristica
                                            select x.TarefaId).FirstOrDefault();

                        VTVerificacaoTipificacaoResultados verificacaoTipificacaoResultados = new VTVerificacaoTipificacaoResultados();
                        verificacaoTipificacaoResultados.AreasParticipantesId = Convert.ToInt32(model.VerificacaoTipificacaoResultados[i].AreasParticipantesId);
                        verificacaoTipificacaoResultados.TarefaId = codigoTarefa;
                        verificacaoTipificacaoResultados.Chave = _verificacao.Chave;
                       

                        db.VTVerificacaoTipificacaoResultados.Add(verificacaoTipificacaoResultados);

                    }
                    else
                    {
                        //CARACTERISTICA TIPIFICACAO

                        var idCaracteristicaTipificacaoTemp = model.VerificacaoTipificacaoResultados[i].CaracteristicaTipificacaoId;

                        var obj = (from x in db.CaracteristicaTipificacao.AsNoTracking()
                                                    where x.cNrCaracteristica == idCaracteristicaTipificacaoTemp select x).FirstOrDefault();

                        var numeroCaracteristica = obj.cNrCaracteristica;
                        numeroCaracteristica = numeroCaracteristica.Substring(0, 3);

                        var codigoCaracteristica = (from x in db.CaracteristicaTipificacao.AsNoTracking()
                                                    where x.cNrCaracteristica == numeroCaracteristica
                                                    select x.nCdCaracteristica).FirstOrDefault();

                        var codigoTarefa = (from x in db.VerificacaoTipificacaoTarefaIntegracao.AsNoTracking()
                                            where x.CaracteristicaTipificacaoId == codigoCaracteristica
                                            select x.TarefaId).FirstOrDefault();

                        VTVerificacaoTipificacaoResultados verificacaoTipificacaoResultados = new VTVerificacaoTipificacaoResultados();
                        verificacaoTipificacaoResultados.CaracteristicaTipificacaoId = Convert.ToInt32(model.VerificacaoTipificacaoResultados[i].CaracteristicaTipificacaoId);
                        verificacaoTipificacaoResultados.TarefaId = codigoTarefa;
                        verificacaoTipificacaoResultados.Chave = _verificacao.Chave;
                      


                        db.VTVerificacaoTipificacaoResultados.Add(verificacaoTipificacaoResultados);
                    }

                }

                //
                db.SaveChanges();

                //Consolidar resultados e tratamento de erro

                

            }
        }
    }
}