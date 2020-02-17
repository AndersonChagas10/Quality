currentEvaluationDCA = {};
function listarParLevel2DCA(isVoltar, pularParaProximaAvaliacao) {

    currentParLevel2_Id = null;

    var listaParLevel2 = retornaParLevel2DCA(currentParLevel1_Id);

    var htmlLista = "";
    var btnProximaAvaliacao = '<button class="btn btn-block btn-primary input-lg col-xs-12" data-proxima-av style="margin-top:10px">Próxima Avaliação</button>';

    var totalDeConformidadeDeColetas = 0;
    var avaliacaoAtual = 0;
    $(listaParLevel2).each(function (i, o) {

        currentEvaluationDCA = getResultEvaluationDCA(currentParLevel1_Id, o.Id);
        
        if(avaliacaoAtual == 0 || currentEvaluationDCA.Evaluation < avaliacaoAtual){
            avaliacaoAtual = currentEvaluationDCA.Evaluation;
        }
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

        var consolidadoAmostraTotal = getAmostraTotalEColetadaEConformePorMonitoramento({ Id: currentParLevel1_Id }, o, avaliacaoAtual);

        var style = '';

        if (consolidadoAmostraTotal.ColetasSincronizadas) {
            style = 'style="background-color:#ddd;cursor:not-allowed"';
        }else{
            btnProximaAvaliacao = '';
        }

        porcentagemDeConformidadePorLevel2 = parseInt(consolidadoAmostraTotal.AmostraTotalColetadasConforme/consolidadoAmostraTotal.AmostraTotalColetada*100);
        totalDeConformidadeDeColetas += ZeroSeForNaN(porcentagemDeConformidadePorLevel2);

        htmlLista += '<button type="button" ' + style + ' class="list-group-item col-xs-12" ' +
            '" data-dca-par-level2-id="' + o.Id + '" ' +
            'data-current-evaluation="' + avaliacaoAtual + '">                       ' +
            '	<div class="col-xs-4">' + o.Name + '</div>                                      ' +
            '	<div class="col-xs-4 text-center">Conforme: '+ZeroSeForNaN(porcentagemDeConformidadePorLevel2)+'%</div>      ' +
            '	<div class="col-xs-4 text-center">Respondido: '+ZeroSeForNaN(parseInt(consolidadoAmostraTotal.AmostraTotalColetada/consolidadoAmostraTotal.AmostraTotal*100))+'%</div>              ' +
            '</button>';
    });

    var voltar = '<a onclick="listarFamiliaProdutoDCA(' + isVoltar + ');" class="btn btn-warning">Voltar</a>';

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
        '               <h2 class="col-xs-6 btn-info text-center" style="height:100px;padding-top: 35px;margin: 0px;">'+ZeroSeForNaN(parseInt(totalDeConformidadeDeColetas/listaParLevel2.length))+'%</h2>' +
        '               <div class="col-xs-6 text-center" style="padding:0px !important">' +
        getSelectProdutosDCA()+
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
}

$('body').off('click', '[data-dca-par-level2-id]').on('click', '[data-dca-par-level2-id]', function (e) {

    currentParLevel2_Id = parseInt($(this).attr('data-dca-par-level2-id'));

    currentTotalEvaluationValue = $(this).attr('data-total-evaluation');
    currentTotalSampleValue = $(this).attr('data-total-sample');
    //var currentEvaluationValue = $(this).attr('data-current-evaluation');

    listarParLevelsDCA();

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