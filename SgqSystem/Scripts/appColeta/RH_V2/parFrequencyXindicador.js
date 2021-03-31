function openParFrequencyXindicador() {

    var html = '';

    _readFile("parFrequency.txt", function (data) {
        if (appIsOnline) {
            
            //globalLoginOnline a variavel nao muda para false mesmo estando off

            openMensagem('Carregando lista de frequencia e indicador', 'blue', 'white');

            $.ajax({
                data: JSON.stringify({
                    ParCompany_Id: currentParCompany_Id
                    , ParCluster_Id: currentParCluster_Id
                    , AppDate: currentCollectDate
                }),
                contentType: "application/json",
                type: 'POST',
                url: urlPreffix + '/api/GetParFrequencyXParLevel1',
                success: function (data) {

                    _writeFile("parFrequency.txt", JSON.stringify(data), function () {
                        listaParFrequency = data;
                        listarParFrequencyXindicador();
                    });

                    closeMensagem();
                },
                timeout: 600000,
                error: function () {
                    $(this).html($(this).attr('data-initial-text'));
                    closeMensagem();
                }
            });

        } else {
            listarParFrequencyXindicador();
        }

    });
}

function listarParFrequencyXindicador() {

    cleanGlobalVarParFrequency();

    currentParFrequency_Id = null;
    currentParLevel1_Id = null;

    _readFile("parFrequency.txt", function (data) {

        data = JSON.parse(data);

        listaParFrequency = data;

        var htmlParFrequency = "";

        data = data.sort((a, b) => (a.Name > b.Name) ? 1 : -1);

        if (globalLogo)
            systemLogo = 'background-image: url(' + globalLogo + ')';

        $(data).each(function (i, o) {

            htmlParFrequency += `<div class="row"> 
                 <div class="col-xs-12"> 
                    <h3 class="alert alert-secondary text-center" style="background-color:#CCC">${o.Name}
                        <button class="btn btn-sm btn-info pull-right" data-select-allLevel1="${o.Id}"> Selecionar todos</button>
                    </h3>
                    </div>
                </div>`;

            htmlParFrequency += '<div class="row">';

            data[i].ParLevel1 = data[i].ParLevel1.sort((a, b) => (a.Name > b.Name) ? 1 : -1);

            $(data[i].ParLevel1).each(function (x, y) {

                htmlParFrequency += '<div class="col-xs-6 col-md-4" style="padding:2px;padding-left: 30px!important;padding-right: 30px!important;"><button type="button" class="list-group-item btn btn-lg btn-block" style="color: #1F497D;background-color:#DCE6F1;" data-selected="false" data-par-frequency-id="' + o.Id + '" data-par-level1-id="' + y.Id + '" >' + y.Name +
                    '</button></div>';
            });
            htmlParFrequency += '</div>';
        });

        var voltar = '<a onclick="validaRota(openParCluster,true);"  style="margin-bottom:10px"  class="btn btn-warning col-xs-12">Voltar</a>';

        html = getHeader() +
            '<div class="container-fluid">                               ' +
            '	<div class="">                                  ' +
            '		<div class="col-xs-12">                        ' +
            '			<div class="panel">          ' +
            '			  <div class="panel-heading" style="background-color:#DCE6F1;">              ' +
            '<div style="height: 220px; text-align: center; background-repeat: no-repeat;background-size: auto 100%;background-position: center;height: 220px; ' + systemLogo + '">' +
            '</div>' +
            '			    <div class="row">                          ' +
            '			      <div class="col-xs-9">                         ' +
            '				<h3 class="panel-title">Selecione o Indicador</h3>      ' +
            '                 </div >                                          ' +
            '                 <div class="col-sm-3">                           ' +

            getBotaoBuscar() +
            '                 </div>                                           ' +
            '               </div>                                             ' +
            '			  </div>                                   ' +
            '			  <div class="panel-body">                 ' +
            '				<div class="list-group">               ' +
            voltar +
            htmlParFrequency +
            '				</div>                                 ' +
            '			  <div class="row">    <div class="col-xs-12">             ' +
            '<button class="btn btn-success col-xs-12" data-coletar>Coletar</button>' +
            '			   </div></div>' +
            '			  </div>                                   ' +
            '			</div>                                     ' +
            '		</div>                                         ' +
            '	</div>                                             ' +
            '</div>';

        $('div#app').html(html);
        setBreadcrumbs();

        if (currentPlanejamento) {
            var level1PlanejadoList_Ids = [...new Set(currentPlanejamento.map(x => x.indicador_Id))];

            level1PlanejadoList_Ids.map(function (o, i) {
                $(`[data-par-level1-id=${o}]`).trigger('click');

            });
        }
    });
}

function cleanGlobalVarParFrequency() {
    currentParDepartment_Id = null;
    currentParCargo_Id = null;
    //currentParFrequency_Id = null;
    currentsParDepartments_Ids = [];
}

$('body').off('click', '[data-par-level1-id]').on('click', '[data-par-level1-id]', function (e) {

    setListLevel1($(this));

});

$('body').off('click', '[data-coletar]').on('click', '[data-coletar]', function (e) {

    setParametrizationObj();

});

function setParametrizationObj() {

    parLevel1List = [];

    if ($('[data-selected=true]').length > 0) {

        $('[data-selected=true]').map(function (i, o) {
            parLevel1List.push({ level1_Id: parseInt($(o).attr('data-par-level1-id')), level1_Name: $(o).text() });
        });

        var frequency = parseInt($('[data-selected=true]').attr('data-par-frequency-id'));
        setCurrentPlanejamentoList(parLevel1List);

        getAppParametrization(frequency);


    } else {
        openMensagem("Selecione ao menos um Indicador!", 'yellow', 'black');
        closeMensagem(2000);
    }

}

function setCurrentPlanejamentoList(level1List) {

    if (currentPlanejamento.length > 0)
        currentPlanejamento = [];

    level1List.map(function (o, i) {
        currentPlanejamento.push({ indicador_Id: o.level1_Id, indicador_Name: o.level1_Name });
    });

}

$('body').off('click', '[data-select-allLevel1]').on('click', '[data-select-allLevel1]', function () {

    var frequency = $(this).attr('data-select-allLevel1');
    var level1List = $('body [data-par-frequency-id="' + frequency + '"]');

    var selectAll = 0;

    level1List.filter(function (i, o) {
        if ($(o).attr('data-selected') == 'true')
            selectAll++;
    });

    level1List.filter(function (i, o) {

        if (selectAll > 0) {
            $(o).addClass('disabled');
            $(o).css({ "background-color": "#a0d3a0" });
            $(o).attr('data-selected', 'true');
        } else {
            $(o).removeClass('disabled');
            $(o).css({ "background-color": "#DCE6F1" });
            $(o).attr('data-selected', 'false');
        }

    });

    level1List.each(function (i, o) {
        setListLevel1($(o));
    });

});

function setListLevel1(btn) {

    disableLevel1Button(btn);

    if ($(btn).attr('data-selected') == 'false') {

        $(btn).css({ "background-color": "#a0d3a0" });
        $(btn).attr('data-selected', 'true');
    } else {

        $(btn).css({ "background-color": "#DCE6F1" });
        $(btn).attr('data-selected', 'false');
    }

    if ($('body [data-selected="true"]').length == 0) {

        $('body [data-selected]').removeClass('disabled');
        $('body [data-select-allLevel1]').removeClass('disabled');
    }
}

function disableLevel1Button(btn) {

    $(".list-group-item").map(function (i, o) {
        if ($(o).attr('data-par-frequency-id') != $(btn).attr('data-par-frequency-id')) {
            $(o).addClass('disabled');
        } else {
            $(o).removeClass('disabled');
        }
    });

    $("[data-select-allLevel1]").map(function (i, o) {
        if ($(o).attr('data-select-allLevel1') != $(btn).attr('data-par-frequency-id')) {
            $(o).addClass('disabled');
        } else {
            $(o).removeClass('disabled');
        }
    });
}

function getAppParametrization(frequencyId) {

    if (!frequencyId) {
        return;
    }

    if (frequencyId != currentParFrequency_Id || parametrization.currentParCluster_Id != currentParCluster_Id) {

        currentParFrequency_Id = frequencyId;
        chamaGetAppParametrization();

    } else {

        openMensagem('Carregando parametrização', 'blue', 'white');

        _readFile("appParametrization.txt", function (data) {

            if (data) {
                parametrization = JSON.parse(data);
                atualizarVariaveisCurrent(parametrization);
            }

            openPlanejamentoColeta();
            closeMensagem();
        });
    }
}

function chamaGetAppParametrization() {

    getDicionarioEstatico();
    openMensagem('Por favor, aguarde até que seja feito o download do planejamento', 'blue', 'white');

    $.ajax({
        data: JSON.stringify({
            ParCompany_Id: currentParCompany_Id
            , ParFrequency_Id: currentParFrequency_Id
            , ParCluster_Id: currentParCluster_Id
            , AppDate: currentCollectDate
            , ParClusterGroup_Id: currentParClusterGroup_Id
        }),
        type: 'POST',
        url: urlPreffix + '/api/AppColeta/GetAppParametrization',
        contentType: "application/json",
        success: function (data) {
            data.currentParFrequency_Id = currentParFrequency_Id;
            data.listaParFrequency = listaParFrequency;
            data.currentParCluster_Id = currentParCluster_Id;
            data.currentParClusterGroup_Id = currentParClusterGroup_Id;
            data.currentParCompany_Id = currentParCompany_Id;
            _writeFile("appParametrization.txt", JSON.stringify(data), function () {
                parametrization = data;
                //openPlanejamentoColeta();
                atualizaColetasParciais();
                clickColetar();
                //closeMensagem();
            });

            sincronizarResultado();
        },
        timeout: 600000,
        error: function () {
            $(this).html($(this).attr('data-initial-text'));
            closeMensagem();
        }

    });
}

