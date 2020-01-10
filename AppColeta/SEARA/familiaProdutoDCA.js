var currentFamiliaProdutoDCA_Id;
var currentProdutoDCA_Id;

function listarFamiliaProdutoDCA(isVoltar) {

    currentParLevel2_Id = null;
	currentFamiliaProdutoDCA_Id = null;

	var htmlLista = "";

	$(parametrization.listaSearaFamiliaProduto).each(function (i, o) {
		htmlLista += '<button type="button" class="list-group-item col-xs-12" ' +
			'" data-familia-produto-id="' + o.Id + '">' + o.Name +
			'<span class="badge">></span>' +
			'</button>';
	});

	var voltar = "";

	voltar = '<a onclick="validaRota(listarParLevel1,null);" class="btn btn-warning">Voltar</a>';

	html = getHeader() +
		'<div class="container-fluid">                                           ' +
		'	<div class="">                                              ' +
		'		<div class="col-xs-12">                                    ' +
		'                                                                  ' +
		'			<div class="panel panel-primary">                      ' +
		'			  <div class="panel-heading">                          ' +
		'				<h3 class="panel-title">' + voltar + ' Selecione a Familia de Produto</h3>            ' +
		'			  </div>                                               ' +
		'			  <div class="panel-body">                             ' +
		'				<div class="list-group">                           ' +
		htmlLista +
		'				</div>                                             ' +
		'			  </div>                                               ' +
		'			</div>                                                 ' +
		'                                                                  ' +
		'		</div>                                                     ' +
		'	</div>                                                         ' +
		'</div>';

    $('div#app').html(html);

    setBreadcrumbsDCA();

    if ($(".list-group button").length == 1 && (isVoltar == false || isVoltar == undefined)) {
        $("[data-par-department-id]").trigger('click');
    }
}

$('body').off('click', '[data-familia-produto-id]').on('click', '[data-familia-produto-id]', function (e) {

    currentFamiliaProdutoDCA_Id = parseInt($(this).attr('data-familia-produto-id'));

	listarParLevel2DCA();

});

function getSearaFamiliaProduto(){
	return $.grep(parametrization.listaSearaFamiliaProduto, function (obj) {
        return obj.Id == currentFamiliaProdutoDCA_Id;
    })[0];
}

function getSearaProdutoPorFamiliaDeProduto(){
	var produtosVinculados = $.grep(parametrization.listaSearaFamiliaProdutoXProduto, function (obj) {
        return obj.SearaFamiliaProduto_Id == currentFamiliaProdutoDCA_Id;
	});
	
	var produtos = $.grep(parametrization.listaSearaProduto, function (produto) {
		var produto_Id = produto.Id;
		
        return $.grep(produtosVinculados, function (vinculo) {
			return vinculo.SearaProduto_Id == produto_Id;
		});
	});

	return produtos;
}

function getSelectProdutosDCA() {

	var htmlLista = '<select name="produtoDCA" size="5" class="form-control" style="height:100px;">'

	$(getSearaProdutoPorFamiliaDeProduto()).each(function (i, o) {
		var selected = "";
		if(currentProdutoDCA_Id == o.Id){
			selected = " selected";
		}
		htmlLista += '<option value="'+o.Id+'" '+selected+'>' + o.Name +'</option>';
	});

	htmlLista += '</select>';

	return htmlLista;
}

$('body').off('change', 'select[name="produtoDCA"]').on('change', 'select[name="produtoDCA"]', function (e) {

    currentProdutoDCA_Id = parseInt($(this).val());

});