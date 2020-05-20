function openParClusterGroup() {

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
                        listarParClusterGroup();
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
            listarParClusterGroup();
        }

    });
}

function listarParClusterGroup() {
    
    cleanGlobalVarParCluster();

    _readFile("parClusterGroup.txt", function (data) {

        data = JSON.parse(data);

        listaParClusterGroup = data;

        var clusterGroup = {};

        var htmlParClusterGroup = "";

        $(data).each(function (i, o) {

            htmlParClusterGroup += '<button type="button" class="list-group-item col-xs-12" data-par-cluster-group-id="' + o.Id + '" ' +
                ((currentParClusterGroup_Id == o.Id || !(currentParClusterGroup_Id > 0)) ? '' : 'style="background-color:#eee;cursor:not-allowed"')
                + '>' + o.Name +
                '</button>';
        });

        var voltar = '<a onclick="voltarParcompany(openParCompany,null);" class="btn btn-warning">Voltar</a>';

        html = getHeader() +
            '<div class="container-fluid">                               ' +
            '	<div class="">                                  ' +
            '		<div class="col-xs-12">                        ' +
            '			<div class="panel panel-primary">          ' +
            '			  <div class="panel-heading">              ' +
            '			    <div class="row">                          ' +
            '			      <div class="col-xs-9">                         ' +
            '				<h3 class="panel-title">' + voltar + 'Qual grupo de cluster deseja realizar coleta?</h3>' +
            '                 </div >                                          ' +
            '                 <div class="col-xs-3">                           ' +
            getBotaoBuscar() +
            '                 </div>                                           ' +
            '               </div>                                             ' +
            '			  </div>                                   ' +
            '			  <div class="panel-body">                 ' +
            '				<div class="list-group">               ' +
            htmlParClusterGroup +
            '				</div>                                 ' +
            '			  </div>                                   ' +
            '			</div>                                     ' +
            '		</div>                                         ' +
            '	</div>                                             ' +
            '</div>';

        $('div#app').html(html);
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