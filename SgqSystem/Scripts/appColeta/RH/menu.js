function openMenu() {

	preencheCurrentPPlanejamento(showMenu);

}

function showMenu() {
	cleanGlobalVarParFrequency();

	var html = '';

	var htmlButtonColetarDesabilitado = "";
	if (!(currentPlanejamento && currentPlanejamento.length > 0)) {
		htmlButtonColetarDesabilitado = ' disabled="true"';
	}

	html = getHeader() +
		'<div class="container-fluid">                               ' +
		'	<div class="">                                  ' +
		'		<div class="col-xs-2">                        ' +
		'		</div>                                         ' +
		'		<div class="col-xs-4">   ' +
		'			<button class="btn btn-lg btn-info btn-block" onclick="clickPlanejar()">Planejar</button>' +
		'		</div>   ' +
		'		<div class="col-xs-4">   ' +
		'<button class="btn btn-lg btn-success btn-block" onclick="clickColetar()" ' + htmlButtonColetarDesabilitado + '>Coletar</button>' +
		'		</div>   ' +
		'		<div class="col-xs-2">                        ' +
		'		</div>                                         ' +
		'	</div>                                             ' +
		'</div>';

	$('div#app').html(html);

	changeStateButtonColetar();
}

function clickPlanejar() {
	if (currentParFrequency_Id > 0) {
		openPlanejamentoColeta();
    } else {
		openParCompany();
        //openParClusterGroup();
        //openParCluster(); //antigo
	}
}

function clickColetar() {
	listarParDepartment(0);
}

function changeStateButtonColetar() {

	if (currentPlanejamento.length > 0) {

		$(".btncoletar").removeClass("disabled");

	} else {

		$(".btncoletar").addClass("disabled");

	}

	if (currentParFrequency_Id > 0) {

		$(".btnGetParams").removeClass("disabled");

	} else {

		$(".btnGetParams").addClass("disabled");

	}
}