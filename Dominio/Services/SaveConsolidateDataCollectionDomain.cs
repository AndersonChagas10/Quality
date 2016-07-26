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



        public void RecieveData(ObjectConsildationDTO obj)
        {
            try
            {
                #region Valida as 2 tabelas principais de inserção de dados, cabeçalho e corpo da Coleta

                foreach (var i in obj.dataCollectionDTO)
                    i.ValidaDataCollectionDTO();

                foreach (var i in obj.dataCollectionResultDTO)
                    i.ValidaDataCollectionResultDTO();

                #endregion

                #region Cria objeto para consolidações level1

                //Cria Consolidação Level3
                //foreach (var i in obj.dataCollectionResultDTO)
                //{
                //    if(i.Level03)
                //}

                //Cria Consolidação Level2
                foreach (var i in obj.dataCollectionDTO)
                {

                    if (i.Level02ConsolidationId > 0)
                    {
                        i.Level02Consolidation = Mapper.Map<Level02ConsolidationDTO>(_baseRepoLevel2Consolidation.GetById(i.Level02ConsolidationId));
                    }
                    else
                    {
                        i.Level02Consolidation = new Level02ConsolidationDTO();   //Implmentar validação de level 2 aqui.
                    }

                    //Cria Consolidação Level1
                    if (i.Level02Consolidation.Level01ConsolidationId > 0)
                    {
                        i.Level02Consolidation.Level01Consolidation = Mapper.Map<Level01ConsolidationDTO>(_baseRepoLevel1Consolidation.GetById(i.Level02Consolidation.Level01ConsolidationId));
                    }
                    else
                    {
                        i.Level02Consolidation.Level01Consolidation = new Level01ConsolidationDTO(); //Implmentar validação de level 2 aqui.
                    }

                }



                #endregion

                #region Cria objeto para consolidações level2



                #endregion

                #region Cria objeto para consolidações level3



                #endregion

                #region Validações para Objetos consolidados.

                //Valida Objeto Level01Consolidation
                foreach (var i in obj.dataCollectionDTO)
                    i.Level02Consolidation.Level01Consolidation.ValidaLevel01ConsolidationDTO();

                //Valida Objeto Level02Consolidation
                foreach (var i in obj.dataCollectionDTO)
                    i.Level02Consolidation.ValidaLevel02ConsolidationDTO();

                ////Valida Objeto Level03
                //foreach (var i in obj.dataCollectionResultDTO)
                //    i.Level03.ValidaLeve03DTO();

                #endregion

                #region Salvando os 5 objetos em Banco de Dados.

                //Salva Objeto Level1
                foreach (var i in obj.dataCollectionDTO)
                    _baseRepoLevel1Consolidation.AddOrUpdate(Mapper.Map<Level01Consolidation>(i.Level02Consolidation.Level01Consolidation));
                
                //Salva Objeto Level2
                foreach (var i in obj.dataCollectionDTO)
                    _baseRepoLevel2Consolidation.AddOrUpdate(Mapper.Map<Level02Consolidation>(i.Level02Consolidation));

                //Salva Objeto Level3
                //foreach (var i in obj.level03ConsolidationDTO)
                //    _baseRepoLevel3Consolidation.AddOrUpdate(Mapper.Map<Level03Consolidation>(i));

                //Salva Objeto Header Coleta
                foreach (var i in obj.dataCollectionDTO)
                    _baseRepoDataCollection.AddOrUpdate(Mapper.Map<DataCollection>(i));

                //Salva Objeto Coleta
                foreach (var i in obj.dataCollectionResultDTO)
                    _baseRepoDataCollectionResult.AddOrUpdate(Mapper.Map<DataCollectionResult>(i));

                #endregion

                #region Feedback

                #endregion
            }
            catch (Exception e)
            {
                #region Trata Exceção de forma Geral.

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
