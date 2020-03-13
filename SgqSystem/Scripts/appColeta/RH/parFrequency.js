function openParFrequency() {

	var html = '';

	_readFile("parFrequency.txt", function (data) {
		if (globalLoginOnline) {

			openMensagem('Carregando lista de frequencia', 'blue', 'white');

			$.ajax({
                data: JSON.stringify({
                    ParCompany_Id: currentParCompany_Id
                    , ParCluster_Id: currentParCluster_Id
                    , AppDate: currentCollectDate
                }),
                contentType: "application/json",
                type: 'POST',
				url: urlPreffix + '/api/parFrequency',
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

        if (listaParFrequency.length == 1) {
            getAppParametrization(listaParFrequency[0].Id);
            return;
        }

		$(data).each(function (i, o) {

			htmlParFrequency += '<button type="button" class="list-group-item col-xs-12" data-par-frequency-id="' + o.Id + '" ' +
				((currentParFrequency_Id == o.Id || !(currentParFrequency_Id > 0)) ? '' : 'style="background-color:#eee;cursor:not-allowed"')
				+ '>' + o.Name +
				'</button>';
		});

        var voltar = '<a onclick="validaRota(openParCluster,null);" class="btn btn-warning">Voltar</a>';

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

	getAppParametrization(frequencyId);

});

function getAppParametrization(frequencyId) {

	if(!frequencyId){
		return;
	}

    if (frequencyId != currentParFrequency_Id || parametrization.currentParCluster_Id != currentParCluster_Id) {

		currentParFrequency_Id = frequencyId;
		chamaGetAppParametrization();

	} else {

		openMensagem('Carregando parametrização', 'blue', 'white');

		_readFile("appParametrization.txt", function (data) {

			if (data){
				parametrization = JSON.parse(data);
                atualizarVariaveisCurrent(parametrization);
			}

			openPlanejamentoColeta();
			closeMensagem();
		});
	}
}

function chamaGetAppParametrization(){
	openMensagem('Por favor, aguarde até que seja feito o download do planejamento', 'blue', 'white');

	$.ajax({
		data: JSON.stringify({
			ParCompany_Id: currentParCompany_Id
			, ParFrequency_Id: currentParFrequency_Id
			, ParCluster_Id: currentParCluster_Id
			, AppDate: currentCollectDate
			, ParClusterGroup_Id: currentParClusterGroup_Id
		}),
		type: 'POST',
		url: urlPreffix + '/api/AppColeta/GetAppParametrization',
		contentType: "application/json",
		success: function (data) {
			data.currentParFrequency_Id = currentParFrequency_Id;
			data.listaParFrequency = listaParFrequency;
			data.currentParCluster_Id = currentParCluster_Id;
			data.currentParClusterGroup_Id = currentParClusterGroup_Id;
			data.currentParCompany_Id = currentParCompany_Id;
			_writeFile("appParametrization.txt", JSON.stringify(data), function () {
				parametrization = data;
				openPlanejamentoColeta();
				closeMensagem();
			});
			sincronizarResultado();
		},
		timeout: 600000,
		error: function () {
			$(this).html($(this).attr('data-initial-text'));
			closeMensagem();
		}
		
	});
}