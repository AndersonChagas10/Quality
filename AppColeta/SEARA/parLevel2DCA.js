function listarParLevel2DCA(isVoltar) {

    currentParLevel2_Id = null;

    var listaParLevel2 = retornaParLevel2DCA(currentParLevel1_Id);

    var htmlLista = "";

    var totalDeConformidadeDeColetas = 0;
    var avaliacaoAtual = 0;
    $(listaParLevel2).each(function (i, o) {

        currentEvaluationDCA = getResultEvaluationDCA(currentParLevel1_Id, o.Id);
        
        if(currentEvaluationDCA.Evaluation < avaliacaoAtual)
            avaliacaoAtual = currentEvaluationDCA.Evaluation;

        var consolidadoAmostraTotal = getAmostraTotalEColetadaEConformePorMonitoramento({ Id: currentParLevel1_Id }, o);

        var style = '';

        if (consolidadoAmostraTotal.AmostraTotalColetada >= consolidadoAmostraTotal.AmostraTotal) {
            style = 'style="background-color:#ddd;cursor:not-allowed"';
        }

        porcentagemDeConformidadePorLevel2 = parseInt(consolidadoAmostraTotal.AmostraTotalColetadasConforme/consolidadoAmostraTotal.AmostraTotalColetada*100);
        totalDeConformidadeDeColetas += ZeroSeForNaN(porcentagemDeConformidadePorLevel2);

        htmlLista += '<button type="button" ' + style + ' class="list-group-item col-xs-12" ' +
            '" data-dca-par-level2-id="' + o.Id + '" ' +
            'data-current-evaluation="' + currentEvaluationDCA.Evaluation + '">                       ' +
            '	<div class="col-xs-4">' + o.Name + '</div>                                      ' +
            '	<div class="col-xs-4 text-center">'+ZeroSeForNaN(porcentagemDeConformidadePorLevel2)+'%</div>      ' +
            '	<div class="col-xs-4 text-center">'+ZeroSeForNaN(parseInt(consolidadoAmostraTotal.AmostraTotalColetada/consolidadoAmostraTotal.AmostraTotal*100))+'%</div>              ' +
            '</button>';
    });

    var voltar = '<a onclick="listarParLevel1(' + isVoltar + ');" class="btn btn-warning">Voltar</a>';

    html = getHeader() +
        '<div class="container-fluid">                                           ' +
        '	<div class="">                                              ' +
        '		<div class="col-xs-12">                                    ' +
        '			<div class="panel panel-primary">                      ' +
        '			  <div class="panel-heading">                          ' +
        '				<h3 class="panel-title">' + voltar + ' Selecione o centro de custo desejado</h3>            ' +
        '			  </div>                                               ' +
        '			  <div class="panel-body">                             ' +
        '               <div class="col-sm-12 text-center" style="padding:20px;margin-bottom:5px">Item: Familia de Produto</div>' +
        '               <div class="col-sm-12 btn-warning text-center" style="padding:20px;margin-bottom:5px">Avaliação ' + avaliacaoAtual + '</div>' +
        '               <h2 class="col-xs-6 btn-info text-center" style="height:100px;padding-top: 35px;margin: 0px;">'+ZeroSeForNaN(parseInt(totalDeConformidadeDeColetas/listaParLevel2.length))+'%</h2>' +
        '               <div class="col-xs-6 text-center" style="padding:0px !important">' +
        '<select name="sometext" size="5" class="form-control" style="height:100px;">' +
        '<option>text1</option>' +
        '<option>text2</option>' +
        '<option>text3</option>' +
        '<option>text4</option>' +
        '<option>text5</option>' +
        '</select></div>' +
        '				<div class="list-group" style="padding-top:5px;clear:both !important">                           ' +
        htmlLista +
        '				</div>                                             ' +
        '			  </div>                                               ' +
        '			</div>                                                 ' +
        '		</div>                                                     ' +
        '	</div>                                                         ' +
        '</div>';

    $('div#app').html(html);

    setBreadcrumbs();

    if ($(".list-group button").length == 1 && (isVoltar == false || isVoltar == undefined)) {
        $("[data-par-department-id]").trigger('click');
    }
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

function getAmostraTotalEColetadaEConformePorMonitoramento(parLevel1, parLevel2) {

    var tarefasVinculadas = $.grep(parametrization.listaParVinculoPeso, function (o) {
        return o.ParLevel1_Id == parLevel1.Id && o.ParLevel2_Id == parLevel2.Id;
    });

    var totalDeAmostras = 0;
    var totalDeAmostrasColetadas = 0;
    var totalDeAmostrasColetadasConforme = 0;
    $(tarefasVinculadas).each(function (i, o) {
        var vinculoPeso = o;
        var quantidadeDeColetasPorTarefa = $.grep(coletasDCA, function (coletas) {
            if(coletas.ParLevel1_Id == parLevel1.Id && coletas.ParLevel2_Id == parLevel2.Id && coletas.ParLevel3_Id == vinculoPeso.ParLevel3_Id){
                totalDeAmostrasColetadasConforme += coletas.IsConform ? 1 : 0;
                return true;
            }
        });

        if (o.Sample > 0) {
            if (o.Sample < quantidadeDeColetasPorTarefa.length) {
                totalDeAmostrasColetadas += quantidadeDeColetasPorTarefa.length;
            } else {
                totalDeAmostrasColetadas += o.Sample;
            }
        }else if(quantidadeDeColetasPorTarefa.length > 0){
            totalDeAmostrasColetadas += 1;
        }

        totalDeAmostras += (o.Sample > 0) ? o.Sample : 1;
    });

    return {AmostraTotal: parseInt(totalDeAmostras)
        , AmostraTotalColetada: parseInt(totalDeAmostrasColetadas)
        , AmostraTotalColetadasConforme: parseInt(totalDeAmostrasColetadasConforme)};

}

function getResultEvaluationDCA(parLevel1_Id, parLevel2_Id) {

	var obj = {
		Evaluation: 1,
		Sample: 1
	};

	$(coletasAgrupadas).each(function (i, o) {
		if (o.ParLevel1_Id == parLevel1_Id && o.ParLevel2_Id == parLevel2_Id) {
			obj = o;
		}
	});

	return obj;

}