function listarParDepartment(parDepartmentId) {

	var listaDepartamentos = retornaDepartamentos(parDepartmentId, true);
	
	var htmlParDepartment = "";
	
	var department = {};

	$(listaDepartamentos).each(function (i, o) {

		if (parseInt(parDepartmentId) > 0 && parDepartmentId == o.Id) {
			department = o;
		}else
		if ((parDepartmentId > 0 && parDepartmentId == o.Parent_Id) || ((parDepartmentId == 0 || parDepartmentId == null) && (o.Parent_Id == 0 || o.Parent_Id == null))) {
			htmlParDepartment += '<button type="button" class="list-group-item col-xs-12" ' +
				'data-par-department-id="' + o.Id + '" data-par-department-parend-id="' + o.Parent_Id + '">' + o.Name +
				'<span class="badge">></span>' +
				'</button>';
		}

	});

	//caso for "" quer dizer que não tem mais filhos, então abre o próximo	
	if (htmlParDepartment == "") {
		currentParDepartmentParent_Id = department.Parent_Id;
		listarParCargo();
		return;
	}

	var voltar = !!department.Id ? '<a onclick="listarParDepartment(' + department.Parent_Id + ');" class="btn btn-warning">Voltar</a>' : '<a onclick="listarParFrequency();" class="btn btn-warning">Voltar</a>';

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
		htmlParDepartment +
		'				</div>                                             ' +
		'			  </div>                                               ' +
		'			</div>                                                 ' +
		'                                                                  ' +
		'		</div>                                                     ' +
		'	</div>                                                         ' +
		'</div>';

	$('div#app').html(html);

	setBreadcrumbs();
}

function retornaDepartamentos(parDepartmentId, retornaDepartamentoAtual){

	currentParDepartmentId = parDepartmentId;

	currentParCargo_Id = null;

	if (currentsParDepartments_Ids.indexOf(parDepartmentId) >= 0)
		currentsParDepartments_Ids = currentsParDepartments_Ids.slice(0, currentsParDepartments_Ids.indexOf(parDepartmentId));

	if (parDepartmentId)
		currentsParDepartments_Ids.push(parseInt(parDepartmentId));
	else
		currentsParDepartments_Ids = [];

	var data = parametrization.listaParDepartment;

	var listaDepartamentos = [];

	$(data).each(function (i, o) {
		if ((retornaDepartamentoAtual && parseInt(parDepartmentId) > 0 && parDepartmentId == o.Id)
		|| (parDepartmentId > 0 && parDepartmentId == o.Parent_Id) 
		|| ((parDepartmentId == 0 || parDepartmentId == null) && (o.Parent_Id == 0 || o.Parent_Id == null))) {
			listaDepartamentos.push(o);
		}
	});

	return listaDepartamentos;
}

$('body').on('click', '[data-par-department-id]', function (e) {

	var parDepartmentId = $(this).attr('data-par-department-id');

	listarParDepartment(parDepartmentId);

});