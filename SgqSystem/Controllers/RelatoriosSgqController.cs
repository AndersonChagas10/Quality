using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.DTO.Params;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class RelatoriosSgqController : BaseController
    {
        #region Constructor

        private FormularioParaRelatorioViewModel form;
        private readonly IRelatorioColetaDomain _relatorioColetaDomain;
        private readonly IUserDomain _userDomain;
        private readonly IBaseDomain<UserSgq, UserDTO> _user;
        private readonly IBaseDomain<Level01, Level01DTO> _level01;
        private readonly IBaseDomain<Level02, Level02DTO> _level02;
        private readonly IBaseDomain<Level03, Level03DTO> _level03;
        private readonly IBaseDomain<Shift, ShiftDTO> _shift;
        private readonly IBaseDomain<Period, PeriodDTO> _period;
        private readonly IBaseDomain<ParCompany, ParCompanyDTO> _unit;

        public RelatoriosSgqController(IRelatorioColetaDomain relatorioColetaDomain
            , IUserDomain userDomain
            , IBaseDomain<UserSgq, UserDTO> user
            , IBaseDomain<Level01, Level01DTO> level01
            , IBaseDomain<Level02, Level02DTO> level02
            , IBaseDomain<Level03, Level03DTO> level03
            , IBaseDomain<Shift, ShiftDTO> shift
            , IBaseDomain<Period, PeriodDTO> period
            , IBaseDomain<ParCompany, ParCompanyDTO> unit
            )
        {
            _unit = unit;
            _userDomain = userDomain;
            _level01 = level01;
            _level02 = level02;
            _level03 = level03;
            _shift = shift;
            _period = period;
            _user = user;
            _relatorioColetaDomain = relatorioColetaDomain;

            form = new FormularioParaRelatorioViewModel();
            form.SetLevel01SelectList(_level01.GetAllNoLazyLoad());
            form.Setlevel02SelectList(_level02.GetAllNoLazyLoad());
            form.SetLevel03SelectList(_level03.GetAllNoLazyLoad());
            form.SetUserSelectList(_user.GetAllNoLazyLoad());
            form.SetShiftSelectList(/*_shift.GetAll()*/);
            form.SetPeriodSelectList(_period.GetAllNoLazyLoad());
            form.SetUnitsSelectList(_unit.GetAllNoLazyLoad());
            form.SetUserSelectList(_user.GetAllNoLazyLoad());
        }

        #endregion

        public ActionResult Scorecard()
        {
            return View(form);
        }

        public JsonResult GetOperacoesPorUnidadeConsolidado(List<int> idUnidade, string dataInicio, string dataFim)
        {
            //if (idUnidade.Count > 0)
            //{
            //    if (idUnidade[0] == 0)
            //    {
            //        var unidadesUsuario = GetUnidadesUsuarioHS();
            //        idUnidade = unidadesUsuario;
            //    }
            //}

            string[][] final = new string[0][];
            using (var db = new SGQ_GlobalEntities())
            {
                #region variaveis

                dataInicio = DateTime.ParseExact(dataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                dataFim = DateTime.ParseExact(dataFim, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");

                //List<GetUnidades_Result> unidades = new List<GetUnidades_Result>();
                double totalNc;
                double totalAv;
                double totalPeso;
                double totalPontos;

                double _real = 0;
                double _meta = 0;
                double _scorecard = 0;

                string _tipoIndicador = string.Empty;
                string _indicador = string.Empty;
                string _criterio = string.Empty;

                string _cluster = "";
                string _regional = "";
                string _empresa = "";

                double totalScorecardPeso = 0;
                double totalScorecardPontos = 0;
                double totalScorecard = 0;

                var tarefas = new List<sp_ChartTarefasNC_Result>();
                List<GetOperacoes_Result> operacoes = new List<GetOperacoes_Result>();
                List<GetScoreCardConsolidado_Result> score = new List<GetScoreCardConsolidado_Result>();

                #endregion

                db.Database.CommandTimeout = 360;
                operacoes = db.GetOperacoes().ToList();
                if (idUnidade.Count == 1)
                {
                    score = db.GetScoreCardConsolidado(dataInicio, dataFim, idUnidade.First()).Where(x => idUnidade.Any(id => id == x.Empresaid)).ToList();
                }
                else
                {
                    score = db.GetScoreCardConsolidado(dataInicio, dataFim, null).Where(x => idUnidade.Any(id => id == x.Empresaid)).ToList();
                }

                //  var listaPontos = db.Pontos.ToList();

                #region Odernar por Critico, Prioritario e controle

                List<GetOperacoes_Result> operacoesCritico = operacoes.Where(x => x.Nivel.Equals("Crítico")).ToList();

                List<GetOperacoes_Result> operacoesPrioritário = operacoes.Where(x => x.Nivel.Equals("Prioritário")).ToList();

                List<GetOperacoes_Result> operacoesControle = operacoes.Where(x => x.Nivel.Equals("Controle")).ToList();

                operacoes.Clear();

                operacoes.AddRange(operacoesCritico);

                operacoes.AddRange(operacoesPrioritário);

                operacoes.AddRange(operacoesControle);

                #endregion


                foreach (GetScoreCardConsolidado_Result s in score)
                {

                    totalNc = 0;
                    totalAv = 0;
                    totalPeso = 0;
                    totalPontos = 0;
                    _real = 0;
                    _meta = 0;
                    _scorecard = 0;
                    _tipoIndicador = string.Empty;
                    _indicador = string.Empty;
                    _criterio = string.Empty;

                    foreach (GetOperacoes_Result o in operacoes)
                    {
                        if (s.OperacaoId == o.Id)
                        {
                            if (idUnidade.Count == 1)
                            {
                                if (string.IsNullOrEmpty(s.Empresa))
                                {
                                    _empresa = "-1";
                                }
                                else
                                {
                                    _empresa = s.Empresa;
                                }
                                _cluster = s.Cluster;
                                _regional = s.Regional;


                            }
                            else
                            {
                                _empresa = "-1";
                            }


                            if (s.TotalForaPadrao != null)
                                totalNc += Convert.ToDouble(s.TotalForaPadrao.Value);
                            if (s.TotalAvaliado != null)
                                totalAv += Convert.ToDouble(s.TotalAvaliado.Value);
                            if (s.Peso != null)
                            {
                                totalPeso += Convert.ToDouble(s.Peso.Value);
                                totalScorecardPeso += Convert.ToDouble(s.Peso.Value);
                            }
                            if (s.Pontos != null)
                            {
                                totalPontos += Convert.ToDouble(s.Pontos.Value);
                                totalScorecardPontos += Convert.ToDouble(s.Pontos.Value);
                            }

                            if (s.Real != null)
                            {
                                _real = Convert.ToDouble(s.Real.Value);
                            }

                            _meta = Convert.ToDouble(s.Meta);




                            //_tipoIndicador = o.Nivel;
                            // _tipoIndicador = listaPontos.Where(x => x.Operacao == s.OperacaoId && x.Clusters.Legenda.Equals(s.Cluster)).Select(x => x.Pontos1).FirstOrDefault().ToString();
                            _tipoIndicador = s.Nivel;
                            _indicador = o.Nome;
                            _criterio = o.Criterio;


                            if (totalAv > 0)
                            {
                                var resultReal = (totalNc / totalAv) * 100;
                                _real = resultReal > 100 ? 100 : resultReal;
                            }
                            else if (_real == 0)
                            {
                                _real = 0;
                            }


                            if (_criterio.Equals("Menor"))
                            {
                                if (_real == 0)
                                {
                                    _scorecard = 100;
                                }
                                else
                                {
                                    if (_real > _meta)
                                    {
                                        _scorecard = (_meta / _real) * 100;
                                    }
                                    else
                                    {
                                        _scorecard = 100;
                                    }
                                }
                            }
                            else
                            {
                                if (_real == 0)
                                {
                                    _scorecard = 0;
                                }
                                else
                                {
                                    if (_real > _meta)
                                    {
                                        _scorecard = 100;
                                    }
                                    else
                                    {
                                        _scorecard = (_real / _meta) * 100;
                                    }
                                }

                            }

                            string[] temp = {
                                    _cluster.ToString(),
                                    _regional.ToString(),
                                    _empresa.ToString(),
                                    _tipoIndicador.ToString(),
                                    _indicador.ToString(),
                                    string.Format("{0:0.##}",totalAv).Replace(",","."),
                                    string.Format("{0:0.##}",totalNc).Replace(",","."),
                                    _criterio.ToString(),
                                    string.Format("{0:0.##}",totalPeso).Replace(",","."),
                                    string.Format("{0:0.##}",_meta).Replace(",","."),
                                    string.Format("{0:0.##}",_real).Replace(",","."),
                                    string.Format("{0:0.##}",totalPontos).Replace(",","."),
                                    string.Format("{0:0.##}",_scorecard).Replace(",","."),
                                    "0",
                                    "0",
                                    "0",
                                };

                            Array.Resize(ref final, final.Length + 1);
                            final[final.Length - 1] = temp;

                        }
                    }
                }

                totalScorecard = (totalScorecardPeso == 0 ? 0 : (totalScorecardPontos / totalScorecardPeso) * 100);

                foreach (string[] s in final)
                {
                    s[13] = string.Format("{0:0.##}", totalScorecardPeso).Replace(",", ".");
                    s[14] = string.Format("{0:0.##}", totalScorecardPontos).Replace(",", ".");
                    s[15] = string.Format("{0:0.##}", totalScorecard).Replace(",", ".");
                }

            }
           


            return Json(final, JsonRequestBehavior.AllowGet);
        }

    }
}