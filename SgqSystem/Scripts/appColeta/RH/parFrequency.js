function openParFrequency() {

	var html = '';

	_readFile("parFrequency.txt", function (data) {
		if (globalLoginOnline) {

			openMensagem('Carregando lista de frequencia', 'blue', 'white');

			$.ajax({
				data: {},
				url: urlPreffix + '/api/parFrequency',
				type: 'GET',
				success: function (data) {

					_writeFile("parFrequency.txt", JSON.stringify(data), function () {
						listaParFrequency = data;
						listarParFrequency();
					});

					closeMensagem();
				},
				timeout: 600000,
				error: function () {
					$(this).html($(this).attr('data-initial-text'));
					closeMensagem();
				}
			});

		} else {
			listarParFrequency();
		}

	});
}

function listarParFrequency() {

	cleanGlobalVarParFrequency();

	_readFile("parFrequency.txt", function (data) {

		data = JSON.parse(data);

		listaParFrequency = data;

		var frequency = {};

		var htmlParFrequency = "";

		$(data).each(function (i, o) {

			htmlParFrequency += '<button type="button" class="list-group-item col-xs-12" data-par-frequency-id="' + o.Id + '" ' +
				((currentParFrequency_Id == o.Id || !(currentParFrequency_Id > 0)) ? '' : 'style="background-color:#eee;cursor:not-allowed"')
				+ '>' + o.Name +
				'</button>';
		});

		var voltar = '<a onclick="openMenu();" class="btn btn-warning">Voltar</a>';

		html = getHeader() +
			'<div class="container-fluid">                               ' +
			'	<div class="">                                  ' +
			'		<div class="col-xs-12">                        ' +
			'			<div class="panel panel-primary">          ' +
			'			  <div class="panel-heading">              ' +
			'				<h3 class="panel-title">' + voltar + ' Qual frequencia deseja realizar coleta?</h3>      ' +
			'			  </div>                                   ' +
			'			  <div class="panel-body">                 ' +
			'				<div class="list-group">               ' +
			htmlParFrequency +
			'				</div>                                 ' +
			'			  </div>                                   ' +
			'			</div>                                     ' +
			'		</div>                                         ' +
			'	</div>                                             ' +
			'</div>';

		$('div#app').html(html);
	});
}

function cleanGlobalVarParFrequency() {
	currentParDepartment_Id = null;
	currentParCargo_Id = null;
	//currentParFrequency_Id = null;
	currentsParDepartments_Ids = [];
}

$('body').off('click', '[data-par-frequency-id]').on('click', '[data-par-frequency-id]', function (e) {

	var frequencyId = parseInt($(this).attr('data-par-frequency-id'));

	getPlanejamentoPorFrequencia(frequencyId);

});

function getPlanejamentoPorFrequencia(frequencyId) {

	if (frequencyId != currentParFrequency_Id) {

		currentParFrequency_Id = frequencyId;
		openMensagem('Por favor, aguarde até que seja feito o download do planejamento selecionado', 'blue', 'white');

		$.ajax({
			data: JSON.stringify({
				ParCompany_Id: currentParCompany_Id
				, ParFrequency_Id: currentParFrequency_Id
				, AppDate: currentCollectDate
			}),
			type: 'POST',
			url: urlPreffix + '/api/AppColeta/GetAppParametrization',
			contentType: "application/json",
			success: function (data) {
				data.currentParFrequency_Id = currentParFrequency_Id;
				data.listaParFrequency = listaParFrequency;
				_writeFile("appParametrization.txt", JSON.stringify(data), function () {
					parametrization = data;
					openPlanejamentoColeta();
					closeMensagem();
				});
			},
			timeout: 600000,
			error: function () {
				$(this).html($(this).attr('data-initial-text'));
				closeMensagem();
			}
			
		});

	} else {

		openMensagem('Carregando parametrização', 'blue', 'white');

		_readFile("appParametrization.txt", function (data) {

			if (data)
				parametrization = JSON.parse(data);

			openPlanejamentoColeta();
			closeMensagem();
		});
	}
}

