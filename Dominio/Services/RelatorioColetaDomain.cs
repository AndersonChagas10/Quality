using Dominio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using DTO.DTO;
using DTO.Helpers;
using Dominio.Interfaces.Repositories;
using AutoMapper;
using DTO;

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

                #region Query

                var consildatedLelve01 = _repoConsolidationLevel01.GetEntryConsildatedLevel01ByDateAndUnit(form);
                var consildatedLelve01List = consildatedLelve01.ToList();

                //var consildatedLelve02 = _repoConsolidationLevel02.GetLastEntryConsildatedLevel02(consildatedLelve01);
                //var consildatedLelve02List = consildatedLelve02.ToList();

                //var collectionLelve02 = _repoCollectionLevel02.GetLastEntryCollectionLevel02(consildatedLelve02, form);
                //var collectionLelve03 = _repoCollectionLevel03.GetLastEntryCollectionLevel03(collectionLelve02, form);

                //if (collectionLelve02.Count() == 0 || collectionLelve03.Count() == 0)
                //    return new GenericReturn<GetSyncDTO>(new GetSyncDTO() { ConsolidationLevel01 = new List<ConsolidationLevel01DTO>() });

                //var collectionLelve02List = collectionLelve02.ToList();
                //var collectionLelve03List = collectionLelve03.ToList();

                #endregion

                #region Mapper

                var consildatedLelve01ListDTO1 = new List<ConsolidationLevel01DTO>();
                consildatedLelve01ListDTO1 = Mapper.Map<List<ConsolidationLevel01DTO>>(consildatedLelve01List);
               
                //var consildatedLelve01ListDTO2 = new List<ConsolidationLevel02DTO>();
                //consildatedLelve01ListDTO2 = Mapper.Map<List<ConsolidationLevel02DTO>>(consildatedLelve02List);

                //var collectionLelve02ListDTO = new List<CollectionLevel02DTO>();
                //collectionLelve02ListDTO = Mapper.Map<List<CollectionLevel02DTO>>(collectionLelve02List);

                //var collectionLelve03ListDTO = new List<CollectionLevel03DTO>();
                //collectionLelve03ListDTO = Mapper.Map<List<CollectionLevel03DTO>>(collectionLelve03List);

                #endregion

                #region Make ResultSet

                //foreach (var i in consildatedLelve01ListDTO1)
                //{
                //    var temp = consildatedLelve01ListDTO2.Where(r => r.Level01ConsolidationId == i.Id);
                //    i.consolidationLevel02DTO = temp.ToList();

                //    var temp2 = collectionLelve02ListDTO.Where(r => temp.Any(z => z.Id == r.ConsolidationLevel02Id));
                //    i.collectionLevel02DTO = temp2.ToList();

                //    for (var v = 0; v < i.collectionLevel02DTO.Count; v++)
                //    {
                //        var y = i.collectionLevel02DTO[v];

                //        //Busca nome level02
                //        y.Name = _listLevel02.FirstOrDefault(z => z.Id == y.Level02Id).Name;

                //        //Encontra os level03 do level02
                //        var level03DoLevel02 = collectionLelve03ListDTO.Where(r => y.Id == r.CollectionLevel02Id).ToList();

                //        //Busca nome level03
                //        foreach (var z in level03DoLevel02)
                //            z.Name = _listLevel03.FirstOrDefault(zz => zz.Id == z.Level03Id).Name;

                //        //Insere level03 no leve02 e remove o 03 da lista temp.
                //        y.collectionLevel03DTO = level03DoLevel02;
                //        collectionLelve03ListDTO.RemoveAll(r => y.Id == r.CollectionLevel02Id);
                //    }

                //}

                #endregion

                var listResult = new GetSyncDTO()
                {
                    ConsolidationLevel01 = consildatedLelve01ListDTO1,
                };



                return new GenericReturn<GetSyncDTO>(listResult);

            }
            catch (Exception e)
            {
                return new GenericReturn<GetSyncDTO>(e, "Cannot get data.");
            }
        }

    }
}
