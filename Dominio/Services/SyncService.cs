using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using System;

namespace Dominio.Services
{
    public class SyncService : ISyncDomain 
    {
        private ISyncRepository _repoSync;

        public SyncService(ISyncRepository repoSync)
        {
            _repoSync = repoSync;
        }

        public void GetDataToSincyAudit()
        {
            try
            {
                _repoSync.GetDataToSincyAudit();
            }
            catch (Exception e)
            {

            }
        }

    }
}
