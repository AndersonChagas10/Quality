function getHeader(){
    var html = '';

html = '<nav class="navbar navbar navbar-inverse">                                                                                                                      '+
      '  <div class="container-fluid">                                                                                                                                   '+
      '    <div class="navbar-header">                                                                                                                                   '+
      '      <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">            '+
      '        <span class="sr-only">Toggle navigation</span>                                                                                                            '+
      '        <span class="icon-bar"></span>                                                                                                                            '+
      '        <span class="icon-bar"></span>                                                                                                                            '+
      '        <span class="icon-bar"></span>                                                                                                                            '+
      '      </button>                                                                                                                                                   '+
      '      <a class="navbar-brand" href="#">APP Coleta</a>                                                                                                             '+
      '    </div>                                                                                                                                                        '+
      '    <div id="navbar" class="navbar-collapse collapse">                                                                                                            '+
      '      <ul class="nav navbar-nav">                                                                                                                                 '+
      '        <li><a href="#">Home</a></li>                                                                                                                             '+
      '        <li><a href="#" onclick="enviarColeta()">Sincronizar</a></li>                                                                                             '+
      '        <!--<li class="dropdown">                                                                                                                                 '+
      '          <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Dropdown                            '+
      '          <span class="caret"></span></a>                                                                                                                         '+
      '          <ul class="dropdown-menu">                                                                                                                              '+
      '            <li><a href="#">Another action</a></li>                                                                                                               '+
      '            <li><a href="#">Something else here</a></li>                                                                                                          '+
      '            <li role="separator" class="divider"></li>                                                                                                            '+
      '            <li class="dropdown-header">Nav header</li>                                                                                                           '+
      '            <li><a href="#">Separated link</a></li>                                                                                                               '+
      '            <li><a href="#">One more separated link</a></li>                                                                                                      '+
      '          </ul>                                                                                                                                                   '+
      '        </li>-->                                                                                                                                                  '+
      '      </ul>                                                                                                                                                       '+
      '      <ul class="nav navbar-nav navbar-right">                                                                                                                    '+
      '        <!--<li><a href="./">Default <span class="sr-only">(current)</span></a></li>                                                                              '+
      '        <li><a href="../navbar-static-top/">Static top</a></li>-->                                                                                                '+
      '        <li><button href="#" class="btn btn-block btn-danger" onclick="logout()" style="color:#fff;margin:7px 7px 7px 0px;padding:6px 15px">Sair</button></li>    '+
      '      </ul>                                                                                                                                                       '+
      '    </div><!--/.nav-collapse -->                                                                                                                                  '+
      '  </div><!--/.container-fluid -->                                                                                                                                 '+
      '</nav>'+
    '<footer class="footer">' +
    '<div class="container">'+
    '<div class="col-xs-6" data-falta-sincronizar>' +
    '(' + globalColetasRealizadas.length + ') Não sincronizadas'+
    '</div>' +
    '</div>' +
'</footer>';
	return html;
	
}
