using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO;
using Newtonsoft.Json.Linq;
using SgqSystem.Handlres;
using SgqSystem.ViewModels;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Example
{
    [HandleApi()]
    [RoutePrefix("api/Example")]
    public class ExampleApiController : BaseApiController
    {

        #region Construtor para injeção de dependencia

        private IExampleDomain _exampleDomain;

        public ExampleApiController(IExampleDomain exampleDomain)
        {
            _exampleDomain = exampleDomain;
        }

        #endregion

        #region Metodos disponíveis na API

        [HttpPost]
        [Route("AddExample")]
        public ContextExampleDTO AddExample([FromBody] ContextExampleViewModel paramsViewModel)
        {
            return _exampleDomain.AddUpdateExample(paramsViewModel);
        }

        #endregion

        [HttpPost]
        [HandleApi()]
        [Route("ExemploPost/{param1}/{param2}")]
        public string ExemploPost(string param1, string param2)
        {
            return param1 + "," + param2;
        }

        [HttpGet]
        [Route("ExemploGet/{param1}/{param2}")]
        public string ExemploGet(string param1, string param2)
        {
            return param1 + "," + param2;
        }

        [HttpGet]
        [Route("ExemploGet2/{idLevel1}/{idLevel2}/{idLevel3}")]
        public string ExemploGet2(int idLevel1, int idLevel2, int idLevel3)
        {
            return "a";
            //returna paramsViewModel;
        }

        [HttpGet]
        [Route("ExemploGet3/{idLevel1}/{idLevel2}/{idLevel3}")]
        public string ExemploGet3(string idLevel1, string idLevel2, string idLevel3)
        {
            return "a";
            //returna paramsViewModel;
        }

        [HttpGet]
        [Route("ChangeCultureTable/{cultureDesejada}")]
        public IEnumerable<DictionaryEntry> ChangeCultureTable(string cultureDesejada)
        {
            if (cultureDesejada != null) //CULTURE DIFERENTE DO PADRÃO
                if (cultureDesejada.Equals("en"))
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("");
                }
                else
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureDesejada);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureDesejada);
                }

            System.Resources.ResourceManager resourceManager = Resources.Resource.ResourceManager;

            var resourceSet = Resources.Resource.ResourceManager.GetResourceSet(Thread.CurrentThread.CurrentUICulture, true, false);

            return resourceSet.Cast<DictionaryEntry>();
        }

        [HttpPost]
        [Route("PostAlbum")]
        public string PostAlbum(JObject jsonData)
        {
            dynamic json = jsonData;
            JObject jalbum = json.Album;
            JObject juser = json.User;
            string token = json.UserToken;

            return string.Empty;
        }

        [HttpPost]
        [Route("PostAlbumJObject")]
        public JObject PostAlbumJObject(JObject jAlbum)
        {
            // dynamic input from inbound JSON
            dynamic album = jAlbum;

            // create a new JSON object to write out
            dynamic newAlbum = new JObject();

            // Create properties on the new instance
            // with values from the first
            newAlbum.AlbumName = " New";
            newAlbum.NewProperty = "something new";
            newAlbum.Songs = new JArray();

            using (var db = new SgqDbDevEntities())
            {
                dynamic teste = new JObject();
                teste = db.Database.SqlQuery<JObject>("Select * from SgqConfig").FirstOrDefault();
            }
            //foreach (dynamic song in album.Songs)
            //{
            //    song.SongName = song.SongName + " New";
            //    newAlbum.Songs.Add(song);
            //}

            return newAlbum;
        }
    }
}