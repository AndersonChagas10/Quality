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

		ParDepartmentsFilter.push($.grep(todosParDepartments, function (o) {

			return o.Id == id;

		})[0]);
	});

	var todosDepartmentsPais = [];

	$.each(ParDepartmentsFilter, function (i, o) {

		var novosDepartments = [o];
		var todosNovos = [];

		do {

			novosDepartments = getDepartmentPai(novosDepartments);

			todosNovos = todosNovos.concat(novosDepartments);

			var novosComPai = $.grep(novosDepartments, function (o) {
				return o.Hash != null;
			});

			novosDepartments = novosComPai;

		} while (novosComPai.length > 0);

		todosDepartmentsPais = todosDepartmentsPais.concat(todosNovos);

	});

	var todosDepartmentsFilhos = [];

	$.each(ParDepartmentsFilter, function (i, o) {

		var novosDepartments = [o];
		var todosNovos = [];

		do {

			novosDepartments = getDepartmentFilho(novosDepartments);

			todosNovos = todosNovos.concat(novosDepartments);

			if (todosNovos.length > 0)
				novosDepartments = todosNovos;

		} while (todosNovos > 0);

		todosDepartmentsFilhos = todosDepartmentsFilhos.concat(todosNovos);

	});

	ParDepartmentsFilter = ParDepartmentsFilter.concat(todosDepartmentsFilhos);
	ParDepartmentsFilter = ParDepartmentsFilter.concat(todosDepartmentsPais);

	return ParDepartmentsFilter;

}

function getDepartmentPai(departmentsParaBuscar) {

	var newDepartments = [];

	$.each(departmentsParaBuscar, function (i, o) {

		var departments = $.grep(parametrization.listaParDepartment, function (oo) {

			return o.Parent_Id == oo.Id

		});

		newDepartments = newDepartments.concat(departments);
	});

	return newDepartments;
}

function getDepartmentFilho(departmentsParaBuscar) {

	debugger
	var newDepartments = [];

	$.each(departmentsParaBuscar, function (i, o) {

		var departments = $.grep(parametrization.listaParDepartment, function (oo) {

			return o.Id == oo.Hash

		});

		newDepartments = newDepartments.concat(departments);
	});

	return newDepartments;

}
