function openColeta(levels) {

    coletaJson = [];
    var html = '';
    var coleta = '';

    levels.forEach(function (level1) {

        var hasLevel2 = false;

        level1.ParLevel2.forEach(function (level2) {

            var hasLevel3 = false;
            var striped = true;

            level2.ParLevel3.forEach(function (level3) {

                var inputLevel3 = getInputLevel3(level3, level2, level1, striped);

                if (inputLevel3.length > 0) {

                    if (hasLevel3 == false) {

                        if (hasLevel2 == false) {
                            coleta += getLevel1(level1);
                            coleta += getParHeaderFieldLevel1(level1);
                            hasLevel2 = true;
                        }

                        coleta += getLevel2(level2, level1);
                        coleta += getParHeaderFieldLevel2(level1, level2);
                        hasLevel3 = true;
                    }

                    coleta += inputLevel3;

                    if (inputLevel3)
                        if (striped)
                            striped = false;
                        else
                            striped = true;


                }
            });
        });
    });

    html = getHeader() +
        '<div class="container-fluid">                                                                                                                   ' +
        '	<div class="">                                                                                                                         ' +
        '		<div class="col-xs-12">                                                                                                            ' +
        '			<div class="panel panel-primary">                                                                                              ' +
        '			  <div class="panel-heading">                                                                                                  ' +
        '				<h3 class="panel-title"><a onclick="validaRota(listarParCargo,currentParCargo_Id);" class="btn btn-warning">Voltar</a> Questionario de Coleta</h3>                                   ' +
        '			  </div>                                                                                                                       ' +
        '			  <div class="panel-body">                                                                                                     ' +
        getContador() +
        getParHeaderFieldDeparment() +
        '				<form data-form-coleta style="text-align:justify">                                                                                                    ' +
        coleta +
        '					<button class="btn btn-block btn-primary input-lg col-xs-12" data-salvar style="margin-top:10px">Salvar</button>       ' +
        '				</form>                                                                                                                    ' +
        '			  </div>                                                                                                                       ' +
        '       </div>                                                                                                                             ' +
        '    </div>                                                                                                                                ' +
        '	</div>                                                                                                                                 ' +
        '</div>';

    $('div#app').html(html);

    setBreadcrumbs();

}