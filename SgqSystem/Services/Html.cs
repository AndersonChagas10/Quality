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

            return "<button type=\"" + type.ToString() + "\" class=\"" + classe.Trim() + "\" " + dataloading  + disabledTag + ">" + label + "</button>";
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
        public string listgroup(string id=null, string classe=null, string outerhtml=null)
        {
            classe += " list-group";

            return "<ul id=\"" + id + "\" class=\"" + classe.Trim() + "\">" + 
                    outerhtml +
                    "</ul>";
        }
        public string listgroupItem(string id=null, string classe=null, string outerhtml=null)
        {
            classe += " list-group-item";
            return "<li id=\"" + id + "\" class=\"" + classe.Trim() + "\">" + outerhtml + "</li>";
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
    }

}