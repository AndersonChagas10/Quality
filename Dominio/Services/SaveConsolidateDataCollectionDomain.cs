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

                //foreach (var v in obj.ListToSave) {
                //    var consildatedLelve01ListDTO1 = new List<ConsolidationLevel01DTO>();
                //    consildatedLelve01ListDTO1 = Mapper.Map<List<ConsolidationLevel01DTO>>(v);
                //}

                foreach (var i in obj.ListToSave)
                {

                    ConsolidationLevel01 level01Consolidation;
                    level01Consolidation = Mapper.Map<ConsolidationLevel01>(i);
                    level01Consolidation.ConsolidationLevel02 = Mapper.Map<List<ConsolidationLevel02>>(i.consolidationLevel02DTO);

                    //foreach (var b in i.ConsolidationLevel02)
                    //{
                    //    b.CollectionLevel02 = i.collectionLevel02DTO;
                    //    foreach (var c in i.collectionLevel02DTO)
                    //    {

                    //    // i.consolidationLevel02DTO
                    //        c.CollectionLevel03 = c.collectionLevel03DTO;
                    //    }
                    //}

                    PrencheFeedaBackPt1(out saving, i, out level01Consolidation);
                    level01Consolidation = SalvaConsolidationLevel01(i, level01Consolidation);
                    ConsolidationLevel02 level02Consolidation;

                    foreach (var j in i.consolidationLevel02DTO)
                    {
                        foreach (var x in i.collectionLevel02DTO)
                        {
                            level02Consolidation = SalvaConsolidationLevel02(i, level01Consolidation, j, x);
                            CollectionLevel02 collectionLevel02 = SalvaCollectionLevel02(level01Consolidation, level02Consolidation, x, x.collectionLevel03DTO, obj);
                            SalvaCorrectiveAction(obj, x, collectionLevel02);
                            SalvaCollectionLevel03(x, collectionLevel02);
                        }
                    }
                }

                #endregion

                #region Retorno

                long elapsedMs = ParaCronometro(watch);
                GenericReturn<SyncDTO> feedback = PreencheFeedBackPt2(obj, saving, elapsedMs);
                return feedback;
                //idSaved aqui é o level01 que foi salvo.
                #endregion

            }
            catch (Exception e)
            {
                #region Trata Exceção de forma Geral.

                return new GenericReturn<SyncDTO>(e, "Cannot sync Data." + e.Message, obj);

                #endregion
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
                var elemento = _baseRepoCollectionHtml.GetAll().FirstOrDefault(r => r.UnitId == objToSync.idUnidade && r.Shift == objToSync.CollectionHtml.Shift);
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

            var feedback = new GenericReturn<SyncDTO>("Susscess! All Data Saved for: " + savingText, obj); //in: " + elapsedMs + " ms,
            feedback.IdSaved = saving;
            return feedback;
        }

        private void SalvaCollectionLevel03(CollectionLevel02DTO x, CollectionLevel02 collectionLevel02)
        {
            List<CollectionLevel03> collectionLevel03Old = new List<CollectionLevel03>();
            List<CollectionLevel03> cll3 = new List<CollectionLevel03>();
            var isAlter = false;
            foreach (var y in x.collectionLevel03DTO)
            {
                y.CollectionLevel02Id = collectionLevel02.Id;
                if (y.Id > 0)
                {
                    var old = _baseRepoCollectionL3.GetById(y.Id);
                    //alterdate, conformedIs, value, valueText, Duplicated
                    //if (y.ConformedIs != old.ConformedIs)
                    //{
                    //    isAlter = true;
                    //    old.ConformedIs = y.ConformedIs;
                    //}
                    //if (y.Value != old.Value)
                    //{
                    //    isAlter = true;
                    //    old.Value = y.Value;
                    //}
                    //if (y.ValueText != old.ValueText)
                    //{
                    //    isAlter = true;
                    //    old.ValueText = y.ValueText;
                    //}
                    //if (y.Duplicated != old.Duplicated)
                    //{
                    //    isAlter = true;
                    //    old.Duplicated = y.Duplicated;
                    //}
                    //if (isAlter)
                    //{
                    //}
                    isAlter = true;
                    old.ConformedIs = y.ConformedIs;
                    old.Value = y.Value;
                    old.ValueText = y.ValueText;
                    old.Duplicated = y.Duplicated;
                    collectionLevel03Old.Add(old);
                }
            }

            if (isAlter)
            {
                //collectionLevel03Old.ForEach(r => r.CollectionLevel02 = null);
                _baseRepoCollectionL3.AddOrUpdateAll(collectionLevel03Old);
            }
            else
            {
                cll3 = Mapper.Map<List<CollectionLevel03>>(x.collectionLevel03DTO);

                #region Coloca flag duplicado.
                _collectionLevel03RepositoryGET.SetDuplicated(cll3, collectionLevel02);
                #endregion

                //Observar por que alguns elemento vem não nulos, o EF buga ao dar o update pois essa property não esta attached.
                cll3.ForEach(r => r.CollectionLevel02 = null);

                _baseRepoCollectionL3.AddAll(cll3);

            }
            x.collectionLevel03DTO = Mapper.Map<List<CollectionLevel03DTO>>(cll3);
            //var counter = 0;
            //foreach (var xx in x.collectionLevel03DTO)
            //{
            //    xx.Id = cll3[counter].Id;
            //    counter++;
            //}
        }

        private void SalvaCorrectiveAction(SyncDTO obj, CollectionLevel02DTO x, CollectionLevel02 collectionLevel02)
        {
            if (x.CorrectiveActionId > 0)
            {
                var CaToSaveDTO = obj.ListToSaveCA.FirstOrDefault(z => z.idcorrectiveaction == x.CorrectiveActionId);
                var CA = Mapper.Map<CorrectiveAction>(CaToSaveDTO);
                CA.CollectionLevel02Id = collectionLevel02.Id;
                _baseRepoCorrectiveAction.AddOrUpdate(CA);
                x.CorrectiveActionSaved = Mapper.Map<CorrectiveActionDTO>(CA);
            }
        }

        private CollectionLevel02 SalvaCollectionLevel02(ConsolidationLevel01 level01Consolidation,
            ConsolidationLevel02 level02Consolidation, CollectionLevel02DTO x, List<CollectionLevel03DTO> maldito,
            SyncDTO obj)
        {
            x.Level01Id = level01Consolidation.Level01Id;
            x.ConsolidationLevel02Id = level02Consolidation.Id;
            CollectionLevel02 collectionLevel02;

            #region Coloca flag duplicado.
            //MOCK

            //if (x.Id > 0)
            //{
            //    if (x.CorrectiveActionId > 0)
            //    {
            //        var CaToSaveDTO = obj.ListToSaveCA.FirstOrDefault(z => z.idcorrectiveaction == x.CorrectiveActionId);
            //        if (CaToSaveDTO != null)
            //        {
            //            x.CorrectiveAction = new List<CorrectiveActionDTO>();
            //            x.CorrectiveAction.Add(CaToSaveDTO);
            //        }
            //        //MOCK
            //        foreach (var i in maldito)
            //            i.CollectionLevel02Id = x.Id;
            //        x.CollectionLevel03 = maldito;
            //    }
            //}

            collectionLevel02 = Mapper.Map<CollectionLevel02>(x);
            _collectionLevel02RepositoryGET.SetDuplicated(collectionLevel02);
            collectionLevel02.CollectionLevel03 = null;
            #endregion
            //_baseRepoCollectionL2.AddOrUpdate(collectionLevel02);
            if (x.Id == 0)
            {
                _baseRepoCollectionL2.Add(collectionLevel02);
            }
            else
            {
                _collectionLevel02Repo.UpdateCollectionLevel02(collectionLevel02);
            }
            x.Id = collectionLevel02.Id;
            return collectionLevel02;
        }

        private ConsolidationLevel02 SalvaConsolidationLevel02(ConsolidationLevel01DTO i, ConsolidationLevel01 level01Consolidation, ConsolidationLevel02DTO j, CollectionLevel02DTO x)
        {
            ConsolidationLevel02 level02Consolidation;
            j.Level01ConsolidationId = level01Consolidation.Id;
            j.Level02Id = x.Level02Id;
            level02Consolidation = Mapper.Map<ConsolidationLevel02>(j);

            #region Procura consolidação existente
            var todasConsolidations = _consolidationLevel02RepositoryGET.GetExistentLevel02Consollidation(level02Consolidation, level01Consolidation);
            #endregion
            //Se não encontrar nenhuma consolidação com o level02Id ja criada, ele cria uma.
            var adicionar = todasConsolidations ?? level02Consolidation;
            //_baseRepoConsolidationL2.AddOrUpdate(adicionar);
            _baseRepoConsolidationL2.AddOrUpdate(adicionar);
            i.Id = level02Consolidation.Id;
            return adicionar;
        }

        private ConsolidationLevel01 SalvaConsolidationLevel01(ConsolidationLevel01DTO i, ConsolidationLevel01 level01Consolidation)
        {
            #region Procura Consolidação existente
            var todasConsolidations = _consolidationLevel01RepositoryGET.GetExistentLevel01Consollidation(level01Consolidation);
            #endregion

            var adicionar = todasConsolidations ?? level01Consolidation;
            _baseRepoConsolidationL1.AddOrUpdate(adicionar);
            level01Consolidation = adicionar;
            //_baseRepoConsolidationL1.AddOrUpdate(level01Consolidation);
            i.Id = level01Consolidation.Id;
            return adicionar;
        }

        private static void PrencheFeedaBackPt1(out int saving, ConsolidationLevel01DTO i, out ConsolidationLevel01 level01Consolidation)
        {
            level01Consolidation = Mapper.Map<ConsolidationLevel01>(i);
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