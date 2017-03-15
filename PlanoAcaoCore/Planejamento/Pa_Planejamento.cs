using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PlanoAcaoCore
{
    public class Pa_Planejamento : Pa_BaseObject
    {

        #region Estrategico

        [Display(Name = "Diretoria")]
        public int Diretoria_Id { get; set; }
        public string Diretoria { get; set; }

        [Display(Name = "Missão")]
        public int Missao_Id { get; set; }
        public string Missao { get; set; }

        [Display(Name = "Visão")]
        public int Visao_Id { get; set; }
        public string Visao { get; set; }

        [Display(Name = "Dimensão")]
        public int Dimensao_Id { get; set; }
        public string Dimensao { get; set; }

        [Display(Name = "Diretrizes / Objetivos")]
        public int Objetivo_Id { get; set; }
        public string Objetivo { get; set; }

        [Display(Name = "Indicadores da Diretrizes / Objetivos")]
        public int IndicadoresDiretriz_Id { get; set; }
        public string IndicadoresDiretriz { get; set; }

        [Display(Name = "Responsavel pela Diretriz")]
        public int Responsavel_Diretriz { get; set; }
        public Pa_Quem Responsavel_Diretriz_Quem
        {
            get
            {
                if (Responsavel_Diretriz > 0)
                    return Pa_Quem.Get(Responsavel_Diretriz);
                else
                    return new Pa_Quem();
            }
        }

        public int? Estrategico_Id { get; set; }

        #endregion

        #region Tático

        [Display(Name = "Gerência")]
        public int Gerencia_Id { get; set; }
        public string Gerencia { get; set; }

        [Display(Name = "Coordenação")]
        public int Coordenacao_Id { get; set; }
        public string Coordenacao { get; set; }

        [Display(Name = "Projeto / Iniciativa")]
        public int Iniciativa_Id { get; set; }
        public string Iniciativa { get; set; }

        [Display(Name = "Indicadores do Projeto / Iniciativa")]
        public int IndicadoresDeProjeto_Id { get; set; }
        public string IndicadoresDeProjeto { get; set; }

        [Display(Name = "Objetivo Gerencial")]
        public int ObjetivoGerencial_Id { get; set; }
        public string ObjetivoGerencial { get; set; }

        [Display(Name = "Valor de")]
        public decimal ValorDe { get; set; }
        public string _ValorDe { get; set; }

        [Display(Name = "Valor para")]
        public decimal ValorPara { get; set; }
        public string _ValorPara { get; set; }

        [Display(Name = "Unidade de medida")]
        public int UnidadeDeMedida_Id { get; set; }
        public Pa_UnidadeMedida _UnidadeDeMedida_Id
        {
            get
            {
                if (Responsavel_Projeto > 0)
                    return Pa_UnidadeMedida.Get(UnidadeDeMedida_Id);
                else
                    return new Pa_UnidadeMedida();
            }
        }

        [Display(Name = "Data inicio")]
        public DateTime? DataInicio { get; set; }

        [Display(Name = "_DataInicio")]
        public string _DataInicio { get; set; }

        [Display(Name = "Data fim")]
        public DateTime? DataFim { get; set; }

        [Display(Name = "_DataFim")]
        public string _DataFim { get; set; }

        [Display(Name = "Responsavel pelo Projeto / Iniciativa")]
        public int Responsavel_Projeto { get; set; }
        public Pa_Quem Responsavel_Projeto_Quem
        {
            get
            {
                if (Responsavel_Projeto > 0)
                    return Pa_Quem.Get(Responsavel_Projeto);
                else
                    return new Pa_Quem();
            }
        }

        public int? Tatico_Id { get; set; }
        #endregion

        #region Depreciado

        [Display(Name = "Tema / Assunto")]
        public int TemaAssunto_Id { get; set; }
        public string TemaAssunto { get; set; }

        #endregion

        public Pa_Acao Acao { get; set; }

        public string resumo
        {
            get
            {
                var retorno = string.Empty;
                if (Id > 0)
                {
                    retorno += "Diretoria: " + Diretoria +
                        " \n Missão: " + Missao +
                        " \n Visão" + Visao +
                        " \n Dimensão" + Dimensao +
                        " \n Diretrizes / Objetivos" + Objetivo +
                        " \n Indicadores da Diretrizes / Objetivos" + IndicadoresDiretriz +
                        " \n Responsavel pela Diretriz" + Responsavel_Diretriz_Quem.Name;

                }
                return retorno;
            }
        }

        public void IsValid()
        {
            //Name = Guard.CheckStringFullSimple(Name);
        }

        private static string query
        {
            get
            {
                return "SELECT Pl.*,                                                                         " +
                    "\n INI.Name AS Inciativa,                                                               " +
                    "\n DIR.Name AS Diretoria,                                                               " +
                    "\n GER.Name AS Gerencia,                                                                " +
                    "\n CORD.Name AS Coordenacao,                                                            " +
                    "\n MISS.Name AS Missao,                                                                 " +
                    "\n VIS.Name AS Visao,                                                                   " +
                    "\n TEM.Name AS TemaAssunto,                                                             " +
                    "\n INDIC.Name AS IndicadoresDiretriz,                                                   " +
                    "\n INDICProj.Name AS IndicadoresDeProjeto,                                              " +
                    "\n OBJT.Name AS ObjetivoGerencial,                                                      " +
                    "\n DIME.Name AS Dimensao,                                                                " +
                    "\n INICI.Name AS Iniciativa,                                                            " +
                    "\n OBJ.Name AS Objetivo                                                                " +
                    "\n  FROM Pa_planejamento Pl                                                             " +
                    "\n LEFT JOIN Pa_Iniciativa INI on INI.Id = Pl.Iniciativa_Id                             " +
                    "\n LEFT JOIN Pa_Diretoria DIR on DIR.Id = Pl.Diretoria_Id                               " +
                    "\n LEFT JOIN Pa_Gerencia GER on Pl.Gerencia_Id = GER.Id                                 " +
                    "\n LEFT JOIN Pa_Coordenacao CORD on CORD.Id = Pl.Coordenacao_Id                         " +
                    "\n LEFT JOIN Pa_Missao MISS on MISS.Id = Pl.Missao_Id                                   " +
                    "\n LEFT JOIN Pa_Visao VIS on VIS.Id = Pl.Visao_Id                                       " +
                    "\n LEFT JOIN Pa_TemaAssunto TEM on TEM.Id = Pl.TemaAssunto_Id                           " +
                    "\n LEFT JOIN Pa_IndicadoresDiretriz INDIC on INDIC.Id = Pl.IndicadoresDiretriz_Id       " +
                    "\n LEFT JOIN Pa_IndicadoresDeProjeto INDICProj on INDIC.Id = Pl.IndicadoresDeProjeto_Id " +
                    "\n LEFT JOIN Pa_Iniciativa INICI on INICI.Id = Pl.Iniciativa_Id                         " +
                    "\n LEFT JOIN Pa_ObjetivoGeral OBJT on OBJT.Id = Pl.ObjetivoGerencial_Id                 " +
                    "\n LEFT JOIN Pa_Objetivo OBJ on OBJ.Id = Pl.Objetivo_Id                                 " +
                    "\n LEFT JOIN Pa_Dimensao DIME on DIME.Id = Pl.Dimensao_Id                               ";

            }
        }
        public static List<Pa_Planejamento> Listar()
        {

            var results = ListarGenerico<Pa_Planejamento>(query);

            foreach (var i in results)
            {
                i._DataFim = i.DataFim.GetValueOrDefault().ToShortDateString() + " " + i.DataFim.GetValueOrDefault().ToShortTimeString();
                i._DataInicio = i.DataInicio.GetValueOrDefault().ToShortDateString() + " " + i.DataInicio.GetValueOrDefault().ToShortTimeString();
            }

            return results;
        }

        public static Pa_Planejamento Get(int Id)
        {
            return GetGenerico<Pa_Planejamento>(query + "WHERE Pl.Id = " + Id);
        }

        public static List<Pa_Planejamento> GetPlanejamentoAcao()
        {
            var retorno = new List<Pa_Planejamento>();
            var planejamentos = Listar();
            var acoes = Pa_Acao.Listar();

            foreach (var i in planejamentos)
            {
                var acoesTmp = acoes.Where(r => r.Panejamento_Id == i.Id);
                if (acoesTmp.Count() > 0)
                {
                    foreach (var k in acoesTmp)
                    {
                        var planTemp = new Pa_Planejamento();
                        foreach (var pt in planTemp.GetType().GetProperties())
                            try
                            {
                                pt.SetValue(planTemp, i.GetType().GetProperty(pt.Name).GetValue(i));
                            }
                            catch (Exception)
                            {
                                //throw;
                            }

                        planTemp.Acao = k;
                        retorno.Add(planTemp);
                    }
                }
                else
                {
                    i.Acao = new Pa_Acao() { CausaMedidasXAcao = new Pa_CausaMedidasXAcao() };

                    retorno.Add(i);
                }
            }

            return retorno;
        }
       
    }

}
