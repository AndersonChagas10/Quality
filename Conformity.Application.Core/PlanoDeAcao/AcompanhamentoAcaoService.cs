using Conformity.Application.Core.Log;
using Conformity.Domain.Core.DTOs;
using Conformity.Domain.Core.Entities.Global;
using Conformity.Domain.Core.Entities.PlanoDeAcao;
using Conformity.Domain.Core.Enums.PlanoDeAcao;
using Conformity.Domain.Core.Interfaces;
using Conformity.Infra.CrossCutting;
using Conformity.Infra.Data.Core.Repository.PlanoDeAcao;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Conformity.Application.Core.PlanoDeAcao
{
    public class AcompanhamentoAcaoService : BaseServiceWithLog<AcompanhamentoAcao>
    {
        private readonly ApplicationConfig _applicationConfig;
        private readonly LogErrorService _logErrorService;

        public AcompanhamentoAcaoService(IPlanoDeAcaoRepositoryNoLazyLoad<AcompanhamentoAcao> repository
            , ApplicationConfig applicationConfig
            , LogErrorService logErrorService
            , EntityTrackService historicoAlteracaoService)
            : base(repository
                  , historicoAlteracaoService)
        {
            _applicationConfig = applicationConfig;
            _logErrorService = logErrorService;
        }

        public AcaoViewModel SalvarAcompanhamentoComNotificaveis(int id, AcompanhamentoAcaoInputModel objAcompanhamentoAcao)
        {
            try
            {
                var listaNotificar = objAcompanhamentoAcao.ListaNotificar
                    .Select(x =>
                        new AcompanhamentoAcaoXNotificar()
                        {
                            UserSgq_Id = x.Id
                        }
                    ).ToList();

                List<AcompanhamentoAcaoXAttributes> listaAcompanhamentoAcaoXAttributes = new List<AcompanhamentoAcaoXAttributes>();

                if (!string.IsNullOrEmpty(objAcompanhamentoAcao.Evidencia1_Base64))
                {
                    var evidencia1_Path = ConverterEvidencia(id, objAcompanhamentoAcao.Evidencia1_Base64);
                    AcompanhamentoAcaoXAttributes attributes = new AcompanhamentoAcaoXAttributes()
                    {
                        FieldName = EAcompanhamentoAcaoXAttributes.Evidencia1,
                        Value = evidencia1_Path
                    };

                    listaAcompanhamentoAcaoXAttributes.Add(attributes);
                }
                
                if (!string.IsNullOrEmpty(objAcompanhamentoAcao.Evidencia2_Base64))
                {
                    var evidencia2_Path = ConverterEvidencia(id, objAcompanhamentoAcao.Evidencia2_Base64);
                    AcompanhamentoAcaoXAttributes attributes = new AcompanhamentoAcaoXAttributes()
                    {
                        FieldName = EAcompanhamentoAcaoXAttributes.Evidencia2,
                        Value = evidencia2_Path
                    };

                    listaAcompanhamentoAcaoXAttributes.Add(attributes);
                }

                var acompanhamento = new AcompanhamentoAcao()
                {
                    ListaNotificar = listaNotificar,
                    Observacao = objAcompanhamentoAcao.Observacao,
                    Status = (int)objAcompanhamentoAcao.Status,
                    Acao_Id = id,
                    UserSgq_Id = _applicationConfig.Authenticated_Id,
                    AcompanhamentoAcaoXAttributes = listaAcompanhamentoAcaoXAttributes
                };
                base.Add(acompanhamento);
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }

        private string ConverterEvidencia(int acao_Id, string fileBase64)
        {
            var basePath = DicionarioEstatico.DicionarioEstaticoHelpers.StorageRoot ?? "~";
            if (basePath.Equals("~"))
            {
                basePath = @AppDomain.CurrentDomain.BaseDirectory;
            }

            basePath = basePath + "\\Acao";
            string fileName = acao_Id + DateTime.Now.GetHashCode() + new Random().Next(1000, 9999) + ".png";


            FileHelper.SavePhoto(fileBase64, basePath, fileName
                      , DicionarioEstatico.DicionarioEstaticoHelpers.credentialUserServerPhoto
                      , DicionarioEstatico.DicionarioEstaticoHelpers.credentialPassServerPhoto
                      , DicionarioEstatico.DicionarioEstaticoHelpers.StorageRoot, out Exception exception);

            if (exception != null)
                _logErrorService.Register(exception);

            var path = Path.Combine(basePath, fileName);

            return path;
        }
    }
}
