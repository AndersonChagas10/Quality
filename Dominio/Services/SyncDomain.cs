using AutoMapper;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using System;
using System.Collections.Generic;

namespace Dominio.Services
{
    public class SyncDomain : ISyncDomain
    {
        //private ISyncRepository<Coleta> _repoSync;
        private ISyncRepository<Level01> _repoSyncLevel1;
        private ISyncRepository<Level02> _repoSyncLevel2;
        private ISyncRepository<Level03> _repoSyncLevel3;
        private ISyncRepository<UserSgq> _repoSyncUserSgq;
        private ISyncRepository<CorrectiveAction> _repoSyncCorrectiveAction;

        public SyncDomain(/*ISyncRepository<Coleta> repoSync,*/
                            ISyncRepository<Level01> repoSyncLevel1,
                            ISyncRepository<Level02> repoSyncLevel2,
                            ISyncRepository<Level03> repoSyncLevel3,
                            ISyncRepository<UserSgq> repoSyncUserSgq,
                            ISyncRepository<CorrectiveAction> repoSyncCorrectiveAction)
        {
            //_repoSync = repoSync;
            _repoSyncLevel1 = repoSyncLevel1;
            _repoSyncLevel2 = repoSyncLevel2;
            _repoSyncLevel3 = repoSyncLevel3;
            _repoSyncUserSgq = repoSyncUserSgq;
            _repoSyncCorrectiveAction = repoSyncCorrectiveAction;
        }

        public GenericReturn<SyncDTO> GetDataToSincyAudit()
        {
            try
            {
                //var queryParaColeta = _repoSync.GetDataToSincyAudit();
                var queryParaLevel1 = _repoSyncLevel1.GetDataToSincyAudit();
                var queryParaLevel2 = _repoSyncLevel2.GetDataToSincyAudit();
                var queryParaLevel3 = _repoSyncLevel3.GetDataToSincyAudit();
                var queryParaUsuarios = _repoSyncUserSgq.GetDataToSincyAudit();
                var queryParaCorrectiveAction = _repoSyncCorrectiveAction.GetDataToSincyAudit();

                var retorno = new SyncDTO()
                {
                    //Coleta = Mapper.Map<List<Coleta>, List<ColetaDTO>>(queryParaColeta),
                    Level1 = Mapper.Map<List<Level01>, List<Level1DTO>>(queryParaLevel1),
                    Level2 = Mapper.Map<List<Level02>, List<Level2DTO>>(queryParaLevel2),
                    Level3 = Mapper.Map<List<Level03>, List<Level3DTO>>(queryParaLevel3),
                    UserSgq = Mapper.Map<List<UserSgq>, List<UserDTO>>(queryParaUsuarios),
                    CorrectiveAction = Mapper.Map<List<CorrectiveAction>, List<CorrectiveActionDTO>>(queryParaCorrectiveAction)
                };

                return new GenericReturn<SyncDTO>(retorno);
            }
            catch (Exception e)
            {
                return new GenericReturn<SyncDTO>(e, "Cannot get data to sync.");
            }
        }

      
    }
}
