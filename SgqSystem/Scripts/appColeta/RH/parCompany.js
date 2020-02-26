function openParCompany() {

    var html = '';

    _readFile("parCompany.txt", function (data) {
        if (globalLoginOnline) {

            openMensagem('Carregando lista de unidades', 'blue', 'white');

            $.ajax({
                data: { userSgq_Id: currentUserSgq_Id },
                url: urlPreffix + '/api/parCompany',
                type: 'GET',
                success: function (data) {
                    _writeFile("parCompany.txt", JSON.stringify(data), function () {
                        listaParCompany = data;
                        listarParCompany();
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
            listarParCompany();
        }

    });
}

function listarParCompany() {
    
    cleanGlobalVarParCluster();

    _readFile("parCompany.txt", function (data) {

        data = JSON.parse(data);

        listaParCompany = data;

        if(listaParCompany.length == 1){
            openParClusterGroup(listaParCompany[0].ParCompany.Id);
        }

        var clusterGroup = {};

        var htmlParCompany = "";

        $(data).each(function (i, o) {

            htmlParCompany += '<button type="button" class="list-group-item col-xs-12" data-par-company-id="' + o.ParCompany.Id + '" ' //+
                //((currentParClusterGroup_Id == o.Id || !(currentParClusterGroup_Id > 0)) ? '' : 'style="background-color:#eee;cursor:not-allowed"')
                + '>' + o.ParCompany.Name +
                '</button>';
        });

        var voltar = '<a onclick="validaRota(openMenu,null);" class="btn btn-warning">Voltar</a>';

        html = getHeader() +
            '<div class="container-fluid">                               ' +
            '	<div class="">                                  ' +
            '		<div class="col-xs-12">                        ' +
            '			<div class="panel panel-primary">          ' +
            '			  <div class="panel-heading">              ' +
            '				<h3 class="panel-title">' + voltar + 'Qual unidade deseja realizar coleta?</h3>' +
            '			  </div>                                   ' +
            '			  <div class="panel-body">                 ' +
            '				<div class="list-group">               ' +
            htmlParCompany +
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

 $('body').off('click', '[data-par-company-id]').on('click', '[data-par-company-id]', function (e) {

     currentParCompany_Id = parseInt($(this).attr('data-par-company-id'));

     parCompanys = currentParCompany_Id;

     openParClusterGroup(currentParCompany_Id);

 });