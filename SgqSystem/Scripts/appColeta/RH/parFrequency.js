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

		var frequency = {};

		var htmlParFrequency = "";

		$(data).each(function (i, o) {

			htmlParFrequency += '<button type="button" class="list-group-item col-xs-12" data-par-frequency-id="' + o.Id + '" ' +
				((currentParFrequency_Id == o.Id || !(currentParFrequency_Id > 0)) ? '' : 'style="background-color:#eee;cursor:not-allowed"')
				+ '>' + o.Name +
				'</button>';
		});

		var voltar = "";

		html = getHeader() +
			'<div class="container-fluid">                               ' +
			'	<div class="">                                  ' +
			'		<div class="col-xs-12">                        ' +
			'			<div class="panel panel-warning">          ' +
			'			  <div class="panel-heading">              ' +
			'				<h3 class="panel-title">Qual frequencia deseja realizar coleta?</h3>      ' +
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
	getAppParametrization(frequencyId);
});

