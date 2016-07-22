using AutoMapper;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using System;
using System.Collections.Generic;

namespace Dominio.Services
{
    public class SyncService : ISyncDomain
    {
        private ISyncRepository<Coleta> _repoSync;
        private ISyncRepository<Level1> _repoSyncLevel1;
        private ISyncRepository<Level2> _repoSyncLevel2;
        private ISyncRepository<Level3> _repoSyncLevel3;
        private ISyncRepository<UserSgq> _repoSyncUserSgq;

        public SyncService(ISyncRepository<Coleta> repoSync,
                            ISyncRepository<Level1> repoSyncLevel1,
                            ISyncRepository<Level2> repoSyncLevel2,
                            ISyncRepository<Level3> repoSyncLevel3,
                            ISyncRepository<UserSgq> repoSyncUserSgq)
        {
            _repoSync = repoSync;
            _repoSyncLevel1 = repoSyncLevel1;
            _repoSyncLevel2 = repoSyncLevel2;
            _repoSyncLevel3 = repoSyncLevel3;
            _repoSyncUserSgq = repoSyncUserSgq;
        }

        public GenericReturn<SyncDTO> GetDataToSincyAudit()
        {
            try
            {
                var queryParaColeta = _repoSync.GetDataToSincyAudit();
                var queryParaLevel1 = _repoSyncLevel1.GetDataToSincyAudit();
                var queryParaLevel2 = _repoSyncLevel2.GetDataToSincyAudit();
                var queryParaLevel3 = _repoSyncLevel3.GetDataToSincyAudit();
                var queryParaUsuarios = _repoSyncUserSgq.GetDataToSincyAudit();

                var retorno = new SyncDTO()
                {
                    Coleta = Mapper.Map<List<Coleta>, List<ColetaDTO>>(queryParaColeta),
                    Level1 = Mapper.Map<List<Level1>, List<Level1DTO>>(queryParaLevel1),
                    Level2 = Mapper.Map<List<Level2>, List<Level2DTO>>(queryParaLevel2),
                    Level3 = Mapper.Map<List<Level3>, List<Level3DTO>>(queryParaLevel3),
                    UserSgq = Mapper.Map<List<UserSgq>, List<UserDTO>>(queryParaUsuarios)
                };

                return new GenericReturn<SyncDTO>(retorno);
            }
            catch (Exception e)
            {
                return new GenericReturn<SyncDTO>(e, "Cannot get data to sync.");
            }
        }

        public GenericReturn<SyncDTO> SetDataToSincyAudit(SyncDTO objToSync)
        {
            try
            {
                foreach (var i in objToSync.Coleta)
                    i.ValidaColeta();

                var coleta = Mapper.Map<List<ColetaDTO>, List<Coleta>>(objToSync.Coleta);
                _repoSync.ValidaFkResultado(coleta);
                _repoSync.SetDataToSincyAudit(coleta);

                return new GenericReturn<SyncDTO>("Sucesso!!!!!!");
            }
            catch (Exception e)
            {
                return new GenericReturn<SyncDTO>(e, "Cannot get data to sync.");
            }
        }
    }
}
