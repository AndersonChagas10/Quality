using AutoMapper;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using System;
using System.Collections.Generic;
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
                
                _baseRepoLevel1Consolidation.AddOrUpdate(Mapper.Map<Level01Consolidation>(obj.level01ConsolidationDTO));

                foreach (var i in obj.level02ConsolidationDTO)
                {
                    i.Level01ConsolidationId = obj.level01ConsolidationDTO.Id;
                    _baseRepoLevel2Consolidation.AddOrUpdate(Mapper.Map<Level02Consolidation>(i));
                }

                foreach (var i in obj.dataCollectionDTO)
                    _baseRepoDataCollection.AddOrUpdate(Mapper.Map<DataCollection>(i));

                foreach(var i in obj.level03ConsolidationDTO)
                    _baseRepoLevel3Consolidation.AddOrUpdate(Mapper.Map<Level03Consolidation>(i));

                foreach (var i in obj.dataCollectionResultDTO)
                    _baseRepoDataCollectionResult.AddOrUpdate(Mapper.Map<DataCollectionResult>(i));

                #endregion

                #region Feedback

                return new GenericReturn<ObjectConsildationDTO>("Susscess!!!");

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