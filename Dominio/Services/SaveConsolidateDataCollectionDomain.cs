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

        public SaveConsolidateDataCollectionDomain(
            IBaseRepository<ConsolidationLevel01> baseRepoConsolidationL1,
            IBaseRepository<ConsolidationLevel02> baseRepoConsolidationL2,
            IBaseRepository<CollectionLevel02> baseRepoCollectionL2,
            IBaseRepository<CollectionLevel03> baseRepoCollectionL3,
            IBaseRepository<CollectionHtml> baseRepoCollectionHtml,
            IBaseRepository<CorrectiveAction> baseRepoCorrectiveAction
            )
        {
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

                    var level01Consolidation = Mapper.Map<ConsolidationLevel01>(i);
                    if (level01Consolidation.Level01Id == 3)
                        saving = "CFF (Cut, Fold and Flaps)";
                    if (level01Consolidation.Level01Id == 1)
                        saving = "HTP";
                    if(level01Consolidation.Level01Id == 2)
                        saving = "Carcass Contamination Audit";

                    _baseRepoConsolidationL1.Add(level01Consolidation);

                    ConsolidationLevel02 level02Consolidation;
                    foreach (var j in i.consolidationLevel02DTO)
                    {
                        
                        j.Level01ConsolidationId = level01Consolidation.Id;
                        level02Consolidation = Mapper.Map<ConsolidationLevel02>(j);
                        _baseRepoConsolidationL2.Add(level02Consolidation);

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
                            _baseRepoCollectionL2.Add(collectionLevel02);

                            foreach (var y in x.collectionLevel03DTO)
                                y.CollectionLevel02Id = collectionLevel02.Id;

                            _baseRepoCollectionL3.AddAll(Mapper.Map<List<CollectionLevel03>>(x.collectionLevel03DTO));
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
                    Html = objToSync.html
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