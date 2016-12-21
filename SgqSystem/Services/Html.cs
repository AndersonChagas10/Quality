using System;
using System.Collections.Generic;
using System.Linq;
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

        public enum  bootstrapcolor
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
        public string div(string id = null, string classe = null, string name = null, string outerhtml = null, string style=null, string tags=null) {

            if(!string.IsNullOrEmpty(tags))
            {
                tags = " " + tags;
            }
            return "<div id=\"" + id + "\" class=\"" + classe + "\" name=\"" + name + "\" style=\"" + style + "\"" +  tags + ">" + outerhtml + "</div>";
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
        public string span(string id=null, string classe=null, string name=null, string outerhtml = null, string style=null)
        {
            return "<span id=\"" + id + "\" class=\"" + classe + "\" name=\"" + name + "\" style=\"" + style + "\">" + outerhtml + "</span>";
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
            return "<h" + (int)headnumber + "  id=\"" + id + "\" class=\"" + classe + "\" name=\"" + name + "\" style=\"" + style + "\">"  + outerhtml + "</h" + (int)headnumber + ">";
        }
        /// <summary>
        /// Retorna um label
        /// </summary>
        /// <param name="labelfor">Atributo labelfor</param>
        /// <param name="classe">Atributo class</param>
        /// <param name="style">Styles</param>
        /// <param name="outerhtml">Conteudo</param>
        /// <returns></returns>
        public string label(string labelfor = null, string classe=null, string style = null, string outerhtml=null)
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
        public string input(type type = type.text, string id=null, string classe=null, string placeholder=null, bool required=false, bool disabled=false)
        {
            string requiredTag = null;
            if(required == true)
            {
                requiredTag = " required";
            }
            string disabledTag = null;
            if(disabled == true)
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
        public string button(string label, type type=type.submit, string id=null, string classe=null, string dataloading=null, bool disabled = false)
        {
            if(!string.IsNullOrEmpty(dataloading))
            {
                dataloading = " data-loading-text=\"" + dataloading + "\"";
            }

            classe += " btn";


            string disabledTag = null;
            if (disabled == true)
            {
                disabledTag = " disabled";
            }

            return "<button type=\"" + type.ToString() + "\" id=\"" +  id + "\" class=\"" + classe.Trim() + "\" " + dataloading  + disabledTag + ">" + label + "</button>";
        }
        /// <summary>
        /// Retorna um select
        /// </summary>
        /// <param name="options">Options do select</param>
        /// <param name="id">Atributo id</param>
        /// <param name="classe">Atributo class</param>
        /// <param name="disabled">Desabilitado</param>
        /// <returns></returns>
        public string select(string options, string id=null, string classe=null, bool disabled=false)
        {
            string disabledTag = null;
            if(disabled == true)
            {
                disabledTag = " disabled";
            }
            
            classe += " form-control";

            return "<select id=\"" + id + "\" class=\"" + classe.Trim() +"\"" + disabledTag + ">" + options + "</select>";
        }
        /// <summary>
        /// Retorna option do select
        /// </summary>
        /// <param name="value">Atributo value</param>
        /// <param name="label">Atributo label</param>
        /// <param name="tags">Tags</param>
        /// <returns></returns>
        public string option(string value, string label, string tags=null)
        {
            if(!string.IsNullOrEmpty(tags))
            {
                tags = " " + tags;
            }
            return "<option value=\"" + value + "\"" + tags + ">" + label + "</option>";
        }
        public string listgroup(string id=null, string classe=null, string outerhtml=null, string tags=null)
        {
            classe += " list-group";

            if(!string.IsNullOrEmpty(tags))
            {
                tags = " " + tags;
            }

            return "<ul id=\"" + id + "\" class=\"" + classe.Trim() + "\"" + tags + ">" + 
                    outerhtml +
                    "</ul>";
        }
        public string listgroupItem(string id=null, string classe=null, string tags=null, string outerhtml=null)
        {
            classe += " list-group-item";

            if(!string.IsNullOrEmpty(tags))
            {
                tags = " " + tags;
                tags = tags.Trim();
            }

            return "<li id=\"" + id + "\" class=\"" + classe.Trim() + "\"" + tags + ">" + outerhtml + "</li>";
        }
        public string accordeon(string id, string label, string classe=null, string outerhtml=null, bool aberto=true, bootstrapcolor? color=null)
        {
            string collapseIn = " in";
            if(aberto == false)
            {
                collapseIn = null;
            }

            string colorPanel = "default";
            if(color != null)
            {
                colorPanel = color.ToString();
            }

            return  "  <div class=\"panel-group\">                                                                                                          "+
                    "    <div class=\"panel panel-" + colorPanel + "\">                                                                                          "+
                    "      <div class=\"panel-heading\">                                                                                                    "+
                    "        <h4 class=\"panel-title\">                                                                                                     "+
                    "          <a data-toggle=\"collapse\" href=\"#collapse" + id +  "\" class=\"\" aria-expanded=\"true\">" + label + "</a>                "+
                    "      </div>                                                                                                                           "+
                    "        </h4>                                                                                                                          "+
                    "      <div id = \"collapse" + id + "\" class=\"panel-collapse collapse" + collapseIn + "\" aria-expanded=\"true\">                     "+
                    "        <ul class=\"list-group\" style=\"margin:0\">                                                                                   "+
                             outerhtml                                                                                                                       +
                    "        </ul>                                                                                                                          "+
                    "      </div>                                                                                                                           " +
                    "    </div>                                                                                                                             " +
                    "  </div>                                                                                                                               ";


            //#region panelHeading
            //string panelHeading = div(classe: "panel-heading",
            //                        id: "heading" + id,
            //                        tags: "role=\"tab\"",
            //                        outerhtml: head(
            //                                          headnumber: h.h4,
            //                                          classe: "panel-title",
            //                                          outerhtml: link(href: "collapse" + id,
            //                                                          tags: "role=\"button\" data-toggle=\"collapse\" data-parent=\"accordion" + id + "\" aria-expanded=\"true\" aria-controls=\"collapse" + id + "\"",
            //                                                          outerhtml: label)
            //                                        ));
            //#endregion



            //string panelCollapseBody = div(classe: "panel-body",
            //                              outerhtml: div(outerhtml:"22222222222") +
            //                                         div(outerhtml: "2222222") +
            //                                         div(outerhtml: "33333333")
                                          
                                          
            //                                         );



            //string panelCollapse = div(classe: "panel-collapse",
            //                          id: "collapse" + id,
            //                          tags: "role=\"tabpanel\" aria-labelledby=\"heading" + id + "\" style=\"height: auto;\"",
            //                          outerhtml: panelCollapseBody
            //                          );



            //string panel = div(classe: "panel panel-default",
            //                   outerhtml: panelHeading +
            //                              panelCollapse);

            //classe += " panel-group";

            //return div(id: "accordion" + id, 
            //           classe: classe.Trim(),
            //           tags: "role=\"tablist\" aria-multiselectable=\"true\"", 
            //           outerhtml: panel);
        }
        public string link(string id = null, string classe = null, string href=null, string tags = null, string outerhtml = null)
        {
            if(string.IsNullOrEmpty(href))
            {
                href = "#";
            }
            if(!string.IsNullOrEmpty(tags))
            {
                tags = " " + tags;
            }
            return "<a id=\"" + id + "\" href=\"" + href + "\" class=\"" + classe + "\"" + tags + ">" + outerhtml + "</a>";
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
        public string campoIntervalo(string id, decimal intervalMin, decimal intervalMax, decimal defaultValue=0, string unitName=null, string classe=null)
        {
            //definir min value //min=\"0\" 
            //definir max value
            //definir default value

            if (!string.IsNullOrEmpty(classe))
            {
                classe = " " + classe;
            }

            string intervalo = "<div class=\"input-group input-group-sm width180 pull-right" + classe + "\">                                                                                                  " +
                                 "    <span class=\"input-group-btn btn-minus\">                                                                                                                              " +
                                 "         <button class=\"btn btn-default\" type=\"button\">                                                                                                                 " +
                                 "             <i class=\"fa fa-minus\" aria-hidden=\"true\"></i>                                                                                                             " +
                                 "             </button></span><input type=\"text\" class=\"form-control text-center levelValue\">     " +
                                 "             <span class=\"input-group-btn btn-plus\"><button class=\"btn btn-default\" type=\"button\">                                                                    " +
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

            string calculado = "<div class=\"input-group input-group-sm width180 pull-right" + classe + "\">                                                                                                  " +
                                 "    <input type=\"text\" class=\"form-control text-center levelValue\">     " +
                                 "</div>                                                                                                                                                                      ";
            return calculado;
        }
        //public string level2(string id, string label, string classe = null, decimal defects = 0, int evaluate = 1, int sample = 1, bool reaudit = false, bool correctiveaction = false, bool phase = false,
        //                     string alertlevel1 = null, string alertlevel2 = null, string alertlevel3 = null, string AlertLevel = null, string ParFrequency_Id = null)

        public string level2(string id, string label, string classe=null, decimal defects = 0, int evaluate = 1, int sample = 1, bool reaudit = false, bool correctiveaction = false, bool phase = false)
        {
            return link(
                           id: id, 
                           classe: "level2" +  classe,
                           tags: "defects=\"" + defects + "\" evaluate=\"" + evaluate + "\" sample=\"" + sample + "\" ",
                           outerhtml: span(outerhtml: label, classe: "levelName")
                       );
        }
        public string user(int UserSGQ_Id, string UserSGQ_Name, string UserSGQ_Login, string UserSGQ_Pass, string Role, int ParCompany_Id, string ParCompany_Name)
        {
            return "<div class=\"user\" userid=\"" + UserSGQ_Id + "\" username=\"" + UserSGQ_Name + "\" userlogin=\"" + UserSGQ_Login.ToLower() + "\" userpass=\"" + UserSGQ_Pass + "\" userprofile=\"" + Role + "\" unidadeid=\"" + ParCompany_Id + "\" unidadename=\"" + ParCompany_Name + "\"></div>";
        }
        public string level1(SGQDBContext.ParLevel1 ParLevel1, string tipoTela, int totalAvaliado, decimal totalDefeitos, decimal alertNivel1, decimal alertNivel2, decimal alertaNivel3, int alertaAtual, int avaliacaoultimoalerta)
        {

            string tags = "parconsolidationtype_id=\"" + ParLevel1.ParConsolidationType_Id + "\" parfrequency_id=\"" + ParLevel1.ParFrequency_Id + "\" hasalert=\"" + ParLevel1.HasAlert.ToString().ToLower() + "\" isspecific=\"" + ParLevel1.IsSpecific.ToString().ToLower() + "\" totalavaliado=\"" + totalAvaliado + "\" totaldefeitos=\"" + totalDefeitos + "\" alertanivel1=\"" + alertNivel1 + "\" alertanivel2=\"" + alertNivel2 + "\" alertanivel3=\"" + alertaNivel3 + "\" alertaatual=\"" + alertaAtual + "\" avaliacaoultimoalerta=\"" + avaliacaoultimoalerta + "\"";

            string level01 = link(

                                id: ParLevel1.Id.ToString(),
                                classe: "level1 col-xs-7 " + tipoTela,
                                //Aqui vai as tags do level01
                                tags: tags,
                                outerhtml: ParLevel1.Name
                                );
            //Adiciona Div Lateral
            level01 += div(
                            //aqui vai os botoes
                            outerhtml: null,
                            classe: "userInfo col-xs-5");
            return level01;
        }
    }
}