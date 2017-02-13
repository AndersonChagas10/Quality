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
                    resultados.RemoveAll(p => p.Chave == verificacaoTipificacao.Chave);
                    //Deleta a Verificação
                    db.VTVerificacaoTipificacao.Remove(verificacaoTipificacao);
                }

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
                    if (model.VerificacaoTipificacaoResultados[i].CaracteristicaTipificacao.cIdentificador.Equals("AREASPARTICIPANTES"))
                    {
                        //PEGA ID DA AREA PARTICIPANTES

                        var idCaracteristicaTipificacaoTemp = model.VerificacaoTipificacaoResultados[i].CaracteristicaTipificacao.nCdCaracteristica;

                        var numeroCaracteristica = (from x in db.AreasParticipantes.AsNoTracking()
                                                    where x.nCdCaracteristica == idCaracteristicaTipificacaoTemp
                                                    select x.cNrCaracteristica).FirstOrDefault();


                        numeroCaracteristica = numeroCaracteristica.Substring(0, 4);

                        var codigoCaracteristica = (from x in db.AreasParticipantes.AsNoTracking()
                                                    where x.cNrCaracteristica.Equals(numeroCaracteristica)
                                                    select x.nCdCaracteristica).FirstOrDefault();

                        var codigoTarefa = (from x in db.VerificacaoTipificacaoTarefaIntegracao.AsNoTracking()
                                            where x.CaracteristicaTipificacaoId == codigoCaracteristica
                                            select x.TarefaId).FirstOrDefault();

                        VerificacaoTipificacaoResultados verificacaoTipificacaoResultados = new VerificacaoTipificacaoResultados()
                        {
                            // VerificacaoTipificacaoId = verificacaoTipificacao.Id,
                            AreasParticipantesId = Convert.ToInt32(model.VerificacaoTipificacaoResultados[i].CaracteristicaTipificacao.nCdCaracteristica),
                            TarefaId = codigoTarefa,
                            Chave = _verificacao.Chave
                        };


                        db.VerificacaoTipificacaoResultados.Add(verificacaoTipificacaoResultados);

                    }
                    else
                    {
                        //CARACTERISTICA TIPIFICACAO

                        var idCaracteristicaTipificacaoTemp = model.VerificacaoTipificacaoResultados[i].CaracteristicaTipificacao.nCdCaracteristica;

                        var numeroCaracteristica = (from x in db.CaracteristicaTipificacao.AsNoTracking()
                                                    where x.nCdCaracteristica == idCaracteristicaTipificacaoTemp
                                                    select x.cNrCaracteristica).FirstOrDefault();


                        numeroCaracteristica = numeroCaracteristica.Substring(0, 4);

                        var codigoCaracteristica = (from x in db.CaracteristicaTipificacao.AsNoTracking()
                                                    where x.cNrCaracteristica.Equals(numeroCaracteristica)
                                                    select x.nCdCaracteristica).FirstOrDefault();

                        var codigoTarefa = (from x in db.VerificacaoTipificacaoTarefaIntegracao.AsNoTracking()
                                            where x.CaracteristicaTipificacaoId == codigoCaracteristica
                                            select x.TarefaId).FirstOrDefault();

                        VerificacaoTipificacaoResultados verificacaoTipificacaoResultados = new VerificacaoTipificacaoResultados()
                        {
                            // VerificacaoTipificacaoId = verificacaoTipificacao.Id,
                            CaracteristicaTipificacaoId = Convert.ToInt32(model.VerificacaoTipificacaoResultados[i].CaracteristicaTipificacao.nCdCaracteristica),
                            TarefaId = codigoTarefa,
                            Chave = _verificacao.Chave
                        };


                        db.VerificacaoTipificacaoResultados.Add(verificacaoTipificacaoResultados);
                    }

                }

                //
                db.SaveChanges();

                //Consolidar resultados e tratamento de erro

                

            }
        }
    }
}