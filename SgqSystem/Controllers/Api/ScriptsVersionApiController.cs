using Dominio;
using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;

namespace SgqSystem.Controllers.Api
{
    [RoutePrefix("api/ScriptsVersion")]
    [HandleApi()]
    public class ScriptsVersionApiController : ApiController
    {
        public SgqDbDevEntities db = new SgqDbDevEntities();

        public class XmlObj {
            public string CardNumber { get; set; }
            public string Version { get; set; }
            public string Description { get; set; }
            public string Script { get; set; }
        }

        [HttpGet]
        [Route("GetScriptsVersion")]
        public IHttpActionResult GetScriptsVersion([FromUri] string cardNumber, string version)
        {
            //XmlDocument doc = new XmlDocument();
            //doc.Load("C:\\Users\\GRT\\source\\repos\\sgq_pa\\DbScriptsVersion.xml");

            List<XmlObj> objList = new List<XmlObj>();

            var xmlString = File.ReadAllText("C:\\Users\\GRT\\source\\repos\\sgq_pa\\DbScriptsVersion.xml");
            var stringReader = new StringReader(xmlString);
            var dsSet = new DataSet();
            dsSet.ReadXml(stringReader);

            // Loading from a file, you can also load from a stream
            var xml = XDocument.Load("C:\\Users\\GRT\\source\\repos\\sgq_pa\\DbScriptsVersion.xml");


            // Query the data and write out a subset of contacts
            var query = from c in xml.Root.Descendants("card")
                        //where c.Element("number").Value == cardNumber
                        //&& c.Element("version").Value == version
                        select c.Element("number").Value + " " +
                               c.Element("version").Value + " " +
                               c.Element("description").Value + " " +
                               c.Element("script").Value;

            return Ok(dsSet);
        }
    }
}
