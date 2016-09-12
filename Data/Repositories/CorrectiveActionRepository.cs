using Dominio;
using Dominio.Interfaces.Repositories;
using System;
using System.Data.Entity;
using System.Linq;
using DTO;
using System.Collections.Generic;

namespace Data.Repositories
{
    public class CorrectiveActionRepository : RepositoryBase<CorrectiveAction>, ICorrectiveActionRepository
    {

        public CorrectiveActionRepository(SgqDbDevEntities _db)
            : base(_db)
        {
        }

        public IEnumerable<CorrectiveAction> GetCorrectiveAction(DataCarrierFormulario data)
        {
            var query = db.CorrectiveAction.Where(r => DbFunctions.TruncateTime(r.AddDate) >= DbFunctions.TruncateTime(data._dataInicio)
                                             && DbFunctions.TruncateTime(r.AddDate) <= DbFunctions.TruncateTime(data._dataFim));

            if (data.shift > 0)
            {
                query = query.Where(r => r.CollectionLevel02.Shift == data.shift);
            }

            if (data.period > 0)
            {
                query = query.Where(r => r.CollectionLevel02.Period == data.period);
            }

            if (data.auditorId > 0)
            {
                query = query.Where(r => r.CollectionLevel02.AuditorId == data.auditorId);
            }

            return query;
        }

        public CorrectiveAction SalvarAcaoCorretiva(CorrectiveAction entitie)
        {
            if (entitie.Id > 0)
            {
                entitie.AlterDate = DateTime.Now;
                db.Entry(entitie).State = EntityState.Modified;
                Commit();
            }
            else
            {
                db.Set<CorrectiveAction>().Add(entitie);
                Commit();
            }
            return entitie;
        }

        //public CorrectiveActionLevels SalvarAcaoCorretivaLevels(CorrectiveActionLevels entitie)
        //{
        //    db.Set<CorrectiveActionLevels>().Add(entitie);
        //    Commit();

        //    return entitie;

        //}

        public CorrectiveAction VerificarAcaoCorretivaIncompleta(CorrectiveAction entitie)
        {
            var result = new CorrectiveAction();
            //var result = db.CorrectiveAction.AsNoTracking().FirstOrDefault(x =>
            //              (x.Slaughter == 0 ||
            //              x.Techinical == 0) &&
            //              x.AuditLevel1 == entitie.AuditLevel1 &&
            //              x.AuditLevel2 == entitie.AuditLevel2 &&
            //              x.AuditLevel3 == entitie.AuditLevel3 &&
            //              //  x.Auditor == entitie.Auditor &&
            //              x.Shift == entitie.Shift &&
            //              x.Period == entitie.Period);

            if (result != null)
            {
                if (result.SlaughterId != 0)
                {
                    result.SlaughterId = db.UserSgq.AsNoTracking().FirstOrDefault(x =>
                          x.Id == result.SlaughterId).Id;
                }

                if (result.TechinicalId != 0)
                {
                    result.TechinicalId = db.UserSgq.AsNoTracking().FirstOrDefault(x =>
                          x.Id == result.TechinicalId).Id;
                }
            }

            return result;
        }

    }
}
