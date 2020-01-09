function listarParLevel1(isVoltar) {

	currentParLevel1_Id = null;
	currentParLevel2_Id = null;

	var listaParLevel1 = parametrization.listaParLevel1;

	var htmlLista = "";

	var department = {};

	$(listaParLevel1).each(function (i, o) {
		htmlLista += '<button type="button" class="list-group-item col-xs-12" ' +
			'" data-par-level1-id="' + o.Id + '">' + o.Name +
			'<span class="badge">></span>' +
			'</button>';
	});

	var voltar = "";

	voltar = '<a onclick="validaRota(openMenu,null);" class="btn btn-warning">Voltar</a>';

	html = getHeader() +
		'<div class="container-fluid">                                           ' +
		'	<div class="">                                              ' +
		'		<div class="col-xs-12">                                    ' +
		'                                                                  ' +
		'			<div class="panel panel-primary">                      ' +
		'			  <div class="panel-heading">                          ' +
		'				<h3 class="panel-title">' + voltar + ' Selecione o centro de custo desejado</h3>            ' +
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

    setBreadcrumbs();

    if ($(".list-group button").length == 1 && (isVoltar == false || isVoltar == undefined)) {
        $("[data-par-department-id]").trigger('click');
    }
}

$('body').off('click', '[data-par-level1-id]').on('click', '[data-par-level1-id]', function (e) {

    currentParLevel1_Id = parseInt($(this).attr('data-par-level1-id'));

	level1BusinessRoute(currentParLevel1_Id);

});