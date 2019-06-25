function openMenu() {

	cleanGlobalVarParFrequency();

	var html = '';

	var htmlButtonColetarDesabilitado = "";
	if(!(currentPlanejamento && currentPlanejamento.length > 0)){
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
		'<button class="btn btn-lg btn-success btn-block" onclick="clickColetar()" '+htmlButtonColetarDesabilitado+'>Coletar</button>' +
		'		</div>   ' +
		'		<div class="col-xs-2">                        ' +
		'		</div>                                         ' +
		'	</div>                                             ' +
		'</div>';

	$('div#app').html(html);

}

function clickPlanejar(){
	if(currentParFrequency_Id > 0){
		openPlanejamentoColeta();
	}else{
		openParFrequency();
	}
}

function clickColetar(){
	listarParDepartment(0);
}
