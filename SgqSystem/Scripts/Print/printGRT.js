var printGRT = { //Print Of Hell

    //Precisa de JQuery 
    //Inserir .avoid-break-page em elementos que não podem ser quebrados no momento da impressão.
    //Inserir <div style="clear:both"> no final de div com elementos flutuantes (Cabeçalho e Rodapé)

    /*
		<script>	
			jsPOH.Inicializar(
				{
					headerHtml : document.getElementById('contentHeader').innerHTML,
					bodyHtml : document.getElementById('contentBody').innerHTML,
					footerHtml : document.getElementById('contentFooter').innerHTML,
					numberMarginRight : 35, 
					numberMarginBottom : 10, 
					renderTableMargin : 35,
					pageMarginHeader : "10 0 0 0",
					pageMarginFooter : "0 0 10 0"
				}  
			);
			
			$('#print').on("click",function(){
				jsPOH.Imprimir();
			});
		</script>
	*/
    sumPage: 0,
    renderDivName: "divRenderPrint",
    renderTableName: "renderPrint",
    headerName: "renderPrintHeader",
    bodyName: "renderPrintBody",
    footerName: "renderPrintFooter",
    numberMarginRight: 35,
    numberMarginBottom: 5,
    renderTableMargin: 35,
    pageMarginHeader: "20 0 0 0",
    pageMarginFooter: "0 0 20 0",
    pageMarginBody: "0 0 0 0",

    Style: function () {
        this.style =
		"<style type='text/css'>                                                   " +
		"	body {                                                                 " +
		"		counter-reset: section;                                            " +
		"		name-reset: section;                                               " +
		"	}                                                                      " +
		"	@page {                                                                " +
		"		margin:0;                                                          " +
		"		size: A4;                                                          " +
		"	}                                                                      " +
		"	                                                                       " +
		"	#" + this.footerName + ":last-child {                                      " +
		"		page-break-after: auto;                                            " +
		"	}                                                                      " +
		"		                                                                   " +
		"	#" + this.renderTableName + "{margin:0px;padding:0px " + this.renderTableMargin + "px;}" +
		"	#" + this.renderTableName + " *{                                           " +
		"		-webkit-print-color-adjust: exact;                                 " +
		"	}                                                                      " +
		"	                                                                       " +
		"	#" + this.headerName + ", #" + this.footerName + "{                            " +
		"		display:block; z-index:99999;                                      " +
		"	}                                                                      " +
		"	#" + this.headerName + "{                                                  " +
		"		padding:" + this.pageMarginHeader + ";                                       " +
		"	}                                                                      " +
		"	#" + this.footerName + "{                                                  " +
		"		padding:" + this.pageMarginFooter + "; text-align:left;                                       " +
		"	}                                                                      " +
		"	                                                                       " +
        "	#" + this.bodyName + "{display:block;height:auto;margin:0px;padding:" + this.pageMarginBody + "}  " +
		"	                                                                       " +
		"	#" + this.bodyName + ", #" + this.headerName + ", #" + this.footerName + "{        " +
		//"		padding-left: 40px;                                                "+
		//"		padding-right:40px;                                               "+
		"	}                                                                      " +
		"	                                                                       " +
		"	@media print                                                           " +
		"	{                                                                      " +
		"		body, html{ margin:0; padding:0}                                   " +
		"		#" + this.renderTableName + " .avoid-break-page{ page-break-inside:avoid; }" +
		"		tr    { page-break-inside:avoid; page-break-after:auto }           " +
		"		td    { page-break-inside:avoid; page-break-after:auto }           " +
		"		tr.break, td.break    { page-break-inside:auto }           " +
		"		thead { display:table-header-group;}                               " +
		"		tfoot { display:table-footer-group; }                              " +
		"		#" + this.bodyName + " > div {page-break-inside:avoid;}                " +
		"		dpiDiv { display:none;} @page thead:first-of-type {background:red;}" +
		"       span#number {                                                      " +
		"       	position: absolute;                                            " +
		"       	page-break-before: always;                                     " +
		"       	page-break-after: always;                                      " +
		"       	right: " + this.numberMarginRight + "px; font-weight:bold;         " +
		"       }                                                                  " +
		"       span#number.first {                                                      " +
		"       	bottom: " + this.numberMarginBottom + "px;                         " +
		"       }                                                                  " +
		"       span#number:not(.first) {                                                      " +
		"       	margin-bottom: " + this.numberMarginBottom + "px;                         " +
		"       }                                                                  " +
		"       span#number:before {                                               " +
		"       	position: relative;                                            " +
		"       	counter-increment: section;                                    " +
		"       	content: counter(section) '/' attr(data-content);              " +
		"       	font-size:15px;                                                " +
		"       	bottom: 0px;                         " +
		"       }                                                                  " +
		"	}                                                                      " +
		"</style>                                                                  ";
    },

    HtmlDivDPI: function () {
        return "<div id='dpiDiv' style='height: 1in; left: -100%; position: absolute; top: -100%; width: 1in;'></div>";
    },

    HtmlRender: function () {
        this.Style();
        this.htmlRender =
			"<div id='" + this.renderDivName + "'>                  " +
			this.style +
			"<table id='" + this.renderTableName + "'>              " +
			"	<thead>                                         " +
			"		<tr>                                        " +
			"			<th>                                    " +
			"				<div id='" + this.headerName + "'>      " +
			"				</div>                              " +
			"			</th>                                   " +
			"		</tr>                                       " +
			"	</thead>                                        " +
			"	<tbody>                                         " +
			"		<tr>                                        " +
			"			<td>                                    " +
			"				<div id='" + this.bodyName + "'>        " +
			"				</div>                              " +
			"			</td>                                   " +
			"		</tr>                                       " +
			"	</tbody>                                        " +
			"	<tfoot>                                         " +
			"		<tr>                                        " +
			"			<th>                                    " +
			"				<div id='" + this.footerName + "'>        " +
			"				</div>                              " +
			"			</th>                                   " +
			"		</tr>                                       " +
			"	</tfoot>                                        " +
			"</table>                                           " +

			"<div>                                                  " +
			"<span id='number' class='first' data-content=''></span>" +
			"</div>                                                 " +
			"<div class='insert'></div>                             " +

			"</div>                                            ";
    },

    CalculaDPI: function () {
        var htmlDivDPI = this.HtmlDivDPI();
        var divDPI = createElement(htmlDivDPI);
        document.body.appendChild(divDPI);

        //Ajusta o tamanho da A4 considerando DPI
        var dpi = document.getElementById('dpiDiv').offsetWidth;
        this.height = 29.7 / 2.54 * dpi;
        this.width = (21 / 2.54 * dpi) - 20;
    },

    RemoveExistente: function () {
        var element = document.getElementById("dpiDiv");
        if (element != null)
            document.body.removeChild(element);
        element = document.getElementById("divRenderPrint");
        if (element != null)
            document.body.removeChild(element);
    },

    Inicializar: function (config) {

        if (config.sumPage != undefined)
            this.sumPage = config.sumPage;

        if (config.numberMarginRight != undefined)
            this.numberMarginRight = config.numberMarginRight;
        if (config.numberMarginBottom != undefined)
            this.numberMarginBottom = config.numberMarginBottom;
        if (config.renderTableMargin != undefined)
            this.renderTableMargin = config.renderTableMargin;
        if (config.pageMarginHeader != undefined)
            this.pageMarginHeader = config.pageMarginHeader;
        if (config.pageMarginBody != undefined)
            this.pageMarginBody = config.pageMarginBody;
        if (config.pageMarginFooter != undefined)
            this.pageMarginFooter = config.pageMarginFooter;

        //use: <div style="clear:both"></div> to fix float usage
        this.RemoveExistente();
        this.HtmlRender();
        var htmlTableRenderPrint = this.htmlRender;

        this.CalculaDPI();

        var tableRenderPrint = createElement(htmlTableRenderPrint);
        document.body.appendChild(tableRenderPrint);
        if (config.headerHtml != undefined)
            if (config.headerHtml != undefined)
                document.getElementById(this.headerName).innerHTML = config.headerHtml;
        if (config.bodyHtml != undefined)
            document.getElementById(this.bodyName).innerHTML = config.bodyHtml;
        if (config.footerHtml != undefined)
            document.getElementById(this.footerName).innerHTML = config.footerHtml;

        document.getElementById(this.renderTableName).style.width = this.width + "px";
        var header = document.getElementById(this.headerName).offsetHeight;
        var footer = document.getElementById(this.footerName).offsetHeight;
        var freeHeight = (this.height - header - footer);
        this.FreeHeight = freeHeight;

        $.each($('.height-fix'), function (i, o) {
            $(this).css('min-height', freeHeight - 10 + 'px');
        });

        var body = document.getElementById(this.bodyName).offsetHeight;
        var spacer = freeHeight - (body % freeHeight);
        var numeroPaginas = Math.floor(body / freeHeight) + 1 + this.sumPage;
        document.getElementById(this.bodyName).style.height = (body + spacer + ((numeroPaginas - 1) * (header + footer)) - 16) + "px";

        //ADICIONA PAGINAÇÃO
        $('span#number').attr('data-content', numeroPaginas);

        var bottom = 0;
        for (var i = 0; i < (numeroPaginas - 1) ; i++) {
            bottom -= 100;
            botString = bottom.toString();
            var $counter = $('body span.first').clone().removeClass('first');
            $counter.css("bottom", botString + "vh");
            ($counter).insertBefore('.insert');
        }
    },

    Imprimir: function () {
        var printContents = document.getElementById(this.renderDivName).innerHTML;
        var originalContents = document.body.innerHTML;
        document.body.innerHTML = printContents;
        window.print();
        document.body.innerHTML = originalContents;
    }
};

function createElement(str) {
    var frag = document.createDocumentFragment();

    var elem = document.createElement('div');
    elem.innerHTML = str;

    while (elem.childNodes[0]) {
        frag.appendChild(elem.childNodes[0]);
    }
    return frag;
}