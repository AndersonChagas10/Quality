using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
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

        //[Display(Name = "Diretrizes / Objetivos")]
        [Display(Name = "Diretriz")]
        public int Objetivo_Id { get; set; }
        public string Objetivo { get; set; }

        [Display(Name = "Indicadores da Diretrizes")]
        public int IndicadoresDiretriz_Id { get; set; }
        public string IndicadoresDiretriz { get; set; }

        [Display(Name = "Responsável pela Diretriz")]
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

        public bool? IsFta { get; set; }

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

        [Display(Name = "Tema do Projeto")]
        public int TemaProjeto_Id { get; set; }
        public string TemaProjeto { get; set; }

        [Display(Name = "Tipo de Projeto")]
        public int TipoProjeto_Id { get; set; }
        public string TipoProjeto { get; set; }

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
                if (UnidadeDeMedida_Id > 0)
                {
                    var unidade = Pa_UnidadeMedida.Get(UnidadeDeMedida_Id);
                    return unidade;
                }
                else
                    return new Pa_UnidadeMedida();
            }
        }

        [Display(Name = "Data inicio Projeto / Iniciativa")]
        public DateTime? DataInicio { get; set; }

        [Display(Name = "_DataInicio")]
        public string _DataInicio { get; set; }

        [Display(Name = "Data fim Projeto / Iniciativa")]
        public DateTime? DataFim { get; set; }

        [Display(Name = "_DataFim")]
        public string _DataFim { get; set; }

        [Display(Name = "Responsável pelo Projeto / Iniciativa")]
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

        public bool IsTatico { get; set; }

        public int? Tatico_Id { get; set; }

        public void Update()
        {
            var query = "UPDATE [dbo].[Pa_Planejamento]                                             " +
                    "\n    SET [Gerencia_Id] = @Gerencia_Id                                " +
                    "\n       ,[Coordenacao_Id] = @Coordenacao_Id                                  " +
                    "\n       ,[Iniciativa_Id] = @Iniciativa_Id                                      " +
                    "\n       ,[IndicadoresDeProjeto_Id] = @IndicadoresDeProjeto_Id              " +
                    "\n       ,[ObjetivoGerencial_Id] = @ObjetivoGerencial_Id                                  " +
                    "\n       ,[ValorDe] = @ValorDe                                  " +
                    "\n       ,[ValorPara] = @ValorPara                                  " +
                    "\n       ,[DataInicio] = @DataInicio                                  " +
                    "\n       ,[DataFim] = @DataFim                                  " +
                    "\n  WHERE Id = @Id                                                      ";

            query += " SELECT CAST(1 AS int)";

            SqlCommand cmd;
            cmd = new SqlCommand(query);

            cmd.Parameters.AddWithValue("@Gerencia_Id", Gerencia_Id);
            cmd.Parameters.AddWithValue("@Coordenacao_Id", Coordenacao_Id);
            cmd.Parameters.AddWithValue("@Iniciativa_Id", Iniciativa_Id);
            cmd.Parameters.AddWithValue("@IndicadoresDeProjeto_Id", IndicadoresDeProjeto_Id);
            cmd.Parameters.AddWithValue("@ObjetivoGerencial_Id", ObjetivoGerencial_Id);
            cmd.Parameters.AddWithValue("@ValorDe", ValorDe);
            cmd.Parameters.AddWithValue("@ValorPara", ValorPara);
            cmd.Parameters.AddWithValue("@DataInicio", DataInicio);
            cmd.Parameters.AddWithValue("@DataFim", DataFim);
            cmd.Parameters.AddWithValue("@Id", Id);

            Salvar(cmd);
        }

        #endregion

        #region Depreciado

        [Display(Name = "Tema / Assunto")]
        public int TemaAssunto_Id { get; set; }
        public string TemaAssunto { get; set; }

        #endregion

        public bool? IsfiltrarAcao { get; set; }

        public Pa_Acao Acao { get; set; }

        public string _resumo
        {
            get
            {
                var retorno = string.Empty;
                if (Id > 0)
                {
                    retorno += //"Id: " + Id + " - Diretoria: " + Diretoria +
                               //" \n Missão: " + Missao +
                               //" \n Visão" + Visao +
                        " Dimensão" + Dimensao +
                        " \n Diretrizes / Objetivos" + Objetivo;
                    //" \n Indicadores da Diretrizes / Objetivos" + IndicadoresDiretriz +
                    //" \n Responsável pela Diretriz" + Responsavel_Diretriz_Quem.Name;

                }
                return retorno;
            }
        }

        public void IsValid()
        {

            //if (Tatico_Id.GetValueOrDefault() > 0)//é planejamento Operacional
            //{

            //}
            //else if (Estrategico_Id.GetValueOrDefault() > 0)
            //{
            //if (Gerencia_Id <= 0)
            //    message += "\n Gerencia,";
            //if (Coordenacao_Id <= 0)
            //    message += "\n Coordenação,";
            //if (Iniciativa_Id <= 0)
            //    message += "\n Projeto / Iniciativa,";
            //if (IndicadoresDeProjeto_Id <= 0)
            //    message += "\n Indicadores do Projeto / Iniciativa,";
            //if (ObjetivoGerencial_Id <= 0)
            //    message += "\n Objetivo Gerencial,";
            //if (_ValorDe == null || _ValorDe == "")
            //    message += "\n Valor De,";
            //if (_ValorPara == null || _ValorPara == "")
            //    message += "\n Valor Para,";
            //if (UnidadeDeMedida_Id <= 0)
            //    message += "\n Unidade de Medida,";
            //if (_DataInicio == null || _DataInicio == "")
            //    message += "\n Data inicio do Projeto / Iniciativa,";
            //if (_DataFim == null || _DataFim == "")
            //    message += "\n Data fim Projeto / Iniciativa,,";
            //if (Responsavel_Projeto < 0)
            //    message += "\n Responsavel pelo Projeto / Iniciativa,";

            //}
            //else
            //{
            //if (Diretoria_Id <= 0)
            //    message += "\n Diretoria,";
            //if (Missao_Id <= 0)
            //    message += "\n Missão,";
            //if (Visao_Id <= 0)
            //    message += "\n Visão,";
            //if (Dimensao_Id <= 0)
            //    message += "\n Dimensão,";
            //if (Objetivo_Id <= 0)
            //    message += "\n Diretrizes / Objetivos,";
            //if (IndicadoresDiretriz_Id <= 0)
            //    message += "\n Indicadores da Diretrizes / Objetivos,";
            //if (Responsavel_Diretriz <= 0)
            //    message += "\n Responsável pela Diretriz,";

            //}

            //VerificaMensagemCamposObrigatorios(message);

        }

        private static string query
        {
            get
            {

                return $@"SELECT
	Pl.Id
   ,Pl.AddDate
   ,Pl.AlterDate
   ,Pl.Diretoria_Id
   ,Pl.Gerencia_Id
   ,Pl.Coordenacao_Id
   ,Pl.Missao_Id
   ,Pl.Visao_Id
   ,Pl.TemaAssunto_Id
   ,Pl.Indicadores_Id
   ,Pl.Iniciativa_Id
   ,Pl.ObjetivoGerencial_Id
   ,Pl.Dimensao
   ,Pl.Objetivo
   ,Pl.ValorDe
   ,Pl.ValorPara
   ,Pl.DataInicio
   ,Pl.DataFim
   ,Pl.[Order]
   ,Pl.Dimensao_Id
   ,Pl.Objetivo_Id
   ,Pl.IndicadoresDiretriz_Id
   ,Pl.IndicadoresDeProjeto_Id
   ,Pl.Estrategico_Id
   ,Pl.Responsavel_Diretriz
   ,Pl.Responsavel_Projeto
   ,Pl.UnidadeDeMedida_Id
   ,Pl.IsTatico
   ,Pl.Tatico_Id
   ,Pl.IsFta
   ,Pl.TemaProjeto_Id
   ,Pl.TipoProjeto_Id
   ,INI.Name AS Inciativa
   ,DIR.Name AS Diretoria
   ,GER.Name AS Gerencia
   ,CORD.Name AS Coordenacao
   ,MISS.Name AS Missao
   ,VIS.Name AS Visao
   ,TEM.Name AS TemaAssunto
   ,TEMPROJ.Name AS TemaProjeto
   ,TIPPROJ.Name AS TipoProjeto
   ,INDIC.Name AS IndicadoresDiretriz
   ,INDICProj.Name AS IndicadoresDeProjeto
   ,OBJT.Name AS ObjetivoGerencial
   ,DIME.Name AS Dimensao
   ,INICI.Name AS Iniciativa
   ,OBJ.Name AS Objetivo
FROM (SELECT
		Pl1.Id
	   ,Pl1.AddDate
	   ,Pl1.AlterDate
	   ,Pl1.Diretoria_Id
	   ,Pl2.Gerencia_Id
	   ,Pl2.Coordenacao_Id
	   ,Pl1.Missao_Id
	   ,Pl1.Visao_Id
	   ,Pl1.TemaAssunto_Id
	   ,Pl1.Indicadores_Id
	   ,Pl2.Iniciativa_Id
	   ,Pl2.ObjetivoGerencial_Id
	   ,Pl1.Dimensao
	   ,Pl1.Objetivo
	   ,Pl2.ValorDe
	   ,Pl2.ValorPara
	   ,Pl2.DataInicio
	   ,Pl2.DataFim
	   ,Pl1.[Order]
	   ,Pl1.Dimensao_Id
	   ,Pl1.Objetivo_Id
	   ,Pl1.IndicadoresDiretriz_Id
	   ,Pl2.IndicadoresDeProjeto_Id
	   ,Pl2.Estrategico_Id
	   ,Pl2.Responsavel_Diretriz
	   ,Pl2.Responsavel_Projeto
	   ,Pl2.UnidadeDeMedida_Id
	   ,Pl2.IsTatico
	   ,Pl2.Tatico_Id
	   ,Pl1.IsFta
	   ,Pl2.TemaProjeto_Id
	   ,Pl2.TipoProjeto_Id
	FROM Pa_Planejamento Pl1
	INNER JOIN Pa_Planejamento Pl2
		ON Pl1.Id = Pl2.Estrategico_Id
	UNION ALL
	SELECT DISTINCT
		pl1.Id
	   ,pl1.AddDate
	   ,pl1.AlterDate
	   ,pl1.Diretoria_Id
	   ,pl1.Gerencia_Id
	   ,pl1.Coordenacao_Id
	   ,pl1.Missao_Id
	   ,pl1.Visao_Id
	   ,pl1.TemaAssunto_Id
	   ,pl1.Indicadores_Id
	   ,pl1.Iniciativa_Id
	   ,pl1.ObjetivoGerencial_Id
	   ,pl1.Dimensao
	   ,pl1.Objetivo
	   ,pl1.ValorDe
	   ,pl1.ValorPara
	   ,pl1.DataInicio
	   ,pl1.DataFim
	   ,pl1.[Order]
	   ,pl1.Dimensao_Id
	   ,pl1.Objetivo_Id
	   ,pl1.IndicadoresDiretriz_Id
	   ,pl1.IndicadoresDeProjeto_Id
	   ,pl1.Estrategico_Id
	   ,pl1.Responsavel_Diretriz
	   ,pl1.Responsavel_Projeto
	   ,pl1.UnidadeDeMedida_Id
	   ,pl1.IsTatico
	   ,pl1.Tatico_Id
	   ,pl1.IsFta
	   ,pl1.TemaProjeto_Id
	   ,pl1.TipoProjeto_Id
	FROM Pa_Planejamento Pl1
	LEFT JOIN Pa_Planejamento Pl2
		ON Pl1.Id = Pl2.Estrategico_Id
	WHERE Pl1.Estrategico_Id IS NULL
	AND Pl2.Estrategico_Id IS NULL) Pl
LEFT JOIN Pa_Iniciativa INI
	ON INI.Id = Pl.Iniciativa_Id
LEFT JOIN Pa_Diretoria DIR
	ON DIR.Id = Pl.Diretoria_Id
LEFT JOIN Pa_Gerencia GER
	ON Pl.Gerencia_Id = GER.Id
LEFT JOIN Pa_Coordenacao CORD
	ON CORD.Id = Pl.Coordenacao_Id
LEFT JOIN Pa_Missao MISS
	ON MISS.Id = Pl.Missao_Id
LEFT JOIN Pa_Visao VIS
	ON VIS.Id = Pl.Visao_Id
LEFT JOIN Pa_TemaAssunto TEM
	ON TEM.Id = Pl.TemaAssunto_Id
LEFT JOIN Pa_TemaProjeto TEMPROJ
	ON TEMPROJ.Id = Pl.TemaProjeto_Id
LEFT JOIN Pa_TipoProjeto TIPPROJ
	ON TIPPROJ.Id = Pl.TipoProjeto_Id
LEFT JOIN Pa_IndicadoresDiretriz INDIC
	ON INDIC.Id = Pl.IndicadoresDiretriz_Id
LEFT JOIN Pa_IndicadoresDeProjeto INDICProj
	ON INDICProj.Id = Pl.IndicadoresDeProjeto_Id
LEFT JOIN Pa_Iniciativa INICI
	ON INICI.Id = Pl.Iniciativa_Id
LEFT JOIN Pa_ObjetivoGeral OBJT
	ON OBJT.Id = Pl.ObjetivoGerencial_Id
LEFT JOIN Pa_Objetivo OBJ
	ON OBJ.Id = Pl.Objetivo_Id
LEFT JOIN Pa_Dimensao DIME
	ON DIME.Id = Pl.Dimensao_Id";

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
            var retorno = GetGenerico<Pa_Planejamento>(query + " WHERE Pl.Id = " + Id);
            return retorno;
        }

        public static Pa_Planejamento GetTatico(int Id)
        {
            return GetGenerico<Pa_Planejamento>(query + " WHERE Pl.Tatico_Id = " + Id);
        }

        public static Pa_Planejamento GetEstrategico(int Id)
        {
            return GetGenerico<Pa_Planejamento>(query + " WHERE Pl.Id = " + Id);
        }

        public static List<Pa_Planejamento> GetPlanejamentoAcao()
        {
            var retorno = new List<Pa_Planejamento>();
            var planejamentos = Listar();
            var acoes = Pa_Acao.Listar();
            var remover = new List<int>();


            foreach (var i in planejamentos)
            {
                //$
                if (i.UnidadeDeMedida_Id == 1)
                {
                    if (i.ValorDe > 0)
                        i._ValorDe = "R$ " + i.ValorDe.ToString("0.##");
                    if (i.ValorPara > 0)
                        i._ValorPara = "R$ " + i.ValorPara.ToString("0.##");
                }
                //Percentual
                if (i.UnidadeDeMedida_Id == 2)
                {
                    if (i.ValorDe > 0)
                        i._ValorDe = i.ValorDe.ToString("0.##") + " %";
                    if (i.ValorPara > 0)
                        i._ValorPara = i.ValorPara.ToString("0.##") + " %";
                }
                //if(i.Estrategico_Id.GetValueOrDefault() >0)
                //{
                //    remover.Add(i.Estrategico_Id.GetValueOrDefault());
                //}
                if (i.DataInicio.GetValueOrDefault() != DateTime.MinValue)
                    i._DataInicio = i.DataInicio.GetValueOrDefault().ToString("dd/MM/yyyy");
                else
                    i._DataInicio = string.Empty;

                if (i.DataFim.GetValueOrDefault() != DateTime.MinValue)
                    i._DataFim = i.DataFim.GetValueOrDefault().ToString("dd/MM/yyyy");
                else
                    i._DataFim = string.Empty;

                var acoesTmp = acoes.Where(r => r.Panejamento_Id == i.Tatico_Id);
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

                        if (k.QuandoInicio != DateTime.MinValue)
                            k._QuandoInicio = k.QuandoInicio.ToString("dd/MM/yyyy");
                        else
                            k._QuandoFim = string.Empty;

                        if (k.QuandoFim != DateTime.MinValue)
                            k._QuandoFim = k.QuandoFim.ToString("dd/MM/yyyy");
                        else
                            k._QuandoFim = string.Empty;

                        if (k.QuantoCusta > 0)
                            k._QuantoCusta = "R$ " + k.QuantoCusta.ToString("0.##");

                        planTemp.Acao = k;
                        retorno.Add(planTemp);
                    }
                }
                else
                {
                    i.Acao = new Pa_Acao();

                    retorno.Add(i);
                }
            }

            //retorno.RemoveAll(r => remover.Any(c => c == r.Id));

            return retorno;
        }

        public static List<Pa_Planejamento> GetPlanejamentoAcao(string dataInit, string dataFim)
        {
            var retorno = new List<Pa_Planejamento>();
            var planejamentos = Listar();
            var acoes = Pa_Acao.Listar();
            var remover = new List<int>();


            foreach (var i in planejamentos)
            {
                //$
                if (i.UnidadeDeMedida_Id == 1)
                {
                    if (i.ValorDe > 0)
                        i._ValorDe = "R$ " + i.ValorDe.ToString("0.##");
                    if (i.ValorPara > 0)
                        i._ValorPara = "R$ " + i.ValorPara.ToString("0.##");
                }
                //Percentual
                if (i.UnidadeDeMedida_Id == 2)
                {
                    if (i.ValorDe > 0)
                        i._ValorDe = i.ValorDe.ToString("0.##") + " %";
                    if (i.ValorPara > 0)
                        i._ValorPara = i.ValorPara.ToString("0.##") + " %";
                }
                else
                {
                    if (i.ValorDe > 0)
                        i._ValorDe = i.ValorDe.ToString("0.##");
                    if (i.ValorPara > 0)
                        i._ValorPara = i.ValorPara.ToString("0.##");
                }
                //if(i.Estrategico_Id.GetValueOrDefault() >0)
                //{
                //    remover.Add(i.Estrategico_Id.GetValueOrDefault());
                //}
                if (i.DataInicio.GetValueOrDefault() != DateTime.MinValue)
                    //i._DataInicio = i.DataInicio.GetValueOrDefault().ToString("dd/MM/yyyy");
                    i._DataInicio = i.DataInicio.GetValueOrDefault().ToString("yyyy-MM-dd");
                else
                    i._DataInicio = string.Empty;

                if (i.DataFim.GetValueOrDefault() != DateTime.MinValue)
                    //i._DataFim = i.DataFim.GetValueOrDefault().ToString("dd/MM/yyyy");
                    i._DataFim = i.DataFim.GetValueOrDefault().ToString("yyyy-MM-dd");
                else
                    i._DataFim = string.Empty;

                var acoesTmp = acoes.Where(r => r.Panejamento_Id == i.Tatico_Id);
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

                        if (k.QuandoInicio != DateTime.MinValue)
                            //k._QuandoInicio = k.QuandoInicio.ToString("dd/MM/yyyy");
                            k._QuandoInicio = k.QuandoInicio.ToString("yyyy-MM-dd");
                        else
                            k._QuandoFim = string.Empty;

                        if (k.QuandoFim != DateTime.MinValue)
                            //k._QuandoFim = k.QuandoFim.ToString("dd/MM/yyyy");
                            k._QuandoFim = k.QuandoFim.ToString("yyyy-MM-dd");
                        else
                            k._QuandoFim = string.Empty;

                        if (k.UnidadeDeMedida_Id == 1)
                        {
                            if (k.QuantoCusta > 0)
                                k._QuantoCusta = "R$ " + k.QuantoCusta.ToString("0.##");
                        }
                        else if (k.UnidadeDeMedida_Id == 2)
                        {
                            if (k.QuantoCusta > 0)
                                k._QuantoCusta = k.QuantoCusta.ToString("0.##") + " %";
                        }
                        else
                        {
                            if (k.QuantoCusta > 0)
                                k._QuantoCusta = k.QuantoCusta.ToString("0.##");
                        }

                        switch (k.TipoIndicador)
                        {
                            case 1:
                                k.TipoIndicadorName = "Diretrizes";
                                break;
                            case 2:
                                k.TipoIndicadorName = "Scorecard";
                                break;
                            default:
                                k.TipoIndicadorName = "";
                                break;
                        }

                        planTemp.Acao = k;
                        retorno.Add(planTemp);
                    }
                }
                else
                {
                    i.Acao = new Pa_Acao();

                    retorno.Add(i);
                }
            }

            var dtInit = DTO.Helpers.Guard.ParseDateToSqlV2(dataInit, DTO.Helpers.Guard.CultureCurrent.BR);
            var dtFim = DTO.Helpers.Guard.ParseDateToSqlV2(dataFim, DTO.Helpers.Guard.CultureCurrent.BR);

            var statusAberto = new int[] { 1, 5, 6 };
            var statusFechado = new int[] { 3, 4, 7, 8 };

            retorno = retorno.Where(r => statusAberto.Contains(r.Acao.Status) || (statusFechado.Contains(r.Acao.Status) && r.Acao._Acompanhamento.LastOrDefault()?.AddDate.Date <= dtFim && r.Acao._Acompanhamento.LastOrDefault()?.AddDate.Date >= dtInit) || r.Acao.Id == 0).ToList();

            //retorno = retorno.Where(r => r.Acao.QuandoFim <= dtFim && r.Acao.QuandoInicio >= dtInit).ToList();

            return retorno;
        }

    }

}
