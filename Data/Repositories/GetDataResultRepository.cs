using Dominio.Interfaces.Repositories;
using System.Linq;
using Dominio;
using System.Collections.Generic;
using System;
using System.Data.Entity;
using DTO.DTO;

namespace Data.Repositories
{
    public class GetDataResultRepository<T> : RepositoryBase<T>, IGetDataResultRepository<T> where T : class
    {

        #region Construtor

        private IBaseRepository<CollectionLevel03> _baseRepoCollectionL3;

        public GetDataResultRepository(SgqDbDevEntities _db, IBaseRepository<CollectionLevel03> baseRepoCollectionL3)
            : base(_db)
        {
            _baseRepoCollectionL3 = baseRepoCollectionL3;
        }

        #endregion

        public CollectionHtml GetHtmlLastEntry(SyncDTO idUnidade)
        {
            var retorno = db.CollectionHtml//OrderByDescending(o => o.Id)
                .FirstOrDefault(r => r.UnitId == idUnidade.CollectionHtml.UnitId 
                    && r.Shift == idUnidade.CollectionHtml.Shift
                    //&& r.Period == idUnidade.CollectionHtml.Period
                );
            if (retorno == null)
                return new CollectionHtml();

            return retorno;
        }

        public void Remove(int id)
        {
            T element = GetById(id);
            Remove(element);
        }

        public void SetDuplicated(IEnumerable<CollectionLevel02> CollectionLevel02Verificar, int level01Id)
        {
            List<CollectionLevel02> results = new List<CollectionLevel02>();
            foreach (var i in CollectionLevel02Verificar)
            {
                var data = db.CollectionLevel02.FirstOrDefault(r =>
                                    //db.CollectionLevel02.Any(c =>
                                    //c.ConsolidationLevel02Id == r.ConsolidationLevel02Id
                                    r.Level01Id == level01Id
                                    && i.Level02Id == r.Level02Id
                                    && i.UnitId == r.UnitId
                                    && i.Shift == r.Shift
                                    && i.Period == r.Period
                                    && i.Phase == r.Phase
                                    && i.ReauditIs == r.ReauditIs
                                    && i.ReauditNumber == r.ReauditNumber
                                    && DbFunctions.TruncateTime(i.CollectionDate) == DbFunctions.TruncateTime(r.CollectionDate)
                                    && i.EvaluationNumber == r.EvaluationNumber
                                    && i.Sample == r.Sample
                                    && r.Duplicated == false
                                    );

                if (data != null)
                {
                    if (data.CollectionDate < i.CollectionDate)
                    {
                        results.Add(data);
                    }
                    else
                    {
                        i.Duplicated = true;
                    }
                }
            }

            if (results == null)
                return;

            if (results.Count() == 0)
                return;

            foreach (var i in results)
            {
                i.Duplicated = true;
                Update(i as T);

                var alterThisDataCollectiolevel03 = db.CollectionLevel03.Where(r => i.Id == r.CollectionLevel02Id).ToList();

                if (alterThisDataCollectiolevel03 == null)
                    throw new Exception("Dados fora atualizados como duplicados para collectionlevel02id = " + i.Id + ", porem não foram encontrados registros para collectionLevel03 com este id Collectionlevel02.");
                if (alterThisDataCollectiolevel03.Count == 0)
                    throw new Exception("Dados fora atualizados como duplicados para collectionlevel02id = " + i.Id + ", porem não foram encontrados registros para collectionLevel03 com este id Collectionlevel02.");

                foreach (var x in alterThisDataCollectiolevel03)
                {
                    x.Duplicated = true;
                    _baseRepoCollectionL3.UpdateNotCommit(x);
                }

                _baseRepoCollectionL3.Commit();
            }


            //return alterThisData.Duplicated;

        }

        public ConsolidationLevel01 GetExistentLevel01Consollidation(ConsolidationLevel01 level01Consolidation)
        {

            var retorno = db.ConsolidationLevel01.FirstOrDefault(r => r.DepartmentId == level01Consolidation.DepartmentId &&
                    r.Level01Id == level01Consolidation.Level01Id &&
                    r.UnitId == level01Consolidation.UnitId &&
                    DbFunctions.TruncateTime(r.AddDate) == DbFunctions.TruncateTime(level01Consolidation.ConsolidationDate)
                    );

            return retorno;
        }

        public ConsolidationLevel02 GetExistentLevel02Consollidation(ConsolidationLevel02 level02Consolidation, ConsolidationLevel01 consolidationLevel01)
        {

            ConsolidationLevel02 consolidacaoExistente;
            if (consolidationLevel01.ConsolidationLevel02 != null)
                return consolidacaoExistente = consolidationLevel01.ConsolidationLevel02.FirstOrDefault(r => r.Level02Id == level02Consolidation.Level02Id);

            return null;
        }



        #region Get Last

        public IEnumerable<CollectionLevel02> GetLastEntryCollectionLevel02(IEnumerable<ConsolidationLevel02> cl2)
        {
            var lastResults = db.CollectionLevel02.Where(r => cl2.Any(x => x.Id == r.ConsolidationLevel02Id));
            return lastResults;

        }

        public IEnumerable<CollectionLevel03> GetLastEntryCollectionLevel03(IEnumerable<CollectionLevel02> cll2)
        {
            var lastResults = db.CollectionLevel03.Where(r => cll2.Any(x => x.Id == r.CollectionLevel02Id));
            return lastResults;
        }

        public IEnumerable<ConsolidationLevel01> GetLastEntryConsildatedLevel01()
        {
            var ids = db.Database.SqlQuery<int>("SELECT max(id) as id FROM [dbo].ConsolidationLevel01 group by Level01Id").ToList();
            var lastResults = db.ConsolidationLevel01.Where(r => ids.Any(x => x == r.Id));
            return lastResults;
        }

        public IEnumerable<ConsolidationLevel02> GetLastEntryConsildatedLevel02(IEnumerable<ConsolidationLevel01> cl1)
        {
            var listResults = db.ConsolidationLevel02.Where(r => cl1.Any(x => x.Id == r.Level01ConsolidationId));
            return listResults;
        }

        #endregion

    }
}
