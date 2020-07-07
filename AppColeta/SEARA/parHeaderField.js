function getParHeaderFieldDeparment() {

    var ParLevelHeaderFiel_Id = 3; //ParDepartment

    return '<div id="headerFieldDepartment" class="col-xs-12 alert-warning" style="padding-top:10px;padding-bottom:10px;display:table;">' +
        montarHeaderFields(ParLevelHeaderFiel_Id, 0) +
        '</div>';


}

function getParHeaderFieldLevel1(parLevel1) {

    var parLevelHeaderFiel_Id = 1; //ParLevel1

    var cabecalhos = montarHeaderFields(parLevelHeaderFiel_Id, parLevel1.Id)

    if (cabecalhos)

        return '<div id="headerFieldLevel1" data-collapse-target="'+parLevel1.Id+'" class="col-xs-12" parLevel1Id=' + parLevel1.Id + ' style="padding-top:10px;padding-bottom:10px;display:table;background-color:#edf5fc;">' +
            cabecalhos +
            '</div>';

    else
        return '';
}

function getParHeaderFieldLevel2(parLevel1, parLevel2) {

    var parLevelHeaderFiel_Id = 2; //ParLevel2

    var cabecalhos = montarHeaderFields(parLevelHeaderFiel_Id, parLevel2.Id)

    if (cabecalhos)

        return '<div id="headerFieldLevel2" data-collapse-target="'+parLevel1.Id+'-'+parLevel2.Id+'" class="col-xs-12" parLevel1Id=' + parLevel1.Id + ' parLevel2Id=' + parLevel2.Id + ' style="padding-top:10px;padding-bottom:10px;display:table;background-color:#fcf4e3;">' +
            cabecalhos +
            '</div>';

    else
        return '';
}

function getParQualification(parLevel1, parLevel2, parLevel3) {

    var retorno = '';

    if (validaParqualification(parLevel1.Id, parLevel2.Id, parLevel3.Id).length > 0) {

        retorno += ' <div class="col-xs-12 hidden" data-collapse-target="' + parLevel1.Id + '-' + parLevel2.Id + '" style="padding-left:10px;background-color: #e9ecef; padding-bottom: 5px;" data-level3 data-qualificationLevel3Value parLevel1Id=' + parLevel1.Id + ' parLevel2Id=' + parLevel2.Id + '  parLevel3Id=' + parLevel3.Id + '>';
        retorno += ' <div class="clearfix"></div>';
        retorno += '</div>';

    }

    return retorno;
}

function montarHeaderFields(parLevelHeaderFiel_Id, Generic_Id) {

    var html = "";

    var headerFields = getHeaderFileds(parLevelHeaderFiel_Id, Generic_Id);

    if (headerFields && headerFields.length)
        headerFields.forEach(function (headerField) {
            html += getInputOrSelect(headerField);
        });

    return html;

}

function getHeaderFileds(parLevelHeaderFiel_Id, Generic_Id) {

    var headerFields = [];

    headerFields = $.grep(parametrization.listaParHeaderFieldGeral, function (headerFieldGeral) {

        return headerFieldGeral.Generic_Id == Generic_Id && headerFieldGeral.ParLevelHeaderField_Id == parLevelHeaderFiel_Id;

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
            html += '<input class="form-control input-sm" type="text" id="cb' + parheaderField.Id + '" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldType_Id + '" data-required="' + required + '" >';
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
            html += '<input class="form-control input-sm " type="time" id="cb' + parheaderField.Id + '" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldType_Id + '" data-required="' + required + '" >';
            html += '</div>';
            break;
        case 8:	//Informações
            html += '<div id="" class="col-sm-3" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<br><button onclick="showInfo(this)" type="button" class="btn btn-info form-control input-sm" data-header-info="' + parheaderField.Description + '">Info</button></div>';
            html += '</div>';
            break;
        case 9:	//Parâmetro: texto
            //input do tipo texto quando 
            html += '<div id="" class="col-sm-3" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<input class="form-control input-sm" type="text" id="cb' + parheaderField.Id + '" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldType_Id + '" data-required="' + required + '" data-param="' + parheaderField.Description + '">';
            html += '</div>';
            break;
        case 10: //Dinâmico: texto
            html += '<div id="" class="col-sm-3" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<input class="form-control input-sm" type="text" id="cb' + parheaderField.Id + '" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldType_Id + '" data-required="' + required + '" data-din="' + parheaderField.Description + '" readonly>';
            html += '</div>';
            //input do tipo texto quando 
            break;
        default:
            break;
    }

    return html;

}

function getParMultipleValues(parheaderField) {

    var parMultipleValues = $.grep(parametrization.listaParMultipleValuesGeral, function (parMultipleValueGeral) {
        return parMultipleValueGeral.ParHeaderFieldGeral_Id == parheaderField.Id;
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

function showInfo(btn) {

    var mensagem = $(btn).attr('data-header-info');
    var html = '<p class="text-justify">' + mensagem + '</p><br><button onclick="closeModal()" type="button" class="btn btn-info float-right">Fechar</button>';

    openModal(html, '#dcdcdc', 'black');

}

