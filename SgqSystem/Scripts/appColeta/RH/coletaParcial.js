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

    for (var i = 0; i < qualification.length; i++) {
        linhaQualification = qualification[i];
        selectQualification = selectQualificationColeta[i];

        if ($(linhaQualification).attr('data-qualification-required') == 'true' && $(selectQualification).length > 0) {
            if ($(selectQualification).val() == null || $(selectQualification).val() == undefined || $(selectQualification).val() == "") {
                errorCount++;
            } 
        }
    }

    for (var i = 0; i < linhasDaColeta.length; i++) {
        data = linhasDaColeta[i];
        inputVal = inputsDaColeta[i];

        if ($(inputVal).attr('data-required-text') == 'true') {

            if ($(data).attr('data-conforme') == "0") {
                if ($(inputVal).val() == null || $(inputVal).val() == undefined || $(inputVal).val() == "") {
                    errorCount++;
                } 
            }

        }

        if ($(data).attr('data-conforme-na') != "") {
            if ($(data).attr('data-conforme') == "" || $(data).attr('data-conforme') == null || $(data).attr('data-conforme') == "undefined") {
                errorCount++;
            }
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
    var inputVal = $(campo).find('input[data-texto]');

    if ($(data).attr('data-conforme') == "0") {
        if ($(inputVal).val() == null || $(inputVal).val() == undefined || $(inputVal).val() == "") {
            return true;
        }
    }

    if ($(data).attr('data-conforme') == "" || $(data).attr('data-conforme') == null || $(data).attr('data-conforme') == "undefined") {
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

        atualizaArquivoColetaParciais(data);

    });

}

function desabilitaColetados(){


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

        var coleta = $.filter(coletasParciais, function(o, i) {

            return o.currentParCompany_Id == currentParCompany_Id &&
                o.currentParDepartmentParent_Id == currentParDepartmentParent_Id &&
                o.currentParDepartment_Id == currentParDepartment_Id &&
                o.currentParClusterGroup_Id == currentParClusterGroup_Id &&
                o.currentParCluster_Id == currentParCluster_Id &&
                o.currentParFrequency_Id == currentParFrequency_Id &&
                o.currentParCargo_Id == currentParCargo_Id &&
                o.ParLevel1_Id == indicador_Id &&
                o.ParLevel2_Id == monitoramento_Id &&
                o.ParLevel3_Id == tarefa_Id &&
                //o.CollectionDate == data
                o.Evaluation == currentEvaluationSample.Evaluation && 
                o.Sample == currentEvaluationSample.Sample;

        })[0];

        if(coleta){


            if (typeof ($(data).find('input[data-valor]').val()) != 'undefined') {
                $(data).find('input[data-valor]').val();
            }

            if (typeof ($(data).find('input[data-texto]').val()) != 'undefined') {
                $(data).find('input[data-texto]').val();
            }

            if (typeof ($(data).find('[data-binario]').val()) != 'undefined') {
                //trocar para o valor que foi coletado (não sei como fazer isso)
            }

            //Inserir valor na linha da coleta

            //Verificar qual é o tipo de entrada

            //Inativar a linha que foi coletado
    

        }

    }

}