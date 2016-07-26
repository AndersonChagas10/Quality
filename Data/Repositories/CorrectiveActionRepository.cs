using Dominio;
using Dominio.Interfaces.Repositories;
using System.Linq;

namespace Data.Repositories
{
    public class CorrectiveActionRepository : RepositoryBase<CorrectiveAction>, ICorrectiveActionRepository
    {

        public CorrectiveActionRepository(SgqDbDevEntities _db)
            : base(_db)
        {
        }

        public void SalvarAcaoCorretiva(CorrectiveAction entitie)
        {
            AddOrUpdate(entitie);
        }

        public CorrectiveAction VerificarAcaoCorretivaIncompleta(CorrectiveAction entitie)
        {

            var result = db.CorrectiveAction.AsNoTracking().FirstOrDefault(x =>
                          (x.SlaughterId == 0 ||
                          x.TechinicalId == 0) &&
                          x.AuditLevel1Id  == entitie.AuditLevel1Id &&
                          x.AuditLevel2Id == entitie.AuditLevel2Id &&
                          x.AuditLevel3Id == entitie.AuditLevel3Id &&
                          //  x.Auditor == entitie.Auditor &&
                          x.ShiftId == entitie.ShiftId &&
                          x.PeriodId == entitie.PeriodId);

            if (result != null)
            {
                if (result.SlaughterId != 0)
                {
                    result.NameSlaughter = db.UserSgq.AsNoTracking().FirstOrDefault(x =>
                          x.Id == result.SlaughterId).Name;
                }

                if (result.TechinicalId != 0)
                {
                    result.NameTechinical = db.UserSgq.AsNoTracking().FirstOrDefault(x =>
                          x.Id == result.TechinicalId).Name;
                }
            }

            return result;
        }

    }
}
