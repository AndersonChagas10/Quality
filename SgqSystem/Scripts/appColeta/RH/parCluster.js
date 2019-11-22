function listarParCluster(isVoltar) {

    cleanGlobalVarParCLuster();

    if (!parametrization.listarParCluster.length > 0) {
        return false;
    }

    var listaParCluster = parametrization.listarParCluster;

    var htmlParCluster = "";

    getCurrentPlanejamentoObj();

    $(listaParCluster).each(function (i, o) {
        htmlParCluster += '<button type="button" class="list-group-item col-xs-12" ' +
            '" data-par-cluster-id="' + o.Id + '">' + o.Name +
            '<span class="badge">></span>' +
            '</button>';
    }); 

    //var voltar = '<a onclick="listarParDepartment(' + currentParDepartmentParent_Id + "," + isVoltar + '  );" class="btn btn-warning">Voltar</a>';
    var voltar = '<a onclick="validaRota(openMenu,null);" class="btn btn-warning">Voltar</a>';

    html = getHeader() +
        '<div class="container-fluid">                                       ' +
        '    <div class="">                                         ' +
        '        <div class="col-xs-12">                               ' +
        '            <div class="panel panel-primary">                 ' +
        '              <div class="panel-heading">                     ' +
        '                <h3 class="panel-title">' + voltar + ' Selecione o cluster que deseja coletar</h3>        ' +
        '              </div>                                          ' +
        '              <div class="panel-body">                        ' +
        '                <div class="list-group" id="divCluster">                      ' +
        htmlParCluster +
        '                </div>                                        ' +
        '              </div>                                          ' +
        '            </div>                                            ' +
        '        </div>                                                ' +
        '    </div>                                                    ' +
        '</div>';

    $('div#app').html(html);

    setBreadcrumbs();

    if ($(".list-group button").length == 1 && (isVoltar == false || isVoltar == undefined)) {
        $("[data-par-cluster-id]").trigger('click');
    }

}

function podeRealizarColeta(_currentEvaluation, _currentTotalEvaluation) {
    _currentTotalEvaluation = parseInt(_currentTotalEvaluation) > 0 ? _currentTotalEvaluation : 1;
    return parseInt(_currentEvaluation) <= parseInt(_currentTotalEvaluation);
}

$('body').off('click', '[data-par-cluster-id]').on('click', '[data-par-cluster-id]', function (e) {

    currentTotalEvaluationValue = $(this).attr('data-total-evaluation');
    currentTotalSampleValue = $(this).attr('data-total-sample');
    var currentEvaluationValue = $(this).attr('data-current-evaluation');

    if (!podeRealizarColeta(currentEvaluationValue, currentTotalEvaluationValue)) {
        //alert('Não há mais avaliações disponiveis para realização de coleta para este cargo');

        openMensagem('Não há mais avaliações disponiveis para realização de coleta para este cargo', 'red', 'white');
        closeMensagem(2000);
        return;
    }

    currentParCluster_Id = parseInt($(this).attr('data-par-cluster-id'));

    listarParLevels();

});

function cleanGlobalVarParCLuster () {
    currentParCluster_Id = null;
}

