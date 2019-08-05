function getParHeaderField() {

    return '<div id="headerField" class="col-xs-12 alert-warning" style="padding-top:10px;padding-bottom:10px;display:table;">' +
        montarBotoesRotinaIntegracao() +
        montarHeaderFields() +
        '</div>';

}

function montarHeaderFields() {

    var html = "";

    var headerFields = getHeaderFiledsbyDepartment();

    if (headerFields && headerFields.length)
        headerFields.forEach(function (headerField) {
            html += getInputOrSelect(headerField);
        });

    return html;

}

function getHeaderFiledsbyDepartment() {

    var ParDepartmentXHeaderFields = $.grep(parametrization.listaParDepartmentXHeaderField, function (parDepartmentXHeaderField) {
        return parDepartmentXHeaderField.ParDepartment_Id == currentParDepartment_Id;
    });

    if (ParDepartmentXHeaderFields.length == 0) {
        return null;
    }

    var headerFields = [];

    ParDepartmentXHeaderFields.forEach(function (parDepartmentXHeaderField) {

        var headerFieldByDepartment = $.grep(parametrization.listaParHeaderField, function (headerField) {

            return parDepartmentXHeaderField.ParHeaderField_Id == headerField.Id;

        })[0];

        if (headerFieldByDepartment) {

            headerFieldByDepartment.IsRequired = parDepartmentXHeaderField.IsRequired;
            headerFields.push(headerFieldByDepartment);

        }

    });

    return headerFields;

}

function getInputOrSelect(parheaderField) {

    var html = "";

    var required = (parheaderField.IsRequired == 1 ? 'true' : 'false');

    switch (parheaderField.ParFieldType_Id) {
        case 1: // Multipla Escolha 
            html += '<div id="" class="col-sm-3" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<select class="form-control input-sm ddl" id="cb' + parheaderField.Id + '" name="cb" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldtype_Id + '" idpai="0" linknumberevaluetion="false" data-required="' + required + '" >';
            html += getParMultipleValues(parheaderField);
            html += '</select>';
            html += '</div>';
            break;
        // case 2:	//Integrações 
        //     break;
        case 3:	//Binario 
            html += '<div id="" class="col-sm-3" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<select class="form-control input-sm ddl" id="cb' + parheaderField.Id + '" name="cb" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldtype_Id + '" idpai="0" linknumberevaluetion="false" data-required="' + required + '" >';
            html += getParMultipleValues(parheaderField);
            html += '</select>';
            html += '</div>';
            break;
        case 4:	//Texto 
            html += '<div id="" class="col-sm-3" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<input class="form-control input-sm" type="text" id="cb' + parheaderField.Id + '" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldType_Id + '" data-required="' + required + '" >'
            html += '</div>';
            break;
        case 5:	//Numerico 
            html += '<div id="" class="col-sm-3" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<input class="form-control input-sm " type="number" id="cb' + parheaderField.Id + '" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldType_Id + '" data-required="' + required + '" >';
            html += '</div>';
            break;
        case 6:	//Data 
            html += '<div id="" class="col-sm-3" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<input class="form-control input-sm " type="date" id="cb' + parheaderField.Id + '" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldType_Id + '" data-required="' + required + '" >';
            html += '</div>';
            break;
        case 7:  //Hora
            html += '<div id="" class="col-sm-3" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<input class="form-control input-sm " type="time" id="cb' + parheaderField.Id + '" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldType_Id + '" data-required="' + required + '" >'
            html += '</div>';
            break;
        case 8:	//Informações
            html += '<div id="" class="col-sm-3" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<br><button onclick="showInfo(this)" type="button" class="btn btn-info form-control input-sm" data-header-info="' + parheaderField.Description + '">Info</button></div>'
            html += '</div>';
            break;
        case 9:	//Parâmetro: texto
            //input do tipo texto quando 
            html += '<div id="" class="col-sm-3" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<input class="form-control input-sm" type="text" id="cb' + parheaderField.Id + '" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldType_Id + '" data-required="' + required + '" data-param="' + parheaderField.Description + '">'
            html += '</div>';
            break;
        case 10: //Dinâmico: texto
            html += '<div id="" class="col-sm-3" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<input class="form-control input-sm" type="text" id="cb' + parheaderField.Id + '" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldType_Id + '" data-required="' + required + '" data-din="' + parheaderField.Description + '" readonly>'
            html += '</div>';
            //input do tipo texto quando 
            break;
        default:
            break;
    }

    return html;

}

function getParMultipleValues(parheaderField) {

    var parMultipleValues = $.grep(parametrization.listaParMultipleValues, function (parMultipleValue) {
        return parMultipleValue.ParHeaderField_Id == parheaderField.Id;
    });


    if (parMultipleValues.length > 0) {

        var options = '<option value="">Selecione...</option>';

        parMultipleValues.forEach(function (parMultipleValue) {
            options += '<option value="' + parMultipleValue.Id + '" punishmentvalue="' + parMultipleValue.PunishmentValue + '"' + (parMultipleValue.IsDefaultOption == 1 ? " selected" : "") + '>' + parMultipleValue.Name + '</option>';
        });

        return options;

    } else {
        return "";
    }

}

function montarBotoesRotinaIntegracao() {

    var html = "";

    //pegar os listaParDepartmentXRotinaIntegracao do DepartmentCurrent
    var ParDepartmentXRotinaIntegracoes = $.grep(parametrization.listaParDepartmentXRotinaIntegracao, function (parDepartmentXRotinaIntegracao) {
        return parDepartmentXRotinaIntegracao.ParDepartment_Id == currentParDepartment_Id;
    });

    var botoes = [];

    ParDepartmentXRotinaIntegracoes.forEach(function (parDepartmentXRotinaIntegracao) {

        //Listar listar as listaRotinaIntegracao do departamento
        var rotinaIntegracao = $.grep(parametrization.listaRotinaIntegracao, function (rotinaIntegracao) {

            return rotinaIntegracao.Id == parDepartmentXRotinaIntegracao.RotinaIntegracao_Id;

        })[0];

        //lista os botoes que buscam dados offline
        if (!rotinaIntegracao)
            rotinaIntegracao = $.grep(parametrization.listaRotinaIntegracaoOffline, function (rotinaIntegracaoOffline) {
                return rotinaIntegracaoOffline.Id == parDepartmentXRotinaIntegracao.RotinaIntegracao_Id;
            })[0];

        if (rotinaIntegracao)
            botoes.push(rotinaIntegracao);

    });

    //fazer foreach nos botoes
    if (botoes && botoes.length > 0) {
        botoes.forEach(function (botao) {

            //criar os botões de pegar as rotinas 
            html += '<div id="" class="col-sm-3" name="" style="margin-bottom: 4px;">';
            html += '<button type="button" class="btn btn-primary" data-id-rotina="' + botao.Id +
                '" data-headerFields="' + botao.Parametro +
                '" data-isoffline="' + botao.IsOffline +
                '" onclick="getRotina(this);" data-headerFieldsClean="' + botao.Retornos +
                '" data-loading-text="">' + botao.Name + '</button>';
            html += '</div>';

        });
    }

    return html;
}

function showInfo(btn) {

    var mensagem = $(btn).attr('data-header-info');
    var html = '<p class="text-justify">' + mensagem + '</p><br><button onclick="closeModal()" type="button" class="btn btn-info float-right">Fechar</button>';

    openModal(html, '#dcdcdc', 'black');

}

