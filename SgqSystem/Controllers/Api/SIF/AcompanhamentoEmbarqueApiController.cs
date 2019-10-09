using Dominio;
using SgqService.ViewModels;
using SgqSystem.Controllers.Photo;
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
                retorno.Fotos = GetFotos(form);

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
                    from CollectionLevel2XParHeaderField C2XHF WITH (NOLOCK)
                    inner join CollectionLevel2 c2 WITH (NOLOCK) on c2.id = C2XHF.CollectionLevel2_Id
                    inner join ParHeaderField phf WITH (NOLOCK) on phf.Id = C2XHF.ParHeaderField_Id
                    left join ParMultipleValues pmv WITH (NOLOCK) on C2XHF.value = cast(PMV.Id as varchar(500))
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
                    var Transportadora = new List<string>();
                    var PlacaDoVeiculo = new List<string>();
                    var NomeMotorista = new List<string>();
                    var LacreNumero = new List<string>();
                    var Termografo_Id = new List<string>();
                    var SifNumber = new List<string>();
                    var Pedido = new List<string>();
                    var DataCarregamento = new List<string>();
                    var Instrucao = new List<string>();
                    var NumeroNotaFiscal = new List<string>();
                    var TipoCarga = new List<string>();
                    var TipoEmbalagem = new List<string>();
                    var TipoProduto = new List<string>();
                    var TemperaturaMin = new List<string>();
                    var TemperaturaMax = new List<string>();


                    int TipoVeiculoId = int.Parse(GetDicionarioEstatico("TipoVeiculo"));
                    int TransportadoraId = int.Parse(GetDicionarioEstatico("Transportador"));
                    int PlacaId = int.Parse(GetDicionarioEstatico("Placa"));
                    int NomeMotoristaId = int.Parse(GetDicionarioEstatico("NomeMotorista"));
                    int LacreNumeroId = int.Parse(GetDicionarioEstatico("LacreNumero"));
                    int TermografoId = int.Parse(GetDicionarioEstatico("Termografo_Id"));
                    int SifNumberId = int.Parse(GetDicionarioEstatico("SifNumber"));
                    int PedidoId = int.Parse(GetDicionarioEstatico("Pedido"));
                    int InstrucaoId = int.Parse(GetDicionarioEstatico("Instrucao"));
                    int TipoCargaId = int.Parse(GetDicionarioEstatico("TipoCarga"));
                    int TipoProdutoId = int.Parse(GetDicionarioEstatico("TipoProduto"));
                    int TipoEmbalagemId = int.Parse(GetDicionarioEstatico("TipoEmbalagem"));
                    int TemperaturaMinId = int.Parse(GetDicionarioEstatico("TemperaturaMin"));
                    int TemperaturaMaxId = int.Parse(GetDicionarioEstatico("TemperaturaMax"));
                    int NumeroNotaFiscalId = int.Parse(GetDicionarioEstatico("NumeroNotaFiscal"));
                    int DataCarregamentoId = int.Parse(GetDicionarioEstatico("DataCarregamento"));

                    foreach (var item in headerFieldsValues)
                    {

                        if (item.Id == TipoVeiculoId)
                        {
                            TipoVeiculo.Add(item.Value);
                        }
                        else if (item.Id == TransportadoraId)
                        {
                            Transportadora.Add(item.Value);
                        }
                        else if (item.Id == PlacaId)
                        {
                            PlacaDoVeiculo.Add(item.Value);
                        }
                        else if (item.Id == NomeMotoristaId)
                        {
                            NomeMotorista.Add(item.Value);
                        }
                        else if (item.Id == LacreNumeroId)
                        {
                            LacreNumero.Add(item.Value);
                        }
                        else if (item.Id == TermografoId)
                        {
                            Termografo_Id.Add(item.Value);
                        }
                        else if (item.Id == SifNumberId)
                        {
                            SifNumber.Add(item.Value);
                        }
                        else if (item.Id == PedidoId)
                        {
                            Pedido.Add(item.Value);
                        }
                        else if (item.Id == InstrucaoId)
                        {
                            Instrucao.Add(item.Value);
                        }
                        else if (item.Id == TipoCargaId)
                        {
                            TipoCarga.Add(item.Value);
                        }
                        else if (item.Id == TipoProdutoId)
                        {
                            TipoProduto.Add(item.Value);
                        }
                        else if (item.Id == TipoEmbalagemId)
                        {
                            TipoEmbalagem.Add(item.Value);
                        }
                        else if (item.Id == TemperaturaMinId)
                        {
                            TemperaturaMin.Add(item.Value);
                        }
                        else if (item.Id == TemperaturaMaxId)
                        {
                            TemperaturaMax.Add(item.Value);
                        }
                        else if (item.Id == NumeroNotaFiscalId)
                        {
                            NumeroNotaFiscal.Add(item.Value);
                        }
                        else if (item.Id == DataCarregamentoId)
                        {
                            DataCarregamento.Add(item.Value);
                        }


                        //switch (item.Id)
                        //{
                        ////JBS
                        //case 198: //Tipo de veículo
                        //    TipoVeiculo.Add(item.Value);
                        //    break;
                        //case 199://Transportadora
                        //    Transportadora.Add(item.Value);
                        //    break;
                        //case 200: //Placa do veículo
                        //    PlacaDoVeiculo.Add(item.Value);
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
                        //case 206: //Data do carregamento
                        //    DataCarregamento.Add(item.Value);
                        //    break;
                        //case 207: //Instrução
                        //    Instrucao.Add(item.Value);
                        //    break;
                        //case 208: //Notas Fiscais
                        //    NumeroNotaFiscal.Add(item.Value);
                        //    break;
                        //case 209: //Tipo de Carga
                        //    TipoCarga.Add(item.Value);
                        //    break;
                        //case 210: //Tipo de embalagem
                        //    TipoEmbalagem.Add(item.Value);
                        //    break;
                        //case 211: //Tipo de produto
                        //    TipoProduto.Add(item.Value);
                        //    break;
                        //case 212: //Termógrafo - T° mín
                        //    TemperaturaMin.Add(item.Value);
                        //    break;
                        //case 213: //Termógrafo - T° máx   
                        //    TemperaturaMax.Add(item.Value);
                        //    break;


                        ////GRT
                        //case 1166: //Tipo de veículo 
                        //    TipoVeiculo.Add(item.Value);
                        //    break;
                        //case 1167://Transportadora
                        //    Transportadora.Add(item.Value);
                        //    break;
                        //case 1168: //Placa do veículo
                        //    PlacaDoVeiculo.Add(item.Value);
                        //    break;
                        //case 1169: //Nome do motorista
                        //    NomeMotorista.Add(item.Value);
                        //    break;
                        //case 1172: //Lacre número
                        //    LacreNumero.Add(item.Value);
                        //    break;
                        //case 1173: //Termógrafo número
                        //    Termografo_Id.Add(item.Value);
                        //    break;
                        //case 1174: //SIF ou Nome
                        //    SifNumber.Add(item.Value);
                        //    break;
                        //case 1175: //Pedido
                        //    Pedido.Add(item.Value);
                        //    break;
                        //case 1176: //Data do Carregamento
                        //    DataCarregamento.Add(item.Value);
                        //    break;
                        //case 1177: //Instrução
                        //    Instrucao.Add(item.Value);
                        //    break;
                        //case 1178: //Notas Fiscais
                        //    NumeroNotaFiscal.Add(item.Value);
                        //    break;
                        //case 1179: //Tipo de Carga
                        //    TipoCarga.Add(item.Value);
                        //    break;
                        //case 1181: //Tipo de produto
                        //    TipoProduto.Add(item.Value);
                        //    break;
                        //case 1180: //Tipo de embalagem
                        //    TipoEmbalagem.Add(item.Value);
                        //    break;
                        //case 1182: //Termógrafo - T° mín
                        //    TemperaturaMin.Add(item.Value);
                        //    break;
                        //case 1183: //Termógrafo - T° máx   
                        //    TemperaturaMax.Add(item.Value);
                        //    break;
                        //}

                    }

                    retorno.TipoVeiculo = string.Join(" ,", TipoVeiculo);
                    retorno.Transportadora = string.Join(" ,", Transportadora);
                    retorno.PlacaDoVeiculo = string.Join(" ,", PlacaDoVeiculo);
                    retorno.NomeMotorista = string.Join(" ,", NomeMotorista);
                    retorno.LacreNumero = string.Join(" ,", LacreNumero);
                    retorno.Termografo_Id = string.Join(" ,", Termografo_Id);
                    retorno.SifNumber = string.Join(" ,", SifNumber);
                    retorno.Pedido = string.Join(" ,", Pedido);
                    retorno.Instrucao = string.Join(" ,", Instrucao);
                    retorno.NumeroNotaFiscal = string.Join(" ,", NumeroNotaFiscal);
                    retorno.TipoCarga = string.Join(" ,", TipoCarga);
                    retorno.TipoEmbalagem = string.Join(" ,", TipoEmbalagem);
                    retorno.TipoProduto = string.Join(" ,", TipoProduto);
                    retorno.TemperaturaMin = string.Join(" ,", TemperaturaMin);
                    retorno.TemperaturaMax = string.Join(" ,", TemperaturaMax);
                    retorno.DataCarregamento = string.Join(" ,", DataCarregamento);

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
	pd.nCdProduto AS Produto_Id
   ,pd.cNmProduto AS Produto
   ,c2.CollectionDate
   ,p2.Id AS ParLevel2_Id
   ,p2.Name AS ParLevel2
   ,c2.EvaluationNumber AS Avaliacao
   ,c2.Sample AS Amostra
   ,r3.ParLevel3_Id
   ,r3.ParLevel3_Name AS ParLevel3
   ,(SELECT TOP 1
			name
		FROM ParLevel3Group WITH (NOLOCK)
		WHERE id = (SELECT TOP 1
				ParLevel3Group_Id
			FROM ParLevel3Level2Level1 P321 WITH (NOLOCK)
			INNER JOIN ParLevel3Level2 P32 WITH (NOLOCK)
				ON P32.Id = P321.ParLevel3Level2_Id
			WHERE 1 = 1
			AND P321.ParLevel1_Id = c2.parLevel1_Id
			AND r3.ParLevel3_Id = P32.ParLevel3_Id
			AND P32.IsActive = 1
			AND P321.Active = 1
			AND c2.ParLevel2_Id = P32.ParLevel2_Id
			AND (c2.UnitId = P32.ParCompany_Id
			OR P32.ParCompany_Id IS NULL)
			AND (c2.UnitId = P321.ParCompany_Id
			OR P321.ParCompany_Id IS NULL)
			ORDER BY P32.ParCompany_Id DESC, P321.ParCompany_Id DESC))
	AS Level3Group
   ,r3.Id
   ,r3.IsConform
   ,r3.IsNotEvaluate
   ,r3.Value
   ,(SELECT TOP 1
			clxhf.value
		FROM CollectionLevel2XParHeaderField clxhf
		WHERE clxhf.CollectionLevel2_Id = C2.Id
		AND clxhf.ParHeaderField_Id = {GetDicionarioEstatico("SIF")})--216) --1186)
	AS SIF
   ,(SELECT TOP 1
			iif(clxhf.value is null, null, cast(clxhf.value as Date))
		FROM CollectionLevel2XParHeaderField clxhf
		WHERE clxhf.CollectionLevel2_Id = C2.Id
		AND clxhf.ParHeaderField_Id = {GetDicionarioEstatico("DataValidade")}) --218) --1188)
	AS DataValidade
   ,(SELECT TOP 1
			iif(clxhf.value is null, null, cast(clxhf.value as date))
		FROM CollectionLevel2XParHeaderField clxhf
		WHERE clxhf.CollectionLevel2_Id = C2.Id
		AND clxhf.ParHeaderField_Id = {GetDicionarioEstatico("DataProducaoEmbarque")}) -- 217) --1187)
	AS DataProducao
   ,(SELECT TOP 1
			clxhf.value
		FROM CollectionLevel2XParHeaderField clxhf
		WHERE clxhf.CollectionLevel2_Id = C2.Id
		AND clxhf.ParHeaderField_Id = {GetDicionarioEstatico("CB")}) -- 215) --1185)
	AS CB
FROM CollectionLevel2 C2 WITH (NOLOCK)
INNER JOIN CollectionLevel2XParHeaderField C2XHF WITH (NOLOCK)
	ON c2.Id = C2XHF.CollectionLevel2_Id
INNER JOIN Produto pd WITH (NOLOCK)
	ON C2XHF.value = CAST(pd.nCdProduto AS VARCHAR(500))
INNER JOIN Result_Level3 R3 WITH (NOLOCK)
	ON R3.CollectionLevel2_Id = c2.Id
INNER JOIN parlevel2 P2 WITH (NOLOCK)
	ON c2.ParLevel2_Id = p2.Id
WHERE 1 = 1
AND CAST(c2.CollectionDate AS DATE) = '{form._dataInicioSQL}'
AND c2.parlevel1_id = { form.level1Id }
AND c2.ParLevel2_Id = { form.level2Id }
AND c2.UnitId = { form.unitId }
AND c2.EvaluationNumber = { form.avaliacao }
AND C2XHF.ParFieldType_Id = 2
ORDER BY (SELECT TOP 1
		name
	FROM ParLevel3Group WITH (NOLOCK)
	WHERE id = (SELECT TOP 1
			ParLevel3Group_Id
		FROM ParLevel3Level2Level1 P321 WITH (NOLOCK)
		INNER JOIN ParLevel3Level2 P32 WITH (NOLOCK)
			ON P32.Id = P321.ParLevel3Level2_Id
		WHERE 1 = 1
		AND P321.ParLevel1_Id = c2.parLevel1_Id
		AND r3.ParLevel3_Id = P32.ParLevel3_Id
		AND P32.IsActive = 1
		AND P321.Active = 1
		AND c2.ParLevel2_Id = P32.ParLevel2_Id
		AND (c2.UnitId = P32.ParCompany_Id
		OR P32.ParCompany_Id IS NULL)
		AND (c2.UnitId = P321.ParCompany_Id
		OR P321.ParCompany_Id IS NULL)
		ORDER BY P32.ParCompany_Id DESC, P321.ParCompany_Id DESC))
DESC, c2.EvaluationNumber, c2.Sample, C2.CollectionDate DESC";


            using (var db = new SgqDbDevEntities())
            {
                db.Database.CommandTimeout = 300;
                var coletas = db.Database.SqlQuery<Coleta>(sql).ToList();

                return coletas;
            }
        }

        private List<Fotos> GetFotos(FormularioParaRelatorioViewModel form)
        {

            var sql = $@"
                    select 
                    	top 100 
                    	CL2XPHF.Value as Produto_Id ,
                    	P.cNmProduto Produto ,
                    	RL3.ParLevel3_Id as ParLevel3_Id ,
                    	RL3.ParLevel3_Name as ParLevel3 ,
                    	CL2.Sample as Amostra ,
                        RL3P.Photo as Photo,
                    	RL3P.Id as Result_Level3_Photos_Id
                    from CollectionLevel2 CL2
                    Inner Join Result_Level3 RL3 with (nolock) on CL2.Id = RL3.CollectionLevel2_Id
                    Inner Join Result_Level3_Photos RL3P with (nolock) on RL3P.Result_Level3_Id = RL3.Id
                    inner join CollectionLevel2XParHeaderField CL2XPHF with (nolock) on CL2.Id = CL2XPHF.CollectionLevel2_Id
                    Inner join Produto P with (nolock) on P.nCdProduto = CL2XPHF.Value
                    where 1 = 1
                    AND CL2XPHF.ParHeaderField_Id = {GetDicionarioEstatico("CodigoProduto")}
                    AND CL2.ParLevel1_Id = {GetDicionarioEstatico("PlanilhaRecebimentoCDs_Id")}
                    AND CAST(CL2.CollectionDate AS DATE) = '{form._dataInicioSQL}'
                    AND CL2.parlevel1_id = { form.level1Id }
                    AND CL2.ParLevel2_Id = { form.level2Id }
                    AND CL2.UnitId = { form.unitId }
                    AND CL2.EvaluationNumber = { form.avaliacao }
                ";

            using (var db = new SgqDbDevEntities())
            {
                db.Database.CommandTimeout = 300;
                var fotos = db.Database.SqlQuery<Fotos>(sql).ToList();

                return fotos;
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
        public string Transportadora { get; set; }
        public string NomeMotorista { get; set; }
        public string ParLevel2_Name { get; set; }
        public string NumeroNotaFiscal { get; set; }
        public string LacreNumero { get; set; }
        public string PlacaDoVeiculo { get; set; }
        public string Instrucao { get; set; }
        public string Aprovador { get; set; }
        public string Elaborador { get; set; }
        public string NomeRelatorio { get; set; }
        public string SiglaUnidade { get; set; }
        public string DataCarregamento { get; set; }
        public string TipoProduto { get; set; }

        public List<Coleta> Coletas { get; set; }

        public List<Fotos> Fotos { get; set; }

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
        public string Value { get; set; }
        public string SIF { get; set; }
        public DateTime? DataValidade { get; set; }
        public DateTime? DataProducao { get; set; }
        public string CB { get; set; }
    }

    public class Fotos
    {
        public string Produto_Id { get; set; }
        public string Produto { get; set; }
        public int ParLevel3_Id { get; set; }
        public string ParLevel3 { get; set; }
        public int Amostra { get; set; }
        public string Photo { get; set; }
        public int Result_Level3_Photos_Id { get; set; }
    }
}