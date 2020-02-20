function openMenu() {
	GetAppParametrization123();
}

function GetAppParametrization123() {

	openMensagem('Por favor, aguarde até que seja feito o download das parametrizações', 'blue', 'white');

	$.ajax({
		data: JSON.stringify({
			ParCompany_Id: currentParCompany_Id
			, AppDate: currentCollectDate
		}),
		type: 'POST',
		url: urlPreffix + '/api/AppColeta/GetAppParametrization123',
		contentType: "application/json",
		success: function (data) {
			data.listaParFrequency = listaParFrequency;
			_writeFile("appParametrization.txt", JSON.stringify(data), function () {
				parametrization = data;
				listarParCluster();
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