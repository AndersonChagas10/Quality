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
            return (parCargo.Id == obj.ParCargo_Id);
        });

        listaParCargoFilter.forEach(function (item) {

            //Verificar primeiramente os que existem ParCargo e ParCompany obs (frequencia e departamento são obrigatorios)
            var listaEvaluation = [];

            //pegar os dados que possuem unidade, cargo 
            listaEvaluation = $.grep(parametrization.listaParEvaluationXDepartmentXCargoAppViewModel, function (parEvaluation) {
                return parEvaluation.ParCargo_Id == obj.ParCargo_Id &&
                    parEvaluation.ParDepartment_Id == currentParDepartment_Id
            });

            //Caso não existir, buscar os que possuem todas as unidades
            if (listaEvaluation.length == 0) {
                listaEvaluation = $.grep(parametrization.listaParEvaluationXDepartmentXCargoAppViewModel, function (parEvaluation) {
                    return parEvaluation.ParCargo_Id == obj.ParCargo_Id &&
                        parEvaluation.ParDepartment_Id == currentParDepartment_Id
                });
            }

            //Busca o que possui todas as unidades e todos os cargos
            if (listaEvaluation.length == 0) {
                listaEvaluation = $.grep(parametrization.listaParEvaluationXDepartmentXCargoAppViewModel, function (parEvaluation) {
                    return parEvaluation.ParCargo_Id == null &&
                        parEvaluation.ParDepartment_Id == currentParDepartment_Id
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
        if (currentEvaluationSample.Sample > o.Evaluation.Sample)
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
            atualizaCorAgendamento(o, currentEvaluationSample);
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

    setBreadcrumbs();

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

function atualizaCorAgendamento(cargo,currentEvaluationSample) {
    setTimeout(function () {
        
        if (cargo['Evaluation'].ParEvaluationScheduleAppViewModel.length > 0) {

            var av = 0;
            var ini = 0;
            var fim = 0;
            var situacao = "";

            $(cargo['Evaluation'].ParEvaluationScheduleAppViewModel).each(
				function (i2, o2) {

				    var frequenciaId = currentParFrequency_Id;
				    var hour = new Date().getHours();
				    var avaliacaoAtual = currentEvaluationSample.Evaluation;
				    var mapeamento = [o2.Av, o2.Inicio, o2.Fim];

				    if (false && frequenciaId == 10) { //Diário com Intervalo

				        var horaPrimeiraAv;
				        var horaMinutoPrimeiraAv;
				        var horaMinPermitida;
				        var horaMaxPermitida;
				        var reaudnumber = 0;
				        var hoje = new Date();
				        var agora = new Date();
				        var intervalo = mapeamento[0];

				        if (!!parseInt(avaliacaoAtual)) {

				            horaPrimeiraAv = $('.Resultlevel2[level1id=' + level1Id + '][unitid=' + unitId + '][reauditnumber=' + reaudnumber + '][level2id=' + level2Id + ']').attr('horaprimeiraavaliacao');

				            horaMinutoPrimeiraAv = horaPrimeiraAv.split(":");

				            if (typeof (horaMinutoPrimeiraAv) == 'undefined' || !horaMinutoPrimeiraAv) {
				                return;
				            }

				            var horaAv = parseInt(horaMinutoPrimeiraAv[0]);
				            var minutoAv = parseInt(horaMinutoPrimeiraAv[1]);

				            var horaMin = parseInt(intervalo.split(":")[0]) * (avaliacaoAtual - 1);
				            var minutosMin = parseInt(intervalo.split(":")[1]) * (avaliacaoAtual - 1);

				            var horaMax = parseInt(intervalo.split(":")[0]) * avaliacaoAtual;
				            var minutosMax = parseInt(intervalo.split(":")[1]) * avaliacaoAtual;

				            horaMinPermitida = new Date().setHours((horaAv + horaMin), (minutoAv + minutosMin), 0, 0);
				            horaMaxPermitida = new Date().setHours((horaAv + horaMax), (minutoAv + minutosMax), 0, 0);

				            if (agora.getTime() > (horaMinPermitida + (horaMaxPermitida - horaMinPermitida) * 0.5) && agora.getTime() <= horaMaxPermitida) {
				                situacao += "3"; //Não sei, acho que é ta quase atrasado

				            } else if (agora.getTime() >= horaMinPermitida && agora.getTime() <= horaMaxPermitida) {
				                situacao += "2"; //Pode coletar

				            } else if (agora.getTime() > horaMaxPermitida) {
				                situacao += "4"; //Atrasado

				            } else if (agora.getTime() < horaMinPermitida) {
				                situacao += "1"; //não pode coletar

				            }

				        } else {
				            situacao += "2"; //Pode coletar

				        }

				    } else {

				        var avVigente = parseInt(mapeamento[0]);

				        if (!(avaliacaoAtual > 0))
				            avaliacaoAtual = 1;

				        if (avaliacaoAtual == avVigente) {
				            av = avVigente;
				            ini = mapeamento[1];
				            fim = mapeamento[2];

				            //diario (controle por horario)
				            if (frequenciaId == 3) {
				                var hour = new Date().getTime();

				                ini = new Date().setHours(mapeamento[1].split(':')[0], mapeamento[1].split(':')[1], 0, 0);
				                fim = new Date().setHours(mapeamento[2].split(':')[0], mapeamento[2].split(':')[1], 0, 0);

				                //se o fim for para o outro dia, ou seja, menor que o inicio, soma um dia
				                if (fim < ini)
				                    fim = new Date(fim).setDate(new Date().getDate() + 1);

				                //danger  = 4 
				                //warning = 3
				                //success = 2
				                //default = 1

				                if (hour >= (ini + (fim - ini) * 0.5) && hour <= fim) {
				                    situacao += "3";
				                } else if (hour >= ini && hour <= fim) {
				                    situacao += "2";
				                } else if (hour > fim) {
				                    situacao += "4";
				                } else if (hour < ini) {
				                    situacao += "1";
				                }

				            } else if (frequenciaId == 4) { //SEMANAL

				                // var day = new Date().getDay();
				                var day = currentCollectDate.getDay()

				                if (day >= (ini + (fim - ini) * 0.5) && day <= fim) {
				                    situacao += "3";
				                } else if (day >= ini && day <= fim) {
				                    situacao += "2";
				                } else if (day > fim) {
				                    situacao += "4";
				                } else if (day < ini) {
				                    situacao += "1";
				                }

				            } else if (frequenciaId == 5) { //QUINZENAL

				                // var day = new Date().getDate() % 15;                               
				                var day = currentCollectDate.getDate() % 15;

				                if (day >= (ini + (fim - ini) * 0.5) && day <= fim) {
				                    situacao += "3";
				                } else if (day >= ini && day <= fim) {
				                    situacao += "2";
				                } else if (day > fim) {
				                    situacao += "4";
				                } else if (day < ini) {
				                    situacao += "1";
				                }

				            } else if (frequenciaId == 6) { //MENSAL

				                // var day = new Date().getDate();
				                var day = currentCollectDate.getDate();

				                if (day >= (ini + (fim - ini) * 0.5) && day <= fim) {
				                    situacao += "3";
				                } else if (day >= ini && day <= fim) {
				                    situacao += "2";
				                } else if (day > fim) {
				                    situacao += "4";
				                } else if (day < ini) {
				                    situacao += "1";
				                }
				            }
				        }
				    }
				}
			)

            //verificar qual avaliação estou, e verificar todas a frente e pegar o pior caso para apresentar no semaforo

            //danger  = 4
            //warning = 3
            //success = 2
            //default = 1

            var elem = $('[data-par-cargo-id="' + cargo.Id + '"]');

            if (situacao.indexOf("4") >= 0) {
                $(elem).attr('style','background-color:red');
            } else if (situacao.indexOf("3") >= 0) {
                $(elem).attr('style', 'background-color:yellow');
            } else if (situacao.indexOf("2") >= 0) {
                $(elem).attr('style', 'background-color:green');
            } else if (situacao.indexOf("1") >= 0) {
                $(elem).attr('style', 'background-color:transparent');
            }
        }
    }, 200);
}