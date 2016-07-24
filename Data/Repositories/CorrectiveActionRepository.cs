using Dominio;
using Dominio.Interfaces.Repositories;

namespace Data.Repositories
{
    public class CorrectiveActionRepository : RepositoryBase<CorrectiveAction>, ICorrectiveActionRepository
    {

        public CorrectiveActionRepository(SgqDbDevEntities _db)
            : base(_db)
        {
        }

        public SgqDbDevEntities db = new SgqDbDevEntities();

        public void SalvarAcaoCorretiva(CorrectiveAction entitie)
        {
            AddOrUpdate(entitie);
        }

        public CorrectiveAction VerificarAcaoCorretivaIncompleta(CorrectiveAction entitie)
        {
            //var result = (from x in db.CorrectiveAction.AsNoTracking()
            //              where x.Slaughter == 0 &&
            //              x.Techinical == 0 &&
            //              x.AuditLevel1 == entitie.AuditLevel1 &&
            //              x.AuditLevel2 == entitie.AuditLevel2 &&
            //              x.AuditLevel3 == entitie.AuditLevel3 &&
            //              //  x.Auditor == entitie.Auditor &&
            //              x.Shift == entitie.Shift &&
            //              x.Period == entitie.Period
            //              select x).FirstOrDefault();
            //return result;

            return new CorrectiveAction();
        }

    }
}
