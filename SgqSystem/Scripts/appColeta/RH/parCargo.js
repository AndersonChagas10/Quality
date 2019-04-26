function listarParCargo() {

    cleanGlobalVarParCargo();

    if (!parametrization.listaParCargoXDepartment.length > 0) {
        return false;
    }

    var listaParCargoXDepartment = $.grep(parametrization.listaParCargoXDepartment, function (item) {
        return item.ParDepartment_Id == currentParDepartment_Id;
    });

    var listaParCargo = [];

    $(listaParCargoXDepartment).each(function (item, obj) {

        var listaParCargoFilter = $.grep(parametrization.listaParCargo, function (parCargo) {
            return (parCargo.Id == obj.ParCargo_Id || obj.ParCargo_Id == null);
        });

        listaParCargoFilter.forEach(function (item) {

            //Verificar primeiramente os que existem ParCargo e ParCompany obs (frequencia e departamento são obrigatorios)
            var listaEvaluation = [];

            //pegar os dados que possuem unidade, cargo e 
            listaEvaluation = $.grep(parametrization.listaParEvaluationXDepartmentXCargoAppViewModel, function (parEvaluation) {
                return parEvaluation.ParCargo_Id == obj.ParCargo_Id &&
                    parEvaluation.ParDepartment_Id == currentParDepartment_Id &&
                    parEvaluation.ParCompany_Id == currentLogin.ParCompany_Id &&
                    parEvaluation.parFrequency_Id == currentParFrequency_Id;
            });

            //Caso não existir, buscar os que possuem todas as unidades
            if (listaEvaluation.length == 0) {
                listaEvaluation = $.grep(parametrization.listaParEvaluationXDepartmentXCargoAppViewModel, function (parEvaluation) {
                    return parEvaluation.ParCargo_Id == obj.ParCargo_Id &&
                        parEvaluation.ParDepartment_Id == currentParDepartment_Id &&
                        parEvaluation.parFrequency_Id == currentParFrequency_Id &&
                        parEvaluation.ParCompany_Id == null
                });
            }

            //Busca o que possui todas as unidades e todos os cargos
            if (listaEvaluation.length == 0) {
                listaEvaluation = $.grep(parametrization.listaParEvaluationXDepartmentXCargoAppViewModel, function (parEvaluation) {
                    return parEvaluation.ParCargo_Id == obj.ParCargo_Id &&
                        parEvaluation.ParDepartment_Id == currentParDepartment_Id &&
                        parEvaluation.ParCompany_Id == null &&
                        parEvaluation.parFrequency_Id == null;
                });
            }

            if (listaEvaluation.length > 0) {
                item['Evaluation'] = listaEvaluation[0]; //o correto é que retorne somente um, mas caso retorne mais do que um, não pode dar erro
                listaParCargo.push(item);
            }
        });
    });

    var htmlParCargo = "";

    $(listaParCargo).each(function (i, o) {
        currentEvaluationSample = getResultEvaluationSample(currentParDepartment_Id, o.Id);

        //FIX para trabalhar de forma correta os valores 
        //que são recebidos do backend com os resultados
        if (currentEvaluationSample.Sample >= o.Evaluation.Sample)
            currentEvaluationSample.Evaluation += 1;

        var style = '';
        if (!podeRealizarColeta(currentEvaluationSample.Evaluation, o.Evaluation.Evaluation)) {
            style = 'style="background-color:#ddd;cursor:not-allowed"';

            htmlParCargo += '<button type="button" ' + style + ' class="list-group-item col-xs-12" ' +
                'data-par-cargo-id="' + o.Id + '"                                                    ' +
                'data-total-evaluation="' + o.Evaluation.Evaluation + '"                             ' +
                'data-total-sample="' + o.Evaluation.Sample + '"                                     ' +
                'data-current-evaluation="' + currentEvaluationSample.Evaluation + '"                ' +
                'data-current-sample="' + currentEvaluationSample.Sample + '">                       ' +
                '	<div class="col-xs-4">' + o.Name + '</div>                                      ' +
                '	<div class="col-xs-4">&nbsp;</div>                                         ' +
                '	<div class="col-xs-4">&nbsp;</div>                                         ' +
                '</button>';
        } else {
            htmlParCargo += '<button type="button" class="list-group-item col-xs-12"                                       ' +
                'data-par-cargo-id="' + o.Id + '"                                                                                ' +
                'data-total-evaluation="' + o.Evaluation.Evaluation + '"                                                         ' +
                'data-total-sample="' + o.Evaluation.Sample + '"                                                                 ' +
                'data-current-evaluation="' + currentEvaluationSample.Evaluation + '"                                            ' +
                'data-current-sample="' + currentEvaluationSample.Sample + '">                                                   ' +
                '	<div class="col-xs-4">' + o.Name + '</div>                                                                  ' +
                '	<div class="col-xs-4">Av: ' + currentEvaluationSample.Evaluation + '/' + o.Evaluation.Evaluation + ' </div>      ' +
                '	<div class="col-xs-4">Am: ' + currentEvaluationSample.Sample + '/' + o.Evaluation.Sample + ' </div>              ' +
                '</button>';
        }
    });

    var voltar = '<a onclick="listarParDepartment(' + currentParDepartmentParent_Id + ');" class="btn btn-warning">Voltar</a>';

    html = getHeader() +
        '<div class="container-fluid">                                       ' +
        '    <div class="">                                         ' +
        '        <div class="col-xs-12">                               ' +
        '            <div class="panel panel-primary">                 ' +
        '              <div class="panel-heading">                     ' +
        '                <h3 class="panel-title">' + voltar + ' Selecione o cargo que deseja coletar</h3>        ' +
        '              </div>                                          ' +
        '              <div class="panel-body">                        ' +
        '                <div class="list-group">                      ' +
        htmlParCargo +
        '                </div>                                        ' +
        '              </div>                                          ' +
        '            </div>                                            ' +
        '        </div>                                                ' +
        '    </div>                                                    ' +
        '</div>';

    $('div#app').html(html);

}

function cleanGlobalVarParCargo() {
    currentParCargo_Id = null;
}

function podeRealizarColeta(_currentEvaluation, _currentTotalEvaluation) {
    _currentTotalEvaluation = parseInt(_currentTotalEvaluation) > 0 ? _currentTotalEvaluation : 1;
    return parseInt(_currentEvaluation) <= parseInt(_currentTotalEvaluation);
}

$('body').off('click', '[data-par-cargo-id]').on('click', '[data-par-cargo-id]', function (e) {

    currentTotalEvaluationValue = $(this).attr('data-total-evaluation');
    currentTotalSampleValue = $(this).attr('data-total-sample');
    var currentEvaluationValue = $(this).attr('data-current-evaluation');

    if (!podeRealizarColeta(currentEvaluationValue, currentTotalEvaluationValue)) {
        //alert('Não há mais avaliações disponiveis para realização de coleta para este cargo');

        openMensagem('Não há mais avaliações disponiveis para realização de coleta para este cargo', 'red', 'white');
        closeMensagem(2000);
        return;
    }

    currentParCargo_Id = parseInt($(this).attr('data-par-cargo-id'));

    listarParLevels();

});