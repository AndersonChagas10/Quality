using Dominio;
using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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

            //var xmlString = File.ReadAllText("C:\\Users\\GRT\\source\\repos\\sgq_pa\\DbScriptsVersion.xml");
            //var stringReader = new StringReader(xmlString);
            //var dsSet = new DataSet();
            //dsSet.ReadXml(stringReader);

            //// Loading from a file, you can also load from a stream
            //var xml = XDocument.Load("C:\\Users\\GRT\\source\\repos\\sgq_pa\\DbScriptsVersion.xml");


            //// Query the data and write out a subset of contacts
            //var query = from c in xml.Root.Descendants("card")
            //            //where c.Element("number").Value == cardNumber
            //            //&& c.Element("version").Value == version
            //            select c.Element("number").Value + " " +
            //                   c.Element("version").Value + " " +
            //                   c.Element("description").Value + " " +
            //                   c.Element("script").Value;

            StringBuilder result = new StringBuilder();
            foreach (XElement level1Element in XElement.Load("C:\\Users\\GRT\\source\\repos\\sgq_pa\\DbScriptsVersion.xml").Elements("card"))
            {
                if (level1Element.Element("number").Value.Contains(cardNumber) || level1Element.Element("version").Value.Contains(version))
                {
                    //result.AppendLine("<number> " + level1Element.Element("number").Value + " </number>");
                    //result.AppendLine("<version> " + level1Element.Element("version").Value + "< /version>");
                    //result.AppendLine("<description> " + level1Element.Element("description").Value + " </description>");
                    //result.AppendLine("<script> " + level1Element.Element("script").Value + " </script>");
                    //result.AppendLine(" ");

                    result.AppendLine("Insert into MigrationHistory(" + level1Element.Element("description").Value + "," + DateTime.Now.ToString() + ") VALUES({{" + level1Element.Element("version").Value + "}}|{{"+ level1Element.Element("description").Value +"}}, GetDate()); {{ " + level1Element.Element("script").Value + "}}" );
                }
            }


            //Insert into MigrationHistory(Name, AddDate) VALUES('{{SGQ-xxx}}|{{1.0.1}}|{{breve descrição}}', GetDate());
            //{ { script} }

            return Ok(result);
        }
    }
}
