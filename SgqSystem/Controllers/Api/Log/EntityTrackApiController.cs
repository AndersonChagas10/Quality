using Conformity.Application.Core.Log;
using Conformity.Domain.Core.DTOs;
using Conformity.Infra.CrossCutting;
using System.Collections.Generic;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Log
{
    [RoutePrefix("api/EntityTrackApi")]

    public class EntityTrackApiController : BaseAuthenticatedApiController
    {
        private readonly EntityTrackService _entityTrackService;

        public EntityTrackApiController(EntityTrackService entityTrackService,
            ApplicationConfig applicationConfig) : base(applicationConfig)
        {
            _entityTrackService = entityTrackService;
        }

        [Route("ObterLogs/{acaoId}")]
        [HttpGet]
        public IEnumerable<EntityTrackViewModel> ObterLogs(int acaoId)
        {
            return _entityTrackService.GetAll("Acao", acaoId);
        }
    }
}