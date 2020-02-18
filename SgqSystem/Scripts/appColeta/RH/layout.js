function getHeader() {
    
    var html = '';

    var parCompanys = $.grep(currentLogin.ParCompanyXUserSgq, function (o, i) { return o.ParCompany.Id == currentLogin.ParCompany_Id });
    var parCompanyName = (parCompanys.length > 0
        && typeof (parCompanys[0].ParCompany) != 'undefined') ? parCompanys[0].ParCompany.Name : "";


    html = '<nav class="navbar navbar navbar-inverse">                                                                                                                      ' +
        '  <div class="container-fluid">                                                                                                                                   ' +
        '    <div class="navbar-header">                                                                                                                                   ' +
        '      <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">            ' +
        '        <span class="sr-only">Toggle navigation</span>                                                                                                            ' +
        '        <span class="icon-bar"></span>                                                                                                                            ' +
        '        <span class="icon-bar"></span>                                                                                                                            ' +
        '        <span class="icon-bar"></span>                                                                                                                            ' +
        '      </button>                                                                                                                                                   ' +
        '      <a class="navbar-brand" style="color:white" href="#" onclick="validaRota(openMenu,null)">AppColeta</a>                                                                                                             ' +
        '    </div>                                                                                                                                                        ' +
        '    <div id="navbar" class="navbar-collapse collapse">                                                                                                            ' +
        '      <ul class="nav navbar-nav">                                                                                                                                 ' +
        '        <li><a href="#" onclick="sincronizarColeta()">Enviar dados coletados</a></li>                                                                                             ' +
        '        <li><a href="#" onclick="sincronizarResultado()">Sincronizar coletas realizadas</a></li>                                                                                             ' +
        '        <li><a href="#" onclick="getAppParametrization()">Atualizar dados/parametrização</a></li>                                                                                             ' +
        '      </ul>                                                                                                                                                       ' +
        '      <ul class="nav navbar-nav nav-pull-right">                                                                                                                                 ' +
        '        <li class="nav-btn">' + currentLogin.FullName + '</li>' +
        '        <li class="nav-btn">' + parCompanyName + '</li>' +
        '        <li class="nav-btn" onclick="openModalChangeDate()">' + stringToDate(currentCollectDate.toJSON()).toLocaleDateString() + '</li>' +
        '        <li><button href="#" class="btn btn-block btn-danger" onclick="validaRota(logout,null)" style="color:#fff;margin:7px 7px 7px 0px;padding:6px 15px">Sair</button></li>    ' +
        '      </ul>                                                                                                                                                       ' +
        '    </div>                                                                                                                                  ' +
        '  </div>                                                                                                                                ' +
        '</nav>' +
        '<footer class="footer">' +
        '<div class="container-fluid">' +
        '<div class="col-xs-4 nav-btn" data-falta-sincronizar>' +
        '(' + globalColetasRealizadas.length + ') Não sincronizadas' +
        '</div>' +
        '<div class="col-xs-4 nav-btn" data-falta-sincronizar-ca>' +
        '(' + globalAcoesCorretivasRealizadas.length + ') Não sincronizadas' +
        '</div>' +
        // '<div class="col-xs-3">&nbsp;</div>' +
        // '<div class="col-xs-3">&nbsp;</div>' +
        '<div data-online-offline></div>' +
        '</div>' +
        '</footer>';

    return html;

}
