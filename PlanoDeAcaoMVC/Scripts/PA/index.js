
var dados = [];
var dadosfilter = [];
var dadosPie2 = [];

//Cores dos Status
var atrasadaColor = '#FF0000';
var concluidoColor = '#0000FF';
var andamentoColor = '#008000';
var concluidoAtrasoColor = '#FFA500';
var canceladoColor = '#000000';
var retornoColor = '#8B4513';
var finalizadaColor = '#00008B';
var finalizadaComAtrasoColor = '#FF4500';
var naoIniciadoColor = '#E0EEEE';

var dataInicio;
var dataFim;
var categories2 = ['Camila', 'Miriã', 'Ana', 'Adão', 'Diego'];
var categories4 = [];
var categories5 = [];
var data5 = [];
var categories6 = [];
var data6 = [];
var enviar = {};
var start = moment();
var end = moment();
var btnOrderFilter = "";
var option = "";
var campo1Panel5Selected = "";
var filtrosDeColunas = [];

var tableModalNovoApartirDe = {};

var counter = 0;
var planejamentoCorrentId = 0;
var isTaticoClicked = false;
var isNovaAcao = false;
var isClickedTaticoVinculado = false;
var isClickedEstrategico = false;
var NovaAcaoBuscaFeita = false;
var tempInput;
var tempName;
var IdBotaoClicado;

var selectPredecessor;
var tableModalNovo;

var Chaveamento = [];
var ChaveamentoObjetivo = [];

var json = FiltraColunas(dados, [
    Resources("directorship"),
    Resources("mission"),
    Resources("view"),
    Resources("dimension"),
    Resources("guidelines"),
    Resources("indicators_guidelines"),
    Resources("responsible_guideline"),
    Resources("management"),
    Resources("coordination"),
    Resources("initiative"),
    Resources("project_initiative"),
    Resources("management_objective"),
    Resources("value_of"),
    Resources("value_for"),
    Resources("start_date"),
    Resources("end_date"),
    Resources("responsible_project_initiative"),
    Resources("regional"),
    Resources("unit"),
    Resources("indicator"),
    Resources("monitoring"),
    Resources("task"),
    Resources("indicators_project_initiative"),
    Resources("generic_cause"),
    Resources("group_cause"),
    Resources("generic_action"),
    Resources("specific_action"),
    Resources("specific_cause2"),
    Resources("who"),
    Resources("when_start"),
    Resources("when_end"),
    Resources("theme_subject"),
    Resources("for_what"),
    Resources("how_much"),
    Resources("status"),
    Resources("term")
]);

var data2 = [{
    name: Resources("completed"),
    data: [5, 3, 4, 7, 2],
    color: concluidoColor
}, {
    name: Resources("late"),
    data: [2, 2, 3, 2, 1],
    color: atrasadaColor
}, {
    name: Resources("in_progress"),
    data: [3, 4, 4, 2, 5],
    color: andamentoColor
}];

var data3 = [{
    id: 'A',
    name: Resources("late"),
    color: atrasadaColor
}, {
    id: 'B',
    name: Resources("completed"),
    color: concluidoColor
}, {
    id: 'O',
    name: Resources("in_progress"),
    color: andamentoColor
}, {
    name: Resources("quality"),
    parent: 'A',
    value: 5
}, {
    name: Resources("operation"),
    parent: 'A',
    value: 3
}, {
    name: Resources("quality"),
    parent: 'B',
    value: 4
}, {
    name: Resources("operation"),
    parent: 'B',
    value: 10
}, {
    name: Resources("quality"),
    parent: 'O',
    value: 1
}, {
    name: Resources("operation"),
    parent: 'O',
    value: 3
}];

var data4 = [{
    type: 'column',
    name: Resources("open"),
    data: [],
    color: andamentoColor
}, {
    type: 'column',
    name: Resources("closed"),
    data: [],
    color: concluidoColor
}, {
    type: 'spline',
    name: Resources("stock"),
    data: [],
    color: atrasadaColor,
    marker: {
        lineWidth: 2,
        lineColor: atrasadaColor,
        fillColor: atrasadaColor
    }
}];

$('#divPlanejamentoAcao table > tbody').on('click', '.btnNovoTatico', function (data, a, b) {

    var data = table.row($(this).parents('tr')).data();

    Clicked(true, false, true);

    $.get(urlGetPlanejamento, { id: data.Id }, function (r) {

        ModalOpcoesEstrategico(Resources("new_tactical_planning_linked"), 0, function () {
            EditarPlanejamento(r)
        });

    });

});

$('#divPlanejamentoAcao table > tbody').on('click', '.btnNovoOperacional', function (data, a, b) {

    var data = table.row($(this).parents('tr')).data();

    planejamentoCorrentId = data.Tatico_Id;

    Clicked(isTaticoClicked, isNovaAcao);

    $('#modalLindo').modal();
    $('#modalLindo').find('.modal-body').empty();
    $('#Header').html(Resources("operational_planning"));

    $.get(PlanejamentoDetalhes, { id: planejamentoCorrentId }, function (r) {

        $('#NovaAcao').show();
        $('#NovaAcao').click();
        $('#modalLindo').find('.modal-body').empty().prepend(r);

        InitDatePiker();

    });

});

$('#divPlanejamentoAcao table > tbody').on('click', '.btnAcompanhamento', function (data, a, b) {

    var data = table.row($(this).parents('tr')).data();

    selecionado = data;

    acaoCorrentId = data.Acao.Id;

    getAcompanhamento(acaoCorrentId);

});

$('#divPlanejamentoAcao table > tbody').on('click', '.btnEditarPlanejamento', function (data, a, b) {

    $('#modalLindo').find('.modal-body').empty().append('<div class="content1"></div><div class="content2"></div><div class="content3"></div>');

    var data = table.row($(this).parents('tr')).data();

    if (data.Id > 0) {

        isClickedEstrategico = true;

        getPlanOp(data, a, b);

    }

    $('#modalLindo').find('.modal-footer button').hide();
    $('#Header').html(Resources("edit"));
    $('#modalLindo').modal();
    $('#Fechar').show();
});

//Criar arrays para graficos Panel5 e 6, os filtros devem ser inseridos antes do makechart
function filtraDadosParaGerarGraficoPanel5Panel6(categoriesFilterVal, seriesFilterVal, order, id, optionsDef) {

    let hasFilter = categoriesFilterVal && seriesFilterVal;

    if (hasFilter) {

        var categoriesArr = [];

        if (id == 'panel5') {

            if ($('#valor1Panel5 option:selected').text() == Resources("all"))
                categoriesArr = MapeiaValorParaHC(dados, categoriesFilterVal).filter(onlyUnique);

            else
                categoriesArr.push($('#valor1Panel5 option:selected').text());

        } else if (id == 'panel6') {

            if ($('#valor1Panel6 option:selected').text() == Resources("all"))
                categoriesArr = MapeiaValorParaHC(dados, categoriesFilterVal).filter(onlyUnique);

            else
                categoriesArr.push($('#valor1Panel6 option:selected').text());
        }

        var categoriesArrOrder = [];

        categoriesArr.sort();

        var serieArrFinal = filtraAgrupaXY(categoriesArr, seriesFilterVal, categoriesFilterVal, dados, true, id);

        if (order == "Asc") {
            //segue o fluxo!

        } else {

            //ordenarção por quantidade de ações
            var soma = [];
            jQuery.each(serieArrFinal, function (i, val) {

                for (j = 0; j < serieArrFinal[i].data.length; j++) {
                    if (i == 0) {
                        soma.push([]);
                    }
                    if (soma[j][0] == undefined) {
                        soma[j][0] = 0;
                    }
                    soma[j][0] = soma[j][0] + serieArrFinal[i].data[j];
                }
            });
            jQuery.each(soma, function (i, val) {
                soma[i][1] = i;
            });
            btnOrder
            soma.sort(sortFunction);
            soma.reverse();

            var oldSerieArrFinal = JSON.parse(JSON.stringify(serieArrFinal));
            var oldCategoriesArr = JSON.parse(JSON.stringify(categoriesArr));

            jQuery.each(oldSerieArrFinal, function (i, val) {
                for (j = 0; j < soma.length; j++) {
                    serieArrFinal[i].data[j] = (oldSerieArrFinal[i].data[soma[j][1]]);
                }
            });

            jQuery.each(soma, function (i, val) {
                categoriesArr[i] = oldCategoriesArr[soma[i][1]].slice();
            });
            //fim ordenarção por quantidade de ações
        }

        makeChart(id, categoriesArr, serieArrFinal, 'bar', categoriesFilterVal);

    } else
        makeChart(id, categoriesArr, serieArrFinal, 'bar', categoriesFilterVal);
}

//Agrupa o arr dados por mes, devolve um Objeto, se mapped = true devolve um array
function agrupaPorMes() {
    var groups = _.groupBy(dados, function (o) {
        return moment(o.Acao.Acompanhamento.AddDate).startOf('mounth').format();
    });
    return groups
}

//Busca no arr de dados registros concluídos, filtro = return
function getRegistrosConcluidos(arr) {
    return _.filter(arr, function (o) {
        return o.Acao._StatusName && (o.Acao._StatusName.indexOf(Resources('completed')) >= 0 || o.Acao._StatusName.indexOf(Resources('finished'))) >= 0;
    });
}

//Busca no arr de dados registros não concluídos, filtro = return
function getRegistrosNaoConcluidos(arr) {
    return _.filter(arr, function (o) {
        return !(o.Acao._StatusName && (o.Acao._StatusName.indexOf(Resources('completed')) >= 0 || o.Acao._StatusName.indexOf(Resources('finished'))) >= 0 && o.Acao._StatusName.indexOf(Resources('cancel')) < 0);
    });
}

function DisabilitaBotaoGerenciar() {
    $('#modalLindo').find('select[disabled]').each(function (i, o) {
        $(o).parents('tr').find('.novoItem').attr('disabled', true);
    });
}

function Clicked(isClickedTatico, isClickedAcao, isTaticoVinculado, clearId) {
    isTaticoClicked = isClickedTatico;
    isNovaAcao = isClickedAcao;
    isClickedTaticoVinculado = isTaticoVinculado;
}

/*Novo item*/
function Novo(btn, encadeado) {

    selectPredecessor = encadeado;

    //Remove select caso exista
    $('#encadeado').find('select').remove();

    if (encadeado == undefined)
        $('#encadeado').hide()

    tempInput = $(btn).parents('tr').find('input , select');
    tempName = $(btn).parents('tr').find('input , select').attr('name');

    var valorSelectSelecionado = $(btn).parents('tr').find('input , select option:selected').html();
    var tempIdNovo = $(btn).parents('tr').find('input , select option:selected').val();

    let isDiretriz = (tempName == "Objetivo_Id");
    let isPriority = false;
    let Objetivo_Id = parseInt($(btn).parents('tr').find('input , select').val());

    //pegar se é true ou false
    if (isDiretriz && valorSelectSelecionado != "" && Objetivo_Id) {

        $.post(urlGetDiretriz + '/' + Objetivo_Id, function (o) {

            isPriority = o.IsPriority;

            SetaValoresModal(tempIdNovo, valorSelectSelecionado, true, isDiretriz, isPriority);

            var label = $(btn).parents('tr').find('label').html();
            $('#modalNovo > div > div.modal-footer').hide();
            $('#modalNovo').modal();
            $('#novoHeader').html(Resources('to_manage') + " " + label);
            $('#nomeItemNovo').html(label);

            if (encadeado != undefined) {
                $('#labelPredecessora').children().remove()
                $('#labelPredecessora').append($('#' + encadeado).clone().removeClass('select2-hidden-accessible'));
                $('#labelPredecessora').find('select').val($('#' + encadeado + ' option:selected').val()).change();
            }

        });

    } else {

        SetaValoresModal(tempIdNovo, valorSelectSelecionado, true, isDiretriz, isPriority);

        var label = $(btn).parents('tr').find('label').html();
        $('#modalNovo > div > div.modal-footer').hide();
        $('#modalNovo').modal();
        $('#novoHeader').html(Resources('to_manage') + " " + label);
        $('#nomeItemNovo').html(label);

        if (encadeado != undefined) {
            $('#labelPredecessora').children().remove()
            $('#labelPredecessora').append($('#' + encadeado).clone().removeClass('select2-hidden-accessible'));
            $('#labelPredecessora').find('select').val($('#' + encadeado + ' option:selected').val()).change();
        }
    }
}

function SalvarNovo() {

    var params = {
        NomeDoItem: $('#newItem').val(),
        ParametroDeBusca: tempName.split('_Id')[0],
        PredecessorId: $('#encadeado').find('select').val(),
        Id: $('#newIdItem').val(),
        IsActive: $('#newIsActiveItem:checked').length == 1,
        IsPriority: $('#isPriority:visible').prop('checked') == undefined ? null : $('#isPriority:visible').prop('checked')
    }

    $.post(urlSalvarNovoItem, params, function (r) {
        if (r.Resposta != null) {
            alert(r.Resposta);
            return;
        }

        var jaExisteoptionNoDropDown = $(tempInput).find('option[value="' + r.Id + '"]').length > 0
        if (jaExisteoptionNoDropDown)
            if (r.IsActive) {
                $(tempInput).find("option[value= " + r.Id + "]").html(r.NomeDoItem);
                $(tempInput).find("option[value= " + r.Id + "]").prop('selected', true);
                $(tempInput).select2('destroy');
                $(tempInput).val(r.Id).trigger('change');
                $(tempInput).select2(configSelect2);

            } else {
                $(tempInput).find('option:first').prop('selected', true);
                $(tempInput).find("option[value= " + r.Id + "]").remove();
                $(tempInput).trigger('change');
            }
        else {
            $(tempInput).find('option:selected').prop('selected', false);
            $(tempInput).append('<option value=' + r.Id + ' selected> ' + r.NomeDoItem + '</option>');
            $(tempInput).find("option[value= " + r.Id + "]").prop('selected', true);
            $(tempInput).trigger('change')
        }
        refreshDataTable();
        $('#btnCancelarNovo').click();
    });

}
/*Fim Novo item*/

var refreshDataTable = function () {
    /*Dados*/
    var param1 = $('#newItem').val();
    var param2 = tempName.split('_Id')[0];
    var param3 = 0;
    if ($('#encadeado').find('select').length) {
        param3 = $('#encadeado').find('select').val();
    }
    $('#labelPredecessoraValue').text($('#encadeado').find('select option:selected').text());

    if (param3 != 0) {
        $('#divPredecessora').removeClass('hide');
    } else {
        $('#divPredecessora').addClass('hide');
    }

    /*Ajax*/
    $.post(urlGetAutoComplete, { NomeDoItem: param1, ParametroDeBusca: param2, PredecessorId: param3 }, function (response) {

        $("#tableNovo").html('');

        if (tableModalNovo) {
            tableModalNovo.destroy();
            $('#tableNovo').empty();
        }

        //Gera a lista
        var list = [];
        $.each(response, function (index, value) {
            list.push([value.Id, value.Name, value.IsActive, value.IsPriority])
        });

        let columns = [];

        if (list.length > 0 && list[0][3] != null) {
            columns = [
                { title: "ID" },
                { title: Resources('name') },
                {
                    title: Resources('active2'),
                    "data": null,
                    "render": function (data, type, full, meta) {
                        if (data[2] == true) {
                            return Resources('active3');
                        }
                        return Resources('inactive');
                    }
                },
                {
                    title: "Prioritário",
                    "data": null,
                    "render": function (data) {
                        if (data[3] == true) {
                            return "SIM";
                        }
                        return "NÃO";
                    }
                },
                {
                    "className": 'options',
                    "data": null,
                    "render": function (data, type, full, meta) {
                        return '<span class="btn btn-mini btn-info pull-right" data-id=' + data.Id + ' data-name=' + data.Name + '>' + Resources('edit') + '</span>';
                    }
                }
            ];
        } else {
            columns = [
                { title: "ID" },
                { title: Resources('name') },
                {
                    title: Resources('active2'),
                    "data": null,
                    "render": function (data, type, full, meta) {
                        if (data[2] == true) {
                            return Resources('active3');
                        }
                        return Resources('inactive');
                    }
                },
                {
                    "className": 'options',
                    "data": null,
                    "render": function (data, type, full, meta) {
                        return '<span class="btn btn-mini btn-info pull-right" data-id=' + data.Id + ' data-name=' + data.Name + '>' + Resources('edit') + '</span>';
                    }
                }
            ]
        }

        tableModalNovo = $('#tableNovo').DataTable({
            data: list,
            destroy: true,
            lengthChange: false,
            pageLength: 3,
            "bInfo": false,
            language: {
                "sEmptyTable": "" + Resources('no_records_found') + "",
                "sInfo": "" + Resources("showing_start_to_end_of_total_records") + "",
                "sInfoEmpty": "" + Resources('showing_0_to_0_of_0_records') + "",
                "sInfoFiltered": "" + Resources('filtered_max_records') + "",
                "sInfoPostFix": "",
                "sInfoThousands": ".",
                "sLengthMenu": "" + Resources('menu_results_per_page') + "",
                "sLoadingRecords": "" + Resources('loading') + "",
                "sProcessing": "" + Resources('processing') + "",
                "sZeroRecords": "" + Resources('no_records_found') + "",
                "sSearch": "" + Resources('search') + "",
                "oPaginate": {
                    "sNext": "" + Resources('next') + "",
                    "sPrevious": "" + Resources('back') + "",
                    "sFirst": "" + Resources('first') + "",
                    "sLast": "" + Resources('last') + ""
                },
                "oAria": {
                    "sSortAscending": "" + Resources('sort_columns_ascending') + "",
                    "sSortDescending": "" + Resources('sort_columns_downward') + ""
                }
            },
            columns: columns,
            initComplete: function () { }
        });

        $('#tableNovo tbody').on('click', 'span', function () {

            SetaValoresModal(
                tableModalNovo.row($(this).parents('tr')).data()[0],
                tableModalNovo.row($(this).parents('tr')).data()[1],
                tableModalNovo.row($(this).parents('tr')).data()[2],
                (tableModalNovo.row($(this).parents('tr')).data()[3] != null && tableModalNovo.row($(this).parents('tr')).data()[3] != undefined),
                tableModalNovo.row($(this).parents('tr')).data()[3]);
        });
    })
};

function Relatorios() {
    win.focus();
}

function processarChaveamento() {

    if (Chaveamento.length > 0) {
        return;
    }

    $.each(dados, function (index, obj) {

        var existe = retornaChaveamentoPorGerenciaId(obj.Gerencia_Id);

        if (existe.length <= 0) {
            Chaveamento.push({ color: randomColor(), gerencia_id: obj.Gerencia_Id });
        }
    });
}

function processarChaveamentoObjetivo() {

    if (ChaveamentoObjetivo.length > 0) {
        return;
    }

    $.each(dados, function (index, obj) {

        var existe = retornaChaveamentoPorObjetivo(obj.Objetivo_Id);

        if (existe.length <= 0) {
            ChaveamentoObjetivo.push({ color: randomColor(), objetivo_id: obj.Objetivo_Id });
        }
    });
}

function retornaChaveamentoPorGerenciaId(gerencia_id) {
    return $.grep(Chaveamento, function (o, i) {
        return o.gerencia_id == gerencia_id;
    });
}

function retornaChaveamentoPorObjetivo(objetivo_id) {
    return $.grep(ChaveamentoObjetivo, function (o, i) {
        return o.objetivo_id == objetivo_id;
    });
}

// CRIAR TATICO E ACAO A PARTIR DE SELECOES PAIS
function AbrirModalNovoApartirDeAlgo(tipoQueSeraCriado, NomeDoQueSeraCriado, NomeApartirDeQueSeraCriado) {

    $('#ModalNovoApartirDeAlgo').find('h4').text(Resources('create') + " " + NomeDoQueSeraCriado + " " + Resources('from') + " " + NomeApartirDeQueSeraCriado);

    $.post(urlGetListPlanejamento + '/' + tipoQueSeraCriado, function (r) {

        if (exibindoAtivos) {
            r.datas = r.datas.filter(x => x.IsActive === "True");
        } else {
            r.datas = r.datas.filter(x => x.IsActive === "False");
        }

        var col = r.columns;

        col.unshift({
            "render": function (data, type, row) {
                return '<button data-id="' + row.Id + '" data-tipo="' + tipoQueSeraCriado + '" class="btn btn-large btn-primary">' + Resources('create') + NomeDoQueSeraCriado + '</button>';
            }
        });

        tableModalNovoApartirDe = datatableGRT.Inicializar({

            idTabela: "ModalNovoApartirDeAlgo table",
            listaDeDados: r.datas,
            colunaDosDados: r.columns,
            buttons: { buttons: [] },
            initComplete: function () {

                $('#ModalNovoApartirDeAlgo table').off().on('click', 'button[data-id]', function () {

                    if ($(this).attr('data-tipo') == 'tatico') {

                        AbreModalDePlanejamento($(this).attr('data-id'));

                    } else if ($(this).attr('data-tipo') == 'acao') {

                        planejamentoCorrentId = $(this).attr('data-id');
                        AbreModalDeAcao(planejamentoCorrentId);
                    }

                    $('#ModalNovoApartirDeAlgo').modal('hide');

                });

                $('#ModalNovoApartirDeAlgo').modal('show');

                setTimeout(function (e) {
                    tableModalNovoApartirDe.draw();
                }, 500);
            }
        });
    });
}

$(function () {

    GetDataTable();

    $('#modalNovo').on('hidden.bs.modal', function () {
        if (selectPredecessor != undefined) {
        } else {
            $('#' + IdBotaoClicado).click();
        }

    })

    $('#modalNovo').on('shown.bs.modal', function (e) {

        $('#btnCancelarNovo').on('click', function () {
            $("#newIdItem").val('');
            $("#newItem").val('');
            $('#btnSalvarNovo').removeClass('hide');
            $('#btnEditarNovo').addClass('hide');
            $('#btnCancelarNovo').addClass('hide');
            $('#divIsActive').addClass('hide');

        });

        refreshDataTable();

    });
});

$(document).ready(function () {

    var zero = 0;

    var url = new URL(window.location.href);
    var parametroUrlId = url.searchParams.get("Acao");

    if (parametroUrlId != null) {
        setTimeout(function () {
            getAcompanhamento(parametroUrlId)
        }, 100)
    }

    $("input[name='daterange']").css('width', '140px');
    $("input[name='daterange']").css('height', '25px');

    $('#campo1Filtro').css('width', '200px');
    $('#valor1Filtro').css('width', '200px');

    if (IsAdmin) {
        $('#NovoPlanejamento').prop("disabled", false);
    }

});