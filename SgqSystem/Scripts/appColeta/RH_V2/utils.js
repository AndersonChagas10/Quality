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
    var secaoCargoCascadeList = mostrarDepartamentoFiltrado(parametrization.listaParDepartment, $(input).val(), currentParDepartment_Id);
    $('.nenhum-resultado').remove();

    if (secaoCargoCascadeList.length > 0) {
        $('[data-par-department-id]').each(function (i, o) {

            var id = $(o).attr('data-par-department-id');
            var lista = $.grep(secaoCargoCascadeList, function (x, i) {
                if (x["ParDepartmentParent_Id"] == id || x["ParDepartment_Id"] == id)
                    return x;
            });

            if (lista.length > 0) {
                $(o).show();
            } else {
                $(o).hide();
            }
        });
        
    } else {
        $('[data-par-department-id]').hide();
        if($('.nenhum-resultado').length == 0){
            $('.list-group').append("<span class='list-group-item col-xs-12 text-center nenhum-resultado'>Nenhum resultado encontrado com o termo digitado.</span>");
        }
    }  
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


function mostrarDepartamentoFiltrado(listaDeDepartamento, busca, parDepartmentParent_Id){
	var desdobramento = retornarArvoreDesdobramentoDepartamentoCargo(listaDeDepartamento);

	var listaDesdobramentosFiltrados = [];
	$(desdobramento).each(function (i, o) {
		if(!parDepartmentParent_Id
		|| (parDepartmentParent_Id && o["ParDepartmentParent_Id"] == parDepartmentParent_Id)){
			var nomesConcatenados = o["ParDepartment_Name"] + o["ParCargo_Name"];
			if(!parDepartmentParent_Id){
				nomesConcatenados = o["ParDepartmentParent_Name"] + nomesConcatenados
			}
			
			if(nomesConcatenados.toUpperCase().indexOf(busca.toUpperCase()) > -1)
				listaDesdobramentosFiltrados.push(o);
        }
		
			
    });
	
	currentParDepartment_Id = parDepartmentParent_Id;
	return listaDesdobramentosFiltrados;
}



