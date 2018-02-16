using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Mvc.Html;

namespace SgqSystem.Helpers
{


    public static class Table
    {
        public enum PosicaoLabel
        {
            top,
            left
        }
        //
        public static MvcHtmlString td(this HtmlHelper html, string idLink, string textoLink = "Voltar")
        {
            string strLink = string.Format("<a id=\"{0}\" href=\"javascript:history.go(-1);\">{1}</a>", idLink, textoLink);
            return new MvcHtmlString(strLink);
        }


        public static MvcHtmlString GerarColuna(MvcHtmlString input,
                                                MvcHtmlString label,
                                                PosicaoLabel posicaoLabel, 
                                                MvcHtmlString error = null, 
                                                string info = null,
                                                int colspan = 0,
                                                string classTd = null,
                                                string style = null)
        {
            String tr = "";
            String tagInfo = "";
            String tagError = "";

            if (info != null)
            {
                tagInfo = " <i class='fa fa-info-circle fa-lg text-primary popovers' data-container='body' data-trigger='hover' data-placement='bottom' data-content='" + info + "' aria-hidden='true'></i>";
            }

            if (error != null)
            {
                tagError = error.ToString();
            }

            if (posicaoLabel == PosicaoLabel.top)
            {
                tr = "<td class='td-erp "+ classTd +"' colspan='" + colspan + "' style='" + style + "'>" +
                        label.ToString() + " " +
                        tagInfo +
                        "<div>"+
                        input.ToString() +
                        "</div>"+
                        tagError +
                     "</td>";
            }
            else if(posicaoLabel == PosicaoLabel.left)
            {
                tr =
                    "<td class='td-erp text-left "+ classTd + "' style='" + style + "'>" +
                        label.ToString() +" "+
                        tagInfo +
                    "</td>" +
                    "<td class='td-erp "+ classTd + "' style='" + style + "'>" +
                        input.ToString() +
                        tagError+
                    "</td>";
            }
            return new MvcHtmlString(tr);
        }

        public static MvcHtmlString GerarColunaLabel(MvcHtmlString label, string info = null)
        {
            String tr = "";
            String tagInfo = "";

            if (info != null)
            {
                tagInfo = " <i class='fa fa-info-circle fa-lg text-primary popovers' data-container='body' data-trigger='hover' data-placement='bottom' data-content='" + info + "' aria-hidden='true'></i>";
            }

            tr = "<td class='td-erp'>" +
                        label.ToString() + " " +
                        tagInfo +
                     "</td>";

            return new MvcHtmlString(tr);
        }

        public static MvcHtmlString GerarColunaButton(MvcHtmlString input,
                                                MvcHtmlString label,
                                                PosicaoLabel posicaoLabel,
                                                MvcHtmlString error = null,
                                                string info = null,
                                                String button = null,
                                                string classTd = null,
                                                string style = null)
        {
            String tr = "";
            String tagInfo = "";
            String tagError = "";

            if (info != null)
            {
                tagInfo = " <i class='fa fa-info-circle fa-lg text-primary popovers' data-container='body' data-trigger='hover' data-placement='bottom' data-content='" + info + "' aria-hidden='true'></i>";
            }

            if (error != null)
            {
                tagError = error.ToString();
            }

            if (posicaoLabel == PosicaoLabel.top)
            {
                tr = "<td class='td-erp "+ classTd + "' style='"+ style + "'>" +
                        label.ToString() + " " +
                        tagInfo +
                        "<div class=\"input-group\">" +
                        input.ToString() +
                        "<span class=\"input-group-btn\">"+
                        button +
                        "</span>"+
                        "</div>" +
                        tagError +
                     "</td>";
            }
            else if (posicaoLabel == PosicaoLabel.left)
            {
                tr =
                    "<td class='td-erp text-left  " + classTd + "' style='" + style + "'>" +
                        label.ToString() + " " +
                        tagInfo +
                    "</td>" +
                    "<td class='td-erp  " + classTd + "' style='" + style + "'>" +
                        input.ToString() +
                        tagError +
                    "</td>";
            }
            return new MvcHtmlString(tr);
        }

        public static MvcHtmlString GerarColunaLabelTitle(MvcHtmlString label, string info = null)
        {
            String tr = "";
            String tagInfo = "";

            if (info != null)
            {
                tagInfo = " <i class='fa fa-info-circle fa-lg text-primary popovers' data-container='body' data-trigger='hover' data-placement='bottom' data-content='" + info + "' aria-hidden='true'></i>";
            }

            tr = "<td class='td-erp'><b>" +
                        label.ToString() + "</b> " +
                        tagInfo +
                     "</td>";

            return new MvcHtmlString(tr);
        }

        public static MvcHtmlString GerarColunaCheckbox(
                                            MvcHtmlString checkbox, 
                                            MvcHtmlString label,  
                                            PosicaoLabel posicaoLabel, 
                                            MvcHtmlString error = null, 
                                            string info = null,
                                            String button = null)
        {
            String tr = "";
            String tagInfo = "";
            String tagError = "";

            if (info != null)
            {
                tagInfo = " <i class='fa fa-info-circle fa-lg text-primary popovers' data-container='body' data-trigger='hover' data-placement='bottom' data-content='" + info + "' aria-hidden='true'></i>";
            }

            if (error != null)
            {
                tagError = error.ToString();
            }

            if (posicaoLabel == PosicaoLabel.top)
            {
                tr = "<td class='td-erp'>" +
                        label.ToString() + 
                        tagInfo + " <br>" +
                        checkbox.ToString() + button +
                     "</td>";
            }
            else if (posicaoLabel == PosicaoLabel.left)
            {
                tr =
                    "<td class='td-erp text-left'>" +
                        label.ToString() + 
                        tagInfo + 
                    "</td>" +
                    "<td class='td-erp'>" +
                        checkbox.ToString() + button +
                    "</td>";
            }
            return new MvcHtmlString(tr);
        }

        public static MvcHtmlString GerarColunaRadioButton(MvcHtmlString label, MvcHtmlString radio1, MvcHtmlString radio2, String radio1Label, String radio2Label, PosicaoLabel posicaoLabel, MvcHtmlString error = null, string info = null)
        {
            String tr = "";
            String tagInfo = "";
            String tagError = "";

            if (info != null)
            {
                tagInfo = " <i class='fa fa-info-circle fa-lg text-primary popovers' data-container='body' data-trigger='hover' data-placement='bottom' data-content='" + info + "' aria-hidden='true'></i>";
            }

            if (error != null)
            {
                tagError = error.ToString();
            }

            if (posicaoLabel == PosicaoLabel.top)
            {
                tr = "<div class='icheck-list'><td class='td-erp'>" +
                        label.ToString() + " " +
                        tagInfo +
                        "</td><td>" +
                        "<div class='radio-erp'><label>" +
                        radio1.ToString() +
                        radio1Label +
                        "</label></div>"+
                        "<div class='radio-erp'><label>" +
                        radio2.ToString() +
                        radio2Label +
                        "</label></div>" +
                     "</td></div>";
            }
            else if (posicaoLabel == PosicaoLabel.left)
            {
                tr =
                    "<div class='icheck-list'><td class='td-erp'>" +
                        label.ToString() + " " +
                        tagInfo + "<br>" +
                        "</td><td>" +
                        radio1.ToString() + radio1Label.ToString() +
                        radio2.ToString() + radio2Label.ToString() + 
                        "</td>"+
                    "</div>";
            }
            return new MvcHtmlString(tr);
        }



        /// <summary>
        /// Exemplo de método para criar componente criado pelo usuário
        /// Como usa no cshtml
        /// @Html.RazorFor(m => m.paramsDto.parLevel1Dto.Id, new { @title = "Customer name", @class = "form-control input-erp" })
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString RazorFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {

            ////Como usa no cshtml
            //@Html.RazorFor(model => model.Name, new { @title = "Customer name", @class = "form-control" })

            var valueGetter = expression.Compile();
            //var value = valueGetter(helper.ViewData.Model);

            var tagTr = new TagBuilder("div");
            var tagLabel = new TagBuilder("label");
            var tagInput = helper.EditorFor(expression).ToHtmlString();

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            Object value = metadata.Model;
            String name = metadata.PropertyName;

            //tagInput.MergeAttribute("id", name);
            //tagInput.MergeAttribute("name", name);

            //tagInput.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            tagLabel.SetInnerText(helper.DisplayNameFor(expression).ToHtmlString());

            tagTr.InnerHtml = tagLabel.ToString() + tagInput.ToString();









            //newTag.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            ////Se vier algum valor no Model, ele ja seta automaticamente
            //if (value != null)
            //{
            //    span.SetInnerText(value.ToString());
            //}

            return new MvcHtmlString(System.Web.HttpUtility.HtmlDecode(tagTr.ToString()));
            //return MvcHtmlString.Create(tagTr.ToString());
        }


    }
}