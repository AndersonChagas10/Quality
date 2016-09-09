using Dominio.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Dominio;
using Dominio.Entities.BaseEntity;
using System.Data.Entity;

namespace Data.Repositories
{
    public class RelatorioColetaRepository<T> : RepositoryBase<T>, IRelatorioColetaRepository<T> where T : class
    {

        public RelatorioColetaRepository(SgqDbDevEntities Db) : base(Db)
        {
        }

        #region Prototype

        public IEnumerable<T> GetByDate(DataCarrierFormulario form)
        {
            //var tipo = typeof(T);

            //if (tipo.Equals(typeof(CollectionLevel03)))
            //{
            //    db.CollectionLevel03.Where(r => DbFunctions.TruncateTime(r.AddDate) >= DbFunctions.TruncateTime(form._dataInicio)
            //                                 && DbFunctions.TruncateTime(r.AddDate) <= DbFunctions.TruncateTime(form._dataFim));
            //}
            //else if (tipo.Equals(typeof(CollectionLevel02)))
            //{
            //    db.CollectionLevel02.Where(r => DbFunctions.TruncateTime(r.AddDate) >= DbFunctions.TruncateTime(form._dataInicio)
            //                                 && DbFunctions.TruncateTime(r.AddDate) <= DbFunctions.TruncateTime(form._dataFim)
            //                                 && r.UnitId == form.unidadeId);
            //}
            //else if (tipo.Equals(typeof(CollectionLevel03)))
            //{
            //    db.CollectionLevel03.Where(r => DbFunctions.TruncateTime(r.AddDate) >= DbFunctions.TruncateTime(form._dataInicio)
            //                                 && DbFunctions.TruncateTime(r.AddDate) <= DbFunctions.TruncateTime(form._dataFim));
            //}

            //var queryResult = Entity.Where(r => DbFunctions.TruncateTime(r.AddDate) >= DbFunctions.TruncateTime(form._dataInicio)
            //                                 && DbFunctions.TruncateTime(r.AddDate) <= DbFunctions.TruncateTime(form._dataFim));
            throw new NotImplementedException();
            //return queryResult;
        }

        #endregion

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

        public IEnumerable<ConsolidationLevel02> GetLastEntryConsildatedLevel02(IEnumerable<ConsolidationLevel01> cl1)
        {
            var listResults = db.ConsolidationLevel02.Where(r => cl1.Any(x => x.Id == r.Level01ConsolidationId));
            return listResults;
        }

        #endregion

        #region Get By Date

        //CORRIGIR Hoje salva varias consolidações, na entrada dos dados precisamos salvar apenas uma consolidação e sobrescreve-la
        public IEnumerable<ConsolidationLevel02> GetEntryConsildatedLevel02ByDate(DataCarrierFormulario form)
        {
            var listResults = db.ConsolidationLevel02
                .Where(r => DbFunctions.TruncateTime(r.AddDate) >= DbFunctions.TruncateTime(form._dataInicio)
                            && DbFunctions.TruncateTime(r.AddDate) <= DbFunctions.TruncateTime(form._dataFim));

            return listResults;
        }

        public IEnumerable<ConsolidationLevel01> GetEntryConsildatedLevel01ByDateAndUnit(DataCarrierFormulario form)
        {
            //var teste1 = db.ConsolidationLevel01.Where(r => r.UnitId == form.unidadeId);
            var lastResults = db.ConsolidationLevel01.Where(r => r.UnitId == form.unidadeId
                    && DbFunctions.TruncateTime(r.AddDate) >= DbFunctions.TruncateTime(form._dataInicio)
                    && DbFunctions.TruncateTime(r.AddDate) <= DbFunctions.TruncateTime(form._dataFim));//.OrderByDescending(r=>r.Id).FirstOrDefault();

            return lastResults;
        }

        #endregion
    }
}
