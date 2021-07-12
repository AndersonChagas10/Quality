using Conformity.Infra.CrossCutting;
using Dominio;
using DTO.DTO;
using Newtonsoft.Json.Linq;
using SgqService.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace SgqSystem.Controllers.Api
{
    public class BaseAuthenticatedApiController : BaseApiController
    {
        private readonly ApplicationConfig _applicationConfig;
        public BaseAuthenticatedApiController(ApplicationConfig applicationConfig) : base()
        {
            _applicationConfig = applicationConfig;
        }

        // GET: BaseAPI
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            // Do some stuff
            base.Initialize(controllerContext);

            try
            {
                token = Request.Headers.GetValues("token").FirstOrDefault().ToString();
            }
            catch
            {
            }

            int? authenticated_Id = GetUsuarioLogadoId();
            if (authenticated_Id != null)
            {
                _applicationConfig.Authenticated_Id = authenticated_Id.Value;
            }
        }

        protected int? GetUsuarioLogadoId()
        {
            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;

                var user = new CredenciaisSgq()
                {
                    Username = tokenFiltros.Split('|')[0],
                    Senha = tokenFiltros.Split('|')[1]
                };

                var usuarioLogado_Id = db.UserSgq.Where(x => x.Name == user.Username)
                .Select(x => x.Id)
                .FirstOrDefault();

                return usuarioLogado_Id;
            }
        }
    }
}