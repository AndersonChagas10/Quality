using AutoMapper;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;

namespace Dominio.Services
{
    public class SaveConsolidateDataCollectionDomain : ISaveConsolidateDataCollectionDomain
    {

        #region Variaveis

        private IBaseRepository<ConsolidationLevel01> _baseRepoConsolidationL1;
        private IBaseRepository<ConsolidationLevel02> _baseRepoConsolidationL2;
        private IBaseRepository<CollectionLevel02> _baseRepoCollectionL2;
        private ICollectionLevel02Repo _collectionLevel02Repo;
        private IBaseRepository<CollectionLevel03> _baseRepoCollectionL3;
        private IBaseRepository<CollectionHtml> _baseRepoCollectionHtml;
        private IBaseRepository<CorrectiveAction> _baseRepoCorrectiveAction;
        private IGetDataResultRepository<ConsolidationLevel01> _consolidationLevel01RepositoryGET;
        private IGetDataResultRepository<ConsolidationLevel02> _consolidationLevel02RepositoryGET;
        private IGetDataResultRepository<CollectionLevel02> _collectionLevel02RepositoryGET;
        private IGetDataResultRepository<CollectionLevel03> _collectionLevel03RepositoryGET;
        private IGetDataResultRepository<CollectionHtml> _baseRepoCollectionHtmlGET;

        #endregion

        #region Construtor

        public SaveConsolidateDataCollectionDomain(
            IBaseRepository<ConsolidationLevel01> baseRepoConsolidationL1,
            IBaseRepository<ConsolidationLevel02> baseRepoConsolidationL2,
            IBaseRepository<CollectionLevel02> baseRepoCollectionL2,
            IBaseRepository<CollectionLevel03> baseRepoCollectionL3,
            IBaseRepository<CollectionHtml> baseRepoCollectionHtml,
            IBaseRepository<CorrectiveAction> baseRepoCorrectiveAction,
            IGetDataResultRepository<ConsolidationLevel01> consolidationLevel01RepositoryGET,
            IGetDataResultRepository<ConsolidationLevel02> consolidationLevel02RepositoryGET,
            IGetDataResultRepository<CollectionLevel02> collectionLevel02RepositoryGET,
            IGetDataResultRepository<CollectionLevel03> collectionLevel03RepositoryGET,
            IGetDataResultRepository<CollectionHtml> baseRepoCollectionHtmlGET,
            ICollectionLevel02Repo collectionLevel02Repo
            )
        {
            _collectionLevel02Repo = collectionLevel02Repo;
            _consolidationLevel01RepositoryGET = consolidationLevel01RepositoryGET;
            _consolidationLevel02RepositoryGET = consolidationLevel02RepositoryGET;
            _collectionLevel02RepositoryGET = collectionLevel02RepositoryGET;
            _collectionLevel03RepositoryGET = collectionLevel03RepositoryGET;
            _baseRepoCollectionHtmlGET = baseRepoCollectionHtmlGET;
            _baseRepoCollectionHtml = baseRepoCollectionHtml;
            _baseRepoConsolidationL1 = baseRepoConsolidationL1;
            _baseRepoConsolidationL2 = baseRepoConsolidationL2;
            _baseRepoCollectionL2 = baseRepoCollectionL2;
            _baseRepoCollectionL3 = baseRepoCollectionL3;
            _baseRepoCorrectiveAction = baseRepoCorrectiveAction;
        }

        #endregion

        public GenericReturn<SyncDTO> SetDataToSincyAuditConsolidated(SyncDTO obj)
        {

            try
            {

                #region Validação e criação de objetos.

                if (obj.Root.Count == 0)
                    throw new ExceptionHelper("Impossible to Sync data. The Sync list is empty.");

                obj.ListToSave = new List<ConsolidationLevel01DTO>();
                obj.ListToSaveCA = new List<CorrectiveActionDTO>();

                CriaListaDeObjectsToSave(obj);

                #endregion
                Stopwatch watch = IniciaCronometro();
                #region Loop Save

                var saving = 0;

                foreach (var i in obj.ListToSave)
                {

                    ////Consol L1
                    //ConsolidationLevel01 level01Consolidation = Mapper.Map<ConsolidationLevel01>(i);
                    //_baseRepoConsolidationL1.AddOrUpdate(level01Consolidation);
                    //level01Consolidation.ConsolidationLevel02 = new List<ConsolidationLevel02>();

                    //List<CollectionLevel02> CollectionLevel02 = Mapper.Map<List<CollectionLevel02>>(i.collectionLevel02DTO);

                    //foreach (var b in CollectionLevel02)
                    //{

                    //    //Consol L2
                    //    var collectionLevel02DestaConsilidacao = b;
                    //    var consolidationLevel02 = new ConsolidationLevel02();
                    //    consolidationLevel02.Level02Id = b.Level02Id;
                    //    collectionLevel02DestaConsilidacao.Level01Id = level01Consolidation.Level01Id;
                    //    collectionLevel02DestaConsilidacao.CollectionLevel03 = Mapper.Map<List<CollectionLevel03>>(b.collectionLevel03DTO);

                    //    consolidationLevel02.Level01ConsolidationId = level01Consolidation.Id;
                    //    consolidationLevel02.CollectionLevel02 = CollectionLevel02;
                    //    consolidationLevel02.Level02Id = b.Level02Id;
                    //    consolidationLevel02.UnitId = level01Consolidation.UnitId;
                    //    _baseRepoConsolidationL2.AddOrUpdate(consolidationLevel02);

                    //    //Coll L2
                    //    collectionLevel02DestaConsilidacao.ConsolidationLevel02Id = consolidationLevel02.Id;
                    //    _baseRepoCollectionL2.AddOrUpdate(collectionLevel02DestaConsilidacao);

                    //    //Coll L  3
                    //    collectionLevel02DestaConsilidacao.CollectionLevel03.All(r => r.CollectionLevel02Id == collectionLevel02DestaConsilidacao.Id);
                    //    _baseRepoCollectionL3.AddOrUpdateAll(collectionLevel02DestaConsilidacao.CollectionLevel03);
                    //}

                    ConsolidationLevel01 level01Consolidation = Mapper.Map<ConsolidationLevel01>(i);
                    PrencheFeedaBackPt1(out saving, level01Consolidation);
                    level01Consolidation = SalvaConsolidationLevel01(level01Consolidation);
                    ConsolidationLevel02 level02Consolidation;

                    foreach (var consolidationLevel02Dto in i.consolidationLevel02DTO)
                    {
                        foreach (var collectionLevel02Dto in i.collectionLevel02DTO)
                        {
                            level02Consolidation = SalvaConsolidationLevel02(level01Consolidation, consolidationLevel02Dto, collectionLevel02Dto.Level02Id);
                            CollectionLevel02 collectionLevel02 = SalvaCollectionLevel02(collectionLevel02Dto, level01Consolidation.Level01Id, level02Consolidation.Id);
                            SalvaCorrectiveAction(obj.ListToSaveCA, collectionLevel02Dto, collectionLevel02.Id);
                            SalvaCollectionLevel03(collectionLevel02Dto, collectionLevel02);
                        }
                    }
                }

                #endregion

                #region Retorno

                long elapsedMs = ParaCronometro(watch);
                GenericReturn<SyncDTO> feedback = PreencheFeedBackPt2(obj, saving, elapsedMs);
                return feedback;
                #endregion

            }
            catch (Exception e)
            {
                return new GenericReturn<SyncDTO>(e, "Cannot sync Data." + e.Message, obj);
            }
            finally
            {
                #region NotImplemented

                #endregion
            }

        }

        public GenericReturn<SyncDTO> SaveHtml(SyncDTO objToSync)
        {

            try
            {
                var html = new CollectionHtml()
                {
                    Html = objToSync.html,
                    Period = objToSync.CollectionHtml.Period,
                    Shift = objToSync.CollectionHtml.Shift,
                    CollectionDate = objToSync.CollectionHtml.CollectionDate,
                    UnitId = objToSync.CollectionHtml.UnitId
                };
                var elemento = _baseRepoCollectionHtml.GetAll()
                    .FirstOrDefault(r => r.UnitId == objToSync.idUnidade 
                        && r.Shift == objToSync.CollectionHtml.Shift 
                        //&& r.Period == objToSync.CollectionHtml.Period
                    );
                if (elemento.IsNull() && (objToSync.html.IsNull()))
                    return new GenericReturn<SyncDTO>("Susscess! Sync.");

                if (elemento.IsNotNull())
                {
                    html.Id = elemento.Id;
                    html.AddDate = elemento.AddDate;
                    _baseRepoCollectionHtml.Dettach(elemento);
                }


                _baseRepoCollectionHtml.AddOrUpdate(html);

                return new GenericReturn<SyncDTO>("Susscess! Sync.");
            }
            catch (Exception e)
            {
                #region Trata Exceção de forma Geral.

                return new GenericReturn<SyncDTO>(e, "Cannot sync HTML.");

                #endregion
            }
            finally
            {
                #region NotImplemented

                #endregion
            }

        }

        #region Auxiliares e Validações

        private static void CriaListaDeObjectsToSave(SyncDTO obj)
        {
            //Contrutor que possui validação do objeto, dentro deste contrutor devem constar as RNs para cada objeto a ser inserido no BD, 
            //caso haja incosistencia, o mesmo deve expedir uma exception, ou exceptionhelper parando a execução, 
            //e prevenindo a entrada de arquivos não válidos no DB.
            foreach (var i in obj.Root)
            {
                obj.ListToSave.Add(i.ValidateAndCreateDtoConsolidationLevel01DTO());
                if (i.correctiveactioncomplete != null)
                    obj.ListToSaveCA = i.makeCA();
            }
        }

        private static long ParaCronometro(Stopwatch watch)
        {
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return elapsedMs;
        }

        private static Stopwatch IniciaCronometro()
        {
            return Stopwatch.StartNew();
        }

        private static GenericReturn<SyncDTO> PreencheFeedBackPt2(SyncDTO obj, int saving, long elapsedMs)
        {
            var savingText = "";
            if (saving == 3) { }
            savingText = "CFF (Cut, Fold and Flaps)";
            if (saving == 1)
                savingText = "HTP";
            if (saving == 2)
                savingText = "Carcass Contamination Audit";

            var feedback = new GenericReturn<SyncDTO>("Susscess! All Data Saved for: " + savingText + ", in: " + elapsedMs + " ms", obj); //,
            feedback.IdSaved = saving;
            return feedback;
        }

        private void SalvaCollectionLevel03(CollectionLevel02DTO collectionLevel02DTO, CollectionLevel02 collectionLevel02Id)
        {

            collectionLevel02DTO.collectionLevel03DTO.Select(c => { c.CollectionLevel02Id = collectionLevel02Id.Id; return c; }).ToList();
            List<CollectionLevel03> collectionLevel03 = Mapper.Map<List<CollectionLevel03>>(collectionLevel02DTO.collectionLevel03DTO);

            //region Coloca flag duplicado.
            if(collectionLevel03.Any(r=>r.Id == 0))
                _collectionLevel03RepositoryGET.SetDuplicated(collectionLevel03, collectionLevel02Id);

            //foreach(var i in collectionLevel03)

            _baseRepoCollectionL3.AddOrUpdateAll(collectionLevel03);
            collectionLevel02DTO.collectionLevel03DTO = Mapper.Map<List<CollectionLevel03DTO>>(collectionLevel03);
            
            //List<CollectionLevel03> collectionLevel03Old = new List<CollectionLevel03>();
            //List<CollectionLevel03> cll3 = new List<CollectionLevel03>();
            //var isAlter = false;
            //foreach (var y in x.collectionLevel03DTO)
            //{
            //    y.CollectionLevel02Id = collectionLevel02.Id;
            //    if (y.Id > 0)
            //    {
            //        var old = _baseRepoCollectionL3.GetById(y.Id);

            //        isAlter = true;
            //        old.ConformedIs = y.ConformedIs;
            //        old.Value = y.Value;
            //        old.ValueText = y.ValueText;
            //        old.Duplicated = y.Duplicated;
            //        collectionLevel03Old.Add(old);
            //    }
            //}

            //if (isAlter)
            //{
            //    //collectionLevel03Old.ForEach(r => r.CollectionLevel02 = null);
            //    _baseRepoCollectionL3.AddOrUpdateAll(collectionLevel03Old);
            //}
            //else
            //{
            //    cll3 = Mapper.Map<List<CollectionLevel03>>(x.collectionLevel03DTO);

            //   

            //    //Observar por que alguns elemento vem não nulos, o EF buga ao dar o update pois essa property não esta attached.
            //    cll3.ForEach(r => r.CollectionLevel02 = null);

            //    _baseRepoCollectionL3.AddAll(cll3);

            //}
            //x.collectionLevel03DTO = Mapper.Map<List<CollectionLevel03DTO>>(cll3);
        }

        private void SalvaCorrectiveAction(List<CorrectiveActionDTO> objListToSaveCA, CollectionLevel02DTO x, int collectionLevel02Id)
        {
            if (x.CorrectiveActionId > 0)
            {
                if (_baseRepoCorrectiveAction.GetById(x.CorrectiveActionId) == null)
                {
                    var CaToSaveDTO = objListToSaveCA.FirstOrDefault(z => z.idcorrectiveaction == x.CorrectiveActionId);
                    var CA = Mapper.Map<CorrectiveAction>(CaToSaveDTO);
                    CA.CollectionLevel02Id = collectionLevel02Id;

                    CA.UserSgq = null;
                    CA.UserSgq1 = null;
                    CA.UserSgq2 = null;
                    CA.CollectionLevel02 = null;

                    _baseRepoCorrectiveAction.AddOrUpdate(CA);
                    x.CorrectiveActionSaved = Mapper.Map<CorrectiveActionDTO>(CA);
                }
            }
        }

        private CollectionLevel02 SalvaCollectionLevel02(CollectionLevel02DTO collectionLevel02DTO, int level01Id
            , int level02ConsolidationId)
        {
            collectionLevel02DTO.Level01Id = level01Id;
            collectionLevel02DTO.ConsolidationLevel02Id = level02ConsolidationId;
            CollectionLevel02 collectionLevel02 = Mapper.Map<CollectionLevel02>(collectionLevel02DTO);

         

            //Coloca flag duplicado.
            if (collectionLevel02.Id == 0)
                _collectionLevel02RepositoryGET.SetDuplicated(collectionLevel02);

            ////_baseRepoCollectionL2.AddOrUpdate(collectionLevel02);
            //if (collectionLevel02DTO.Id == 0)
            //{
            //    _baseRepoCollectionL2.Add(collectionLevel02);
            //}
            //else
            //{
            //    _collectionLevel02Repo.addor(collectionLevel02);
            //}

            collectionLevel02.CollectionLevel03 = null;
            collectionLevel02.Level01 = null;
            collectionLevel02.Level02 = null;
            collectionLevel02.UserSgq = null;
            collectionLevel02.ConsolidationLevel02 = null;

            _baseRepoCollectionL2.AddOrUpdate(collectionLevel02);

            collectionLevel02DTO.Id = collectionLevel02.Id;
            return collectionLevel02;
        }

        private ConsolidationLevel02 SalvaConsolidationLevel02(ConsolidationLevel01 level01Consolidation, ConsolidationLevel02DTO consolidationLevel02DTO, int level02Id)
        {
            ConsolidationLevel02 level02Consolidation = Mapper.Map<ConsolidationLevel02>(consolidationLevel02DTO);
            level02Consolidation.Level01ConsolidationId = level01Consolidation.Id;
            level02Consolidation.Level02Id = level02Id;
            
            // Procura consolidação existente
            var consolidacaoExistente = _consolidationLevel02RepositoryGET.GetExistentLevel02Consollidation(level02Consolidation, level01Consolidation);

            //AGREGA DADOS A CONSOLIDACAO EXISTENTE AQUI
            if (consolidacaoExistente != null)
            {
                level02Consolidation = consolidacaoExistente;
            }

            //Salva / update
            _baseRepoConsolidationL2.AddOrUpdate(level02Consolidation);
            return level02Consolidation;
        }

        private ConsolidationLevel01 SalvaConsolidationLevel01(ConsolidationLevel01 level01Consolidation)
        {
            //Procura Consolidação existente
            var consolidacaoLevel01Existente = _consolidationLevel01RepositoryGET.GetExistentLevel01Consollidation(level01Consolidation);

            //AGREGA DADOS A CONSOLIDACAO EXISTENTE AQUI
            if (consolidacaoLevel01Existente != null)
            {
                level01Consolidation = consolidacaoLevel01Existente;
            }
            //Salva / update
            _baseRepoConsolidationL1.AddOrUpdate(level01Consolidation);
            return level01Consolidation;
        }

        private static void PrencheFeedaBackPt1(out int saving, ConsolidationLevel01 level01Consolidation)
        {
            
            if (level01Consolidation.Level01Id == 3) { }
            saving = 3;
            if (level01Consolidation.Level01Id == 1)
                saving = 1;
            if (level01Consolidation.Level01Id == 2)
                saving = 2;
        }

        #endregion
    }
}