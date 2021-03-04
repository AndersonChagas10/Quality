function openParFrequencyXindicador() {

    var html = '';

    _readFile("parFrequency.txt", function (data) {
        if (globalLoginOnline) {

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
            listarParFrequency();
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

        var frequency = {};

        var htmlParFrequency = "";
        var htmlParLevel1 = "";


        data = data.sort((a, b) => (a.Name > b.Name) ? 1 : -1);

        if(globalLogo)
            systemLogo = 'background-image: url(' + globalLogo + ')';

        $(data).each(function (i, o) {

            htmlParFrequency += '<div class="row"> <div class="col-xs-12"> <h3 class="alert alert-secondary text-center" style="background-color:#CCC">' + o.Name +
                '</h3> </div> </div>';

            htmlParFrequency += '<div class="row">';
            data[i].ParLevel1 = data[i].ParLevel1.sort((a, b) => (a.Name > b.Name) ? 1 : -1);
            $(data[i].ParLevel1).each(function (x, y) {

                htmlParFrequency += '<div class="col-xs-6 col-md-4" style="padding:2px;padding-left: 30px!important;padding-right: 30px!important;"><button type="button" class="list-group-item btn btn-lg btn-block" style="color: #1F497D;background-color:#DCE6F1;" data-par-frequency-id="' + o.Id + '" data-par-level1-id="' + y.Id + '" >' + y.Name +
                    '</button></div>';
            });
            htmlParFrequency += '</div>';
        });
        var voltar = '<a onclick="validaRota(openParCluster,null);"  style="margin-bottom:10px"  class="btn btn-warning col-xs-12">Voltar</a>';

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
            '			  </div>                                   ' +
            '			</div>                                     ' +
            '		</div>                                         ' +
            '	</div>                                             ' +
            '</div>';

        $('div#app').html(html);
        setBreadcrumbs();
    });
}

function cleanGlobalVarParFrequency() {
    currentParDepartment_Id = null;
    currentParCargo_Id = null;
    //currentParFrequency_Id = null;
    currentsParDepartments_Ids = [];
}

$('body').off('click', '[data-par-frequency-id]').on('click', '[data-par-frequency-id]', function (e) {

    var frequencyId = parseInt($(this).attr('data-par-frequency-id'));

    getAppParametrization(frequencyId);

});

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
                openPlanejamentoColeta();
                atualizaColetasParciais();
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