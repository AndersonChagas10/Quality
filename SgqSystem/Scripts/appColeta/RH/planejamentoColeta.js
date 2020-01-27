function openPlanejamentoColeta() {

    preencheCurrentPPlanejamento(getFrequenciaSelecionada);

}

function preencheCurrentPPlanejamento(callback) {
    _readFile("planejamento.txt", function (data) {
        if (data && data.length > 1)
            currentPlanejamento = JSON.parse(data);

        if (callback)
            callback();
    });
}

function getFrequenciaSelecionada() {

    _readFile("parFrequency.txt", function (data) {
        var frequencia = JSON.parse(data);
        _readFile("parCluster.txt", function (data2) {
            _readFile("parClusterGroup.txt", function (data3) {

                data = frequencia;
                data2 = JSON.parse(data2);
                data3 = JSON.parse(data3);

                var frequenciaSelecionada = $.map(data, function (n) {
                    if (currentParFrequency_Id == n.Id) {
                        return n;
                    }
                });

                var clusterSelecionado = $.map(data2, function (n) {
                    if (currentParCluster_Id == n.Id) {
                        return n;
                    }
                });

                var clusterGroupSelecionado = $.map(data3, function (n) {
                    if (currentParClusterGroup_Id == n.Id) {
                        return n;
                    }
                });
                renderPlanejamentoColeta(frequenciaSelecionada[0], clusterSelecionado[0], clusterGroupSelecionado[0]);
                //$(data).each(function (i, o) {
                //    if (currentParFrequency_Id == o.Id) {
                //        frequenciaSelecionada = o;
                //        renderPlanejamentoColeta(frequenciaSelecionada);
                //        return;
                //    }
                //});
            });
        });
    });
}

function renderPlanejamentoColeta(frequencia, cluster, clusterGroup) {

    var html = '';

    var voltar = '<a onclick="voltarPlanejamentoColeta();" class="btn btn-warning">Voltar</a>';

    var btnColetar = '<button type="button" class="btn btn-success pull-right btncoletar" onclick="clickColetar()">Coletar</button>';

    var btnbaixarParams = '<button type="button" style="margin-right:10px;" class="btn btn-success pull-right btnGetParams" onclick="getParametrizationByButon()">Baixar Parametrização</button>';

    html = getHeader() +
        '<div class="container-fluid">                               ' +
        '	<div class="">                                  ' +
        '		<div class="col-xs-12">                        ' +
        '			<div class="panel panel-primary">          ' +
        '			  <div class="panel-heading" style="display: table;width: 100%;">              ' +
        '				<h3 class="panel-title">' + voltar +
        btnColetar +
        //btnbaixarParams +
        '<br>' +
        '			  </h3></div>                                   ' +
        '			  <div class="panel-body" style="padding-top: 10px !important">                 ' +
        '				<div class="list-group">               ' +
        '					<div class="col-sm-6">               ' +

        '	<div class="form-group">' +
        '	<label>Grupo de Cluster:</label>' +
        '	<input type="hidden"value="' + clusterGroup.Id + '">' +
        '	<input type="text" class="form-control" value="' + clusterGroup.Name + '" readonly>' +
        '</div>' +

        '	<div class="form-group">' +
        '	<label>Cluster:</label>' +
        '	<input type="hidden"value="' + cluster.Id + '">' +
        '	<input type="text" class="form-control" value="' + cluster.Name + '" readonly>' +
        '</div>' +

        '	<div class="form-group">' +
        '	<label>Frequencia:</label>' +
        '	<input type="hidden"value="' + frequencia.Id + '">' +
        '	<input type="text" class="form-control" value="' + frequencia.Name + '" readonly>' +
        '</div>' +
        '<div data-selects-cc>' +
        criaHtmlSelect('Centro de Custo:', retornaOptions(retornaDepartamentos(0, undefined, parametrization.listaParDepartment), 'Id', 'Name', 'Selecione')) +
        '</div>' +
        '<div data-selects-cargo>' +
        '</div>' +
        '<div data-selects-indicador>' +
        '</div>' +
        '<div class="form-group">' +
        '<button type="button" style="margin-right:10px;" class="btn btn-primary" onClick="savePlanejar()">Planejar</button>' +
           '<button type="button" class="btn btn-success btncoletar" onclick="clickColetar()">Coletar</button>' +
        '</div>' +
      

        '</div>                                 ' +
        '	<div class="col-sm-6">               ' +
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

    changeStateButtonColetar();

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
        openParCluster();
        //openParFrequency();
    }
}

function verificaAlgumIndicadorClicado(levels1) {
    if ($(levels1).hasClass('btn-success'))
        return true;
    else
        return false;
}

var planejamento = {};
function savePlanejar() {

    var levels1 = $('body [data-selects-indicador] button');

    //caso seja planejado antes de mostrar os indicadores
    if (planejamento.parCargo_Name == null || planejamento.parCargo_Name == undefined) {
        validaPlanejamentoESalva(planejamento);
        return false;
    }

    if (verificaAlgumIndicadorClicado(levels1)) {

        var todos = true;
        $(levels1).each(function (i, o) {
            if ($(o).hasClass('btn-default')) {
                todos = false;
                return false;
            }
        });

        planejamento.indicador_Id = undefined;
        planejamento.indicador_Name = undefined;

        if (todos != true) {
            $(levels1).each(function (i, o) {
                if ($(o).hasClass('btn-success')) {
                    planejamento.indicador_Id = $(o).attr('data-level1-id');
                    planejamento.indicador_Name = $(o).text();

                    validaPlanejamentoESalva(planejamento);
                }
            });
        } else {
            validaPlanejamentoESalva(planejamento);
        }

    } else {
        openMensagem("Selecione um ou mais indicadores para planejar", '#428bca', 'white');
        closeMensagem(2000);
        return false;
    }
}

function validaPlanejamentoESalva(planejamento) {
    if (!planejamentoIsValid())
        return false;

    if (planejamento.parDepartment_Id > 0) {
        currentPlanejamento.push($.extend({}, planejamento));
        $('[data-save-planned]').html(renderPlanejamentos());
        saveInFilePlanejamento();
        changeStateButtonColetar();
    }
}

function planejamentoIsValid() {

    var plan = $.grep(currentPlanejamento, function (o) {
        return o.parDepartment_Id == planejamento.parDepartment_Id &&
            o.parCargo_Id == planejamento.parCargo_Id &&
            o.indicador_Id == planejamento.indicador_Id
    });

    if (plan && plan.length > 0) {
        openMensagem("Planejamento já existente", '#428bca', 'white');
        closeMensagem(2000);
        return false;
    }


    return true;
}

function removePlanejamento(index) {
    currentPlanejamento.splice(index, 1);
    $('[data-save-planned]').html(renderPlanejamentos());
    saveInFilePlanejamento();
    changeStateButtonColetar();
}

function saveInFilePlanejamento() {
    _writeFile("planejamento.txt", JSON.stringify(currentPlanejamento), function () {
    });
}

function downloadPlanejamento() {
    console.log(JSON.stringify({
        ParCompany_Id: currentParCompany_Id
        , ParFrequency_Id: currentParFrequency_Id
        , ParCluster_Id: currentParCluster_Id
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
            var departamentos = retornaOptions(options, 'Id', 'Name', 'Selecione');

            $('[data-selects-cc]').append(criaHtmlSelect('', departamentos));
        } else {
            $('[data-selects-cargo]').html(criaHtmlSelect('Cargo:', retornaOptions(retornaCargos($(this).val()), 'Id', 'Name', 'Selecione')));
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

        $('[data-selects-indicador]').html(criaHtmlButtonsIndicador('Indicador:', level1List));
    } else {
        planejamento.parCargo_Id = undefined;
        planejamento.parCargo_Name = undefined;
    }

});

function criaHtmlButtonsIndicador(titulo, level1List) {
    var htmlLevel1Button = "";
    htmlLevel1Button += '<label>Indicador:</label> <div class="form-group">';

    htmlLevel1Button += '<div class="form-check">' +
                            '<input type="checkbox" class="form-check-input" id="checkLevel1">' +
                            '<label class="form-check-label" for="checkLevel1">Selecionar todos os indicadores:</label>' +
                        '</div>';
                        
    $(level1List).each(function (i, o) {
        htmlLevel1Button += '<button type="button" data-level1-id="' + o.Id + '" onclick="flagSelectedLevel1(this)" class="btn btn-md btn-default" style="margin: 5px;">' + o.Name + '</button>';
    });
    htmlLevel1Button += '</div>';
    return htmlLevel1Button;
}

$('body').off('click', '#checkLevel1').on('click', '#checkLevel1', function (e) {

    if ($("#checkLevel1").is(":checked")) {
        $($('body [data-selects-indicador] button')).each(function (i, o) {
            $(o).removeClass('btn-default');
            $(o).addClass('btn-success');
        });
    } else {
        $($('body [data-selects-indicador] button')).each(function (i, o) {
            $(o).removeClass('btn-success');
            $(o).addClass('btn-default');
        });
    }
});


function flagSelectedLevel1(data) {
    if ($(data).hasClass('btn-success')) {
        $(data).removeClass('btn-success');
        $(data).addClass('btn-default');
    } else {
        $(data).removeClass('btn-default');
        $(data).addClass('btn-success');
    }

    if (!$('body [data-selects-indicador] button').hasClass('btn-default')) {
        $("#checkLevel1").trigger('click');
    } else {
        $("#checkLevel1").prop('checked', false);
    }
}


//Filtros de planejamento de coleta
function getParDepartmentPlanejado() {

    var todosParDepartments = parametrization.listaParDepartment;

    var ParDepartments_Ids = $.unique($.map(currentPlanejamento, function (o) {

        return o["parDepartment_Id"];

    }).sort());

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

    // var planejamentos = $.grep(currentPlanejamento, function (o) {
    // 	if (o.parCargo_Id)
    // 		return o.ParDepartment_Id = currentParDepartment_Id;
    // });

    var planejamentos = $.grep(currentPlanejamentoArr, function (o) {
        if (o.parCargo_Id)
            return o.ParDepartment_Id = currentParDepartment_Id;
    });

    if (planejamentos.length == 0)
        return listaParCargo;

    var listaCargoFiltrada = $.map(planejamentos, function (o) {
        return o.parCargo_Id;
    });

    if (listaCargoFiltrada)
        listaCargoFiltrada = $.unique(listaCargoFiltrada.sort());

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

    // var planejamentos = $.grep(currentPlanejamento, function (o) {
    // 	if (o.parCargo_Id && o.indicador_Id)
    // 		return o.ParDepartment_Id == currentParDepartment_Id && o.parCargo_Id == currentParCargo_Id;
    // });

    var planejamentos = $.grep(currentPlanejamentoArr, function (o) {
        if (o.parCargo_Id && o.indicador_Id)
            return o.ParDepartment_Id == currentParDepartment_Id &&
                o.parCargo_Id == currentParCargo_Id;
    });

    if (planejamentos.length == 0)
        return listaParLevel1;

    var listaIndicadorFiltrado = $.map(planejamentos, function (o) {
        return o.indicador_Id;
    });

    if (listaIndicadorFiltrado)
        listaIndicadorFiltrado = $.unique(listaIndicadorFiltrado.sort());

    var newListaIndicador = [];

    $.each(listaIndicadorFiltrado, function (i, o) {

        var listaDeIndicadorPlanejado = $.grep(listaParLevel1, function (oo) {
            return oo.Id == o;
        });

        newListaIndicador = newListaIndicador.concat(listaDeIndicadorPlanejado);
    });

    return newListaIndicador;
}

var currentPlanejamentoArr = [];

function getCurrentPlanejamentoObj() {


    if (!!currentPlanejamentoArr.length &&
        currentsParDepartments_Ids.indexOf(currentPlanejamentoArr[0].ParDepartment_Id) >= 0) {

        return currentPlanejamentoArr

    }

    var arr = $.grep(currentPlanejamento, function (o) {

        if (o.parDepartment_Id == currentParDepartment_Id &&
            o.parCargo_Id == undefined &&
            o.indicador_Id == undefined)

            return o;
    });

    if (!arr.length)

        arr = $.grep(currentPlanejamento, function (o) {


            if (o.parDepartment_Id == currentParDepartment_Id &&
                o.parCargo_Id == currentParCargo_Id &&
                o.indicador_Id == undefined)

                return o;

        });

    if (!arr.length)

        arr = $.grep(currentPlanejamento, function (o) {

            if (o.parDepartment_Id == currentParDepartment_Id &&
                o.parCargo_Id == currentParCargo_Id)

                return o;

        });

    if (!arr.length)

        arr = $.grep(currentPlanejamento, function (o) {

            if (o.parDepartment_Id == currentParDepartment_Id &&
                o.indicador_Id == undefined)

                return o;
        });

    if (!arr.length)

        arr = $.grep(currentPlanejamento, function (o) {

            if (o.parDepartment_Id == currentParDepartment_Id)

                return o;
        });

    currentPlanejamentoArr = !!arr.length ? arr : [];
}