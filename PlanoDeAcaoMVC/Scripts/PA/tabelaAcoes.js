var table;
var btnDetalhes = '<button type="button" class="details btn btn-default btn-sm" style="text-align: left; width:150px !important"><span title="Detalhes" style="cursor:pointer" class="glyphicon glyphicon-list-alt"></span>&nbsp' + Resources('details') + '</button> ';
var btnNovoTatico = '<button type="button" class="btnNovoTatico showAsEstrategy btn btn-default btn-sm" style="text-align: left; width:150px !important"><span title="Novo Planejamento Tático para este Planejamento Estratégico" style="cursor:pointer" class="glyphicon glyphicon-tag"></span>&nbsp' + Resources('new_tactic') + '</button>';
var btnNovoOperacional = '<button type="button" class="btnNovoOperacional btn btn-default btn-sm" style="text-align: left; width:150px !important"><span title="Novo Planejamento Operacional Vinculado ao Planejamento Tático e Estratégico" style="cursor:pointer" class="glyphicon glyphicon-tags"></span>&nbsp' + Resources('new_action') + '</button> ';
var btnAcompanhamento = '<button type="button" class="btnAcompanhamento btn btn-default btn-sm" style="text-align: left; width:150px !important"><span title="Acompanhamento" style="cursor:pointer" class="glyphicon glyphicon-book"></span>&nbsp' + Resources('accompaniment') + '</button> ';
var btnEditarPlanejamento = '<button type="button" class="btnEditarPlanejamento btn btn-default btn-sm" style="text-align: left; width:150px !important"><span title="EditarPlanejamento" style="cursor:pointer" class="glyphicon glyphicon-book"></span>&nbsp' + Resources('edit_planning') + '</button> ';
var btnEditarPlanejamentoDisabled = '<button disabled type="button" class="btnEditarPlanejamento btn btn-default btn-sm" style="text-align: left; width:150px !important"><span title="EditarPlanejamento" style="cursor:pointer" class="glyphicon glyphicon-book"></span>&nbsp' + Resources('edit_planning') + '</button>';

function GetDataTable(campo, filtro, campo2, filtro2) {

    $.get(urlGetPlanejamentoAcaoRange, enviar, function (r) {

        dadosAtivos = r.filter(x => x.IsActive == true && (x.IsActive_Tatico == true || x.Tatico_Id === 0) && (x.Acao.Id === 0 || x.Acao.IsActive == true));
        dadosInativos = r.filter(x => x.IsActive === false || (x.IsActive_Tatico === false && x.Tatico_Id > 0) || (x.Acao.Id > 0 && x.Acao.IsActive === false));

        Inicializar(campo, filtro, campo2, filtro2, dadosAtivos);

        exibindoAtivos = true;

    });
}

function Inicializar(campo, filtro, campo2, filtro2, r) {

    dadosfilter = r;
    dados = Object.assign([], r);

    let usuarioIscorporativo = false;

    if (usuarioIscorporativo) {
        dados = $.grep(dados, function (a, b) { return a.Acao.TipoIndicador == 2 });
        $('.optionAlternativeRole').remove();
    }

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

    $('#btnpanel5').click();
    $('#btnpanel6').click();

    json = dados;

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

        });
    }

    $('#spanSubTable').text(Resources("all_tasks_filter"));

    distinctFilter(dados, $('#campo1FiltroPie2').val(), 'valor1FiltroPie2');
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
                }
            }
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
                var bgColorStatus = "";
                var bgColorPrazo = "";

                if (data.Acao.Status == 2) {
                    bgColorStatus = "grey";
                    bgColorPrazo = "rgb(126, 194, 253)";
                } else if (data.Acao.Status == 5) {
                    bgColorStatus = "#ADD8E6";
                } else if (data.Acao.Status == 5 && data.Acao.StatusName.indexOf('Atrasado') > -1) {
                    bgColorStatus = "#458fa8";
                } else if (data.Acao.Status == 3) {
                    bgColorPrazo = "rgb(126, 194, 253)";
                    bgColorStatus = "cyan";
                } else if (data.Acao.Status == 4) {
                    bgColorPrazo = "rgb(126, 194, 253)";
                    bgColorStatus = "steelblue";
                } else if (data.Acao.Status == 9) {
                    bgColorStatus = "#E0EEEE";
                }

                $(row.cells[38]).css("background", bgColorStatus);

                /*Prazo*/
                if (data.Acao._Prazo.match(/\d+/g)) {
                    let numero = data.Acao._Prazo.split(" ")[0];

                    if (numero == 0) {
                        bgColorPrazo = "rgba(253, 245, 154, 0.67)";
                    } else if (isPositiveInteger(numero)) {
                        bgColorPrazo = "#90EE90"
                    } else {
                        bgColorPrazo = "rgb(250, 128, 114)";
                    }
                }
                else if (data.Acao._Prazo.indexOf('-') > -1 && data.Acao._Prazo.indexOf(' Dias') > -1) {
                    bgColorPrazo = "rgb(250, 128, 114)";
                }

                $(row.cells[39]).css("background", bgColorPrazo);

            } catch (e) { }

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

function tableDraw() {
    setTimeout(function () {
        table.draw();
    }, 50);
}

function objectToArr(myObj) {
    let array = $.map(myObj, function (value, index) {
        return [value];
    });

    return array;
}

function isPositiveInteger(n) {
    return n == "0" || ((n | 0) > 0 && n % 1 == 0);
}

function tableAcaoRedrown() {
    setTimeout(function () {
        table.draw();
    }, 500);
}