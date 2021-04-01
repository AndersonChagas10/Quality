//Data string deve ser yyyy-MM-dd HH:MM:SS sendo a hora opicional
function stringToDate(dateString) {

    var dia = parseInt(dateString.substr(8, 2));
    var mes = parseInt(dateString.substr(5, 2)) - 1;
    var ano = parseInt(dateString.substr(0, 4));

    var hora = parseInt(dateString.substr(11, 2)) ? parseInt(dateString.substr(11, 2)) : 0;
    var segundos = parseInt(dateString.substr(14, 2)) ? parseInt(dateString.substr(14, 2)) : 0;
    var mimutos = parseInt(dateString.substr(18, 2)) ? parseInt(dateString.substr(18, 2)) : 0;

    //new Date(year, month, day, hours, minutes, seconds, milliseconds)
    return new Date(ano, mes, dia, hora, mimutos, segundos);
}

function convertDateToJson(date) {

    var newDate = new Date(date);

    return new Date(newDate.setHours(newDate.getHours() - 3)).toJSON();
}

function getCurrentDate() {

    //hora atual
    var hour = new Date().toLocaleTimeString();

    //data collectionDate
    var data = new Date(currentCollectDate).toJSON().substr(0, 10);

    return convertDateToJson(data + " " + hour);
}

function retornaOptionsPeloArray(lista, value, text, defaultText){
    var html = "";
    if(typeof(defaultText) != 'undefined'){
        html += '<option value="">'+defaultText+'</option>';
    }
    $(lista).each(function (i, o) {
        html += '<option value="'+o[value]+'">'+o[text]+'</option>';
    });
    return html;
}

function retornaOptions(lista, value, text, defaultText) {
    var html = "";
    if (typeof (defaultText) != 'undefined') {
        html += '<option value="">' + defaultText + '</option>';
    }
    $(lista).each(function (i, o) {
        if (lista.length == 1) {
            html += '<option value="' + o[value] + '" selected>' + o[text] + '</option>';

            setTimeout(function () {
                var selectCentroCusto = $('body [data-selects-cc] select')[0];
                var selectSecao = $('body [data-selects-cc] select')[1];
                var selectCargo = $('body [data-selects-cargo] select')[0];

                if (!!selectCargo && parseInt($(selectCargo).val()) > 0) {
                    $(selectCargo).trigger('change');
                    $(selectCargo).parent().css('display', 'none');
                } else if (!!selectSecao && parseInt($(selectSecao).val()) > 0) {
                    $(selectSecao).trigger('change');
                    $(selectSecao).parent().css('display', 'none');
                } else if (!!selectCentroCusto && parseInt($(selectCentroCusto).val()) > 0) {
                    $(selectCentroCusto).trigger('change');
                }
                //.css('display', 'none');
            }, 1);
        } else {
            html += '<option value="' + o[value] + '">' + o[text] + '</option>';
        }
    });
    return html;
}

function criaHtmlSelect(titulo,options){
	return '<div class="form-group">'+
	'	<label>'+titulo+'</label>'+
	'	<select class="form-control">'+options+'</select>'+
	'</div>';
}

function dateDiff(date1, date2) {
    var dataInicial = new Date(date1);
    var dataFinal = new Date(date2);
    return (dataFinal - dataInicial) / 1000 / 60 / 60 / 24;
}

function getBotaoBuscar() {
    var botaoBuscar = '<div class="pull-right">                     ' +
        '                  <label style="padding-right:5px">Buscar</label> ' +
        '                  <input type="text" onkeyup="buscarItemNaLista(this)"/>' +
        '              </div>';
    return botaoBuscar;
}

function getBotaoBuscarSecaoXCargo() {
    var botaoBuscar = '<div class="pull-right">                     ' +
        '                  <label style="padding-right:5px">Buscar</label> ' +
        '                  <input type="text" onkeyup="buscarItemNaListaSecaoXCargo(this)"/>' +
        '              </div>';
    return botaoBuscar;
}

function buscarItemNaListaSecaoXCargo(input) {
    $('body').off('keyup', input).on('keyup', input, function () {
        $('button.list-group-item').each(function (i, o) {
            var mostrarItem = $(o).text().toLowerCase().includes($(input).val().toLowerCase());
            if (mostrarItem) {
                $(o).show();
            } else {
                $(o).hide(); 
            }
        });

        var secaoCascateList = searchSecaoCascade(input);
        
        if ($('button.list-group-item:visible').length == 0 && secaoCascateList.length <= 0) {
            if ($('span.list-group-item').length == 0) {
                $('.list-group').append("<span class='list-group-item col-xs-12 text-center'>Nenhum resultado encontrado com o termo digitado.</span>");
            } else {
                $('span.list-group-item').show();
            }
        } else {
            if ($('span.list-group-item').length > 0 && secaoCascateList.length <= 0) {
                $('span.list-group-item').hide();
            } else {
                $(secaoCascateList).map(function (i, o) {
                    if (o.Parent_Id)
                        $(`[data-par-department-id=${o.Parent_Id}]`).show();
                    else
                        $(`[data-par-department-id=${o.Id}]`).show();
                });
            }
        }
    });
}

function buscarItemNaLista(input) {
    $('body').off('keyup', input).on('keyup', input, function () {
        $('button.list-group-item').each(function (i, o) {
            var mostrarItem = $(o).text().toLowerCase().includes($(input).val().toLowerCase());
            if (mostrarItem) {
                $(o).show();
            } else {
                $(o).hide();
            }
        });

        if ($('button.list-group-item:visible').length == 0) {
            if ($('span.list-group-item').length == 0) {
                $('.list-group').append("<span class='list-group-item col-xs-12 text-center'>Nenhum resultado encontrado com o termo digitado.</span>");
            } else {
                $('span.list-group-item').show();
            }
        } else {
            if ($('span.list-group-item').length > 0) {
                $('span.list-group-item').hide();
            }
        }
    });
}

function searchSecaoCascade(input) {

    var list = [];
    $('[data-par-department-id]').each(function (i, o) {

        var parDepartmentId = $(o).attr('data-par-department-id');
        var listaSecao = [];
        var listaDepartamentos = retornaDepartamentos(parDepartmentId, true, parametrization.listaParDepartment);
        currentsParDepartments_Ids = [];

        $(listaDepartamentos).each(function (i, o) {
            if ((parDepartmentId > 0 && parDepartmentId == o.Parent_Id) || ((parDepartmentId == 0 || parDepartmentId == null) && (o.Parent_Id == 0 || o.Parent_Id == null))) {
                listaSecao.push(o);
            }
        });

        var possuiItem;
        listaSecao.map(function (o,i) {
            possuiItem = o.Name.toLowerCase().includes($(input).val().toLowerCase());

            if (possuiItem) {
                list.push(o);
            }
        });

        //se a lista de se��es estiver vazia, busco na lista de cargos
        if (list.length <= 0) {

            var listaParCargo = retornaCargos(parDepartmentId);

            listaParCargo = retornaCargosPlanejados(listaParCargo);

            listaParCargo.map(function (o, i) {
                possuiItem = o.Name.toLowerCase().includes($(input).val().toLowerCase());

                if (possuiItem) {
                    list.push(o);
                }
            });

            if (possuiItem) {
                list.push(o);
            }
        }
    });
    return list;
}
