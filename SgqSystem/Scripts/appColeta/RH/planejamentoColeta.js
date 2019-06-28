function openPlanejamentoColeta() {

	_readFile("planejamento.txt", function (data) {
		if (data && data.length > 1)
			currentPlanejamento = JSON.parse(data);

		getFrequenciaSelecionada();
	});

}

function getFrequenciaSelecionada() {

	var frequenciaSelecionada = {};

	_readFile("parFrequency.txt", function (data) {

		data = JSON.parse(data);

		$(data).each(function (i, o) {
			if (currentParFrequency_Id == o.Id) {
				frequenciaSelecionada = o;
				renderPlanejamentoColeta(frequenciaSelecionada);
				return;
			}
		});

	});
}

function renderPlanejamentoColeta(frequencia) {

	var html = '';

	var voltar = '<a onclick="voltarPlanejamentoColeta();" class="btn btn-warning">Voltar</a>';

	html = getHeader() +
		'<div class="container-fluid">                               ' +
		'	<div class="">                                  ' +
		'		<div class="col-xs-12">                        ' +
		'			<div class="panel panel-primary">          ' +
		'			  <div class="panel-heading">              ' +
		'				<h3 class="panel-title">' + voltar + ' Qual frequencia deseja realizar coleta?' +
		'<button class="btn btn-success pull-right" onclick="downloadPlanejamento()">Baixar Planejamento</button> </h3>      ' +
		'			  </div>                                   ' +
		'			  <div class="panel-body" style="padding-top: 10px !important">                 ' +
		'				<div class="list-group">               ' +
		'					<div class="col-sm-6">               ' +
		'<div class="form-group">' +
		'	<label>Frequencia:</label>' +
		'	<input type="hidden"value="' + frequencia.Id + '">' +
		'	<input type="text" class="form-control" value="' + frequencia.Name + '" readonly>' +
		'</div>' +
		'<div data-selects-cc>' +
		criaHtmlSelect('Centro de Custo:', retornaOptionsPeloArray(retornaDepartamentos(0, undefined, parametrization.listaParDepartment), 'Id', 'Name', 'Selecione')) +
		'</div>' +
		'<div data-selects-cargo>' +
		'</div>' +
		'<div data-selects-indicador>' +
		'</div>' +
		'<div class="form-group">' +
		'	<button type="button" class="btn btn-primary" onClick="savePlanejar()">Planejar</button>' +
		'</div>' +
		'					</div>                                 ' +
		'					<div class="col-sm-6">               ' +
		'			<div class="panel panel-warning">          ' +
		'			  <div class="panel-heading">              ' +
		'				<h3 class="panel-title">Planejamentos salvos</h3>      ' +
		'			  </div>                                   ' +
		'			  <div class="panel-body" data-save-planned>                 ' +
		renderPlanejamentos() +
		'			  </div>                                   ' +
		'			</div>                                     ' +
		'					</div>                                 ' +
		'				</div>                                 ' +
		'			  </div>                                   ' +
		'			</div>                                     ' +
		'		</div>                                         ' +
		'	</div>                                             ' +
		'</div>';

	$('div#app').html(html);

}

function renderPlanejamentos() {
	var html = '<table class="table table-hover"><thead>' +
		'<tr><th>Departamento</th><th>Cargo</th><th>Indicador</th><th></th></tr></thead><tbody>';

	$(currentPlanejamento).each(function (i, o) {
		html += '<tr data-table-planejado="' + i + '"><td>' + o.parDepartment_Name + '</td><td>' +
			(typeof (o.parCargo_Name) == 'undefined' ? '-' : o.parCargo_Name) + '</td><td>' +
			(typeof (o.indicador_Name) == 'undefined' ? '-' : o.indicador_Name) + '</td><td>' +
			'<button class="btn btn-danger" onclick="removePlanejamento(' + i + ')">X</button></td></tr>';
	});

	html += "</tbody>";
	return html;
}

function voltarPlanejamentoColeta() {
	if (currentPlanejamento.length > 0) {
		openMenu();
	} else {
		openParFrequency();
	}
}

var planejamento = {};
function savePlanejar() {
	if (planejamento.parDepartment_Id > 0) {
		currentPlanejamento.push($.extend({}, planejamento));
		$('[data-save-planned]').html(renderPlanejamentos());
		saveInFilePlanejamento();
	}
}

function removePlanejamento(index) {
	currentPlanejamento.splice(index, 1);
	$('[data-save-planned]').html(renderPlanejamentos());
	saveInFilePlanejamento();
}

function saveInFilePlanejamento() {
	_writeFile("planejamento.txt", JSON.stringify(currentPlanejamento), function () {
	});
}

function downloadPlanejamento() {
	console.log(JSON.stringify({
		ParCompany_Id: curretParCompany_Id
		, ParFrequency_Id: currentParFrequency_Id
		, AppDate: currentCollectDate
		, Planejamento: currentPlanejamento.map(function (obj) {
			return {
				ParDepartment_Id: obj.parDepartment_Id,
				ParCargo_Id: obj.parCargo_Id,
				Indicador_Id: obj.indicador_Id
			}
		})
	}));
}

$('body').off('change', '[data-selects-cc] select').on('change', '[data-selects-cc] select', function (e) {
	var parDepartment_Id = $(this).val();

	$(this).parent().nextAll().remove();
	$('[data-selects-cargo]').html('');
	$('[data-selects-indicador]').html('');
	planejamento = {};

	if (parDepartment_Id > 0) {
		var options = retornaDepartamentos(parDepartment_Id, undefined, parametrization.listaParDepartment);

		currentParDepartment_Id = $(this).val();
		planejamento.parDepartment_Id = currentParDepartment_Id;
		planejamento.parDepartment_Name = $(this).find(':selected').text();

		if (options.length > 0) {
			var departamentos = retornaOptionsPeloArray(options, 'Id', 'Name', 'Selecione');

			$('[data-selects-cc]').append(criaHtmlSelect('', departamentos));
		} else {
			$('[data-selects-cargo]').html(criaHtmlSelect('Cargo:', retornaOptionsPeloArray(retornaCargos($(this).val()), 'Id', 'Name', 'Selecione')));
		}
	}
});

$('body').off('change', '[data-selects-cargo] select').on('change', '[data-selects-cargo] select', function (e) {
	var cargo_Id = $(this).val();

	$('[data-selects-indicador]').html('');
	planejamento.indicador_Id = undefined;
	planejamento.indicador_Name = undefined;

	if (cargo_Id > 0) {
		currentParCargo_Id = cargo_Id;
		planejamento.parCargo_Id = currentParCargo_Id;
		planejamento.parCargo_Name = $(this).find(':selected').text();

		var level1List = [];
		montarLevel1(level1List);

		$('[data-selects-indicador]').html(criaHtmlSelect('Indicador:', retornaOptionsPeloArray(level1List, 'Id', 'Name', 'Selecione')));
	} else {
		planejamento.parCargo_Id = undefined;
		planejamento.parCargo_Name = undefined;
	}

});

$('body').off('change', '[data-selects-indicador] select').on('change', '[data-selects-indicador] select', function (e) {
	var indicador_Id = $(this).val();

	if (indicador_Id > 0) {
		planejamento.indicador_Id = indicador_Id;
		planejamento.indicador_Name = $(this).find(':selected').text();
	} else {
		planejamento.indicador_Id = undefined;
		planejamento.indicador_Name = undefined;
	}
});

//Filtros de planejamento de coleta
function getParDepartmentPlanejado() {

	var todosParDepartments = parametrization.listaParDepartment;

	var ParDepartments_Ids = $.unique($.map(currentPlanejamento, function (o) {

		return o["parDepartment_Id"];

	}));

	var ParDepartmentsFilter = [];

	$.each(ParDepartments_Ids, function (i, id) {

		var teste = $.grep(todosParDepartments, function (o) {

			return o.Id == id;

		})[0];

		if (teste)
			ParDepartmentsFilter.push(teste);
	});

	if (ParDepartmentsFilter.length == 0) {
		return [];
	}

	var todosDepartmentsPais = [];

	$.each(ParDepartmentsFilter, function (i, o) {

		var novosDepartments = [o];

		todosDepartmentsPais = todosDepartmentsPais.concat(getDepartmentPai(novosDepartments));

	});

	var todosDepartmentsFilhos = [];

	$.each(ParDepartmentsFilter, function (i, o) {

		var novosDepartments = [o];
		var todosNovos = [];

		do {

			var novos = getDepartmentFilho(novosDepartments);

			if (novos.length == 0)
				break;

			novosDepartments = novos;

			todosNovos = todosNovos.concat(novos);

		} while (novos.length > 0);

		todosDepartmentsFilhos = todosDepartmentsFilhos.concat(todosNovos);

	});

	ParDepartmentsFilter = ParDepartmentsFilter.concat(todosDepartmentsFilhos);
	ParDepartmentsFilter = ParDepartmentsFilter.concat(todosDepartmentsPais);
	ParDepartmentsFilter = removeDuplicateId(ParDepartmentsFilter)

	allParDepartments = ParDepartmentsFilter;

	return ParDepartmentsFilter;

}

function removeDuplicateId(myArray) {

	var newArray = [];

	$.each(myArray, function (key, value) {

		var exists = false;

		$.each(newArray, function (k, val2) {

			if (value.Id == val2.Id) {
				exists = true
			};
		});

		if (exists == false && value.Id != "") {
			newArray.push(value);
		}

	});

	return newArray;
}

var allParDepartments = [];

function getDepartmentPai(departmentsParaBuscar) {

	var newDepartments = [];

	$.each(departmentsParaBuscar, function (i, o) {

		if (o.Hash != null) {

			var idsPais = o.Hash.split("|");

			$.each(idsPais, function (i, id) {
				var departments = $.grep(parametrization.listaParDepartment, function (oo) {

					return id == oo.Id

				})[0];

				if (departments)
					newDepartments = newDepartments.concat(departments);
			});
		}

	});

	return newDepartments;
}

function getDepartmentFilho(departmentsParaBuscar) {

	var newDepartments = [];

	$.each(departmentsParaBuscar, function (i, o) {

		var departments = $.grep(parametrization.listaParDepartment, function (oo) {

			return o.Id == oo.Parent_Id

		});

		newDepartments = newDepartments.concat(departments);
	});

	return newDepartments;

}

function retornaCargosPlanejados(listaParCargo) {

	var planejamentos = $.grep(currentPlanejamento, function (o) {
		if (o.parCargo_Id)
			return o.ParDepartment_Id = currentParDepartment_Id;
	});

	if (planejamentos.length == 0)
		return listaParCargo;

	listaCargoFiltrada = $.map(planejamentos, function (o) {
		return o.parCargo_Id;
	});

	var newListaParCargo = [];

	$.each(listaCargoFiltrada, function (i, o) {

		var listaDeCargoPlanejado = $.grep(listaParCargo, function (oo) {
			return oo.Id == o;
		});

		newListaParCargo = newListaParCargo.concat(listaDeCargoPlanejado);
	});

	return newListaParCargo;

}

function retornaLevels1Planejados(listaParLevel1) {

	var planejamentos = $.grep(currentPlanejamento, function (o) {
		if (o.parCargo_Id && o.indicador_Id)
			return o.ParDepartment_Id == currentParDepartment_Id && o.parCargo_Id == currentParCargo_Id;
	});

	if (planejamentos.length == 0)
		return listaParLevel1;

	var listaIndicadorFiltrado = $.map(planejamentos, function (o) {
		return o.indicador_Id;
	});

	var newListaIndicador = [];

	$.each(listaIndicadorFiltrado, function (i, o) {

		var listaDeIndicadorPlanejado = $.grep(listaParLevel1, function (oo) {
			return oo.Id == o;
		});

		newListaIndicador = newListaIndicador.concat(listaDeIndicadorPlanejado);
	});

	return newListaIndicador;
}