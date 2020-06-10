currentEvaluationDCA = {};
var currentParLevel2DCATotalPorcentagem = 0;
function listarParLevel2DCA(isVoltar, pularParaProximaAvaliacao) {

    currentParLevel2_Id = null;

    var listaParLevel2 = retornaParLevel2DCA(currentParLevel1_Id);
    listaParLevel2.sort((a, b) => a.Name.localeCompare(b.Name));

    var htmlLista = "";
    var btnProximaAvaliacao = '<button class="btn btn-block btn-primary input-lg col-xs-12" data-proxima-av style="margin-top:10px">Próxima Avaliação</button>';

    var quantidadeDeLevel2ComPeso = 0;
    var porcentagemTotal = 0;
    var avaliacaoAtual = 0;
    $(listaParLevel2).each(function (i, o) {

        currentEvaluationDCA = getResultEvaluationDCA(currentParLevel1_Id, o.Id);
        
        if(avaliacaoAtual == 0 || currentEvaluationDCA.Evaluation < avaliacaoAtual){
            avaliacaoAtual = currentEvaluationDCA.Evaluation;
        }

        var parVinculoPesoLevel2 = getParVinculoPesoParLevel2PorIndicador(o.Id,currentParLevel1_Id);
        quantidadeDeLevel2ComPeso += parVinculoPesoLevel2.Peso;
    });

    var foiRealizadaColetaParaAProximaAvaliacao = $.grep(coletasDCA, function (o) {

        return o.ParLevel1_Id == currentParLevel1_Id &&
            avaliacaoAtual < o.Evaluation &&
            o.Outros.indexOf('ParFamiliaProduto_Id:'+currentFamiliaProdutoDCA_Id+',') > 0;

    });

    if(foiRealizadaColetaParaAProximaAvaliacao.length > 0 || pularParaProximaAvaliacao == true){
        avaliacaoAtual++;
        currentEvaluationDCA.Evaluation++;
    }

    $(listaParLevel2).each(function (i, o) {

        var calculoPorMonitoramento = getCalculoPorMonitoramento(currentParLevel1_Id, o.Id, avaliacaoAtual);

        var style = '';

        if (calculoPorMonitoramento.ColetasSincronizadas) {
            style = 'style="background-color:#ddd;cursor:not-allowed"';
        }else{
            btnProximaAvaliacao = '';
        }

        var porcentagemTotalConsiderandoPeso = ZeroSeForNaN((calculoPorMonitoramento.ParVinculoPesoParLevel2.Peso/quantidadeDeLevel2ComPeso)*100);
        var porcentagemAtualConsiderandoPeso = ZeroSeForNaN((calculoPorMonitoramento.Porcentagem / 100) * porcentagemTotalConsiderandoPeso);
        porcentagemTotal += ZeroSeForNaN(porcentagemAtualConsiderandoPeso);

        htmlLista += '<button type="button" ' + style + ' class="list-group-item col-xs-12" ' +
            '" data-dca-par-level2-id="' + o.Id + '" ' +
            'data-current-evaluation="' + avaliacaoAtual + '"                       ' +
            'data-total-porcentagem="' + porcentagemTotalConsiderandoPeso + '">                       ' +
            '	<div class="col-xs-4">' + o.Name + '</div>                                      ' +
            '	<div class="col-xs-4 text-center">Conforme: ' + porcentagemAtualConsiderandoPeso+'% / '+porcentagemTotalConsiderandoPeso+'%</div>      ' +
            '	<div class="col-xs-4 text-center">Respondido: '+ZeroSeForNaN(parseInt(calculoPorMonitoramento.AmostraTotalColetada/calculoPorMonitoramento.AmostraTotal*100))+'%</div>              ' +
            '</button>';
    });

    var voltar = '<a onclick="listarParLevel1(' + isVoltar + ');" class="btn btn-warning">Voltar</a>';

    html = getHeader() +
        '<div class="container-fluid">                                           ' +
        '	<div class="">                                              ' +
        '		<div class="col-xs-12">                                    ' +
        '			<div class="panel panel-primary">                      ' +
        '			  <div class="panel-heading">                          ' +
        '				<h3 class="panel-title">' + voltar + ' Selecione o Monitoramento/SKU para coletar</h3>            ' +
        '			  </div>                                               ' +
        '			  <div class="panel-body">                             ' +
        '               <div class="col-sm-12 text-center" style="padding:20px;margin-bottom:5px">Item: '+getParFamiliaProduto().Name+'</div>' +
        '               <div class="col-sm-12 btn-warning text-center" style="padding:20px;margin-bottom:5px">Avaliação ' + avaliacaoAtual + '</div>' +
        '               <h2 class="col-xs-6 btn-info text-center" style="height:100px;padding-top: 35px;margin: 0px;">'+porcentagemTotal+'%</h2>' +
        '               <div class="col-xs-6 text-center" style="padding:0px !important">' +
        getSelectProdutosDCA()+
        '</div>' +
        '<div class="col-sx-12">' +
        getParHeaderFieldLevel1({ Id: currentParLevel1_Id }) +
        '</div>' +
        '				<div class="list-group" style="padding-top:5px;clear:both !important">                           ' +
        htmlLista +
        btnProximaAvaliacao +
        '				</div>                                             ' +
        '			  </div>                                               ' +
        '			</div>                                                 ' +
        '		</div>                                                     ' +
        '	</div>                                                         ' +
        '</div>';

    $('div#app').html(html);

    setBreadcrumbsDCA();
    $('select[name="produtoDCA"]').trigger('change');

    if (Object.keys(objCabecalhoLevel1).length !== 0) {
        setObjectToForm(objCabecalhoLevel1);
    }
}

$('body').off('click', '[data-dca-par-level2-id]').on('click', '[data-dca-par-level2-id]', function (e) {

    if (!hederFieldIsValid("#headerFieldLevel1")) {
        openMensagem("Existem cabeçalhos obrigatórios não preenchidos!","blue", "white");
        setTimeout(closeMensagem, 3000);
        return false;
    }

    currentParLevel2_Id = parseInt($(this).attr('data-dca-par-level2-id'));
    currentParLevel2DCATotalPorcentagem = parseInt($(this).attr('data-total-porcentagem'));

    currentTotalEvaluationValue = $(this).attr('data-total-evaluation');
    currentTotalSampleValue = $(this).attr('data-total-sample');
    //var currentEvaluationValue = $(this).attr('data-current-evaluation');

    var objCabecalho = serializeFormToObject("#headerFieldLevel1");

    listarParLevelsDCA(objCabecalho);

});

function retornaParLevel2DCA(parLevel1Id) {

    var listaLevel2 = $.grep(parametrization.listaParVinculoPeso, function (item) {
        return item.ParLevel1_Id == parLevel1Id;
    });

    var parLevel2Ids = $.map(listaLevel2, function (obj) {
        return obj.ParLevel2_Id;
    });

    parLevel2Ids = parLevel2Ids.sort();

    parLevel2Ids = $.unique(parLevel2Ids);

    var listaParLevel2 = [];

    $(parLevel2Ids).each(function (index, parLevel2Id) {

        var listaParLevel2Filter = $.grep(parametrization.listaParLevel2, function (obj) {
            return (obj.Id == parLevel2Id);
        });

        listaParLevel2Filter.forEach(function (item) {

            //Verificar primeiramente os que existem ParCargo e ParCompany obs (frequencia e departamento são obrigatorios)
            var listaEvaluation = [];

            //Caso não existir, buscar os que possuem todas as unidades
            listaEvaluation = $.grep(parametrization.listaParVinculoPeso, function (parEvaluation) {
                return parEvaluation.ParLevel1_Id == parLevel1Id
            });

            if (listaEvaluation.length > 0) {
                item['Evaluation'] = listaEvaluation[0]; //o correto é que retorne somente um, mas caso retorne mais do que um, não pode dar erro
                listaParLevel2.push(item);
            }
        });
    });

    return listaParLevel2;
}

function getAmostraTotalEColetadaEConformePorMonitoramento(parLevel1, parLevel2, avaliacao) {

    var tarefasVinculadas = $.grep(parametrization.listaParVinculoPeso, function (o) {
        return o.ParLevel1_Id == parLevel1.Id && o.ParLevel2_Id == parLevel2.Id;
    });

    var avaliacaoAtual = avaliacao;
    var totalDeAmostras = 0;
    var totalDeAmostrasColetadas = 0;
    var totalDeAmostrasColetadasConforme = 0;
    var coletasSincronizadas = false;
    $(tarefasVinculadas).each(function (i, o) {
        var vinculoPeso = o;
        var quantidadeDeColetasPorTarefa = $.grep(coletasDCA, function (coletas) {
            if(coletas.ParLevel1_Id == parLevel1.Id 
                && coletas.ParLevel2_Id == parLevel2.Id 
                && coletas.ParLevel3_Id == vinculoPeso.ParLevel3_Id
                && coletas.Evaluation == avaliacaoAtual
                && coletas.Outros.indexOf('ParFamiliaProduto_Id:'+currentFamiliaProdutoDCA_Id+',') > 0){
                totalDeAmostrasColetadasConforme += coletas.IsConform ? 1 : 0;
                return true;
            }
        });

        if(coletasSincronizadas == false && quantidadeDeColetasPorTarefa.length > 0)
            coletasSincronizadas = quantidadeDeColetasPorTarefa[0].Synced == true;

        if (o.Sample > 0 && quantidadeDeColetasPorTarefa.length > 0) {
            if (o.Sample > quantidadeDeColetasPorTarefa.length) {
                totalDeAmostrasColetadas += quantidadeDeColetasPorTarefa.length;
            } else {
                totalDeAmostrasColetadas += o.Sample;
            }
        }else if(quantidadeDeColetasPorTarefa.length > 0){
            totalDeAmostrasColetadas += 1;
        }

        totalDeAmostras += (o.Sample > 0) ? o.Sample : 0;
    });

    return {AmostraTotal: parseInt(totalDeAmostras)
        , AmostraTotalColetada: parseInt(totalDeAmostrasColetadas)
        , AmostraTotalColetadasConforme: parseInt(totalDeAmostrasColetadasConforme)
        , ColetasSincronizadas: coletasSincronizadas};

}

function getResultEvaluationDCA(parLevel1_Id, parLevel2_Id) {

	var obj = {
		Evaluation: 1
    };
    
    for(var i = coletasAgrupadas.length-1; i >= 0; i--){
        var coleta = coletasAgrupadas[i];
        
        if (coleta.ParLevel1_Id == parLevel1_Id 
            && coleta.ParLevel2_Id == parLevel2_Id
            && coleta.ParFamiliaProduto_Id == currentFamiliaProdutoDCA_Id) {
                obj = {
                    Evaluation: coleta.Evaluation,
                    ParFamiliaProduto_Id: coleta.ParFamiliaProduto_Id,
                };
                break;
		}
    }

	return obj;
}

$('body').off('click', '[data-proxima-av]').on('click', '[data-proxima-av]', function (e) {
    listarParLevel2DCA(false, true)
 });

 function getParVinculoPesoParLevel2PorIndicador(parLevel1_Id, parLevel2_Id){
     var _parLevel1_Id = parLevel1_Id;
     var _parLevel2_Id = parLevel2_Id;
    var exists = $.grep(parametrization.listaParVinculoPesoParLevel2, function (parVinculoPesoLevel2) {
        if(parVinculoPesoLevel2.ParLevel1_Id == _parLevel1_Id 
            && parVinculoPesoLevel2.ParLevel2_Id == _parLevel2_Id){
            return true;
        }
    });
    
    if(exists.length > 0){
        return exists[0];
    }else{
        return {Peso:1,Equacao:''};
    }
 }

function getCalculoPorMonitoramento(parLevel1_Id, parLevel2_Id, avaliacaoAtual) {
    var pesoCritico = 0;
    var _avaliacaoAtual = avaliacaoAtual;
    var _parLevel1_Id = parLevel1_Id;
    var _parLevel2_Id = parLevel2_Id; currentParLevel2_Id = _parLevel2_Id;
    var listaDeTarefas = GetListaDeTarefasPorMonitoramento();
    var tarefasVinculadas = $.grep(parametrization.listaParVinculoPeso, function (o) {
        return o.ParLevel1_Id == _parLevel1_Id && o.ParLevel2_Id == _parLevel2_Id;
    });
    var totalTarefasComAlgumaNC = 0;
    var totalTarefasAcimaLimiteNC = 0;
    var totalDeAmostras = 0;
    var totalDeAmostrasColetadas = 0;
    var totalDeAmostrasColetadasConforme = 0;
    var coletasSincronizadas = false;
    $(tarefasVinculadas).each(function (i, o) {
        var amostrasColetadas = 0;
        var amostrasColetadasConforme = 0;
        var vinculoPeso = o;
        var tarefa = $.grep(listaDeTarefas, function (level3) {
            if (level3.Id == vinculoPeso.ParLevel3_Id) { return true; }
        });

        var quantidadeDeColetasPorTarefa = $.grep(coletasDCA, function (coletas) {
            if (coletas.ParLevel1_Id == _parLevel1_Id
                && coletas.ParLevel2_Id == _parLevel2_Id
                && coletas.ParLevel3_Id == vinculoPeso.ParLevel3_Id
                && coletas.Evaluation == _avaliacaoAtual
                && coletas.Outros.indexOf('ParFamiliaProduto_Id:' + currentFamiliaProdutoDCA_Id + ',') > 0) {
                amostrasColetadasConforme += coletas.IsConform ? 1 : 0;
                return true;
            }
        });

        //Valida amostras quando são salvar automaticamente
        if(quantidadeDeColetasPorTarefa.length > 0 && (tarefa[0].ParLevel3InputType.Id == 2 || tarefa[0].ParLevel3InputType.Id == 15)){
            amostrasColetadasConforme = quantidadeDeColetasPorTarefa[0].Sample - (quantidadeDeColetasPorTarefa[0].Value > 0 ? quantidadeDeColetasPorTarefa[0].Value : 0);
        }

        totalDeAmostrasColetadasConforme += amostrasColetadasConforme;
        for (i = 0; i < quantidadeDeColetasPorTarefa.length; i++) {
            if (quantidadeDeColetasPorTarefa[0].IsConform == false
                && quantidadeDeColetasPorTarefa[0].WeiDefects == 5) {
                pesoCritico = 1;
            }
        }
        if (o.Sample > 0 && quantidadeDeColetasPorTarefa.length > 0) {
            if (o.Sample > quantidadeDeColetasPorTarefa.length) {
                if(tarefa[0].ParLevel3InputType.Id == 2 || tarefa[0].ParLevel3InputType.Id == 15){
                    amostrasColetadas += quantidadeDeColetasPorTarefa[0].Sample;
                }else{
                    amostrasColetadas += quantidadeDeColetasPorTarefa.length;
                }
            } else {
                amostrasColetadas += o.Sample;
            }
        } else if (quantidadeDeColetasPorTarefa.length > 0) {
            amostrasColetadas += 1;
        }
        totalDeAmostrasColetadas += amostrasColetadas;
        var limiteNCDaTarefa = 1;
        if (tarefa.length > 0 && tarefa[0] && tarefa[0].ParLevel3Value) {
            limiteNCDaTarefa = UmSeForNaNOuNull(tarefa[0].ParLevel3Value.LimiteNC);
        }
        if (tarefa[0].ParLevel3InputType
            && quantidadeDeColetasPorTarefa.length > 0
            && (tarefa[0].ParLevel3InputType.Id == 2 || tarefa[0].ParLevel3InputType.Id == 15)) {
            if ((o.Sample - amostrasColetadasConforme) > limiteNCDaTarefa) {
                totalTarefasAcimaLimiteNC++;
            }
            if ((o.Sample - amostrasColetadasConforme) > 0){
                totalTarefasComAlgumaNC++;
            }
        }else{
            if ((amostrasColetadas - amostrasColetadasConforme) > limiteNCDaTarefa) {
                totalTarefasAcimaLimiteNC++;
            }
            if ((amostrasColetadas - amostrasColetadasConforme) > 0) {
                totalTarefasComAlgumaNC++;
            }
        }
        if (coletasSincronizadas == false && quantidadeDeColetasPorTarefa.length > 0)
            coletasSincronizadas = quantidadeDeColetasPorTarefa[0].Synced == true;
        totalDeAmostras += (o.Sample > 0) ? o.Sample : 0;
    });
    var parVinculoPesoParLevel2 = getParVinculoPesoParLevel2PorIndicador(_parLevel1_Id, _parLevel2_Id);

    /*

    QtdeNC      = Quantidade total de Não Conformidades (Por amostra)

    QtdeC       = Quantidade total de Conformidade (Por amostra)

    QtdeTLNC    = Quantidade de tarefas que passaram do limite de Não Conformidade

    QtdeT       = Quantidade de tarefas

    QtdeTNC     = Quantidade de tarefas com alguma Não Conformidade

    QtdeTC      = Quantidade de tarefas com conformidade

    TCrit	   = Existe tarefa critica? 0 não 1 sim    

*/

    var variaveisEquacao = [{ id: /QtdeNC/g, valor: (parseInt(totalDeAmostrasColetadas) - parseInt(totalDeAmostrasColetadasConforme)) },
        { id: /QtdeTLNC/g, valor: parseInt(totalTarefasAcimaLimiteNC) },
        { id: /QtdeTNC/g, valor: parseInt(totalTarefasComAlgumaNC) },
        { id: /QtdeTC/g, valor: parseInt(listaDeTarefas.length) - parseInt(totalTarefasComAlgumaNC) },
        { id: /QtdeC/g, valor: (parseInt(totalDeAmostrasColetadasConforme)) },
        { id: /QtdeT/g, valor: parseInt(listaDeTarefas.length) },
        { id: /TCrit/g, valor: parseInt(pesoCritico) },];
    var equacao = parVinculoPesoParLevel2.Equacao;
    variaveisEquacao.forEach(function (variavel) {
        equacao = equacao.replace(variavel.id, variavel.valor);
    });
    try {
        var porcentagemEquacao = eval(equacao);
    } catch (e) {
        console.warn(e); porcentagemEquacao = null;
    }
    var porcentagemTotal = parseInt(totalDeAmostrasColetadasConforme) / parseInt(totalDeAmostrasColetadas) * 100;
    return {
        AmostraTotal: parseInt(totalDeAmostras),
        AmostraTotalColetada: parseInt(totalDeAmostrasColetadas),
        AmostraTotalColetadasConforme: parseInt(totalDeAmostrasColetadasConforme),
        TotalTarefasAcimaLimiteNC: parseInt(totalTarefasAcimaLimiteNC),
        ColetasSincronizadas: coletasSincronizadas,
        ParVinculoPesoParLevel2: parVinculoPesoParLevel2,
        Equacao: equacao,
        PorcentagemEquacao: porcentagemEquacao,
        Porcentagem: equacao.length > 0 ? porcentagemEquacao : porcentagemTotal,
        PorcentagemTotalRespondida: porcentagemTotal
    };
}

function GetListaDeTarefasPorMonitoramento(){
    var listaDeTarefa = [];
    var levels = GetLevelsDCA()
    
    levels.forEach(function (level1) {
        level1.ParLevel2.forEach(function (level2) {
            level2.ParLevel3.forEach(function (level3) {
                listaDeTarefa.push(level3);
            });
        });
    });

    return listaDeTarefa;
}