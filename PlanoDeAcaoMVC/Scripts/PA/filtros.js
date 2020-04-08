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

    var arrayfilter;

    if (idPanel == 'panel5') {
        arrayfilter = FilterColumnOfClickBar(dados, $('#campo1Panel5 option:selected').val().replace("Acao.", ""), ($('#campo2Panel5 option:selected').val()).replace("Acao.", ""), category, name);
        retorno = $('#campo1Panel5 option:selected').text() + ': ' + category + ' | ' + $('#campo2Panel5 option:selected').text() + ': ' + name;
    } else if (idPanel == 'panel6') {
        arrayfilter = FilterColumnOfClickBar(dados, $('#campo1Panel6 option:selected').val().replace("Acao.", ""), ($('#campo2Panel6 option:selected').val()).replace("Acao.", ""), category, name);
        retorno = $('#campo1Panel6 option:selected').text() + ': ' + category + ' | ' + $('#campo2Panel6 option:selected').text() + ': ' + name;
    } else if (idPanel == 'panel4') {
        arrayfilter = FilterColumnOfClickBar(dados, "", "", "", name);
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


                        let valueY = o.Acao[categoryY];

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


                        let valueY = o.Acao[categoryY];

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

function FiltraLinhas(array, arrColuna, arrValue) {

    let novoArr = [];

    array.forEach(function (o, c) {

        var flag = true;

        if (arrColuna == "_Quem" || arrColuna == "_GrupoCausa" || arrColuna == "_CausaGenerica" || arrColuna == "_ContramedidaGenerica"
            || arrColuna == "UnidadeName" || arrColuna == "_StatusName" || arrColuna == "Regional"
            || arrColuna == "Level1Name" || arrColuna == "Level2Name" || arrColuna == "Level3Name" || arrColuna == "Acao.TipoIndicador") {

            if (arrColuna == "Acao.TipoIndicador" && arrValue[0] != "Todos") {
                arrValueAux = [];
                if (arrValue[0] == "Diretrizes")
                    arrValueAux.push(1);
                else if (arrValue[0] == "Scorecard")
                    arrValueAux.push(2);
                else if (arrValue[0] === "Sem planejamento operacional")
                    arrValueAux.push(0);

                arrColuna.forEach(function (oo, cc) {

                    if (o.Acao[oo.replace('Acao.', '')] != arrValueAux[cc]) {
                        flag = false;
                    }
                });

                if (flag) {
                    novoArr.push(o);
                }

            } else {
                arrColuna.forEach(function (oo, cc) {

                    if (o.Acao[oo] != arrValue[cc]) {
                        flag = false;
                    }
                });

                if (flag) {
                    novoArr.push(o);
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
                    novoArr.push(o);
                }
            }
        }
    });

    return novoArr;
}

function FiltraLinhasComTodos(array, arrColuna, arrValue) {

    let novoArr = [];

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
            novoArr.push(o);
        }
    });

    return novoArr;
}

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
        };

        ColvisarrayVisaoUsuario_show = ColvisarrayVisaoAtual_show;
        ColvisarrayVisaoUsuario_hide = ColvisarrayVisaoAtual_hide;

    } else if (tabela == "Planejamento") {

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
        };

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

function distinctFilter(lista, filtro, selectId) {

    filtro = filtro.replace("Acao.", "");
    lista = FiltraColunas(lista, [filtro]);

    var l = [];
    var retorno = [];

    jQuery.each(lista, function (i) {
        if (lista[i][filtro] !== undefined && lista[i][filtro] !== null && lista[i][filtro] !== "")
            l.push(lista[i][filtro]);
    });

    l.sort();

    retorno = l.filter(onlyUnique);

    $('#' + selectId).children('option').remove();

    $('#' + selectId).append($("<option></option>").attr("value", 0).text(Resources("all")));

    $.each(retorno, function (key, value) {

        if (filtro == "TipoIndicador") {

            if (value == 0)
                value = Resources("no_operational_planning");

            else if (value == 1)
                value = Resources("guidelines");

            else if (value == 2)
                value = "Scorecard";
        }
        if (value != null && value != "0") {
            $('#' + selectId).append($("<option></option>").attr("value", key).text(value));
        }

    });

    $('#' + selectId).trigger('change');

}

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
        pintaStatus(seriesFilter, serieArrFinal);

    return serieArrFinal;
}

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

function atualizarTopFilters() {
    $('.dropdown-toggle').css('font-size', 'xx-small');
    $('.select2').css('width', '50px');
    $('.select2').css('font-size', 'xx-small');
}

$(document).ready(function () {

    $('.defaultFilter').css('color', '#000000');

    $("input[name='daterange']").css('font-size', 'xx-small');
    $("input[name='daterange']").css('width', '140px');
    $("input[name='daterange']").css('height', '25px');

    setTimeout(atualizarTopFilters, 5000);

});

$('#btnTop').click(function () {
    dadosPie2 = "";
    FiltraLinhasComTodos(dados, "", "Todos");
    getDateRange($("input[name='daterange']").val());

    GetDataTable($('#campo1Filtro option:selected').val(),
        $('#valor1Filtro option:selected').text(),
        $('#campo2Filtro option:selected').val(),
        $('#valor2Filtro option:selected').text());
});

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
});

$('#btnpanel5').off('click').on('click', function () {

    var nameY = [];
    if ($('#filtroY option:selected').val() != "") {
        nameY.push($('#filtroY option:selected').text());
    }

    filtraDadosParaGerarGraficoPanel5Panel6($('#campo1Panel5 option:selected').val(), $('#campo2Panel5 option:selected').val(), nameY, 'panel5')
    $('#FirstParamPanel5').html($('#campo1Panel5 option:selected').text());
    $('#LastParamPanel5').html($('#campo2Panel5 option:selected').text());
});

$('#btnpanel6').off('click').on('click', function () {
    filtraDadosParaGerarGraficoPanel5Panel6($('#campo1Panel6 option:selected').val(), $('#campo2Panel6 option:selected').val(), [], 'panel6');
    $('#FirstParamPanel6').html($('#campo1Panel6 option:selected').text());
    $('#LastParamPanel6').html($('#campo2Panel6 option:selected').text());
});