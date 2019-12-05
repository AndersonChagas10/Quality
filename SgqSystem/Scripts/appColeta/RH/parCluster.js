function openParCluster() {

    var html = '';

    _readFile("parCluster.txt", function (data) {
        if (globalLoginOnline) {

            openMensagem('Carregando lista de clusters', 'blue', 'white');

            $.ajax({
                data: {},
                url: urlPreffix + '/api/parCluster',
                type: 'GET',
                success: function (data) {

                    _writeFile("parCluster.txt", JSON.stringify(data), function () {
                        listaParCluster = data;
                        listarParCluster();
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
            listarParCluster();
        }

    });
}

function listarParCluster() {

    cleanGlobalVarParCluster();

    _readFile("parCluster.txt", function (data) {

        data = JSON.parse(data);

        listaParCluster = data;

        var cluster = {};

        var htmlParCluster = "";

        $(data).each(function (i, o) {

            htmlParCluster += '<button type="button" class="list-group-item col-xs-12" data-par-cluster-id="' + o.Id + '" ' +
                ((currentParCluster_Id == o.Id || !(currentParCluster_Id > 0)) ? '' : 'style="background-color:#eee;cursor:not-allowed"')
                + '>' + o.Name +
                '</button>';
        });

        var voltar = '<a onclick="validaRota(openMenu,null);" class="btn btn-warning">Voltar</a>';

        html = getHeader() +
            '<div class="container-fluid">                               ' +
            '	<div class="">                                  ' +
            '		<div class="col-xs-12">                        ' +
            '			<div class="panel panel-primary">          ' +
            '			  <div class="panel-heading">              ' +
            '				<h3 class="panel-title">' + voltar +   'Qual cluster deseja realizar coleta?</h3>'+
            '			  </div>                                   ' +
            '			  <div class="panel-body">                 ' +
            '				<div class="list-group">               ' +
            htmlParCluster +
            '				</div>                                 ' +
            '			  </div>                                   ' +
            '			</div>                                     ' +
            '		</div>                                         ' +
            '	</div>                                             ' +
            '</div>';

        $('div#app').html(html);
    });
}

function cleanGlobalVarParCluster() {
    currentParDepartment_Id = null;
    currentParCargo_Id = null;
    currentParCluster_Id = null;
    currentsParDepartments_Ids = [];
}

$('body').off('click', '[data-par-cluster-id]').on('click', '[data-par-cluster-id]', function (e) {

    currentParCluster_Id = parseInt($(this).attr('data-par-cluster-id'));

    openParFrequency();
    //getPlanejamentoPorFrequencia(frequencyId);

});

function getPlanejamentoPorCluster(frequencyId) {

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
                    openParFrequency();
                    closeMensagem();
                });
                sincronizarResultado(currentParFrequency_Id);
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

            openParFrequency();
            closeMensagem();
        });
    }
}
















//function listarParCluster(isVoltar) {

//    cleanGlobalVarParCLuster();

//    if (!parametrization.listarParCluster.length > 0) {
//        return false;
//    }

//    var listaParCluster = parametrization.listarParCluster;

//    var htmlParCluster = "";

//    getCurrentPlanejamentoObj();

//    $(listaParCluster).each(function (i, o) {
//        htmlParCluster += '<button type="button" class="list-group-item col-xs-12" ' +
//            '" data-par-cluster-id="' + o.Id + '">' + o.Name +
//            '<span class="badge">></span>' +
//            '</button>';
//    }); 

//    //var voltar = '<a onclick="listarParDepartment(' + currentParDepartmentParent_Id + "," + isVoltar + '  );" class="btn btn-warning">Voltar</a>';
//    var voltar = '<a onclick="validaRota(openMenu,null);" class="btn btn-warning">Voltar</a>';

//    html = getHeader() +
//        '<div class="container-fluid">                                       ' +
//        '    <div class="">                                         ' +
//        '        <div class="col-xs-12">                               ' +
//        '            <div class="panel panel-primary">                 ' +
//        '              <div class="panel-heading">                     ' +
//        '                <h3 class="panel-title">' + voltar + ' Selecione o cluster que deseja coletar</h3>        ' +
//        '              </div>                                          ' +
//        '              <div class="panel-body">                        ' +
//        '                <div class="list-group" id="divCluster">                      ' +
//        htmlParCluster +
//        '                </div>                                        ' +
//        '              </div>                                          ' +
//        '            </div>                                            ' +
//        '        </div>                                                ' +
//        '    </div>                                                    ' +
//        '</div>';

//    $('div#app').html(html);

//    setBreadcrumbs();

//    if ($(".list-group button").length == 1 && (isVoltar == false || isVoltar == undefined)) {
//        $("[data-par-cluster-id]").trigger('click');
//    }

//}

//function podeRealizarColeta(_currentEvaluation, _currentTotalEvaluation) {
//    _currentTotalEvaluation = parseInt(_currentTotalEvaluation) > 0 ? _currentTotalEvaluation : 1;
//    return parseInt(_currentEvaluation) <= parseInt(_currentTotalEvaluation);
//}

//$('body').off('click', '[data-par-cluster-id]').on('click', '[data-par-cluster-id]', function (e) {

//    currentTotalEvaluationValue = $(this).attr('data-total-evaluation');
//    currentTotalSampleValue = $(this).attr('data-total-sample');
//    var currentEvaluationValue = $(this).attr('data-current-evaluation');

//    if (!podeRealizarColeta(currentEvaluationValue, currentTotalEvaluationValue)) {
//        //alert('Não há mais avaliações disponiveis para realização de coleta para este cargo');

//        openMensagem('Não há mais avaliações disponiveis para realização de coleta para este cargo', 'red', 'white');
//        closeMensagem(2000);
//        return;
//    }

//    currentParCluster_Id = parseInt($(this).attr('data-par-cluster-id'));

//    listarParLevels();

//});

//function cleanGlobalVarParCLuster () {
//    currentParCluster_Id = null;
//}

