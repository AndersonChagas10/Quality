using Dominio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using DTO.DTO;
using DTO.Helpers;
using Dominio.Interfaces.Repositories;
using AutoMapper;
using DTO;
using DTO.TableResults;

namespace Dominio.Services
{
    public class RelatorioColetaDomain : IRelatorioColetaDomain
    {

        #region Construtor
        private IBaseRepository<Level02> _baseLevel02Repo;
        private IBaseRepository<Level03> _baseLevel03Repo;
        private IEnumerable<Level02> _listLevel02;
        private IEnumerable<Level03> _listLevel03;
        private IRelatorioColetaRepository<CollectionLevel02> _repoCollectionLevel02;
        private IRelatorioColetaRepository<CollectionLevel03> _repoCollectionLevel03;
        private IRelatorioColetaRepository<ConsolidationLevel01> _repoConsolidationLevel01;
        private IRelatorioColetaRepository<ConsolidationLevel02> _repoConsolidationLevel02;

        public RelatorioColetaDomain(
            IRelatorioColetaRepository<CollectionLevel02> repoCollectionLevel02,
            IRelatorioColetaRepository<CollectionLevel03> repoCollectionLevel03,
            IRelatorioColetaRepository<ConsolidationLevel01> repoConsolidationLevel01,
            IRelatorioColetaRepository<ConsolidationLevel02> repoConsolidationLevel02,
            IBaseRepository<Level02> baseLevel02Repo,
            IBaseRepository<Level03> baseLevel03Repo
            )
        {
            _baseLevel02Repo = baseLevel02Repo;
            _baseLevel03Repo = baseLevel03Repo;
            _repoCollectionLevel02 = repoCollectionLevel02;
            _repoCollectionLevel03 = repoCollectionLevel03;
            _repoConsolidationLevel01 = repoConsolidationLevel01;
            _repoConsolidationLevel02 = repoConsolidationLevel02;

            _listLevel02 = _baseLevel02Repo.GetAll();
            _listLevel03 = _baseLevel03Repo.GetAll();
        }

        #endregion

        #region Prototype

        public GenericReturn<ResultSetRelatorioColeta> GetCollectionLevel02(DataCarrierFormulario form)
        {
            try
            {

                var result = _repoCollectionLevel02.GetByDate(form).ToList();

                Guard.CheckListNullOrEmpty(result, "Collection Level02 dont have results.");

                var resultSet = new ResultSetRelatorioColeta()
                {
                    listCollectionLevel02DTO = Mapper.Map<List<CollectionLevel02DTO>>(result)
                };

                return new GenericReturn<ResultSetRelatorioColeta>(resultSet);

            }
            catch (Exception e)
            {
                return new GenericReturn<ResultSetRelatorioColeta>(e, "Failid to obtain data for: Collection Level02");
            }
        }

        public GenericReturn<ResultSetRelatorioColeta> GetCollectionLevel03(DataCarrierFormulario form)
        {
            try
            {

                var result = _repoCollectionLevel03.GetByDate(form).ToList();

                Guard.CheckListNullOrEmpty(result, "Collection Level03 dont have results.");

                var resultSet = new ResultSetRelatorioColeta()
                {
                    listCollectionLevel02DTO = Mapper.Map<List<CollectionLevel02DTO>>(result)
                };

                return new GenericReturn<ResultSetRelatorioColeta>(resultSet);

            }
            catch (Exception e)
            {
                return new GenericReturn<ResultSetRelatorioColeta>(e, "Failid to obtain data for: Collection Level03");
            }
        }

        public GenericReturn<ResultSetRelatorioColeta> GetConsolidationLevel01(DataCarrierFormulario form)
        {
            try
            {

                var result = _repoCollectionLevel03.GetByDate(form).ToList();

                Guard.CheckListNullOrEmpty(result, "Consolidation Level01 dont have results.");

                var resultSet = new ResultSetRelatorioColeta()
                {
                    listCollectionLevel02DTO = Mapper.Map<List<CollectionLevel02DTO>>(result)
                };

                return new GenericReturn<ResultSetRelatorioColeta>(resultSet);

            }
            catch (Exception e)
            {
                return new GenericReturn<ResultSetRelatorioColeta>(e, "Failid to obtain data for: Consolidation Level01");
            }
        }

        public GenericReturn<ResultSetRelatorioColeta> GetConsolidationLevel02(DataCarrierFormulario form)
        {
            try
            {

                var result = _repoCollectionLevel03.GetByDate(form).ToList();

                Guard.CheckListNullOrEmpty(result, "Consolidation Level02 dont have results.");

                var resultSet = new ResultSetRelatorioColeta()
                {
                    listCollectionLevel02DTO = Mapper.Map<List<CollectionLevel02DTO>>(result)
                };

                return new GenericReturn<ResultSetRelatorioColeta>(resultSet);

            }
            catch (Exception e)
            {
                return new GenericReturn<ResultSetRelatorioColeta>(e, "Failid to obtain data for: Consolidation Level02");
            }
        }

        public GenericReturn<ResultSetRelatorioColeta> GetAllData(DataCarrierFormulario form)
        {
            try
            {

                var resultCollectionLevel02 = _repoCollectionLevel02.GetByDate(form).ToList();
                var resultCollectionLevel03 = _repoCollectionLevel03.GetByDate(form).ToList();
                var resultConsolidationLevel01 = _repoConsolidationLevel01.GetByDate(form).ToList();
                var resultConsolidationLevel02 = _repoConsolidationLevel02.GetByDate(form).ToList();

                if (resultCollectionLevel03.IsNull() && resultCollectionLevel02.IsNull() && resultConsolidationLevel01.IsNull() && resultConsolidationLevel02.IsNull())
                    throw new ExceptionHelper("All data empty.");

                var resultSet = new ResultSetRelatorioColeta()
                {
                    listCollectionLevel02DTO = Mapper.Map<List<CollectionLevel02DTO>>(resultCollectionLevel02),
                    listCollectionLevel03DTO = Mapper.Map<List<CollectionLevel03DTO>>(resultCollectionLevel03),
                    listConsolidationLevel01 = Mapper.Map<List<ConsolidationLevel01DTO>>(resultConsolidationLevel01),
                    listConsolidationLevel02 = Mapper.Map<List<ConsolidationLevel02DTO>>(resultConsolidationLevel02),
                };

                return new GenericReturn<ResultSetRelatorioColeta>(resultSet);

            }
            catch (Exception e)
            {
                return new GenericReturn<ResultSetRelatorioColeta>(e, "Failid to obtain data for: Consolidation Level02");
            }
        }

        #endregion

        public GenericReturn<GetSyncDTO> GetEntryByDate(DataCarrierFormulario form)
        {
            try
            {

                //Query
                var consildatedLelve01List = _repoConsolidationLevel01.GetEntryConsildatedLevel01ByDateAndUnit(form).ToList();

                //Mapper
                var consildatedLelve01ListDTO1 = Mapper.Map<List<ConsolidationLevel01DTO>>(consildatedLelve01List);

                //Processa Resultados
                var processador = new TableResultsForDataTable();
                var resultadosProcessadosParaFormatoTabela = processador.DataCollectionReportsProcessedResults(consildatedLelve01ListDTO1);
                
                //Retorno
                return new GenericReturn<GetSyncDTO>(new GetSyncDTO()
                {
                    ConsolidationLevel01 = resultadosProcessadosParaFormatoTabela
                });

            }
            catch (Exception e)
            {
                return new GenericReturn<GetSyncDTO>(e, "Cannot get data.");
            }
        }

    }
}
