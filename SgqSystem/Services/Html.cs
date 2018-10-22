﻿using DTO;
using SGQDBContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Web;

namespace SgqSystem.Services
{
    public class Html
    {
        /// <summary>
        /// Enumeradores Header
        /// </summary>
        public enum h
        {
            h1 = 1,
            h2 = 2,
            h3 = 3,
            h4 = 4,
            h5 = 5,
            h6 = 6
        }
        /// <summary>
        /// Enumeradores de Type
        /// </summary>
        public enum type
        {
            text,
            password,
            submit
        }

        public enum bootstrapcolor
        {
            _default,
            primary,
            success,
            info,
            warning,
            danger
        }

        /// <summary>
        /// Retorna uma div
        /// </summary>
        /// <param name="id">Atributo Id</param>
        /// <param name="classe">Atributo Classe</param>
        /// <param name="name">Atributo Name</param>
        /// <param name="outerhtml">Conteudo</param>
        /// <param name="style">Styles</param>
        /// <param name="tags">Tags</param>
        /// <returns></returns>
        public string div(string id = null, string classe = null, string name = null, string outerhtml = null, string style = null, string tags = null, string outerAngular = null)
        {

            if (!string.IsNullOrEmpty(tags))
            {
                tags = " " + tags;
            }
            if (!string.IsNullOrEmpty(outerAngular))
                outerhtml += outerAngular;

            return "<div id=\"" + id + "\" class=\"" + classe + "\" name=\"" + name + "\" style=\"" + style + "\"" + tags + ">" + outerhtml + "</div>";
        }

        /// <summary>
        /// Retorna uma div
        /// </summary>
        /// <param name="id">Atributo Id</param>
        /// <param name="classe">Atributo Classe</param>
        /// <param name="name">Atributo Name</param>
        /// <param name="outerhtml">Conteudo</param>
        /// <param name="style">Styles</param>
        /// <param name="tags">Tags</param>
        /// <returns></returns>
        public void div(bool t, StringBuilder outerhtml, string id = null, string classe = null, string name = null,string style = null, string tags = null, string outerAngular = null)
        {

            if (!string.IsNullOrEmpty(tags))
            {
                tags = " " + tags;
            }
            if (!string.IsNullOrEmpty(outerAngular))
                outerhtml.Append(outerAngular);

            outerhtml.Insert(0, "<div id=\"" + id + "\" class=\"" + classe + "\" name=\"" + name + "\" style=\"" + style + "\"" + tags + ">");
            outerhtml.Append("</div>");
        }
        /// <summary>
        /// Retorna um span
        /// </summary>
        /// <param name="id">Atributo Id</param>
        /// <param name="classe">Atributo Classe</param>
        /// <param name="name">Atributo Name</param>
        /// <param name="outerhtml">Conteudo</param>
        /// <param name="style">Styles</param>
        /// <returns></returns>
        public string span(string id = null, string classe = null, string name = null, string outerhtml = null, string style = null, string attr = null)
        {
            return "<span id=\"" + id + "\" class=\"" + classe + "\" name=\"" + name + "\" style=\"" + style + "\" "+ attr + ">" + outerhtml + "</span>";
        }
        /// <summary>
        /// Retorna uma quebra de linha <br />
        /// </summary>
        /// <returns></returns>
        public string br()
        {
            return "<br />";
        }
        /// <summary>
        /// Retorna um form
        /// </summary>
        /// <param name="id">Atributo Id</param>
        /// <param name="classe">Atributo classe</param>
        /// <param name="name">Atributo nome</param>
        /// <param name="outerhtml">Conteudo</param>
        /// <param name="style">Styles</param>
        /// <returns></returns>
        public string form(string id = null, string classe = null, string name = null, string outerhtml = null, string style = null)
        {
            return "<form  id=\"" + id + "\" class=\"" + classe + "\" name=\"" + name + "\" style=\"" + style + "\">" + outerhtml + "</form>";
        }
        /// <summary>
        /// Retornam um head <h></h>
        /// </summary>
        /// <param name="headnumber">Tamanho do Head</param>
        /// <param name="id">Atributo id</param>
        /// <param name="classe">Atributo class</param>
        /// <param name="name">Atributo names</param>
        /// <param name="outerhtml">Conteudo</param>
        /// <param name="style">Style</param>
        /// <returns></returns>
        public string head(h headnumber, string id = null, string classe = null, string name = null, string outerhtml = null, string style = null)
        {
            return "<h" + (int)headnumber + "  id=\"" + id + "\" class=\"" + classe + "\" name=\"" + name + "\" style=\"" + style + "\">" + outerhtml + "</h" + (int)headnumber + ">";
        }
        /// <summary>
        /// Retorna um label
        /// </summary>
        /// <param name="labelfor">Atributo labelfor</param>
        /// <param name="classe">Atributo class</param>
        /// <param name="style">Styles</param>
        /// <param name="outerhtml">Conteudo</param>
        /// <returns></returns>
        public string label(string labelfor = null, string classe = null, string style = null, string outerhtml = null)
        {
            return "<label for=\"" + labelfor + "\" class=\"" + classe + "\" style=\"" + style + "\">" + outerhtml + "</label>";
        }
        /// <summary>
        /// Retorna um input
        /// </summary>
        /// <param name="type">Atributo type (Ex: input, password...)</param>
        /// <param name="id">Atributo id</param>
        /// <param name="classe">Atributo class</param>
        /// <param name="placeholder">Atributo placeholder</param>
        /// <param name="required">Requerido</param>
        /// <param name="disabled">Desabilitado</param>
        /// <returns></returns>
        public string input(type type = type.text, string id = null, string classe = null, string placeholder = null, bool required = false, bool disabled = false)
        {
            string requiredTag = null;
            if (required == true)
            {
                requiredTag = " required";
            }
            string disabledTag = null;
            if (disabled == true)
            {
                disabledTag = " disabled";
            }

            classe += " form-control";
            return "<input type=\"" + type.ToString() + "\" id=\"" + id + "\" class=\"" + classe + "\" placeholder=\"" + placeholder + "\"" + requiredTag + disabledTag + ">";
        }
        /// <summary>
        /// Retorna um button
        /// </summary>
        /// <param name="label">Label button</param>
        /// <param name="type">Atributo type (Ex: submit...)</param>
        /// <param name="id">Atributo id</param>
        /// <param name="classe">Atributo class</param>
        /// <param name="dataloading">Mensagem que aparece no botão enquando a requisição é executada</param>
        /// <param name="disabled">Desabilitado</param>
        /// <returns></returns>
        public string button(string label, type type = type.submit, string id = null, string classe = null, string dataloading = null, bool disabled = false, string style = null)
        {
            if (!string.IsNullOrEmpty(dataloading))
            {
                dataloading = " data-loading-text=\"" + dataloading + "\"";
            }

            classe += " btn";


            string disabledTag = null;
            if (disabled == true)
            {
                disabledTag = " disabled";
            }

            return "<button type=\"" + type.ToString() + "\" id=\"" + id + "\" class=\"" + classe.Trim() + "\" " + dataloading + disabledTag + " style=\""+ style + "\">" + label + "</button>";
        }
        /// <summary>
        /// Retorna um select
        /// </summary>
        /// <param name="options">Options do select</param>
        /// <param name="id">Atributo id</param>
        /// <param name="classe">Atributo class</param>
        /// <param name="disabled">Desabilitado</param>
        /// <returns></returns>
        public string select(string options, string id = null, string classe = null, bool disabled = false, string style = null)
        {
            string disabledTag = null;
            if (disabled == true)
            {
                disabledTag = " disabled";
            }

            classe += " form-control";

            return "<select id=\"" + id + "\" class=\"" + classe.Trim() + "\"" + disabledTag + " style=\"" + style + "\">" + options + "</select>";
        }
        /// <summary>
        /// Retorna option do select
        /// </summary>
        /// <param name="value">Atributo value</param>
        /// <param name="label">Atributo label</param>
        /// <param name="tags">Tags</param>
        /// <returns></returns>
        public string option(string value, string label, string tags = null)
        {
            if (!string.IsNullOrEmpty(tags))
            {
                tags = " " + tags;
            }
            return "<option value=\"" + value + "\"" + tags + ">" + label + "</option>";
        }
        public string listgroup(string id = null, string classe = null, string outerhtml = null, string tags = null)
        {
            classe += " list-group";

            if (!string.IsNullOrEmpty(tags))
            {
                tags = " " + tags;
            }

            return "<ul id=\"" + id + "\" class=\"" + classe.Trim() + "\"" + tags + ">" +
                    outerhtml +
                    "</ul>";
        }
        public string listgroupItem(string id = null, string classe = null, string tags = null, string outerhtml = null, string totalDefeitos = null)
        {
            classe += " list-group-item";

            if (!string.IsNullOrEmpty(tags))
            {
                tags = " " + tags;
                tags = tags.Trim();
            }

            return "<li id=\"" + id + "\" class=\"" + classe.Trim() + "\"" + tags + " totalDefeitos='" + totalDefeitos + "'>" + outerhtml + "</li>";
        }

        public string accordeon(string id, string label, string classe = null, string outerhtml = null, bool aberto = false, bootstrapcolor? color = null, string accordeonId = "", string othersTags = null)
        {
            string collapseIn = " in";
            if (aberto == false)
            {
                collapseIn = null;
            }

            string colorPanel = "default";
            if (color != null)
            {
                colorPanel = color.ToString();
            }

            if (!string.IsNullOrEmpty(classe))
            {
                classe = " " + classe;
            }

            string accordeonIdTag = null;
            if (accordeonId.Length > 0)
            {
                accordeonIdTag = " id=\"" + accordeonId + "\"";
            }

            return "  <div class=\"panel-group" + classe + "\"" + accordeonIdTag + " " + othersTags + ">                                                                                                          " +
                    "    <div class=\"panel panel-" + colorPanel + "\">                                                                                          " +
                    "      <div class=\"panel-heading\" role=\"tab\" id=\"heading" + id + "\">                                                                                                    " +
                    "        <h4 class=\"panel-title\">                                                                                                     " +
                    "          <a role=\"button\" data-toggle=\"collapse\" href=\"#collapse" + id + "\" class=\"\" aria-expanded=\"true\" aria-controls=\"collapse" + id + "\">" + label + "</a>                " +
                    "      </div>                                                                                                                           " +
                    "        </h4>                                                                                                                          " +
                    "         <div id=\"collapse" + id + "\" class=\"panel-collapse collapse" + collapseIn + "\" role=\"tabpanel\" aria-labelledby=\"heading" + id + "\">                     " +
                    "        <ul class=\"list-group\" style=\"margin:0\">                                                                                   " +
                             outerhtml +
                    "        </ul>                                                                                                                          " +
                    "      </div>                                                                                                                           " +
                    "    </div>                                                                                                                             " +
                    "  </div>                                                                                                                               ";

        }

        public string link(string id = null, string classe = null, string href = null, string tags = null, string outerhtml = null)
        {
            if (string.IsNullOrEmpty(href))
            {
                href = "#";
            }
            if (!string.IsNullOrEmpty(tags))
            {
                tags = " " + tags;
            }
            if(href != null && href != "#")
            {
                href = " href=\"" + href + "\"";
            }else
            {
                href = "";
            }
            return "<a id=\"" + id + "\" " + href + " class=\"" + classe + "\"" + tags + " style=\"cursor: pointer\">" + outerhtml + "</a>";
        }

        public string campoBinario(string id, string booltrueName, string boolfalseName, string classe = null)
        {
            //classe = " list-group-item level03  row";

            //string binario = "<li id=\"" + id + "\" class=\"" + classe.Trim() + "\">" +
            //                     "<div class=\"col-xs-9\"></div>" +
            //                     "<div class=\"col-xs-3 text-center\">" +
            //                        "<span class=\"pull-right marginRight30 response\" value=\"1\" booltrueName=\"" + booltrueName + "\" boolfalseName=\"" + boolfalseName + "\">" + booltrueName + "</span>" +
            //                     "</div>" +
            //                 "</li>";
            string binario = "<span class=\"pull-right marginRight30 response\" value=\"1\" booltrueName=\"" + booltrueName + "\" boolfalseName=\"" + boolfalseName + "\">" + booltrueName + "</span>";

            return binario;
        }

        public string campoIntervalo(string id, decimal intervalMin, decimal intervalMax, decimal defaultValue = 0, string unitName = null, string classe = null)

        {
            //definir min value //min=\"0\" 
            //definir max value
            //definir default value

            if (!string.IsNullOrEmpty(classe))
            {
                classe = " " + classe;
            }

            string intervalo = "<div class=\"input-group input-group-sm width180 pull-right" + classe + "\" style=\"width: 100% !important;\">                                                                                                  " +
                                 "    <span class=\"input-group-btn btn-minus\">                                                                                                                              " +
                                 "         <button class=\"btn btn-default\" type=\"button\" style=\"padding:0px 2px;\">                                                                                                                 " +
                                 "             <i class=\"fa fa-minus\" aria-hidden=\"true\"></i>                                                                                                             " +
                                 "             </button></span><input type=\"text\" value=\"\" class=\"form-control text-center levelValue interval\">     " +
                                 "             <span class=\"input-group-btn btn-plus\"><button class=\"btn btn-default\" type=\"button\" style=\"padding:0px 2px;\">                                                                    " +
                                 "             <i class=\"fa fa-plus\" aria-hidden=\"true\"></i>                                                                                                              " +
                                 "         </button>                                                                                                                                                          " +
                                 "     </span>                                                                                                                                                                " +
                                 "</div>   ";

            //string intervalo = "<div class=\"input-group input-group-sm width180 pull-right" + classe + "\" style=\"width: 100% !important;\">                                                                                                  " +
            //                     "              <input type=\"text\" class=\"form-control text-center levelValue interval\" value=\"\">     " +
            //                     "</div> ";
            return intervalo;
        }

        public string campoIntervaloTexto(string id, decimal intervalMin, decimal intervalMax, decimal defaultValue = 0, string unitName = null, string classe = null)
        {
            //definir min value //min=\"0\" 
            //definir max value
            //definir default value

            if (!string.IsNullOrEmpty(classe))
            {
                classe = " " + classe;
            }

            string intervalo = $@"<div class=""input-group input-group-sm width180 pull-right { classe }"" style=""width: 100% !important;"">
                                     <span class=""input-group-btn btn-minus"">
                                          <input type=""text"" value="""" class=""form-control text-center levelValue interval""  style=""width:30%;"" >
                                          <textarea class=""form-control text-center levelValueNotes naoValidarInput"" style=""width:70%;"" placeholder=""{ Resources.Resource.notes }""></textarea>
                                     </span>
                                 </div>";

            //string intervalo = "<div class=\"input-group input-group-sm width180 pull-right" + classe + "\" style=\"width: 100% !important;\">                                                                                                  " +
            //                     "              <input type=\"text\" class=\"form-control text-center levelValue interval\" value=\"\">     " +
            //                     "</div> ";
            return intervalo;
        }

        public string campoTexto(string id, string classe = null)
        {
            //definir min value //min=\"0\" 
            //definir max value
            //definir default value

            if (!string.IsNullOrEmpty(classe))
            {
                classe = " " + classe;
            }

            string texto = "<div class=\"input-group input-group-sm width180 pull-right" + classe + "\" style=\"width: 100% !important;\">                            " +
                                 "    <span class=\"input-group-btn btn-minus\"> </span>             " +
                                 "         <input type=\"text\" class=\"form-control text-center levelValue texto naoValidarInput\" style=\"width:100%;\">     " +
                                 "</div>                                                    ";
            return texto;
        }

        public string campoObservacao(string id, string classe = null)
        {
            //definir min value //min=\"0\" 
            //definir max value
            //definir default value

            if (!string.IsNullOrEmpty(classe))
            {
                classe = " " + classe;
            }

            string texto = "<div class=\"input-group input-group-sm width180 pull-right" + classe + "\" style=\"width: 100% !important;\">                            " +
                                 "         <input type=\"text\" class=\"form-control text-center levelValue naoValidarInput\" style=\"width:100%;\">     " +
                                 "</div>                                                    ";
            return texto;
        }

        public string campoTextoBinario(string id, string classe = null)
        {
            //definir min value //min=\"0\" 
            //definir max value
            //definir default value

            if (!string.IsNullOrEmpty(classe))
            {
                classe = " " + classe;
            }

            string texto = "<div class=\"input-group input-group-sm width180 pull-right" + classe + "\" style=\"width: 100% !important;\">                            " +
                                 "    <span class=\"input-group-btn btn-minus\"> </span>             " +
                                 "         <input type=\"text\" class=\"form-control text-center levelValue naoValidarInput\" style=\"width:100%;\">     " +
                                 "</div>                                                    ";
            return texto;
        }

        public string campoTextoMinutos_(string id, string classe = null)
        {
            string texto = "<div class=\"input-group input-group-sm width180 pull-right" + classe + "\" style=\"width: 100% !important;\">                            " +
                                 "         <input type=\"time\" class=\"form-control text-center levelValue antes naoValidarInput\" style=\"width:50%;\">     " +
                                 "         <input type=\"time\" class=\"form-control text-center levelValue depois naoValidarInput\" style=\"width:50%;\">     " +
                                 "</div>                                                    ";
            return texto;
        }

        public string campoTextoMinutos(string id, decimal intervalMin, decimal intervalMax, decimal defaultValue = 0, string unitName = null, string classe = null)

        {
            //definir min value //min=\"0\" 
            //definir max value
            //definir default value

            if (!string.IsNullOrEmpty(classe))
            {
                classe = " " + classe;
            }

            string intervalo = "<div class=\"input-group input-group-sm width180 pull-right" + classe + "\" style=\"width: 100% !important;\">                                                                                                  " +
                                 "    <span class=\"input-group-btn btn-minus\">                                                                                                                              " +
                                  "         <input type=\"time\" class=\"form-control text-center levelValue antes inputMinutes\" style=\"width:50%;\">     " +
                                 "         <input type=\"time\" class=\"form-control text-center levelValue depois inputMinutes\" style=\"width:50%;\">     " +
                                 "     </span>                                                                                                                                                                " +
                                 "</div>   ";

            //string intervalo = "<div class=\"input-group input-group-sm width180 pull-right" + classe + "\" style=\"width: 100% !important;\">                                                                                                  " +
            //                     "              <input type=\"text\" class=\"form-control text-center levelValue interval\" value=\"\">     " +
            //                     "</div> ";
            return intervalo;
        }

        public string campoRangeSlider(string id, decimal min, decimal max, string classe = null, string idRandomico = null)
        {
            //var idRandomico = "valor_range_" + new Random().Next(999999);
            string texto = "<div class=\"input-group input-group-sm width180 pull-right" + classe + "\" style=\"width: 100% !important;\">                            " +
                                 "         <input type=\"range\" oninput=\"document.getElementById('" + idRandomico + "').innerHTML = this.value;\" min=\"" + Convert.ToInt32(min) + "\" max=\"" + Convert.ToInt32(max) + "\" class=\"form-control text-center levelValue naoValidarInput\" style=\"width:80%;\">     " +
                                 "         <span id=\"" + idRandomico + "\" style=\"display: block; float:right; width: 20%; text-align:center;\"></span>     " +
                                 "</div>                                                    ";
            return texto;
        }

        public string campoResultado(string id, string value, string classe = null)
        {
            string texto = "<div class=\"input-group input-group-sm width180 pull-right" + classe + "\" style=\"width: 100% !important;\">" +
                                 "         <input readonly type=\"text\" resultado=\"" + value + "\" class=\"form-control text-center levelValue interval\" style=\"width:100%;\">     " +
                                 "</div>";
            return texto;
        }


        public string campoOperador(string id, string classe = null)
        {

            string texto = "<div class=\"input-group input-group-sm width180 pull-right" + classe + "\" style=\"width: 100% !important;\">                            " +
                                 "    <span class=\"input-group-btn btn-minus\"> </span>             " +
                                 "         <input type=\"text\" class=\"form-control text-center levelValue Operador naoValidarInput\" style=\"width:100%;\">     " +
                                 "</div>                                                    ";
            return texto;
        }

        public string campoNumeroDeDefeitos(string id, decimal intervalMin, decimal intervalMax, decimal defaultValue = 0, string unitName = null, string classe = null)
        {
            //definir min value //min=\"0\" 
            //definir max value
            //definir default value

            if (!string.IsNullOrEmpty(classe))
            {
                classe = " " + classe;
            }

            string intervalo = "<div class=\"input-group input-group-sm width180 pull-right" + classe + "\" style=\"width: 100% !important;\">                                                                                                  " +
                                 "    <span class=\"input-group-btn btn-minus\">                                                                                                                              " +
                                 "         <button class=\"btn btn-default\" type=\"button\" style=\"padding:0px 2px;\">                                                                                                                 " +
                                 "             <i class=\"fa fa-minus\" aria-hidden=\"true\"></i>                                                                                                             " +
                                 "             </button></span><input type=\"text\" class=\"form-control text-center levelValue defects\" value=\"0\">     " +
                                 "             <span class=\"input-group-btn btn-plus\"><button class=\"btn btn-default\" type=\"button\" style=\"padding:0px 2px;\">                                                                    " +
                                 "             <i class=\"fa fa-plus\" aria-hidden=\"true\"></i>                                                                                                              " +
                                 "         </button>                                                                                                                                                          " +
                                 "     </span>                                                                                                                                                                " +
                                 "</div>                                                                                                                                                                      ";
            return intervalo;
        }

        public string campoCalculado(string id, decimal intervalMin, decimal intervalMax, decimal defaultValue = 0, string unitName = null, string classe = null)
        {
            //definir min value //min=\"0\" 
            //definir max value
            //definir default value

            if (!string.IsNullOrEmpty(classe))
            {
                classe = " " + classe;
            }

            string calculado = "<div class=\"input-group input-group-sm width180 pull-right" + classe + "\" style=\"width: 100% !important;\">                                                                                                  " +
                                 "    <input type=\"text\" style=\"width:25px; padding: 0px;\" value=\"\" class=\"form-control text-center input01 \">     " +
                                 " <span style=\"padding-left:2px;padding-right:2px; font-size:10px\"><b>x10^</b></span>" +
                                 "    <input type=\"text\" style=\"width:25px; padding: 0px;\" value=\"\" class=\"form-control text-center input02 \">     " +
                                 " <br><span style='font-size:x-small;'>      " +
                                 " <span class=\"value \" style=\"text-weight: bold;\"></span> " +
                                 " <span class=\"valueDecimal \" style=\"text-weight: bold;\"></span>" +
                                 " </span> " +
                                 "</div>                                                                                                                                                                      ";
            return calculado;
        }

        public string campoIntervaloMinuto(string id, decimal intervalMin, decimal intervalMax, decimal defaultValue = 0, string unitName = null, string classe = null)
        {

            string calculado = "<div class=\"input-group input-group-sm width180 pull-right " + classe + "\" style=\"width: 100% !important;\">   " +
                                 "    <input type=\"text\" style=\"width:25px; padding: 0px;\" value=\"\" class=\"form-control text-center input01 \">     " +
                                 " <span style=\"padding-left:2px;padding-right:2px; font-size:10px\"><b>x10^</b></span>" +
                                 "    <input type=\"text\" style=\"width:25px; padding: 0px;\" value=\"\" class=\"form-control text-center input02 \">     " +
                                 " </span> " +
                                 "</div> ";
            return calculado;
        }
        //public string level2(string id, string label, string classe = null, decimal defects = 0, int evaluate = 1, int sample = 1, bool reaudit = false, bool correctiveaction = false, bool phase = false,
        //                     string alertlevel1 = null, string alertlevel2 = null, string alertlevel3 = null, string AlertLevel = null, string ParFrequency_Id = null)

        public string level2(string id, string label,
                             string classe = null, decimal defects = 0, int evaluate = 1, int sample = 1,
                             bool reaudit = false, bool correctiveaction = false, bool phase = false,
                             bool HasSampleTotal = false, bool IsEmptyLevel3 = false, string level1Group_Id = "",
                             int RuleId = 0, string RuleValue = null, decimal AlertValue = 0, int ParFrequency_Id = 0,
                             bool HasTakePhoto = false)
        {

            string tagLevel1Group = null;
            if (level1Group_Id.Length > 0)
            {
                tagLevel1Group = " parlevel1_id_group=\"" + level1Group_Id + "\"";
            }
            return link(
                           id: id,
                           classe: "level2 " + classe,
                           // tags: "defects=\"" + defects + "\" evaluate=\"" + evaluate + "\" sample=\"" + sample + "\" av=\"0\" avdb=\"0\" ncdb=\"0\" avlocal=\"0\" nclocal=\"0\" nc=\"0\"",
                           tags: "defects=\"" + defects +
                           "\" evaluate=\"" + evaluate +
                           "\" sample=\"" + sample +
                           "\" weievaluation=\"0" +
                           "\" evaluatetotal=\"0" +
                           "\" defectstotal=\"0\" weidefects=\"0\"" +
                           " totallevel3evaluation=\"0\"" +
                           " totallevel3withdefects=\"0\"" +
                           " parfrequency_id=\"" + ParFrequency_Id + "\"" +
                           " hassampletotal=\"" + HasSampleTotal.ToString().ToLower() + "\"" +
                           " isemptylevel3=\"" + IsEmptyLevel3.ToString().ToLower()
                           + "\" ParNotConformityRule_id=\"" + RuleId.ToString()
                           + "\" ParNotConformityRule_value=\"" + RuleValue
                           + "\" AlertValue=\"" + AlertValue.ToString()
                           + "\" reaudit=\"" + reaudit.ToString().ToLower()
                           + "\" hastakephoto=\"" + HasTakePhoto.ToString().ToLower()
                           + "\"" + tagLevel1Group,
                           outerhtml: span(outerhtml: label, classe: "levelName")
                       );
        }
        public string level3(SGQDBContext.ParLevel3 parLevel3, string input, string classe = null, string labelsInputs = null)
        {
            //Coloca botão de não avaliado ParLevel3
            //vai ter que ter uma configuração na parametrização
            string btnNaoAvaliado = button(
                                       label: span(
                                                    classe: "cursorPointer iconsArea",
                                                    outerhtml: "N/A"
                                                ),
                                       classe: "btn-warning btnNotAvaliable na font11"
                                   );


            string tags = " weight=\"" + parLevel3.Weight + "\" intervalmin=\"" + parLevel3.IntervalMin.ToString().Replace(",", ".") + "\" intervalmax=\"" + parLevel3.IntervalMax.ToString().Replace(",", ".") + "\" weievaluation=\"0\" inputtype=\"" + parLevel3.ParLevel3InputType_Id + "\" hastakephoto=\""+parLevel3.HasTakePhoto.ToString().ToLower() + "\"";


            //Gera o level3
            string level3 = link(
                                        outerhtml: span(outerhtml: parLevel3.Name, classe: "levelName") + "<br>" + span(outerhtml: "", classe: "levelNameDebug"),
                                        classe: "col-xs-4"
                                        );

            //gera os labels
            string labels = "";
            if (parLevel3.ParLevel3InputType_Id == 5)
            {
                labels = div(
                                    outerhtml: labelsInputs,
                                    classe: "col-xs-0"
                                );
            }
            else
            {
                labels = div(
                                    outerhtml: labelsInputs,
                                    classe: "col-xs-3"
                                );
            }

            //gera os contadores
            string counters = "";
            if (parLevel3.ParLevel3InputType_Id == 5)
            {
                counters = div(
                              outerhtml: input,
                              classe: "col-xs-6 counters"
                              );
            }
            else
            {
                counters = div(
                               outerhtml: input,
                               classe: "col-xs-3 counters"
                               );
            }


            //gera os botoes
            string buttons = div(
                                        outerhtml: btnNaoAvaliado,
                                        classe: "col-xs-2",
                                        style: "text-align:right"
                                        );


            return listgroupItem(
                                    id: parLevel3.Id.ToString(),
                                    classe: "level3 row" + classe,
                                    tags: tags,
                                    outerhtml: level3 +
                                                labels +
                                                counters +
                                                buttons
                                );

        }
        public string user(int UserSGQ_Id, string UserSGQ_Name, string UserSGQ_Login, string UserSGQ_Pass, string Role, int ParCompany_Id, string ParCompany_Name, IEnumerable<RoleXUserSgq> roles)
        {
            string user = "<div class=\"user\" userid=\"" + UserSGQ_Id + "\" username=\"" + UserSGQ_Name + "\" userlogin=\"" + UserSGQ_Login.ToLower() + "\" userpass=\"" + UserSGQ_Pass + "\" userprofile=\"" + Role + "\" unidadeid=\"" + ParCompany_Id + "\" unidadename=\"" + ParCompany_Name + "\">";

            if (roles != null)
            {
                foreach (RoleXUserSgq role in roles)
                {
                    if (role.Type == 0
                        || (role.Type == 3 && role.RoleJBS != null && role.RoleSGQ != null)
                        || (role.Type == 1 && role.RoleSGQ != null)
                        || (role.Type == 2 && role.RoleJBS != null))
                    {
                        user += "<div class='role'>" + role.HashKey + "</div>";
                    }
                }
            }


            //user += "<div class='role'>comp001</div><div class='role'>comp002</div><div class='role'>comp004</div>";

            user += "</div>";

            return user;
        }
        public string level1(SGQDBContext.ParLevel1 ParLevel1, string tipoTela, int totalAvaliado, decimal totalDefeitos, decimal alertNivel1, decimal alertNivel2,
                             string alertaNivel3, int alertaAtual, int avaliacaoultimoalerta, int monitoramentoultimoalerta, decimal volumeAlertaIndicador, decimal metaIndicador,
                             decimal numeroAvaliacoes, decimal metaDia, decimal metaTolerancia, decimal metaAvaliacao,
                             bool IsLimitedEvaluetionNumber, IEnumerable<ParRelapse> listParRelapse, string ParCluster_Id)
        {

            StringBuilder tags = new StringBuilder("parconsolidationtype_id=\"" + ParLevel1.ParConsolidationType_Id + "\" parfrequency_id=\"" + ParLevel1.ParFrequency_Id + "\" hasalert=\"" + ParLevel1.HasAlert.ToString().ToLower() + "\" isspecific=\"" + ParLevel1.IsSpecific.ToString().ToLower() + "\" totalavaliado=\"" + totalAvaliado + "\" totaldefeitos=\"" + totalDefeitos + "\" volumeAlertaIndicador=\"" + volumeAlertaIndicador + "\" metaIndicador=\"" + metaIndicador + "\" numeroAvaliacoes=\"" + numeroAvaliacoes + "\" metaDia=\"" + metaDia + "\" metaTolerancia=\"" + metaTolerancia + "\" metaAvaliacao=\"" + metaAvaliacao + "\" alertanivel1=\"" + alertNivel1 + "\" alertanivel2=\"" + alertNivel2 + "\" alertanivel3=\"" + alertaNivel3 + "\" alertaatual=\"" + alertaAtual + "\" avaliacaoultimoalerta=\"" + avaliacaoultimoalerta + "\" monitoramentoultimoalerta=\"" + monitoramentoultimoalerta + "\" av=\"0\" avdb=\"0\" ncdb=\"0\" avlocal=\"0\" nclocal=\"0\" nc=\"0\" haverealtimeconsolidation=\"" + ParLevel1.haveRealTimeConsolidation.ToString().ToLower() + "\" realtimeconsolitationupdate=\"" + ParLevel1.RealTimeConsolitationUpdate + "\" islimitedevaluetionnumber=\"" + ParLevel1.IsLimitedEvaluetionNumber.ToString().ToLower() + "\" hashkey=\"" + ParLevel1.hashKey + "\" ispartialsave=\"" + ParLevel1.IsPartialSave.ToString().ToLower() + "\" hascompleteevaluation=\"" + ParLevel1.HasCompleteEvaluation.ToString().ToLower() + "\" hasgrouplevel2=\"" + ParLevel1.HasGroupLevel2.ToString().ToLower() + "\" reaudit=\"" + ParLevel1.IsReaudit.ToString().ToLower() + "\" editlevel2=\""+ParLevel1.EditLevel2.ToString().ToLower() + "\" hastakephoto=\"" + ParLevel1.HasTakePhoto.ToString().ToLower()+"\" ParCluster_Id=\"" + ParCluster_Id + "\"");

            string btnReaudit = button(Resources.Resource.reaudit, type.submit, "", classe: "btn-primary pull-right btnReaudit btn-sm hide", style: "margin-left: 4px;");
            string btnCA = button(Resources.Resource.corrective_action, type.submit, "", classe: "btn-danger pull-right btnCALevel1 btn-sm hide");
                        
            if(listParRelapse.Count() > 0)
            {
                var i = 1;
                foreach (var parRelapse in listParRelapse)
                {
                    tags.Append(" phase" + i + "='" + parRelapse.ParFrequency_Id + ";" + parRelapse.EffectiveLength + ";"+ parRelapse.NcNumber+"' ");
                    i++;
                }
            }

            string level01 = link(
                                id: ParLevel1.Id.ToString(),
                                classe: "level1 col-xs-7 " + tipoTela,
                                //Aqui vai as tags do level01
                                tags: tags.ToString(),
                                outerhtml: span(outerhtml: ParLevel1.Name, classe: "levelName")
                                );
            //Adiciona Div Lateral
            level01 += div(
                            //aqui vai os botoes
                            outerhtml: btnReaudit + btnCA,
                            classe: "userInfo col-xs-5");
            return level01;
        }


        public string cluster(string ParCluster_Id, string ParCluster_Name)
        {

            StringBuilder tags = new StringBuilder("parcluster_id=\"" + ParCluster_Id + "\" parcluster_name=\"" + ParCluster_Name + "\"");

            string clusterList = link(
                                id: ParCluster_Id,
                                classe: "cluster col-xs-7 ",
                                //Aqui vai as tags do level01
                                tags: tags.ToString(),
                                outerhtml: span(outerhtml: ParCluster_Name, classe: "levelName")
                                );
            //Adiciona Div Lateral
            clusterList += div(
                            //aqui vai os botoes
                            
                            classe: "userInfo col-xs-5");
            return clusterList;
        }

        public string painelCounters(IEnumerable<SGQDBContext.ParCounter> parCounterList, string css = "")
        {
            if (parCounterList.Count() == 0)
            {
                return "";
            }
            else
            {
                string countersArray = "";

                foreach (SGQDBContext.ParCounter parCounter in parCounterList)
                {
                    countersArray += counter(parCounter.Counter, parCounter.Level, parCounter.indicador, parCounter.Local);
                }
                return countersArray;
            }

            //div(outerhtml: countersLine, classe: "counters row " + classe, style: "background-color: #f1f1f1; padding-top: 5px;padding-bottom:5px;");
        }
        public string counter(string label, string level, string indicador, string headerLevel)
        {
            //return "<span class=\"counter " + classe + "\"><b><span class=\"labelCounter\">" + label.Trim() + "</span></b>: <span class=\"value\">" + value.Trim() + "</span></span>";
            return "<span class=\"counter hide col-xs-4\" counter=\""+label+"\" level=\"" + level + "\" indicador=\"" + indicador + "\" headerLevel=\"" + headerLevel + 
                "\"><b><span class=\"labelCounter\">" + label+"</span></b>: <b><span class=\"value\">0</span></span></b>";
        }

        public DictionaryEntry getResource(string value)
        {
            System.Resources.ResourceManager resourceManager = Resources.Resource.ResourceManager;

            if (resourceManager == null) //se portugues
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("");
            }

            var list = resourceManager.GetResourceSet(
                Thread.CurrentThread.CurrentUICulture, true, false).Cast<DictionaryEntry>();

            foreach (var r in list)
            {
                if (r.Key.ToString() == value)
                    return r;
            }

            return new DictionaryEntry();
        }
    }


}