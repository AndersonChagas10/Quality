using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class WebServiceController : Controller
    {

        public JsonResult GetTelaTeste()
        {

            String html =   "<div class='dataCollection content'>" +
                            "<div class='level01List'>" +
                            "</div>" +
                            "<!--indicador desejado-->" +
                            "<div id = '3' class='level01'>" +
                            "<!--cabecalho do indicador-->" +
                            "<div class='level01Head head fontSize24 bgColorHead'>Carcass Contamination</div>" +
                            "<div class='level02Head painel bgColorPainel fontSize24'>Painel</div>" +
                            "<!--lista dos monitoramentos do indicador-->" +
                            "<div class='level02List' level01ID='3'>" +
                            "<!--monitoramento-->" +
                            "<div id = '22' class='level02 cursorPointer fontSize28'>" +
                            "<div class='row'>" +
                            "<div class='col-md-7 col-xs-7 level02Name hover01'>Area 01</div>" +
                            "<div class='col-md-5 col-xs-5 fontSize16'>" +
                            "<div class='hover01'><span>Defects:</span><span class='defects bold marginLeft10 fontSize18'>0</span></div>" +
                            "<div class='hover01'><span>Inspections:</span><span class='inspections bold marginLeft10 fontSize18'>1/10</span></div>" +
                            "</div>" +
                            "</div>" +
                            "</div>" +
                            "<div id = '23' class='level02 cursorPointer fontSize28'>" +
                            "<div class='row'>" +
                            "<div class='col-md-7 col-xs-7 hover01 '>Area 02</div>" +
                            "<div class='col-md-5 col-xs-5 fontSize16'>" +
                            "<div class='hover01'><span>Defects:</span><span class='defects bold marginLeft10 fontSize18'>0</span></div>" +
                            "<div class='hover01'><span>Inspections:</span><span class='inspections bold marginLeft10 fontSize18'>1/10</span></div>" +
                            "</div>" +
                            "</div>" +
                            "</div>" +
                            "<div id = '24' class='level02 cursorPointer fontSize28'>" +
                            "<div class='row'>" +
                            "<div class='col-md-7 col-xs-7 hover01'>Area 03</div>" +
                            "<div class='col-md-5 col-xs-5 fontSize16'>" +
                            "<div class='hover01'><span>Defects:</span><span class='defects bold marginLeft10 fontSize18'>0</span></div>" +
                            "<div class='hover01'><span>Inspections:</span><span class='inspections bold marginLeft10 fontSize18'>1/10</span></div>" +
                            "</div>" +
                            "</div>" +
                            "</div>" +
                            "<div id = '25' class='level02 cursorPointer fontSize28'>" +
                            "<div class='row'>" +
                            "<div class='col-md-7 col-xs-7 hover01'>Area 04</div>" +
                            "<div class='col-md-5 col-xs-5 fontSize16'>" +
                            "<div class='hover01'><span>Defects:</span><span class='defects bold marginLeft10 fontSize18'>0</span></div>" +
                            "<div class='hover01'><span>Inspections:</span><span class='inspections bold marginLeft10 fontSize18'>1/10</span></div>" +
                            "</div>" +
                            "</div>" +
                            "</div>" +
                            "</div>" +
                            "</div>" +
                            "</div>";

            return Json(html, JsonRequestBehavior.AllowGet);

        }











        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }

    }
}