var reprocessoLists = {};

function getListsReprocesso(){
    $.get(urlPreffix+"/api/Reprocesso/Get/"+$('.App').attr('unidadeid'), function(result){
        console.log(result);
        if(result){
            reprocessoLists = result;
            saveListsReprocessos();
        }
    }).fail(function() {
        loadListsReprocessos();
    });
}

function saveListsReprocessos(){
    _writeFile('listsReprocesso.json', JSON.stringify(reprocessoLists));
}

function loadListsReprocessos(){
    _readFile('listsReprocesso.json', function(result){
        if(result){
            reprocessoLists = JSON.parse(result);
        }        
    });
}

function setupRetrocesso(){

    $('.font-small').css('height','22px');

    if(reprocessoLists.headerFieldsEntrada){
        reprocessoLists.headerFieldsEntrada.forEach(function(element) {
            var select = $('select:visible[id='+element.Id+']');
            if(select.length > 0){
                $('.level1.selected').addClass('reprocesso');
                select.parent().addClass('reprocesso');
		select.parent().addClass('reprocessoEntrada');
                select.empty();
                $.grep(reprocessoLists.parReprocessoHeaderOPs, function(o, i){
                    select.append($("<option value='"+o.nCdOrdemProducao+"'>"+o.nCdOrdemProducao+"</option>"));
                });
                select.change();
            }
        });
    }

    if(reprocessoLists.headerFieldsSaida){
        reprocessoLists.headerFieldsSaida.forEach(function(element) {
            var select = $('select:visible[id='+element.Id+']');
            if(select.length > 0){
                $('.level1.selected').addClass('reprocesso');
                select.parent().addClass('reprocesso');
		select.parent().addClass('reprocessoSaida');
                select.empty();
                $.grep(reprocessoLists.parReprocessoHeaderOPs, function(o, i){
                    select.append($("<option value='"+o.nCdOrdemProducao+"'>"+o.nCdOrdemProducao+"</option>"));
                });
                select.change();
            }
        });
    }

}

var minhaOP = 0;
var tipoReprocesso = '';

$(document).on('change', '.header.reprocesso select', function(){
    
    var listProdutosEntrada = []; 
    var listProdutosSaida = []; 
    
    

    $('.header.reprocesso:visible select').each(function(elem){
        var value = parseInt($(this).val());

        minhaOP = $.grep(reprocessoLists.parReprocessoHeaderOPs, function(a,b){
     return a.nCdOrdemProducao == value 
})

	tipoReprocesso = $(this).parent().hasClass('reprocessoEntrada') == true ? 'reprocessoEntrada' : 'reprocessoSaida';
	
        if($(this).parent().hasClass('reprocessoEntrada')){
            listProdutosEntrada.push.apply(listProdutosEntrada, $.grep(reprocessoLists.parReprocessoEntradaOPs, function(o, i){
                return o.nCdOrdemProducao == value;
            }));
            listProdutosSaida.push.apply(listProdutosSaida, $.grep(reprocessoLists.parReprocessoSaidaOPs, function(o, i){
                return o.nCdOrdemProducao == value;
            }));
        } else {
            listProdutosEntrada.push.apply(listProdutosEntrada, $.grep(reprocessoLists.parReprocessoEntradaOPs, function(o, i){
                return o.nCdOrdemProducao == value && o.dProducao != "0001-01-01T00:00:00";
            }));
            listProdutosSaida.push.apply(listProdutosSaida, $.grep(reprocessoLists.parReprocessoSaidaOPs, function(o, i){
                return o.nCdOrdemProducao == value && o.dProducao != "0001-01-01T00:00:00";
            }));
        }
    });

    var collapse = "<li class='painel row list-group-item collapseReprocesso'>";

    if(listProdutosEntrada.length > 0)
    {
        collapse += montarPainel(montarTabelaEntrada(listProdutosEntrada), "Produtos de Entrada", "Entrada");
    }

    collapse += "<br />";
    
    if(listProdutosSaida.length > 0)
    {
        collapse += montarPainel(montarTabelaSaida(listProdutosSaida), "Produtos de Saida", "Saida");
    }

    collapse += "</li>";

    

    $('.collapseReprocesso').remove();
    $('.level3:visible:first').before($(collapse));

    $('.tblReprocesso').css('font-size','xx-small');
    
});

function montarTabelaEntrada(listProdutos) {
    var table = "<table class='table table-sm tblReprocesso'>";

    if(tipoReprocesso == 'reprocessoEntrada'){
        table += "<thead>" +
        "<tr>" +
        "<th scope='col'>Produto</th>" +
        "<th scope='col'>Local do estoque</th>" +
        "</tr>" +
        "</thead>";
    }else{
	

        table += "<thead>" +
        "<tr>" +
        "<th scope='col'>Produto</th>" +
        //"<th scope='col'>Local do Estoque</th>" +
        "<th scope='col'>Data produção</th>" +
        "<th scope='col'>Data embalagem</th>" +
        "<th scope='col'>Data validade</th>" +
        "<th scope='col'>SIF</th>" +
        "<th scope='col'>Cod. Rastreabilidade</th>" +
        "<th scope='col'>Volume</th>" +
        "<th scope='col'>Peso</th>" +
        "</tr>" +
        "</thead>";
    }
    table += "<tbody>";
    listProdutos.forEach(function (element) {


    if(tipoReprocesso == 'reprocessoEntrada'){
        table +=
            "<tr>" +
            "<th scope='row'>(" + element.nCdProduto + ") " + (element.produto ? element.produto.cNmProduto : "") + "</th>" +
            "<td>" + element.cNmLocalEstoque + "</td>" 
            "</tr>";
    }else{

	    var dProducao = '';
        if(element.dProducao == "0001-01-01T00:00:00"){
		dProducao = '';
        }else{
                dProducao = dateInternacionalFormat(element.dProducao);
        }

	    var dEmbalagem= '';
        if(element.dEmbalagem == "0001-01-01T00:00:00"){
		dEmbalagem = '';
        }else{
                dEmbalagem = dateInternacionalFormat(element.dEmbalagem);
        }

	    var dValidade = '';
        if(element.dValidade == "0001-01-01T00:00:00"){
		    dValidade = '';
        }else{
            dValidade = dateInternacionalFormat(element.dValidade);
        }

        table +=
            "<tr>" +
            "<th scope='row'>(" + element.nCdProduto + ") " + (element.produto ? element.produto.cNmProduto : "") + "</th>" +
            "<td>" + dProducao + "</td>" +
            "<td>" + dEmbalagem + "</td>" +
            "<td>" + dValidade + "</td>" +
            "<td>" + element.cCdOrgaoRegulador + "</td>" +
            "<td>" + element.cCdRastreabilidade + "</td>" +
            "<td>" + element.iVolume + "</td>" +
            "<td>" + element.nPesoLiquido + "</td>" +
            "</tr>";
    }
        
    });
    table += "</tbody>";
    table += "</table>";
    return table;
}

function montarTabelaSaida(listProdutos) {
    var table = "<table class='table table-sm tblReprocesso'>";

    if(tipoReprocesso == 'reprocessoEntrada'){
        table += "<thead>" +
        "<tr>" +
        "<th scope='col'>Produto</th>" +
        "<th scope='col'>Data de Produção</th>" +
        "<th scope='col'>Data de Validade</th>" +
        "<th scope='col'>Rastreabilidade</th>" +
        "<th scope='col'>Habilitação</th>" +
        "<th scope='col'>Quantidade Prevista</th>" +

        "</tr>" +
        "</thead>";
    }else{
	table += "<thead>" +
        "<tr>" +
        "<th scope='col' colspan='1'></th>" +
        "<th scope='col' colspan='8' style='background-color: #2F4F4F; color: white; margin: 4px;'>Realizado</th>" +
        "<th scope='col' colspan='1' style='background-color: #483D8B; color: white; margin: 4px;'>Previsto</th>" +

        "</tr>" +
        "</thead>";

        table += "<thead>" +
        "<tr>" +
        "<th scope='col'>Produto</th>" +
        
        "<th scope='col'>Data de Produção</th>" +
        "<th scope='col'>Data de Validade</th>" +
        "<th scope='col'>Rastreabilidade</th>" +
        "<th scope='col'>Habilitacao</th>" +
        "<th scope='col'>Local do Estoque</th>" +
        "<th scope='col'>Total Peça</th>" +
        "<th scope='col'>Total Volume</th>" +
        "<th scope='col'>Total Peso</th>" +
	"<th scope='col'>Quantidade Prevista</th>" +
        "</tr>" +
        "</thead>";
    }

        
    table += "<tbody>";
    listProdutos.forEach(function (element) {

	var tipo = "";

	if(element.cQtdeTipo == 1){
             tipo = "(Volume)";
        }else if(element.cQtdeTipo == 2){
             tipo = "(Peso)";
        }else if(element.cQtdeTipo == 3){
             tipo = "(Peça)";
        }else if(element.cQtdeTipo == 4){
             tipo = "(Área)";
        }

    if(tipoReprocesso == 'reprocessoEntrada'){

	var dProducao = '';
        if(element.dProducao == "0001-01-01T00:00:00"){
		dProducao = '';
        }else{
                dProducao = dateInternacionalFormat(element.dProducao);
        }

	var dValidade = '';
        if(element.dValidade == "0001-01-01T00:00:00"){
		dValidade = '';
        }else{
                dValidade = dateInternacionalFormat(element.dValidade);
        }

        table +=
            "<tr>" +
            "<th scope='row'>(" + element.nCdProduto + ") " + (element.produto ? element.produto.cNmProduto : "") + "</th>" +
            "<td>" + dProducao + "</td>" + 
            "<td>" + dValidade + "</td>" + 
            "<td>" + minhaOP[0].cCdRastreabilidade + "</td>" +
            "<td>" + minhaOP[0].cSgHabilitacao + "</td>" +
            "<td>" + element.iQtdePrevista + " " + tipo + "</td>" +

            "</tr>";
    }else{

	var dProducao = '';
        if(element.dProducao == "0001-01-01T00:00:00"){
		dProducao = '';
        }else{
                dProducao = dateInternacionalFormat(element.dProducao);
        }

	var dValidade = '';
        if(element.dValidade == "0001-01-01T00:00:00"){
		dValidade = '';
        }else{
                dValidade = dateInternacionalFormat(element.dValidade);
        }

        var dEmbalagem= '';
        if(element.dEmbalagem == "0001-01-01T00:00:00"){
		dEmbalagem = '';
        }else{
                dEmbalagem = dateInternacionalFormat(element.dEmbalagem);
        }

        table +=
            "<tr>" +
            "<th scope='row'>(" + element.nCdProduto + ") " + (element.produto ? element.produto.cNmProduto : "") + "</th>" +
            
            "<td>" + dProducao + "</td>" + 
            "<td>" + dValidade + "</td>" +           
            "<td>" + minhaOP[0].cCdRastreabilidade + "</td>" +
            "<td>" + minhaOP[0].cSgHabilitacao + "</td>" +
            "<td>" + element.cNmLocalEstoque + "</td>" +
            "<td>" + element.iTotalPeca + "</td>" +
            "<td>" + element.iTotalVolume + "</td>" +
            "<td>" + element.nTotalPeso + "</td>" +
	    "<td>" + element.iQtdePrevista + " " + tipo + "</td>" +
            "</tr>";

    }


            
    });
    table += "</tbody>";
    table += "</table>";
    return table;
}
 function montarPainel(table, title, id){
var p = "<div class='panel-group'>"+
            "<div class='panel panel-primary'>"+
                "<div class='panel-heading' role='tab' id='"+id+"'>"+
                    "<h4 class='panel-title'>"+
                        "<a role='button' data-toggle='collapse' href='#collapse"+id+"' aria-expanded='true' aria-controls='collapse"+id+"'>"+
                            title+
                        "</a>"+
                    "</h4>"+
                "</div>"+
                "<div id='collapse"+id+"' class='panel-collapse collapse in' role='tabpanel' aria-labelledby='"+id+"'>"+
                    "<ul class='list-group' style='margin:0'>"+
                        "<li class='row list-group-item' style='overflow-x: auto; margin-right: 0px'>"+
                            table+
                        "</li>"+
                    "</ul>"+
                "</div>"+
            "</div>"+
        "</div>";
    return p;
        
 }