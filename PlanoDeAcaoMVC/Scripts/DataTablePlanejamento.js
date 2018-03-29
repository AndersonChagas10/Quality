
//var ColvisarrayVisaoAtual_show = [];
//var ColvisarrayVisaoAtual_hide = [];
var tablePlanejamento;
//var btnDetalhes = '<button type="button" class="details btn btn-default btn-sm" style="text-align: left; width:150px !important"><span title="Detalhes" style="cursor:pointer" class="glyphicon glyphicon-list-alt"></span>&nbsp Detalhes</button>';
//var btnNovoTatico = '<button type="button" class="btnNovoTatico showAsEstrategy btn btn-default btn-sm" style="text-align: left; width:150px !important"><span title="Novo Planejamento Tático para este Planejamento Estratégico" style="cursor:pointer" class="glyphicon glyphicon-tag"></span>&nbsp Novo Tático</button>';
//var btnNovoOperacional = '<button type="button" class="btnNovoOperacional btn btn-default btn-sm" style="text-align: left; width:150px !important"><span title="Novo Planejamento Operacional Vinculado ao Planejamento Tático e Estratégico" style="cursor:pointer" class="glyphicon glyphicon-tags"></span>&nbsp Nova Ação</button>';
//var btnAcompanhamento = '<button type="button" class="btnAcompanhamento btn btn-default btn-sm" style="text-align: left; width:150px !important"><span title="Acompanhamento" style="cursor:pointer" class="glyphicon glyphicon-book"></span>&nbsp Acompanhamento</button>';
//var btnEditarPlanejamento = '<button type="button" class="btnEditarPlanejamento btn btn-default btn-sm" style="text-align: left; width:150px !important"><span title="EditarPlanejamento" style="cursor:pointer" class="glyphicon glyphicon-book"></span>&nbsp Editar Planejamento</button>';
//var btnEditarPlanejamentoDisabled = '<button disabled type="button" class="btnEditarPlanejamento btn btn-default btn-sm" style="text-align: left; width:150px !important"><span title="EditarPlanejamento" style="cursor:pointer" class="glyphicon glyphicon-book"></span>&nbsp Editar Planejamento</button>';
var dadosPlanejamento = [];
//var ColvisarrayVisaoUsuario_show = [];
//var ColvisarrayVisaoUsuario_hide = [];

function GetDataTablePlanejamento(campo, filtro) {

    $.get(urlGetPlanejamentoRange, enviar, function (r) {

        //Recupera colunas visíveis do usuário -- Não está em funçao pois dava loop infinito (16/01/2018 - Renan)
        //if (getCookie('webControlCookie')) {

        //    Pa_Quem_Id = getCookie('webControlCookie')[0].split('=')[1];

        //    $.post(urlGetUserColvis, { "Pa_Quem_Id": Pa_Quem_Id }, function (r) {

        //        if (r.length > 0) {

        //            ColvisarrayVisaoAtual_show = objectToArr(r[0].ColVisShow.split(","));
        //            ColvisarrayVisaoAtual_hide = objectToArr(r[0].ColVisHide.split(","));

        //            if (ColvisarrayVisaoAtual_hide.length > 0) {
        //                ColvisarrayVisaoUsuario_show = ColvisarrayVisaoAtual_show;
        //                ColvisarrayVisaoUsuario_hide = ColvisarrayVisaoAtual_hide;
        //            }

        //        }

        //        //Monta a tabela
        //        MountDataTablePlanejamento(r);

        //    });
        //}

        MountDataTablePlanejamento(r);

        //$('#spanSubTable').text('TODAS AS TAREFAS PARA FILTRAR');

    });
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
            //{
            //    extend: 'colvisGroup',
            //    text: 'Planejamento Operacional',
            //    show: [3, 20/*, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40*/],
            //    hide: [0, 1, 2, 4, 5, 6, 7, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19]
            //},
            {
                extend: 'colvisGroup',
                text: 'Mostrar Todos',
                show: ':hidden'
            },
            //{
            //    extend: 'colvisGroup',
            //    text: 'Visão atual',
            //    //show: ColvisarrayVisaoAtual_show,
            //    //hide: ColvisarrayVisaoAtual_hide
            //    show: [],
            //    hide: []
            //},
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
            //{
            //    text: 'Minhas Colunas',
            //    extend: 'colvisGroup',
            //    //show: ColvisarrayVisaoUsuario_show,
            //    //hide: ColvisarrayVisaoUsuario_hide
            //    show: [],
            //    hide: []
            //},
            //{
            //    text: 'Salvar Colunas',
            //    action: function (e, dt, node, config) {
            //        SaveUserColVis();
            //    },
            //},
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

        //if (ColvisarrayVisaoAtual_hide.length != 0) {

        //    $('#TablePlanejamento_wrapper > div.dt-buttons > a:nth-child(5)').click();
        //} else {
        //    $('#TablePlanejamento_wrapper > div.dt-buttons > a:nth-child(1)').click();
        //}

        $(".dataTables_filter").css("display", "block");

        tablePlanejamento.draw();

    }, 1100);

    //$('#virtualBody').css('width', '100%');

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
    });

    tablePlanejamento.draw();

    if (ColvisarrayVisaoAtual_show.length == 0) {
        setArrayColvisAtual();

        setTimeout(function () {
            $('body > div.dt-button-background').click();
        }, 5);
    }

    //deixa escondido o botão que mantem as colunas atuais
    //$('#TablePlanejamento_wrapper > div.dt-buttons > a:nth-child(5)').hide();

    //clicar no botão escondido das colunas atuais
    //if (ColvisarrayVisaoAtual_show.length > 0) {
    //    $('#TablePlanejamento_wrapper > div.dt-buttons > a:nth-child(5)').click();
    //}

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

//$('table > tbody').on('click', '.btnNovoTatico', function (data, a, b) {
//    var data = tablePlanejamento.row($(this).parents('tr')).data();

//    Clicked(true, false, true);
//    $.get(urlGetPlanejamento, {
//        id: data.Id
//    }, function (r) {

//        ModalOpcoesEstrategico("Novo Planejamento Tático Vinculado", 0, function () {
//            EditarPlanejamento(r)
//        });
//    });

//});

//$('table > tbody').on('click', '.btnNovoOperacional', function (data, a, b) {
//    var data = tablePlanejamento.row($(this).parents('tr')).data();

//    planejamentoCorrentId = data.Tatico_Id;
//    Clicked(isTaticoClicked, isNovaAcao);

//    $('#modalLindo').modal();
//    $('#modalLindo').find('.modal-body').empty();
//    $('#Header').html("Planejamento Operacional");

//    $.get(PlanejamentoDetalhes, { id: planejamentoCorrentId }, function (r) {

//        $('#modalLindo').find('.modal-body').empty().append(r);
//        $('#NovaAcao').show();
//        $('#NovaAcao').click();

//    });

//});

//$('table > tbody').on('click', '.btnAcompanhamento', function (data, a, b) {

//    var data = tablePlanejamento.row($(this).parents('tr')).data();
//    selecionado = data;

//    acaoCorrentId = data.Acao.Id;

//    getAcompanhamento(acaoCorrentId);

//});


//$('table > tbody').on('click', '.btnEditarPlanejamento', function (data, a, b) {

//    $('#modalLindo').find('.modal-body').empty().append('<div class="content1"></div><div class="content2"></div><div class="content3"></div>');

//    var data = tablePlanejamento.row($(this).parents('tr')).data();

//    console.log(data);

//    if (data.Id > 0) {

//        isClickedEstrategico = true;

//        getPlanOp(data, a, b);

//    }

//    $('#modalLindo').find('.modal-footer button').hide();
//    $('#Header').html("Editar");
//    $('#modalLindo').modal();

//});

//function setArrayColvisAtual() {

//    if (tablePlanejamento) {

//        ColvisarrayVisaoAtual_show = [];
//        ColvisarrayVisaoAtual_hide = [];
//        var ss = [];

//        $('#TablePlanejamento_wrapper > div.dt-buttons > a.dt-button.buttons-collection.buttons-colvis').click();

//        $('body > div.dt-button-collection.fixed.four-column').hide();

//        ss = $('.buttons-columnVisibility');

//        ss.each(function (i, o) {

//            if ($(o).hasClass('active')) {

//                ColvisarrayVisaoAtual_show.push(i);

//            } else {

//                ColvisarrayVisaoAtual_hide.push(i);
//            }
//        }).promise().done(function () {

//            $('body > div.dt-button-background').click();

//        });
//    }
//}

//function SaveUserColVis() {

//    setArrayColvisAtual();

//    Pa_Quem_Id = getCookie('webControlCookie')[0].split('=')[1];

//    ColvisarrayVisaoAtual_show = $.grep(ColvisarrayVisaoAtual_show, function (arr) {
//        return (arr != 39 && arr != 40);
//    });

//    ColvisarrayVisaoAtual_hide = $.grep(ColvisarrayVisaoAtual_hide, function (arr) {
//        return (arr != 39 && arr != 40);
//    });

//    ColvisarrayVisaoAtual_show.push(39);
//    ColvisarrayVisaoAtual_show.push(40);

//    ColvisarrayVisaoAtual_hide

//    let objColvis = {
//        "ColVisShow": ColvisarrayVisaoAtual_show.toString(),
//        "ColVisHide": ColvisarrayVisaoAtual_hide.toString(),
//        "Pa_Quem_Id": Pa_Quem_Id
//    }


//    ColvisarrayVisaoUsuario_show = ColvisarrayVisaoAtual_show;
//    ColvisarrayVisaoUsuario_hide = ColvisarrayVisaoAtual_hide;

//    $.post(urlSaveUserColvis, objColvis, function (r) {

//        $('body > div.dt-button-background').click();
//        console.log(r);
//        if (r == "") {
//            openMessageModal("Colunas salvas!", "As Colunas foram salvas com sucesso!");
//        } else {
//            openMessageModal("Erro ao salvar!");
//        }
//    })

//}


