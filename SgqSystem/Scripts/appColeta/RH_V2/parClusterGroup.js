function openParClusterGroup(isVoltar) {

    var html = '';

    _readFile("parClusterGroup.txt", function (data) {
        if (globalLoginOnline) {

            openMensagem('Carregando lista de grupo de clusters', 'blue', 'white');

            $.ajax({
                data: { parCompany_Id: currentParCompany_Id },
                url: urlPreffix + '/api/parClusterGroup',
                type: 'GET',
                success: function (data) {
                    _writeFile("parClusterGroup.txt", JSON.stringify(data), function () {
                        listaParClusterGroup = data;
                        listarParClusterGroup(isVoltar);
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
            listarParClusterGroup(isVoltar);
        }

    });
}

function listarParClusterGroup(isVoltar) {
    
    cleanGlobalVarParCluster();

    currentParClusterGroup_Id = null;
    currentParCluster_Id = null;
    currentParFrequency_Id = null;


    _readFile("parClusterGroup.txt", function (data) {

        data = JSON.parse(data);

        listaParClusterGroup = data;

        var clusterGroup = {};

        var htmlParClusterGroup = "";

        data = data.sort((a, b) => (a.Name > b.Name) ? 1 : -1);

        if (globalLogo)
            systemLogo = 'background-image: url(' + globalLogo + ')';

        htmlParClusterGroup += '<div class="row">';
        $(data).each(function (i, o) {

            htmlParClusterGroup += '<div class="col-xs-6 col-md-4" style="padding:2px;padding-left: 30px!important;padding-right: 30px!important"><button type="button" class="list-group-item btn btn-lg btn-block"  style="color: #1F497D;background-color:#DCE6F1;white-space: break-spaces;" data-par-cluster-group-id="' + o.Id + '" title="' + o.Name + '">' + o.Name +
                '</button></div>';
        });
        htmlParClusterGroup += '</div>';
        var voltar = '<a onclick="voltarParcompany(openParCompany,null);" style="margin-bottom:10px" class="btn btn-warning col-xs-12">Voltar</a>';

        html = getHeader() +
            '<div class="container-fluid">                               ' +
            '	<div class="">                                  ' +
            '		<div class="col-xs-12">                        ' +
            '			<div class="panel">          ' +
            '			  <div class="panel-heading"  style="background-color:#DCE6F1;" >              ' +
            '<div style="height: 220px; text-align: center; background-repeat: no-repeat;background-size: auto 100%;background-position: center;height: 220px; ' + systemLogo + '">' +
            '</div>' +
            '			    <div class="row">                          ' +
            '			      <div class="col-xs-9">                         ' +
            '				<h3 class="panel-title">Selecione o Grupo de Cluster</h3>' +
            '                 </div >                                          ' +
            '                 <div class="col-sm-3">                           ' +
            getBotaoBuscar() +
            '                 </div>                                           ' +
            '               </div>                                             ' +
            '			  </div>                                   ' +
            '			  <div class="panel-body">                 ' +
            '				<div class="list-group">               ' +
            voltar +
            htmlParClusterGroup +
            '				</div>                                 ' +
            '			  </div>                                   ' +
            '			</div>                                     ' +
            '		</div>                                         ' +
            '	</div>                                             ' +
            '</div>';

        $('div#app').html(html);
        setBreadcrumbs();
        if ($(".list-group button").length == 1 && (isVoltar == false || isVoltar == undefined)) {
            $("[data-par-cluster-group-id]").trigger('click');
        }
    });
}

function cleanGlobalVarParClusterGroup() {
    currentParDepartment_Id = null;
    currentParCargo_Id = null;
    currentsParDepartments_Ids = [];
}

$('body').off('click', '[data-par-cluster-group-id]').on('click', '[data-par-cluster-group-id]', function (e) {

    currentParClusterGroup_Id = parseInt($(this).attr('data-par-cluster-group-id'));

    openParCluster(currentParClusterGroup_Id);

});

function voltarParcompany() {

	listarParCompany(true);
}