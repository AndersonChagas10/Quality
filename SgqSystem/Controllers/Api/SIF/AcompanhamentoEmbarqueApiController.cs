﻿using Dominio;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.SIF
{
    [RoutePrefix("api/AcompanhamentoEmbarque")]
    public class AcompanhamentoEmbarqueApiController : BaseApiController
    {
        [HttpPost]
        [Route("Get")]
        public Retorno Get([FromBody] FormularioParaRelatorioViewModel form)
        {
            var retorno = GetHeadersValues(form);

            if (retorno != null)
            {
                using (var db = new SgqDbDevEntities())
                {
                    retorno.Aprovador = getAprovadorName(form, db);

                    retorno.Elaborador = getElaboradorName(form, db);

                    retorno.NomeRelatorio = getNomeRelatorio(form, db);

                    retorno.SiglaUnidade = getSiglaUnidade(form, db);
                }
            }

            return retorno;
        }

        private Retorno GetHeadersValues(FormularioParaRelatorioViewModel form)
        {
            var sql = $@"select distinct 
                    	phf.Name, 
                    	case when C2XHF.ParFieldType_Id = 1 
                    		then pmv.Name 
                    		else C2XHF.Value 
                    	end as 'Value'
                    	,C2XHF.ParHeaderField_Id as Id
                    from CollectionLevel2XParHeaderField C2XHF
                    inner join CollectionLevel2 c2 on c2.id = C2XHF.CollectionLevel2_Id
                    inner join ParHeaderField phf on phf.Id = C2XHF.ParHeaderField_Id
                    left join ParMultipleValues pmv on C2XHF.value = cast(PMV.Id as varchar(500))
                    where c2.parlevel1_id = { form.level1Id } 
                    	and c2.ParLevel2_Id = { form.level2Id }
                        and c2.UnitId = { form.unitId }
                        and c2.EvaluationNumber = { form.avaliacao }
                    	and cast(c2.CollectionDate as Date) = '{ form._dataInicioSQL }'
                    	and C2XHF.ParFieldType_Id <> 2";

            using (var db = new SgqDbDevEntities())
            {
                var headerFieldsValues = db.Database.SqlQuery<HeaderFieldsValues>(sql).ToList();

                if (headerFieldsValues.Count > 0)
                {
                    var retorno = new Retorno();


                    var TipoVeiculo = new List<string>();
                    var Transportador = new List<string>();
                    var Placa = new List<string>();
                    var NomeMotorista = new List<string>();
                    var LacreNumero = new List<string>();
                    var Termografo_Id = new List<string>();
                    var SifNumber = new List<string>();
                    var Pedido = new List<string>();
                    var Instrucao = new List<string>();
                    var NumeroNotaFiscal = new List<string>();
                    var TipoCarga = new List<string>();
                    var TipoProduto = new List<string>();
                    var TipoEmbalagem = new List<string>();
                    var TemperaturaMin = new List<string>();
                    var TemperaturaMax = new List<string>();


                    foreach (var item in headerFieldsValues)
                    {

                        switch (item.Id)
                        {
                            //case 198: //Tipo de veículo
                            //    TipoVeiculo.Add(item.Value);
                            //    break;
                            //case 199://Transportadora
                            //    Transportador.Add(item.Value);
                            //    break;
                            //case 200: //Placa do veículo
                            //    Placa.Add(item.Value);
                            //    break;
                            //case 201: //Nome do motorista
                            //    NomeMotorista.Add(item.Value);
                            //    break;
                            //case 202: //Lacre número
                            //    LacreNumero.Add(item.Value);
                            //    break;
                            //case 203: //Termógrafo número
                            //    Termografo_Id.Add(item.Value);
                            //    break;
                            //case 204: //SIF ou Nome
                            //    SifNumber.Add(item.Value);
                            //    break;
                            //case 205: //Pedido
                            //    Pedido.Add(item.Value);
                            //    break;
                            //case 207: //Instrução
                            //    Instrucao.Add(item.Value);
                            //    break;
                            //case 208: //Notas Fiscais
                            //    NumeroNotaFiscal.Add(item.Value);
                            //    break;
                            //case 209: //Tipo de produto
                            //    TipoCarga.Add(item.Value);
                            //    break;
                            //case 210: //Tipo de embalagem
                            //    TipoEmbalagem.Add(item.Value);
                            //    break;
                            //case 211: //Tipo de veículo
                            //    TipoVeiculo.Add(item.Value);
                            //    break;
                            //case 212: //Termógrafo - T° mín
                            //    TemperaturaMin.Add(item.Value);
                            //    break;
                            //case 213: //Termógrafo - T° máx   
                            //    TemperaturaMax.Add(item.Value);
                            //    break;


                            case 1166: //Tipo de veículo
                                TipoVeiculo.Add(item.Value);
                                break;
                            case 1167://Transportadora
                                Transportador.Add(item.Value);
                                break;
                            case 1168: //Placa do veículo
                                Placa.Add(item.Value);
                                break;
                            case 1169: //Nome do motorista
                                NomeMotorista.Add(item.Value);
                                break;
                            case 1170: //Lacre número
                                LacreNumero.Add(item.Value);
                                break;
                            case 1172: //Lacre número
                                LacreNumero.Add(item.Value);
                                break;
                            case 1171: //Termógrafo número
                                Termografo_Id.Add(item.Value);
                                break;
                            case 1173: //Termógrafo número
                                Termografo_Id.Add(item.Value);
                                break;
                            case 1174: //SIF ou Nome
                                SifNumber.Add(item.Value);
                                break;
                            case 1175: //Pedido
                                Pedido.Add(item.Value);
                                break;
                            case 1177: //Instrução
                                Instrucao.Add(item.Value);
                                break;
                            case 1178: //Notas Fiscais
                                NumeroNotaFiscal.Add(item.Value);
                                break;
                            case 1179: //Tipo de Carga
                                TipoCarga.Add(item.Value);
                                break;
                            case 1181: //Tipo de produto
                                TipoProduto.Add(item.Value);
                                break;
                            case 1180: //Tipo de embalagem
                                TipoEmbalagem.Add(item.Value);
                                break;
                            case 1182: //Termógrafo - T° mín
                                TemperaturaMin.Add(item.Value);
                                break;
                            case 1183: //Termógrafo - T° máx   
                                TemperaturaMax.Add(item.Value);
                                break;
                        }

                    }

                    retorno.TipoVeiculo = string.Join(" ,", TipoVeiculo);
                    retorno.Transportador = string.Join(" ,", Transportador);
                    retorno.Placa = string.Join(" ,", Placa);
                    retorno.NomeMotorista = string.Join(" ,", NomeMotorista);
                    retorno.LacreNumero = string.Join(" ,", LacreNumero);
                    retorno.Termografo_Id = string.Join(" ,", Termografo_Id);
                    retorno.SifNumber = string.Join(" ,", SifNumber);
                    retorno.Pedido = string.Join(" ,", Pedido);
                    retorno.Instrucao = string.Join(" ,", Instrucao);
                    retorno.NumeroNotaFiscal = string.Join(" ,", NumeroNotaFiscal);
                    retorno.TipoCarga = string.Join(" ,", TipoCarga);
                    retorno.TipoEmbalagem = string.Join(" ,", TipoEmbalagem);
                    retorno.TemperaturaMin = string.Join(" ,", TemperaturaMin);
                    retorno.TemperaturaMax = string.Join(" ,", TemperaturaMax);

                    retorno.ParCompanyName = db.ParCompany.Find(form.unitId).Name;

                    retorno.ParLevel2_Name = db.ParLevel2.Find(form.level2Id).Name;

                    var dataColeta = DateTime.ParseExact(form._dataInicioSQL, "yyyyMMdd", null);
                    var dataColetaAmanha = DateTime.ParseExact(form._dataInicioSQL, "yyyyMMdd", null).AddDays(1);

                    var usersColection = db.CollectionLevel2.Where(
                        r => (r.CollectionDate >= dataColeta && r.CollectionDate <= dataColetaAmanha) &&
                        r.ParLevel1_Id == form.level1Id &&
                        r.ParLevel2_Id == form.level2Id &&
                        r.EvaluationNumber == form.avaliacao &&
                        r.UnitId == form.unitId).Select(r => r.AuditorId).Distinct().ToList();

                    var usuarios = db.UserSgq.Where(r => usersColection.Contains(r.Id)).Select(r => r.FullName).ToList();

                    retorno.NomeUsuario = String.Join(",", usuarios);

                    retorno.Coletas = GetColetas(form);

                    retorno.CollectionDate = dataColeta;

                    return retorno;

                }

                return null;
            }

        }

        private List<Coleta> GetColetas(FormularioParaRelatorioViewModel form)
        {
            var sql = $@"SELECT
	pd.nCdProduto as Produto_Id
   ,pd.cNmProduto as Produto
   ,c2.CollectionDate
   ,p2.Id AS ParLevel2_Id
   ,p2.Name AS ParLevel2
   ,c2.EvaluationNumber as Avaliacao
   ,c2.Sample as Amostra
   ,r3.ParLevel3_Id
   ,r3.ParLevel3_Name as ParLevel3
   ,L3G.Name as Level3Group
   ,r3.Id
   ,r3.IsConform
   ,r3.IsNotEvaluate
FROM CollectionLevel2 C2
	INNER JOIN CollectionLevel2XParHeaderField C2XHF ON c2.Id = C2XHF.CollectionLevel2_Id
	INNER JOIN Produto pd ON C2XHF.value = CAST(pd.nCdProduto AS VARCHAR(500))
	INNER JOIN Result_Level3 R3 ON R3.CollectionLevel2_Id = c2.Id
	INNER JOIN parlevel2 P2 ON c2.ParLevel2_Id = p2.Id
	INNER JOIN ParLevel3Level2 L3L2 ON r3.ParLevel3_Id = L3L2.ParLevel3_Id and c2.ParLevel2_Id = L3L2.ParLevel2_Id and (c2.UnitId = L3L2.ParCompany_Id or L3L2.ParCompany_Id is null)
	LEFT JOIN ParLevel3Group L3G on L3G.Id = L3L2.ParLevel3Group_Id
WHERE 1 = 1
	AND CAST(c2.CollectionDate AS DATE) = '{form._dataInicioSQL}'
	AND c2.parlevel1_id = { form.level1Id }
	AND c2.ParLevel2_Id = { form.level2Id }
    AND c2.UnitId = { form.unitId }
    AND c2.EvaluationNumber = { form.avaliacao }
 	AND C2XHF.ParFieldType_Id = 2
ORDER BY L3G.Name DESC, c2.EvaluationNumber, c2.Sample, C2.CollectionDate DESC";

            using (var db = new SgqDbDevEntities())
            {
                var coletas = db.Database.SqlQuery<Coleta>(sql).ToList();

                return coletas;
            }
        }

    }

    public class Retorno
    {
        public DateTime CollectionDate { get; set; }
        public string ParCompanyName { get; set; }
        public string SifNumber { get; set; }
        public string TipoVeiculo { get; set; }
        public string Pedido { get; set; }
        public string NomeUsuario { get; set; }
        public string Termografo_Id { get; set; }
        public bool TermografoIsPresente
        {
            get
            {
                if (Termografo_Id != null)
                {
                    return true;
                }

                return false;
            }
        }
        public string TemperaturaMax { get; set; }
        public string TemperaturaMin { get; set; }
        public string TipoEmbalagem { get; set; }
        public string TipoCarga { get; set; }
        public string Transportador { get; set; }
        public string NomeMotorista { get; set; }
        public string ParLevel2_Name { get; set; }
        public string NumeroNotaFiscal { get; set; }
        public string LacreNumero { get; set; }
        public string Placa { get; set; }
        public string Instrucao { get; set; }
        public string Aprovador { get; set; }
        public string Elaborador { get; set; }
        public string NomeRelatorio { get; set; }
        public string SiglaUnidade { get; set; }

        public List<Coleta> Coletas { get; set; }

    }

    public class HeaderFieldsValues
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Coleta
    {
        public decimal Produto_Id { get; set; }
        public string Produto { get; set; }
        public DateTime CollectionDate { get; set; }
        public int ParLevel2_Id { get; set; }
        public string ParLevel2 { get; set; }
        public int Avaliacao { get; set; }
        public int Amostra { get; set; }
        public int ParLevel3_Id { get; set; }
        public string ParLevel3 { get; set; }
        public string Level3Group { get; set; }
        public int Id { get; set; }
        public bool IsConform { get; set; }
        public bool IsNotEvaluate { get; set; }
    }

}