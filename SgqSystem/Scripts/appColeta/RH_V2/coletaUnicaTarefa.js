$(document)
//.off('keyup click change', 'form[data-form-coleta] div[data-linha-coleta]')
.on('keyup click change', 'form[data-form-coleta] div[data-linha-coleta]', function () {

    if (globalDicionarioEstatico.clustersIdsColetaUnicaTarefa == undefined) {
        globalDicionarioEstatico = JSON.parse(globalDicionarioEstatico);
    }

    if ($(this).find('input, button').prop('disabled')
        || globalDicionarioEstatico.clustersIdsColetaUnicaTarefa.indexOf('|' + currentParCluster_Id + '|') < 0)
        return;

    var isNA = $(this).attr('data-conforme-na') == "";

    if (($(this).attr('data-input-type') == "11" && $(this).find('input[data-texto]').val() != 'undefined' && $(this).find('input[data-texto]').val() != '')
        || ($(this).find('input[data-valor]').length > 0 && $(this).find('input[data-valor]').val() != 'undefined' && $(this).find('input[data-valor]').val() != '')
        || ($(this).find('button[data-binario]').length > 0 && $(this).attr('data-conforme') != $(this).attr('data-default-answer'))
        || isNA) {

        disableAllInputsButThis(this);

    } else if (($(this).attr('data-input-type') == "6" && $(this).find('input[data-texto]').val() != 'undefined' && $(this).find('input[data-texto]').val() != '')) {

        var texto = $(this).find('input[data-texto]').val();

        $(this).find('[data-binario]').trigger('click');

        disableAllInputsButThis(this);
        $(this).find('input[data-texto]').val(texto);

    }
    else {
        enableAllInputs();
    }

    validaAllInputsRequired()
});


function disableAllInputsButThis(self) {

    $('form[data-form-coleta] div[data-linha-coleta]')
        .not(self)
        .css("background-color", "#999")
        .find('input, button')
        .css("background-color", "")
        .prop("disabled", true);

    $('form[data-form-coleta] div[data-linha-coleta]')
        .not(self)
        .addClass('naoSalvar');

    $('.headerFieldL3, .headerFieldL3 input, .headerFieldL3 select').not('[parlevel1id="' + $(self).attr('data-level1') + '"][parlevel2id="' + $(self).attr('data-level2') + '"][parlevel3id="' + $(self).attr('data-level3') + '"]')
        .addClass('naoSalvar')
        .not(self)
        .css("background-color", "#999")
        .find('input, button').prop("disabled", true);

}

function enableAllInputs() {

    $('form[data-form-coleta] div[data-linha-coleta]').css("background-color", "#FFFFFF")
        .find('input, button')
        .prop("disabled", false);

    $('form[data-form-coleta] div[data-linha-coleta]')
        .removeClass('naoSalvar');

    $('.headerFieldL3, .headerFieldL3 input, .headerFieldL3 select')
        .removeClass('naoSalvar')
        .css("background-color", "#FFFFFF")
        .find('input, button').prop("disabled", false);
}

function disableBtnSave() {
    $('button[data-salvar]').prop("disabled", true);
}

function enableBtnSave() {
    $('button[data-salvar]').prop("disabled", false);
}

function initializeColetaUnica() {
    if (globalDicionarioEstatico.clustersIdsColetaUnicaTarefa.indexOf('|' + currentParCluster_Id + '|') >= 0) {
        enableAllInputs();
        disableBtnSave();
    }
}

function validaAllInputsRequired() {

    if (HeaderFieldsIsValid(false, false) && coletaUnicaInputsIsValid())
        enableBtnSave();
    else
        disableBtnSave();
}

function coletaUnicaInputsIsValid() {

    var linhasDaColeta = $('form[data-form-coleta] div[data-linha-coleta]').not('.naoSalvar');
    var inputsDaColeta = $('form[data-form-coleta] div[data-linha-coleta]').not('.naoSalvar').find('input[data-texto]');
    var qualification = $('form[data-form-coleta] div[data-qualificationlevel3value] div[data-qualification-required]');
    var selectQualificationColeta = $('form[data-form-coleta] div[data-qualificationlevel3value] select[data-qualificationselect]');

    var errorCount = 0;
    var inputVal;
    var data;
    var linhaQualification;
    var selectQualification;

    for (var i = 0; i < qualification.length; i++) {
        linhaQualification = qualification[i];
        selectQualification = selectQualificationColeta[i];

        if ($(linhaQualification).attr('data-qualification-required') == 'true' && $(selectQualification).length > 0) {
            if ($(selectQualification).val() == null || $(selectQualification).val() == undefined || $(selectQualification).val() == "")
                errorCount++;
        }
    }

    for (var i = 0; i < linhasDaColeta.length; i++) {
        data = linhasDaColeta[i];
        inputVal = inputsDaColeta[i];

        if ($(inputVal).attr('data-required-text') == 'true') {
            if ($(inputVal).val() == null || $(inputVal).val() == undefined || $(inputVal).val() == "")
                errorCount++;
        }

        if ($(data).attr('data-conforme-na') != "") {
            if ($(data).attr('data-conforme') == ""
                || $(data).attr('data-conforme') == null
                || $(data).attr('data-conforme') == "undefined")

                errorCount++;
        }
    }
    if (errorCount > 0) {
        return false;
    } else
        return true;
}


$(document).off('keyup click change', '[parheaderfield_id]').on('keyup click change', '[parheaderfield_id]', function () {
    if (globalDicionarioEstatico.clustersIdsColetaUnicaTarefa.indexOf('|' + currentParCluster_Id + '|') >= 0)
        validaAllInputsRequired();
});
