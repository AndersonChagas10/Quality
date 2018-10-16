﻿using ADOFactory;
using AutoMapper;
using Dominio;
using Dominio.Interfaces.Services;
using DTO;
using DTO.DTO;
using DTO.DTO.Params;
using DTO.Helpers;
using Newtonsoft.Json;
using SgqSystem.Controllers.Api.SelectVinculado;
//using SGQDBContext;
using SgqSystem.Handlres;
using SgqSystem.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Params
{
    [HandleApi()]
    [RoutePrefix("api/ParamsApi")]
    public class ParamsApiController : ApiController
    {


        #region Constructor

        private IParamsDomain _paramdDomain;
        private IBaseDomain<ParLevel1, ParLevel1DTO> _baseParLevel1;
        private IBaseDomain<ParLevel2, ParLevel2DTO> _baseParLevel2;
        private IBaseDomain<ParLevel3, ParLevel3DTO> _baseParLevel3;
        private IBaseDomain<ParHeaderField, ParHeaderFieldDTO> _baseParHeaderField;
        private IBaseDomain<ParMultipleValues, ParMultipleValuesDTO> _baseParMultipleValues;

        public ParamsApiController(IParamsDomain paramdDomain
            , IBaseDomain<ParLevel2, ParLevel2DTO> baseParLevel2
            , IBaseDomain<ParLevel3, ParLevel3DTO> baseParLevel3
            , IBaseDomain<ParLevel1, ParLevel1DTO> baseParLevel1
            , IBaseDomain<ParHeaderField, ParHeaderFieldDTO> baseParHeaderField
            , IBaseDomain<ParMultipleValues, ParMultipleValuesDTO> baseParMultipleValues)
        {
            _baseParLevel1 = baseParLevel1;
            _baseParLevel2 = baseParLevel2;
            _baseParLevel3 = baseParLevel3;
            _paramdDomain = paramdDomain;
            _baseParHeaderField = baseParHeaderField;
            _baseParMultipleValues = baseParMultipleValues;
        }

        #endregion

        #region Metods

        [HttpPost]
        [ValidateModel]
        [Route("AddUpdateLevel1")]
        public ParamsViewModel AddUpdateLevel1([FromBody] ParamsViewModel paramsViewModel)
        {
            #region GAMBIARRA LEVEL 100!
            paramsViewModel.paramsDto.parLevel1Dto.IsSpecificNumberEvaluetion = paramsViewModel.paramsDto.parLevel1Dto.IsSpecificNumberSample;
            #endregion
            paramsViewModel.paramsDto = _paramdDomain.AddUpdateLevel1(paramsViewModel.paramsDto);
            return paramsViewModel;
        }   

        [HttpPost]
        [Route("SetRequiredCCAB/{Id}/{Required}")]
        public bool SetRequiredCamposCabecalho(int id, int required)
        {
            return _paramdDomain.SetRequiredCamposCabecalho(id, required);
        }

        [HttpPost]
        [Route("SetDefaultCCAB/{IdHeader}/{IdMultiple}")]
        public ParMultipleValues SetDefaultMultiplaEscolha(int idHeader, int idMultiple)
        {
            return _paramdDomain.SetDefaultMultiplaEscolha(idHeader, idMultiple);
        }

        [HttpPost]
        [Route("AddUpdateLevel2")]
        public ParamsViewModel AddUpdateLevel2([FromBody] ParamsViewModel paramsViewModel)
        {
            paramsViewModel.paramsDto = _paramdDomain.AddUpdateLevel2(paramsViewModel.paramsDto);
            return paramsViewModel;
        }

        [HttpPost]
        [Route("AddUpdateParLevel3Group")]
        public ParamsViewModel AddUpdateParLevel3Group([FromBody] ParamsViewModel paramsViewModel)
        {
            paramsViewModel.paramsDto = _paramdDomain.AddUpdateLevel2(paramsViewModel.paramsDto);
            return paramsViewModel;
        }

        [HttpPost]
        [Route("RemoveParLevel3Group/{Id}")]
        public ParLevel3GroupDTO RemoveParLevel3Group(int Id)
        {
            return _paramdDomain.RemoveParLevel3Group(Id);
        }

        [HttpPost]
        [Route("AddUpdateLevel3")]
        public ParamsViewModel AddUpdateLevel3([FromBody] ParamsViewModel paramsViewModel)
        {
            paramsViewModel.paramsDto = _paramdDomain.AddUpdateLevel3(paramsViewModel.paramsDto);
            return paramsViewModel;
        }

        //[HttpPost]
        //[Route("AlteraAvaliacaoAmostra")]
        //public void AlteraAvaliacaoAmostra(ParLevel2SampleEvaluationDTO alterObj)
        //{
        //    using (var db = new SgqDbDevEntities())
        //    {
        //        /*Busco do DB*/
        //        var sample = db.ParSample.FirstOrDefault(r => r.Id == alterObj.sampleId);
        //        var evaluation = db.ParEvaluation.FirstOrDefault(r => r.Id == alterObj.evaluationId);

        //        /*Altero*/
        //        sample.Number = alterObj.sampleNumber;
        //        evaluation.Number = alterObj.evaluationNumber;
        //        sample.AlterDate = DateTime.Now;
        //        evaluation.AlterDate = DateTime.Now;

        //        /*Explico para o EF que alterei*/
        //        db.ParSample.Attach(sample);
        //        var entrySample = db.Entry(sample);
        //        entrySample.Property(e => e.Number).IsModified = true;
        //        entrySample.Property(e => e.AlterDate).IsModified = true;

        //        db.ParEvaluation.Attach(evaluation);
        //        var entryEvaluation = db.Entry(evaluation);
        //        entryEvaluation.Property(e => e.Number).IsModified = true;
        //        entryEvaluation.Property(e => e.AlterDate).IsModified = true;

        //        /*Salvo*/
        //        db.SaveChanges();
        //    }
        //}

        //[HttpPost]
        //[Route("SalvarAvaliacaoAmostra")]
        //public void SalvarAvaliacaoAmostra(ParLevel2SampleEvaluationDTO alterObj)
        //{
        //    using (var db = new SgqDbDevEntities())
        //    {
        //        /*Crio obj*/
        //        var sample = new ParSample()
        //        {
        //            AddDate = DateTime.Now,
        //            Number = alterObj.sampleNumber,
        //            IsActive = true,
        //            ParCompany_Id = alterObj.companyId,
        //            ParLevel2_Id = alterObj.level2Id
        //        };
        //        var evaluation = new ParEvaluation()
        //        {
        //            AddDate = DateTime.Now,
        //            Number = alterObj.evaluationNumber,
        //            IsActive = true,
        //            ParCompany_Id = alterObj.companyId,
        //            ParLevel2_Id = alterObj.level2Id
        //        };

        //        /*Explico para o EF que Adicionei*/
        //        db.ParSample.Add(sample);
        //        db.ParEvaluation.Add(evaluation);

        //        /*Salvo*/
        //        db.SaveChanges();
        //    }
        //}

        //[HttpPost]
        //[Route("AlteraVinculoLevel3")]
        //public void AlteraVinculoLevel3(ParLevel3Level2 l3l2)
        //{
        //    using (var db = new SgqDbDevEntities())
        //    {
        //        /*Busco do DB*/
        //        var parLevel3Level2 = db.ParLevel3Level2.FirstOrDefault(r => r.Id == l3l2.Id);

        //        /*Altero*/
        //        parLevel3Level2.AlterDate = DateTime.Now;
        //        parLevel3Level2.Weight = l3l2.Weight;

        //        parLevel3Level2.ParLevel3Group_Id = l3l2.ParLevel3Group_Id;

        //        /*Explico para o EF que alterei*/
        //        db.ParLevel3Level2.Attach(parLevel3Level2);
        //        var entrySample = db.Entry(parLevel3Level2);
        //        entrySample.Property(e => e.Weight).IsModified = true;
        //        entrySample.Property(e => e.AlterDate).IsModified = true;
        //        entrySample.Property(e => e.ParLevel3Group_Id).IsModified = true;

        //        /*Salvo*/
        //        db.SaveChanges();
        //    }
        //}


        #endregion

        #region Vinculo Level3 com Level2

        //[HttpGet]
        //[Route("AddVinculoL3L2/{idLevel2}/{idLevel3}/{peso}/{groupLevel2}")]
        //public ParLevel3Level2DTO AddVinculoL3L2(int idLevel2, int idLevel3, decimal peso, int? groupLevel2 = 0)
        //{
        //    return _paramdDomain.AddVinculoL3L2(idLevel2, idLevel3, peso, groupLevel2);
        //}


        #endregion

        #region Vinculo Level1 com Level2

        [HttpGet]
        [Route("AddVinculoL1L2/{idLevel1}/{idLevel2}/{idLevel3}/{userId}")]
        public List<ParLevel3Level2Level1DTO> AddVinculoL1L2(int idLevel1, int idLevel2, int idLevel3, int userId)
        {
            return _paramdDomain.AddVinculoL1L2(idLevel1, idLevel2, idLevel3, userId);
        }

        //[HttpPost]
        //[Route("RemVinculoL1L2/{idLevel1}/{idLevel2}")]
        //public bool RemVinculoL1L2(int idLevel1, int idLevel2)
        //{
        //    return _paramdDomain.RemVinculoL1L2(idLevel1, idLevel2);
        //}

        [HttpPost]
        [Route("RemVinculoL2L3/{idLevel1}/{idLevel2}/{idLevel3}/{companyId}")]
        public string RemVinculoL2L3(int idLevel1, int idLevel2, int idLevel3, int? companyId = null)
        {
            var retorno = "Registro desvinculado.";

            using (var db = new SgqDbDevEntities())
            {
                //throw new Exception("teste");
                var parLevel3Level2Level1 = db.ParLevel3Level2Level1.Include("ParLevel3Level2").Where(r => r.ParLevel3Level2.ParLevel2_Id == idLevel2 && r.ParLevel3Level2.ParLevel3_Id == idLevel3 && r.ParCompany_Id == companyId);

                var existe = db.ParLevel3Level2Level1.Include("ParLevel3Level2").Where(r => r.ParLevel3Level2.ParLevel2_Id == idLevel2 && r.ParLevel3Level2.ParLevel3_Id == idLevel3 && r.ParCompany_Id == companyId).ToList();

                if (existe == null || existe.Count() == 0)
                    return retorno;

                foreach (var i in parLevel3Level2Level1)
                    db.ParLevel3Level2.Remove(i.ParLevel3Level2);

                db.ParLevel3Level2Level1.RemoveRange(parLevel3Level2Level1);

                db.SaveChanges();
            }

            //return _paramdDomain.RemVinculoL1L2(idLevel1, idLevel2);
            return retorno;
        }

        [HttpPost]
        [Route("VerificaShowBtnRemVinculoL1L2/{idLevel1}/{idLevel2}")]
        public bool VerificaShowBtnRemVinculoL1L2(int idLevel1, int idLevel2)
        {
            return _paramdDomain.VerificaShowBtnRemVinculoL1L2(idLevel1, idLevel2);
        }

        #endregion

        [HttpPost]
        [Route("GetAutorizacoes/{l1}/{l2}/{l3}")]
        public ParLevel2DTO GetAutorizacoes(int? l1 = 0, int? l2 = 0, int? l3 = 0)
        {
            ParLevel1 level1 = new ParLevel1();
            ParLevel2DTO level2 = new ParLevel2DTO();
            using (var db = new SgqDbDevEntities())
            {
                if (l1 > 0)
                {
                    level1 = db.ParLevel1.FirstOrDefault(r => r.Id == l1);
                    if (l2 > 0)
                    {
                        level2 = Mapper.Map<ParLevel2DTO>(db.ParLevel2.FirstOrDefault(r => r.Id == l2));
                        level2.RegrasParamsLevel1(Mapper.Map<ParLevel1DTO>(level1));
                    }
                }
            }

            return level2;
        }

        [HttpPost]
        [Route("AddRemoveParHeaderLevel2")]
        public ParLevel2XHeaderField AddRemoveParHeaderLevel2(ParLevel2XHeaderField parLevel2XHeaderField)
        {
            return _paramdDomain.AddRemoveParHeaderLevel2(parLevel2XHeaderField);
        }

        public class ObjHeaderField
        {
            public int Id { get; set; }
            public int Group_Id { get; set; }
            public int ParLevel1_Id { get; set; }
            public string HeaderFieldGroup { get; set; }
        }

        /*
         * novo metodo getConsolidation
         * Autor: Gabriel Nunes
         * Data: 2017 04 28
         */
        public partial class ResultadoUmaColuna
        {
            //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            public string retorno { get; set; }

        }

        [HttpPost]
        [Route("AddRemoveParHeaderDuplicate")]
        public void AddRemoveParHeaderDuplicate(ObjHeaderField parHeaderField_id)
        {

            ParHeaderField parHeaderField;
            using (var db = new SgqDbDevEntities())
            {
                parHeaderField = db.ParHeaderField.FirstOrDefault(r => r.Id == parHeaderField_id.Id);

                string sql = "SELECT CAST(duplicate AS VARCHAR) as retorno FROM ParHeaderField WHERE Id = " + parHeaderField_id.Id.ToString();

                List<ResultadoUmaColuna> Lista1 = new List<ResultadoUmaColuna>();
                using (Factory factory = new Factory("DefaultConnection"))
                {
                    Lista1 = factory.SearchQuery<ResultadoUmaColuna>(sql).ToList();
                }

                if (Lista1.Count > 0)
                {
                    if (Lista1[0].retorno == "0")
                    {
                        db.Database.ExecuteSqlCommand("UPDATE ParHeaderField SET duplicate = 1 where id = " + parHeaderField_id.Id.ToString());

                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand("UPDATE ParHeaderField SET duplicate = 0 where id = " + parHeaderField_id.Id.ToString());

                    }
                }
            }
        }

        [HttpPost]
        [Route("GroupParHeaderField")]
        public ObjHeaderField GroupParHeaderField(ObjHeaderField objHeaderField)
        {
            using (var db = new SgqDbDevEntities())
            {
                var parentHeaderFieldGroup = db.ParLevel1XHeaderField.OrderByDescending(r => r.HeaderFieldGroup.Length).FirstOrDefault(r => (r.ParHeaderField_Id == objHeaderField.Group_Id || r.ParHeaderField_Id == objHeaderField.Id) && r.ParLevel1_Id == objHeaderField.ParLevel1_Id);
                if (!string.IsNullOrEmpty(parentHeaderFieldGroup.HeaderFieldGroup))
                {
                    var arrayId = parentHeaderFieldGroup.HeaderFieldGroup.Split('-').Select(m => Convert.ToInt32(m)).Distinct().ToList();
                    var listParHeaderField = db.ParLevel1XHeaderField.Where(r => arrayId.Contains(r.ParHeaderField_Id) && r.ParLevel1_Id == objHeaderField.ParLevel1_Id).ToList();

                    if (listParHeaderField.Count > 0)
                    {
                        arrayId.Add(objHeaderField.Id);
                        arrayId.Add(objHeaderField.Group_Id);
                        arrayId = arrayId.Distinct().ToList();
                        objHeaderField.HeaderFieldGroup = String.Join("-", arrayId);
                        db.Database.ExecuteSqlCommand($"UPDATE parlevel1xheaderfield SET HeaderFieldGroup = '{objHeaderField.HeaderFieldGroup}' where id in ({String.Join(",", arrayId)}) and parlevel1_id = {objHeaderField.ParLevel1_Id}");
                        return objHeaderField;
                    }
                }
                else if (objHeaderField.Id > 0 && objHeaderField.Group_Id > 0)
                {
                    objHeaderField.HeaderFieldGroup = objHeaderField.Group_Id + "-" + objHeaderField.Id;
                    db.Database.ExecuteSqlCommand($"UPDATE parlevel1xheaderfield SET HeaderFieldGroup = '{objHeaderField.HeaderFieldGroup}' where id in ({objHeaderField.Group_Id},{objHeaderField.Id}) and parlevel1_id = {objHeaderField.ParLevel1_Id}");
                    return objHeaderField;
                }
            }
            return null;
        }

        [HttpPost]
        [Route("UnlinkGroupParHeaderField")]
        public ObjHeaderField UnlinkGroupParHeaderField(ObjHeaderField objHeaderField)
        {
            using (var db = new SgqDbDevEntities())
            {
                var parentHeaderFieldGroup = db.ParLevel1XHeaderField.OrderByDescending(r => r.HeaderFieldGroup.Length).FirstOrDefault(r => r.ParHeaderField_Id == objHeaderField.Id && r.ParLevel1_Id == objHeaderField.ParLevel1_Id);
                if (!string.IsNullOrEmpty(parentHeaderFieldGroup.HeaderFieldGroup))
                {
                    var arrayId = parentHeaderFieldGroup.HeaderFieldGroup.Split('-').Select(m => Convert.ToInt32(m)).Distinct().ToList();
                    var listParHeaderField = db.ParLevel1XHeaderField.Where(r => arrayId.Contains(r.ParHeaderField_Id) && r.ParLevel1_Id == objHeaderField.ParLevel1_Id).ToList();

                    if (listParHeaderField.Count > 0)
                    {
                        arrayId.Remove(objHeaderField.Id);
                        arrayId = arrayId.Distinct().ToList();
                        objHeaderField.HeaderFieldGroup = arrayId.Count > 1 ? String.Join("-", arrayId) : "";
                        db.Database.ExecuteSqlCommand($"UPDATE parlevel1xheaderfield SET HeaderFieldGroup = '{objHeaderField.HeaderFieldGroup}' where id in ({String.Join(",", arrayId)}) and parlevel1_id = {objHeaderField.ParLevel1_Id}");
                        db.Database.ExecuteSqlCommand($"UPDATE parlevel1xheaderfield SET HeaderFieldGroup = null where id in ({objHeaderField.Id}) and parlevel1_id = {objHeaderField.ParLevel1_Id}");
                        return objHeaderField;
                    }
                }
            }
            return null;
        }

        [HttpGet]
        [Route("AddUnidadeDeMedida")]
        public ParMeasurementUnit AddUnidadeDeMedida(string valor)
        {
            var save = new ParMeasurementUnit() { Name = valor, AddDate = DateTime.Now, Description = string.Empty, IsActive = true };
            using (var db = new SgqDbDevEntities())
            {
                db.ParMeasurementUnit.Add(save);
                db.SaveChanges();
            }
            return save;
        }

        [HttpPost]
        [Route("AddEntradaBinaria")]
        public ExpandoObject AddEntradaBinaria(ParLevel3Bool parLevel3)
        {

            ParLevel3BoolFalse boolFalse = new ParLevel3BoolFalse() { Name = parLevel3.nameFalse, AddDate = DateTime.Now, IsActive = true };
            ParLevel3BoolTrue boolTrue = new ParLevel3BoolTrue() { Name = parLevel3.nameTrue, AddDate = DateTime.Now, IsActive = true };

            using (var db = new SgqDbDevEntities())
            {
                db.ParLevel3BoolTrue.Add(boolTrue);
                db.ParLevel3BoolFalse.Add(boolFalse);
                db.SaveChanges();
            }

            dynamic Retorno = new ExpandoObject();

            Retorno.True = boolTrue;
            Retorno.False = boolFalse;

            return Retorno;

        }

        [HttpPost]
        [Route("GetListLevel1")]
        public List<ParLevel1DTO> GetListLevel1()
        {
            return _baseParLevel1.GetAllNoLazyLoad().Where(P => P.IsActive).ToList();
        }

        [HttpPost]
        [Route("GetListLevel2")]
        public List<ParLevel2DTO> GetListLevel2()
        {
            return _baseParLevel2.GetAllNoLazyLoad().Where(P => P.IsActive).ToList();

        }

        [HttpPost]
        [Route("GetListLevel3")]
        public List<ParLevel3DTO> GetListLevel3()
        {
            return _baseParLevel3.GetAllNoLazyLoad().Where(P => P.IsActive).ToList();
        }

        [HttpPost]
        [Route("GetListLevel2VinculadoLevel1/{level1Id}")]
        public List<ParLevel2DTO> GetListLevel2VinculadoLevel1(int level1Id)
        {
            var list = new List<ParLevel2DTO>();

            using (var db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var result = db.ParLevel3Level2Level1.Where(r => r.ParLevel1_Id == level1Id).Select(r => r.ParLevel3Level2.ParLevel2).ToList().GroupBy(r => r.Id);
                list = Mapper.Map<List<ParLevel2DTO>>(result.Select(r => r.First()));
            }

            return list;
        }

        [HttpPost]
        [Route("GetListLevel2VinculadoLevel1")]
        public List<ParLevel2DTO> GetListLevel2VinculadoLevel1([FromBody] ModelForm model)
        {
            var list = new List<ParLevel2DTO>();

            using (var db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var result = db.ParLevel3Level2Level1.Where(r => model.Level1IdArr.Contains(r.ParLevel1_Id)).Select(r => r.ParLevel3Level2.ParLevel2).ToList().GroupBy(r => r.Id);
                list = Mapper.Map<List<ParLevel2DTO>>(result.Select(r => r.First()));
            }

            return list;
        }

        [HttpPost]
        [Route("GetListLevel3VinculadoLevel2/{level2Id}")]
        public List<ParLevel3DTO> GetListLevel3VinculadoLevel2(int level2Id)
        {
            var list = new List<ParLevel3DTO>();

            using (var db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var result = db.ParLevel3Level2.Where(r => r.ParLevel2_Id == level2Id).Select(r => r.ParLevel3).ToList().GroupBy(r => r.Id);
                list = Mapper.Map<List<ParLevel3DTO>>(result.Select(r => r.First()));
            }

            return list;
        }

        [HttpPost]
        [Route("GetListLevel3VinculadoLevel2")]
        public List<ParLevel3DTO> GetListLevel3VinculadoLevel2([FromBody] ModelForm model)
        {
            var list = new List<ParLevel3DTO>();

            using (var db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                if (model.Level1IdArr.Length > 0)
                {
                    var result = db.ParLevel3Level2Level1.Where(r => model.Level2IdArr.Contains(r.ParLevel3Level2.ParLevel2_Id) && model.Level1IdArr.Contains(r.ParLevel1_Id)).Select(r => r.ParLevel3Level2.ParLevel3).ToList().GroupBy(r => r.Id).Select(r => r.First());
                    //var result = db.ParLevel3Level2.Where(r => r.ParLevel2_Id == level2.Id).Select(r => r.ParLevel3).ToList().GroupBy(r => r.Id).Select(r => r.First());
                    list = Mapper.Map<List<ParLevel3DTO>>(result);
                }
                else
                {
                    var result = db.ParLevel3Level2.Where(r => model.Level2IdArr.Contains(r.ParLevel2_Id)).Select(r => r.ParLevel3).ToList().GroupBy(r => r.Id);
                    list = Mapper.Map<List<ParLevel3DTO>>(result.Select(r => r.First()));
                }
            }

            return list;
        }

        [HttpPost]
        [Route("GetListLevel3VinculadoLevel2Level1/{level1Id}/{level2Id}")]
        public List<ParLevel3DTO> GetListLevel3VinculadoLevel2Level1(int level1Id, int level2Id)
        {
            var list = new List<ParLevel3DTO>();

            using (var db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var result = db.ParLevel3Level2Level1.Where(r => r.ParLevel3Level2.ParLevel2_Id == level2Id && r.ParLevel1_Id == level1Id).Select(r => r.ParLevel3Level2.ParLevel3).ToList().GroupBy(r => r.Id).Select(r => r.First());
                //var result = db.ParLevel3Level2.Where(r => r.ParLevel2_Id == level2.Id).Select(r => r.ParLevel3).ToList().GroupBy(r => r.Id).Select(r => r.First());
                list = Mapper.Map<List<ParLevel3DTO>>(result);
            }

            return list;
        }

        [HttpPost]
        [Route("atualizaCabecalho")]
        public ParHeaderFieldDTO atualizaCabecalho([FromBody] ParamsViewModel parr)
        {
            try
            {
                if (parr.paramsDto.parHeaderFieldDto.ParFieldType_Id == 1 || parr.paramsDto.parHeaderFieldDto.ParFieldType_Id == 3)
                {
                    List<ParMultipleValuesDTO> lista = parr.paramsDto.parHeaderFieldDto.ParMultipleValues;
                    int count = 0;
                    while (count < lista.Count())
                    {
                        ParMultipleValuesDTO mv = lista[count];
                        _baseParMultipleValues.AddOrUpdate(mv);
                        count++;
                    }
                }

                parr.paramsDto.parHeaderFieldDto.IsActive = true;
                return _baseParHeaderField.AddOrUpdate(parr.paramsDto.parHeaderFieldDto);
            }
            catch (DbEntityValidationException e)
            {
                var listaDeErros = e.EntityValidationErrors;
                StringBuilder mensagem = new StringBuilder();
                foreach (var item in listaDeErros)
                {
                   var erros = item.ValidationErrors;
                   foreach(var erro in erros)
                    {
                        mensagem.Append(erro.ErrorMessage).Append(" ");
                    }
                }
                throw new Exception(mensagem.ToString());
            }
        }

        [HttpGet]
        [Route("GetResource/{language}")]
        public IEnumerable<DictionaryEntry> GetResource(string language)
        {
            if (language.Equals("pt-br") || (language.Equals("default") && GlobalConfig.LanguageBrasil)) //se portugues
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
            }
            else if (language.Equals("en-us") || (language.Equals("default") && GlobalConfig.LanguageEUA))//inglês
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("");
            }

            System.Resources.ResourceManager resourceManager = Resources.Resource.ResourceManager;

            return resourceManager.GetResourceSet(
                Thread.CurrentThread.CurrentUICulture, true, false).Cast<DictionaryEntry>();
        }

        [HttpPost]
        [Route("GetLevel3PorUnidadeRecravacaoDdl")]
        public List<ParLevel3Value> GetLevel3PorUnidadeRecravacaoDdl([FromBody] int level3Id)
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Re‌​ferenceLoopHandling = ReferenceLoopHandling.Ignore;
            using (var db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var level3 = db.ParLevel3Value.Include("ParMeasurementUnit").Where(r => r.ParLevel3_Id == level3Id).ToList();
                return level3;
            }
        }
    }

    public class ParLevel3Bool
    {
        public string nameTrue { get; set; }
        public string nameFalse { get; set; }
    }
}
