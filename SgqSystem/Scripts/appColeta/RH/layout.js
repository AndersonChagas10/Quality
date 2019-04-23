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
      '      <a class="navbar-brand" style="color:white" href="#">AppColeta</a>                                                                                                             '+
      '    </div>                                                                                                                                                        '+
      '    <div id="navbar" class="navbar-collapse collapse">                                                                                                            '+
      '      <ul class="nav navbar-nav">                                                                                                                                 '+
      '        <li><a href="#" onclick="sincronizar()">Sincronizar</a></li>                                                                                             '+      
      '      </ul>                                                                                                                                                       '+
      '      <ul class="nav navbar-nav nav-pull-right">                                                                                                                                 '+
      '        <li class="nav-btn">'+currentLogin.FullName+'</li>'+      
      '        <li class="nav-btn">'+currentLogin.ParCompanyXUserSgq[0].ParCompany.Name+'</li>'+      
      '        <li class="nav-btn">'+currentCollectDate+'</li>'+      
	  '        <li><button href="#" class="btn btn-block btn-danger" onclick="logout()" style="color:#fff;margin:7px 7px 7px 0px;padding:6px 15px">Sair</button></li>    '+
      '      </ul>                                                                                                                                                       '+
      '    </div>                                                                                                                                  '+
      '  </div>                                                                                                                                '+
      '</nav>'+
    '<footer class="footer">' +
    '<div class="container-fluid">'+
    '<div class="col-xs-3 nav-btn" data-falta-sincronizar>' +
    '(' + globalColetasRealizadas.length + ') Não sincronizadas'+
    '</div>' +   
	'<div class="col-xs-3">&nbsp;</div>' +
	'<div class="col-xs-3">&nbsp;</div>' +
	'<div data-online-offline></div>' +
    '</div>' +
'</footer>';
	return html;
	
}

function sincronizar(){
	openMensagem('Iniciada sequencia forçada de sincronização','orange','white');
	enviarColeta();
	closeMensagem(2000);
}
