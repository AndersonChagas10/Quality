var hasPartialSave = false;
var coletasParciais = [];

//Todo: validar se todos os inputs foram preenchidos, caso não tenha sido, alterar flag hasPartialSave para true
function verificaIsPartialSave() {

    hasPartialSave = false;

    if (!currentIsPartialSave) {
        return;
    }

    //percorrer todos os inputs e procurar em todos, se algum não foi preenchido
    var linhasDaColeta = $('form[data-form-coleta] div[data-linha-coleta]');
    var inputsDaColeta = $('form[data-form-coleta] div[data-linha-coleta] input[data-texto]');
    var qualification = $('form[data-form-coleta] div[data-qualificationlevel3value] div[data-qualification-required]');
    var selectQualificationColeta = $('form[data-form-coleta] div[data-qualificationlevel3value] select[data-qualificationselect]');

    var errorCount = 0;
    var inputVal;
    var data;
    var linhaQualification;
    var selectQualification;

    // for (var i = 0; i < qualification.length; i++) {
    //     linhaQualification = qualification[i];
    //     selectQualification = selectQualificationColeta[i];

    //     if ($(linhaQualification).attr('data-qualification-required') == 'true' && $(selectQualification).length > 0) {
    //         if ($(selectQualification).val() == null || $(selectQualification).val() == undefined || $(selectQualification).val() == "") {
    //             errorCount++;
    //         } 
    //     }
    // }

    for (var i = 0; i < linhasDaColeta.length; i++) {

        data = linhasDaColeta[i];

        if (typeof ($(data).find('[data-binario]').val()) != 'undefined') {
            if ($(data).find('[data-binario]').text() == "")
                errorCount++;

        } else if (typeof ($(data).find('input[data-valor]').val()) != 'undefined') {
            if ($(data).find('input[data-valor]').val() == "");
            errorCount++;

        } else if (typeof ($(data).find('input[data-texto]').val()) != 'undefined') {
            if ($(data).find('input[data-valor]').val() == "")
                errorCount++
        } 
    }
    if (errorCount > 0) {
        hasPartialSave = true;
    } 
}


function fieldIsEmpty(campo) {

    if (!currentIsPartialSave)
        return false;

    var data = campo;

    if (typeof ($(data).find('[data-binario]').val()) != 'undefined') {
        if ($(data).find('[data-binario]').text().trim() == "")
            return true;

    } else if (typeof ($(data).find('input[data-valor]').val()) != 'undefined') {
        if ($(data).find('input[data-valor]').val() == "")
            return true;

    } else if (typeof ($(data).find('input[data-texto]').val()) != 'undefined') {
        if ($(data).find('input[data-valor]').val() == "")
            return true;
    } 

    return false;

}

function atualizaColetasParciais(data){

    buscarColetasParciais();

}

function buscarColetasParciais(){

    pingLogado(urlPreffix, function () {

        $.ajax({

            data: JSON.stringify({
                ParCompany_Id: currentParCompany_Id,
                ParFrequency_Id: currentParFrequency_Id,
                ParCluster_Id: currentParCluster_Id,
                CollectionDate: convertDateToJson(currentCollectDate)
            }),
            type: 'POST',
            url: urlPreffix + '/api/AppColeta/GetColetaParcial',
            contentType: "application/json",
            success: function (data) {

                _writeFile("coletasParciais.txt", JSON.stringify(data), function () {

                    atualizaArquivoColetaParciais(data);

                });

            },
            timeout: 600000,
            error: function () {
                
            }

        });
    });
}

function atualizaArquivoColetaParciais(data) {

    coletasParciais = data;
}

function readColetasParciais() {

    _readFile("coletasParciais.txt", function (data) {

        atualizaArquivoColetaParciais(JSON.parse(data));

    });

}

function desabilitaColetados() {


    if (!currentIsPartialSave) {
        return;
    }

    //percorrer todos os inputs e procurar em todos, se algum não foi preenchido
    var linhasDaColeta = $('form[data-form-coleta] div[data-linha-coleta]');
    var inputsDaColeta = $('form[data-form-coleta] div[data-linha-coleta] input[data-texto]');
    var qualification = $('form[data-form-coleta] div[data-qualificationlevel3value] div[data-qualification-required]');
    var selectQualificationColeta = $('form[data-form-coleta] div[data-qualificationlevel3value] select[data-qualificationselect]');

    for (var i = 0; i < qualification.length; i++) {

        var linhaQualification = qualification[i];
        var selectQualification = selectQualificationColeta[i];

        if ($(linhaQualification).attr('data-qualification-required') == 'true' && $(selectQualification).length > 0) {
            if ($(selectQualification).val() == null || $(selectQualification).val() == undefined || $(selectQualification).val() == "") {
                errorCount++;
            }
        }

    }

    for (var i = 0; i < linhasDaColeta.length; i++) {

        debugger

        var data = linhasDaColeta[i];
        var inputVal = inputsDaColeta[i];

        var indicador_Id = parseInt($(data).attr('data-level1'));
        var monitoramento_Id = parseInt($(data).attr('data-level2'));
        var tarefa_Id = parseInt($(data).attr('data-level3'));

        var coleta = $.grep(coletasParciais, function (o, i) {

            return o.ParCompany_Id == currentParCompany_Id &&
                o.ParDepartment_Id == currentParDepartment_Id &&
                o.ParCluster_Id == currentParCluster_Id &&
                o.Parfrequency_Id == currentParFrequency_Id &&
                o.ParCargo_Id == currentParCargo_Id &&
                o.ParLevel1_Id == indicador_Id &&
                o.ParLevel2_Id == monitoramento_Id &&
                o.ParLevel3_Id == tarefa_Id &&
                //o.CollectionDate == data
                o.Evaluation == currentEvaluationSample.Evaluation &&
                o.Sample == currentEvaluationSample.Sample &&
                o.ParHeaderField_Id == null;

        })[0];

        if (coleta) {

            if (typeof ($(data).find('input[data-valor]').val()) != 'undefined') {
                $(data).find('input[data-valor]').val(coleta.Value);
            }

            if (typeof ($(data).find('input[data-texto]').val()) != 'undefined') {
                $(data).find('input[data-texto]').val(coleta.ValueText);
            }

            if (typeof ($(data).find('[data-binario]').val()) != 'undefined') {
                setBinarioRespondido(data, coleta.IsConform);
            }

            $(data).find('input, button').prop("disabled", true);
            $(data).css("background-color", "#999");

        }
    }
}

function setBinarioRespondido(self, isConform) {

    var linha = $(self).parents('[data-conforme]');

    resetarLinha(linha);
    linha.attr('data-conforme', isConform ? 0 : 1);
    setFieldColorGray($(self));

    var button = $(self).find('[data-binario]');

    if (isConform) {

        $(button).text($(button).attr('data-positivo'));

    } else {

        $(button).text($(button).attr('data-negativo'));

    }

    validateShowQualification(linha);

    $(self).addClass('btn-default');
    $(self).removeClass('btn-secundary');

}