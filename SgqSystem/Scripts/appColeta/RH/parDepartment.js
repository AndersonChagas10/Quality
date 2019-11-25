function listarParDepartment(parDepartmentId, isVoltar) {

	//listaDepartamentos = getParDepartmentPlanejado(); parametrization.listaParDepartment


    var listaDepartamentos = retornaDepartamentos(parDepartmentId, true, getParDepartmentPlanejado());

	var htmlParDepartment = "";

	var department = {};

	$(listaDepartamentos).each(function (i, o) {

		if (parseInt(parDepartmentId) > 0 && parDepartmentId == o.Id) {
			department = o;
		} else
			if ((parDepartmentId > 0 && parDepartmentId == o.Parent_Id) || ((parDepartmentId == 0 || parDepartmentId == null) && (o.Parent_Id == 0 || o.Parent_Id == null))) {
				htmlParDepartment += '<button type="button" class="list-group-item col-xs-12" ' +
					'data-par-department-id="' + o.Id + '" data-par-department-parend-id="' + o.Parent_Id + '">' + o.Name +
					'<span class="badge">></span>' +
					'</button>';
			}

	});

	currentParDepartment_Id = department.Id;

	getCurrentPlanejamentoObj();

    //caso for "" quer dizer que não tem mais filhos, então abre o próximo	
    if (htmlParDepartment == "") {
        currentParDepartmentParent_Id = department.Parent_Id;
        listarParCargo(isVoltar);
		return;
	}

	var voltar = "";

	if (parDepartmentId == 0 || parDepartmentId == undefined || parDepartmentId == null) 

        voltar = '<a onclick="validaRota(openMenu,null);" class="btn btn-warning">Voltar</a>';

	else {

		voltar = '<a onclick="voltarDepartment(' + department.Parent_Id + ');" class="btn btn-warning">Voltar</a>';
	}

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

    if ($(".list-group button").length == 1 && (isVoltar == false || isVoltar == undefined)) {
        $("[data-par-department-id]").trigger('click');
    }

	
}

function retornaDepartamentosPorCluster(parClusterId, retornaDepartamentoAtual, listaParEvaluationXDepartmentXCargo) {
    currentParCluster_Id = parClusterId;

    currentParDepartment_Id = null;

    var listaDepartamentos = [];
    var listaParDepartment_Ids = [];
    var listaSecao = [];

    $(listaParEvaluationXDepartmentXCargo).each(function (i, o) {
        if (o.ParCluster_Id == parClusterId) {
            listaParDepartment_Ids.push(o.ParDepartment_Id);
        }
    });

    var parDepartmentList = $.map(listaParDepartment_Ids, function (a) {
        if (a == null) {
            return parametrization.listaParDepartment;
        }
    });

    if (parDepartmentList != null && parDepartmentList.length > 0) {
         listaDepartamentos = parDepartmentList;
    }
    else {
        for (var i = 0; i < parametrization.listaParDepartment.length; i++) {
            for (var j = 0; j < listaParDepartment_Ids.length; j++) {
                if (parametrization.listaParDepartment[i].Id == listaParDepartment_Ids[j]) {
                    listaSecao.push(parametrization.listaParDepartment[i]);
                }
            }
        }
        //pegar o id do pai pelo hash para caso o select se tornar multiplo a manutenção seja mais fácil 
        //hash.slice(0,hash.indexOf("|"))
        var hash;
        for (var i = 0; i < parametrization.listaParDepartment.length; i++) {
            for (var j = 0; j < listaSecao.length; j++) {
                hash = listaSecao[j].Hash;
                if (hash.indexOf("|") != -1) {
                    hash = hash.slice(0, hash.indexOf("|"));
                } else {
                    hash = listaSecao[j].Parent_Id;
                }
                if (parametrization.listaParDepartment[i].Id == hash) {
                    listaDepartamentos.push(parametrization.listaParDepartment[i]);
                }
            }
        }
    }

    return listaDepartamentos;
}


function retornaDepartamentos(parDepartmentId, retornaDepartamentoAtual, listaParDepartment) {

	currentParDepartment_Id = parDepartmentId;

	currentParCargo_Id = null;

	if (currentsParDepartments_Ids.indexOf(parDepartmentId) >= 0)
		currentsParDepartments_Ids = currentsParDepartments_Ids.slice(0, currentsParDepartments_Ids.indexOf(parDepartmentId));

	if (parDepartmentId)
		currentsParDepartments_Ids.push(parseInt(parDepartmentId));
	else
		currentsParDepartments_Ids = [];

	var data = listaParDepartment;

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

$('body').off('click', '[data-par-department-id]').on('click', '[data-par-department-id]', function (e) {

	var parDepartmentId = $(this).attr('data-par-department-id');

	listarParDepartment(parDepartmentId, false);

});

function voltarDepartment(parent_Id) {

	currentParDepartment_Id = parent_Id;

	listarParDepartment(currentParDepartment_Id, true);
}