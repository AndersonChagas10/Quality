﻿using PlanoAcaoCore.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;

namespace PlanoAcaoCore
{
    public class Pa_Planejamento : Pa_BaseObject
    {

        public bool EmDia { get; set; } = true;

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
        public bool ObjetivoPriorizado { get; set; }

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

        public int QtdeAcao { get; set; }

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
   ,OBJ.IsPriority AS ObjetivoPriorizado
FROM (SELECT
		Estrategico.Id
	   ,Estrategico.AddDate
	   ,Estrategico.AlterDate
	   ,Estrategico.Diretoria_Id
	   ,Tatico.Gerencia_Id
	   ,Tatico.Coordenacao_Id
	   ,Estrategico.Missao_Id
	   ,Estrategico.Visao_Id
	   ,Estrategico.TemaAssunto_Id
	   ,Estrategico.Indicadores_Id
	   ,Tatico.Iniciativa_Id
	   ,Tatico.ObjetivoGerencial_Id
	   ,Estrategico.Dimensao
	   ,Estrategico.Objetivo
	   ,Tatico.ValorDe
	   ,Tatico.ValorPara
	   ,Tatico.DataInicio
	   ,Tatico.DataFim
	   ,Estrategico.[Order]
	   ,Estrategico.Dimensao_Id
	   ,Estrategico.Objetivo_Id
	   ,Estrategico.IndicadoresDiretriz_Id
	   ,Tatico.IndicadoresDeProjeto_Id
	   ,Tatico.Estrategico_Id
	   ,Estrategico.Responsavel_Diretriz
	   ,Tatico.Responsavel_Projeto
	   ,Tatico.UnidadeDeMedida_Id
	   ,Tatico.IsTatico
	   ,Tatico.Id as Tatico_Id
	   ,Estrategico.IsFta
	   ,Tatico.TemaProjeto_Id
	   ,Tatico.TipoProjeto_Id
	FROM Pa_Planejamento Estrategico
	INNER JOIN Pa_Planejamento Tatico ON Estrategico.Id = Tatico.Estrategico_Id
	UNION ALL
	SELECT DISTINCT
		Estrategico.Id
	   ,Estrategico.AddDate
	   ,Estrategico.AlterDate
	   ,Estrategico.Diretoria_Id
	   ,Estrategico.Gerencia_Id
	   ,Estrategico.Coordenacao_Id
	   ,Estrategico.Missao_Id
	   ,Estrategico.Visao_Id
	   ,Estrategico.TemaAssunto_Id
	   ,Estrategico.Indicadores_Id
	   ,Estrategico.Iniciativa_Id
	   ,Estrategico.ObjetivoGerencial_Id
	   ,Estrategico.Dimensao
	   ,Estrategico.Objetivo
	   ,Estrategico.ValorDe
	   ,Estrategico.ValorPara
	   ,Estrategico.DataInicio
	   ,Estrategico.DataFim
	   ,Estrategico.[Order]
	   ,Estrategico.Dimensao_Id
	   ,Estrategico.Objetivo_Id
	   ,Estrategico.IndicadoresDiretriz_Id
	   ,Estrategico.IndicadoresDeProjeto_Id
	   ,Estrategico.Estrategico_Id
	   ,Estrategico.Responsavel_Diretriz
	   ,Estrategico.Responsavel_Projeto
	   ,Estrategico.UnidadeDeMedida_Id
	   ,Estrategico.IsTatico
	   ,null as Tatico_Id
	   ,Estrategico.IsFta
	   ,Estrategico.TemaProjeto_Id
	   ,Estrategico.TipoProjeto_Id
	FROM Pa_Planejamento Estrategico
	LEFT JOIN Pa_Planejamento Tatico ON Estrategico.Id = Tatico.Estrategico_Id
	WHERE Estrategico.Estrategico_Id IS NULL AND Tatico.Estrategico_Id IS NULL) Pl
LEFT JOIN Pa_Iniciativa INI ON INI.Id = Pl.Iniciativa_Id
LEFT JOIN Pa_Diretoria DIR ON DIR.Id = Pl.Diretoria_Id
LEFT JOIN Pa_Gerencia GER ON Pl.Gerencia_Id = GER.Id
LEFT JOIN Pa_Coordenacao CORD ON CORD.Id = Pl.Coordenacao_Id
LEFT JOIN Pa_Missao MISS ON MISS.Id = Pl.Missao_Id
LEFT JOIN Pa_Visao VIS ON VIS.Id = Pl.Visao_Id
LEFT JOIN Pa_TemaAssunto TEM ON TEM.Id = Pl.TemaAssunto_Id
LEFT JOIN Pa_TemaProjeto TEMPROJ ON TEMPROJ.Id = Pl.TemaProjeto_Id
LEFT JOIN Pa_TipoProjeto TIPPROJ ON TIPPROJ.Id = Pl.TipoProjeto_Id
LEFT JOIN Pa_IndicadoresDiretriz INDIC ON INDIC.Id = Pl.IndicadoresDiretriz_Id
LEFT JOIN Pa_IndicadoresDeProjeto INDICProj ON INDICProj.Id = Pl.IndicadoresDeProjeto_Id
LEFT JOIN Pa_Iniciativa INICI ON INICI.Id = Pl.Iniciativa_Id
LEFT JOIN Pa_ObjetivoGeral OBJT ON OBJT.Id = Pl.ObjetivoGerencial_Id
LEFT JOIN Pa_Objetivo OBJ ON OBJ.Id = Pl.Objetivo_Id
LEFT JOIN Pa_Dimensao DIME ON DIME.Id = Pl.Dimensao_Id";

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

                if (i.UnidadeDeMedida_Id == 1)
                {
                    i._ValorDe = "R$ " + i.ValorDe.ToString("0.##");
                    i._ValorPara = "R$ " + i.ValorPara.ToString("0.##");
                }
                //Percentual
                if (i.UnidadeDeMedida_Id == 2)
                {
                    i._ValorDe = i.ValorDe.ToString("0.##") + " %";
                    i._ValorPara = i.ValorPara.ToString("0.##") + " %";
                }

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
                    foreach (var acao in acoesTmp)
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

                        if (acao.QuandoInicio != DateTime.MinValue)
                            acao._QuandoInicio = acao.QuandoInicio.ToString("dd/MM/yyyy");
                        else
                            acao._QuandoFim = string.Empty;

                        if (acao.QuandoFim != DateTime.MinValue)
                            acao._QuandoFim = acao.QuandoFim.ToString("dd/MM/yyyy");
                        else
                            acao._QuandoFim = string.Empty;

                        if (acao.QuantoCusta > 0)
                            acao._QuantoCusta = "R$ " + acao.QuantoCusta.ToString("0.##");

                        if (!string.IsNullOrEmpty(acao.ParDepartments_Hash))
                        {
                            var departamentos_id = acao.ParDepartments_Hash.Split('-').ToList();
                            var departamentosNames = acao.ParDepartmentsName.Split('|').ToList();

                            acao.SecaoName = departamentosNames.Last();
                            acao.Secao_Id = Int32.Parse(departamentos_id.Last());

                            departamentos_id.RemoveAt(departamentos_id.Count - 1);
                            departamentosNames.RemoveAt(departamentos_id.Count - 1);

                            acao.ParDepartmentsName = string.Join("|", departamentosNames);
                            acao.ParDepartments_Hash = string.Join("-", departamentos_id);
                        }

                        planTemp.Acao = acao;
                        retorno.Add(planTemp);
                    }
                }
                else
                {
                    i.Acao = new Pa_Acao();

                    retorno.Add(i);
                }
            }


            return retorno;
        }

        public static List<Pa_Planejamento> GetPlanejamentoAcao(string dataInit, string dataFim)
        {
            var retorno = new List<Pa_Planejamento>();
            var planejamentos = Listar();
            var acoes = Pa_Acao.Listar();
            var remover = new List<int>();

            var statusAberto = new int[] { 1, 5, 6, 9 };
            var statusFechado = new int[] { 3, 4, 7, 8 };

            var dtInit = DTO.Helpers.Guard.ParseDateToSqlV2(dataInit, DTO.Helpers.Guard.CultureCurrent.BR);
            var dtFim = DTO.Helpers.Guard.ParseDateToSqlV2(dataFim, DTO.Helpers.Guard.CultureCurrent.BR);

            foreach (var i in planejamentos)
            {
                //$
                if (i.UnidadeDeMedida_Id == 1)
                {
                    i._ValorDe = "R$ " + i.ValorDe.ToString("0.##");
                    i._ValorPara = "R$ " + i.ValorPara.ToString("0.##");
                }
                else//Percentual
                if (i.UnidadeDeMedida_Id == 2)
                {
                    i._ValorDe = i.ValorDe.ToString("0.##") + " %";
                    i._ValorPara = i.ValorPara.ToString("0.##") + " %";
                }
                else
                {
                    i._ValorDe = i.ValorDe.ToString("0.##");
                    i._ValorPara = i.ValorPara.ToString("0.##");
                }


                if (i.DataInicio.GetValueOrDefault() != DateTime.MinValue)
                    i._DataInicio = i.DataInicio.GetValueOrDefault().ToString("yyyy-MM-dd");
                else
                    i._DataInicio = string.Empty;

                if (i.DataFim.GetValueOrDefault() != DateTime.MinValue)
                    i._DataFim = i.DataFim.GetValueOrDefault().ToString("yyyy-MM-dd");
                else
                    i._DataFim = string.Empty;

                var acoesTmp = acoes.Where(r => r.Panejamento_Id == i.Tatico_Id);
                if (acoesTmp.Count() > 0)
                {
                    bool existemAcoesAbertas = acoesTmp.Any(a => statusAberto.Contains(a.Status));
                    i.EmDia =
                        (!existemAcoesAbertas &&
                            (i.DataInicio >= dtInit && i.DataInicio <= dtFim)
                            ||
                            (i.DataFim <= dtFim && i.DataFim >= dtInit))
                            ||
                            existemAcoesAbertas && (i.DataFim <= dtFim || i.DataInicio <= dtFim);

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
                            }

                        if (k.QuandoInicio != DateTime.MinValue)
                            k._QuandoInicio = k.QuandoInicio.ToString("yyyy-MM-dd");
                        else
                            k._QuandoFim = string.Empty;

                        if (k.QuandoFim != DateTime.MinValue)
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

                        k._StatusName = k._StatusName ?? "";
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

            retorno = retorno.Where(r => (r.Acao.Id == 0 //Projetos sem ações
                || r.EmDia) && r.Acao.Status != (int)Enums.Status.Cancelado //Projetos que não estão em dia
            ).ToList();


            return retorno;
        }

        public static List<Pa_Planejamento> GetPlanejamentoRange(string dataInit, string dataFim)
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
                else//Percentual
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

                if (i.DataInicio.GetValueOrDefault() != DateTime.MinValue)
                    i._DataInicio = i.DataInicio.GetValueOrDefault().ToString("yyyy-MM-dd");
                else
                    i._DataInicio = string.Empty;

                if (i.DataFim.GetValueOrDefault() != DateTime.MinValue)
                    i._DataFim = i.DataFim.GetValueOrDefault().ToString("yyyy-MM-dd");
                else
                    i._DataFim = string.Empty;

                var acoesTmp = acoes.Where(r => r.Panejamento_Id == i.Tatico_Id);

                i.QtdeAcao = acoesTmp.Count();

   

                retorno.Add(i);
            }

            var dtInit = DTO.Helpers.Guard.ParseDateToSqlV2(dataInit, DTO.Helpers.Guard.CultureCurrent.BR);
            var dtFim = DTO.Helpers.Guard.ParseDateToSqlV2(dataFim, DTO.Helpers.Guard.CultureCurrent.BR);

            var statusAberto = new int[] { 1, 5, 6 };
            var statusFechado = new int[] { 3, 4, 7, 8 };

            retorno = retorno.Where(r => r.DataFim <= dtFim && r.DataInicio >= dtInit).ToList();

            return retorno;
        }

    }

}
