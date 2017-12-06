

    //var urlGetPlanejamentoAcao = 'http://192.168.25.200/PlanoAcao/api/Pa_Planejamento/GetPlanejamentoAcao';

    //var urlGetPlanejamentoAcao = 'http://mtzsvmqsc/PlanoDeAcao/api/Pa_Planejamento/GetPlanejamentoAcao';

    //var urlGetPlanejamentoAcao = 'http://localhost:59907/api/Pa_Planejamento/GetPlanejamentoAcao';
    var urlGetPlanejamentoAcao = 'http://192.168.25.200/PlanoAcao/api/Pa_Planejamento/GetPlanejamentoAcaoRange';



var dados = [];
var dadosPie2 = [];
function GetDataTable(campo, filtro) {
    $.get(urlGetPlanejamentoAcao, enviar, function (r) {

        dados = r;
        //https://grtsolucoes.atlassian.net/browse/JBS-110
        let usuarioIscorporativo = false//getRole("Admin");

        if (usuarioIscorporativo) {
            dados = $.grep(dados, function (a, b) { return a.Acao.TipoIndicador == 2 });
            $('.optionAlternativeRole').remove();
        }

        //fim https://grtsolucoes.atlassian.net/browse/JBS-110
        if (campo == "") {
            campo = "Todos";
            $("campo1FiltroPie2 select").val("Todos");

        }


        if (campo != "Todos" && filtro != "Todos") {
            dados = FiltraLinhas(dados, [campo], [filtro]);
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

        MountDataTable(json);

        distinctFilter(dados, $('#campo1FiltroPie2').val(), 'valor1FiltroPie2')
    });
}

function MountDataTable(json) {
    $('#example').DataTable({
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
            { "mData": "Gerencia" },
            { "mData": "Coordenacao" },
            { "mData": "Iniciativa" },
            { "mData": "IndicadoresDeProjeto" },
            { "mData": "ObjetivoGerencial" },
            { "mData": "_ValorDe" },
            { "mData": "ValorPara" },
            { "mData": "_DataInicio" },
            { "mData": "_DataFim" },
            { "mData": "Responsavel_Projeto_Quem.Name" },
            { "mData": "Acao.Regional" },
            { "mData": "Acao.UnidadeName" },
            { "mData": "Acao.Level1Name" },
            { "mData": "Acao.Level2Name" },
            { "mData": "Acao.Level3Name" },
            { "mData": "IndicadoresDeProjeto" }, // VER INDICADOR OPERACIONAL*
            { "mData": "Acao._CausaGenerica" },
            { "mData": "Acao._GrupoCausa" },
            { "mData": "Acao._ContramedidaGenerica" },
            { "mData": "Acao.ContramedidaEspecifica" },
            { "mData": "Acao.CausaEspecifica" },
            { "mData": "Acao._Quem" },
            { "mData": "Acao._QuandoInicio" },
            { "mData": "Acao._QuandoFim" },
            { "mData": "TemaAssunto" },
            { "mData": "Acao.PraQue" },
            { "mData": "Acao.QuantoCusta" },
            { "mData": "Acao._StatusName" },
            { "mData": "Acao._Prazo" }
        ],

        'aoColumnDefs': [
            { "sTitle": "Diretoria", "aTargets": [0], "width": "100px" },
            { "sTitle": "Missão", "aTargets": [1], "width": "200px" },
            { "sTitle": "Visão", "aTargets": [2], "width": "200px" },
            { "sTitle": "Dimensão", "aTargets": [3], "width": "50px" },
            { "sTitle": "Diretrizes", "aTargets": [4], "width": "200px" }, // ver diretriz
            { "sTitle": "Indicadores Diretriz", "aTargets": [5], "width": "300px" },
            { "sTitle": "Responsável pela Diretriz", "aTargets": [6], "width": "50px" },
            { "sTitle": "Gerência", "aTargets": [7], "width": "100px" },
            { "sTitle": "Coordenação", "aTargets": [8], "width": "100px" },
            { "sTitle": "Iniciativa", "aTargets": [9], "width": "200px" },
            { "sTitle": "Indicadores de Projeto/Iniciativa", "aTargets": [10], "width": "100px" },
            { "sTitle": "Objetivo Gerencial", "aTargets": [11], "width": "100px" },
            { "sTitle": "Valor de", "aTargets": [12], "width": "50px" },
            { "sTitle": "Valor para", "aTargets": [13], "width": "50px" },
            { "sTitle": "Data Início", "aTargets": [14], "width": "50px" },
            { "sTitle": "Data Fim", "aTargets": [15], "width": "50px" },
            { "sTitle": "Responsável pelo Projeto/Iniciativa", "aTargets": [16], "width": "50px" },
            { "sTitle": "Regional", "aTargets": [17], "width": "50px" },
            { "sTitle": "Unidade", "aTargets": [18], "width": "50px" },
            { "sTitle": "Indicador SGQ", "aTargets": [19], "width": "100px" },
            { "sTitle": "Monitoramento SGQ", "aTargets": [20], "width": "100px" },
            { "sTitle": "Tarefa SGQ", "aTargets": [21], "width": "100px"  },
            { "sTitle": "Indicadores Operacional", "aTargets": [22], "width": "100px" }, // ver indicador operacional*
            { "sTitle": "Causa Genérica", "aTargets": [23], "width": "200px" },
            { "sTitle": "Grupo Causa", "aTargets": [24], "width": "200px" },
            { "sTitle": "Ação Genérica", "aTargets": [25], "width": "100px" },
            { "sTitle": "Causa Específica", "aTargets": [26], "width": "100px" },
            { "sTitle": "Ação Específica", "aTargets": [27], "width": "100px" },
            { "sTitle": "Quem", "aTargets": [28], "width": "200px" },
            { "sTitle": "Quando (Início)", "aTargets": [29], "width": "50px" },
            { "sTitle": "Quando (Fim)", "aTargets": [30], "width": "50px" },
            { "sTitle": "Como Pontos Importantes", "aTargets": [31], "width": "200px" },
            { "sTitle": "Pra que", "aTargets": [32], "width": "200px" },
            { "sTitle": "Quanto custa", "aTargets": [33], "width": "50px" },
            { "sTitle": "Status", "aTargets": [34], "width": "50px" },
            { "sTitle": "Prazo", "aTargets": [35], "width": "50px" }
        ],

        responsive: true,
        //"bFilter": false,
        "scrollY": 370,
        "scrollX": 500,
        "bLengthChange": false,

        dom: 'Bfrtip',
        buttons: [
            { extend: 'excel', text: 'Excel' },
            { extend: 'colvis', text: 'Colunas' }
        ],

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

    $('#virtualBody').css('width', '100%');

    //Filtros por coluna

    $('.dataTable thead th').each(function (i) {
        var title = $('.dataTable thead th').eq($(this).index()).text();
        $(this).html(title + '<br><input type="text" style="font-size:xx-small; color: #555; text-align:center; width:50px" placeholder=" ' + title + '" data-index="' + i + '" />');
    });

    $('.dataTable thead th').css('text-align', 'center');

    $('.dataTables_filter').hide();

    // DataTable
    var table = $('.dataTable').DataTable();

    // Filter event handler
    $(table.table().container()).on('keyup', 'thead input', function () {
        table
            .column($(this).data('index'))
            .search(this.value)
            .draw();
    });

    table.draw();

}

/**
 *
 * [{Nome: 'Renan', Idade: 25}, {Nome: 'Leonardo', Idade: 23}, {Nome: 'Lucas', Idade: 25}] array
 * ['Nome','Idade'] arrColuna
 */
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

/**
 *
 * [{Nome: 'Renan', Idade: 25}, {Nome: 'Leonardo', Idade: 23}, {Nome: 'Lucas', Idade: 25}] array
 * ['Nome','Idade'] arrColuna
 * ['Renan',25] arrValue
 */
function FiltraLinhas(array, arrColuna, arrValue) {

    let novoArr = []

    array.forEach(function (o, c) {

        var flag = true;

        ///////////

        if (arrColuna == "_Quem" || arrColuna == "_GrupoCausa" || arrColuna == "_CausaGenerica" || arrColuna == "_ContramedidaGenerica"
            || arrColuna == "UnidadeName" || arrColuna == "_StatusName" || arrColuna == "Regional"
            || arrColuna == "Level1Name" || arrColuna == "Level2Name" || arrColuna == "Level3Name" || arrColuna == "TipoIndicador") {

            if (arrColuna == "TipoIndicador" && arrValue != "Todos") {

                arrValueAux = [];
                if (arrValue == "Outros")
                    arrValueAux.push(1);
                else if (arrValue == "Score")
                    arrValueAux.push(2);



                arrColuna.forEach(function (oo, cc) {

                    if (o.Acao[oo] != arrValueAux[cc]) {
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

        if (flag) {
            novoArr.push(o)
        }
    });

    return novoArr;
}

function FiltraLinhasComTodos(array, arrColuna, arrValue) {

    let novoArr = []

    array.forEach(function (o, c) {
        var flag = true;
        if (arrValue != "Todos" && arrValue != "Todas") {
            if (arrColuna == "_Quem" || arrColuna == "_GrupoCausa" || arrColuna == "_CausaGenerica" || arrColuna == "_ContramedidaGenerica"
                || arrColuna == "UnidadeName" || arrColuna == "_StatusName" || arrColuna == "Regional"
                || arrColuna == "Level1Name" || arrColuna == "Level2Name" || arrColuna == "Level3Name" || arrColuna == "TipoIndicador") {

                if (arrColuna == "TipoIndicador" && arrValue != 0) {

                    arrValueAux = [];
                    if (arrValue == "Outros")
                        arrValueAux.push(1);
                    else if (arrValue == "Score")
                        arrValueAux.push(2);

                    arrColuna.forEach(function (oo, cc) {

                        if (o.Acao[oo] != arrValueAux[cc]) {
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



var atrasadaColor = '#ff6666';
var concluidoColor = 'lightgreen';
var andamentoColor = 'lightblue';

var dataInicio;
var dataFim;

/*
var data1 = [
    { name: 'Concluídas', y: 22, color: concluidoColor },
    { name: 'Atrasadas', y: 10, color: atrasadaColor },
    { name: 'Em Andamento', y: 18, color: andamentoColor },
];
*/

var categories2 = ['Camila', 'Miriã', 'Ana', 'Adão', 'Diego'];

var data2 = [{
    name: 'Concluídas',
    data: [5, 3, 4, 7, 2],
    color: concluidoColor
}, {
    name: 'Atrasadas',
    data: [2, 2, 3, 2, 1],
    color: atrasadaColor
}, {
    name: 'Em Andamento',
    data: [3, 4, 4, 2, 5],
    color: andamentoColor
}];

var data3 = [{
    id: 'A',
    name: 'Atrasadas',
    color: atrasadaColor
}, {
    id: 'B',
    name: 'Concluídas',
    color: concluidoColor
}, {
    id: 'O',
    name: 'Em Andamento',
    color: andamentoColor
}, {
    name: 'Qualidade',
    parent: 'A',
    value: 5
}, {
    name: 'Operação',
    parent: 'A',
    value: 3
}, {
    name: 'Qualidade',
    parent: 'B',
    value: 4
}, {
    name: 'Operação',
    parent: 'B',
    value: 10
}, {
    name: 'Qualidade',
    parent: 'O',
    value: 1
}, {
    name: 'Operação',
    parent: 'O',
    value: 3
}];

var categories4 = [];

var data4 = [{
    type: 'column',
    name: 'Abertas',
    data: [],
    color: andamentoColor
}, {
    type: 'column',
    name: 'Fechada',
    data: [],
    color: concluidoColor
}, {
    type: 'spline',
    name: 'Estoque',
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
//    [{
//    name: 'Atrasado',
//    data: [4, 2, 6],
//    color: atrasadaColor
//}, {
//    name: 'Concluido',
//    data: [5, 4, 1],
//    color: concluidoColor
//}, {
//    name: 'Em Andamento',
//    data: [6, 2, 4],
//    color: andamentoColor
//}];

var categories6 = []; //categories2

var data6 = [];
//[{
//    name: 'Atrasado',
//    data: [4, 2, 6.2, 2],
//    color: atrasadaColor
//}, {
//    name: 'Concluido',
//    data: [5, 4, 1, 4, 2],
//    color: concluidoColor
//}, {
//    name: 'Em Andamento',
//    data: [6, 2, 4, 1, 0],
//    color: andamentoColor
//}];

var json = FiltraColunas(dados, ["Diretoria",
    "Missao",
    "Visao",
    "Dimensao",
    "Objetivo",
    "IndicadoresDiretriz",
    "Responsavel_Diretriz_Quem.Name",
    "Gerencia",
    "Coordenacao",
    "Iniciativa",
    "IndicadoresDeProjeto",
    "ObjetivoGerencial",
    "_ValorDe",
    "ValorPara",
    "_DataInicio",
    "_DataFim",
    "Responsavel_Projeto_Quem.Name",
    "Acao.Regional",
    "Acao.UnidadeName",
    "Acao.Level1Name",
    "Acao.Level2Name",
    "Acao.Level3Name",
    "IndicadoresDeProjeto",
    "Acao._CausaGenerica",
    "Acao._GrupoCausa",
    "Acao._ContramedidaGenerica",
    "Acao.ContramedidaEspecifica",
    "Acao.CausaEspecifica",
    "Acao._Quem",
    "Acao._QuandoInicio",
    "Acao._QuandoFim",
    "TemaAssunto",
    "Acao.PraQue",
    "Acao.QuantoCusta",
    "Acao._StatusName",
    "Acao._Prazo"]);



//    //Graficos Instancia

//    //Cria Arr de dados e instancia HC para Grafico Estoque, panel4
//function graficoEstoque() {


//    dadosEstoque = [];

//    jQuery.each(dados, function (i, val) {
//        if (dados[i].Acao.AddDate)
//            dadosEstoque.push(dados[i]);
//    });
//        //MOCK Acompanhamento
//        dadosEstoque.forEach(function (o, c) {
//            //if (c < 5)
//            //    o.Acao['Acompanhamento'] = { AddDate: "2017-11-08T14:43:19.3044096" }
//            //else if (c > 5 && c < 8)
//            //    o.Acao['Acompanhamento'] = { AddDate: "2017-10-08T14:43:19.3044096" }
//            //else
//            //    o.Acao['Acompanhamento'] = { AddDate: "2017-09-08T14:43:19.3044096" }

//            //o.Acao['Acompanhamento'] = { AddDate: '' + o.Acao.AddDate.substring(0, 4) + '-' + o.Acao.AddDate.substring(8, 10) + '-' + o.Acao.AddDate.substring(5, 7) + '' };

//            o.Acao['Acompanhamento'] = { AddDate: o.Acao.AddDate };

//            console.log(o.Acao['Acompanhamento']);

//        })
//        //FIM MOCK Acompanhamento

//        let groups = _.groupBy(dadosEstoque, function (o) {
//            return moment(o.Acao.Acompanhamento.AddDate.substring(0, 7) + '-01' + o.Acao.Acompanhamento.AddDate.substring(10, 25)).startOf('mounth').format();
//        });

//        let categoriesArr = _.map(groups, function (group, day) {

//            switch (day.substring(3, 5)) {
//                case '01':
//                    retorno = "Janeiro";
//                    break;
//                case '02':
//                    retorno = "Fevereiro";
//                    break;
//                case '03':
//                    retorno = "Março";
//                    break;
//                case '04':
//                    retorno =  "Abril";
//                    break;
//                case '05':
//                    retorno =  "Maio";
//                    break;
//                case '06':
//                    retorno =  "Junho";
//                    break;
//                case '07':
//                    retorno =  "Julho";
//                    break;
//                case '08':
//                    retorno =  "Agosto";
//                    break;
//                case '09':
//                    retorno =  "Setembro";
//                    break;
//                case '10':
//                    retorno =  "Outubro";
//                    break;
//                case '11':
//                    retorno =  "Novembro";
//                    break;
//                case '12':
//                    retorno =  "Dezembro";
//                    break;
//            }

//            return retorno + ' de ' + day.substring(6, 10);
//        })

//        let serieArrFinal = [{
//            type: 'column',
//            name: 'Abertas',
//            data: _.map(groups, function (group, day) {
//                return getRegistrosNaoConcluidos(group).length
//            }),
//            color: andamentoColor
//        }, {
//            type: 'column',
//            name: 'Fechada',
//            data: _.map(groups, function (group, day) {
//                return getRegistrosConcluidos(group).length
//            }),
//            color: concluidoColor
//        }, {
//            type: 'spline',
//            name: 'Estoque',
//            data: _.map(groups, function (group, day) {
//                return getRegistrosNaoConcluidos(group).length - getRegistrosConcluidos(group).length
//            }),
//            color: atrasadaColor,
//            marker: {
//                lineWidth: 2,
//                lineColor: atrasadaColor,
//                fillColor: atrasadaColor
//            }
//        }];

//        makeChart('panel4', categoriesArr, serieArrFinal, 'column', '', {
//            //plotOptions: {
//            //    series: {
//            //        //stacking: 'normal',
//            //        events: {
//            //            click: function (event) {
//            //                filterBar1ForDataTable(event.point.name, event.point.category)
//            //            }
//            //        }
//            //    },
//            //    bar: {
//            //        dataLabels: {
//            //            enabled: false
//            //        }
//            //    }
//            //},
//        })

//    }

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
            if ($('#valor1Panel5 option:selected').text() == "Todas")
                categoriesArr = MapeiaValorParaHC(dados, categoriesFilterVal).filter(onlyUnique);
            else
                categoriesArr.push($('#valor1Panel5 option:selected').text());
        } else if (id == 'panel6') {
            if ($('#valor1Panel6 option:selected').text() == "Todas")
                categoriesArr = MapeiaValorParaHC(dados, categoriesFilterVal).filter(onlyUnique);
            else
                categoriesArr.push($('#valor1Panel6 option:selected').text());
        }


        var categoriesArrOrder = [];

        categoriesArr.sort();

        var serieArrFinal = filtraAgrupaXY(categoriesArr, seriesFilterVal, categoriesFilterVal, dados, true, id)

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

//FIM Graficos Instancia

//Aux

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
        return o.Acao._StatusName && (o.Acao._StatusName.indexOf('Concluído') >= 0 || o.Acao._StatusName.indexOf('Finalizada')) >= 0;
    });
}
//Busca no arr de dados registros não concluídos, filtro = return
function getRegistrosNaoConcluidos(arr) {
    return _.filter(arr, function (o) {
        return !(o.Acao._StatusName && (o.Acao._StatusName.indexOf('Concluído') >= 0 || o.Acao._StatusName.indexOf('Finalizada')) >= 0 && o.Acao._StatusName.indexOf('Cancelada') < 0);
    });
}
//Instancia HighCharts em um grafico padrão, aceita options para sobrescrita
function makeChart(id, categoriesArr, seriesArr, type, yAxisTitle, optionsDef) {
    let options = {
        chart: {
            type: type
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
            min: 0,
            //max: 30,
            allowDecimals: false,
            title: {
                text: yAxisTitle,
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
            return o[propArray[0]][propArray[1]];
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
        if ($('#valor2Panel5 option:selected').text() == "Todas")
            filtroEixoX = MapeiaValorParaHC(dados, seriesFilter).filter(onlyUnique);
        else {
            filtroEixoX.push($('#valor2Panel5 option:selected').text());

        }
    } else if (id == 'panel6'){
        if ($('#valor2Panel6 option:selected').text() == "Todas")
            filtroEixoX = MapeiaValorParaHC(dados, seriesFilter).filter(onlyUnique);
        else {
            filtroEixoX.push($('#valor2Panel6 option:selected').text());

        }
    }


    let seriesAux = filtroEixoX;
    let serieArrFinal = []
    seriesAux.forEach(function (o, c) {
        var serieData = {};
        serieData["name"] = o;
        serieData["data"] = [];
        categoriesArr.forEach(function (oo, cc) {
            serieData["data"].push($.grep(dados, function (e) {

                var retornoSeries;
                var retornoCategorias;

                var propArray = seriesFilter.split('.');
                if (propArray.length == 2) {
                    retornoSeries = e[propArray[0]][propArray[1]];
                } else {
                    retornoSeries = e[seriesFilter];
                }

                var propArray = categoriesFilter.split('.');
                if (propArray.length == 2) {
                    retornoCategorias = e[propArray[0]][propArray[1]];
                } else {
                    retornoCategorias = e[categoriesFilter];
                }

                return retornoSeries == o && retornoCategorias == categoriesArr[cc];
            }).length)
        })
        serieArrFinal.push(serieData);
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
    } else
        return serieArrFinal
}

//Fim Aux

//DATEPICKER

//GLOBAL
var enviar = {};
var start = moment().add('month', -1);
var end = moment();

//DatePicker Config / Instance
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
        "applyLabel": "Aplicar",
        "cancelLabel": "Cancelar",
        "fromLabel": "de",
        "toLabel": "até",
        "customRangeLabel": "Customizado",
        "weekLabel": "W",
        "daysOfWeek": [
            "Dom",
            "Seg",
            "Ter",
            "Qua",
            "Qui",
            "Sex",
            "Sab"
        ],
        "monthNames": [
            "Janeiro",
            "Fevereiro",
            "Março",
            "Abril",
            "Maio",
            "Junho",
            "Julho",
            "Agosto",
            "Setembro",
            "Outubro",
            "Novembro",
            "Dezembro"
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
                "applyLabel": "Aplicar",
                "cancelLabel": "Cancelar",
                "customRangeLabel": "Seleção",
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



var btnOrderFilter = "";
var option = "";
var campo1Panel5Selected = "";
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
            titleSpan = "Ordem por Valor";
            break;
        case "Asc":
            titleSpan = "Ordem Alfabética";
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

    $('#valor2Panel5').append($("<option></option>").attr("value", 0).text("Todas"));
    $('#valor1FiltroPie2').append($("<option></option>").attr("value", 0).text("Todas"));
    $('#valor1Panel5').append($("<option></option>").attr("value", 0).text("Todas"));
    $('#valor1Panel6').append($("<option></option>").attr("value", 0).text("Todas"));
    $('#valor2Panel6').append($("<option></option>").attr("value", 0).text("Todas"));


    $.each(retorno, function (key, value) {
        if ($('#campo1Filtro option:selected').val() == "TipoIndicador" ||
            $('#campo1FiltroPie2 option:selected').val() == "TipoIndicador" ||
            $('#campo1Panel5 option:selected').val() == "TipoIndicador" ||
            $('#campo2Panel5 option:selected').val() == "TipoIndicador" ||
            $('#campo1Panel6 option:selected').val() == "TipoIndicador" ||
            $('#campo2Panel6 option:selected').val() == "TipoIndicador"
         ) {
            if (value == 0)
                value = "Todos"
            else if (value == 1)
                value = "Outros";
            else if (value == 2)
                value = "Score";
        }
        if (value != null) {

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
                campo = 'Sem Status';
                cor = '#fdffb2';
                break;
            case 1:
                campo = 'Atrasado';
                cor = atrasadaColor;
                break;
            case 2:
                campo = 'Cancelado';
                cor = 'green';
                break;
            case 3:
                campo = 'Concluído';
                cor = concluidoColor;
                break;
            case 4:
                campo = 'Concluído com atraso';
                cor = 'pink';
                break;
            case 5:
                campo = 'Em Andamento';
                cor = andamentoColor;
                break;
            case 6:
                campo = 'Retorno';
                cor = 'pink';
                break;
            case 7:
                campo = 'Finalizada';
                cor = 'pink';
                break;
            case 8:
                campo = 'Finalizada com atraso';
                cor = 'pink';
                break;
            default:
                campo = 'Status';
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
                campo = 'Sem Status';
                cor = '#fdffb2';
                break;
            case 1:
                campo = 'Atrasado';
                cor = atrasadaColor;
                break;
            case 2:
                campo = 'Cancelado';
                cor = 'green';
                break;
            case 3:
                campo = 'Concluído';
                cor = concluidoColor;
                break;
            case 4:
                campo = 'Concluído com atraso';
                cor = 'pink';
                break;
            case 5:
                campo = 'Em Andamento';
                cor = andamentoColor;
                break;
            case 6:
                campo = 'Retorno';
                cor = 'pink';
                break;
            case 7:
                campo = 'Finalizada';
                cor = 'pink';
                break;
            case 8:
                campo = 'Finalizada com atraso';
                cor = 'pink';
                break;
            default:
                campo = 'Status';
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
    MountDataTable(arrayfilter);

}

//filtro de grafico Pie para tabela
function filterPieForDataTable(name) {
    var arrayfilter = FiltraColunasOfClickPie(dados, "_StatusName", name);
    MountDataTable(arrayfilter);

}

//monta arrau com o filtro do status passado
function FiltraColunasOfClickPie(array, Atribute, name) {

    let novoArr = [];

    array.forEach(function (o, c) {

        if (Atribute == "_Quem" || Atribute == "_GrupoCausa" || Atribute == "_CausaGenerica" || Atribute == "_ContramedidaGenerica"
            || Atribute == "UnidadeName" || Atribute == "_StatusName" || Atribute == "Regional"
            || Atribute == "Level1Name" || Atribute == "Level2Name" || Atribute == "Level3Name" ) {


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
        var arrayfilter = FilterColumnOfClickBar(dados, $('#campo1Panel5 option:selected').val(), ($('#campo2Panel5 option:selected').val()).replace("Acao.", ""), category, name);
        retorno = $('#campo1Panel5 option:selected').text() + ': ' + category + ' | ' + $('#campo2Panel5 option:selected').text() + ': ' + name;
    } else if (idPanel == 'panel6') {
        var arrayfilter = FilterColumnOfClickBar(dados, $('#campo1Panel6 option:selected').val(), ($('#campo2Panel6 option:selected').val()).replace("Acao.", ""), category, name);
        retorno = $('#campo1Panel6 option:selected').text() + ': ' + category + ' | ' + $('#campo2Panel6 option:selected').text() + ': ' + name;
    } else if (idPanel == 'panel4') {
        var arrayfilter = FilterColumnOfClickBar(dados, "", "", "", name);
        retorno = $('#campo1Panel6 option:selected').text() + ': ' + category + ' | ' + $('#campo2Panel6 option:selected').text() + ': ' + name;
    }
    MountDataTable(arrayfilter);

    $('#spanSubTable').text(retorno);
}

function FilterColumnOfClickBar(array, categoryY, categoryX, Atribute, name) {

    let novoArr = [];
    if (categoryY != "" && categoryY != "" && Atribute != "") {


        array.forEach(function (o, c) {

            if (categoryX == "_Quem" || categoryX == "_GrupoCausa" || categoryX == "_CausaGenerica" || categoryX == "_ContramedidaGenerica"
                || categoryX == "UnidadeName" || categoryX == "_StatusName" || categoryX == "Regional"
                || categoryX == "Level1Name" || categoryX == "Level2Name" || categoryX == "Level3Name") {

                if (o.Acao[categoryX] == name) {

                    if (o[categoryY] == Atribute) {

                        novoArr.push(o);
                    }
                }
            } else {

                if (o[categoryX] == name) {
                    if (o[categoryY] == Atribute) {
                        novoArr.push(o);
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
            text: 'Número de Ações',
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
            text: 'Número de Ações',
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

    GetDataTable($('#campo1Filtro option:selected').val(), $('#valor1Filtro option:selected').text());
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



//Celso
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


$(document).ready(function () {

    console.log("ready!");
    GetDataTable();



    $('.defaultFilter').css('color', '#000000');

    $("input[name='daterange']").css('font-size', 'xx-small');
    $("input[name='daterange']").css('width', '140px');
    $("input[name='daterange']").css('height', '25px');

    setTimeout(atualizarTopFilters, 100);




});

