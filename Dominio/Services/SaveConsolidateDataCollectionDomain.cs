using AutoMapper;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dominio.Services
{
    public class SaveConsolidateDataCollectionDomain : ISaveConsolidateDataCollectionDomain
    {
        private IBaseRepository<ConsolidationLevel01> _baseRepoConsolidationL1;
        private IBaseRepository<ConsolidationLevel02> _baseRepoConsolidationL2;
        private IBaseRepository<CollectionLevel02> _baseRepoCollectionL2;
        private IBaseRepository<CollectionLevel03> _baseRepoCollectionL3;
        private IBaseRepository<CollectionHtml> _baseRepoCollectionHtml;
        private IBaseRepository<CorrectiveAction> _baseRepoCorrectiveAction;
        private IGetDataResultRepository<ConsolidationLevel01> _consolidationLevel01RepositoryGET;
        private IGetDataResultRepository<ConsolidationLevel02> _consolidationLevel02RepositoryGET;
        private IGetDataResultRepository<CollectionLevel02> _collectionLevel02RepositoryGET;
        private IGetDataResultRepository<CollectionLevel03> _collectionLevel03RepositoryGET;
        private IGetDataResultRepository<CollectionHtml> _baseRepoCollectionHtmlGET;

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
            IGetDataResultRepository<CollectionHtml> baseRepoCollectionHtmlGET
            )
        {
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

        public GenericReturn<SyncDTO> SetDataToSincyAuditConsolidated(SyncDTO obj)
        {

            try
            {

                #region Validação e criação de objetos.

                if (obj.Root.Count == 0)
                    throw new ExceptionHelper("Impossible to Sync data. The Sync list is empty.");

                var ListToSave = new List<ConsolidationLevel01DTO>();
                var ListToSaveCA = new List<CorrectiveActionDTO>();
                //Contrutor que possui validação do objeto, dentro deste contrutor devem constar as RNs para cada objeto a ser inserido no BD, 
                //caso haja incosistencia, o mesmo deve expedir uma exception, ou exceptionhelper parando a execução, 
                //e prevenindo a entrada de arquivos não válidos no DB.
                foreach (var i in obj.Root)
                {
                    ListToSave.Add(i.ValidateAndCreateDtoConsolidationLevel01DTO());
                    if (i.correctiveactioncomplete != null)
                        ListToSaveCA.Add(i.makeCA());
                }

                #endregion

                #region Salvando os 5 objetos em Banco de Dados.
                //Iniciar Cronometro.
                var watch = Stopwatch.StartNew();

                #region Loop Save
                var saving = "";
                foreach (var i in ListToSave)
                {
                    //ConsolidationLevel01 existentLevel1 = _baseRepoConsolidationL1.CheckForExistent();

                    var level01Consolidation = Mapper.Map<ConsolidationLevel01>(i);
                    if (level01Consolidation.Level01Id == 3)
                        saving = "CFF (Cut, Fold and Flaps)";
                    if (level01Consolidation.Level01Id == 1)
                        saving = "HTP";
                    if (level01Consolidation.Level01Id == 2)
                        saving = "Carcass Contamination Audit";

                    #region Procura Consolidação existente
                    //var idTempLevel01ConsollidationId = _consolidationLevel01RepositoryGET.GetExistentLevel01Consollidation(level01Consolidation);
                    //level01Consolidation.Id = idTempLevel01ConsollidationId;
                    #endregion

                    _baseRepoConsolidationL1.AddOrUpdate(level01Consolidation);

                    ConsolidationLevel02 level02Consolidation;
                    foreach (var j in i.consolidationLevel02DTO)
                    {

                        //Procura Consolidação existente

                        j.Level01ConsolidationId = level01Consolidation.Id;
                        level02Consolidation = Mapper.Map<ConsolidationLevel02>(j);

                        #region Procura Consolidação existente
                        //var idTempLevel02ConsollidationId = _consolidationLevel02RepositoryGET.GetExistentLevel02Consollidation(level02Consolidation);
                        //level02Consolidation.Id = idTempLevel02ConsollidationId;
                        #endregion

                        _baseRepoConsolidationL2.AddOrUpdate(level02Consolidation);

                        foreach (var x in i.collectionLevel02DTO)
                        {

                            x.Level01Id = level01Consolidation.Level01Id;
                            x.ConsolidationLevel02Id = level02Consolidation.Id;

                            if (x.CorrectiveActionId > 0)
                            {
                                var CA = Mapper.Map<CorrectiveAction>(ListToSaveCA.FirstOrDefault(z => z.idcorrectiveaction == x.CorrectiveActionId));
                                _baseRepoCorrectiveAction.Add(CA);
                                x.CorrectiveActionId = CA.Id;
                            }

                            var collectionLevel02 = Mapper.Map<CollectionLevel02>(x);

                            #region Coloca flag duplicado.
                            _collectionLevel02RepositoryGET.SetDuplicated(collectionLevel02);
                            #endregion

                            _baseRepoCollectionL2.Add(collectionLevel02);

                            foreach (var y in x.collectionLevel03DTO)
                                y.CollectionLevel02Id = collectionLevel02.Id;

                            List<CollectionLevel03> cll3 = Mapper.Map<List<CollectionLevel03>>(x.collectionLevel03DTO);

                            #region Coloca flag duplicado.
                            _collectionLevel03RepositoryGET.SetDuplicated(cll3, collectionLevel02);
                            #endregion

                            _baseRepoCollectionL3.AddAll(cll3);
                        }
                    }
                }

                #endregion

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;

                #endregion

                #region Feedback

                return new GenericReturn<SyncDTO>("Susscess! All Data Saved in: " + elapsedMs + " ms, for: " + saving);

                #endregion

            }
            catch (Exception e)
            {
                #region Trata Exceção de forma Geral.

                return new GenericReturn<SyncDTO>(e, "Cannot sync.");

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
                    CollectionDate = objToSync.CollectionHtml.CollectionDate
                };
                _baseRepoCollectionHtml.Add(html);
                return new GenericReturn<SyncDTO>("Susscess! sync HTML.");
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
    }
}