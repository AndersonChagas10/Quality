function getParHeaderField() {

    return '<div id="headerField" class="col-xs-12 alert-warning" style="padding-top:10px;padding-bottom:10px;display:table;">' +
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
            html += '<select class="form-control input-sm ddl" id="cb' + parheaderField.Id + '" name="cb" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldtype_Id + '" idpai="0" linknumberevaluetion="false" data-required="' + required + '"">';
            html += getParMultipleValues(parheaderField);
            html += '</select>';
            html += '</div>';
            break;
        // case 2:	//Integrações 
        //     break;
        case 3:	//Binario 
            html += '<div id="" class="col-sm-3" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<select class="form-control input-sm ddl" id="cb' + parheaderField.Id + '" name="cb" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldtype_Id + '" idpai="0" linknumberevaluetion="false" data-required="' + required + '"">';
            html += getParMultipleValues(parheaderField);
            html += '</select>';
            html += '</div>';
            break;
        case 4:	//Texto 
            html += '<div id="" class="col-sm-3" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<input class="form-control input-sm" type="text" id="cb' + parheaderField.Id + '" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldType_Id + '" data-required="' + required + '"">'
            html += '</div>';
            break;
        case 5:	//Numerico 
            html += '<div id="" class="col-sm-3" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<input class="form-control input-sm " type="number" id="cb' + parheaderField.Id + '" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldType_Id + '" data-required="' + required + '"">';
            html += '</div>';
            break;
        case 6:	//Data 
            html += '<div id="" class="col-sm-3" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<input class="form-control input-sm " type="date" id="cb' + parheaderField.Id + '" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldType_Id + '" data-required="' + required + '"">';
            html += '</div>';
            break;
        case 7:  //Hora
            html += '<div id="" class="col-sm-3" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<input class="form-control input-sm " type="time" id="cb' + parheaderField.Id + '" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldType_Id + '" data-required="' + required + '"">'
            html += '</div>';
            break;
        case 8:	//Informações
            html += '<div id="" class="col-sm-3" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '</div>';
            break;
        case 9:	//Parâmetro: texto
            break;
        case 10: //Dinâmico: texto
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