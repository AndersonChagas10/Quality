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

        public GetDataResultRepository(SgqDbDevEntities _db)
            : base(_db)
        {

        }

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

        public void SetDuplicated(List<CollectionLevel03> cll3, CollectionLevel02 i)
        {
            var collectionLevel02 = db.CollectionLevel02.Where(r =>
                               r.Level01Id == i.Level01Id &&
                               r.Level02Id == i.Level02Id &&
                               r.Period == i.Period &&
                               r.Shift == i.Shift &&
                               r.Sample == i.Sample &&
                               r.UnitId == i.UnitId &&
                               r.ReauditNumber == i.ReauditNumber &&
                               r.EvaluationNumber == i.EvaluationNumber &&
                               r.Phase == i.Phase &&
                               r.ReauditIs == i.ReauditIs &&
                               DbFunctions.TruncateTime(r.CollectionDate) == DbFunctions.TruncateTime(i.CollectionDate) &&
                               (r.Duplicated == true)
                               ).OrderByDescending(r => r.Id).FirstOrDefault();


            //var lista = collectionLevel02.ToList();
            if (collectionLevel02 == null)
                return;

            //if (collectionLevel02.Count == 0)
            //    return;

            var alterThisData = db.CollectionLevel03.Where(r => collectionLevel02.Id == r.CollectionLevel02Id).ToList();

            if (alterThisData == null)
                throw new Exception("Dados requerem atualização para Duplicated = true em level03 porem não foram encontrados.");
            if (alterThisData.Count == 0)
                throw new Exception("Dados requerem atualização para Duplicated = true em level03 porem não foram encontrados.");

            foreach (var x in alterThisData)
            {
                x.Duplicated = true;
                UpdateNotCommit(x as T);
            }

            Commit();

        }

        public void SetDuplicated(CollectionLevel02 i)
        {

            var alterThisData = db.CollectionLevel02.FirstOrDefault(r =>
                                r.ConsolidationLevel02Id == i.ConsolidationLevel02Id &&
                                r.Level01Id == i.Level01Id &&
                                r.Level02Id == i.Level02Id &&
                                r.UnitId == i.UnitId &&
                                r.Shift == i.Shift &&
                                r.Period == i.Period &&
                                r.Phase == i.Phase &&
                                r.ReauditIs == i.ReauditIs &&
                                r.ReauditNumber == i.ReauditNumber &&
                                DbFunctions.TruncateTime(r.CollectionDate) == DbFunctions.TruncateTime(i.CollectionDate) &&
                                //r.StartPhaseDate == i.StartPhaseDate &&
                                r.EvaluationNumber == i.EvaluationNumber &&
                                r.Sample == i.Sample &&
                                //r.CattleTypeId == i.CattleTypeId &&
                                //r.Chainspeed == i.Chainspeed &&
                                //r.ConsecutiveFailureIs == i.ConsecutiveFailureIs &&
                                //r.ConsecutiveFailureTotal == i.ConsecutiveFailureTotal &&
                                //r.LotNumber == i.LotNumber &&
                                //r.Mudscore == i.Mudscore &&
                                //r.NotEvaluatedIs == i.NotEvaluatedIs &&
                                (r.Duplicated == false)
                                );

            if (alterThisData == null)
                return;

            alterThisData.Duplicated = true;
            Update(alterThisData as T);

        }

        public ConsolidationLevel01 GetExistentLevel01Consollidation(ConsolidationLevel01 level01Consolidation)
        {

            var retorno = db.ConsolidationLevel01.FirstOrDefault(r => r.DepartmentId == level01Consolidation.DepartmentId &&
                    r.Level01Id == level01Consolidation.Level01Id &&
                    r.UnitId == level01Consolidation.UnitId &&
                    DbFunctions.TruncateTime(r.AddDate) == DbFunctions.TruncateTime(DateTime.Now)
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
