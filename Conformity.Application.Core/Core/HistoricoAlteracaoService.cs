using Conformity.Application.Util;
using Conformity.Domain.Core.DTOs;
using Conformity.Domain.Core.Entities;
using Conformity.Domain.Core.Interfaces;
using Conformity.Infra.Data.Core;
using Conformity.Infra.Data.Core.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Conformity.Application.Core.Core
{
    public class HistoricoAlteracaoService : BaseService<HistoricoAlteracoes>
    {
        private HistoricoAlteracaoRepository _historicoAlteracaoRepository;
        private ApplicationConfig _applicationConfig;
        public HistoricoAlteracaoService(IRepositoryNoLazyLoad<HistoricoAlteracoes> repository
            , HistoricoAlteracaoRepository historicoAlteracaoRepository
            , ApplicationConfig applicationConfig) 
            : base(repository)
        {
            _historicoAlteracaoRepository = historicoAlteracaoRepository;
            _applicationConfig = applicationConfig;
        }
    
        public IEnumerable<HistoricoAlteracoesViewModel> GetAll(string tabelaAlterada, int entidadeId)
        {
            return _historicoAlteracaoRepository.GetAll(tabelaAlterada, entidadeId);
        }

        public void RegistrarAlteracoes(object objOriginal, object objAlterado)
        {
            var historico = MapearDiferencasEntreObjetos(objOriginal, objAlterado);
            _repository.AddAll(historico);
        }

        private List<HistoricoAlteracoes> MapearDiferencasEntreObjetos(object objAntes, object objDepois)
        {
            List<HistoricoAlteracoes> list = new List<HistoricoAlteracoes>();

            var listaPropriedadesObj1 = objAntes.GetType().GetProperties();
            foreach (var x in listaPropriedadesObj1)
            {
                var valorAntes = objAntes.GetType().GetProperty(x.Name).GetValue(objAntes);
                var valorDepois = objAntes.GetType().GetProperty(x.Name).GetValue(objDepois);
                if ((valorAntes != null && !valorAntes.Equals(valorDepois))
                    || (valorDepois != null && !valorDepois.Equals(valorAntes)))
                {
                    list.Add(new HistoricoAlteracoes()
                    {
                        EntidadeId = Convert.ToInt32(objAntes.GetType().GetProperty("Id").GetValue(objAntes)),
                        TabelaAlterada = objAntes.GetType().Name,
                        DataModificacao = DateTime.Now,
                        UsuarioModificacaoId = _applicationConfig.Authenticated_Id,
                        ValorAlterado = Convert.ToString(valorDepois),
                        ValorAnterior = Convert.ToString(valorAntes),
                        Propriedade = x.Name
                    });
                }
            }

            return list;
        }

    }
}
