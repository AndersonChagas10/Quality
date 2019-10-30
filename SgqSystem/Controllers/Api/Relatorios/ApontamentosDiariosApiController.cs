using ADOFactory;
using AutoMapper;
using Dominio;
using DTO;
using DTO.DTO.Params;
using DTO.Helpers;
using DTO.ResultSet;
using Newtonsoft.Json;
using SgqService.ViewModels;
using SgqSystem.Handlres;
using SgqSystem.Helpers;
using SgqSystem.Services;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/ApontamentosDiarios")]
    public class ApontamentosDiariosApiController : ApiController
    {
        private string conexao;
        public ApontamentosDiariosApiController()
        {
            conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            db.Configuration.LazyLoadingEnabled = false;
        }

        private List<ApontamentosDiariosResultSet> _mock { get; set; }
        private List<ApontamentosDiariosResultSet> _list { get; set; }
        private List<ApontamentosDiariosDomingoResultSet> _listApontomentosDiarioDomingo { get; set; }
        private SgqDbDevEntities db = new SgqDbDevEntities();

        [HttpPost]
        [Route("Get")]
        public List<ApontamentosDiariosResultSet> GetApontamentosDiarios([FromBody] FormularioParaRelatorioViewModel form)
        {

            CommonLog.SaveReport(form, "Report_Apontamentos_Diarios");

            var query = new ApontamentosDiariosResultSet().Select(form);

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<ApontamentosDiariosResultSet>(query).ToList();

                return _list;
            }
        }

        [HttpPost]
        [Route("GetApontamentosDiariosRH")]
        public List<ApontamentosDiariosResultSet> GetApontamentosDiariosRH([FromBody] DataCarrierFormularioNew form)
        {

            var query = new ApontamentosDiariosResultSet().SelectRH(form);

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<ApontamentosDiariosResultSet>(query).ToList();

                return _list;
            }
        }

        [HttpPost]
        [Route("GetApontamentosDomingo")]
        public List<ApontamentosDiariosDomingoResultSet> GetApontamentosDomingo([FromBody] FormularioParaRelatorioViewModel form)
        {

            CommonLog.SaveReport(form, "Report_Apontamentos_Diarios");

            var query = new ApontamentosDiariosDomingoResultSet().Select(form);

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _listApontomentosDiarioDomingo = factory.SearchQuery<ApontamentosDiariosDomingoResultSet>(query).ToList();
            }
            return _listApontomentosDiarioDomingo;
        }

        [HttpGet]
        [Route("PhotoPreview/{ResultLevel3Id}")]
        public String GetPhotoPreview(int ResultLevel3Id)
        {
            var result = db.Result_Level3_Photos.FirstOrDefault(r => r.Result_Level3_Id == ResultLevel3Id);
            if (result != null)
                return result.Photo_Thumbnaills;
            return null;
        }

        [HttpGet]
        [Route("Photos/{ResultLevel3Id}")]
        public List<Result_Level3_Photos> GetPhotos(int ResultLevel3Id)
        {
            return db.Result_Level3_Photos.Where(r => r.Result_Level3_Id == ResultLevel3Id).ToList();
        }

        [HttpPost]
        [Route("Edit/{id}")]
        public Result_Level3DTO EditResultLevel3(int id)
        {
            bool possuiVinculosResultado = false;
            using (var databaseSgq = new SgqDbDevEntities())
            {

                var resultlevel3 = databaseSgq.Result_Level3.Where(x => x.Id == id).FirstOrDefault();

                var resultlevel3Final = databaseSgq.Result_Level3
                   .Join(databaseSgq.ParLevel3Value, pl3v => pl3v.ParLevel3_Id, rl3 => rl3.ParLevel3_Id, (rl3, pl3v) => new { rl3, pl3v })
                   .Where(x => x.rl3.CollectionLevel2_Id == resultlevel3.CollectionLevel2_Id
                   && x.pl3v.ParLevel3InputType_Id == 10
                   && (x.pl3v.DynamicValue.Contains("{" + resultlevel3.ParLevel3_Id.ToString() + "}")
                   || x.pl3v.DynamicValue.Contains("{" + resultlevel3.ParLevel3_Id.ToString() + "?}"))
                   && x.pl3v.IsActive).ToList();

                if (resultlevel3Final.Count > 0)
                    possuiVinculosResultado = true;
            }
            if (!possuiVinculosResultado)
            {
                var retorno = Mapper.Map<Result_Level3DTO>(Result_Level3DTO.GetById(id));
                retorno.IntervalMin = retorno.IntervalMin == "-9999999999999.9000000000" ? "Sem limite mínimo" : retorno.IntervalMin;
                retorno.IntervalMax = retorno.IntervalMax == "9999999999999.9000000000" ? "Sem limite Máximo" : retorno.IntervalMax;
                return retorno;
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        [Route("Save/{userSgq_Id}")]
        public Result_Level3DTO SaveResultLevel3([FromUri] int userSgq_Id, [FromBody] Result_Level3DTO resultLevel3)
        {
            var parLevel3Value = new ParLevel3Value();
            using (var databaseSgq = new SgqDbDevEntities())
            {
                databaseSgq.Configuration.LazyLoadingEnabled = false;
                var resultlevel3 = databaseSgq.Result_Level3.Where(x => x.Id == resultLevel3.Id).FirstOrDefault();
                LogSystem.LogTrackBusiness.Register(resultlevel3, resultlevel3.Id, "Result_Level3", userSgq_Id);
                var parLevel3 = databaseSgq.ParLevel3.Where(x => x.Id == resultlevel3.ParLevel3_Id).FirstOrDefault();
                parLevel3Value = databaseSgq.ParLevel3Value.Where(x => x.ParLevel3_Id == parLevel3.Id).FirstOrDefault();
                var parInputTypeValues = databaseSgq.ParInputTypeValues.Where(x => x.ParLevel3Value_Id == parLevel3Value.Id && resultLevel3.Value == x.Intervalo.ToString()).FirstOrDefault();
                if (parLevel3Value.ParLevel3InputType_Id == 8)
                    resultLevel3.ValueText = parInputTypeValues.Valor.ToString();
            }

            //[TODO] Inserir registro de log de edição (salvar resultlevel3_Id == resultLevel3.Id)
            var query = resultLevel3.CreateUpdate();
            try
            {
                db.Database.ExecuteSqlCommand(query);
                var level3Result = db.Result_Level3.FirstOrDefault(r => r.Id == resultLevel3.Id);

                ConsolidacaoEdicao(resultLevel3.Id);
                return Mapper.Map<Result_Level3DTO>(Result_Level3DTO.GetById(resultLevel3.Id));
            }
            catch (System.Exception e)
            {
                throw e;
            }


        }

        [HttpPost]
        [Route("GetRL/{level1}/{shift}/{period}/{date}")]
        public List<CollectionLevel2> GetResultLevel3(int level1, int shift, int period, DateTime date)
        {
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;


            var retorno = db.CollectionLevel2.Where(r => r.ParLevel1_Id == level1).Include("Result_Level3").Include("Result_Level3.ParlLevel3").ToList();
            return retorno;

        }

        public void ConsolidacaoEdicao(int id)
        {
            var level3 = db.Result_Level3.Include("CollectionLevel2").FirstOrDefault(r => r.Id == id);

            var data = level3.CollectionLevel2.CollectionDate;
            var company_Id = level3.CollectionLevel2.UnitId;
            var level1_Id = level3.CollectionLevel2.ParLevel1_Id;

            var service = new SgqServiceBusiness.Api.SyncServiceApiController(conexao,conexao);

            service.ReconsolidationLevel3ByCollectionLevel2Id(level3.CollectionLevel2_Id.ToString());

            var retorno = service._ReConsolidationByLevel1(company_Id, level1_Id, data);
        }

        [HttpPost]
        [Route("EditCabecalho/{ResultLevel3_Id}")]
        public string EditCabecalho(int ResultLevel3_Id)
        {
            var retorno = getSelects(ResultLevel3_Id);

            var json = JsonConvert.SerializeObject(retorno, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return json;
        }

        [HttpPost]
        [Route("SaveCabecalho")]
        public bool SaveCabecalho([FromBody] ListCollectionLevel2XParHeaderField Lsc2xhf)
        {
            try
            {
                foreach (var item in Lsc2xhf.HeaderField)
                {
                    if (item.Id > 0)//Update
                    {
                        var original = db.CollectionLevel2XParHeaderField.FirstOrDefault(c => c.Id == item.Id);

                        //[TODO] Inserir registro de log de edição
                        LogSystem.LogTrackBusiness.Register(original, original.Id, "CollectionLevel2XParHeaderField", Lsc2xhf.UserSgq_Id);

                        if (string.IsNullOrEmpty(item.Value))//Remover
                        {
                            db.CollectionLevel2XParHeaderField.Remove(original);
                        }
                        else //Update
                        {
                            original.Value = item.Value;
                            original.ParHeaderField_Id = item.ParHeaderField_Id;
                            original.ParHeaderField_Name = item.ParHeaderField_Name;
                        }
                    }
                    else if (!string.IsNullOrEmpty(item.Value)) //Add
                    {
                        db.CollectionLevel2XParHeaderField.Add(item);
                    }
                }

                db.SaveChanges();

                //Reconsolida
                if (Lsc2xhf.HeaderField.Count > 0)
                {
                    var syncServices = new SgqServiceBusiness.Api.SyncServiceApiController(conexao,conexao);

                    syncServices.ReconsolidationToLevel3(Lsc2xhf.HeaderField[0].CollectionLevel2_Id.ToString());
                }

            }
            catch (Exception)
            {
                return false;
                throw;
            }

            return true;
        }

        public class Select
        {
            public ParHeaderField HeaderField { get; set; }
            public List<ParMultipleValues> Values { get; set; }
            public string ValueSelected { get; set; }
            public int CollectionLevel2XParHeaderField_Id { get; set; }
            public CollectionLevel2 CollectionLevel2 { get; set; }
        }

        public class ParLevels
        {
            public int Id { get; set; }
            public int ParLevel1_Id { get; set; }
            public int ParLevel2_Id { get; set; }
            public int EvaluationNumber { get; set; }
            public int Sample { get; set; }
        }

        public List<Select> getSelects(int ResultLevel3_Id)
        {
            //pegar os cabeçalhos
            var resultHeaderField = new List<Select>();
            var collectionLevel2 = getCollectionLevel2ByResultLevel3(ResultLevel3_Id);

            var query = $@"SELECT *
            FROM CollectionLevel2XParHeaderField
            WHERE CollectionLevel2_Id IN (SELECT
            		Id
            	FROM CollectionLevel2
            	WHERE id IN (SELECT
            			CollectionLevel2_Id
            		FROM Result_Level3
            		WHERE id = { ResultLevel3_Id }))";

            var coletas = db.Database.SqlQuery<CollectionLevel2XParHeaderField>(query).ToList();

            //Ids dos cabeçalhos de monitoramentos
            var level1HeaderFields_Id = db.ParLevel1XHeaderField.Include("ParHeaderField").Where(r => r.ParLevel1_Id == collectionLevel2.ParLevel1_Id && r.IsActive && r.ParHeaderField.ParLevelDefinition_Id == 1).Select(r => r.ParHeaderField_Id).ToList();

            //Ids dos cabeçalhos que não fazem parte do Monitoramento
            var headerFields_IdNot = db.ParLevel2XHeaderField.Where(r => r.ParLevel1_Id == collectionLevel2.ParLevel1_Id && r.ParLevel2_Id == collectionLevel2.ParLevel2_Id && r.IsActive).Select(r => r.ParHeaderField_Id).Except(level1HeaderFields_Id).ToList();

            //Ids dos cabeçalhos válidos
            var headerFields_Ids = db.ParLevel1XHeaderField.Where(r => r.ParLevel1_Id == collectionLevel2.ParLevel1_Id && !headerFields_IdNot.Contains(r.ParHeaderField_Id) && r.IsActive).Select(r => r.ParHeaderField_Id).ToList();

            //Seleciona os cabeçalhos
            var headerFields = db.ParHeaderField.Where(r => headerFields_Ids.Contains(r.Id)).OrderBy(r => r.ParLevelDefinition_Id).ThenBy(r => r.Id).ToList();

            var values = db.ParMultipleValues.ToList();

            foreach (var headerField in headerFields)
            {
                var select = new Select();

                //Atribui o campo de cabeçalho
                select.HeaderField = headerField;

                if (headerField.ParFieldType_Id == 2) //Se for campo integração
                {
                    /* Se não for produto que digito o código e busco em uma lista*/
                    if (headerField.Description != "Produto")
                    {
                        SGQDBContext.ParFieldType ParFieldTypeDB = new SGQDBContext.ParFieldType();

                        select.Values = ParFieldTypeDB.getIntegrationValues(headerField.Id, headerField.Description, collectionLevel2.UnitId).ToList();

                    }
                }
                else
                {
                    //pegar values dos campos de cabeçalho
                    select.Values = values.Where(r => r.ParHeaderField_Id == headerField.Id).ToList();
                }

                //Atribui o selecionado
                //Se tiver mais do que um valor duplica a inserção
                var resultados = coletas.Where(r => r.ParHeaderField_Id == headerField.Id).ToList();

                select.CollectionLevel2 = collectionLevel2;

                //Quantidades de campos coletados
                if (resultados.Count > 0)
                {
                    foreach (var resultado in resultados)
                    {
                        //Atribui a quantidade de cabeçalhos                       
                        select.CollectionLevel2XParHeaderField_Id = resultado.Id;
                        select.ValueSelected = resultado.Value;
                        resultHeaderField.Add(new Select()
                        {
                            CollectionLevel2 = select.CollectionLevel2,
                            CollectionLevel2XParHeaderField_Id = select.CollectionLevel2XParHeaderField_Id,
                            HeaderField = select.HeaderField,
                            Values = select.Values,
                            ValueSelected =
                            select.ValueSelected
                        });
                    }
                }
                else
                {
                    //Somente atribui sem valor selecionado
                    resultHeaderField.Add(select);
                }
            }

            //pegar os valor que está selecionado
            return resultHeaderField;
        }

        public CollectionLevel2 getCollectionLevel2ByResultLevel3(int ResultLevel3_Id)
        {
            var query = $@"SELECT
                        *
                    FROM CollectionLevel2
                    WHERE id = (SELECT
                    		CollectionLevel2_Id
                    	FROM Result_Level3
                    	WHERE id = {ResultLevel3_Id})";

            return db.Database.SqlQuery<CollectionLevel2>(query).FirstOrDefault();
        }

        public class ListCollectionLevel2XParHeaderField
        {
            public int UserSgq_Id { get; set; } //Quem editou
            public List<CollectionLevel2XParHeaderField> HeaderField { get; set; }
        }
    }
}