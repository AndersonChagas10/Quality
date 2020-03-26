var acaoCorretivaObrigatoria = false;

var defectsPerEvaluation = [];

var listaDefeitos = [];

var listaDeDefeitosAlerta8 = [];
var listaDeAlertasAlerta8 = [];

function adicionaNaListaDeDefeitos(obj) {
    if (listaDefeitos.length > 0) {
        if (!!listaDefeitos[0].datetime && !!obj.datetime && listaDefeitos[0].datetime.substring(0, 10) != obj.datetime.substring(0, 10)) {
            listaDefeitos = [];
        }
    }
    listaDefeitos.push(obj);
}


/***********************************************************************************************
INICIO DOS METODOS QUE GERAM ALERTA
*********************************************************************************************/

//--------------------------------------Contas apenas para o tablet para gerar alerta


//� a tag do level1 que recebe a conta do todo mundo
//O nivel alerta vai jogar no level1



//Gerar uma linha de level1
//N�o importa o paconsolidationType � sempre igual
//N�mero de avalia��es no indicador
// A soma de avalia��es nos monitoramentos

//N�mero de defeitos no indicador 
// � a soma do n�mero de defeitos nos monitoramentos

//Gera uma linha de level2
//tipo de consolida��o do indicador
//Se ParConsolidationType
//N�mero de avalia��es no monitoramento
//1 A soma de avalia��es ponderadas das tarefas
//2 Se a soma de avalia��es ponderadas for maior que 1 ent�o 1 se n�o zero
//3 � igual ao item 1

//N�mero de defeitos no monitoramento 
//1 � a soma do n�mero de defeitos ponderados nas tarefas
//2 se a soma dos defeitos ponderados for maior que 1 ent�o 1 se n�o zero
//3 � igual ao item 1
//---------------------------------------------------------------------------------------

//Gera uma linha de level3

//1 binario vai trazer o valor do weiEvaluation � o mesmo valor do peso
//2 numero de defeitos o valor do weievaluation � o numero de amostragem (Sampling)
//3 intervalos vai trazer o valor do weievaluation � o mesmo do peso
//4 intervalos vai trazer o valor do weievaluation � o mesmo do peso

//Numero de avalia��es
//1 binario vai trazer o valor do Evaluation 1 
//2 numero de defeitos o valor do Evaluation � o numero de amostragem (Sampling)
//3 intervalos vai trazer o valor Ealuation 1
//4 intervalos vai trazer o valor Ealuation 1


//Defeitos Result_Level3
//1 binario vai trazer o valor do Evaluation 1 quando for conforme zero, quando for nao conforme � 1
//2 numero de defeitos o valor do Evaluation � o numero de defeitos
//3 intervalos vai trazer o valor Ealuation 1
//4 intervalos vai trazer o valor Ealuation 1

//WeiDefects 
//1 binario vai trazer o valor do Evaluation 1 quando for conforme zero, quando for nao conforme � (1 e multiplica pelo peso) mais (puni��o vezes o peso) 
//2 numero de defeitos o valor do Evaluation � o (numero de defeitos � multiplica pelo peso) mais (puni��o vezes o peso) 
//3 intervalos vai trazer o valor Ealuation 1 e multiplica pelo peso (1 e multiplica pelo peso) mais (puni��o vezes o peso) 
//4 intervalos vai trazer o valor Ealuation 1 e multiplica pelo peso (1 e multiplica pelo peso) mais (puni��o vezes o peso) 

//CT4Eva3  n�mero de nivel 3 avaliados
//Se o peso for maior que zero � 1 se nao � zero

//N 3 defeituos (is conform)
//se for defeito � false e valor defeito � 1
//SE sem defeito o valor � true e o valor definido � zero


//ORDEM DOS METODOS QUE GERAM OS ALERTAS
/*
AO CLICAR EM SALVAR: 
setValoresLevel3Alertas()
setValoresLevel2Alertas()
setValoresLevel1Alertas()
getAlertaLevel1()
setNovoNivelAlertaLevel1()
setGravaAlerta()
setEnviaEmail()
*/

function setValoresLevel3Alertas(level3, level2Resultado) {

    var resultado = [];

    var valor = parseFloat(ReplaceVirgula(level3.attr('value')));
    var punicao = parseFloat(ReplaceVirgula(level3.attr('punishmentvalue')));
    var numeroAmostragem = parseFloat(level3.parents('.level02Result').attr('sample'));

    //numeroAmostragem pagar do input
    //valor;

    //Trazer o tipo de input do level3
    //var inputType = parseInt($('.level3[id=' + level3.attr('level03id') + ']:visible').attr('inputtype'));
    var inputType = parseInt($('.level3[id=' + level3.attr('level03id') + ']').attr('inputtype'));

    /*****
    SE FOR CAMPO CALCULADO, PRECISAMOS REVERTER O VALOR PARA DECIMAL
    *****/
    //if (inputType == 4) {
    //    valor = converteNotacaoBaseDezParaDecimal(valor);
    //}

    var peso = parseFloat(level3.attr('weight'));
    var avaliacoesPonderadas = 0;
    var avaliacoes = 0;
    var limiteInferior = parseFloat(level3.attr('intervalmin'));
    var limiteSuperior = parseFloat(level3.attr('intervalmax'));

    var defeitos = 0;
    var defeitosPonderados = 0;
    var level3Avaliado = 0;
    var level3ComDefeitos = 0;

    //Calculo da avalia��o ponderada
    //Calculo do n�mero de avalia��es
    //Calculo level3 avaliados
    switch (inputType) {
        case 1:
            avaliacoes = 1;
            avaliacoesPonderadas = peso;
            level3Avaliado = avaliacoesPonderadas > 0 ? 1 : 0;
            break;
        case 6:
            avaliacoes = 1;
            avaliacoesPonderadas = peso;
            level3Avaliado = avaliacoesPonderadas > 0 ? 1 : 0;
            break;
        case 2:
            avaliacoes = numeroAmostragem;
            if ($(_level1).attr('parconsolidationtype_id') == 5) {
                avaliacoesPonderadas = peso;
            } else {
                avaliacoesPonderadas = numeroAmostragem * peso;
            }
            level3Avaliado = avaliacoesPonderadas > 0 ? 1 : 0;
            break;
        case 3:
            avaliacoes = 1;
            avaliacoesPonderadas = peso;
            level3Avaliado = avaliacoesPonderadas > 0 ? 1 : 0;
            break;
        case 7:
            avaliacoes = 1;
            avaliacoesPonderadas = peso;
            level3Avaliado = avaliacoesPonderadas > 0 ? 1 : 0;
            break;
        case 8:
            avaliacoes = 1;
            avaliacoesPonderadas = peso;
            level3Avaliado = avaliacoesPonderadas > 0 ? 1 : 0;
            break;
        case 4:
            avaliacoes = 1;
            avaliacoesPonderadas = peso;
            level3Avaliado = avaliacoesPonderadas > 0 ? 1 : 0;
            break;
        case 5:

            var vvalor = level3.attr('valuetext');

            avaliacoes = 0;
            avaliacoesPonderadas = 0;
            level3Avaliado = 0;

            if (vvalor != "undefined") {
                avaliacoes = 1;
                avaliacoesPonderadas = peso;
                level3Avaliado = avaliacoesPonderadas > 0 ? 1 : 0;
            }

            break;
        default:
            avaliacoes = 1;
            avaliacoesPonderadas = peso;
            level3Avaliado = avaliacoesPonderadas > 0 ? 1 : 0;
            break;
    }

    var defeitosVar = 0;

    //Calculo do defeito
    //Calculo de Defeitos Ponderados
    //Is Conform
    defeitosVar = 0;
    switch (inputType) {
        case 1:
            var vvalor = parseFloat(level3.attr('value').replace(",", "."));
            var vmin = parseFloat(level3.attr('intervalmin').replace(",", "."));
            var vmax = parseFloat(level3.attr('intervalmax').replace(",", "."));

            if (vvalor >= vmin && vvalor <= vmax) {

                defeitosVar = 0;

            } else {

                if (isNaN(vvalor)) {
                    defeitosVar = parseFloat(level3.attr('defects'));
                } else {
                    defeitosVar = 1;
                }

            }

            defeitos = defeitosVar > 0 ? 1 : 0;
            if (isEUA)
                defeitosPonderados = (defeitos * peso) + (punicao * peso);
            else
                defeitosPonderados = defeitosVar > 0 ? (defeitos * peso) + (punicao * peso) : 0;

            level3ComDefeitos = defeitosPonderados > 0 ? 1 : 0;
            break;
        case 6:
            var vvalor = parseFloat(level3.attr('value').replace(",", "."));
            var vmin = parseFloat(level3.attr('intervalmin').replace(",", "."));
            var vmax = parseFloat(level3.attr('intervalmax').replace(",", "."));

            if (vvalor >= vmin && vvalor <= vmax) {

                defeitosVar = 0;

            } else {

                if (isNaN(vvalor)) {
                    defeitosVar = parseFloat(level3.attr('defects'));
                } else {
                    defeitosVar = 1;
                }

            }

            defeitos = defeitosVar > 0 ? 1 : 0;
            if (isEUA)
                defeitosPonderados = (defeitos * peso) + (punicao * peso);
            else
                defeitosPonderados = defeitosVar > 0 ? (defeitos * peso) + (punicao * peso) : 0;

            level3ComDefeitos = defeitosPonderados > 0 ? 1 : 0;
            break;
        case 2:
            defeitos = parseFloat(level3.attr('value').replace(",", "."));

            if (isEUA)
                defeitosPonderados = (parseFloat(level3.attr('value').replace(",", ".")) * peso) + (punicao * (parseFloat(level3.attr('value').replace(",", ".")) * peso));
            else {
                defeitosPonderados = (parseFloat(level3.attr('value').replace(",", ".")) * peso) + (punicao * (parseFloat(level3.attr('value').replace(",", ".")) * peso));
                defeitosPonderados = defeitosVar > 0 ? (parseFloat(level3.attr('value').replace(",", ".")) * peso) + (punicao * (parseFloat(level3.attr('value').replace(",", ".")) * peso)) : defeitosPonderados;
            }
            level3ComDefeitos = defeitosPonderados > 0 ? 1 : 0;
            break;
        case 3:
            if (valor >= limiteInferior && valor <= limiteSuperior) {
                defeitos = 0;
            }
            else {
                defeitos = 1;
            }

            if (isEUA)
                defeitosPonderados = (defeitos * peso) + (punicao * peso);
            else
                defeitosPonderados = defeitos > 0 ? (defeitos * peso) + (punicao * peso) : 0;
            level3ComDefeitos = defeitosPonderados > 0 ? 1 : 0;
            break;
        case 7:
            if (valor >= limiteInferior && valor <= limiteSuperior) {
                defeitos = 0;
            }
            else {
                defeitos = 1;
            }

            if (isEUA)
                defeitosPonderados = (defeitos * peso) + (punicao * peso);
            else
                defeitosPonderados = defeitos > 0 ? (defeitos * peso) + (punicao * peso) : 0;
            level3ComDefeitos = defeitosPonderados > 0 ? 1 : 0;
            break;
        case 8:
            if (valor >= limiteInferior && valor <= limiteSuperior) {
                defeitos = 0;
            }
            else {
                defeitos = 1;
            }

            if (isEUA)
                defeitosPonderados = (defeitos * peso) + (punicao * peso);
            else
                defeitosPonderados = defeitos > 0 ? (defeitos * peso) + (punicao * peso) : 0;
            level3ComDefeitos = defeitosPonderados > 0 ? 1 : 0;
            break;
        case 4:
            if (valor >= limiteInferior && valor <= limiteSuperior) {
                defeitos = 0;
            }
            else {
                defeitos = 1;
            }

            if (isEUA)
                defeitosPonderados = (defeitos * peso) + (punicao * peso);
            else
                defeitosPonderados = defeitos > 0 ? (defeitos * peso) + (punicao * peso) : 0;
            level3ComDefeitos = defeitosPonderados > 0 ? 1 : 0;
            break;
        case 5:

            var vvalor = level3.attr('valuetext');

            defeitos = 0;
            defeitosPonderados = 0;
            level3ComDefeitos = 0;

            if (vvalor != "undefined") {
                defeitos = 1;
                defeitosPonderados = (1 * peso) + (punicao * peso);
                level3ComDefeitos = defeitosPonderados > 0 ? 1 : 0;
            }

            break;
        case 9:
            if (valor >= limiteInferior && valor <= limiteSuperior) {
                defeitos = 0;
            }
            else {
                defeitos = 1;
            }

            if (isEUA)
                defeitosPonderados = (defeitos * peso) + (punicao * peso);
            else
                defeitosPonderados = defeitos > 0 ? (defeitos * peso) + (punicao * peso) : 0;
            level3ComDefeitos = defeitosPonderados > 0 ? 1 : 0;
            break;
        case 10:
            if (valor >= limiteInferior && valor <= limiteSuperior) {
                defeitos = 0;
            }
            else {
                defeitos = 1;
            }

            if (isEUA)
                defeitosPonderados = (defeitos * peso) + (punicao * peso);
            else
                defeitosPonderados = defeitos > 0 ? (defeitos * peso) + (punicao * peso) : 0;
            level3ComDefeitos = defeitosPonderados > 0 ? 1 : 0;
            break;
        default:
            defeitos = 0;
            defeitosPonderados = 0;
            level3ComDefeitos = defeitosPonderados > 0 ? 1 : 0;
            break;
    }

    //cria lista sem os dados 2020-03-02	
    var listaDefeitosExiste = $.grep(listaDefeitos, function (obj) {
        return (
            obj.parlevel1_id == level2Resultado.attr('level01id')
            && obj.parlevel2_id == level2Resultado.attr('level02id')
            && obj.parlevel3_id == level3.attr('level03id')
            && obj.period == level2Resultado.attr('period')
            && obj.shift == level2Resultado.attr('shift')
            && obj.evaluate == level2Resultado.attr('evaluate')
            && obj.tipo == 'defeito'
            && parseInt(obj.sample) >= parseInt(level2Resultado.attr('sample'))
        )
    });

    for (i = 0; i < listaDefeitos.length; i++) {
        for (j = 0; j < listaDefeitosExiste.length; j++) {
            if (listaDefeitos[i] == listaDefeitosExiste[j]) {
                listaDefeitos[i] = '';
            }
        }
    }

    listaDefeitosExiste = $.grep(listaDefeitos, function (obj) {
        return (
            obj.parlevel1_id == level2Resultado.attr('level01id')
            && obj.parlevel2_id == level2Resultado.attr('level02id')
            && obj.period == level2Resultado.attr('period')
            && obj.shift == level2Resultado.attr('shift')
            && obj.evaluate == level2Resultado.attr('evaluate')
            && obj.tipo == 'alerta'
            && parseInt(obj.sample) >= parseInt(level2Resultado.attr('sample'))
        );
    });

    for (i = 0; i < listaDefeitos.length; i++) {
        for (j = 0; j < listaDefeitosExiste.length; j++) {
            if (listaDefeitos[i] == listaDefeitosExiste[j]) {
                listaDefeitos[i] = ''
            }
        }
    }

    var parlevel1_id = level2Resultado.attr('level01id');
    var parlevel2_id = level2Resultado.attr('level02id');
    var parlevel3_id = level3.attr('level03id')
    var datetime = level2Resultado.attr('datetime');
    var shift = level2Resultado.attr('shift');
    var period = level2Resultado.attr('period');
    var sample = level2Resultado.attr('sample');
    var evaluate = level2Resultado.attr('evaluate');
    var isKO = JSON.parse($('#' + level3.attr('level03id') + '.level3:visible').attr('isknockout').toLowerCase());

    if (defeitosPonderados > 0) {

        adicionaNaListaDeDefeitos({
            tipo: "defeito"
            , parlevel1_id: level2Resultado.attr('level01id')
            , parlevel2_id: level2Resultado.attr('level02id')
            , datetime: level2Resultado.attr('datetime')
            , shift: level2Resultado.attr('shift')
            , period: level2Resultado.attr('period')
            , sample: level2Resultado.attr('sample')
            , evaluate: level2Resultado.attr('evaluate')
            , parlevel3_id: level3.attr('level03id')
            , isKO: $('#' + level3.attr('level03id') + '.level3:visible').attr('isknockout')
        });

        if ($(_level1).attr('alertanivel3') == "a8")

            listaDeDefeitosAlerta8.push({
                Date: datetime
                , ParLevel1_Id: parlevel1_id
                , ParLevel2_Id: parlevel2_id
                , ParLevel3_Id: parlevel3_id
                , Shift: shift
                , Period: period
                , Evaluate: evaluate
                , Sample: sample
                , IsKO: isKO
            });
    }

    resultado.push(avaliacoesPonderadas, avaliacoes, defeitos, defeitosPonderados, level3Avaliado, level3ComDefeitos);

    return resultado;
}

function setValoresLevel2Alertas(level1, level2, level2Result, mensagemAlerta) {

    var mensagemHtml = "";

    var totalAvaliacoesPonderadas = level2.attr('totalavaliacoesponderadas') != undefined ? parseFloat(level2.attr('totalavaliacoesponderadas')) : 0;
    var totalAvaliacoes = level2.attr('totalavaliacoes') != undefined ? parseFloat(level2.attr('totalavaliacoes')) : 0;
    var totalDefeitos = level2.attr('totaldefeitos') != undefined ? parseFloat(level2.attr('totaldefeitos')) : 0;
    var totalDefeitosPonderados = level2.attr('totaldefeitosPonderados') != undefined ? parseFloat(level2.attr('totaldefeitosPonderados')) : 0;
    var totalLevel3Avaliados = level2.attr('totallevel3avaliados') != undefined ? parseFloat(level2.attr('totallevel3avaliados')) : 0;
    var totalLevel3ComDefeitos = level2.attr('totallevel3comdefeitos') != undefined ? parseFloat(level2.attr('totallevel3comdefeitos')) : 0;

    var resultadoAvaliado = level2.attr('resultadoavaliado') != undefined ? parseFloat(level2.attr('resultadoavaliado')) : 0;
    var resultadoDefeitos = level2.attr('resultadodefeitos') != undefined ? parseFloat(level2.attr('resultadodefeitos')) : 0;

    var evaluateCurrent = $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .painelLevel03 .evaluateCurrent').text();
    var sampleCurrent = $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .painelLevel03 .sampleCurrent').text();

    if ($('.level1.selected').attr('hasgrouplevel2') == "true") {
        evaluateCurrent = $('.level3Group[level1idgroup=' + $('.level1.selected').attr('id') + '] .painelLevel03 .evaluateCurrent').text();
        sampleCurrent = $('.level3Group[level1idgroup=' + $('.level1.selected').attr('id') + '] .painelLevel03 .sampleCurrent').text();
    }

    var numeroDeLevel2 = $('.level01Result.selected').children('.level02Result[level02id=' + level2.attr('id') + '][evaluate=' + evaluateCurrent + '][sample=' + sampleCurrent + ']');
    var numeroDaReauditCorrenteDoLevel1 = $('.level01Result.selected').children('.level02Result[level02id=' + level2.attr('id') + '][evaluate=' + evaluateCurrent +
        '][period=' + $('.App').attr('period') + '][shift=' + $('.App').attr('shift') + '][sample=' + sampleCurrent + ']:last').attr('reauditnumber');
    if (numeroDaReauditCorrenteDoLevel1 > 0) {
        //numeroDaReauditCorrenteDoLevel1 = level1.attr('reauditnumber');
        numeroDeLevel2 = $('.level01Result.selected').children('.level02Result[level02id=' + level2.attr('id') + '][evaluate=' + evaluateCurrent + '][sample=' + sampleCurrent + '][reauditnumber=' + numeroDaReauditCorrenteDoLevel1 + ']');
    }
    //***Para bem estar animal ***////
    //****importante colocar outras opcoes aqui ///****

    if ($('.level1.BEA').hasClass('selected')) {
        numeroDeLevel2 = $('.level01Result.selected').children('.level02Result:last');
    }

    var leve1ConsolidationType = parseInt(level1.attr('parconsolidationtype_id'));

    var resultadoAvaliadoL2 = 0;
    var resultadoDefeitosL2 = 0;

    numeroDeLevel2.each(function (e) {

        //Contadores para o Level2
        var totalAvaliacoesPonderadasL2 = 0;
        var totalAvaliacoesL2 = 0;
        var totalDefeitosL2 = 0;
        var totalDefeitosPonderadosL2 = 0;
        var totalLevel3AvaliadosL2 = 0;
        var totalLevel3ComDefeitosL2 = 0;

        resultadoAvaliadoL2 = 0;
        resultadoDefeitosL2 = 0;

        var level2Resultado = $(this);
        var numeroDeLevel3 = level2Resultado.children('.level03Result');
        //Soma dos atributos do Level3
        numeroDeLevel3.each(function (e) {

            var level3 = $(this);


            //verificar o tipo de input para amostragem om JB testar depois
            if (level3.attr('isnotevaluate') != 'true') {
                var resultadoLevel3 = setValoresLevel3Alertas(level3, level2Resultado);

                level3.attr('weievaluation', (resultadoLevel3[0] > 0 ? resultadoLevel3[0] : 0));
                totalAvaliacoesPonderadas += (resultadoLevel3[0] > 0 ? resultadoLevel3[0] : 0);
                totalAvaliacoesPonderadasL2 += (resultadoLevel3[0] > 0 ? resultadoLevel3[0] : 0);

                totalAvaliacoes += (resultadoLevel3[1] > 0 ? resultadoLevel3[1] : 0);
                totalAvaliacoesL2 += (resultadoLevel3[1] > 0 ? resultadoLevel3[1] : 0);

                totalDefeitos += (resultadoLevel3[3] > 0 ? (resultadoLevel3[2] > 0 ? resultadoLevel3[2] : 0) : 0);
                totalDefeitosL2 += (resultadoLevel3[3] > 0 ? (resultadoLevel3[2] > 0 ? resultadoLevel3[2] : 0) : 0);
                level3.attr('defects', (resultadoLevel3[2] > 0 ? resultadoLevel3[2] : 0));
                level3.attr('weidefects', (resultadoLevel3[3] > 0 ? resultadoLevel3[3] : 0));
                totalDefeitosPonderados += (resultadoLevel3[3] > 0 ? resultadoLevel3[3] : 0);
                totalDefeitosPonderadosL2 += (resultadoLevel3[3] > 0 ? resultadoLevel3[3] : 0);

                totalLevel3Avaliados += (resultadoLevel3[4] > 0 ? resultadoLevel3[4] : 0);
                totalLevel3AvaliadosL2 += (resultadoLevel3[4] > 0 ? resultadoLevel3[4] : 0);

                totalLevel3ComDefeitos += (resultadoLevel3[5] > 0 ? resultadoLevel3[5] : 0);
                totalLevel3ComDefeitosL2 += (resultadoLevel3[5] > 0 ? resultadoLevel3[5] : 0);
            }
            else {
                level3.attr('weievaluation', "0");
                level3.attr('weidefects', "0");
            }
        });

        //totalDefeitosPonderadosL2 = totalDefeitosPonderadosL2 > totalAvaliacoesPonderadasL2 ? totalAvaliacoesPonderadasL2 : totalDefeitosPonderadosL2;

        if ($(_level1).attr('parconsolidationtype_id') != 5) {
            totalDefeitosPonderadosL2 = totalDefeitosPonderadosL2 > totalAvaliacoesPonderadasL2 ? totalAvaliacoesPonderadasL2 : totalDefeitosPonderadosL2;
        } else {
            //totalDefeitosPonderadosL2 = totalDefeitosPonderadosL2 > totalAvaliacoesPonderadasL2 ? totalAvaliacoesPonderadasL2 : totalDefeitosPonderadosL2;
        }

        totalDefeitosPonderados = totalDefeitosPonderadosL2;

        resultadoAvaliado += totalAvaliacoesPonderadasL2 > 0 ? 1 : 0;
        resultadoAvaliadoL2 += totalAvaliacoesPonderadasL2 > 0 ? 1 : 0;
        resultadoDefeitos += totalDefeitosPonderadosL2 > 0 ? 1 : 0;
        resultadoDefeitosL2 += totalDefeitosPonderadosL2 > 0 ? 1 : 0;


        //switch (leve1ConsolidationType) {
        //    case 1:
        //        resultadoAvaliadoL2 = totalAvaliacoesPonderadasL2;
        //        resultadoDefeitosL2 = totalDefeitosPonderadosL2 > totalAvaliacoesPonderadasL2 ? totalAvaliacoesPonderadasL2 : totalDefeitosPonderadosL2;
        //        break;
        //    case 2:
        //        resultadoAvaliadoL2 = totalAvaliacoesPonderadasL2 > 0 ? 1 : 0;
        //        resultadoDefeitosL2 = totalDefeitosPonderadosL2 > 0 ? 1 : 0;
        //        break;
        //    case 3:
        //        resultadoAvaliadoL2 = totalAvaliacoesPonderadasL2;
        //        resultadoDefeitosL2 = totalDefeitosPonderadosL2 > totalAvaliacoesPonderadasL2 ? totalAvaliacoesPonderadasL2 : totalDefeitosPonderadosL2;
        //        break;
        //    default:
        //}

        //TEM QUE SOMAR OS DEFEITOS DO LEVEL1 AQUI POIS NO OUTRO RESULTADO, SOBRESCREVIA
        var totalDefeitosLevel1 = parseFloat(level1.attr('totaldefeitos'));
        var totalDefeitosL1L2 = 0;
        var totalDefeitosL1L2Acumulado = 0;

        switch (leve1ConsolidationType) {
            case 1:
                totalDefeitosLevel1 += totalDefeitosPonderadosL2 > totalAvaliacoesPonderadasL2 ? totalAvaliacoesPonderadasL2 : totalDefeitosPonderadosL2;
                totalDefeitosL1L2 = totalDefeitosPonderadosL2 > totalAvaliacoesPonderadasL2 ? totalAvaliacoesPonderadasL2 : totalDefeitosPonderadosL2;

                totalDefeitosL1L2Acumulado = isNaN(parseFloat(level2.attr('totaldefeitosponderados'))) ? 0 : parseFloat(level2.attr('totaldefeitosponderados'));
                totalDefeitosL1L2Acumulado += totalDefeitosPonderadosL2 > totalAvaliacoesPonderadasL2 ? totalAvaliacoesPonderadasL2 : totalDefeitosPonderadosL2;

                break;
            case 2:
                totalDefeitosLevel1 += resultadoDefeitosL2;
                totalDefeitosL1L2 = resultadoDefeitosL2;
                totalDefeitosL1L2Acumulado = isNaN(parseFloat(level2.attr('totaldefeitos'))) ? 0 : parseFloat(level2.attr('totaldefeitos'));
                totalDefeitosL1L2Acumulado += resultadoDefeitosL2;

                break;
            case 3:
                totalDefeitosLevel1 += totalDefeitosPonderadosL2 > 0 ? 1 : 0;
                totalDefeitosL1L2 = totalDefeitosPonderadosL2 > 0 ? 1 : 0;
                totalDefeitosL1L2Acumulado = isNaN(parseFloat(level2.attr('totaldefeitosponderados'))) ? 0 : parseFloat(level2.attr('totaldefeitosponderados'));
                totalDefeitosL1L2Acumulado += totalDefeitosPonderadosL2 > 0 ? 1 : 0;
                break;
            case 5:
                totalDefeitosLevel1 += totalDefeitosPonderadosL2;
                totalDefeitosL1L2 = totalDefeitosPonderadosL2;
                totalDefeitosL1L2Acumulado = isNaN(parseFloat(level2.attr('totaldefeitosponderados'))) ? 0 : parseFloat(level2.attr('totaldefeitosponderados'));
                totalDefeitosL1L2Acumulado += totalDefeitosPonderadosL2;

                break;
            case 6:
                totalDefeitosLevel1 += totalDefeitosPonderadosL2;
                totalDefeitosL1L2 = totalDefeitosPonderadosL2;
                totalDefeitosL1L2Acumulado = isNaN(parseFloat(level2.attr('totaldefeitosponderados'))) ? 0 : parseFloat(level2.attr('totaldefeitosponderados'));
                totalDefeitosL1L2Acumulado += totalDefeitosPonderadosL2;

                break;
            default:
        }

        //regra de alerta level2 -- comeco

        //Esta regra n?o est? pronta Gabriel 2017-03-09

        //if (level1.attr('hasalert') == "true" && !isLevelReaudit() && 1 == 2) {
        if (parseInt(level2.attr('parnotconformityrule_id')) > 0 && parseInt(level2.attr('parnotconformityrule_id')) < 5) {
            var tipoDeAlerta = $(_level2).attr("parnotconformityrule_id");
            var valorDoAlerta = parseFloat($(_level2).attr("parnotconformityrule_value").replace(',', '.'));

            var alertaatual = level1.attr('alertaatual') == undefined ? 0 : parseInt(level1.attr('alertaatual'));

            var mensagem = "";

            if (totalDefeitosL1L2 > valorDoAlerta) {
                mensagem = getResource("nc_target_exceed") + " (" + (totalDefeitosL1L2).toFixed(2) + ") " + getResource("nc_of") + " (" + valorDoAlerta.toFixed(2) + ") " + getResource("allowed");

                switch (alertaatual) {
                    case 0: mensagem += "<br><br>" + getResource("supervisor_notification");
                        break;
                    case 1: mensagem += "<br><br>" + getResource("supervisor_manager_notification");
                        break;
                    case 2: mensagem += "<br><br>" + getResource("supervisor_manager_director");
                        break;
                    default: mensagem += "<br><br>" + getResource("supervisor_manager_notification");
                        break;
                }

                alertaatual = alertaatual + 1;
                level1.attr('avaliacaoultimoalerta', $('.painel:visible .evaluateCurrent').text());
                level1.attr('monitoramentoultimoalerta', _level2.id);

                if (isEUA == false)
                    openMessageModal(getResource("warning") + " " + getResource("level") + alertaatual + " " + getResource("fired"), mensagem, 'alerta');

                mensagemHtml = $('.message').html();

                level1.attr('alertaatual', alertaatual);

                if (isEUA == true) {

                    if ($(_level1).attr('disparaalerta') == "True") {
                        level1.attr('havecorrectiveaction', 'true');
                    } else {
                        level1.attr('havecorrectiveaction', 'true');
                    }

                }

                if (level2Result) {
                    if (level1.attr('phase1')) {

                        var phase = getPhaseNumber();
                        phase = phase + 1;

                        if (level1.attr('phase' + phase) != undefined) {

                            //var phaseDef = parseInt(level1.attr('phase'+phase).split(';')[2]);
                            //var phaseAlert = getAlertNumber();

                            setPhase(level1.attr('id'), level2.attr('id'), getCollectionDate(), alertaatual, phase, $('.App').attr('period'), $('.App').attr('shift'));
                            level2Result.attr('phase', phase);
                            level2.attr('phase', phase);

                        }
                    }

                    if (level2.attr("reaudit") == "true") {
                        level2.attr('havereaudit', 'true');
                        level2.attr('reauditlevel', '2');
                        level2Result.attr('havereaudit', 'true');
                        level2Result.attr('reauditlevel', '2');
                    } else if ($(_level2).attr('isreaudit') == "true") {
                        reauditnumber = level2Result.attr('reauditnumber') == undefined ? 0 : parseInt(level2Result.attr('reauditnumber'));
                        reauditnumber++;
                    }
                    //level2Result.attr('reauditnumber', reauditnumber);
                    if (isNaN(reauditnumber))
                        reauditnumber = $(_level1).attr('reauditnumber');

                    level2.attr('reauditnumber', reauditnumber);
                }
            }

            else {
                if (level2.attr("isreaudit") == "true") {
                    level2Result.removeAttr('havereaudit');
                    level2.attr('havereaudit', 'false');
                    level2.attr('reauditlevel', '0');
                }
            }

        } else if (parseInt(level2.attr('parnotconformityrule_id')) == 5) {
            var tipoDeAlerta = $(_level2).attr("parnotconformityrule_id");
            var valorDoAlerta = parseFloat($(_level2).attr("parnotconformityrule_value").replace(',', '.'));

            var alertaatual = level1.attr('alertaatual') == undefined ? 0 : parseInt(level1.attr('alertaatual'));

            var mensagem = "";

            if (totalDefeitosL1L2 > valorDoAlerta) {
                mensagem = getResource("nc_target_exceed") + " (" + (totalDefeitosL1L2).toFixed(2) + ") " + getResource("nc_of") + " (" + valorDoAlerta.toFixed(2) + ") " + getResource("allowed");

                switch (alertaatual) {
                    case 0: mensagem += "<br><br>" + getResource("supervisor_notification");
                        break;
                    case 1: mensagem += "<br><br>" + getResource("supervisor_manager_notification");
                        break;
                    case 2: mensagem += "<br><br>" + getResource("supervisor_manager_director");
                        break;
                    default: mensagem += "<br><br>" + getResource("supervisor_manager_notification");
                        break;
                }

                alertaatual = alertaatual + 1;
                level1.attr('avaliacaoultimoalerta', $('.painel:visible .evaluateCurrent').text());
                level1.attr('monitoramentoultimoalerta', _level2.id);

                if (isEUA == false || tipoDeAlerta == 5)
                    openMessageModal(getResource("warning") + " " + getResource("level") + alertaatual + " " + getResource("fired"), mensagem, 'alerta');

                mensagemHtml = $('.message').html();

                level1.attr('alertaatual', alertaatual);

                if (isEUA == true) {

                    if ($(_level1).attr('disparaalerta') == "True") {
                        level1.attr('havecorrectiveaction', 'true');
                    }

                } else {
                    level1.attr('havecorrectiveaction', 'true');
                }

                if (level2Result) {
                    if (level1.attr('phase1')) {

                        var phase = getPhaseNumber();
                        phase = phase + 1;

                        if (level1.attr('phase' + phase) != undefined) {

                            //var phaseDef = parseInt(level1.attr('phase'+phase).split(';')[2]);
                            //var phaseAlert = getAlertNumber();

                            setPhase(level1.attr('id'), level2.attr('id'), getCollectionDate(), alertaatual, phase, $('.App').attr('period'), $('.App').attr('shift'));
                            level2Result.attr('phase', phase);
                            level2.attr('phase', phase);

                        }
                    }

                    if (level2.attr("reaudit") == "true") {
                        level2.attr('havereaudit', 'true');
                        level2.attr('reauditlevel', '2');
                        level2Result.attr('havereaudit', 'true');
                        level2Result.attr('reauditlevel', '2');
                    } else if ($(_level2).attr('isreaudit') == "true") {
                        reauditnumber = level2Result.attr('reauditnumber') == undefined ? 0 : parseInt(level2Result.attr('reauditnumber'));
                        reauditnumber++;
                    }
                    //level2Result.attr('reauditnumber', reauditnumber);
                    if (isNaN(reauditnumber))
                        reauditnumber = $(_level1).attr('reauditnumber');

                    level2.attr('reauditnumber', reauditnumber);
                }

            } else {

                if (level2.attr("isreaudit") == "true") {
                    level2Result.removeAttr('havereaudit');
                    level2.attr('havereaudit', 'false');
                    level2.attr('reauditlevel', '0');
                }

            }

        } else if (parseInt(level2.attr('parnotconformityrule_id')) == 6) {

            var tipoDeAlerta = $(_level2).attr("parnotconformityrule_id");
            var valorDoAlerta = parseFloat($(_level2).attr("parnotconformityrule_value").replace(',', '.'));

            var alertaatual = level1.attr('alertaatual') == undefined ? 0 : parseInt(level1.attr('alertaatual'));

            var mensagem = "";

            var defectsNow = $.grep(defectsPerEvaluation, function (o) {

                return o.level1_id == _level1.id && o.level2_id == _level2.id && o.evaluation == evaluateCurrent


            });

            if (defectsNow.length == 0) {

                defectsPerEvaluation.push({
                    'level1_id': _level1.id, 'level2_id': _level2.id, 'evaluation': evaluateCurrent
                    , 'defects': 0
                });

                defectsNow = $.grep(defectsPerEvaluation, function (o) {

                    return o.level1_id == _level1.id && o.level2_id == _level2.id && o.evaluation == evaluateCurrent

                });

            }

            defectsNow[0].defects = defectsNow[0].defects + totalDefeitosL1L2;

            for (var i = 0; i < defectsPerEvaluation.length; i++) {

                if (defectsPerEvaluation[i].level1_id == _level1.id &&
                    defectsPerEvaluation[i].level2_id == _level2.id &&
                    defectsPerEvaluation[i].evaluation == evaluateCurrent) {

                    defectsPerEvaluation[i].defects = defectsNow[0].defects
                }
            }

            if (resultadoDefeitosL2 > 0 && defectsNow[0].defects > valorDoAlerta) {
                mensagem = getResource("nc_target_exceed") + " (" + (defectsNow[0].defects).toFixed(2) + ") " + getResource("nc_of") + " (" + valorDoAlerta.toFixed(2) + ") " + getResource("allowed");

                switch (alertaatual) {
                    case 0: mensagem += "<br><br>" + getResource("supervisor_notification");
                        break;
                    case 1: mensagem += "<br><br>" + getResource("supervisor_manager_notification");
                        break;
                    case 2: mensagem += "<br><br>" + getResource("supervisor_manager_director");
                        break;
                    default: mensagem += "<br><br>" + getResource("supervisor_manager_notification");
                        break;
                }

                alertaatual = alertaatual + 1;
                level1.attr('avaliacaoultimoalerta', $('.painel:visible .evaluateCurrent').text());
                level1.attr('monitoramentoultimoalerta', _level2.id);

                if (isEUA == false || tipoDeAlerta == 5 || tipoDeAlerta == 6)
                    openMessageModal(getResource("warning") + " " + getResource("level") + alertaatual + " " + getResource("fired"), mensagem, 'alerta');

                mensagemHtml = $('.message').html();

                level1.attr('alertaatual', alertaatual);

                if (isEUA == true) {

                    if ($(_level1).attr('disparaalerta') == "True") {

                        level1.attr('havecorrectiveaction', 'true');
                    }

                } else {
                    level1.attr('havecorrectiveaction', 'true');
                }

                if (level2Result) {
                    if (level1.attr('phase1')) {

                        var phase = getPhaseNumber();
                        phase = phase + 1;

                        if (level1.attr('phase' + phase) != undefined) {

                            //var phaseDef = parseInt(level1.attr('phase'+phase).split(';')[2]);
                            //var phaseAlert = getAlertNumber();

                            setPhase(level1.attr('id'), level2.attr('id'), getCollectionDate(), alertaatual, phase, $('.App').attr('period'), $('.App').attr('shift'));
                            level2Result.attr('phase', phase);
                            level2.attr('phase', phase);

                        }
                    }

                    if (level2.attr("reaudit") == "true") {
                        level2.attr('havereaudit', 'true');
                        level2.attr('reauditlevel', '2');
                        level2Result.attr('havereaudit', 'true');
                        level2Result.attr('reauditlevel', '2');
                    } else if ($(_level2).attr('isreaudit') == "true") {
                        reauditnumber = level2Result.attr('reauditnumber') == undefined ? 0 : parseInt(level2Result.attr('reauditnumber'));
                        reauditnumber++;
                    }
                    //level2Result.attr('reauditnumber', reauditnumber);
                    if (isNaN(reauditnumber))
                        reauditnumber = $(_level1).attr('reauditnumber');

                    level2.attr('reauditnumber', reauditnumber);
                }
            }
            else {
                if (level2.attr("isreaudit") == "true") {
                    level2Result.removeAttr('havereaudit');
                    level2.attr('havereaudit', 'false');
                    level2.attr('reauditlevel', '0');
                }
            }
        }

        var reauditnumber = 0;
        if ($(_level1).attr('isreaudit') == "true") {
            if (isNaN($(_level1).attr('reauditnumber')) == false)
                reauditnumber = $(_level1).attr('reauditnumber');
            if (reauditnumber == 0)
                reauditnumber++;
            if (parseInt($('.level2[completed=completed][reauditevaluation]').length) == parseInt($('.level2').length))
                reauditnumber++;
            level1.attr('reauditnumber', reauditnumber);
            $(_level2).attr('reauditnumber', reauditnumber);
        }

        //regra de alerta level2 -- final

        level1.attr('totaldefeitos', totalDefeitosLevel1);
        level1.attr('defectstotal', totalDefeitosLevel1);
        level1.attr('defectsresultl1', totalDefeitosLevel1);

        if ($('.level1.BEA.selected').length > 0) {
            level2Resultado.attr('weievaluation', $('.pecasAvaliadas:visible').val());
        }
        else {
            level2Resultado.attr('weievaluation', totalAvaliacoesPonderadasL2);
        }

        if ($('.level1.selected.PCC1B').length > 0) {
            setDefectsCounters(
                parseInt($('.level2.selected').attr('id')),
                parseInt(resultadoDefeitosL2),
                null,
                parseInt($('.sequencial:visible').val()),
                parseInt($('.banda:visible').val())
            );
        }

        level2Resultado.attr('weidefects', totalDefeitosPonderadosL2);
        level2Resultado.attr('defects', totalDefeitosL2);
        level2Resultado.attr('totallevel3withdefects', totalLevel3ComDefeitosL2);
        level2Resultado.attr('totalLevel3evaluation', totalLevel3AvaliadosL2);


        //level2Resultado.attr('evaluatedresult', resultadoAvaliadoL2);
        //level2Resultado.attr('defectsresult', resultadoDefeitosL2);

        var totalAvaliadoLevel1 = level1.attr('totalavaliado') != undefined ? parseFloat(level1.attr('totalavaliado')) : 0;
        var totalAvaliadoL1L2 = 0;

        switch (leve1ConsolidationType) {
            case 1:
                totalAvaliadoLevel1 += totalAvaliacoesPonderadasL2;
                totalAvaliadoL1L2 = totalAvaliacoesPonderadasL2;
                break;
            case 2:
                totalAvaliadoLevel1 += resultadoAvaliadoL2;
                totalAvaliadoL1L2 = resultadoAvaliadoL2;
                break;
            case 3:
                totalAvaliadoLevel1 += totalAvaliacoesPonderadasL2 > 0 ? 1 : 0;
                totalAvaliadoL1L2 = totalAvaliacoesPonderadasL2 > 0 ? 1 : 0;
                break;
            default:
        }

        /*
        CONTROLE DE ALERTAS POR AVALIA��O
        */

        var existe = false;
        for (var i = 0; i < _totalAvaliacoesPorIndicadorPorAvaliacao.length; i++) {
            if (_totalAvaliacoesPorIndicadorPorAvaliacao[i].ParLevel1_Id == _level1.id &&
                _totalAvaliacoesPorIndicadorPorAvaliacao[i].CurrentEvaluation == $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .painelLevel03 .evaluateCurrent').text()) {
                _totalAvaliacoesPorIndicadorPorAvaliacao[i].Date = new Date($.now()).toUTCString();
                _totalAvaliacoesPorIndicadorPorAvaliacao[i].Evaluations += totalAvaliadoL1L2;
                _totalAvaliacoesPorIndicadorPorAvaliacao[i].Defects += totalDefeitosL1L2;
                existe = true;
            }
        }

        if (!existe) {
            _totalAvaliacoesPorIndicadorPorAvaliacao.push({
                ParCompany_Id: parseInt($('.App').attr('unidadeid')),
                CurrentEvaluation: $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .painelLevel03 .evaluateCurrent').text(),
                Date: new Date($.now()).toUTCString(),
                ParLevel1_Id: _level1.id,
                Evaluations: totalAvaliadoL1L2,
                Defects: totalDefeitosL1L2
            });
        }

        existe = false;

        for (var i = 0; i < _totalAvaliacoesPorMonitoramentoPorAvaliacao.length; i++) {
            if (_totalAvaliacoesPorMonitoramentoPorAvaliacao[i].ParLevel1_Id == $('.level1.selected').attr('id') &&
                _totalAvaliacoesPorMonitoramentoPorAvaliacao[i].ParLevel2_Id == $('.level2.selected').attr('id') &&
                _totalAvaliacoesPorMonitoramentoPorAvaliacao[i].CurrentEvaluation == $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .painelLevel03 .evaluateCurrent').text()) {
                _totalAvaliacoesPorMonitoramentoPorAvaliacao[i].Date = new Date($.now()).toUTCString();
                _totalAvaliacoesPorMonitoramentoPorAvaliacao[i].Evaluations += totalAvaliacoesL2;
                _totalAvaliacoesPorMonitoramentoPorAvaliacao[i].Defects += totalDefeitosPonderadosL2;
                existe = true;
            }
        }

        if (!existe) {
            _totalAvaliacoesPorMonitoramentoPorAvaliacao.push({
                ParCompany_Id: parseInt($('.App').attr('unidadeid')),
                CurrentEvaluation: $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .painelLevel03 .evaluateCurrent').text(),
                Date: new Date($.now()).toUTCString(),
                ParLevel1_Id: $('.level1.selected').attr('id'),
                ParLevel2_Id: $('.level2.selected').attr('id'),
                Evaluations: totalAvaliacoesL2,
                Defects: totalDefeitosPonderadosL2
            });
        }

        sendTotalAvaliacoesPorIndicadorPorAvaliacao();

        level1.attr('totalavaliado', totalAvaliadoLevel1);

        var resultadoDefeitosLevel1 = level1.attr('resultadodefeitos') != undefined ? parseFloat(level1.attr('resultadodefeitos')) : 0;
        resultadoDefeitosLevel1 += resultadoDefeitosL2;
        level1.attr('resultadodefeitos', resultadoDefeitosLevel1);


        level2.attr('totalavaliacoesponderadas', totalAvaliacoesPonderadas);
        level2Resultado.attr('totalavaliacoesponderadas', totalAvaliacoesPonderadasL2);

        level2.attr('totalavaliacoes', totalAvaliacoes);
        level2Resultado.attr('totalavaliacoes', totalAvaliacoesL2);

        level2.attr('totaldefeitos', totalDefeitos);
        level2Resultado.attr('totaldefeitos', totalDefeitosL2);

        level2.attr('totaldefeitosponderados', totalDefeitosPonderados);
        level2Resultado.attr('totaldefeitosponderados', totalDefeitosPonderadosL2);

        level2.attr('totallevel3avaliados', totalLevel3Avaliados);
        level2Resultado.attr('totallevel3avaliados', totalLevel3AvaliadosL2);

        level2.attr('totalLevel3comdefeitos', totalLevel3ComDefeitos)
        level2Resultado.attr('totallevel3comdefeitos', totalLevel3ComDefeitosL2)

        level2.attr('resultadoavaliado', resultadoAvaliado);
        level2Resultado.attr('resultadoavaliado', resultadoAvaliadoL2);

        level2.attr('resultadodefeitos', resultadoDefeitos)
        level2Resultado.attr('resultadodefeitos', resultadoDefeitosL2)


    });


    //Gera o resultado do monitoramento
    //switch (leve1ConsolidationType) {
    //    case 1:
    //        resultadoAvaliado = totalAvaliacoesPonderadas;
    //        resultadoDefeitos = totalDefeitosPonderados > totalAvaliacoesPonderadas ? totalAvaliacoesPonderadas : totalDefeitosPonderados;
    //        break;
    //    case 2:
    //        resultadoAvaliado = totalAvaliacoesPonderadas > 0 ? 1 : 0;
    //        resultadoDefeitos = totalDefeitosPonderados > 0 ? 1 : 0;
    //        break;
    //    case 3:
    //        resultadoAvaliado = totalAvaliacoesPonderadas;
    //        resultadoDefeitos = totalDefeitosPonderados > totalAvaliacoesPonderadas ? totalAvaliacoesPonderadas : totalDefeitosPonderados;
    //        break;
    //    default:
    //}
    if (mostraDebug == true) {
        //var debug = '<b>totalavaliacoesponderadas: </b>' + totalAvaliacoesPonderadas +
        //         ' | <b>totalavaliacoes: </b>' + totalAvaliacoes +
        //         ' | <b>totaldefeitos: </b>' + totalDefeitos +
        //         ' | <b>totaldefeitosponderados: </b>' + totalDefeitosPonderados +
        //         ' | <b>totallevel3avaliados: </b>' + totalLevel3Avaliados +
        //         ' | <b>totalLevel3comdefeitos: </b>' + totalLevel3ComDefeitos +
        //         ' | <b>resultadoavaliado: </b>' + resultadoAvaliado +
        //         ' | <b>resultadodefeitos: </b>' + resultadoDefeitos;

        //level2.parents('.row').children('.level2Debug').html(debug);

        //debug = $('<div class="debug">' + debug + '</div>');

        //$('.level3Group:visible .debug').remove();

        //$('.level2.selected').parents('.row').append(debug);
        //$('.level3Group:visible .level3:first').before(debug);
    }

    mensagemAlerta.mensagem = mensagemHtml;

    var resultados = [];
    resultados.push(resultadoAvaliado, resultadoDefeitos, resultadoDefeitosL2);

    //Teste de novos Alertas
    // var novosdefeitos = totalDefeitosPonderadosL2;
    // return controleDeAlertaSgq(novosdefeitos);
    return resultados;
}

function setValoresLevel1Alertas(level1, level2, level2Result) {

    //  var totalAvaliado = parseFloat(level1.attr('totalavaliado'));

    //var totalDefeitosLevel1 = parseFloat(level1.attr('totaldefeitos'));

    var alertaNivel1 = parseFloat(level1.attr('alertanivel1'));
    var alertaNivel2 = parseFloat(level1.attr('alertanivel2'));
    var alertaNivel3 = parseFloat(level1.attr('alertanivel3'));

    var alertaAtual = level1.attr('alertaatual') == undefined ? 0 : parseFloat(level1.attr('alertaatual'));

    var resultadoLevel2 = [];

    var mensagemAlerta = { mensagem: "" };
    if (level2) {
        resultadoLevel2 = setValoresLevel2Alertas(level1, level2, level2Result, mensagemAlerta);
    }

    //totalAvaliado = resultadoLevel2[0];


    //totalDefeitos = resultadoLevel2[1];
    //totalDefeitosLevel1 += totalDefeitosLevel1;


    //totalDefeitos = resultadoLevel2[1];
    //totalDefeitosLevel1 += totalDefeitosLevel1;

    //level1.attr('totalavaliado', totalAvaliado);
    //level1.attr('totaldefeitos', totalDefeitosLevel1);

    //setAlertaLevel2(level2, resultadoLevel2);

    setAlertaLevel1(level1, resultadoLevel2, level2Result, mensagemAlerta);

    if (mostraDebug == true) {
        //var debug = '<b>totalavaliadoporindicador: </b>' + level1.attr('totalavaliado') +
        //        ' | <b>totalDefeitosPorIndicador: </b>' + level1.attr('totaldefeitos') +
        //        ' | <b>alertaatual: </b>' + level1.attr('alertaatual');
        ////' | <b>defeitos: </b>' + defeitos +
        ////' | <b>totaldefeitosponderados: </b>' + avaliacaoUltimoAlerta +
        ////' | <b>alertanivel1: </b>' + alertanivel1 +
        ////' | <b>alertanivel2: </b>' + alertanivel2 +
        ////' | <b>alertanivel3: </b>' + alertanivel3 +
        ////' | <b>alertaatual: </b>' + level1.attr('alertaatual') +
        ////' | <b>avaliacaoultimoalerta: </b>' + level1.attr('avaliacaoultimoalerta');

        //debug = $('<div class="debug">' + debug + '</div>');

        //level1.parents('.row').next('.debug').remove();
        //level1.parents('.row').after(debug);
    }

    return resultadoLevel2;
}

function setAlertaLevel1(level1, resultadoLevel2, level2Result, mensagemAlerta) {

    var totalAvaliadoPorMonitoramento = resultadoLevel2[0];
    var totalDefeitosPorMonitoramento = resultadoLevel2[2];

    var defeitosLevel1 = parseFloat($(_level1).attr('totaldefeitos'));

    // if($('.level1.selected').hasClass('PCC1B')){
    //     defeitosLevel1 = parseFloat();
    // }

    var defeitosLevel2 = 0;

    for (var i = 0; i < _totalAvaliacoesPorIndicadorPorAvaliacao.length; i++) {
        if (_totalAvaliacoesPorIndicadorPorAvaliacao[i].ParLevel1_Id == _level1.id &&
            _totalAvaliacoesPorIndicadorPorAvaliacao[i].CurrentEvaluation == $('.painel:visible .evaluateCurrent').text()) {

            defeitosLevel2 = _totalAvaliacoesPorIndicadorPorAvaliacao[i].Defects;

        }
    }

    !defeitosLevel2 ? 0 : defeitosLevel2;

    var tipoDeAlerta = $(_level1).attr("alertanivel3");
    var valorDoAlerta = parseFloat($(_level1).attr("alertanivel2").replace(',', '.'));


    var avaliacaoUltimoAlerta = parseFloat(level1.attr('avaliacaoultimoalerta'));

    //var alertanivel1 = parseFloat(level1.attr('alertanivel1'));
    //var alertanivel2 = parseFloat(level1.attr('alertanivel2'));
    //var alertanivel3 = parseFloat(level1.attr('alertanivel3'));

    var numeroavaliacoes = parseFloat($(_level1).attr('numeroavaliacoes'));
    var metatolerancia = parseFloat($(_level1).attr('metatolerancia'));
    var metadia = parseFloat($(_level1).attr('metadia'));
    var metaavaliacao = parseFloat($(_level1).attr('metaavaliacao'));
    var volumealertaindicador = parseFloat($(_level1).attr('volumealertaindicador').replace(',', '.'));

    var metaIndicador = parseFloat($(_level1).attr('metaindicador').replace(',', '.'));

    var alertaatual = level1.attr('alertaatual') == undefined ? 0 : (parseFloat(level1.attr('alertaatual')) > 0 ? parseFloat(level1.attr('alertaatual')) : 0);

    var disparaalertas = false;
    var mensagem = "";

    var tipoConsolidacao = $(_level1).attr('parconsolidationtype_id');

    var controleDeAlerta = false;

    var controleAlertaCFF = false;

    //se n�o tenho mais monitoramentos n�o completos e n�o � uma reauditoria
    if ($('.level2Group[level01id=' + $('.level1.selected').attr('id') + '] .level2').not('[completed=completed]').length == 0 && !isLevelReaudit()) {
        controleDeAlerta = true;
    } else {
        //se o indicador n�o exigir que complete a avalia��o 
        if ($(_level1).attr('hascompleteevaluation') == "false") {
            /*
                // ( Se a avalia��o ao qual o monitoramento faz parte � maior que a avalia��o do ultimo alerta emitido E tenho defeitos ) OU ( o monitoramento atual � diferente do monitoraemnto que eu disparei o ultimo alerta E tenho defeitos )
                if ((parseInt($('.level3Group[level2id=' + level2.id + '] .evaluateCurrent').text()) > avaliacaoUltimoAlerta && totalDefeitosPorMonitoramento > 0) || (level2.id != $(_level1).attr('monitoramentoultimoalerta') && totalDefeitosPorMonitoramento > 0)) {
                */
            if (totalDefeitosPorMonitoramento > 0) {
                controleDeAlerta = true;
            }
        } else if ($(_level1).attr('hascompleteevaluation') == "true") {
            if (parseInt($('.level3Group[level2id=' + _level2.id + '] .evaluateCurrent').text()) > avaliacaoUltimoAlerta && totalDefeitosPorMonitoramento > 0) {
                controleDeAlerta = true;
            }
        } else {
            controleDeAlerta = true;
        }

        if ($('.level2.selected').attr('havereaudit') == "true" && $('.level1.selected').attr('reaudit') == "true") {
            controleDeAlerta = true;
        }

        if (isEUA == true && _level1.id.includes('9878955')) {
            controleDeAlerta = false;
            controleAlertaCFF = true;
            if ((tdef.text() >= 3 || tdefAv.text() >= 6)) {
                controleDeAlerta = true;
                controleAlertaCFF = true;
            }
        }

        if (tipoDeAlerta == "a7") {

            controleDeAlerta = false;

            mensagem = "";

            //alerta de KO
            //Verifica se existe defeitos do tipo KO na lista 
            var valor = $.grep(listaDefeitos, function (obj) {
                return obj.parlevel1_id == level2Result.attr('level01id')
                    && obj.parlevel2_id == level2Result.attr('level02id')
                    && obj.period == level2Result.attr('period')
                    && obj.shift == level2Result.attr('shift')
                    && obj.isKO == "True"
                    && obj.evaluate == level2Result.attr('evaluate')
                    && obj.tipo == "defeito";
            });

            //Se existe defeito defeitos do tipo KO na lista 
            if (valor.length > 0) {

                //verifica se existe alerta na lista
                var valor = $.grep(listaDefeitos, function (obj) {
                    return obj.parlevel1_id == level2Result.attr('level01id')
                        && obj.parlevel2_id == level2Result.attr('level02id')
                        && obj.period == level2Result.attr('period')
                        && obj.shift == level2Result.attr('shift')
                        && obj.evaluate == level2Result.attr('evaluate')
                        && obj.tipo == "alerta";
                });

                //Se existir alerta
                if (valor.length == 0) {
                    controleDeAlerta = true;
                    //alert("Alerta de KO");
                    mensagem += " Critical ";
                }

                //Adiciona um defeito do tipo alerta
                adicionaNaListaDeDefeitos(
                    {
                        tipo: "alerta"
                        , parlevel1_id: level2Result.attr('level01id')
                        , parlevel2_id: level2Result.attr('level02id')
                        , datetime: level2Result.attr('datetime')
                        , shift: level2Result.attr('shift')
                        , period: level2Result.attr('period')
                        , sample: level2Result.attr('sample')
                        , evaluate: level2Result.attr('evaluate')
                        , parlevel3_id: ""
                        , isKO: ""
                    }
                );

            }

            //alerta de reincidencia
            //Verifica se existe defeito
            var valor = $.grep(listaDefeitos, function (obj) {
                return obj.parlevel1_id == level2Result.attr('level01id')
                    && obj.parlevel2_id == level2Result.attr('level02id')
                    && obj.period == level2Result.attr('period')
                    && obj.shift == level2Result.attr('shift')
                    && obj.evaluate == level2Result.attr('evaluate')
                    && obj.tipo == "defeito";
            });

            var listaAmostraDefeito = [];

            for (var i = 0; i < valor.length; i++) {
                listaAmostraDefeito.push(valor[i].parlevel3_id);
            }

            listaAmostraDefeito.sort();

            var listaAmostraDefeitoUnica = listaAmostraDefeito;

            var contador = 0;

            //Verifica se existe mais de um defeito seguido para a mesma tarefa
            for (var i = 1; i < listaAmostraDefeitoUnica.length; i++) {
                if (contador < 2) {
                    if (listaAmostraDefeitoUnica[i] == listaAmostraDefeito[i - 1]) {
                        contador++;
                    }
                }
            }

            if (contador > 0) {

                var valor = $.grep(listaDefeitos, function (obj) {
                    return obj.parlevel1_id == level2Result.attr('level01id')
                        && obj.parlevel2_id == level2Result.attr('level02id')
                        && obj.period == level2Result.attr('period')
                        && obj.shift == level2Result.attr('shift')
                        && obj.evaluate == level2Result.attr('evaluate')
                        && obj.tipo == "alerta";
                });

                if (valor.length == 0) {
                    controleDeAlerta = true;
                    //alert("Alerta de reincidencia com erros")
                    mensagem += " One recurring defect ";
                }

                adicionaNaListaDeDefeitos(
                    {
                        tipo: "alerta"
                        , parlevel1_id: level2Result.attr('level01id')
                        , parlevel2_id: level2Result.attr('level02id')
                        , datetime: level2Result.attr('datetime')
                        , shift: level2Result.attr('shift')
                        , period: level2Result.attr('period')
                        , sample: level2Result.attr('sample')
                        , evaluate: level2Result.attr('evaluate')
                        , parlevel3_id: ""
                        , isKO: ""
                    }
                );
            }

            //alerta de amostras
            var valor = $.grep(listaDefeitos, function (obj) {
                return obj.parlevel1_id == level2Result.attr('level01id')
                    && obj.parlevel2_id == level2Result.attr('level02id')
                    && obj.period == level2Result.attr('period')
                    && obj.shift == level2Result.attr('shift')
                    && obj.evaluate == level2Result.attr('evaluate')
                    && obj.tipo == "defeito";

            });

            var listaAmostraDefeito = [];

            for (var i = 0; i < valor.length; i++) {
                listaAmostraDefeito.push(valor[i].sample);
            }

            listaAmostraDefeito.sort();

            var listaAmostraDefeitoUnica = [];

            listaAmostraDefeitoUnica.push(listaAmostraDefeito[0]);

            for (var i = 1; i < listaAmostraDefeito.length; i++) {
                if (listaAmostraDefeito[i] != listaAmostraDefeitoUnica[listaAmostraDefeitoUnica.length - 1])
                    listaAmostraDefeitoUnica.push(listaAmostraDefeito[i]);
            }

            if (listaAmostraDefeitoUnica.length > valorDoAlerta) {

                var valor = $.grep(listaDefeitos, function (obj) {
                    return obj.parlevel1_id == level2Result.attr('level01id')
                        && obj.parlevel2_id == level2Result.attr('level02id')
                        && obj.period == level2Result.attr('period')
                        && obj.shift == level2Result.attr('shift')
                        && obj.evaluate == level2Result.attr('evaluate')
                        && obj.tipo == "alerta";
                });

                if (valor.length == 0) {

                    controleDeAlerta = true;
                    //alert("Alerta de amostras com erros")
                    mensagem += listaAmostraDefeitoUnica.length + " defected pieces ";
                }

                adicionaNaListaDeDefeitos(
                    {
                        tipo: "alerta"
                        , parlevel1_id: level2Result.attr('level01id')
                        , parlevel2_id: level2Result.attr('level02id')
                        , datetime: level2Result.attr('datetime')
                        , shift: level2Result.attr('shift')
                        , period: level2Result.attr('period')
                        , sample: level2Result.attr('sample')
                        , evaluate: level2Result.attr('evaluate')
                        , parlevel3_id: ""
                        , isKO: ""

                    }
                );
            }
        }

        //alert8.js
        if (tipoDeAlerta == "a8") {

            debugger
            controleDeAlerta = false;

            if (!hasAlert(level2Result)) {

                var haveAlert = getAlertKO(level2Result, mensagem);
                if (haveAlert) {
                    controleDeAlerta = true;

                    getAlertMessage(tipoDeAlerta, alertaatual, ((defeitosLevel1 / (volumealertaindicador / 100 * metaIndicador)) * metaIndicador).toFixed(2), metaIndicador.toFixed(2));

                } else {

                    haveAlert = getAlertReincidencia(level2Result, mensagem);
                    if (haveAlert) {
                        controleDeAlerta = true;

                        getAlertMessage(tipoDeAlerta, alertaatual, ((defeitosLevel1 / (volumealertaindicador / 100 * metaIndicador)) * metaIndicador).toFixed(2), metaIndicador.toFixed(2));

                    } else {

                        haveAlert = getAlertPorcentageNC(level2Result);
                        if (haveAlert) {
                            controleDeAlerta = true;
                        }
                    }
                }
            }
        }
    }


    if (isEUA == false)
        ZerarContadorDefeito();

    if (controleDeAlerta) {

        acaoCorretivaObrigatoria = false;
        controleDeAlerta = false;

        if (tipoDeAlerta == "a3") {

            valorDoAlerta = (alertaatual + 1) * valorDoAlerta;

            if (defeitosLevel1 / (volumealertaindicador / 100 * metaIndicador) * 100 > valorDoAlerta) {
                //alert("Alerta emitido: " + defeitosLevel1 / (volumealertaindicador /100*metaIndicador) + "% de NC");
                // mensagem = getResource("target_exceed") + (defeitosLevel1 / (volumealertaindicador / 100 * metaIndicador) * 100).toFixed(2) + "% de NC de " + valorDoAlerta.toFixed(2) + " % permitidos)";

                // switch (alertaatual) {
                //     case 0: mensagem += "<br><br>" + getResource("supervisor_notification");
                //         break;
                //     case 1: mensagem += "<br><br>" + getResource("supervisor_manager_notification");
                //         break;
                //     case 2: mensagem += "<br><br>" + getResource("supervisor_manager_director");
                //         break;
                //     default: mensagem += "<br><br>" + getResource("supervisor_manager_notification");
                //         break;
                // }

                mensagem = getAlertMessage(tipoDeAlerta, alertaatual, ((defeitosLevel1 / (volumealertaindicador / 100 * metaIndicador)) * metaIndicador).toFixed(2), metaIndicador.toFixed(2));

                acaoCorretivaObrigatoria = true;
                disparaalertas = true;
            }

        } else if (tipoDeAlerta == "a2") {

            //valorDoAlerta = (alertaatual + 1) * valorDoAlerta;

            if (controleAlertaCFF) {
                //mensagem = getResource("nc_target_exceed") + getResource("allowed");
                mensagem = getAlertMessage("");

                disparaalertas = true;
            }

            else if (defeitosLevel1 > valorDoAlerta) {

                // mensagem = getResource("nc_target_exceed") + " (" + (defeitosLevel1).toFixed(2) + " " + getResource("nc_of") + " " + valorDoAlerta.toFixed(2) + ") " + getResource("allowed").toLowerCase();

                // switch (alertaatual) {
                //     case 0: mensagem += "<br><br>" + getResource("supervisor_notification");
                //         break;
                //     case 1: mensagem += "<br><br>" + getResource("supervisor_manager_notification");
                //         break;
                //     case 2: mensagem += "<br><br>" + getResource("supervisor_manager_director");
                //         break;
                //     default: mensagem += "<br><br>" + getResource("supervisor_manager_notification");
                //         break;
                // }

                mensagem = getAlertMessage(tipoDeAlerta, alertaatual, defeitosLevel1.toFixed(2), parseFloat($(_level1).attr('metatolerancia').replace(',', '.')).toFixed(2), defeitosLevel2.toFixed(2), metaavaliacao.toFixed(2));

                disparaalertas = true;
            }

        } else if (tipoDeAlerta == "a1" || tipoDeAlerta == "a4") {

            switch (alertaatual) {
                case 0:
                    if (defeitosLevel1 > metatolerancia) {

                        //mensagem = getResource("tolerance_target_exceed") + " (" + defeitosLevel1.toFixed(2) + " " + getResource("nc_of") + " " + metatolerancia.toFixed(2) + ") " + getResource("allowed").toLowerCase();
                        mensagem = getAlertMessage(tipoDeAlerta, alertaatual, defeitosLevel1.toFixed(2), parseFloat($(_level1).attr('metatolerancia').replace(',', '.')).toFixed(2), defeitosLevel2.toFixed(2), metaavaliacao.toFixed(2));

                        disparaalertas = true;
                    }
                    break;
                case 1:
                    if (((defeitosLevel1 > metatolerancia) && (defeitosLevel2 >= metaavaliacao)) || (defeitosLevel1 > metadia)) {

                        if (((defeitosLevel1 > metatolerancia) && (defeitosLevel2 >= metaavaliacao))) {
                            mensagem = "Foi excedida a " + (alertaatual + 1) + "� meta toler�ncia (" + defeitosLevel1.toFixed(2) + " pontos NC de " + metatolerancia.toFixed(2) + " pontos permitidos) e meta avalia��o* (" + defeitosLevel2.toFixed(2) + " pontos NC de " + metaavaliacao.toFixed(2) + " pontos permitidos por avalia��o). <br>";
                            mensagem += "<br><br>" + "   * " + (alertaatual + 1) + "� meta toler�ncia corresponde a meta do indicador transformada em pontos. <br>" +
                                "   ** Meta avalia��o corresponde ao n�mero de pontos permitidos por avalia��o e � calculada atrav�s da divis�o da meta do indicador transformada em pontos pelo n�mero de avalia��es por monitoramento. <br>";

                            // mensagem = getResource("tolerance_target_exceed") + " (" + defeitosLevel1.toFixed(2) + " " + getResource("nc_of") + " " + metatolerancia.toFixed(2) + ") " + getResource("allowed").toLowerCase();
                            // mensagem += getResource("evaluation_target") + " (" + defeitosLevel2.toFixed(2) + " " + getResource("nc_of") + " " + metaavaliacao.toFixed(2) + getResource("allowed") + " <br>";
                        } else {
                            mensagem = "";
                        }

                        if ((defeitosLevel1 > metadia)) {
                            mensagem += "<br><br>" + "A meta di�ria* foi excedida (" + defeitosLevel1.toFixed(2) + " pontos NC de " + metadia.toFixed(2) + " pontos permitidos). <br>";
                            //mensagem += getResource("day_target_exceed") + " (" + defeitosLevel1.toFixed(2) + " " + getResource("nc_of") + " " + metadia.toFixed(2) + ") " + getResource("allowed").toLowerCase();
                            mensagem += "<br><br>" + "   * A meta di�ria corresponde a meta do indicador transformada em pontos. <br>";

                        }

                        mensagem += "<br><br>" + getResource("supervisor_manager_notification");
                        //mensagem = getAlertMessage(tipoDeAlerta, alertaatual, defeitosLevel1.toFixed(2), metatolerancia.toFixed(2), defeitosLevel2.toFixed(2), metaavaliacao.toFixed(2));

                        disparaalertas = true;
                    }
                    break;
                case 2:
                    if ((defeitosLevel1 > metatolerancia) && (defeitosLevel2 >= metaavaliacao)) {
                        // mensagem = getResource("tolerance_target_exceed") + " (" + defeitosLevel1.toFixed(2) + " " + getResource("nc_of") + " " + metatolerancia.toFixed(2) + ") " + getResource("allowed").toLowerCase() + " <br>";
                        // mensagem += getResource("evaluation_target") + " (" + defeitosLevel2.toFixed(2) + " " + getResource("nc_of") + " " + metaavaliacao.toFixed(2) + ") " + getResource("allowed").toLowerCase();

                        // mensagem += "<br><br>" + getResource("supervisor_manager_director");

                        mensagem = getAlertMessage(tipoDeAlerta, alertaatual, defeitosLevel1.toFixed(2), metatolerancia.toFixed(2), defeitosLevel2.toFixed(2), metaavaliacao.toFixed(2));

                        disparaalertas = true;
                    }
                    break;
                default:
                    if ((defeitosLevel1 > metatolerancia) && (defeitosLevel2 >= metaavaliacao)) {
                        // mensagem = getResource("tolerance_target_exceed") + " (" + defeitosLevel1.toFixed(2) + " " + getResource("nc_of") + " " + metatolerancia.toFixed(2) + ") " + getResource("allowed").toLowerCase() + " <br>";
                        // mensagem += getResource("evaluation_target") + " (" + defeitosLevel2.toFixed(2) + " " + getResource("nc_of") + " " + metaavaliacao.toFixed(2) + ") " + getResource("allowed").toLowerCase();

                        // mensagem += "<br><br>" + getResource("supervisor_manager_notification");

                        mensagem = getAlertMessage(tipoDeAlerta, alertaatual, defeitosLevel1.toFixed(2), metatolerancia.toFixed(2), defeitosLevel2.toFixed(2), metaavaliacao.toFixed(2));
                        disparaalertas = true;
                    }
                    break;
            }
        } else if (tipoDeAlerta == "a5") {

            switch (alertaatual) {
                case 0:
                    if (defeitosLevel1 > metatolerancia) {

                        //mensagem = getResource("tolerance_target_exceed") + " (" + defeitosLevel1.toFixed(2) + " " + getResource("nc_of") + " " + metatolerancia.toFixed(2) + ") " + getResource("allowed").toLowerCase();
                        mensagem = getAlertMessage(tipoDeAlerta, alertaatual, defeitosLevel1.toFixed(2), parseFloat($(_level1).attr('metatolerancia').replace(',', '.')).toFixed(2), defeitosLevel2.toFixed(2), metaavaliacao.toFixed(2));

                        disparaalertas = true;
                    }
                    break;
                case 1:
                    if (((defeitosLevel1 > metatolerancia) && (defeitosLevel2 >= metaavaliacao)) || (defeitosLevel1 > metadia)) {

                        if (((defeitosLevel1 > metatolerancia) && (defeitosLevel2 >= metaavaliacao))) {
                            mensagem = "Foi excedida a " + (alertaatual + 1) + "� meta toler�ncia (" + defeitosLevel1.toFixed(2) + " pontos NC de " + metatolerancia.toFixed(2) + " pontos permitidos) e meta avalia��o* (" + defeitosLevel2.toFixed(2) + " pontos NC de " + metaavaliacao.toFixed(2) + " pontos permitidos por avalia��o). <br>";
                            mensagem += "<br><br>" + "   * " + (alertaatual + 1) + "� meta toler�ncia corresponde a meta do indicador transformada em pontos. <br>" +
                                "   ** Meta avalia��o corresponde ao n�mero de pontos permitidos por avalia��o e � calculada atrav�s da divis�o da meta do indicador transformada em pontos pelo n�mero de avalia��es por monitoramento. <br>";

                            // mensagem = getResource("tolerance_target_exceed") + " (" + defeitosLevel1.toFixed(2) + " " + getResource("nc_of") + " " + metatolerancia.toFixed(2) + ") " + getResource("allowed").toLowerCase();
                            // mensagem += getResource("evaluation_target") + " (" + defeitosLevel2.toFixed(2) + " " + getResource("nc_of") + " " + metaavaliacao.toFixed(2) + getResource("allowed") + " <br>";
                        } else {
                            mensagem = "";
                        }

                        if ((defeitosLevel1 > metadia)) {
                            mensagem += "<br><br>" + "A meta di�ria* foi excedida (" + defeitosLevel1.toFixed(2) + " pontos NC de " + metadia.toFixed(2) + " pontos permitidos). <br>";
                            //mensagem += getResource("day_target_exceed") + " (" + defeitosLevel1.toFixed(2) + " " + getResource("nc_of") + " " + metadia.toFixed(2) + ") " + getResource("allowed").toLowerCase();
                            mensagem += "<br><br>" + "   * A meta di�ria corresponde a meta do indicador transformada em pontos. <br>";

                        }

                        mensagem += "<br><br>" + getResource("supervisor_manager_notification");
                        //mensagem = getAlertMessage(tipoDeAlerta, alertaatual, defeitosLevel1.toFixed(2), metatolerancia.toFixed(2), defeitosLevel2.toFixed(2), metaavaliacao.toFixed(2));

                        disparaalertas = true;
                    }
                    break;
                case 2:
                    if ((defeitosLevel1 > metatolerancia) && (defeitosLevel2 >= metaavaliacao)) {
                        // mensagem = getResource("tolerance_target_exceed") + " (" + defeitosLevel1.toFixed(2) + " " + getResource("nc_of") + " " + metatolerancia.toFixed(2) + ") " + getResource("allowed").toLowerCase() + " <br>";
                        // mensagem += getResource("evaluation_target") + " (" + defeitosLevel2.toFixed(2) + " " + getResource("nc_of") + " " + metaavaliacao.toFixed(2) + ") " + getResource("allowed").toLowerCase();

                        // mensagem += "<br><br>" + getResource("supervisor_manager_director");

                        mensagem = getAlertMessage(tipoDeAlerta, alertaatual, defeitosLevel1.toFixed(2), metatolerancia.toFixed(2), defeitosLevel2.toFixed(2), metaavaliacao.toFixed(2));

                        disparaalertas = true;
                    }
                    break;
                default:
                    if ((defeitosLevel1 > metatolerancia) && (defeitosLevel2 >= metaavaliacao)) {
                        // mensagem = getResource("tolerance_target_exceed") + " (" + defeitosLevel1.toFixed(2) + " " + getResource("nc_of") + " " + metatolerancia.toFixed(2) + ") " + getResource("allowed").toLowerCase() + " <br>";
                        // mensagem += getResource("evaluation_target") + " (" + defeitosLevel2.toFixed(2) + " " + getResource("nc_of") + " " + metaavaliacao.toFixed(2) + ") " + getResource("allowed").toLowerCase();

                        // mensagem += "<br><br>" + getResource("supervisor_manager_notification");

                        mensagem = getAlertMessage(tipoDeAlerta, alertaatual, defeitosLevel1.toFixed(2), metatolerancia.toFixed(2), defeitosLevel2.toFixed(2), metaavaliacao.toFixed(2));
                        disparaalertas = true;
                    }
                    break;
            }

        } else if (tipoDeAlerta == "a7") {

            //alert("Teste de Deus!");

            //mensagem = "Falha USA Fred"
            disparaalertas = true;

        }
        else if (tipoDeAlerta == "a8") {

            disparaalertas = true;

        }
    }

    var mensagemHtml = mensagemAlerta.mensagem;

    if (disparaalertas == true) {


        alertaatual = alertaatual + 1;
        level1.attr('avaliacaoultimoalerta', $('.painel:visible .evaluateCurrent').text());
        var _level02id = 0;
        if (_level2) {
            _level02id = _level2.id;
        }
        level1.attr('monitoramentoultimoalerta', _level02id);

        if (level1.attr('hasalert') == "true") {

            if (controleAlertaCFF) {
                if (isEUA == false)
                    openMessageModal(getResource("warning") + " " + getResource("fired"), mensagem, 'alerta');
            }
            else {
                //if (isEUA == false)
                openMessageModal(getResource("warning") + " " + alertaatual + " " + getResource("fired"), mensagem, 'alerta');
            }

            mensagemHtml = $('.message').html();

            disparaalertas = false;
            level1.attr('alertaatual', alertaatual);

            if (isEUA != true || (isEUA == true && $(_level1).attr('disparaalerta') == "True")) {

                level1.attr('havecorrectiveaction', 'true');

                if (level1.attr("reaudit") == "true") {
                    level1.attr('havereaudit', 'true');

                    level2Result.attr('havereaudit', 'true');
                    level2Result.attr('reauditlevel', '1');

                    var level3Group = $('.level3Group[level1id=' + $('.level1.selected').attr('id') + '][level2id=' + $('.level2.selected').attr('id') + ']');

                    level3Group.find('select[linknumberevaluetion=true]').each(
                        function (i, e) {
                            ReauditByHeader.SetReaudit(
                                $(e).attr('parheaderfield_id'), $(e).val(),
                                level3Group.find('.headerEvaluationCount').text(),
                                level3Group.find('.headerSampleCount').text(), 1,
                                level2Result
                            );
                        }
                    );

                    if (level1.attr('isreaudit') != 'true') {
                        var reauditnumber = level2Result.attr('reauditnumber') == undefined ? 0 : parseInt(level2Result.attr('reauditnumber'));
                        level2Result.attr('reauditnumber', reauditnumber);
                    }

                    $('.level2.selected').attr('havereaudit', 'true');
                }
            }

        }
    } else {

        // if (level1.attr("isreaudit") == "true") {
        //     level2Result.attr('reauditlevel', '1');
        // }

    }

    setGravaAlertaDBLocal(level1, alertaatual, defeitosLevel1, mensagemHtml);

}

function setGravaAlertaDBLocal(level1, alertaatual, defeitos, mensagem) {

    var level1 = $('.level1.selected');

    if (typeof (level1.attr('id')) == 'undefined') {
        level1 = $(_level1);
    }

    var level2 = $('.level2.selected');

    var evaluateCurrent = $('.level3Group[level1id=' + $('.level1.selected').attr('id') + '][level2id=' + $('.level2.selected').attr('id') + '] .evaluateCurrent').text();
    var sampleCurrent = $('.level3Group[level1id=' + $('.level1.selected').attr('id') + '][level2id=' + $('.level2.selected').attr('id') + '] .sampleCurrent').text();

    var unidadeId = $('.App').attr('unidadeid');
    var period = $('.App').attr('period');
    var shift = $('.App').attr('shift');
    var date = $('.App').attr('date');
    var level1Id = level1.attr('id');
    var level2Id = level2.attr('id');

    //verificar level2 com gabriel	
    var deviation = '<div class="deviation" parcompany_id="' + unidadeId + '" period="' + period + '" shift="' + shift + '" collectiondate="' + date + '" parlevel1_id="' + level1Id + '" parlevel2_id="' + level2Id + '" evaluation="' + evaluateCurrent + '" sample="' + sampleCurrent + '" alertnumber="' + alertaatual + '" defects="' + defeitos + '" deviationdate="' + dateTimeFormat() + '" sync="false">' +
        '<div class="messageDeviation">' + mensagem + '</div>' +
        '</div>';

    apagaDeviationDuplicada(unidadeId, period, shift, date, level1Id, level2Id, evaluateCurrent, sampleCurrent);

    var deviations = $('.Deviations');

    var nc = parseInt(level2.attr('nc'));
    var level2NcLocal = parseInt(level2.attr('nclocal'));

    nc++;
    level2NcLocal++;
    level2.attr('nclocal', level2NcLocal);

    appendDevice(deviation, deviations);

    _writeFile("deviations.txt", $('.Deviations').html(), sendDeviations);
    //gravar no banco
    //api para enviar e-mail
}

function geraAlerta(level2) {
    //melhorar
    var defeitosLevel = parseFloat(level2.attr('defects'));
    var alertlevel = parseInt(level2.attr('alertlevel'));

    var defeitosPermitidos = 0;

    var defeitosLevel1 = parseFloat(level2.attr('alertlevel1'));
    var defeitosLevel2 = parseFloat(level2.attr('alertlevel2'));
    var defeitosLevel3 = parseFloat(level2.attr('alertlevel3'));

    if (alertlevel == 0) {
        defeitosPermitidos = defeitosLevel1;
    }
    else if (alertlevel == 1) {
        defeitosPermitidos = defeitosLevel2;
    }
    else if (alertlevel == 2) {
        defeitosPermitidos = defeitosLevel3;
    }
    else if (alertlevel > 3) {
        defeitosPermitidos = defeitosLevel1 * alertlevel;
    }

    if (defeitosLevel > defeitosPermitidos) {
        alertlevel++;
    }

    level2.attr('alertlevel', alertlevel);
}

var divDebugAlertasVisivel = 0;
function showHideDivDebugAlerta() {
    if (divDebugAlertasVisivel == 0) {
        $('#divDebugAlertas').show();
        divDebugAlertasVisivel = 1;
    } else {
        $('#divDebugAlertas').hide();
        divDebugAlertasVisivel = 0;
    }

}

function showDebugAlertas() {

    if (_level1 != null) {

        var ncporav = "";

        for (var i = 0; i < _totalAvaliacoesPorIndicadorPorAvaliacao.length; i++) {
            if (_totalAvaliacoesPorIndicadorPorAvaliacao[i].ParLevel1_Id == parseFloat(_level1.id)) {
                ncporav += "Av: " + _totalAvaliacoesPorIndicadorPorAvaliacao[i].CurrentEvaluation + " NC: " + _totalAvaliacoesPorIndicadorPorAvaliacao[i].Defects + " | ";
            }
        }

        //console.log(_level1);
        $('#divDebugAlertas #level1').html($('.breadcrumb:visible .active').text());
        $('#divDebugAlertas #volumeTotal').html(parseFloat($(_level1).attr('volumealertaindicador')).toFixed(2));
        $('#divDebugAlertas #meta').html(parseFloat($(_level1).attr('metaindicador')).toFixed(2) + '%');
        $('#divDebugAlertas #metaTolerancia').html(parseFloat($(_level1).attr('metatolerancia')).toFixed(2));
        $('#divDebugAlertas #metaDia').html(parseFloat($(_level1).attr('metadia')).toFixed(2));
        $('#divDebugAlertas #metaAvaliacao').html(parseFloat($(_level1).attr('metaavaliacao')).toFixed(2));
        $('#divDebugAlertas #totalAv').html(parseFloat($(_level1).attr('totalavaliado')).toFixed(2));
        $('#divDebugAlertas #totalNc').html(parseFloat($(_level1).attr('totaldefeitos')).toFixed(2));
        $('#divDebugAlertas #totalNcNaAvalicao').html(ncporav);
    }

}


function mostraDebugAlertas(aparecer) {

    //level1 = $('.level1');
    //level1.each(function (index, self) {
    //    $(level1[index]).find('.levelName').text($(level1[index]).find('.levelName').text() + ' Volume: ' + ($(level1[index]).attr('volumealertaindicador')) + ' Meta: ' + ($(level1[index]).attr('metaindicador')) + ' N�veis Alertas: N�vel 01 - ' + ($(level1[index]).attr('alertanivel1')) + ' | N�vel 02 - ' + ($(level1[index]).attr('alertanivel2')) + ' | N�vel 03 - ' + ($(level1[index]).attr('alertanivel3')));
    //    //console.log($(level1[index]));
    //});
    level3 = $('.level3');

    if (aparecer == 0) {
        level3.each(function (index, self) {
            $(level3[index]).find('.levelNameDebug').text('');
            $('#ControlaDivDebugAlertas').hide();
            $('#divDebugAlertas').hide();
            //console.log($(level3[index]));
        });
    } else {
        level3.each(function (index, self) {
            $(level3[index]).find('.levelNameDebug').text(' Peso: ' + parseFloat($(level3[index]).attr('weight')).toFixed(0));
            $('#ControlaDivDebugAlertas').show();
            $('#divDebugAlertas').hide();
            //console.log($(level3[index]));
        });
    }


}

function level03AlertAdd(input) {
    var valor = parseInt(input.val());
    if (valor > 0) {
        input.parents('li, div.level03').addClass('bgAlert');
    }
    else {
        input.parents('li, div.level03').removeClass('bgAlert');
    }
}

function sendDeviations() {
    ping(setGravaAlerta);
}

function setGravaAlerta() {
    var deviations = $('.Deviations .deviation[sync=false]');

    var deviationsSend = "";
    deviations.each(function (e) {
        var deviation = $(this);
        deviation.attr('send', 'true');

        var result = $('.App').attr('unidadeid'); // 0
        result += ";" + deviation.attr('parlevel1_id'); // 1
        result += ";" + deviation.attr('parlevel2_id');// 2
        result += ";" + deviation.attr('evaluation');// 3
        result += ";" + deviation.attr('sample');// 4
        result += ";" + deviation.attr('alertnumber');// 5
        result += ";" + deviation.attr('defects');// 6
        result += ";" + deviation.attr('deviationdate');// 7
        result += ";" + encodeURI(deviation.children('.message').text().replace(/ +(?= )/g, '')).replace(/'/g, '');// 8
        result += ";" + deviation.attr('period');// 9
        result += ";" + deviation.attr('shift');// 10
        result += ";" + deviation.attr('collectiondate');// 11
        //result += ";" + "alerta";// 7

        deviationsSend += "<deviation>" + result + "</deviation>";
    });

    $.ajax({
        type: 'POST'
        , url: urlPreffix + '/api/SyncServiceApi/insertDeviation'
        , contentType: 'application/json; charset=utf-8'
        , headers: token()
        , dataType: 'json'
        , data: "{deviations : '" + deviationsSend + "'}"
        //, data: '{' + "obj: '" + objectSend + "', collectionDate : '" + level02.attr('datetime') + "', level01Id: '" + level01.attr('level01Id') + "', level02Id: '" + level02.attr('level02id') + "', unitId: '" + level01.attr('unidadeid') + "', period: '" + level01.attr('period') + "', shift: '" + level01.attr('shift') + "', device: '123', version: '" + versao + "', ambient: '" + baseAmbiente + "'" + '}'
        , async: false //blocks window close
        , success: function (data, status) {
            if (data != null && data == "error") {
                //createLog(XMLHttpRequest.responseText);
                //console.log(XMLHttpRequest.responseText);
            }
            else {
                $('.deviation[sync=false][send=true]').removeAttr('send').attr('sync', 'true');
                $('.deviation[sync=true]').remove();
                _writeFile("deviations.txt", $('.Deviations').html());
                setTimeout(function (e) {
                    setEnviaEmail();
                }, 300)
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //createLog(XMLHttpRequest.responseText);
            //console.log(XMLHttpRequest.responseText);
        }
    });

}

$(document).on('change', 'select#selectCattleType', function (e) {
    $('span.cattleType').html($("select#selectCattleType :selected").text());
});

//Função criada para "corrigir" um problema dos USA - (Repetindo a mesma av e estourando alerta) - 02-03-2020	
function apagaDeviationDuplicada(unidadeId, period, shift, date, level1Id, level2Id, evaluateCurrent, sampleCurrent) {
    var deviations = $('.Deviations');
    var deviationCurrent = deviations.find('[parcompany_id="' + unidadeId + '"][period="' + period + '"][shift="' + shift + '"][collectiondate="' + date + '"][parlevel1_id="' + level1Id + '"][parlevel2_id="' + level2Id + '"][evaluation="' + evaluateCurrent + '"]');
    $.each(deviationCurrent, function () {
        //apagar amostras que forem maior ou igual a esta	
        if (parseInt($(this).attr('sample')) >= sampleCurrent)
            $(this).remove();
    });
}