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
                          (x.Slaughter == 0 ||
                          x.Techinical == 0) &&
                          x.AuditLevel1 == entitie.AuditLevel1 &&
                          x.AuditLevel2 == entitie.AuditLevel2 &&
                          x.AuditLevel3 == entitie.AuditLevel3 &&
                          //  x.Auditor == entitie.Auditor &&
                          x.Shift == entitie.Shift &&
                          x.Period == entitie.Period);

            if (result != null)
            {
                if (result.Slaughter != 0)
                {
                    result.NameSlaughter = db.UserSgq.AsNoTracking().FirstOrDefault(x =>
                          x.Id == result.Slaughter).Name;
                }

                if (result.Techinical != 0)
                {
                    result.NameTechinical = db.UserSgq.AsNoTracking().FirstOrDefault(x =>
                          x.Id == result.Techinical).Name;
                }
            }

            return result;
        }

    }
}
