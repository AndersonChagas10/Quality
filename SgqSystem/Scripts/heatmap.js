
var HeatMap = {
    Inicializar: function (config) {
        //MagicString
        if (typeof config.json == "string")
            this.jsonObject = JSON.parse(config.json);
        else
            this.jsonObject = config.json;

        if (config.idPlaceholder != undefined) {
            this.idPlaceholder = config.idPlaceholder;
            if (config.maxWidth != undefined)
                $(this.idPlaceholder).css("max-width", config.maxWidth).css("min-width", config.maxWidth);
        }

        var minPorc = 0;
        var maxPorc = 100;

        if (config.min != undefined || config.minMedia != undefined || config.minRodape != undefined) {
            this.min = minPorc;
            this.minMedia = minPorc;
            this.minRodape = minPorc;
        }

        if (config.max != undefined || config.maxMedia != undefined || config.maxRodape != undefined) {
            this.min = maxPorc;
            this.minMedia = maxPorc;
            this.minRodape = maxPorc;
        }

        if (config.cellWidth != undefined)
            this.cellWidth = config.cellWidth;

        if (config.cellHeight != undefined)
            this.cellHeight = config.cellHeight;

        if (config.tituloIndicador != undefined)
            this.TituloIndicador = config.tituloIndicador;
        if (config.tituloDesdobramento != undefined)
            this.TituloDesdobramento = config.tituloDesdobramento;
        if (config.tituloColuna != undefined)
            this.TituloColuna = config.tituloColuna;

        if (config.jsonName != undefined)
            this.jsonName = config.jsonName;
        if (config.Cabecalho != undefined)
            this.Cabecalho = config.Cabecalho;
        if (config.Indicador != undefined)
            this.Indicador = config.Indicador;

        if (config.widthColumn1 != undefined)
            this.widthColumn1 = config.widthColumn1;
        if (config.widthColumn2 != undefined)
            this.widthColumn2 = config.widthColumn2;
        if (config.widthColumn3 != undefined)
            this.widthColumn3 = config.widthColumn3;

        if (config.isGradient != undefined)
            this.isGradient = config.isGradient;
        if (config.heatmapType != undefined)
            this.heatmapType = config.heatmapType;

        if (config.colorMin != undefined)
            this.colorMin = config.colorMin;
        if (config.colorMax != undefined)
            this.colorMax = config.colorMax;
        if (config.colorMid != undefined)
            this.colorMid = config.colorMid;
        if (config.valorVazio != undefined)
            this.valorVazio = config.valorVazio;
        if (config.colorMain != undefined)
            this.colorMain = config.colorMain;
        if (config.colorEmptyTd != undefined)
            this.colorEmptyTd = config.colorEmptyTd;

        if (config.Valores != undefined)
            this.Valores = config.Valores;

        if (config.callback != undefined)
            this.callback = config.callback;

        this.Estrutura = MontaEstrutura();

        this.CabecalhoX = "CabecalhoX";
        this.IndicadorY = "IndicadorY";
        this.dataHash = "data-hash";
        this.dataCabecalho = "data-cabecalho";
        this.dataIndicador = "data-indicador";
        this.dataValor = "data-valor";

        this.idIndicador = this.idPlaceholder + " #listaInidicador";
        this.idCabecalho = this.idPlaceholder + " #listaCabecalho";
        this.idCabecalhoValores = this.idPlaceholder + " #listaCabecalhoValores";
        this.idCabecalhoTotal = this.idPlaceholder + " #cabecalhoTotal";
        this.idValores = this.idPlaceholder + " #listaValores";
        this.idRodape = this.idPlaceholder + " #listaRodape";
        this.idMedia = this.idPlaceholder + " #listaMedia";
        this.idTotalMedia = this.idPlaceholder + " #totalMedia";

        this.Indicadores = [];
        this.Cabecalhos = [];

        this.minMaxPorIndicador = [];

        this.Preparar();
    },

    jsonName: "Data",
    TituloIndicador: "",
    TituloColuna: "INDICADOR",
    TituloDesdobramento: "",
    CabecalhoX: "CabecalhoX",
    IndicadorY: "IndicadorY",
    Cabecalho: "Cabecalho",
    Indicador: "Indicador",
    cellWidth: "50",
    cellHeight: "40",

    valorVazio: "-",

    Indicadores: [],
    Cabecalhos: [],

    Valores: [],

    Preparar: Preparar,
    callback: function () { },

    isGradient: true,
    heatmapType: "all",

    colorMin: "",
    percentageMid: 0.51,
    colorMid: "",
    percentageMax: 0.95,
    colorMax: "",


    RecalculaMatrizPorcentagem: function () {

        //Recalcula totais para matriz com porcentagem de NC
        HeatMap.minMaxPorIndicador = [];
        HeatMap.minMedia = undefined;
        HeatMap.minRodape = undefined;
        HeatMap.maxMedia = undefined;
        HeatMap.maxRodape = undefined;

        //Total direita
        for (var j = 0; j < HeatMap.Indicadores.length; j++) {
            for (var z = 0; z < HeatMap.Valores.length; z++) {

                let indicadorY = j;
                let valorCabecalho = RetornaTituloValor(z);
                let identificador = HeatMap.idMedia + ' td[' + HeatMap.dataIndicador + '="' + indicadorY + '"][' + HeatMap.dataValor + '="' + valorCabecalho + '"]';

                let amostragem = this.jsonObject.filter(x => x.IndicadorY === indicadorY).map(x => x.Amostragem).reduce(sumReduce);
                let qtdeNC = this.jsonObject.filter(x => x.IndicadorY === indicadorY).map(x => x.QtdeNC).reduce(sumReduce);

                let porcentagemNCTotal = (parseFloat(qtdeNC) / parseFloat(amostragem)) * 100;

                porcentagemNCTotal = isNaN(porcentagemNCTotal) ? 0 : porcentagemNCTotal;

                let identificadorValores = HeatMap.idValores + ' td[' + HeatMap.dataIndicador + '="' + indicadorY + '"][' + HeatMap.dataValor + '="' + valorCabecalho + '"]';

                $(identificadorValores).each(function (i, o) {
                    var valor = ValorNumerico($(o).text());
                    if (HeatMap.Valores[z]["heatmap"] == true && $(o).text() != HeatMap.valorVazio) {
                        SetaMinimoMaximoPorIndicador(indicadorY, valor);
                        SetaMinimoMaximo(valor);
                    }
                });

                $(identificador).html(porcentagemNCTotal.toFixed(2));

                SetaMinimoMaximoMedia(parseFloat($(identificador).html()));
            }
        }

        //Total Rodapé
        for (var i = 0; i < HeatMap.Cabecalhos.length; i++) {
            for (var z = 0; z < HeatMap.Valores.length; z++) {

                let cabecalhoX = i;
                let valorCabecalho = RetornaTituloValor(z);
                let identificador = HeatMap.idRodape + ' td[' + HeatMap.dataCabecalho + '="' + cabecalhoX + '"][' + HeatMap.dataValor + '="' + valorCabecalho + '"]';

                let amostragem = this.jsonObject.filter(x => x.CabecalhoX === cabecalhoX).map(x => x.Amostragem).reduce(sumReduce);
                let qtdeNC = this.jsonObject.filter(x => x.CabecalhoX === cabecalhoX).map(x => x.QtdeNC).reduce(sumReduce);

                let porcentagemNCTotal = (parseFloat(qtdeNC) / parseFloat(amostragem)) * 100;

                porcentagemNCTotal = isNaN(porcentagemNCTotal) ? 0 : porcentagemNCTotal;

                $(identificador).html(porcentagemNCTotal.toFixed(2));

                SetaMinimoMaximoRodape(parseFloat($(identificador).html()));
            }
        }

        //Total total
        for (var z = 0; z < HeatMap.Valores.length; z++) {

            let valorCabecalho = RetornaTituloValor(z);

            let identificador = HeatMap.idTotalMedia + ' td[' + HeatMap.dataValor + '="' + valorCabecalho + '"]'

            let amostragem = this.jsonObject.map(x => x.Amostragem).reduce(sumReduce);
            let qtdeNC = this.jsonObject.map(x => x.QtdeNC).reduce(sumReduce);

            let porcentagemNCTotal = (parseFloat(qtdeNC) / parseFloat(amostragem)) * 100;

            porcentagemNCTotal = isNaN(porcentagemNCTotal) ? 0 : porcentagemNCTotal;

            $(identificador).html(porcentagemNCTotal.toFixed(2));

        }

        PreencheCalorNasCelulasValores();
        PreencheCalorMedia();
        PreencheCalorRodape();
    },
    RecalculaMatrizSoma: function () {

        //Recalcula totais para matriz com porcentagem de NC
        HeatMap.minMaxPorIndicador = [];
        HeatMap.minMedia = undefined;
        HeatMap.minRodape = undefined;
        HeatMap.maxMedia = undefined;
        HeatMap.maxRodape = undefined;

        //Total direita
        for (var j = 0; j < HeatMap.Indicadores.length; j++) {
            for (var z = 0; z < HeatMap.Valores.length; z++) {

                let indicadorY = j;
                let valorCabecalho = RetornaTituloValor(z);
                let identificador = HeatMap.idMedia + ' td[' + HeatMap.dataIndicador + '="' + indicadorY + '"][' + HeatMap.dataValor + '="' + valorCabecalho + '"]';

                let pontos = this.jsonObject.filter(x => x.IndicadorY === indicadorY).map(x => x.Pontos).reduce(sumReduce);

                let identificadorValores = HeatMap.idValores + ' td[' + HeatMap.dataIndicador + '="' + indicadorY + '"][' + HeatMap.dataValor + '="' + valorCabecalho + '"]';

                $(identificadorValores).each(function (i, o) {
                    var valor = ValorNumerico($(o).text());
                    if (HeatMap.Valores[z]["heatmap"] == true && $(o).text() != HeatMap.valorVazio) {
                        SetaMinimoMaximoPorIndicador(indicadorY, valor);
                        SetaMinimoMaximo(valor);
                    }
                });

                pontos = pontos ? pontos : 0;

                $(identificador).html(pontos.toFixed(2));

                SetaMinimoMaximoMedia(parseFloat($(identificador).html()));
            }
        }

        //Total Rodapé
        for (var i = 0; i < HeatMap.Cabecalhos.length; i++) {
            for (var z = 0; z < HeatMap.Valores.length; z++) {

                let cabecalhoX = i;
                let valorCabecalho = RetornaTituloValor(z);
                let identificador = HeatMap.idRodape + ' td[' + HeatMap.dataCabecalho + '="' + cabecalhoX + '"][' + HeatMap.dataValor + '="' + valorCabecalho + '"]';

                let pontos = this.jsonObject.filter(x => x.CabecalhoX === cabecalhoX).map(x => x.Pontos).reduce(sumReduce);

                pontos = pontos ? pontos : 0;

                $(identificador).html(pontos.toFixed(2));

                SetaMinimoMaximoRodape(parseFloat($(identificador).html()));
            }
        }

        //Total total
        for (var z = 0; z < HeatMap.Valores.length; z++) {

            let valorCabecalho = RetornaTituloValor(z);

            let identificador = HeatMap.idTotalMedia + ' td[' + HeatMap.dataValor + '="' + valorCabecalho + '"]'

            let pontos = this.jsonObject.map(x => x.Pontos).reduce(sumReduce);

            pontos = pontos ? pontos : 0;

            $(identificador).html(pontos.toFixed(2));

        }

        PreencheCalorNasCelulasValores();
        PreencheCalorMedia();
        PreencheCalorRodape();
    }

}

function PosicaoNaLista(lista, valor) {
    if (lista != null && lista != undefined) {
        for (var i = 0; i < lista.length; i++) {
            if (lista[i]["name"] == valor)
                return i;
        }
    }
    return -1;
}

function ValorCelula(identificador) {
    return ValorNumerico($(identificador + " :last-child").text());
}

function ValorNumerico(valor) {
    return !!parseFloat(valor) ? parseFloat(valor) : 0;
}

function SetaMinimoMaximo(valor) {
    if (HeatMap.min == undefined)
        HeatMap.min = valor;
    if (HeatMap.min > valor)
        HeatMap.min = valor;
    if (HeatMap.max == undefined)
        HeatMap.max = valor;
    if (HeatMap.max < valor)
        HeatMap.max = valor;
}

function SetaMinimoMaximoPorIndicador(indicador, valor) {
    if (HeatMap.minMaxPorIndicador == undefined)
        HeatMap.minMaxPorIndicador = [];

    if (HeatMap.minMaxPorIndicador[indicador] == undefined) {
        HeatMap.minMaxPorIndicador[indicador] = { min: valor, max: valor };
    } else {
        if (HeatMap.minMaxPorIndicador[indicador].min > valor) {
            HeatMap.minMaxPorIndicador[indicador].min = valor;
        }

        if (HeatMap.minMaxPorIndicador[indicador].max < valor) {
            HeatMap.minMaxPorIndicador[indicador].max = valor;
        }
    }
}

function SetaMinimoMaximoMedia(valor) {
    if (HeatMap.minMedia == undefined)
        HeatMap.minMedia = valor;
    if (HeatMap.minMedia > valor)
        HeatMap.minMedia = valor;
    if (HeatMap.maxMedia == undefined)
        HeatMap.maxMedia = valor;
    if (HeatMap.maxMedia < valor)
        HeatMap.maxMedia = valor;
}

function SetaMinimoMaximoRodape(valor) {
    if (HeatMap.minRodape == undefined)
        HeatMap.minRodape = valor;
    if (HeatMap.minRodape > valor)
        HeatMap.minRodape = valor;
    if (HeatMap.maxRodape == undefined)
        HeatMap.maxRodape = valor;
    if (HeatMap.maxRodape < valor)
        HeatMap.maxRodape = valor;
}

/**FUNCOES**/
function PreencheFundo(color) {
    return " color:#000 !important;font-weight:bold !important;background-color:#" + color;
}

function MontaColunaUnica(valor, indicadorY, cssClass) {
    return "<tr><th class='" + cssClass + "' " + HeatMap.dataIndicador + "='" + indicadorY + "'>" + valor + "</th></tr>";
}

function MontaColunaMedia(valor, indicadorY, valorCabecalho) {
    return "<tr><td " + HeatMap.dataIndicador + "='" + indicadorY + "' " + HeatMap.dataValor + "='" + valorCabecalho + "'>" + valor + "</td></tr>";
}

function MontaCelulaTD(valor, colspan) {
    return "<td colspan='" + colspan + "'>" + valor + "</td>";
}

function MontaCelulaTH(valor, colspan, cssClass) {
    return "<th class='" + cssClass + "' colspan='" + colspan + "'>" + valor + "</th>";
}

function MontaCelulaValores(valor, indicadorY, cabecalhoX, valorCabecalho) {
    return "<td " + HeatMap.dataHash + "='" + indicadorY + cabecalhoX + valorCabecalho + "' " + HeatMap.dataIndicador + "='" + indicadorY + "' " + HeatMap.dataCabecalho + "='" + cabecalhoX + "' " + HeatMap.dataValor + "='" + valorCabecalho + "'>" + valor + "</td>";
}

function RetornaTituloValor(posicao, valor) {
    if (valor == null || valor == undefined)
        valor = "title";
    return HeatMap.Valores[posicao][valor];
}

function RetornaValorPeloTitulo(title) {
    for (var j = 0; j < HeatMap.Valores.length; j++) {
        if (HeatMap.Valores[j]["title"] == title)
            return HeatMap.Valores[j];
    }
    return undefined;
}

function RetornaValorCorpoExp(item, title) {
    var valor = RetornaValorPeloTitulo(title);

    if (valor == null || valor == undefined)
        return valor;

    if (valor["render"] != undefined) {
        return RetornaValorExp(item, valor["render"]());
    } else {
        return item[valor["data"]];
    }
}



function RetornaValorExpMedia(idSeletor, dataCondicaoSeletor, expressao) {

    if (expressao.indexOf('{') >= 0) {
        var regExp = new RegExp('({([^{]|)*})', 'g');
        var html = expressao;
        var match = html.match(regExp);
        for (var j = 0; j < match.length; j++) {
            var valorTemp = match[j].replace("{", "").replace("}", "");
            var valor = $(idSeletor + ' td' + dataCondicaoSeletor + '[data-valor="' + valorTemp + '"]').text();
            html = html.replace(match[j], valor != undefined ? valor : "-");
        }
        try {
            if (html != "(0/0)*100") {
                return (eval(html)).toFixed(2);
            }
        } catch (e) {
            return 0;
        }
    }
}

function RetornaValorExp(item, title) {

    if (title.indexOf('{') >= 0) {
        var regExp = new RegExp('({([^{]|)*})', 'g');
        var html = title;
        var match = html.match(regExp);
        for (var j = 0; j < match.length; j++) {
            var valorTemp = item[match[j].replace("{", "").replace("}", "")];
            html = html.replace(match[j], valorTemp != undefined ? valorTemp : "-");
        }
        return html;
    } else {
        return item[title];
    }
}
/**MONTAGEM**/
function MontaIndicadores() {
    var html = "";

    for (var i = 0; i < HeatMap.Indicadores.length; i++) {
        html += MontaColunaUnica(HeatMap.Indicadores[i]["name"], null, "esquerda");
    }
    return html;
}

function MontaCabecalhos() {
    var html = "";
    for (var i = 0; i < HeatMap.Cabecalhos.length; i++) {
        html += MontaCelulaTH(HeatMap.Cabecalhos[i]["name"], HeatMap.Valores.length, "strong");
    }
    return html;
}

function MontaCabecalhoValores() {
    var html = "";
    for (var i = 0; i < HeatMap.Cabecalhos.length; i++) {
        for (var j = 0; j < HeatMap.Valores.length; j++) {
            html += MontaCelulaTH(RetornaTituloValor(j));
        }
        if (i == 0) {
            HeatMap.TituloTotal = html;
        }
    }
    return html;
}

function MontaValores() {
    for (var i = 0; i < HeatMap.jsonObject.length; i++) {

        var item = HeatMap.jsonObject[i];
        var indicadorY = item[HeatMap.IndicadorY];
        var cabecalhoX = item[HeatMap.CabecalhoX];

        for (var j = 0; j < HeatMap.Valores.length; j++) {
            var valorCabecalho = RetornaTituloValor(j);
            var identificador = HeatMap.idValores + ' td[' + HeatMap.dataHash + '="' + indicadorY + cabecalhoX + valorCabecalho + '"]';
            var valorDaCelula = ValorCelula(identificador);
            $(identificador).html(RetornaValorCorpoExp(item, valorCabecalho));
        }

    }
}

function MontaMedia() {

    for (var j = 0; j < HeatMap.Indicadores.length; j++) {
        for (var z = 0; z < HeatMap.Valores.length; z++) {
            var indicadorY = j;
            var valorCabecalho = RetornaTituloValor(z);
            var identificadorValores = HeatMap.idValores + ' td[' + HeatMap.dataIndicador + '="' + indicadorY + '"][' + HeatMap.dataValor + '="' + valorCabecalho + '"]';
            var identificador = HeatMap.idMedia + ' td[' + HeatMap.dataIndicador + '="' + indicadorY + '"][' + HeatMap.dataValor + '="' + valorCabecalho + '"]';
            var valorDaCelula = 0;
            $(identificadorValores).each(function (i, o) {
                var valor = ValorNumerico($(o).text());
                if (HeatMap.Valores[z]["heatmap"] == true && $(o).text() != HeatMap.valorVazio) {
                    SetaMinimoMaximoPorIndicador(indicadorY, valor);
                    SetaMinimoMaximo(valor);
                }
                valorDaCelula += valor;
            });
            var count = $.grep($(identificadorValores), function (n, i) { return $(n).text() != HeatMap.valorVazio }).length;
            if (HeatMap.Valores[z]["groupFunction"] == "AVG" && count > 0) {
                valorDaCelula = valorDaCelula / count;
            }
            if (HeatMap.Valores[z]["groupFunction"] == "hide") {
                valorDaCelula = "-";
                $(identificador).html(valorDaCelula);
            } else {
                $(identificador).html(valorDaCelula.toFixed(2));
            }
            SetaMinimoMaximoMedia(parseFloat($(identificador).html()));
        }
    }

    for (var j = 0; j < HeatMap.Indicadores.length; j++) {
        for (var z = 0; z < HeatMap.Valores.length; z++) {
            var indicadorY = j;
            var valorCabecalho = RetornaTituloValor(z);
            var identificador = HeatMap.idMedia + ' td[' + HeatMap.dataIndicador + '="' + indicadorY + '"][' + HeatMap.dataValor + '="' + valorCabecalho + '"]';

            if (HeatMap.Valores[z]["Exp"] != null && HeatMap.Valores[z]["Exp"].length > 0) {
                var obj = $(identificador);
                obj.text(RetornaValorExpMedia(HeatMap.idMedia, '[' + HeatMap.dataIndicador + '="' + j + '"]', HeatMap.Valores[z]["Exp"]))
            }
        }
    }
}



function MontaRodape() {

    for (var i = 0; i < HeatMap.Cabecalhos.length; i++) {
        for (var z = 0; z < HeatMap.Valores.length; z++) {
            var cabecalhoX = i;
            var valorCabecalho = RetornaTituloValor(z);
            var identificadorValores = HeatMap.idValores + ' td[' + HeatMap.dataCabecalho + '="' + cabecalhoX + '"][' + HeatMap.dataValor + '="' + valorCabecalho + '"]';
            var identificador = HeatMap.idRodape + ' td[' + HeatMap.dataCabecalho + '="' + cabecalhoX + '"][' + HeatMap.dataValor + '="' + valorCabecalho + '"]';
            var valorDaCelula = 0;
            $(identificadorValores).each(function (i, o) {
                var valor = ValorNumerico($(o).text());
                valorDaCelula += valor;
            });
            var count = $.grep($(identificadorValores), function (n, i) { return $(n).text() != HeatMap.valorVazio }).length;
            if (HeatMap.Valores[z]["groupFunction"] == "AVG" && count > 0) {
                valorDaCelula = valorDaCelula / count;
            }
            if (HeatMap.Valores[z]["groupFunction"] == "hide") {
                valorDaCelula = "-";
                $(identificador).html(valorDaCelula);
            } else {
                $(identificador).html(valorDaCelula.toFixed(2));
            }
            SetaMinimoMaximoRodape(parseFloat($(identificador).html()));
        }
    }

    for (var i = 0; i < HeatMap.Cabecalhos.length; i++) {
        for (var z = 0; z < HeatMap.Valores.length; z++) {
            var cabecalhoX = i;
            var valorCabecalho = RetornaTituloValor(z);
            var identificador = HeatMap.idRodape + ' td[' + HeatMap.dataCabecalho + '="' + cabecalhoX + '"][' + HeatMap.dataValor + '="' + valorCabecalho + '"]';

            if (HeatMap.Valores[z]["Exp"] != null && HeatMap.Valores[z]["Exp"].length > 0) {
                var obj = $(identificador);
                obj.text(RetornaValorExpMedia(HeatMap.idRodape, '[' + HeatMap.dataCabecalho + '="' + i + '"]', HeatMap.Valores[z]["Exp"]))
            }
        }
    }
}

function MontaTotalMedia() {

    for (var z = 0; z < HeatMap.Valores.length; z++) {
        var valorCabecalho = RetornaTituloValor(z);
        var identificadorValores = HeatMap.idMedia + ' td[' + HeatMap.dataValor + '="' + valorCabecalho + '"]';
        var identificador = HeatMap.idTotalMedia + ' td[' + HeatMap.dataValor + '="' + valorCabecalho + '"]';
        var valorDaCelula = 0;
        $(identificadorValores).each(function (i, o) {
            valorDaCelula += ValorNumerico($(o).html());
        });
        var count = $.grep($(identificadorValores), function (n, i) { return $(n).text() != HeatMap.valorVazio }).length;
        if (HeatMap.Valores[z]["groupFunction"] == "AVG" && count > 0) {
            valorDaCelula = valorDaCelula / count;
        }
        if (HeatMap.Valores[z]["groupFunction"] == "hide") {
            valorDaCelula = "-";
            $(identificador).html(valorDaCelula);
        } else {
            $(identificador).html(valorDaCelula.toFixed(2));
        }
    }

    for (var z = 0; z < HeatMap.Valores.length; z++) {
        var valorCabecalho = RetornaTituloValor(z);
        var identificador = HeatMap.idTotalMedia + ' td[' + HeatMap.dataValor + '="' + valorCabecalho + '"]';

        if (HeatMap.Valores[z]["Exp"] != null && HeatMap.Valores[z]["Exp"].length > 0) {
            var obj = $(identificador);
            obj.text(RetornaValorExpMedia(HeatMap.idTotalMedia, '', HeatMap.Valores[z]["Exp"]))
        }
    }
}

/**VAZIOS**/
function MontaValoresVazios() {
    var html = "";
    for (var i = 0; i < HeatMap.Indicadores.length; i++) {
        var tr = "<tr>";
        for (var j = 0; j < HeatMap.Cabecalhos.length; j++) {
            for (var z = 0; z < HeatMap.Valores.length; z++) {
                tr += MontaCelulaValores(HeatMap.valorVazio, i, j, RetornaTituloValor(z));
            }
        }
        tr += "</tr>";
        html += tr;
    }
    return html;
}

function MontaRodapeVazios() {
    var html = "<tr>";
    for (var j = 0; j < HeatMap.Cabecalhos.length; j++) {
        for (var z = 0; z < HeatMap.Valores.length; z++) {
            html += MontaCelulaValores(HeatMap.valorVazio, -1, j, RetornaTituloValor(z));
        }
    }
    html += "</tr>";
    return html;
}

function MontaMediaVazios() {
    var html = "";

    for (var i = 0; i < HeatMap.Indicadores.length; i++) {
        html += MontaTotalMedias(i);
    }
    return html;
}

function MontaTotalMedias(indicadorY) {
    var html = "<tr>";
    for (var z = 0; z < HeatMap.Valores.length; z++) {
        var valorCabecalho = RetornaTituloValor(z);
        html += MontaCelulaValores(HeatMap.valorVazio, indicadorY, null, valorCabecalho)
    }
    html += "</tr>";
    return html;
}

/**HEATMAP**/
function GetColor(ratio) {
    var hex = function (x) {
        x = x.toString(16);
        return (x.length == 1) ? '0' + x : x;
    };

    //ratio = 1;

    if (HeatMap.isGradient) {
        if (ratio < 0.5) {
            ratio *= 2;
            var r = Math.ceil(parseInt(HeatMap.colorMin.substring(0, 2), 16) * (1 - ratio) + parseInt(HeatMap.colorMid.substring(0, 2), 16) * ratio);
            var g = Math.ceil(parseInt(HeatMap.colorMin.substring(2, 4), 16) * (1 - ratio) + parseInt(HeatMap.colorMid.substring(2, 4), 16) * ratio);
            var b = Math.ceil(parseInt(HeatMap.colorMin.substring(4, 6), 16) * (1 - ratio) + parseInt(HeatMap.colorMid.substring(4, 6), 16) * ratio);
        } else if (ratio >= 0.5) {
            ratio = (ratio - 0.5) * 2;
            var r = Math.ceil(parseInt(HeatMap.colorMid.substring(0, 2), 16) * (1 - ratio) + parseInt(HeatMap.colorMax.substring(0, 2), 16) * ratio);
            var g = Math.ceil(parseInt(HeatMap.colorMid.substring(2, 4), 16) * (1 - ratio) + parseInt(HeatMap.colorMax.substring(2, 4), 16) * ratio);
            var b = Math.ceil(parseInt(HeatMap.colorMid.substring(4, 6), 16) * (1 - ratio) + parseInt(HeatMap.colorMax.substring(4, 6), 16) * ratio);
        }
    } else {
        if (ratio < HeatMap.percentageMid) {
            return HeatMap.colorMin;
        } else
            if (ratio >= HeatMap.percentageMid && ratio < HeatMap.percentageMax) {
                return HeatMap.colorMid;
            } else {
                return HeatMap.colorMax;
            }
    }

    return hex(r) + hex(g) + hex(b);
}

function PreencheCalorNasCelulasValores() {

    if (HeatMap.heatmapType == "all") {
        for (var j = 0; j < HeatMap.Indicadores.length; j++) {
            for (var z = 0; z < HeatMap.Valores.length; z++) {
                if (HeatMap.Valores[z]["heatmap"] == true) {
                    var indicadorY = j;
                    var valorCabecalho = RetornaTituloValor(z);
                    var identificadorValores = HeatMap.idValores + ' td[' + HeatMap.dataIndicador + '="' + indicadorY + '"][' + HeatMap.dataValor + '="' + valorCabecalho + '"]';
                    $(identificadorValores + " :last-child").each(function (i, o) {
                        var valor = ValorNumerico($(o).text());
                        if ($(o).text() != HeatMap.valorVazio) {
                            var ratio = (valor - HeatMap.min) / ((HeatMap.max - HeatMap.min) / 100);
                            if (isNaN(ratio)) {
                                ratio = 100 / 100;
                            } else if (ratio != 0) {
                                ratio /= 100;
                            }
                            $(o).parent("td").attr("style", PreencheFundo(GetColor(ratio)));
                        }
                    });
                }
            }
        }
    } else if (HeatMap.heatmapType == "line") {
        for (var j = 0; j < HeatMap.Indicadores.length; j++) {
            for (var z = 0; z < HeatMap.Valores.length; z++) {
                if (HeatMap.Valores[z]["heatmap"] == true) {
                    var indicadorY = j;
                    var identificadorValores = HeatMap.idValores + ' td[' + HeatMap.dataIndicador + '="' + indicadorY + '"]';

                    let divsValores = $(identificadorValores + " :last-child");

                    if (divsValores.length === 1 ||
                        (HeatMap.minMaxPorIndicador[indicadorY] !== undefined &&
                            (HeatMap.minMaxPorIndicador[indicadorY].max === HeatMap.minMaxPorIndicador[indicadorY].min)))
                        continue;

                    $(divsValores).each(function (i, o) {
                        var valor = ValorNumerico($(o).text());
                        if ($(o).text() != HeatMap.valorVazio) {
                            var ratio = (valor - HeatMap.minMaxPorIndicador[indicadorY].min) / ((HeatMap.minMaxPorIndicador[indicadorY].max - HeatMap.minMaxPorIndicador[indicadorY].min) / 100);
                            if (isNaN(ratio)) {
                                ratio = 100 / 100;
                            } else if (ratio != 0) {
                                ratio /= 100;
                            }
                            $(o).parent("td").attr("style", PreencheFundo(GetColor(ratio)));
                        }
                    });
                }
            }
        }
    }
}

function PreencheCalorMedia() {
    for (var j = 0; j < HeatMap.Indicadores.length; j++) {
        for (var z = 0; z < HeatMap.Valores.length; z++) {
            if (HeatMap.Valores[z]["heatmap"] == true) {
                var indicadorY = j;
                var valorCabecalho = RetornaTituloValor(z);
                var identificador = HeatMap.idMedia + ' td[' + HeatMap.dataIndicador + '="' + indicadorY + '"][' + HeatMap.dataValor + '="' + valorCabecalho + '"]';

                if (HeatMap.Indicadores.length === 1 || (HeatMap.minMedia === HeatMap.maxMedia))
                    continue;

                $(identificador).each(function (i, o) {
                    var valor = ValorNumerico($(o).text());
                    if ($(o).text() != HeatMap.valorVazio) {
                        var ratio = (valor - HeatMap.minMedia) / ((HeatMap.maxMedia - HeatMap.minMedia) / 100);
                        if (isNaN(ratio)) {
                            ratio = 100 / 100;
                        } else if (ratio != 0) {
                            ratio /= 100;
                        }
                        $(o).attr("style", PreencheFundo(GetColor(ratio)));
                    }
                });
            }
        }
    }
}

function PreencheCalorRodape() {
    for (var i = 0; i < HeatMap.Cabecalhos.length; i++) {
        for (var z = 0; z < HeatMap.Valores.length; z++) {
            if (HeatMap.Valores[z]["heatmap"] == true) {
                var cabecalhoX = i;
                var valorCabecalho = RetornaTituloValor(z);
                var identificador = HeatMap.idRodape + ' td[' + HeatMap.dataCabecalho + '="' + cabecalhoX + '"][' + HeatMap.dataValor + '="' + valorCabecalho + '"]';

                if (HeatMap.Cabecalhos.length === 1 || (HeatMap.minRodape === HeatMap.maxRodape))
                    continue;

                $(identificador).each(function (i, o) {
                    var valor = ValorNumerico($(o).text());
                    if ($(o).text() != HeatMap.valorVazio) {
                        var ratio = (valor - HeatMap.minRodape) / ((HeatMap.maxRodape - HeatMap.minRodape) / 100);
                        if (isNaN(ratio)) {
                            ratio = 100 / 100;
                        } else if (ratio != 0) {
                            ratio /= 100;
                        }
                        $(o).attr("style", PreencheFundo(GetColor(ratio)));
                    }
                });
            }
        }
    }
}

function sumReduce(accumulator, currentValue) {

    if (typeof accumulator === 'string')
        accumulator = accumulator.replace(",", ".");

    if (typeof currentValue === 'string')
        currentValue = currentValue.replace(",", ".");

    accumulator = parseFloat(accumulator);
    currentValue = parseFloat(currentValue);

    accumulator = isNaN(accumulator) ? 0 : accumulator;
    currentValue = isNaN(currentValue) ? 0 : currentValue;


    return accumulator += currentValue
}

/**INICIALIZAR**/
function Preparar() {

    $(HeatMap.idPlaceholder).empty().append(HeatMap.Estrutura);

    /**MONTA ARRAYS**/
    for (var i = 0; i < HeatMap.jsonObject.length; i++) {
        var item = HeatMap.jsonObject[i];

        //Preenche array com cabecalhos e seta posição de cada objeto
        var index = PosicaoNaLista(HeatMap.Indicadores, RetornaValorExp(item, HeatMap.Indicador));
        if (index < 0) {
            index = HeatMap.Indicadores.length;
            HeatMap.Indicadores.push({ "name": RetornaValorExp(item, HeatMap.Indicador) });
        }
        HeatMap.jsonObject[i][HeatMap.IndicadorY] = index;

        //Preenche array com cabecalhos e seta posição de cada objeto
        index = PosicaoNaLista(HeatMap.Cabecalhos, RetornaValorExp(item, HeatMap.Cabecalho));
        if (index < 0) {
            index = HeatMap.Cabecalhos.length;
            HeatMap.Cabecalhos.push({ "name": RetornaValorExp(item, HeatMap.Cabecalho) });
        }
        HeatMap.jsonObject[i][HeatMap.CabecalhoX] = index;

    }

    /**MONTA NO HTML**/
    $(HeatMap.idIndicador).empty().append(MontaIndicadores());

    $(HeatMap.idCabecalho).empty().append(MontaCabecalhos());

    $(HeatMap.idCabecalhoValores).empty().append(MontaCabecalhoValores());

    $(HeatMap.idCabecalhoTotal + " tr:eq(0) th:eq(0)").attr('colspan', $(HeatMap.TituloTotal).length);
    $(HeatMap.idCabecalhoTotal + " tr:eq(1)").html(HeatMap.TituloTotal);

    $(HeatMap.idValores).empty().append(MontaValoresVazios());

    $(HeatMap.idRodape).empty().append(MontaRodapeVazios());

    $(HeatMap.idMedia).empty().append(MontaMediaVazios());

    $(HeatMap.idTotalMedia).empty().append(MontaTotalMedias());

    /**INSERE VALORES NO HTML**/
    MontaValores();
    MontaMedia();
    MontaRodape();
    MontaTotalMedia();

    /**MONTA MAPA DE CALOR**/
    PreencheCalorNasCelulasValores();
    PreencheCalorMedia();
    PreencheCalorRodape();

    /**FUNCAO PARA SINCRONIZAR SCROLL**/
    $(HeatMap.idPlaceholder + " .horizontal").attr('data-placeholder', HeatMap.idPlaceholder);
    $(HeatMap.idPlaceholder + " .horizontal").on("scroll", function () {
        var id = $(this).attr("data-placeholder");
        $(id + " .horizontal").scrollLeft($(this).scrollLeft());
    });

    $(HeatMap.idPlaceholder + " .vertical").attr('data-placeholder', HeatMap.idPlaceholder);
    $(HeatMap.idPlaceholder + " .vertical").on("scroll", function () {
        var id = $(this).attr("data-placeholder");
        $(id + " .vertical").scrollTop($(this).scrollTop());
    });

    HeatMap.callback();

}

function MontaEstrutura() {
    var css = '<style>                                                                                                                             ' +
        '	                                                                                                                                 ' +
        '	' + HeatMap.idPlaceholder + ' .direita, ' + HeatMap.idPlaceholder + ' .esquerda{                                                                         ' +
        '		width:100%;   font-weight:bold !important;                                                                                                              ' +
        '	}                                                                                                                                ' +
        '	                                                                                                                                 ' +
        '	' + HeatMap.idPlaceholder + ' .strong{                                                                      ' +
        '		font-weight:bold !important;                                                                                                               ' +
        '	}                                                                                                                                ' +
        '	                                                                                                                                 ' +
        '	' + HeatMap.idPlaceholder + ' div.column1{                                                           ' +
        '		width: ' + HeatMap.widthColumn1 + ' !important; float:left;                                                                                                       ' +
        '	}                                                                                                                                ' +
        '	                                                                                                                                 ' +
        '	' + HeatMap.idPlaceholder + ' div.column2{                                                            ' +
        '		max-width: ' + HeatMap.widthColumn2 + ' ; float:left;                                                                                                       ' +
        '	}                                                                                                                                ' +
        '	                                                                                                                                 ' +
        '	' + HeatMap.idPlaceholder + ' div.column3{                                                                      ' +
        '		width: ' + HeatMap.widthColumn3 + ' !important; float:right;                                                                                                               ' +
        '	}                                                                                                                                ' +
        '	                                                                                                                                 ' +
        '	' + HeatMap.idPlaceholder + ' .vertical                                                                                                      ' +
        '	{                                                                                                                                ' +
        '		overflow-y: hidden;                                                                                                          ' +
        '		overflow-x: hidden;                                                                                                          ' +
        '		max-height:200px;                                                                                                            ' +
        '	}                                                                                                                                ' +
        '	                                                                                                                                 ' +
        '	' + HeatMap.idPlaceholder + ' .horizontal                                                                                                    ' +
        '	{                                                                                                                                ' +
        '		overflow-x: hidden;                                                                                                          ' +
        '		overflow-y: hidden;                                                                                                          ' +
        '	}                                                                                                                                ' +
        '	                                                                                                                                 ' +
        '	' + HeatMap.idPlaceholder + ' .vh{                                                                                                           ' +
        '		overflow-x: auto;                                                                                                            ' +
        '		overflow-y: auto;                                                                                                            ' +
        '		max-height:200px;      height:auto;                                                                                                          ' +
        '	}                                                                                                                                ' +
        '	                                                                                                                                 ' +
        '	' + HeatMap.idPlaceholder + ' .linha2{                                                                                                       ' +
        '		max-height:150px;                                                                                                            ' +
        '	}                                                                                                                                ' +
        '                                                                                                                                    ' +
        '	' + HeatMap.idPlaceholder + ' .linha{                                                                                                       ' +
        '		clear:both;  margin:0px !important;                                                                                                          ' +
        '	}                                                                                                                                ' +
        '                                                                                                                                    ' +
        '	' + HeatMap.idPlaceholder + ' table                                                                                                          ' +
        '	{                                                                                                                                ' +
        '		border: 1px solid #fff;                                                                                                     ' +
        '		margin-right: 0px;                                                                                                          ' +
        '	}                                                                                                                                ' +
        '                                                                                                                                    ' +
        '	' + HeatMap.idPlaceholder + ' td, ' + HeatMap.idPlaceholder + ' th                                                                                       ' +
        '	{                                                                                                                                ' +
        '		min-height:  ' + HeatMap.cellHeight + 'px; font-size:10px !important;                                                                                                            ' +
        '		height:  ' + HeatMap.cellHeight + 'px;                                                                                                                ' +
        '		max-height:  ' + HeatMap.cellHeight + 'px !important;                                                                                                            ' +
        '		min-width: ' + HeatMap.cellWidth + 'px;width: ' + HeatMap.cellWidth + 'px !important;                                                                                                            ' +
        '		max-width: ' + HeatMap.cellWidth + 'px !important;                                                                                                            ' +
        '		text-align:center;                                                                                                           ' +
        '		border:1px solid #fff;                                                                                                       ' +
        '		padding-bottom:0px !important;  vertical-align: middle;                                                                                                     ' +
        '		padding-top:0px !important;                                                                                                       ' +
        '	}                                                                                                                                ' +
        '                                                                                                                                    ' +
        '	' + HeatMap.idPlaceholder + ' td                                                                                       ' +
        '	{                                                                                                                                ' +
        '		background-color:#' + HeatMap.colorEmptyTd + '                                                                                                            ' +
        '	}                                                                                                                                ' +
        '	                                                                                                                                 ' +
        '	' + HeatMap.idPlaceholder + ' th{                                                                                   ' +
        '		background-color:#' + HeatMap.colorMain + ';                                                                                                       ' +
        '	}                                                                                                                                ' +
        '	                                                                                                                                 ' +
        '	' + HeatMap.idPlaceholder + ' .titleHeatMap{                                                                                                 ' +
        '		height:auto;                                                                                                                 ' +
        '		text-align:center;                                                                                                           ' +
        '		font-weight:bold;                                                                                                            ' +
        '		font-size:10px !important;                                                                                                              ' +
        '		vertical-align:40px;                                                                                                         ' +
        '		background-color:#' + HeatMap.colorMain + ';                                                                                                       ' +
        '	}                                                                                                                                ' +
        '	                                                                                                                                 ' +
        '	' + HeatMap.idPlaceholder + '  ::-webkit-scrollbar {                                                                                         ' +
        '		width: 7px;                                                                                                                  ' +
        '		height: 7px;                                                                                                                 ' +
        '	} /* this targets the default scrollbar (compulsory) */                                                                          ' +
        '	                                                                                                                                 ' +
        '	' + HeatMap.idPlaceholder + '  ::-webkit-scrollbar-track {                                                                                   ' +
        '		background-color: #fff;                                                                                                      ' +
        '	} /* the new scrollbar will have a flat appearance with the set background color */                                              ' +
        '	                                                                                                                                 ' +
        '	' + HeatMap.idPlaceholder + '  ::-webkit-scrollbar-thumb {                                                                                   ' +
        '		background-color: #333;                                                                                                      ' +
        '	} /* this will style the thumb; ignoring the track */                                                                            ' +
        '	                                                                                                                                 ' +
        '	' + HeatMap.idPlaceholder + '  ::-webkit-scrollbar-button {                                                                                  ' +
        '		width:0px;                                                                                                                   ' +
        '		height:0px;                                                                                                                  ' +
        '	} /* optionally; you can style the top and the bottom buttons (left and right for horizontal bars) */                            ' +
        '	                                                                                                                                 ' +
        '	' + HeatMap.idPlaceholder + '  ::-webkit-scrollbar-corner {                                                                                  ' +
        '		background-color: black;                                                                                                     ' +
        '	} /* if both the vertical and the horizontal bars appear; then perhaps the riglistaCabecalhoValoresht bottom corner also needs to be styled */        ' +
        '	                                                                                                                                 ' +
        '	' + HeatMap.idPlaceholder + ' .removeMargin{                                                                                                 ' +
        '		margin-left:0px !important;                                                                                                  ' +
        '		margin-right:0px !important;                                                                                                 ' +
        '		padding-left:0px !important;                                                                                                 ' +
        '		padding-right:0px !important;                                                                                                ' +
        '	}                                                                                                                                ' +
        '</style>                                                                                                                            ';

    $('head').append(css);

    return '	<div>                                                          ' +
        '		<div class="row linha">                                     ' +
        '		<h5 style="font-weight:bold" id="tituloDesdobramento">' + HeatMap.TituloDesdobramento + '</h5>                                     ' +
        '		</div>                                     ' +
        '		<div class="row row1 linha">                                     ' +
        '			<div class="column1 removeMargin titleHeatMap">       ' +
        '				<table class="esquerda"><tr><th>' + HeatMap.TituloColuna + '</th></tr><tr><th>' + HeatMap.TituloIndicador + '</th></tr></table>                                          ' +
        '			</div>                                                 ' +
        '			<div class="column2 removeMargin">                    ' +
        '				<div class="horizontal">                           ' +
        '					<table>                                        ' +
        '						<tr id="listaCabecalho">                   ' +
        '						</tr>                                      ' +
        '						<tr id="listaCabecalhoValores">            ' +
        '						</tr>                                      ' +
        '					</table>                                       ' +
        '				</div>                                             ' +
        '			</div>                                                 ' +
        '			<div class="column3 removeMargin titleHeatMap">       ' +
        '				<table class="esquerda" id="cabecalhoTotal"><tr><th >TOTAL</th></tr><tr></tr></table>                                              ' +
        '			</div>                                                 ' +
        '		</div>                                                     ' +
        '		<div class="row linha2 linha">                                   ' +
        '			<div class="column1 removeMargin">                    ' +
        '				<div class="vertical">                             ' +
        '					<table class="esquerda" id="listaInidicador">  ' +
        '					</table>                                       ' +
        '				</div>                                             ' +
        '			</div>                                                 ' +
        '			<div class="column2 removeMargin">                    ' +
        '				<div class="vh horizontal vertical">               ' +
        '					<table id="listaValores">                      ' +
        '					</table>                                       ' +
        '				</div>                                             ' +
        '			</div>                                                 ' +
        '			<div class="column3 removeMargin">                    ' +
        '				<div class="vertical">                             ' +
        '					<table class="direita" id="listaMedia">        ' +
        '					</table>                                       ' +
        '				</div>                                             ' +
        '			</div>                                                 ' +
        '		</div>                                                     ' +
        '		<div class="row linha">                                          ' +
        '			<div class="column1 removeMargin titleHeatMap">       ' +
        '				<table class="esquerda"><tr><th>TOTAL</th></tr></table>                                              ' +
        '			</div>                                                 ' +
        '			<div class="column2 removeMargin">                    ' +
        '				<div class="horizontal">                           ' +
        '					<table id="listaRodape">                       ' +
        '					</table>                                       ' +
        '				</div>                                             ' +
        '			</div>                                                 ' +
        '			<div class="column3 removeMargin titleHeatMap">       ' +
        '				<div class="horizontal">                           ' +
        '					<table id="totalMedia" class="direita">	       ' +
        '					</table>    	   	                           ' +
        '				</div>                                             ' +
        '			</div>                                                 ' +
        '		</div>                                                     ' +
        '	</div>                                                         ';
}