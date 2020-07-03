var currentFamiliaProdutoDCA_Id;
var currentProdutoDCA_Id;

function listarFamiliaProdutoDCA(isVoltar) {

    currentParLevel2_Id = null;
	currentFamiliaProdutoDCA_Id = null;

	var htmlLista = "";

	$(parametrization.listaParFamiliaProduto).each(function (i, o) {
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

function getParFamiliaProduto(){
	return $.grep(parametrization.listaParFamiliaProduto, function (obj) {
        return obj.Id == currentFamiliaProdutoDCA_Id;
    })[0];
}

function getParProdutoPorFamiliaDeProduto(){
	var produtosVinculados = $.grep(parametrization.listaParFamiliaProdutoXParProduto, function (obj) {
        return obj.ParFamiliaProduto_Id == currentFamiliaProdutoDCA_Id;
	});
	
	var produtos = $.grep(parametrization.listaParProduto, function (produto) {
		var produto_Id = produto.Id;
		
        return $.grep(produtosVinculados, function (vinculo) {
			return vinculo.ParProduto_Id == produto_Id;
		}).length > 0;
	});

	return produtos;
}

function getSelectProdutosDCA() {
    var htmlLista = '<input type="text" id="buscaProdutoDCA" class="form-control" style="height:50px;" placeholder="Buscar Produto/ SKU">';
    htmlLista += '<select id="selectProdutoDCA" name="produtoDCA" class="form-control" style="height:50px;">';

    var listaParProdutoPorFamiliaDeProduto = getParProdutoPorFamiliaDeProduto();
    listaParProdutoPorFamiliaDeProduto.sort((a, b) => a.Name.localeCompare(b.Name));
    $(listaParProdutoPorFamiliaDeProduto).each(function (i, o) {
		var selected = "";
		if(currentProdutoDCA_Id == o.Id){
			selected = " selected";
		}
		htmlLista += '<option value="'+o.Id+'" '+selected+'>' + o.Name +'</option>';
	});

	htmlLista += '</select>';

	return htmlLista;
}

$('body').off('keyup', '#buscaProdutoDCA').on('keyup', '#buscaProdutoDCA', function (e) {
    var listaProdutosDCA = "";
    $(getParProdutoPorFamiliaDeProduto()).each(function (i, o) {
        
        if (o.Name.toLowerCase().includes($('#buscaProdutoDCA').val().toLowerCase())) {
            listaProdutosDCA += '<option value="' + o.Id + '">' + o.Name + '</option>';
        }
    });
    if (listaProdutosDCA == "") {
        listaProdutosDCA = "<option value='' disabled selected>Nenhum resultado encontrado.</option>"
    }
    $('#selectProdutoDCA').html(listaProdutosDCA);

    currentProdutoDCA_Id = parseInt($('select[name="produtoDCA"]').val());
});

$('body').off('change', 'select[name="produtoDCA"]').on('change', 'select[name="produtoDCA"]', function (e) {

    currentProdutoDCA_Id = parseInt($(this).val());

});