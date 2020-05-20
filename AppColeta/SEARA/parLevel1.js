function listarParLevel1(isVoltar) {

	currentParLevel1_Id = null;
	currentParLevel2_Id = null;
	objCabecalhoLevel1 = {};

	var listaParLevel1 = RetornarParLevel1(currentParCluster_Id);

	var htmlLista = "";

	var department = {};

	$(listaParLevel1).each(function (i, o) {
		htmlLista += '<button type="button" class="list-group-item col-xs-12" ' +
			'" data-par-level1-id="' + o.Id + '">' + o.Name +
			'<span class="badge">></span>' +
			'</button>';
	});

	var voltar = "";

	voltar = '<a onclick="validaRota(listarParCluster,null);" class="btn btn-warning">Voltar</a>';

	html = getHeader() +
		'<div class="container-fluid">                                           ' +
		'	<div class="">                                              ' +
		'		<div class="col-xs-12">                                    ' +
		'                                                                  ' +
		'			<div class="panel panel-primary">                      ' +
        '			  <div class="panel-heading">                          ' +
        '			    <div class="row">                          ' +
        '			      <div class="col-xs-9">                         ' +
        '				    <h3 class="panel-title">' + voltar + ' Selecione o Indicador</h3>' +
        '                 </div >                                          ' +
        '                 <div class="col-xs-3">                           ' +
        '                     <div class="pull-right">                     ' +
        '                        <label style="padding-right:5px">Buscar</label> ' +
        '                        <input id="buscarIndicador" type="text" />' +
        '                     </div>                                       ' +
        '                 </div>                                           ' +
        '               </div>                                             ' +
		'			  </div>                                               ' +
		'			  <div class="panel-body">                             ' +
		'				<div class="list-group" id="listaIndicadores">                           ' +
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

$('body').off('keyup', "#buscarIndicador").on('keyup', "#buscarIndicador", function () {

    var listaParLevel1 = RetornarParLevel1(currentParCluster_Id);
    var listaIndicadores = "";

    $(listaParLevel1).each(function (i, o) {
        var mostrarIndicador = o.Name.toLowerCase().includes($("#buscarIndicador").val().toLowerCase());
        if (mostrarIndicador) {
            listaIndicadores += '<button type="button" class="list-group-item col-xs-12" ' +
                '" data-par-level1-id="' + o.Id + '">' + o.Name +
                '<span class="badge">></span>' +
                '</button>';
        }
    });
    if (listaIndicadores == "") {
        listaIndicadores = "<span class='list-group-item col-xs-12 text-center'>Nenhum resultado encontrado com o termo digitado.</span>";
    }
    $("#listaIndicadores").html(listaIndicadores);
});

$('body').off('click', '[data-par-level1-id]').on('click', '[data-par-level1-id]', function (e) {

    currentParLevel1_Id = parseInt($(this).attr('data-par-level1-id'));

	level1BusinessRoute(currentParLevel1_Id);

});

function RetornarParLevel1(parCluster_Id){
	var listaParLevel1XCluster = $.grep(parametrization.listaParLevel1XCluster, function (item) {
        return item.ParCluster_Id == parCluster_Id;
	}); 
	
	return $.grep(parametrization.listaParLevel1, function (parLevel1) {
		var exists = $.grep(listaParLevel1XCluster, function (item) {
			return item.ParLevel1_Id == parLevel1.Id;
		});
        return exists.length > 0;
    });
}