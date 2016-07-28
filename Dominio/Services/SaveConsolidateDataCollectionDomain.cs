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
        private IBaseRepository<Level01Consolidation> _baseRepoLevel1Consolidation;
        private IBaseRepository<Level02Consolidation> _baseRepoLevel2Consolidation;
        private IBaseRepository<Level03Consolidation> _baseRepoLevel3Consolidation;
        private IBaseRepository<DataCollection> _baseRepoDataCollection;
        private IBaseRepository<DataCollectionResult> _baseRepoDataCollectionResult;

        public SaveConsolidateDataCollectionDomain(
            IBaseRepository<Level01Consolidation> baseRepoLevel1Consolidation,
            IBaseRepository<Level02Consolidation> baseRepoLevel2Consolidation,
            IBaseRepository<Level03Consolidation> baseRepoLevel3Consolidation,
            IBaseRepository<DataCollection> baseRepoDataCollection,
            IBaseRepository<DataCollectionResult> baseRepoDataCollectionResult
            )
        {
            _baseRepoLevel1Consolidation = baseRepoLevel1Consolidation;
            _baseRepoLevel2Consolidation = baseRepoLevel2Consolidation;
            _baseRepoLevel3Consolidation = baseRepoLevel3Consolidation;
            _baseRepoDataCollection = baseRepoDataCollection;
            _baseRepoDataCollectionResult = baseRepoDataCollectionResult;
        }



        public GenericReturn<ObjectConsildationDTO> SetDataToSincyAuditConsolidated(ObjectConsildationDTO obj)
        {

            try
            {
                #region Valida as 2 tabelas principais de inserção de dados, cabeçalho e corpo da Coleta

                obj.level01ConsolidationDTO.ValidaLevel01ConsolidationDTO();

                foreach (var i in obj.level02ConsolidationDTO)
                    i.ValidaLevel02ConsolidationDTO();

                foreach (var i in obj.dataCollectionDTO)
                    i.ValidaDataCollectionDTO();

                foreach (var i in obj.level03ConsolidationDTO)
                    i.ValidaLevel03ConsolidationDTO();

                foreach (var i in obj.dataCollectionResultDTO)
                    i.ValidaDataCollectionResultDTO();

                #endregion

                #region Cria Level2Consolidation e level1


                //if (obj.dataCollectionDTO.Level02ConsolidationId == 0)
                //{

                //    if (obj.dataCollectionDTO.Level02Consolidation.Level01ConsolidationId == 0)
                //    {

                //    }
                //}

                #endregion


                #region Salvando os 5 objetos em Banco de Dados.

                var watch = Stopwatch.StartNew();

                var level01ConsolidateSaved = Mapper.Map<Level01Consolidation>(obj.level01ConsolidationDTO);
                _baseRepoLevel1Consolidation.Add(level01ConsolidateSaved);

                foreach (var i in obj.level02ConsolidationDTO)
                {

                    i.Level01ConsolidationId = level01ConsolidateSaved.Id;
                    var level02ConsolidateSaved = Mapper.Map<Level02Consolidation>(i);
                    _baseRepoLevel2Consolidation.Add(level02ConsolidateSaved); //Save

                    var listOfdataCollectionDTO = obj.dataCollectionDTO.Where(r => r.Control == i.Control).ToList();
                    foreach (var x in listOfdataCollectionDTO)
                    {
                        x.Level02ConsolidationId = level02ConsolidateSaved.Id;
                        var dataCollectionDTOSaved = Mapper.Map<DataCollection>(x);
                        _baseRepoDataCollection.Add(dataCollectionDTOSaved); //Save

                        var listOfdataCollectionResultDTO = obj.dataCollectionResultDTO.Where(r => r.Control == x.Control).ToList();
                        foreach (var w in listOfdataCollectionResultDTO)
                            w.DataCollectionId = dataCollectionDTOSaved.Id;

                        var dataCollectionResultSaved = Mapper.Map<List<DataCollectionResult>>(listOfdataCollectionResultDTO);
                        _baseRepoDataCollectionResult.AddAll(dataCollectionResultSaved); //Save
                    }

                    var listOflevel03ConsolidationDTO = obj.level03ConsolidationDTO.Where(r => r.Control == i.Control).ToList();
                    foreach (var y in listOflevel03ConsolidationDTO)
                        y.Level02ConsolidationId = level02ConsolidateSaved.Id;

                    var level03ConsolidationSaved = Mapper.Map<List<Level03Consolidation>>(listOflevel03ConsolidationDTO);
                    _baseRepoLevel3Consolidation.AddAll(level03ConsolidationSaved); // Save

                }

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;

                #endregion

                #region Feedback

                return new GenericReturn<ObjectConsildationDTO>("Susscess! All Data Saved in: " + elapsedMs + " ms.");

                #endregion
            }
            catch (Exception e)
            {
                #region Trata Exceção de forma Geral.

                return new GenericReturn<ObjectConsildationDTO>(e, "Cannot sync.");

                #endregion
            }
            finally
            {
                #region NotImplemented

                #endregion
            }

        }

        public ObjectConsildationDTO SendData()
        {
            throw new NotImplementedException();
        }
    }
}