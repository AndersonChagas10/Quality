﻿function openParCluster(isVoltar) {

    var html = '';

    _readFile("parCluster.txt", function (data) {
        if (globalLoginOnline) {

            openMensagem('Carregando lista de clusters', 'blue', 'white');

            $.ajax({
                data: { parClusterGroupId: currentParClusterGroup_Id, parCompany_Id: currentParCompany_Id },
                url: urlPreffix + '/api/parCluster',
                type: 'GET',
                success: function (data) {

                    _writeFile("parCluster.txt", JSON.stringify(data), function () {
                        listaParCluster = data;
                        listarParCluster(isVoltar);
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
            listarParCluster(isVoltar);
        }

    });
}

function listarParCluster(isVoltar) {

    cleanGlobalVarParCluster();

    currentParCluster_Id = null;
    currentParFrequency_Id = null;

    _readFile("parCluster.txt", function (data) {

        data = JSON.parse(data);

        listaParCluster = data;

        var cluster = {};

        var htmlParCluster = "";

        data = data.sort((a, b) => (a.Name > b.Name) ? 1 : -1);


        if (globalLogo)
            systemLogo = 'background-image: url(' + globalLogo + ')';

        htmlParCluster += '<div class="row">';
        $(data).each(function (i, o) {

            var flagAcao = "";
            if (getAcoesByCluster(o.Id).length > 0)
                flagAcao = '<span class="badge">Ação Pendente</span>';

            htmlParCluster += '<div class="col-xs-6 col-md-4 divBtnLista"><button type="button" class="list-group-item btn btn-lg btn-block btnLista" data-par-cluster-id="' + o.Id + '" title="' + o.Name + '">' + o.Name +
                '</button>' + flagAcao + '</div>';
        });

        htmlParCluster += "</div>";

        var voltar = '<a onclick="validaRota(openParClusterGroup,true);" style="margin-bottom:10px"  class="btn btn-warning col-xs-12">Voltar</a>';

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
            '				<h3 class="panel-title">Selecione o Cluster</h3>' +
            '                 </div >                                          ' +
            '                 <div class="col-sm-3">                           ' +
            getBotaoBuscar() +
            '                 </div>                                           ' +
            '               </div>                                             ' +
            '			  </div>                                   ' +
            '			  <div class="panel-body">                 ' +
            '				<div class="list-group">               ' +
            voltar +
            htmlParCluster +
            '				</div>                                 ' +
            '			  </div>                                   ' +
            '			</div>                                     ' +
            '		</div>                                         ' +
            '	</div>                                             ' +
            '</div>';

        $('div#app').html(html);
        setBreadcrumbs();

        if ($(".list-group button").length == 1 && (isVoltar == false || isVoltar == undefined)) {
            $("[data-par-cluster-id]").trigger('click');
        }
    });
}

function cleanGlobalVarParCluster() {
    currentParDepartment_Id = null;
    currentParCargo_Id = null;
    currentsParDepartments_Ids = [];
}

$('body').off('click', '[data-par-cluster-id]').on('click', '[data-par-cluster-id]', function (e) {

    currentParCluster_Id = parseInt($(this).attr('data-par-cluster-id'));

    openParFrequencyXindicador();

});