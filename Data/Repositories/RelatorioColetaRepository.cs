using Dominio.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using DTO;
using Dominio;
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

        public IEnumerable<CollectionLevel02> GetLastEntryCollectionLevel02(IEnumerable<ConsolidationLevel02> cl2, DataCarrierFormulario form)
        {
            IEnumerable<CollectionLevel02> lastResults;

            lastResults = db.CollectionLevel02.Where(r => cl2.Any(x => x.Id == r.ConsolidationLevel02Id));

            if (form.shift > 0)
            {
                lastResults = lastResults.Where(r => r.Shift == form.shift);
            }

            if (form.period > 0)
            {
                lastResults = lastResults.Where(r => r.Period == form.period);
            }

            if (form.auditorId > 0)
            {
                lastResults = lastResults.Where(r => r.AuditorId == form.auditorId);
            }

            return lastResults;

        }

        public IEnumerable<CollectionLevel03> GetLastEntryCollectionLevel03(IEnumerable<CollectionLevel02> cll2, DataCarrierFormulario form)
        {
            var ids = new List<int>();
            foreach (var i in cll2)
                ids.Add(i.Id);

            var lastResults = db.CollectionLevel03.Where(r => ids.Any(x => x == r.CollectionLevel02Id));
            if (form.hasErros)
            {
                lastResults = lastResults.Where(r => r.Value > 0 || r.ValueText.Length > 0);
            }

            //var teste = lastResults.ToList();
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
            IEnumerable<ConsolidationLevel01> lastResults;
            if (form.unitId > 0)
            {
                lastResults =  db.ConsolidationLevel01.Where(r => r.UnitId == form.unitId
                    && DbFunctions.TruncateTime(r.AddDate) >= DbFunctions.TruncateTime(form._dataInicio)
                    && DbFunctions.TruncateTime(r.AddDate) <= DbFunctions.TruncateTime(form._dataFim));
            }
            else {
                lastResults = db.ConsolidationLevel01.Where(r => DbFunctions.TruncateTime(r.AddDate) >= DbFunctions.TruncateTime(form._dataInicio)
                   && DbFunctions.TruncateTime(r.AddDate) <= DbFunctions.TruncateTime(form._dataFim));
            }
                        
            return lastResults;
        }


        public IEnumerable<ConsolidationLevel01> GetEntryConsildatedLevel01ByDate(DataCarrierFormulario form)
        {
            //var teste1 = db.ConsolidationLevel01.Where(r => r.UnitId == form.unidadeId);
            var lastResults = db.ConsolidationLevel01.Where(r =>  DbFunctions.TruncateTime(r.AddDate) >= DbFunctions.TruncateTime(form._dataInicio)
                    && DbFunctions.TruncateTime(r.AddDate) <= DbFunctions.TruncateTime(form._dataFim));//.OrderByDescending(r=>r.Id).FirstOrDefault();
            return lastResults;
        }

        


        #endregion
    }
}
