using AutoMapper;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Services
{
    public class SaveConsolidateDataCollectionDomain : ISaveConsolidateDataCollectionDomain
    {
        private IBaseRepository<ConsolidationLevel01> _baseRepoConsolidationL1;
        private IBaseRepository<ConsolidationLevel02> _baseRepoConsolidationL2;
        private IBaseRepository<CollectionLevel02> _baseRepoCollectionL2;
        private IBaseRepository<CollectionLevel03> _baseRepoCollectionL3;

        public SaveConsolidateDataCollectionDomain(
            IBaseRepository<ConsolidationLevel01> baseRepoConsolidationL1,
            IBaseRepository<ConsolidationLevel02> baseRepoConsolidationL2,
            IBaseRepository<CollectionLevel02> baseRepoCollectionL2,
            IBaseRepository<CollectionLevel03> baseRepoCollectionL3
            )
        {
            _baseRepoConsolidationL1 = baseRepoConsolidationL1;
            _baseRepoConsolidationL2 = baseRepoConsolidationL2;
            _baseRepoCollectionL2 = baseRepoCollectionL2;
            _baseRepoCollectionL3 = baseRepoCollectionL3;
        }

        public GenericReturn<SyncDTO> SetDataToSincyAuditConsolidated(SyncDTO obj)
        {

            try
            {
                #region Validação e criação de objetos.

                if (obj.Root.Count == 0)
                    throw new ExceptionHelper("Impossible to Sync data. The Sync list is empty.");

                var ListToSave = new List<ConsolidationLevel01DTO>();
                obj.Root.ForEach(x => ListToSave.Add(x.ValidateAndCreateDtoConsolidationLevel01DTO()));

                #endregion



                #region Salvando os 5 objetos em Banco de Dados.

                var watch = Stopwatch.StartNew();

                foreach (var i in ListToSave)
                {
                    var level01Consolidation = Mapper.Map<ConsolidationLevel01>(i);
                    _baseRepoConsolidationL1.Add(level01Consolidation);

                    ConsolidationLevel02 level02Consolidation;
                    foreach (var j in i.consolidationLevel02DTO)
                    {
                        j.Level01Consolidation_Id = level01Consolidation.Id;
                        level02Consolidation = Mapper.Map<ConsolidationLevel02>(j);
                        _baseRepoConsolidationL2.Add(level02Consolidation);

                        foreach (var x in i.collectionLevel02DTO)
                        {

                            x.Level01_Id = level01Consolidation.Level01_Id;
                            x.ConsolidationLevel02_Id = level02Consolidation.Id;

                            var collectionLevel02 = Mapper.Map<CollectionLevel02>(x);
                            _baseRepoCollectionL2.AddNotCommit(collectionLevel02);
                            foreach (var y in i.collectionLevel03DTO)
                            {
                                y.CollectionLevel02_ID = collectionLevel02.Id;
                                _baseRepoCollectionL3.AddNotCommit(Mapper.Map<CollectionLevel03>(y));
                            }

                        }
                    }

                    _baseRepoCollectionL2.Commit();
                    _baseRepoCollectionL3.Commit();
                }

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;

                #endregion

                #region Feedback

                return new GenericReturn<SyncDTO>("Susscess! All Data Saved in: " + elapsedMs + " ms.");

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

        //public ObjectConsildationDTO SendData()
        //{
        //    throw new NotImplementedException();
        //}
    }
}