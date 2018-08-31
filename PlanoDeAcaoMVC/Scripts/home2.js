//var urlGetPlanejamentoAcaoRange = 'http://192.168.25.200/PlanoAcao/api/Pa_Planejamento/GetPlanejamentoAcaoRange';
//var urlGetPlanejamentoAcaoRange = 'http://mtzsvmqsc/PlanoDeAcao/api/Pa_Planejamento/GetPlanejamentoAcaoRange';
//var urlGetPlanejamentoAcaoRange = 'http://localhost:59907/api/Pa_Planejamento/GetPlanejamentoAcaoRange';

var table;
var btnDetalhes = '<button type="button" class="details btn btn-default btn-sm" style="text-align: left; width:150px !important"><span title="Detalhes" style="cursor:pointer" class="glyphicon glyphicon-list-alt"></span>&nbsp' + Resources('details') + '</button> ';
var btnNovoTatico = '<button type="button" class="btnNovoTatico showAsEstrategy btn btn-default btn-sm" style="text-align: left; width:150px !important"><span title="Novo Planejamento Tático para este Planejamento Estratégico" style="cursor:pointer" class="glyphicon glyphicon-tag"></span>&nbsp' + Resources('new_tactic') + '</button>';
var btnNovoOperacional = '<button type="button" class="btnNovoOperacional btn btn-default btn-sm" style="text-align: left; width:150px !important"><span title="Novo Planejamento Operacional Vinculado ao Planejamento Tático e Estratégico" style="cursor:pointer" class="glyphicon glyphicon-tags"></span>&nbsp' + Resources('new_action') + '</button> ';
var btnAcompanhamento = '<button type="button" class="btnAcompanhamento btn btn-default btn-sm" style="text-align: left; width:150px !important"><span title="Acompanhamento" style="cursor:pointer" class="glyphicon glyphicon-book"></span>&nbsp' + Resources('accompaniment') + '</button> ';
var btnEditarPlanejamento = '<button type="button" class="btnEditarPlanejamento btn btn-default btn-sm" style="text-align: left; width:150px !important"><span title="EditarPlanejamento" style="cursor:pointer" class="glyphicon glyphicon-book"></span>&nbsp' + Resources('edit_planning') + '</button> ';
var btnEditarPlanejamentoDisabled = '<button disabled type="button" class="btnEditarPlanejamento btn btn-default btn-sm" style="text-align: left; width:150px !important"><span title="EditarPlanejamento" style="cursor:pointer" class="glyphicon glyphicon-book"></span>&nbsp' + Resources('edit_planning') + '</button>';
var dados = [];
var dadosfilter = [];
var dadosPie2 = [];

//Colunas Visiveis do Usuário na Tabela de Ações
var ColvisarrayVisaoUsuario_show = [];
var ColvisarrayVisaoUsuario_hide = [];
var ColvisarrayVisaoAtual_show = [];
var ColvisarrayVisaoAtual_hide = [];

//Colunas Visiveis do Usuário na Tabela de Projetos
var ColvisarrayProjVisaoUsuario_show = [];
var ColvisarrayProjVisaoUsuario_hide = [];
var ColvisarrayProjVisaoAtual_show = [];
var ColvisarrayProjVisaoAtual_hide = [];

//Cores dos Status
var atrasadaColor = '#FF0000';
var concluidoColor = '#0000FF';
var andamentoColor = '#008000';
var concluidoAtrasoColor = '#FFA500'
var canceladoColor = '#000000'
var retornoColor = '#8B4513'
var finalizadaColor = '#00008B'
var finalizadaComAtrasoColor = '#FF4500'
var naoIniciadoColor = '#E0EEEE'

var dataInicio;
var dataFim;

var categories2 = ['Camila', 'Miriã', 'Ana', 'Adão', 'Diego'];

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

var categories4 = [];

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

var categories5 = []; //['Qualidade', 'Operação', 'Industria'];

var data5 = [];

var categories6 = []; //categories2

var data6 = [];

var json = FiltraColunas(dados, [Resources("directorship"),
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
Resources("term")]);

var enviar = {};
var start = moment();
var end = moment();
var btnOrderFilter = "";
var option = "";
var campo1Panel5Selected = "";

var filtrosDeColunas = [];

function GetDataTable(campo, filtro, campo2, filtro2) {

    $.get(urlGetPlanejamentoAcaoRange, enviar, function (r) {

        dados = r;

        //dados = $.grep(dados, function (a, b) { return a.Acao._Quem != 'gabrielnunes-mtz' }); //tira gabriel

        dadosfilter = dados;

        //https://grtsolucoes.atlassian.net/browse/JBS-110
        let usuarioIscorporativo = false//getRole("Admin");

        if (usuarioIscorporativo) {
            dados = $.grep(dados, function (a, b) { return a.Acao.TipoIndicador == 2 });
            $('.optionAlternativeRole').remove();
        }

        //fim https://grtsolucoes.atlassian.net/browse/JBS-110
        //Regras para o primeiro filtro geral
        if (campo == "") {
            campo = "Todos";
            $("campo1FiltroPie2 select").val("Todos");
        }

        if (campo != "Todos" && filtro != "Todos") {
            dados = FiltraLinhas(dados, [campo], [filtro]);
        }

        //Regras para o segundo filtro geral
        if (campo2 == "") {
            campo2 = "Todos";
            $("campo1FiltroPie2 select").val("Todos");
        }

        if (campo2 != "Todos" && filtro2 != "Todos") {
            dados = FiltraLinhas(dados, [campo2], [filtro2]);
        }

        //gera o gráfico de Pizza
        geraData1();
        //GERAL GRAFICO POR PESSOAS ENVOLVIDAS
        geraData2(dados);

        distinctFilter(dados, $('#campo1Panel5').val(), 'valor1Panel5');
        distinctFilter(dados, $('#campo2Panel5').val(), 'valor2Panel5');
        distinctFilter(dados, $('#campo1Panel6').val(), 'valor1Panel6');
        distinctFilter(dados, $('#campo2Panel6').val(), 'valor2Panel6');

        graficoEstoque();

        //celso

        //$('#btnFiltroPie2').click();
        $('#btnpanel5').click();
        $('#btnpanel6').click();

        json = dados;

        //MountDataTable(json);

        //Recupera colunas visíveis do usuário -- Não está em funçao pois dava loop infinito (16/01/2018 - Renan)
        if (getCookie('webControlCookie')) {

            Pa_Quem_Id = getCookie('webControlCookie')[0].split('=')[1];

            $.post(urlGetUserColvis, { "Pa_Quem_Id": Pa_Quem_Id }, function (r) {

                if (r.length > 0) {

                    //Colunas da Tabela de Ações
                    if (!!r[0].ColVisShow && !!r[0].ColVisHide) {

                        ColvisarrayVisaoAtual_show = objectToArr(r[0].ColVisShow.split(","));
                        ColvisarrayVisaoAtual_hide = objectToArr(r[0].ColVisHide.split(","));

                        if (ColvisarrayVisaoAtual_hide.length > 0) {
                            ColvisarrayVisaoUsuario_show = ColvisarrayVisaoAtual_show;
                            ColvisarrayVisaoUsuario_hide = ColvisarrayVisaoAtual_hide;
                        }
                    }

                    if (!!r[0].ColVisProjShow && !!r[0].ColVisProjHide) {
                        //Colunas da Tabela de Planejamento
                        ColvisarrayProjVisaoAtual_show = objectToArr(r[0].ColVisProjShow.split(","));
                        ColvisarrayProjVisaoAtual_hide = objectToArr(r[0].ColVisProjHide.split(","));

                        if (ColvisarrayProjVisaoAtual_hide.length > 0) {
                            ColvisarrayProjVisaoUsuario_show = ColvisarrayProjVisaoAtual_show;
                            ColvisarrayProjVisaoUsuario_hide = ColvisarrayProjVisaoAtual_hide;
                        }
                    }

                }

                //Monta a tabela
                MountDataTable(json);
                //MountDataTablePlanejamento(json);

            });
        }

        $('#spanSubTable').text(Resources("all_tasks_filter"));

        //$('#example_wrapper > div.dt-buttons > a:nth-child(1)').click();

        distinctFilter(dados, $('#campo1FiltroPie2').val(), 'valor1FiltroPie2');

    });
}

function objectToArr(myObj) {
    let array = $.map(myObj, function (value, index) {
        return [value];
    });

    return array
}

function MountDataTable(json) {

    GetDataTablePlanejamento(Object.assign([], json));

    if (ColvisarrayVisaoAtual_show.length != 0) {
        setArrayColvisAtual();

        setTimeout(function () {

            $('body > div.dt-button-background').click();
        }, 5);
    }

    table = null;

    //console.log(ColvisarrayVisaoAtual_show);
    //console.log(ColvisarrayVisaoAtual_hide);

    table = $('#example').DataTable({
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
            {
                //"mData": "_DataInicio",
                "mData": null,
                "render": function (data, type, row, meta) {
                    let html = "";
                    if (data.DataInicio != null && data.DataInicio != "0001-01-01T00:00:00")
                        html = "<span style='display:none'>" + data._DataInicio + "</span>" + new Date(data.DataInicio).toLocaleDateString();

                    return html;

                }
            },
            {
                //"mData": "_DataFim"
                "mData": null,
                "render": function (data, type, row, meta) {
                    let html = "";
                    if (data.DataInicio != null && data.DataFim != "0001-01-01T00:00:00")
                        html = "<span style='display:none'>" + data._DataFim + "</span>" + new Date(data.DataFim).toLocaleDateString();

                    return html;

                }
            },
            { "mData": "Responsavel_Projeto_Quem.Name" },
            { "mData": "Acao.Regional" },
            { "mData": "Acao.UnidadeName" },
            { "mData": "Acao.TipoIndicadorName" },
            { "mData": "Acao.Level1Name" },
            { "mData": "Acao.Level2Name" },
            { "mData": "Acao.Level3Name" },
            { "mData": "IndicadoresDeProjeto" }, // VER INDICADOR OPERACIONAL*
            { "mData": "Acao._CausaGenerica" },
            { "mData": "Acao._GrupoCausa" },
            { "mData": "Acao._ContramedidaGenerica" },
            { "mData": "Acao.CausaEspecifica" },
            { "mData": "Acao.ContramedidaEspecifica" },
            { "mData": "Acao._Quem" },
            {
                //"mData": "Acao._QuandoInicio"
                "mData": null,
                "render": function (data, type, row, meta) {
                    let html = "";
                    if (data.DataInicio != null && data.Acao.QuandoInicio != "0001-01-01T00:00:00")
                        html = "<span style='display:none'>" + data.Acao._QuandoInicio + "</span>" + new Date(data.Acao.QuandoInicio).toLocaleDateString();

                    return html;

                }
            },
            {
                //"mData": "Acao._QuandoFim"
                "mData": null,
                "render": function (data, type, row, meta) {
                    let html = "";
                    if (data.DataInicio != null && data.Acao.QuandoFim != "0001-01-01T00:00:00")
                        html = "<span style='display:none'>" + data.Acao._QuandoFim + "</span>" + new Date(data.Acao.QuandoFim).toLocaleDateString();

                    return html;

                }
            },
            { "mData": "Acao.ComoPontosimportantes" },
            { "mData": "Acao.PraQue" },
            { "mData": "Acao._QuantoCusta" },
            { "mData": "Acao._StatusName" },
            { "mData": "Acao._Prazo" },
            {
                "mData": null,
                "render": function (data, type, row, meta) {
                    var html = "";

                    if (!!(parseInt(data.Id) && parseInt(data.Id) > 0 || parseInt(data.Tatico_Id) && parseInt(data.Tatico_Id)) && (!parseInt(data.Acao.Id) && !parseInt(data.Acao.Id))) {
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

                    if (!!parseInt(data.Acao.Id) && parseInt(data.Acao.Id) > 0)  // Possui plan Operac
                        html += "<br>" + btnAcompanhamento

                    return html;
                }
            }
        ],
        'aoColumnDefs': [
            { "sTitle": Resources("directorship"), "aTargets": [0], "width": "100px" },
            { "sTitle": Resources("mission"), "aTargets": [1], "width": "200px" },
            { "sTitle": Resources("view"), "aTargets": [2], "width": "200px" },
            { "sTitle": Resources("dimension"), "aTargets": [3], "width": "50px" },
            { "sTitle": Resources("guidelines"), "aTargets": [4], "width": "200px" }, // ver diretriz
            { "sTitle": Resources("indicators_guidelines"), "aTargets": [5], "width": "300px" },
            { "sTitle": Resources("responsible_guideline"), "aTargets": [6], "width": "50px" },
            { "sTitle": Resources("theme_subject"), "aTargets": [7], "width": "100px" },
            { "sTitle": Resources("management"), "aTargets": [8], "width": "100px" },
            { "sTitle": Resources("coordination"), "aTargets": [9], "width": "100px" },
            { "sTitle": Resources("type_of_project"), "aTargets": [10], "width": "100px" },
            { "sTitle": Resources("project_theme"), "aTargets": [11], "width": "100px" },
            { "sTitle": Resources("project_initiative"), "aTargets": [12], "width": "200px" },
            { "sTitle": Resources("indicators_project_initiative"), "aTargets": [13], "width": "100px" },
            { "sTitle": Resources("management_objective"), "aTargets": [14], "width": "100px" },
            { "sTitle": Resources("value_of"), "aTargets": [15], "width": "50px" },
            { "sTitle": Resources("value_for"), "aTargets": [16], "width": "50px" },
            { "sTitle": Resources("start_date"), "aTargets": [17], "width": "50px" },
            { "sTitle": Resources("end_date"), "aTargets": [18], "width": "50px" },
            { "sTitle": Resources("responsible_project_initiative"), "aTargets": [19], "width": "50px" },
            { "sTitle": Resources("regional"), "aTargets": [20], "width": "50px" },
            { "sTitle": Resources("unit"), "aTargets": [21], "width": "50px" },
            { "sTitle": Resources("indicator_type"), "aTargets": [22], "width": "50px" },
            { "sTitle": Resources("indicator_sgq"), "aTargets": [23], "width": "100px" },
            { "sTitle": Resources("monitoring_sgq"), "aTargets": [24], "width": "100px" },
            { "sTitle": Resources("task_sgq"), "aTargets": [25], "width": "100px" },
            { "sTitle": Resources("operational_indicators"), "aTargets": [26], "width": "100px" }, // ver indicador operacional*
            { "sTitle": Resources("generic_cause"), "aTargets": [27], "width": "200px" },
            { "sTitle": Resources("group_cause"), "aTargets": [28], "width": "200px" },
            { "sTitle": Resources("generic_action"), "aTargets": [29], "width": "100px" },
            { "sTitle": Resources("specific_cause2"), "aTargets": [30], "width": "100px" },
            { "sTitle": Resources("specific_action"), "aTargets": [31], "width": "100px" },
            { "sTitle": Resources("who"), "aTargets": [32], "width": "200px" },
            { "sTitle": Resources("when_start"), "aTargets": [33], "width": "50px" },
            { "sTitle": Resources("when_end"), "aTargets": [34], "width": "50px" },
            { "sTitle": Resources("as_important_points"), "aTargets": [35], "width": "200px" },
            { "sTitle": Resources("for_what"), "aTargets": [36], "width": "200px" },
            { "sTitle": Resources("how_much"), "aTargets": [37], "width": "50px" },
            { "sTitle": Resources("status"), "aTargets": [38], "width": "50px" },
            { "sTitle": Resources("term"), "aTargets": [39], "width": "50px" },
            { "sTitle": Resources("action") },

        ],
        "responsive": true,
        "bSearchable": true,
        "bFilter": true,
        "paging": true,
        "lengthMenu": [[10, 20, 50, -1], [10, 20, 50, Resources("all")]],
        "info": true,
        "scrollY": 370,
        "scrollX": 500,
        "bLengthChange": true,
        "dom": 'Blfrtip',
        "buttons": [
            {
                extend: 'colvisGroup',
                text: Resources("initial_view"),
                //show: [4, 7, 8, 12, 13, 14, 15, 16, 17, 18, 19, 39, 40],
                //hide: [0, 1, 2, 3, 5, 6, 9, 10, 11, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38]                
                show: [3, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40],
                hide: [0, 1, 2, 4, 5, 6, 7, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19]
            },
            {
                extend: 'colvisGroup',
                text: Resources("strategic_planning"),
                show: [0, 1, 2, 3, 4, 5, 6, 35, 39, 40],
                hide: [7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 36, 37, 38]
            },
            {
                extend: 'colvisGroup',
                text: Resources("tactical_planning"),
                show: [7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 39, 40],
                hide: [0, 1, 2, 4, 5, 6, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38]
            },
            {
                extend: 'colvisGroup',
                text: Resources("operational_planning"),
                show: [3, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40],
                hide: [0, 1, 2, 4, 5, 6, 7, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19]
            },
            {
                extend: 'colvisGroup',
                text: Resources("show_all"),
                show: ':hidden'
            },
            {
                extend: 'colvisGroup',
                text: Resources("current_view"),
                show: ColvisarrayVisaoAtual_show,
                hide: ColvisarrayVisaoAtual_hide
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
                text: Resources("visible_columns"),
                collectionLayout: 'fixed four-column',
                exportOptions: {
                    columns: ':visible',
                }
            },
            {
                text: Resources("update"),
                action: function (e, dt, node, config) {
                    $('#btnTop').click();
                },
            },
            {
                text: Resources("my_columns"),
                extend: 'colvisGroup',
                show: ColvisarrayVisaoUsuario_show,
                hide: ColvisarrayVisaoUsuario_hide
            },
            {
                text: Resources("save_columns"),
                action: function (e, dt, node, config) {
                    let Tabela = "Acao"
                    SaveUserColVis(Tabela);
                },
            },
            //novaAcao: {
            //    text: 'Nova Ação',
            //    action: function (e, dt, node, config) {
            //        Clicked(isTaticoClicked, isNovaAcao);

            //        $('#modalLindo').modal();
            //        $('#modalLindo').find('.modal-body').empty();
            //        $('#Header').html("Planejamento Operacional");

            //        $.get(PlanejamentoDetalhes, { id: 1040 }, function (r) {
            //            $('#modalLindo').find('.modal-body').empty().append(r);
            //            $('#NovaAcao').show();
            //            $('#NovaAcao').click();
            //        });
            //    },
            //}
            //}
        ],
        fixedColumns: {
            leftColumns: 0,
            rightColumns: 2,
        },
        initComplete: function () {

            $('#example_wrapper > div.dt-buttons > a.dt-button.buttons-collection.buttons-colvis').on("click", function () {

                $('body > div.dt-button-collection.fixed.four-column > a:nth-child(40)').hide();
                $('body > div.dt-button-collection.fixed.four-column > a:nth-child(41)').hide();
            });
        },
        createdRow: function (row, data, index) {

            try {
                var bgColorStatus = ""
                var bgColorPrazo = ""
                /*Status*/
                //console.log(data.Acao.Status);
                if (data.Acao.Status == 2) {
                    bgColorStatus = "grey";
                    bgColorPrazo = "rgb(126, 194, 253)";
                } else if (data.Acao.Status == 5) {
                    bgColorStatus = "#ADD8E6";
                } else if (data.Acao.Status == 5 && data.Acao.StatusName.indexOf('Atrasado') > -1) {
                    bgColorStatus = "#458fa8"
                } else if (data.Acao.Status == 3/* > -1 && data.Acao.StatusName.indexOf('Prazo') > -1*/) {
                    bgColorPrazo = "rgb(126, 194, 253)";
                    bgColorStatus = "cyan"
                } else if (data.Acao.Status == 4) {
                    bgColorPrazo = "rgb(126, 194, 253)";
                    bgColorStatus = "steelblue"
                } else if (data.Acao.Status == 9) {
                    bgColorStatus = "#E0EEEE";
                }

                //else if (data.Acao.StatusName.indexOf('Replanejado') > -1) {
                //    bgColorStatus = "yellow"
                //}

                $(row.cells[38]).css("background", bgColorStatus);

                /*Prazo*/
                if (data.Acao.Status == 2) {

                } else if (data.Acao.Status == 3) {

                    //} else if (data.Acao._Prazo.indexOf('Faltam') > -1) {
                } else if (data.Acao._Prazo.match(/\d+/g)) {
                    let numero = data.Acao._Prazo.split(" ")[0]
                    //console.log(numero)
                    if (numero == 0) {
                        bgColorPrazo = "rgba(253, 245, 154, 0.67)"
                    } else if (isPositiveInteger(numero)) {
                        bgColorPrazo = "#90EE90"
                    } else {
                        bgColorPrazo = "rgb(250, 128, 114)"
                    }
                }
                else if (data.Acao._Prazo.indexOf('-') > -1 && data.Acao._Prazo.indexOf(' Dias') > -1) {
                    bgColorPrazo = "rgb(250, 128, 114)"
                }

                $(row.cells[39]).css("background", bgColorPrazo);

                //    if (data.Tatico_Id > 0) { // possui plan tatico
                //        $(row.cells[38]).find('.btnNovoOperacional').show();
                //    } else {
                //        $(row.cells[38]).find('.btnNovoOperacional').hide();
                //    }

                //    if (data.Id > 0) { // Possui plan Estrat
                //        $(row.cells[38]).find('.btnNovoTatico').show();
                //    } else {
                //        $(row.cells[38]).find('.btnNovoTatico').hide();
                //    }

                //    if (data.Acao.Id > 0) { // Possui plan Operac
                //        $(row.cells[38]).find('.btnAcompanhamento').show();
                //    } else {
                //        $(row.cells[38]).find('.btnAcompanhamento').hide();
                //    }

            } catch (e) { }

            //if (data.Tatico_Id > 0) { // possui plan tatico
            //    $(row.cells[38]).find('.btnNovoOperacional').show();
            //} else {
            //    $(row.cells[38]).find('.btnNovoOperacional').hide();
            //}

            //if (data.Id > 0) { // Possui plan Estrat
            //    $(row.cells[38]).find('.btnNovoTatico').show();
            //} else {
            //    $(row.cells[38]).find('.btnNovoTatico').hide();
            //}

            //if (data.Acao.Id > 0) { // Possui plan Operac
            //    $(row.cells[38]).find('.btnAcompanhamento').show();
            //} else {
            //    $(row.cells[38]).find('.btnAcompanhamento').hide();
            //}

        },
        "language": {
            "sEmptyTable": Resources("no_records_found"),
            "sInfo": Resources("showing_start_to_end_of_total_records"),
            "sInfoEmpty": Resources("showing_0_to_0_of_0_records"),
            "sInfoFiltered": Resources("filtered_max_records"),
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": Resources("menu_results_per_page"),
            "sLoadingRecords": Resources("loading"),
            "sProcessing": Resources("processing"),
            "sZeroRecords": Resources("no_records_found"),
            "sSearch": Resources("search"),
            "oPaginate": {
                "sNext": Resources("next"),
                "sPrevious": Resources("back"),
                "sFirst": Resources("first"),
                "sLast": Resources("last")
            },
            "oAria": {
                "sSortAscending": Resources("sort_columns_ascending"),
                "sSortDescending": Resources("sort_columns_downward")
            }
        }

    });

    setTimeout(function () {

        if (ColvisarrayVisaoAtual_hide.length != 0) {

            $('#example_wrapper > div.dt-buttons > a:nth-child(6)').click();

        } else {
            $('#example_wrapper > div.dt-buttons > a:nth-child(1)').click();
        }

        $(".dataTables_filter").css("display", "block");

    }, 1100);

    $('#virtualBody').css('width', '100%');

    //Filtros por coluna

    $('#example_wrapper .dataTable:not(.DTFC_Cloned) thead th').each(function (i) {
        //$('.dataTable thead th').each(function (i) {
        var title = $('#example_wrapper .dataTable:not(.DTFC_Cloned) thead th').eq($(this).index()).text();
        $(this).html(title + '<br><input type="text" style="font-size:xx-small; color: #555; text-align:center; width:50px" placeholder=" ' + title + '" data-index="' + i + '" />');
    });

    $('.dataTable thead th').css('text-align', 'center');

    // Filter event handler
    $(table.table().container()).on('keyup', 'thead input', function () {
        table
            .column($(this).data('index'))
            .search(this.value)
            .draw();

        GetFiltrosDeColunas();
    });

    SetFiltrosDeColunas();

    table.draw();

    if (ColvisarrayVisaoAtual_show.length == 0) {
        setArrayColvisAtual();

        setTimeout(function () {

            $('body > div.dt-button-background').click();
        }, 5);
    }

    //deixa escondido o botão que mantem as colunas atuais
    $('#example_wrapper > div.dt-buttons > a:nth-child(6)').hide();

    //clicar no botão escondido das colunas atuais
    if (ColvisarrayVisaoAtual_show.length > 0) {

        $('#example_wrapper > div.dt-buttons > a:nth-child(6)').click();

    }

    $('#example_wrapper > div.DTFC_ScrollWrapper > div.DTFC_RightWrapper > div.DTFC_RightHeadWrapper > table > thead > tr > th:nth-child(2) > input[type="text"]').hide();

    $('#example_wrapper > div.dt-buttons').on('click', 'a:nth-child(2)', function () {
        tableDraw();
    });

    $('#example_wrapper > div.dt-buttons').on('click', 'a:nth-child(3)', function () {
        tableDraw();
    });

    $('#example_wrapper > div.dt-buttons').on('click', 'a:nth-child(4)', function () {
        tableDraw();
    });

    $('#example_wrapper > div.dt-buttons').on('click', 'a:nth-child(4)', function () {
        tableDraw();
    });

    $('#example_wrapper > div.dt-buttons').on('click', 'a:nth-child(5)', function () {
        tableDraw();
    });

}

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

function tableDraw() {
    setTimeout(function () {
        table.draw();
    }, 50);
}

function FiltraColunas(array, arrFiltro) {

    let novoArr = [];

    array.forEach(function (o, c) {

        obj = {};
        objt = {};
        arrFiltro.forEach(function (oo, cc) {

            obj[oo] = o[oo];
            objt[oo] = o.Acao[oo];

            if (cc == "TipoIndocador") {
                if (objt.TipoIndicador == 1) {
                    $('')
                }
            }
        });

        novoArr.push(obj);
        novoArr.push(objt);
    });

    return novoArr;
}

function FiltraLinhas(array, arrColuna, arrValue) {

    let novoArr = []

    array.forEach(function (o, c) {

        var flag = true;

        if (arrColuna == "_Quem" || arrColuna == "_GrupoCausa" || arrColuna == "_CausaGenerica" || arrColuna == "_ContramedidaGenerica"
            || arrColuna == "UnidadeName" || arrColuna == "_StatusName" || arrColuna == "Regional"
            || arrColuna == "Level1Name" || arrColuna == "Level2Name" || arrColuna == "Level3Name" || arrColuna == "Acao.TipoIndicador") {

            if (arrColuna == "Acao.TipoIndicador" && arrValue != "Todos") {



                arrValueAux = [];
                if (arrValue == "Diretrizes")
                    arrValueAux.push(1);
                else if (arrValue == "Scorecard")
                    arrValueAux.push(2);



                arrColuna.forEach(function (oo, cc) {

                    if (o.Acao[oo.replace('Acao.', '')] != arrValueAux[cc]) {
                        flag = false;
                    }
                });

                if (flag) {
                    novoArr.push(o)
                }

            } else {
                arrColuna.forEach(function (oo, cc) {

                    if (o.Acao[oo] != arrValue[cc]) {
                        flag = false;
                    }
                });

                if (flag) {
                    novoArr.push(o)
                }
            }


        } else {

            if (arrValue[0] == 'Todos') {

                novoArr.push(o);

            } else {

                arrColuna.forEach(function (oo, cc) {
                    if (o[oo] != arrValue[cc]) {
                        flag = false;
                    }
                });

                if (flag) {
                    novoArr.push(o)
                }
            }
        }
    });

    return novoArr;
}

function FiltraLinhasComTodos(array, arrColuna, arrValue) {

    let novoArr = []

    array.forEach(function (o, c) {
        var flag = true;
        if (arrValue != "Todos" && arrValue != "Todos") {
            if (arrColuna == "_Quem" || arrColuna == "_GrupoCausa" || arrColuna == "_CausaGenerica" || arrColuna == "_ContramedidaGenerica"
                || arrColuna == "UnidadeName" || arrColuna == "_StatusName" || arrColuna == "Regional"
                || arrColuna == "Level1Name" || arrColuna == "Level2Name" || arrColuna == "Level3Name" || arrColuna == "Acao.TipoIndicador") {

                if (arrColuna == "Acao.TipoIndicador" && arrValue != 0) {

                    arrValueAux = [];
                    if (arrValue == "Diretrizes")
                        arrValueAux.push(1);
                    else if (arrValue == "Scorecard")
                        arrValueAux.push(2);

                    arrColuna.forEach(function (oo, cc) {

                        if (o.Acao[oo.replace('Acao.', '')] != arrValueAux[cc]) {
                            flag = false;
                        }
                    });
                } else {
                    arrColuna.forEach(function (oo, cc) {

                        if (o.Acao[oo] != arrValue[cc]) {
                            flag = false;
                        }
                    });
                }
            } else {
                arrColuna.forEach(function (oo, cc) {

                    if (o[oo] != arrValue[cc]) {
                        flag = false;
                    }
                });
            }
        }
        if (flag) {
            novoArr.push(o)
        }
    });

    return novoArr;
}

function isPositiveInteger(n) {
    return n == "0" || ((n | 0) > 0 && n % 1 == 0);
}

function getDateRange(campo) { //$("input[name='daterange']").val()
    var datas = campo.split(' - ');
    var dataPTInicio = datas[0].split("/");
    var dataPTFim = datas[1].split("/");

    dataInicio = new Date(dataPTInicio[2] + ' ' + dataPTInicio[1] + ' ' + dataPTInicio[0]);
    dataFim = new Date(dataPTFim[2] + ' ' + dataPTFim[1] + ' ' + dataPTFim[0]);
}

function getDateUSA(campo) { //$("input[name='daterange']").val()
    var dataPTInicio = campo.split("/");
    data = new Date(dataPTInicio[2] + ' ' + dataPTInicio[1] + ' ' + dataPTInicio[0]);
    if (data != 'Invalid Date') {
        return data;
    }
}

function sortFunction(a, b) {
    if (a[0] === b[0]) {
        return 0;
    }
    else {
        return (a[0] < b[0]) ? -1 : 1;
    }
}

//Criar arrays para graficos Panel5 e 6, os filtros devem ser inseridos antes do makechart
function filtraDadosParaGerarGraficoPanel5Panel6(categoriesFilterVal, seriesFilterVal, order, id, optionsDef) {
    let hasFilter = categoriesFilterVal && seriesFilterVal
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

        makeChart(id, categoriesArr, serieArrFinal, 'bar', categoriesFilterVal)
    } else
        makeChart(id, categoriesArr, serieArrFinal, 'bar', categoriesFilterVal)
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
//Instancia HighCharts em um grafico padrão, aceita options para sobrescrita
function makeChart(id, categoriesArr, seriesArr, type, yAxisTitle, optionsDef) {

    if (id == 'panel5') {
        $('#semDados1').hide();
    }

    if (id == 'panel6') {
        $('#semDados2').hide();
    }

    if (seriesArr[0] != undefined) {

        var semDados = false;

        for (var h = 0; h < seriesArr.length; h++) {
            if (seriesArr[h].data.length == 1 && seriesArr[h].data[0] == 0) {
                semDados = false;
            } else {
                semDados = true;
                break;
            }

        }

        if (!semDados) {
            if (id == 'panel5') {
                $('#semDados1').show();
                return;
            }

            if (id == 'panel6') {
                $('#semDados2').show();
                return;
            }
        }
    }

    let options = {
        chart: {
            type: type,
            zoomType: 'xy'
        },
        title: {
            text: false,
        },
        subtitle: {
            text: false, //'Source: <a href="https://en.wikipedia.org/wiki/World_population">Wikipedia.org</a>'
        },
        xAxis: {

            categories: categoriesArr,
            title: {
                text: null
            },
            labels: {

                style: {
                    fontSize: '8px',
                    fontFamily: 'Verdana, sans-serif'
                }
            },
            tickInterval: 1,
            allowDecimals: false
        },
        yAxis: {
            //min: 0,
            //max: 30,
            allowDecimals: false,
            title: {
                text: GetPropertyRealName(yAxisTitle),
                align: 'high',
                style: {
                    fontSize: '8px',
                    fontFamily: 'Verdana, sans-serif'
                }
            },
            labels: {
                overflow: 'justify',

                style: {
                    fontSize: '8px',
                    fontFamily: 'Verdana, sans-serif'
                }

            }
        },
        tooltip: {
            valueSuffix: ''
        },
        plotOptions: {
            series: {
                stacking: 'normal',
                events: {
                    click: function (event) {
                        filterBar1ForDataTable(this.name, event.point.category, id)

                    }
                }

            },
            bar: {
                dataLabels: {
                    enabled: false
                }
            }
        },
        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'bottom',
            floating: false,
            itemStyle: {
                fontSize: '8px',
                fontFamily: 'Verdana, sans-serif'
            }
        },
        credits: {
            enabled: false
        },
        /*
            {name: 'Concluídas', y: 56.33, color: concluidoColor },
            {name: 'Atrasadas', y: 10.38, color: atrasadaColor },
            {name: 'Em Andamento', y: 4.77, color: andamentoColor },
            */
        series: seriesArr
    }

    if (optionsDef)
        Object.assign(options, optionsDef);

    Highcharts.chart(id, options);
}
//Distinct usage: MapeiaValorParaHC(dados, seriesFilter).filter(onlyUnique)
function onlyUnique(value, index, self) {
    return self.indexOf(value) === index;
}
//Desc pendente
function MapeiaValorParaHC(array, prop, isInteger) {
    var arrayRetorno = $.map(array, function (o, c) {
        if (isInteger) {
            var valor = parseInt(o[prop]);
            if (isNaN(valor)) return 0;
            return valor;
        }
        var propArray = prop.split('.');
        if (propArray.length == 2) {
            if (propArray[1] == "TipoIndicador") {
                var value = o[propArray[0]][propArray[1]];
                if (value == 0)
                    value = Resources("no_operational_planning");
                else if (value == 1)
                    value = Resources("guidelines");
                else if (value == 2)
                    value = "Scorecard";
                return value;
            } else {
                return o[propArray[0]][propArray[1]];

            }
        } else {
            return o[prop];
        }
    });
    return arrayRetorno;
}
//Desc pendente
function orderArray(array) {

    array.sort(function (a, b) {
        if (a.name > b.name) {
            return 1;
        }
        if (a.name < b.name) {
            return -1;
        }

        return 0;
    });

    return array;
}
//Desc pendente
function filtraAgrupaXY(categoriesArr, seriesFilter, categoriesFilter, dados, verifyStatus, id) {

    var filtroEixoX = [];

    if (id == 'panel5') {

        if ($('#valor2Panel5 option:selected').text() == Resources("all")) {

            let categories = categoriesFilter.split('.');

            let dados2 = $.grep(dados, function (r) {

                if (categories.length == 2) {
                    if ($('#valor1Panel5 option:selected').text() == Resources("all")) {
                        return r;

                    } else {

                        if (categories[1] == "TipoIndicador") {

                            let value = $('#valor1Panel5 option:selected').text();

                            if (value == Resources("no_operational_planning"))
                                return r[categories[0]][categories[1]] == 0;
                            else if (value == Resources("guidelines"))
                                return r[categories[0]][categories[1]] == 1;
                            else if (value == "Scorecard")
                                return r[categories[0]][categories[1]] == 2;

                        } else {

                            return r[categories[0]][categories[1]] == $('#valor1Panel5 option:selected').text();
                        }
                    }
                } else {
                    if ($('#valor1Panel5 option:selected').text() == Resources("all")) {
                        return r;
                    } else {
                        return r[categories] == $('#valor1Panel5 option:selected').text();
                    }
                }
            });

            filtroEixoX = MapeiaValorParaHC(dados2, seriesFilter).filter(onlyUnique);

        } else {

            filtroEixoX.push($('#valor2Panel5 option:selected').text());

        }

    } else if (id == 'panel6') {

        if ($('#valor2Panel6 option:selected').text() == Resources("all")) {

            let categories = categoriesFilter.split('.');

            let dados2 = $.grep(dados, function (r) {

                if (categories.length == 2) {

                    if ($('#valor1Panel6 option:selected').text() == Resources("all")) {
                        return r;

                    } else {
                        if (categories[1] == "TipoIndicador") {

                            let value = $('#valor1Panel6 option:selected').text();

                            if (value == Resources("no_operational_planning"))
                                return r[categories[0]][categories[1]] == 0;

                            else if (value == Resources("guidelines"))
                                return r[categories[0]][categories[1]] == 1;

                            else if (value == "Scorecard")
                                return r[categories[0]][categories[1]] == 2;

                        } else {

                            return r[categories[0]][categories[1]] == $('#valor1Panel6 option:selected').text();
                        }
                    }
                } else {
                    if ($('#valor1Panel6 option:selected').text() == Resources("all")) {
                        return r;
                    } else {
                        return r[categories] == $('#valor1Panel6 option:selected').text();
                    }

                }
            });

            filtroEixoX = MapeiaValorParaHC(dados2, seriesFilter).filter(onlyUnique);
        }
        else {

            filtroEixoX.push($('#valor2Panel6 option:selected').text());

        }
    }


    let seriesAux = filtroEixoX;
    let serieArrFinal = [];

    seriesAux.forEach(function (o, c) {

        let serieData = {};

        serieData["name"] = o;
        serieData["data"] = [];

        categoriesArr.forEach(function (oo, cc) {

            let QtdePorSerie = $.grep(dados, function (e) {

                let retornoSeries;
                let retornoCategorias;

                let propArrayS = seriesFilter.split('.');

                if (propArrayS.length == 2) {
                    retornoSeries = e[propArrayS[0]][propArrayS[1]];
                } else {
                    retornoSeries = e[seriesFilter];
                }

                let propArrayC = categoriesFilter.split('.');

                if (propArrayC.length == 2) {
                    retornoCategorias = e[propArrayC[0]][propArrayC[1]];
                } else {
                    retornoCategorias = e[categoriesFilter];
                }

                if (propArrayC[1] == "TipoIndicador") {

                    let value = retornoCategorias;

                    if (value == 0)
                        value = Resources("no_operational_planning");
                    else if (value == 1)
                        value = Resources("guidelines");
                    else if (value == 2)
                        value = "Scorecard";
                    retornoCategorias = value;
                }

                if (propArrayS[1] == "TipoIndicador") {

                    let value = retornoSeries;

                    if (value == 0)
                        value = Resources("no_operational_planning");
                    else if (value == 1)
                        value = Resources("guidelines");
                    else if (value == 2)
                        value = "Scorecard";

                    retornoSeries = value;
                }

                return retornoSeries == o && retornoCategorias == categoriesArr[cc];

            }).length

            serieData["data"].push(QtdePorSerie);

        });

        serieArrFinal.push(serieData);
    });

    //remove as series que não possuem quantidade
    serieArrFinal.forEach(function (o, i) {
        if (o.data.reduce((a, b) => a + b, 0) == 0) {
            serieArrFinal.splice(i, 1);
        }
    });

    if (verifyStatus)
        pintaStatus(seriesFilter, serieArrFinal)
    return serieArrFinal
}
//Desc pendente
function pintaStatus(seriesFilter, serieArrFinal) {
    if (seriesFilter == 'Status' && serieArrFinal != '') {

        Highcharts.setOptions({
            colors: ['red', 'pink', 'blue', 'orange', 'green', 'black', "yellow", "gray"]
        });

        var serieAux;
        var listaVazia;
        var aux = 0;
        serieAux = orderArray(serie);
        serie = [];

        for (var i = 0; i < listaStatus.length; i++) {
            if (serieAux[aux].name == listaStatus[i]) {
                serie.push(serieAux[aux]);
                if (serieAux.length > aux + 1) {
                    aux++;
                }
            } else {
                listaVazia = { name: listaStatus[i], data: null, visible: false };
                serie.push(listaVazia);
            }
        }
        return serie
    } else {

        serieArrFinal.forEach(function (c, o) {
            if (c.name == Resources("late")) {
                c["color"] = atrasadaColor;
            } else if (c.name == Resources("completed")) {
                c["color"] = concluidoColor;
            } else if (c.name == Resources("completed_late")) {
                c["color"] = concluidoAtrasoColor;
            } else if (c.name == Resources("in_progress")) {
                c["color"] = andamentoColor;
            } else if (c.name == Resources("canceled")) {
                c["color"] = canceladoColor;
            } else if (c.name == Resources("_return")) {
                c["color"] = retornoColor;
            } else if (c.name == Resources("finished")) {
                c["color"] = finalizadaColor;
            } else if (c.name == Resources("finished_late")) {
                c["color"] = finalizadaComAtrasoColor;
            } else if (c.name == Resources("not_started")) {
                c["color"] = naoIniciadoColor;
            }
        });

        return serieArrFinal
    }
}

$(function () {

    var rangesBR = {
        'Hoje': [moment(), moment()],
        'Ontem': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
        'Últimos 7 Dias': [moment().subtract(6, 'days'), moment()],
        'Últimos 30 Dias': [moment().subtract(29, 'days'), moment()],
        'Este mês': [moment().startOf('month'), moment().endOf('month')],
        'Último mês': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
    }

    moment.locale("Pt-Br");
    moment.defaultFormat = "DD/MM/YYYY";

    $.fn.daterangepicker('locale', {
        "format": "DD/MM/YYYY",
        "separator": " - ",
        "applyLabel": Resources("apply"),
        "cancelLabel": Resources("cancel"),
        "fromLabel": Resources("of"),
        "toLabel": Resources("until"),
        "customRangeLabel": Resources("customized"),
        "weekLabel": "W",
        "daysOfWeek": [
            Resources("sun"),
            Resources("mon"),
            Resources("tue"),
            Resources("wed"),
            Resources("thur"),
            Resources("fri"),
            Resources("sat")
        ],
        "monthNames": [
            Resources("jan2"),
            Resources("feb2"),
            Resources("mar2"),
            Resources("apr2"),
            Resources("may2"),
            Resources("june2"),
            Resources("july2"),
            Resources("aug2"),
            Resources("sept2"),
            Resources("oct2"),
            Resources("nov2"),
            Resources("dec2")
        ],
        //"firstDay": 0
    });

    rangesCalendar = rangesBR;

    $.fn.daterangepicker('ranges', rangesCalendar);

    //Start config datepickerrange full

    function cb(start, end) {
        $('#reportrange span').html(start.format() + ' - ' + end.format());
        enviar['startDate'] = start.format();
        enviar['endDate'] = end.format();
    }
    var configQueryData = true; // @*@Html.Raw(Json.Encode(GlobalConfig.LanguageBrasil));*@
    var configCallendar = {};
    if (configQueryData) {
        $('input[name="daterange"]').daterangepicker({
            startDate: start,
            endDate: end,
            //opens: "left",
            //autoApply: false,
            locale: {
                "format": "DD/MM/YYYY",
                "separator": " - ",
                "applyLabel": Resources("apply"),
                "cancelLabel": Resources("cancel"),
                "customRangeLabel": Resources("selection"),
            },
            alwaysShowCalendars: true,
            showDropdowns: true,
            ranges: rangesCalendar,
            linkedCalendars: false
        }, cb);
    } else {
        $('input[name="daterange"]').daterangepicker({
            startDate: start,
            endDate: end,
            //autoApply: false,
            locale: {
                "format": "MM/DD/YYYY",
                "separator": " - ",
            },
            alwaysShowCalendars: true,
            showDropdowns: true,
            ranges: rangesCalendar,
            linkedCalendars: false
        }, cb);
    }

    cb(start, end);
    //End config datepickerrange full

    enviar['startDate'] = moment().add('month', -1).format();
    enviar['endDate'] = moment().format();

    //$('input[name="daterange"]').daterangepicker();

});

function filterEixoY(selectId) {

    if (campo1Panel5Selected != $('#campo1Panel5 option:selected').val()) {
        campo1Panel5Selected = $('#campo1Panel5 option:selected').val();
        var valores = MapeiaValorParaHC(dados, $('#campo1Panel5 option:selected').val()).filter(onlyUnique);
        option = "";
        option += "<option name = \"todos\" value = \"\">Todos</option>";
        valores.forEach(logArrayElements);

        $('#' + selectId).empty();
        $('#' + selectId).append(option);
        $('#' + selectId).selectpicker('refresh');

    }

}

function logArrayElements(element, index, array) {
    var auxElement = element.replace(" ", "")
    option += "<option value=\"" + index + "\" name=\"y\" >" + element + "</option>";

}

function btnOrder(campo1, campo2, painel, btn) {

    if (btnOrderFilter == "Asc")
        btnOrderFilter = "";
    else if (btnOrderFilter == "")
        btnOrderFilter = "Asc";

    var titleSpan = "";
    var classSpan = "";
    switch (btnOrderFilter) {
        case "":
            classSpan = "glyphicon glyphicon-sort-by-attributes-alt";
            titleSpan = Resources("order_by_value");
            break;
        case "Asc":
            titleSpan = Resources("alphabetical_order");
            classSpan = "glyphicon glyphicon-sort-by-alphabet";
            break;
    }
    $('#' + btn + ' span').removeClass().addClass(classSpan);
    document.getElementById(btn).title = titleSpan;

    filtraDadosParaGerarGraficoPanel5Panel6($('#' + campo1 + ' option:selected').val(), $('#' + campo2 + ' option:selected').val(), btnOrderFilter, painel)

}

function distinctFilter(lista, filtro, selectId) {

    filtro = filtro.replace("Acao.", "");
    lista = FiltraColunas(lista, [filtro]);

    var l = [];
    var retorno = [];

    jQuery.each(lista, function (i, val) {


        l.push(lista[i][filtro]);


    })

    l.sort();

    jQuery.each(l, function (i, val) {

        if (l[0] == undefined) {
            retorno.push(l[i]);
        } else if (retorno[retorno.length - 1] == l[i]) {

        } else {
            retorno.push(l[i]);
        }
    });

    $('#' + selectId).children('option').remove();

    $('#' + selectId).append($("<option></option>").attr("value", 0).text(Resources("all")));

    //$('#valor2Panel5').append($("<option></option>").attr("value", 0).text("Todos"));
    //$('#valor1FiltroPie2').append($("<option></option>").attr("value", 0).text("Todos"));
    //$('#valor1Panel5').append($("<option></option>").attr("value", 0).text("Todos"));
    //$('#valor1Panel6').append($("<option></option>").attr("value", 0).text("Todos"));
    //$('#valor2Panel6').append($("<option></option>").attr("value", 0).text("Todos"));


    $.each(retorno, function (key, value) {
        if ($('#campo1Filtro option:selected').val() == "Acao.TipoIndicador" ||
            $('#campo1FiltroPie2 option:selected').val() == "Acao.TipoIndicador" ||
            $('#campo1Panel5 option:selected').val() == "Acao.TipoIndicador" ||
            $('#campo2Panel5 option:selected').val() == "Acao.TipoIndicador" ||
            $('#campo1Panel6 option:selected').val() == "Acao.TipoIndicador" ||
            $('#campo2Panel6 option:selected').val() == "Acao.TipoIndicador"
        ) {
            if (value == 0)
                value = Resources("no_operational_planning");
            else if (value == 1)
                value = Resources("guidelines");
            else if (value == 2)
                value = "Scorecard";
        }
        if (value != null && value != "0") {

            $('#' + selectId)

                .append($("<option></option>")

                    .attr("value", key)

                    .text(value));
        }

    });

    $('#' + selectId).trigger('change');//selectpicker('refresh');

}

function geraData1() {

    data1 = [];

    dados.sort(function (a, b) {
        return a.Acao.Status - b.Acao.Status;
    });

    //jQuery.each(dados, function (i, val) {
    //    console.log(dados[i].Acao.Status);
    //});

    jQuery.each(dados, function (i, val) {

        var campo = '';
        var cor = '';

        switch (dados[i].Acao.Status) {
            case 0:
                campo = Resources('without_status');
                cor = '#fdffb2';
                break;
            case 1:
                campo = Resources('late');
                cor = atrasadaColor;
                break;
            case 2:
                campo = Resources('cancel');
                cor = canceladoColor;
                break;
            case 3:
                campo = Resources('completed');
                cor = concluidoColor;
                break;
            case 4:
                campo = Resources('completed_late');
                cor = concluidoAtrasoColor;
                break;
            case 5:
                campo = Resources('in_progress');
                cor = andamentoColor;
                break;
            case 6:
                campo = Resources('_return');
                cor = retornoColor;
                break;
            case 7:
                campo = Resources('finished');
                cor = finalizadaColor;
                break;
            case 8:
                campo = Resources('finished_late');
                cor = finalizadaComAtrasoColor;
                break;
            case 9:
                campo = Resources('not_started');
                cor = naoIniciadoColor;
                break;
            default:
                campo = Resources('status');
                cor = 'black';
                break;

        }

        if (data1[0] == undefined) {
            if (dados[i].Acao.Status != 0) {
                data1.push({ name: campo, y: 1, color: cor })
            }
        } else if (data1[data1.length - 1].name == campo) {
            data1[data1.length - 1].y = data1[data1.length - 1].y + 1;
        } else if (dados[i].Acao.Status != 0) {
            data1.push({ name: campo, y: 1, color: cor })
        }
    });

    //getGraphPanel1(data1);
}

function geraData2(dadosFiltrados) {

    data1 = [];

    dadosFiltrados.sort(function (a, b) {
        return a.Acao.Status - b.Acao.Status;
    });


    jQuery.each(dadosFiltrados, function (i, val) {

        var campo = '';
        var cor = '';

        switch (dadosFiltrados[i].Acao.Status) {
            case 0:
                campo = Resources('without_status');
                cor = '#fdffb2';
                break;
            case 1:
                campo = Resources('late');
                cor = atrasadaColor;
                break;
            case 2:
                campo = Resources('cancel');
                cor = canceladoColor;
                break;
            case 3:
                campo = Resources('completed');
                cor = concluidoColor;
                break;
            case 4:
                campo = Resources('completed_late');
                cor = concluidoAtrasoColor;
                break;
            case 5:
                campo = Resources('in_progress');
                cor = andamentoColor;
                break;
            case 6:
                campo = Resources('_return');
                cor = retornoColor;
                break;
            case 7:
                campo = Resources('finished');
                cor = finalizadaColor;
                break;
            case 8:
                campo = Resources('finished_late');
                cor = finalizadaComAtrasoColor;
                break;
            case 9:
                campo = Resources('not_started');
                cor = naoIniciadoColor;
                break;
            default:
                campo = Resources('status');
                cor = 'black';
                break;

        }

        if (data1[0] == undefined) {
            if (dadosFiltrados[i].Acao.Status != 0) {
                data1.push({ name: campo, y: 1, color: cor })
            }
        } else if (data1[data1.length - 1].name == campo) {
            data1[data1.length - 1].y = data1[data1.length - 1].y + 1;
        } else if (dadosFiltrados[i].Acao.Status != 0) {
            data1.push({ name: campo, y: 1, color: cor })
        }
    });

    getGraphPanel2(data1);
}

function atualizarTopFilters() {
    $('.dropdown-toggle').css('font-size', 'xx-small');
    $('.select2').css('width', '50px');
    $('.select2').css('font-size', 'xx-small');
}

function getGraphPanel1(meuDado) {
    Highcharts.chart('panel1', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        credits: {
            enabled: false,
        },
        title: {
            text: false,
        },
        tooltip: {
            pointFormat: '<b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '{point.percentage:.1f} %',
                    distance: -10,
                    style: {
                        //color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'white',
                        color: 'black',
                        fontSize: '10px',
                    },
                    connectorColor: 'silver'
                }
            },
            series: {
                cursor: 'pointer',
                events: {
                    click: function (event) {
                        filterPieForDataTable(event.point.name)
                    }
                }
            }
        },
        series: [{
            name: '',
            data: meuDado
        }]
    });
}

function getGraphPanel2(meuDado) {
    Highcharts.chart('panel2', {
        chart: {
            backgroundColor: 'rgba(255, 255, 255, 0.0)',
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        credits: {
            enabled: false,
        },
        title: {
            text: false,
        },
        tooltip: {
            pointFormat: '<b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '{point.percentage:.1f} %',
                    distance: -10,
                    style: {
                        //color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'white',
                        color: 'black',
                        fontSize: '10px',
                    },
                    connectorColor: 'silver'
                }
            },
            series: {
                cursor: 'pointer',
                events: {
                    click: function (event) {
                        filterPie2ForDataTable(event.point.name)
                    }
                }
            }
        },
        series: [{
            name: '',
            data: meuDado
        }]
    });
}

//filtro de grafico Pie2 para tabela
function filterPie2ForDataTable(name) {
    var dadosAux = "";

    if (dadosPie2 == "") {
        dadosAux = dados;
    } else
        dadosAux = dadosPie2;

    var arrayfilter = FiltraColunasOfClickPie(dadosAux, "_StatusName", name);
    //MountDataTable(arrayfilter);

    var retorno = '';

    if ($('#valor1FiltroPie2 option:selected').text() == "Todos") {
        retorno = name;
    } else {
        retorno = $('#campo1FiltroPie2 option:selected').text() + ': ' + $('#valor1FiltroPie2 option:selected').text() + ' | ' + 'Status' + ': ' + name;

    }


    MountDataTable(arrayfilter);

    $('#spanSubTable').text(retorno);



}

//filtro de grafico Pie para tabela
function filterPieForDataTable(name) {
    var arrayfilter = FiltraColunasOfClickPie(dados, "_StatusName", name);
    MountDataTable(arrayfilter);

}

//monta arrau com o filtro do status passado
function FiltraColunasOfClickPie(array, Atribute, name) {

    Atribute = Atribute == "(vazio)" ? null : Atribute;

    let novoArr = [];

    array.forEach(function (o, c) {

        if (Atribute == "_Quem" || Atribute == "_GrupoCausa" || Atribute == "_CausaGenerica" || Atribute == "_ContramedidaGenerica"
            || Atribute == "UnidadeName" || Atribute == "_StatusName" || Atribute == "Regional"
            || Atribute == "Level1Name" || Atribute == "Level2Name" || Atribute == "Level3Name") {


            if (o.Acao[Atribute] == name) {
                novoArr.push(o);
            }

        } else {

            if (o[Atribute] == name) {
                novoArr.push(o);
            }

        }
    });
    return novoArr;
}

function filterBar1ForDataTable(name, category, idPanel) {
    var retorno = '';
    if (idPanel == 'panel5') {
        var arrayfilter = FilterColumnOfClickBar(dados, $('#campo1Panel5 option:selected').val().replace("Acao.", ""), ($('#campo2Panel5 option:selected').val()).replace("Acao.", ""), category, name);
        retorno = $('#campo1Panel5 option:selected').text() + ': ' + category + ' | ' + $('#campo2Panel5 option:selected').text() + ': ' + name;
    } else if (idPanel == 'panel6') {
        var arrayfilter = FilterColumnOfClickBar(dados, $('#campo1Panel6 option:selected').val().replace("Acao.", ""), ($('#campo2Panel6 option:selected').val()).replace("Acao.", ""), category, name);
        retorno = $('#campo1Panel6 option:selected').text() + ': ' + category + ' | ' + $('#campo2Panel6 option:selected').text() + ': ' + name;
    } else if (idPanel == 'panel4') {
        var arrayfilter = FilterColumnOfClickBar(dados, "", "", "", name);
        retorno = $('#campo1Panel6 option:selected').text() + ': ' + category + ' | ' + $('#campo2Panel6 option:selected').text() + ': ' + name;
    }
    MountDataTable(arrayfilter);

    $('#spanSubTable').text(retorno);
}

function FilterColumnOfClickBar(array, categoryY, categoryX, Atribute, name) {

    Atribute = Atribute == "(vazio)" ? null : Atribute;

    let novoArr = [];
    if (categoryY != "" && categoryY != "" && Atribute != "") {


        array.forEach(function (o, c) {

            if (categoryX == "_Quem" || categoryX == "_GrupoCausa" || categoryX == "_CausaGenerica" || categoryX == "_ContramedidaGenerica"
                || categoryX == "UnidadeName" || categoryX == "_StatusName" || categoryX == "Regional"
                || categoryX == "Level1Name" || categoryX == "Level2Name" || categoryX == "Level3Name" || categoryX == "TipoIndicador") {


                if (categoryX == "TipoIndicador") {

                    if (name == Resources("no_operational_planning"))
                        name = 0;
                    else if (name == Resources("guidelines"))
                        name = 1;
                    else if (name == "Scorecard")
                        name = 2;
                }

                if (o.Acao[categoryX] == name) {

                    if (categoryY == "_Quem" || categoryY == "_GrupoCausa" || categoryY == "_CausaGenerica" || categoryY == "_ContramedidaGenerica"
                        || categoryY == "UnidadeName" || categoryY == "_StatusName" || categoryY == "Regional"
                        || categoryY == "Level1Name" || categoryY == "Level2Name" || categoryY == "Level3Name" || categoryY == "TipoIndicador") {


                        var valueY = o.Acao[categoryY];

                        if (categoryY == "TipoIndicador") {

                            if (valueY == 0)
                                valueY = Resources("no_operational_planning");
                            else if (valueY == 1)
                                valueY = Resources("guidelines");
                            else if (valueY == 2)
                                valueY = "Scorecard";
                        }

                        if (valueY == Atribute) {

                            novoArr.push(o);
                        }
                    } else {
                        if (o[categoryY] == Atribute) {

                            novoArr.push(o);
                        }
                    }
                }
            } else {

                if (o[categoryX] == name) {
                    if (categoryY == "_Quem" || categoryY == "_GrupoCausa" || categoryY == "_CausaGenerica" || categoryY == "_ContramedidaGenerica"
                        || categoryY == "UnidadeName" || categoryY == "_StatusName" || categoryY == "Regional"
                        || categoryY == "Level1Name" || categoryY == "Level2Name" || categoryY == "Level3Name" || categoryY == "TipoIndicador") {


                        var valueY = o.Acao[categoryY];

                        if (categoryY == "TipoIndicador") {

                            if (valueY == 0)
                                valueY = Resources("no_operational_planning");
                            else if (valueY == 1)
                                valueY = Resources("guidelines");
                            else if (valueY == 2)
                                valueY = "Scorecard";
                        }

                        if (valueY == Atribute) {

                            novoArr.push(o);
                        }
                    } else {
                        if (o[categoryY] == Atribute) {

                            novoArr.push(o);
                        }
                    }
                }
            }
        });
    } else {

        switch (name) {
            case "Abertas":
                novoArr = getRegistrosNaoConcluidos(dados);
                break;
            case "Fechada":
                novoArr = getRegistrosConcluidos(dados);
                break;
            case "Estoque":
                novoArr = "";
                break;
        }
    }

    return novoArr;
}

Highcharts.chart('panel4', {
    title: {
        text: false,
    },
    legend: {
        enabled: true,
        verticalAlign: 'top',
        itemStyle: {
            fontSize: '8px',
            fontFamily: 'Verdana, sans-serif'
        }
    },
    credits: {
        enabled: false,
    },
    xAxis: {
        tickInterval: 1,

        categories: categories4,
        labels: {

            style: {
                fontSize: '8px',
                fontFamily: 'Verdana, sans-serif'
            }
        }
    },
    yAxis: {
        enabled: false,
        min: 0,
        title: {
            text: false,
        },
        labels: {

            style: {
                fontSize: '8px',
                fontFamily: 'Verdana, sans-serif'
            }
        }
    },
    plotOptions: {
        series: {
            stacking: 'normal',
            events: {
                click: function (event) {
                    filterBar1ForDataTable(event.point.name, event.point.category)
                }
            }
        },
        bar: {
            dataLabels: {
                enabled: false
            }
        }
    },
    /*
    { name: 'Concluídas', y: 56.33, color: concluidoColor },
    { name: 'Atrasadas', y: 10.38, color: atrasadaColor },
    { name: 'Em Andamento', y: 4.77, color: andamentoColor },
    */
    series: data4
});

Highcharts.chart('panel5', {
    chart: {
        type: 'bar'
    },
    title: {
        text: false,
    },
    subtitle: {
        text: false, //'Source: <a href="https://en.wikipedia.org/wiki/World_population">Wikipedia.org</a>'
    },
    xAxis: {
        tickInterval: 1,

        categories: categories5,
        title: {
            text: null
        },
        labels: {

            style: {
                fontSize: '8px',
                fontFamily: 'Verdana, sans-serif'
            }
        }
    },
    yAxis: {
        min: 0,
        title: {
            text: Resources('number_action'),
            align: 'high',
            style: {
                fontSize: '8px',
                fontFamily: 'Verdana, sans-serif'
            },
            allowDecimals: false
        },
        labels: {
            overflow: 'justify',

            style: {
                fontSize: '8px',
                fontFamily: 'Verdana, sans-serif'
            }

        }
    },
    tooltip: {
        valueSuffix: ''
    },
    plotOptions: {
        series: {
            stacking: 'normal',
            events: {
                click: function (event) {
                    filterBar1ForDataTable(event.point.name, event.point.category)
                }
            }
        },
        bar: {
            dataLabels: {
                enabled: false
            }
        }
    },
    legend: {
        layout: 'horizontal',
        align: 'center',
        verticalAlign: 'bottom',
        floating: false,
        itemStyle: {
            fontSize: '8px',
            fontFamily: 'Verdana, sans-serif'
        }
    },
    credits: {
        enabled: false
    },
    /*
        { name: 'Concluídas', y: 56.33, color: concluidoColor },
        { name: 'Atrasadas', y: 10.38, color: atrasadaColor },
        { name: 'Em Andamento', y: 4.77, color: andamentoColor },
        */
    series: data5
});

Highcharts.chart('panel6', {
    chart: {
        type: 'bar'
    },
    title: {
        text: false,
    },
    subtitle: {
        text: false, //'Source: <a href="https://en.wikipedia.org/wiki/World_population">Wikipedia.org</a>'
    },
    xAxis: {
        tickInterval: 1,
        categories: categories6,
        title: {
            text: null
        },
        labels: {

            style: {
                fontSize: '8px',
                fontFamily: 'Verdana, sans-serif'
            }
        },
        allowDecimals: false
    },
    yAxis: {
        min: 0,
        title: {
            text: Resources('number_action'),
            align: 'high',
            style: {
                fontSize: '8px',
                fontFamily: 'Verdana, sans-serif'
            }
        },
        labels: {
            overflow: 'justify',

            style: {
                fontSize: '8px',
                fontFamily: 'Verdana, sans-serif'
            }

        }
    },
    tooltip: {
        valueSuffix: ' millions'
    },
    plotOptions: {
        series: {
            stacking: 'normal'
        },
        bar: {
            dataLabels: {
                enabled: false
            }
        }
    },
    legend: {
        layout: 'horizontal',
        align: 'center',
        verticalAlign: 'bottom',
        floating: false,
        itemStyle: {
            fontSize: '8px',
            fontFamily: 'Verdana, sans-serif'
        }
    },
    credits: {
        enabled: false
    },
    /*
        { name: 'Concluídas', y: 56.33, color: concluidoColor },
        { name: 'Atrasadas', y: 10.38, color: atrasadaColor },
        { name: 'Em Andamento', y: 4.77, color: andamentoColor },
        */
    series: data6
});

$('#btnpanel5').click(function () {
    //alert($('#campo1Panel5').val() + '|' + $('#valor1Panel5').val() + '|' + $('#campo2Panel5').val() + '|' + $('#valor2Panel5').val());
})

$('#btnpanel6').click(function () {
    // alert($('#campo1Panel6').val() + '|' + $('#valor1Panel6').val() + '|' + $('#campo2Panel6').val() + '|' + $('#valor2Panel6').val());
})

$('#btnTop').click(function () {
    dadosPie2 = "";
    FiltraLinhasComTodos(dados, "", "Todos");
    getDateRange($("input[name='daterange']").val());

    GetDataTable($('#campo1Filtro option:selected').val(),
        $('#valor1Filtro option:selected').text(),
        $('#campo2Filtro option:selected').val(),
        $('#valor2Filtro option:selected').text());

    //GetDataTablePlanejamento($('#campo1Filtro option:selected').val(), $('#valor1Filtro option:selected').text());
})

$('#btnFiltroPie2').click(function () {
    var arrayColuna = [];
    arrayColuna.push($('#campo1FiltroPie2 option:selected').val());
    var arrayValor = [];
    arrayValor.push($('#valor1FiltroPie2 option:selected').text());

    if (arrayValor == "" || arrayValor == "0")
        arrayValor = "Todos";

    dadosPie2 = FiltraLinhasComTodos(dados, arrayColuna, arrayValor);

    geraData2(dadosPie2);
    $('#spanPie2').html($('#campo1FiltroPie2 option:selected').text());
})

$('#btnpanel5').off('click').on('click', function () {

    var nameY = [];
    if ($('#filtroY option:selected').val() != "") {
        nameY.push($('#filtroY option:selected').text());
    }

    filtraDadosParaGerarGraficoPanel5Panel6($('#campo1Panel5 option:selected').val(), $('#campo2Panel5 option:selected').val(), nameY, 'panel5')
    $('#FirstParamPanel5').html($('#campo1Panel5 option:selected').text());
    $('#LastParamPanel5').html($('#campo2Panel5 option:selected').text());
})

$('#btnpanel6').off('click').on('click', function () {
    filtraDadosParaGerarGraficoPanel5Panel6($('#campo1Panel6 option:selected').val(), $('#campo2Panel6 option:selected').val(), [], 'panel6');
    $('#FirstParamPanel6').html($('#campo1Panel6 option:selected').text());
    $('#LastParamPanel6').html($('#campo2Panel6 option:selected').text());
})

function setArrayColvisAtual() {

    if (table) {

        ColvisarrayVisaoAtual_show = [];
        ColvisarrayVisaoAtual_hide = [];
        let ss = [];

        $('#example_wrapper > div.dt-buttons > a.dt-button.buttons-collection.buttons-colvis').click();

        $('body > div.dt-button-collection.fixed.four-column').hide();
        ss = $('.dt-button-collection:eq(0) .buttons-columnVisibility');
        ss.each(function (i, o) {
            if ($(o).hasClass('active')) {
                ColvisarrayVisaoAtual_show.push(i);
            } else {
                ColvisarrayVisaoAtual_hide.push(i);
            }
        }).promise().done(function () {
            $('body > div.dt-button-background').click();
        });
    }
}

function setArrayProjColvisAtual() {

    if (tablePlanejamento) {

        ColvisarrayProjVisaoAtual_show = [];
        ColvisarrayProjVisaoAtual_hide = [];
        let ss = [];

        $('#TablePlanejamento_wrapper > div.dt-buttons > a.dt-button.buttons-collection.buttons-colvis').click();

        $('body > div.dt-button-collection.fixed.four-column').hide();
        ss = $('.dt-button-collection:eq(1) .buttons-columnVisibility');
        ss.each(function (i, o) {
            if ($(o).hasClass('active')) {
                ColvisarrayProjVisaoAtual_show.push(i);
            } else {
                ColvisarrayProjVisaoAtual_hide.push(i);
            }
        }).promise().done(function () {
            $('body > div.dt-button-background').click();
        });
    }
}

$(document).ready(function () {

    $('.defaultFilter').css('color', '#000000');

    $("input[name='daterange']").css('font-size', 'xx-small');
    $("input[name='daterange']").css('width', '140px');
    $("input[name='daterange']").css('height', '25px');

    setTimeout(atualizarTopFilters, 5000);

});

function SaveUserColVis(tabela) {

    setArrayColvisAtual();
    setArrayProjColvisAtual();

    Pa_Quem_Id = getCookie('webControlCookie')[0].split('=')[1];

    let objColvis = {};

    if (tabela == "Acao") {

        ColvisarrayVisaoAtual_show = $.grep(ColvisarrayVisaoAtual_show, function (arr) {
            return (arr != 39 && arr != 40);
        });
        ColvisarrayVisaoAtual_hide = $.grep(ColvisarrayVisaoAtual_hide, function (arr) {
            return (arr != 39 && arr != 40);
        });

        ColvisarrayVisaoAtual_show.push(39);
        ColvisarrayVisaoAtual_show.push(40);

        objColvis = {
            "ColVisShow": ColvisarrayVisaoAtual_show.toString(),
            "ColVisHide": ColvisarrayVisaoAtual_hide.toString(),
            "Pa_Quem_Id": Pa_Quem_Id,
            "Tabela": tabela
        }

        ColvisarrayVisaoUsuario_show = ColvisarrayVisaoAtual_show;
        ColvisarrayVisaoUsuario_hide = ColvisarrayVisaoAtual_hide;

    } else if ("Planejamento") { //Tabela Projetos

        ColvisarrayProjVisaoAtual_show = $.grep(ColvisarrayProjVisaoAtual_show, function (arr) {
            return (arr != 21 && arr != 22);
        });
        ColvisarrayProjVisaoAtual_hide = $.grep(ColvisarrayProjVisaoAtual_hide, function (arr) {
            return (arr != 21 && arr != 22);
        });

        ColvisarrayProjVisaoAtual_show.push(21);
        ColvisarrayProjVisaoAtual_show.push(22);

        objColvis = {
            "ColVisProjShow": ColvisarrayProjVisaoAtual_show.toString(),
            "ColVisProjHide": ColvisarrayProjVisaoAtual_hide.toString(),
            "Pa_Quem_Id": Pa_Quem_Id,
            "Tabela": tabela
        }

        ColvisarrayProjVisaoUsuario_show = ColvisarrayProjVisaoAtual_show;
        ColvisarrayProjVisaoUsuario_hide = ColvisarrayProjVisaoAtual_hide;
    }

    $.post(urlSaveUserColvis, objColvis, function (r) {

        $('body > div.dt-button-background').click();

        if (r == "") {
            openMessageModal("Colunas salvas!", Resources("columns_saved_successfully"));
        } else {
            openMessageModal(Resources("error_saving"));
        }
    });
}

function GetFiltrosDeColunas() {

    filtrosDeColunas = [];

    $('#example_wrapper > div.DTFC_ScrollWrapper > div.dataTables_scroll > div.dataTables_scrollHead > div > table > thead > tr th input[type="text"]').each(function (a) {
        if ($(this).val() != "") {
            filtrosDeColunas.push({ Key: $(this).parent().text(), Val: $(this).val() });
        }
    });
}

function SetFiltrosDeColunas() {

    if (filtrosDeColunas.length > 0) {

        filtrosDeColunas.forEach(function (o, c) {

            $('#example_wrapper > div.DTFC_ScrollWrapper > div.dataTables_scroll > div.dataTables_scrollHead > div > table > thead > tr th input').each(function (a) {

                if ($(this).parent().text() == o.Key) {
                    $(this).val(o.Val);
                    table.column(a).search(o.Val).draw();
                }
            });
        });
    }
}

function GetPropertyRealName(propertyName) {

    switch (propertyName) {

        case "Diretoria":
            return Resources("directorship");
        case "Dimensao":
            return Resources("dimension");
        case "Objetivo":
            return Resources("guideline");
        case "IndicadoresDiretriz":
            return Resources("indicators_guideline");
        case "TemaAssunto":
            return Resources("theme_subject");
        case "Gerencia":
            return Resources("management");
        case "Coordenacao":
            return Resources("coordination");
        case "TipoProjeto":
            return Resources("type_of_project");
        case "TemaProjeto":
            return Resources("project_theme");
        case "Iniciativa":
            return Resources("project_initiative");
        case "Acao._GrupoCausa":
            return Resources("group_cause");
        case "Acao._CausaGenerica":
            return Resources("generic_cause");
        case "Acao._ContramedidaGenerica":
            return Resources("generic_action");
        case "Acao.UnidadeName":
            return Resources("unit");
        case "Acao._Quem":
            return Resources("who");
        case "Acao._StatusName":
            return Resources("status");
        case "Acao.Regional":
            return Resources("regional");
        case "Acao.Level1Name":
            return Resources("indicator");
        case "Acao.Level2Name":
            return Resources("monitoring");
        case "Acao.Level3Name":
            return Resources("task");
        case "Acao.TipoIndicador":
            return Resources("indicator_type");
        default:
            return "";
    }
}