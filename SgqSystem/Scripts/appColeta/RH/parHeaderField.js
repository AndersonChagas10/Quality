function getParHeaderFieldDeparment() {

    var ParLevelHeaderFiel_Id = 3; //ParDepartment

    return '<div id="headerFieldDepartment" class="col-xs-12 alert-warning" style="padding-top:10px;padding-bottom:10px;display:table;">' +
        montarBotoesRotinaIntegracao() +
        montarHeaderFields(ParLevelHeaderFiel_Id, currentParDepartment_Id) +
        '</div>';


}

function getParHeaderFieldLevel1(parLevel1) {

    var parLevelHeaderFiel_Id = 1; //ParLevel1

    var cabecalhos = montarHeaderFields(parLevelHeaderFiel_Id, parLevel1.Id)

    if (cabecalhos)

        return '<div id="headerFieldLevel1" data-collapse-target="' + parLevel1.Id + '" class="col-xs-12" parLevel1Id=' + parLevel1.Id + ' style="padding-top:10px;padding-bottom:10px;display:table;background-color:#edf5fc;">' +
            montarBotoesRotinaIntegracao() +
            cabecalhos +
            '</div>';

    else
        return '';
}

function getParHeaderFieldLevel2(parLevel1, parLevel2) {

    var parLevelHeaderFiel_Id = 2; //ParLevel2

    var cabecalhos = montarHeaderFields(parLevelHeaderFiel_Id, parLevel2.Id)

    if (cabecalhos)

        return '<div id="headerFieldLevel2" data-collapse-target="' + parLevel1.Id + '-' + parLevel2.Id + '" class="col-xs-12" parLevel1Id=' + parLevel1.Id + ' parLevel2Id=' + parLevel2.Id + ' style="padding-top:10px;padding-bottom:10px;display:table;background-color:#fcf4e3;">' +
            montarBotoesRotinaIntegracao() +
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

function getParHeaderFieldGeralLevel3(parLevel1, parLevel2, parLevel3) {
    //buscar os campos de cabeçalho no nivel da tarefa
    var lista = [];
    parametrization.listaParHeaderFieldGeral.forEach(function (o, i) {
        if (parLevel3.ParLevel3Value != undefined) {
            if (o.Generic_Id == parLevel3.ParLevel3Value.Id && o.ParLevelHeaderField_Id == 4)
                lista.push(o);
        }
    });

    if (lista.length > 0) {
        var retorno = '';
        var flagPullRight = 'pull-right';
        retorno += ' <div class="col-xs-12 clearfix" id="headerFieldLevel3" parLevel1Id=' + parLevel1.Id + ' parLevel2Id=' + parLevel2.Id + '  parLevel3Id=' + parLevel3.Id + ' data-level3 style="padding-left:10px;background-color: #e9ecef; padding-bottom: 5px;">';
        lista.forEach(function (o, i) {
            
            //retorno += ' <div class="col-xs-3 no-gutters pull-right">';
           // retorno += ' <div class="col-xs-3"><small style="font-weight:550 !important">' + o.Name + '</small></div>';
            //retorno += ' <div class="col-xs-12">';
            retorno += getInputOrSelect(o, flagPullRight, "disabled");
            //retorno += ' </div>';
            //retorno += ' </div>';
        });
        retorno += '</div>';

    } else
        return '';

    return retorno;
}


function montarHeaderFields(parLevelHeaderFiel_Id, Generic_Id) {

    var html = "";
    var flagPullRight = "";

    var headerFields = getHeaderFileds(parLevelHeaderFiel_Id, Generic_Id);

    if (headerFields && headerFields.length)
        headerFields.forEach(function (headerField) {
            html += getInputOrSelect(headerField,flagPullRight, "disabled");
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


function getInputOrSelect(parheaderField, flagPullRight, flagDisableRemoveDuplicate) {

    var html = "";

    var required = (parheaderField.IsRequired == 1 ? 'true' : 'false');

    var htmlDuplicate = "";

    if (parheaderField.Duplicate == true) {

        htmlDuplicate += "<button class='btn btn-sm' data-duplicate-click-add=" + parheaderField.Id + " type='button'><i class='fa fa-plus' aria-hidden='true'></i></button>";
        htmlDuplicate += "<button class='btn btn-sm " + flagDisableRemoveDuplicate + "' data-duplicate-click-remove type='button'><i class='fa fa-minus' aria-hidden='true'></i></button>";
    }

    switch (parheaderField.ParFieldType_Id) {
        case 1: // Multipla Escolha 
            html += '<div id="" class="col-sm-3 ' + flagPullRight + '" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<select class="form-control input-sm ddl" id="cb' + parheaderField.Id + '" name="cb" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldtype_Id + '" idpai="0" linknumberevaluetion="false" data-required="' + required + '" >';
            html += getParMultipleValues(parheaderField);
            html += '</select>';
            html += htmlDuplicate;
            html += '</div>';
            break;
        // case 2:	//Integrações 
        //     break;
        case 3:	//Binario 
            html += '<div id="" class="col-sm-3 ' + flagPullRight + '" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<select class="form-control input-sm ddl" id="cb' + parheaderField.Id + '" name="cb" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldtype_Id + '" idpai="0" linknumberevaluetion="false" data-required="' + required + '" >';
            html += getParMultipleValues(parheaderField);
            html += '</select>';
            html += htmlDuplicate;
            html += '</div>';
            break;
        case 4:	//Texto 
            html += '<div id="" class="col-sm-3 ' + flagPullRight + '" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<input class="form-control input-sm" type="text" id="cb' + parheaderField.Id + '" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldType_Id + '" data-required="' + required + '" >';
            html += htmlDuplicate;
            html += '</div>';
            break;
        case 5:	//Numerico 
            html += '<div id="" class="col-sm-3 ' + flagPullRight + '" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<input class="form-control input-sm " type="number" id="cb' + parheaderField.Id + '" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldType_Id + '" data-required="' + required + '" >';
            html += htmlDuplicate;
            html += '</div>';
            break;
        case 6:	//Data 
            debugger
            html += '<div id="" class="col-sm-3 ' + flagPullRight + '" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<input class="col-xs-12 input-sm" type="date" id="cb' + parheaderField.Id + '" data-cb="cb' + parheaderField.Id + '" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldType_Id + '" data-required="' + required + '" >';
            html += htmlDuplicate;
            html += '</div>';
            break;
        case 7:  //Hora
            html += '<div id="" class="col-sm-3 ' + flagPullRight + '" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<input class="form-control input-sm " type="time" id="cb' + parheaderField.Id + '" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldType_Id + '" data-required="' + required + '" >';
            html += htmlDuplicate;
            html += '</div>';
            break;
        case 8:	//Informações
            html += '<div id="" class="col-sm-3 ' + flagPullRight + '" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<br><button onclick="showInfo(this)" type="button" class="btn btn-info form-control input-sm" data-header-info="' + parheaderField.Description + '">Info</button></div>';
            html += htmlDuplicate;
            html += '</div>';
            break;
        case 9:	//Parâmetro: texto
            //input do tipo texto quando 
            html += '<div id="" class="col-sm-3 ' + flagPullRight + '" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<input class="form-control input-sm" type="text" id="cb' + parheaderField.Id + '" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldType_Id + '" data-required="' + required + '" data-param="' + parheaderField.Description + '">';
            html += htmlDuplicate;
            html += '</div>';
            break;
        case 10: //Dinâmico: texto
            html += '<div id="" class="col-sm-3 ' + flagPullRight + '" name="" style="margin-bottom: 4px;">';
            html += '<label class="font-small" style="height: 22px;">' + parheaderField.Name + '</label>';
            html += '<input class="form-control input-sm" type="text" id="cb' + parheaderField.Id + '" parheaderfield_id="' + parheaderField.Id + '" parfieldtype_id="' + parheaderField.ParFieldType_Id + '" data-required="' + required + '" data-din="' + parheaderField.Description + '" readonly>';
            html += htmlDuplicate;
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

