function listarParCluster() {

    var htmlParCluster = "";

    parametrization.listaParCluster.sort((a, b) => a.Name.localeCompare(b.Name));
    $(parametrization.listaParCluster).each(function (i, o) {

        htmlParCluster += '<button type="button" class="list-group-item col-xs-12" data-par-cluster-id="' + o.Id + '" '
            + '>' + o.Name +
            '</button>';
    });

    var voltar = '';//'<a onclick="validaRota(openParClusterGroup,null);" class="btn btn-warning">Voltar</a>';

    html = getHeader() +
        '<div class="container-fluid">                               ' +
        '	<div class="">                                  ' +
        '		<div class="col-xs-12">                        ' +
        '			<div class="panel panel-primary">          ' +
        '			  <div class="panel-heading">              ' +
        '			    <div class="row">                          ' +
        '			      <div class="col-xs-9">                         ' +
        '				<h3 class="panel-title">' + voltar + 'Qual cluster deseja realizar coleta?</h3>' +
        '                 </div >                                          ' +
        '                 <div class="col-md-3">                           ' +
        getBotaoBuscar() +
        '                 </div>                                           ' +
        '               </div>                                             ' +
        '			  </div>                                   ' +
        '			  <div class="panel-body">                 ' +
        '				<div class="list-group">               ' +
        htmlParCluster +
        '				</div>                                 ' +
        '			  </div>                                   ' +
        '			</div>                                     ' +
        '		</div>                                         ' +
        '	</div>                                             ' +
        '</div>';

    $('div#app').html(html);
}

$('body').off('click', '[data-par-cluster-id]').on('click', '[data-par-cluster-id]', function (e) {

    currentParCluster_Id = parseInt($(this).attr('data-par-cluster-id'));

    listarParLevel1();

});