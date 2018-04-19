var tablePlanejamento;
var dadosPlanejamento = [];

//function GetDataTablePlanejamento(campo, filtro) {
//    //$.get(urlGetPlanejamentoRange, enviar, function (r) {
//    //    MountDataTablePlanejamento(r);
//    //});
//}

function GetDataTablePlanejamento(json) {

    dadosPlanejamento = [];

    var idsTaticos = [];

    json.forEach(function (r) {
        if (r.Tatico_Id != null && r.Tatico_Id != undefined)
            idsTaticos.push(r.Tatico_Id)
    })

    idsTaticos = idsTaticos.filter(onlyUnique);

    idsTaticos.forEach(function (o) {

        var Tatico

        var Planejamentos = $.grep(json, function (oo) {
            if (oo.Tatico_Id == o) { return oo };
        });

        var PlanejamentosComAcoes = $.grep(Planejamentos, function (oo) {
            if (oo.Acao.Id > 0) { return oo };
        });

        Tatico = $.extend({},Planejamentos[0]);

        Tatico["QtdeAcao"] = PlanejamentosComAcoes.length;

        Tatico.Acao = null;

        dadosPlanejamento.push(Tatico);

    });

    MountDataTablePlanejamento(dadosPlanejamento);
}

function objectToArr(myObj) {
    let array = $.map(myObj, function (value, index) {
        return [value];
    });

    return array
}

function MountDataTablePlanejamento(json) {

    if (ColvisarrayVisaoAtual_show.length != 0) {
        setArrayColvisAtual();

        setTimeout(function () {
            $('body > div.dt-button-background').click();
        }, 5);
    }

    tablePlanejamento = null;

    tablePlanejamento = $('#TablePlanejamento').DataTable({
        destroy: true,
        "aaData": json,
        "bAutoWidth": false,
        "aoColumns": [
            { "mData": "Diretoria" },
            { "mData": "Missao" },
            { "mData": "Visao" },
            { "mData": "Dimensao" },
            { "mData": "Objetivo" }, //ver diretriz
            { "mData": "IndicadoresDiretriz" },
            { "mData": "Responsavel_Diretriz_Quem.Name" },
            { "mData": "TemaAssunto" },
            { "mData": "Gerencia" },
            { "mData": "Coordenacao" },
            { "mData": "TipoProjeto" },
            { "mData": "TemaProjeto" },
            { "mData": "Iniciativa" },
            { "mData": "IndicadoresDeProjeto" },
            { "mData": "ObjetivoGerencial" },
            { "mData": "_ValorDe" },
            { "mData": "_ValorPara" },
            { "mData": "_DataInicio" },
            { "mData": "_DataFim" },
            { "mData": "Responsavel_Projeto_Quem.Name" }, 
            { "mData": "IndicadoresDeProjeto" }, // VER INDICADOR OPERACIONAL*
            { "mData": "QtdeAcao" },
            {
                "mData": null,
                "render": function (data, type, row, meta) {
                    var html = "";

                    if (!!(parseInt(data.Id) && parseInt(data.Id) > 0 || parseInt(data.Tatico_Id) && parseInt(data.Tatico_Id))) {
                        if (!IsAdmin && !parseInt(data.Tatico_Id) && !parseInt(data.Tatico_Id) > 0) {
                            html += "<br>" + btnEditarPlanejamentoDisabled;
                        } else {
                            html += "<br>" + btnEditarPlanejamento;
                        }
                    }

                    if (!!parseInt(data.Id) && parseInt(data.Id) > 0) // Possui plan Estrat
                        html += btnNovoTatico;

                    if (!!parseInt(data.Tatico_Id) && parseInt(data.Tatico_Id) > 0)  // possui plan tatico
                        html += "<br class='showAsEstrategy'>" + btnNovoOperacional;

                    return html;
                }
            }
        ],
        'aoColumnDefs': [
            { "sTitle": "Diretoria", "aTargets": [0], "width": "100px" },
            { "sTitle": "Missão", "aTargets": [1], "width": "200px" },
            { "sTitle": "Visão", "aTargets": [2], "width": "200px" },
            { "sTitle": "Dimensão", "aTargets": [3], "width": "50px" },
            { "sTitle": "Diretrizes", "aTargets": [4], "width": "200px" }, // ver diretriz
            { "sTitle": "Indicadores Diretriz", "aTargets": [5], "width": "300px" },
            { "sTitle": "Responsável pela Diretriz", "aTargets": [6], "width": "50px" },
            { "sTitle": "Tema | Assunto", "aTargets": [7], "width": "100px" },
            { "sTitle": "Gerência", "aTargets": [8], "width": "100px" },
            { "sTitle": "Coordenação", "aTargets": [9], "width": "100px" },
            { "sTitle": "Tipo de Projeto", "aTargets": [10], "width": "100px" },
            { "sTitle": "Tema do Projeto", "aTargets": [11], "width": "100px" },
            { "sTitle": "Projeto | Iniciativa", "aTargets": [12], "width": "200px" },
            { "sTitle": "Indicadores de Projeto/Iniciativa", "aTargets": [13], "width": "100px" },
            { "sTitle": "Objetivo Gerencial", "aTargets": [14], "width": "100px" },
            { "sTitle": "Valor de", "aTargets": [15], "width": "50px" },
            { "sTitle": "Valor para", "aTargets": [16], "width": "50px" },
            { "sTitle": "Data Início", "aTargets": [17], "width": "50px" },
            { "sTitle": "Data Fim", "aTargets": [18], "width": "50px" },
            { "sTitle": "Responsável pelo Projeto/Iniciativa", "aTargets": [19], "width": "50px" },//
            { "sTitle": "Indicadores Operacional", "aTargets": [20], "width": "100px" }, // ver indicador operacional*
            { "sTitle": "Quantidade de Ações", "aTargets": [21], "width": "50px" },
            { "sTitle": "Ação" },

        ],
        "responsive": true,
        "bSearchable": true,
        "bFilter": true,
        "paging": true,
        "lengthMenu": [[10, 20, 50, -1], [10, 20, 50, "Todos"]],
        "info": true,
        "scrollY": 370,
        "scrollX": 500,
        "bLengthChange": true,
        "dom": 'Blfrtip',
        "buttons": [
            {
                extend: 'colvisGroup',
                text: 'Visão Inicial',
                show: [4, 7, 8, 12, 13, 14, 15, 16, 17, 18, 19/*, 39, 40*/],
                hide: [0, 1, 2, 3, 5, 6, 9, 10, 11, 20/*, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38*/]
            },
            {
                extend: 'colvisGroup',
                text: 'Planejamento Estratégico',
                show: [0, 1, 2, 3, 4, 5, 6/*, 35, 39, 40*/],
                hide: [7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20/*, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 36, 37, 38*/]
            },
            {
                extend: 'colvisGroup',
                text: 'Planejamento Tático',
                show: [7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19/*, 39, 40*/],
                hide: [0, 1, 2, 4, 5, 6, 20/*, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38*/]
            },
            {
                extend: 'colvisGroup',
                text: 'Mostrar Todos',
                show: ':hidden'
            },
            {
                extend: 'excel',
                text: 'Excel',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'colvis',
                text: 'Colunas Visíveis',
                collectionLayout: 'fixed four-column',
                exportOptions: {
                    columns: ':visible',
                }
            },
            {
                text: 'Atualizar',
                action: function (e, dt, node, config) {
                    $('#btnTop').click();
                },
            },
        ],
        fixedColumns: {
            leftColumns: 0,
            rightColumns: 2,
        },
        initComplete: function () {
            //$('#TablePlanejamento_wrapper > div.dt-buttons > a.dt-button.buttons-collection.buttons-colvis').on("click", function () {
            //    $('body > div.dt-button-collection.fixed.four-column > a:nth-child(40)').hide();
            //    $('body > div.dt-button-collection.fixed.four-column > a:nth-child(41)').hide();
            //});

        },
        "language": {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_ resultados por página",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar",
            "oPaginate": {
                "sNext": "Próximo",
                "sPrevious": "Anterior",
                "sFirst": "Primeiro",
                "sLast": "Último"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        }
    });

    setTimeout(function () {

        $(".dataTables_filter").css("display", "block");

        tablePlanejamento.draw();

    }, 1100);

    //Filtros por coluna
    $('#TablePlanejamento_wrapper .dataTable:not(.DTFC_Cloned) thead th').each(function (i) {
        var title = $('#TablePlanejamento_wrapper .dataTable:not(.DTFC_Cloned) thead th').eq($(this).index()).text();
        $(this).html(title + '<br><input type="text" style="font-size:xx-small; color: #555; text-align:center; width:50px" placeholder=" ' + title + '" data-index="' + i + '" />');
    });

    $('.dataTable thead th').css('text-align', 'center');

    // Filter event handler
    $(tablePlanejamento.table().container()).on('keyup', 'thead input', function () {
        tablePlanejamento
            .column($(this).data('index'))
            .search(this.value)
            .draw();

        GetFiltrosDeColunasTablePlanejamento();
    });

    SetFiltrosDeColunasTablePlanejamento();

    tablePlanejamento.draw();

    if (ColvisarrayVisaoAtual_show.length == 0) {
        setArrayColvisAtual();

        setTimeout(function () {
            $('body > div.dt-button-background').click();
        }, 5);
    }

    $('#TablePlanejamento_wrapper > div.DTFC_ScrollWrapper > div.DTFC_RightWrapper > div.DTFC_RightHeadWrapper > table > thead > tr > th:nth-child(2) > input[type="text"]').hide();

}

$('#divPlanejamento table > tbody').on('click', '.btnNovoTatico', function () {

    let data = tablePlanejamento.row($(this).parents('tr')).data();

    Clicked(true, false, true);

    $.get(urlGetPlanejamento, { id: data.Id }, function (r) {

        ModalOpcoesEstrategico("Novo Planejamento Tático Vinculado", 0, function () {

            EditarPlanejamento(r);

        });
    });

});

$('#divPlanejamento table > tbody').on('click', '.btnNovoOperacional', function () {

    let data = tablePlanejamento.row($(this).parents('tr')).data();

    planejamentoCorrentId = data.Tatico_Id;
    Clicked(isTaticoClicked, isNovaAcao);

    $('#modalLindo').modal();
    $('#modalLindo').find('.modal-body').empty();
    $('#Header').html("Planejamento Operacional");

    $.get(PlanejamentoDetalhes, { id: planejamentoCorrentId }, function (r) {

        $('#modalLindo').find('.modal-body').empty().append(r);
        $('#NovaAcao').show();
        $('#NovaAcao').click();

    });
});

$('#divPlanejamento table > tbody').on('click', '.btnAcompanhamento', function () {

    let data = tablePlanejamento.row($(this).parents('tr')).data();

    selecionado = data;

    acaoCorrentId = data.Acao.Id;

    getAcompanhamento(acaoCorrentId);

});

$('#divPlanejamento table > tbody').on('click', '.btnEditarPlanejamento', function () {

    $('#modalLindo').find('.modal-body').empty().append('<div class="content1"></div><div class="content2"></div><div class="content3"></div>');

    let data = tablePlanejamento.row($(this).parents('tr')).data();

    if (data.Id > 0) {

        isClickedEstrategico = true;

        getPlanOp(data);
    }

    $('#modalLindo').find('.modal-footer button').hide();
    $('#Header').html("Editar");
    $('#modalLindo').modal();
    $('#Fechar').show();

});

var filtrosDeColunasTablePlanejamento = [];

function GetFiltrosDeColunasTablePlanejamento() {

    filtrosDeColunasTablePlanejamento = [];

    $('#TablePlanejamento_wrapper > div.DTFC_ScrollWrapper > div.dataTables_scroll > div.dataTables_scrollHead > div > table > thead > tr th input[type="text"]').each(function (a) {
        if ($(this).val() != "") {
            filtrosDeColunasTablePlanejamento.push({ Key: $(this).parent().text(), Val: $(this).val() });
        }
    });
}

function SetFiltrosDeColunasTablePlanejamento() {

    if (filtrosDeColunasTablePlanejamento.length > 0) {

        filtrosDeColunasTablePlanejamento.forEach(function (o, c) {

            $('#TablePlanejamento_wrapper > div.DTFC_ScrollWrapper > div.dataTables_scroll > div.dataTables_scrollHead > div > table > thead > tr th input').each(function (a) {

                if ($(this).parent().text() == o.Key) {
                    $(this).val(o.Val);
                    tablePlanejamento.column(a).search(o.Val).draw();
                }
            });
        });
    }
}
