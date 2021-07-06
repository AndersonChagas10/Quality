using Conformity.Application.Util;
using Conformity.Domain.Core.DTOs;
using Conformity.Domain.Core.Entities.Log;
using Conformity.Domain.Core.Interfaces;
using Conformity.Infra.Data.Core.Repository.Log;
using System;
using System.Collections.Generic;

namespace Conformity.Application.Core.Log
{
    public class EntityTrackService : BaseService<EntityTrack>
    {
        private readonly EntityTrackRepository _historicoAlteracaoRepository;
        private readonly ApplicationConfig _applicationConfig;
        public EntityTrackService(IRepositoryNoLazyLoad<EntityTrack> repository
            , EntityTrackRepository historicoAlteracaoRepository
            , ApplicationConfig applicationConfig) 
            : base(repository)
        {
            _historicoAlteracaoRepository = historicoAlteracaoRepository;
            _applicationConfig = applicationConfig;
        }
    
        public IEnumerable<EntityTrackViewModel> GetAll(string tabelaAlterada, int entidadeId)
        {
            return _historicoAlteracaoRepository.GetAll(tabelaAlterada, entidadeId);
        }

        public void RegisterCreate(object obj)
        {
            EntityTrack entityTrack = new EntityTrack()
            {
                Entity_Id = Convert.ToInt32(obj.GetType().GetProperty("Id").GetValue(obj)),
                TableName = obj.GetType().Name,
                RegisterDate = DateTime.Now,
                User_Id = _applicationConfig.Authenticated_Id,
                NewValue = null,
                OldValue = null,
                FieldName = null,
                TypeEntityTrack = Domain.Core.Enums.Log.ELogTrackEvent.Create
            };

            _repository.Add(entityTrack);
        }

        public void RegisterUpdate(object objOriginal, object objAlterado)
        {
            List<EntityTrack> historico = MapUpdatedFields(objOriginal, objAlterado);
            _repository.AddAll(historico);
        }

        private List<EntityTrack> MapUpdatedFields(object objAntes, object objDepois)
        {
            List<EntityTrack> list = new List<EntityTrack>();

            System.Reflection.PropertyInfo[] listaPropriedadesObj1 = objAntes.GetType().GetProperties();
            foreach (System.Reflection.PropertyInfo x in listaPropriedadesObj1)
            {
                object valorAntes = objAntes.GetType().GetProperty(x.Name).GetValue(objAntes);
                object valorDepois = objAntes.GetType().GetProperty(x.Name).GetValue(objDepois);
                if ((valorAntes != null && !valorAntes.Equals(valorDepois))
                    || (valorDepois != null && !valorDepois.Equals(valorAntes)))
                {
                    list.Add(new EntityTrack()
                    {
                        Entity_Id = Convert.ToInt32(objAntes.GetType().GetProperty("Id").GetValue(objAntes)),
                        TableName = objAntes.GetType().Name,
                        RegisterDate = DateTime.Now,
                        User_Id = _applicationConfig.Authenticated_Id,
                        NewValue = Convert.ToString(valorDepois),
                        OldValue = Convert.ToString(valorAntes),
                        FieldName = x.Name,
                        TypeEntityTrack = Domain.Core.Enums.Log.ELogTrackEvent.Update
                    });
                }
            }

            return list;
        }

    }
}
