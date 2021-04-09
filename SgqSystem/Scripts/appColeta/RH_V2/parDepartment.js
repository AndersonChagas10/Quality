function listarParDepartment(parDepartmentId, isVoltar) {

    var listaDepartamentos = retornaDepartamentos(parDepartmentId, true, parametrization.listaParDepartment);

	var htmlParDepartment = "";

	var department = {};

    listaDepartamentos = listaDepartamentos.sort((a, b) => (a.Name > b.Name) ? 1 : -1);

    var listaAvAmPorDepartamento = retornaListaContadorPorDepartamento(parDepartmentId);

	$(listaDepartamentos).each(function (i, o) {

        var objAvAm = $.grep(listaAvAmPorDepartamento, function (x, i) {
            if (x.parDepartmentParent_Id == o.Id || x.parDepartment_Id == o.Id)
                return x;
        });

        var contador = {};
        if (objAvAm[0])
            contador = objAvAm[0];
            

		var style = '';
		var contadorHtml = '';

		var temp_currentsParDepartments_Ids = $.grep(currentsParDepartments_Ids, function (o, i) {return true;});
		var temp_currentParDepartment_Id = currentParDepartment_Id;
        if (bloqueiaCentroDeCustoParaColeta(o.Id) == true) {
            style = 'style="background-color:#ddd;cursor:not-allowed"';
        } else {
            contadorHtml = `<span> AV: ${contador.av}/${contador.avMax} | AM:  ${contador.am}/${contador.amMax} </span>`;
        }
		currentsParDepartments_Ids = temp_currentsParDepartments_Ids;
		currentParDepartment_Id = temp_currentParDepartment_Id;

		if (parseInt(parDepartmentId) > 0 && parDepartmentId == o.Id) {
			department = o;
		} else
			if ((parDepartmentId > 0 && parDepartmentId == o.Parent_Id) || ((parDepartmentId == 0 || parDepartmentId == null) && (o.Parent_Id == 0 || o.Parent_Id == null))) {
				htmlParDepartment += '<button type="button" '+style+' class="list-group-item col-xs-12" ' +
                    'data-par-department-id="' + o.Id + '" data-par-department-parend-id="' + o.Parent_Id + '">' + o.Name +
                    contadorHtml +
					'<span class="badge">></span>' +
					'</button>';
			}

	});

	currentParDepartment_Id = department.Id;

    //caso for "" quer dizer que não tem mais filhos, então abre o próximo	
    if (htmlParDepartment == "") {
        currentParDepartmentParent_Id = department.Parent_Id;
        listarParCargo(isVoltar);
		return;
	}

	var voltar = "";

    if (globalLogo)
        systemLogo = 'background-image: url(' + globalLogo + ')';

	if (parDepartmentId == 0 || parDepartmentId == undefined || parDepartmentId == null) 

        voltar = '<a onclick="validaRota(openParFrequencyXindicador,null);" class="btn btn-warning col-xs-12" style="margin-bottom:10px;">Voltar</a>';

	else {

        voltar = '<a onclick="voltarDepartment(' + department.Parent_Id + ');" class="btn btn-warning col-xs-12" style="margin-bottom:10px;">Voltar</a>';
	}

	html = getHeader() +
		'<div class="container-fluid">                                           ' +
		'	<div class="">                                              ' +
		'		<div class="col-xs-12">                                    ' +
		'                                                                  ' +
        '			<div class="panel">                      ' +
        '			  <div class="panel-heading" style="background-color:#DCE6F1;">                          ' +

        '<div style="height: 220px; text-align: center; background-repeat: no-repeat;background-size: auto 100%;background-position: center;height: 220px; ' + systemLogo + '">' +
        '</div>' +

        '			    <div class="row">                          ' +
        '			      <div class="col-xs-9">                         ' +
        '				      <h3 class="panel-title">Selecione o centro de custo desejado</h3>            ' +
        '                 </div >                                          ' +
        '                 <div class="col-sm-3">                           ' +
        getBotaoBuscarSecaoXCargo() +
        '                 </div>                                           ' +
        '               </div>                                             ' +

        '			  </div>                                               ' +
		'			  <div class="panel-body">                             ' +
		'				<div class="list-group">                           ' +
        voltar +
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

function bloqueiaCentroDeCustoParaColeta(departmentId){
    var listaDeDepartamentos = retornaDepartamentos(departmentId, false, parametrization.listaParDepartment);
	
	currentParDepartment_Id = departmentId;

	if(listaDeDepartamentos.length > 0){
		for(var i = 0; i < listaDeDepartamentos.length; i++){
			var d = listaDeDepartamentos[i];
			if(bloqueiaCentroDeCustoParaColeta(d.Id) == false){
				return false;
			}
		}
	}else{
        var listaParCargo = retornaCargos(currentParDepartment_Id);
		
		for(var i = 0; i < listaParCargo.length; i++){
			if(bloqueiaCargoParaColeta(listaParCargo[i]) == false){
				return false;
			}
		}
	}
	return true;
}

function retornarArvoreDesdobramentoDepartamentoCargo(listaDeDepartamentos) {
    var listaDesdobramento = [];
    $(listaDeDepartamentos).each(function (i_departamento, departamento) {
        if (departamento["Parent_Id"]) {
            return;
        }
        var listaFilho = retornaDepartamentos(departamento["Id"], true, parametrization.listaParDepartment);

        $(listaFilho).each(function (i_secao, secao) {
            if (!secao["Parent_Id"]) {
                return;
            }
            var listaCargo = retornaCargos(secao["Id"]);

            $(listaCargo).each(function (i_cargo, cargo) {
                listaDesdobramento.push(
                    {
                        ParDepartmentParent_Id: departamento["Id"],
                        ParDepartmentParent_Name: departamento["Name"],
                        ParDepartment_Id: secao["Id"],
                        ParDepartment_Name: secao["Name"],
                        ParCargo_Id: cargo["Id"],
                        ParCargo_Name: cargo["Name"],
                    }
                );
            });
        });
    });

    return listaDesdobramento;
}
function retornaContadorPorDepartamento(listaDeDepartamento, parDepartmentParent_Id, parDepartment_Id) {
    var desdobramento = retornarArvoreDesdobramentoDepartamentoCargo(listaDeDepartamento);

    var contador = {
        av: 0,
        am: 0,
        avMax: 0,
        amMax: 0,
        parDepartmentParent_Id: parDepartmentParent_Id,
        parDepartment_Id: parDepartment_Id
    };

    $(desdobramento).each(function (i, o) {
        if (parDepartmentParent_Id != o["ParDepartmentParent_Id"])
            return;

        if (parDepartment_Id && parDepartment_Id != o["ParDepartment_Id"])
            return;

        var listaParCargo = retornaCargos(o["ParDepartment_Id"]);
        for (var i_cargo = 0; i_cargo < listaParCargo.length; i_cargo++) {
            var cargo = listaParCargo[i_cargo];

            if ((parDepartment_Id == cargo["Evaluation"]['ParDepartment_Id'] || parDepartment_Id == undefined) && o["ParCargo_Id"] == cargo['Id']) {
                var avMaxima = cargo["Evaluation"]["Evaluation"];
                var amMaxima = cargo["Evaluation"]["Sample"];

                contador["avMax"] += avMaxima;
                contador["amMax"] += avMaxima * amMaxima;

                if ((parDepartment_Id == o['ParDepartment_Id'] || parDepartment_Id == undefined)
                    && parDepartmentParent_Id == o['ParDepartmentParent_Id']) {

                    //Regra para pegar AV e AM coletadas
                    for (var i_coleta = 0; i_coleta < coletasAgrupadas.length; i_coleta++) {

                        var coleta = coletasAgrupadas[i_coleta];
                        var avAtual = 0;
                        var amAtual = 0;
                        var avColetada = 0;

                        if (parDepartment_Id != undefined) {
                            if (parDepartment_Id == coleta["ParDepartment_Id"]
                                && coleta['ParCargo_Id'] == o['ParCargo_Id']) {

                                retornaValoresContador(coleta, avAtual, amAtual, avColetada, contador, avMaxima, amMaxima);
                                break;
                            }
                        } else {
                            if (coleta["ParDepartment_Id"] == o['ParDepartment_Id']
                                && coleta['ParCargo_Id'] == o['ParCargo_Id']) {

                                retornaValoresContador(coleta, avAtual, amAtual, avColetada, contador, avMaxima, amMaxima);
                                break;
                            }
                        }

                       
                    }
                    break;
                }
       
            }
        }
    });

    return contador;
}

function retornaValoresContador(coleta, avAtual, amAtual, avColetada, contador, avMaxima, amMaxima) {
    avAtual = coleta["Evaluation"];
    amAtual = coleta["Sample"];
    avColetada = avMaxima > avAtual ? avAtual : avMaxima;
    contador["av"] += avColetada;
    contador["am"] += (amAtual - 1) + ((avAtual - 1) * amMaxima);
}

function retornaListaContadorPorDepartamento(parDepartmentParent_Id) {
    //Retorna a lista de departamento, ou seção.
    var listaDepartamento = retornaDepartamentos(parDepartmentParent_Id, true, parametrization.listaParDepartment);
    var listaContador = [];

    //Percorre a lista e retorna o contador, seja secao ou departamento
    $(listaDepartamento).each(function (i, o) {
        var contador;
        if (parDepartmentParent_Id && o["Parent_Id"]) {
            contador = retornaContadorPorDepartamento(parametrization.listaParDepartment, o["Parent_Id"], o["Id"]);
            listaContador.push(contador);
        } else if (!parDepartmentParent_Id) {
            contador = retornaContadorPorDepartamento(parametrization.listaParDepartment, o["Id"]);
            listaContador.push(contador);
        }
    });

    return listaContador;
}

function bloqueiaCargoParaColeta(c) {
	currentEvaluationSample = getResultEvaluationSample(currentParDepartment_Id, c.Id);
	
	//FIX para trabalhar de forma correta os valores 
	//que são recebidos do backend com os resultados
    if (currentEvaluationSample.Sample > c.Evaluation.Sample) {
		currentEvaluationSample.Evaluation += 1;
		currentEvaluationSample.Sample = 1;
	}
	
	return !podeRealizarColeta(currentEvaluationSample.Evaluation, c.Evaluation.Evaluation);
}