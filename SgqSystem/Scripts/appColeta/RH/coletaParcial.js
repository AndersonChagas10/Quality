var hasPartialSave = false;
var coletasParciais = [];

//coletar sem validar
function preparaColetaParcialFim() {

    closeMensagemImediatamente();
    PrepararColetas();

}

function preparaColetaParcial() {

    closeMensagemImediatamente();

    if (currentIsPartialSave) {
        verificaIsPartialSave();
    }

    PrepararColetas();

}

function verificaIsPartialSave() {

    if (!currentIsPartialSave) {
        return;
    }

    var linhasDaColeta = $('form[data-form-coleta] div[data-linha-coleta]');
    var errorCount = 0;
    var data;

    for (var i = 0; i < linhasDaColeta.length; i++) {

        data = linhasDaColeta[i];

        if (typeof ($(data).find('[data-binario]').val()) != 'undefined') {
            if ($(data).find('[data-binario]').text().trim() == "" && !isTarefaNA(data))
                errorCount++;

        } else if (typeof ($(data).find('input[data-valor]').val()) != 'undefined') {
            if ($(data).find('input[data-valor]').val() == "" && !isTarefaNA(data))
                errorCount++;

        } else if (typeof ($(data).find('input[data-texto]').val()) != 'undefined') {
            if ($(data).find('input[data-texto]').val() == "" && !isTarefaNA(data))
                errorCount++
        } 
    }
    if (errorCount > 0) {
        hasPartialSave = true;
    } 
}

function isTarefaNA(linhaTarefa) {

    var isNA = $(linhaTarefa).is('[data-conforme-na]');

    return isNA;
}

function hasOnlyTextField() {

    //Se apenas existir campos textos vazios para serem preenchidos
    if (!currentIsPartialSave) {
        return false;
    }

    var linhasDaColeta = $('form[data-form-coleta] div[data-linha-coleta]');
    var data;
    var existeCampoTextoVazio = false;
    var existeCampoVazio = false;

    for (var i = 0; i < linhasDaColeta.length; i++) {

        data = linhasDaColeta[i];

        var inputType = parseInt($(data).attr('data-input-type'));

        switch (inputType) {
            case 1:
            case 6:
                if ($(data).find('[data-binario]').text().trim() == ""  && !isTarefaNA(data))
                    existeCampoVazio = true;

                break;

            case 5:
            case 11:
                if (($(data).find('input[data-texto]').val() == "" || $(data).find('input[data-valor]').val() == "")  && !isTarefaNA(data))
                    existeCampoTextoVazio = true;

                break;

            default:
                if ($(data).find('input[data-valor]').val() == ""  && !isTarefaNA(data))
                    existeCampoVazio = true;

                break;
        }
    }

    //validar campos de cabeçalho faltantes
    var cabecalhos = $("[id=headerFieldDepartment],[id=headerFieldLevel1],[id=headerFieldLevel2],[id=headerFieldLevel3]");

    cabecalhos.find("select, input").each(function(i, o){

        if ($(o).is("select")) {
            if ($(o).find(":selected").val() == "")
                existeCampoTextoVazio = true;
        } else {
            if ($(o).text() == "") {
                existeCampoTextoVazio = true;
            }
        }

    });

    if (existeCampoTextoVazio && !existeCampoVazio) {
        return true;
    } else {
        return false;
    }
}

function fieldIsEmpty(campo) {

    if (!currentIsPartialSave)
        return false;

    var data = campo;

    if (typeof ($(data).find('[data-binario]').val()) != 'undefined') {
        if ($(data).find('[data-binario]').text().trim() == "" && !isTarefaNA(data))
            return true;

    } else if (typeof ($(data).find('input[data-valor]').val()) != 'undefined') {
        if ($(data).find('input[data-valor]').val() == "" && !isTarefaNA(data))
            return true;

    } else if (typeof ($(data).find('input[data-texto]').val()) != 'undefined') {
        if ($(data).find('input[data-texto]').val() == "" && !isTarefaNA(data))
            return true;
    }

    return false;

}

function atualizaColetasParciais(data){

    buscarColetasParciais();

}

function buscarColetasParciais(){

    pingLogado(urlPreffix, function () {

        openMensagem("Aguarde, Carregando Coletas Parciais...", "blue", "white");

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
                atualizaArquivoColetaParciais(data);
                closeMensagemImediatamente();
            },
            timeout: 600000,
            error: function () {

            }

        });
    });
}

function atualizaArquivoColetaParciais(data, callback) {

    _writeFile("coletasParciais.txt", JSON.stringify(data), function () {

        atualizaVariavelColetaParciais(data);

        if (callback)
            callback();

    });
}

function atualizaVariavelColetaParciais(data) {

    coletasParciais = data;
}

function addColetasParciais(data, callback) {

    data = $.grep(data, function (o, i) {
        return o.IsPartialSave === true;
    });

    if (data && data.length > 0) {

        data = coletasParciais.concat(data);

        atualizaArquivoColetaParciais(data, callback);

    }

}

function readColetasParciais(callback) {

    _readFile("coletasParciais.txt", function (data) {

        atualizaVariavelColetaParciais(JSON.parse(data));

        if (callback)
            callback();

    });

}

function desabilitaColetados() {

    if (!currentIsPartialSave) {
        return;
    }

    var linhasDaColeta = $('form[data-form-coleta] div[data-linha-coleta]');

    for (var i = 0; i < linhasDaColeta.length; i++) {

        var data = linhasDaColeta[i];
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

            setLeve3IsNA(coleta, data);
            $(data).find('input, button').prop("disabled", true);
            $(data).css("background-color", "#999");
            validateShowQualification(data);

        }
    }

    desabilitaCamposCabecalho();
}

function desabilitaCamposCabecalho() {
    
    setHeaderFieldLevel1();
    setHeaderFieldLevel2();
    setHeaderFieldLevel3();
    setQualificationField();
}

function setLeve3IsNA(coleta, linha) {

    if (coleta.IsNotEvaluate) {
        $(linha).find('[data-na]').trigger('click');
    }

}

function setQualificationField(){

    var qualification = $('form[data-form-coleta] div[data-qualificationlevel3value]');

    qualification.each(function (index, value) {

        var inputSelect = $(value).find('input, select');
        var parLevel1_Id = parseInt($(value).attr('parlevel1id'));
        var parLevel2_Id = parseInt($(value).attr('parlevel2id'));
        var parLevel3_Id = parseInt($(value).attr('parlevel3id'));

        inputSelect.each(function (index, value) {

            var coletaQualification = $.grep(coletasParciais, function (o, i) {

                return o.ParCompany_Id == currentParCompany_Id &&
                    o.ParDepartment_Id == currentParDepartment_Id &&
                    o.ParCluster_Id == currentParCluster_Id &&
                    o.Parfrequency_Id == currentParFrequency_Id &&
                    o.ParCargo_Id == currentParCargo_Id &&
                    o.ParLevel1_Id == parLevel1_Id &&
                    o.ParLevel2_Id == parLevel2_Id &&
                    o.ParLevel3_Id == parLevel3_Id &&
                    o.Evaluation == currentEvaluationSample.Evaluation &&
                    o.Sample == currentEvaluationSample.Sample &&
                    (o.Outros != null && o.Outros != '{"Qualification_Value":[]}');

            })[0];

            if (coletaQualification) {

                var qualificationValue = JSON.parse(coletaQualification.Outros);

                if (qualificationValue && qualificationValue.Qualification_Value[0]) {

                    if ($(value).is("select")) {

                        $(value).val(qualificationValue.Qualification_Value[0])

                    } else {

                        $(value).val(qualificationValue.Qualification_Value[0])
                        $(value).text(qualificationValue.Qualification_Value[0])
                    }

                    $(value).prop("disabled", true);
                }

            }

        });

    });
}

function setHeaderFieldLevel1() {

    var listaHeaderFieldLevel1 = $('[id=headerFieldLevel1]');

    listaHeaderFieldLevel1.each(function (index, value) {

        var inputSelect = $(value).find('input, select');
        var parLevel1_Id = parseInt($(value).attr('parlevel1id'));

        inputSelect.each(function (index, value) {

            var parHeaderField_Id = parseInt($(value).attr('parheaderfield_id'));


            var coletaHeaderFieldL1 = $.grep(coletasParciais, function (o, i) {

                return o.ParCompany_Id == currentParCompany_Id &&
                    o.ParDepartment_Id == currentParDepartment_Id &&
                    o.ParCluster_Id == currentParCluster_Id &&
                    o.Parfrequency_Id == currentParFrequency_Id &&
                    o.ParCargo_Id == currentParCargo_Id &&
                    o.ParLevel1_Id == parLevel1_Id &&
                    o.ParLevel2_Id == null &&
                    o.ParLevel3_Id == null &&
                    o.Evaluation == currentEvaluationSample.Evaluation &&
                    o.Sample == currentEvaluationSample.Sample &&
                    o.ParHeaderField_Id == parHeaderField_Id;

            })[0];

            if (coletaHeaderFieldL1) {

                if ($(value).is("select")) {

                    $(value).val(coletaHeaderFieldL1.ParHeaderField_Value)

                } else {

                    $(value).val(coletaHeaderFieldL1.ParHeaderField_Value)
                    $(value).text(coletaHeaderFieldL1.ParHeaderField_Value)
                }

                $(value).prop("disabled", true);

            }

        });

    });
}

function setHeaderFieldLevel2() {

    var listaHeaderFiledLevel2 = $('[id=headerFieldLevel2]');

    listaHeaderFiledLevel2.each(function(index, value){

        var inputSelect = $(value).find('input, select');
        var parLevel1_Id = parseInt($(value).attr('parlevel1id'));
        var parLevel2_Id = parseInt($(value).attr('parlevel2id'));

        inputSelect.each(function (index, value) {

            var parHeaderField_Id = parseInt($(value).attr('parheaderfield_id'));

            var coletaHeaderFieldL2 = $.grep(coletasParciais, function (o, i) {

                return o.ParCompany_Id == currentParCompany_Id &&
                    o.ParDepartment_Id == currentParDepartment_Id &&
                    o.ParCluster_Id == currentParCluster_Id &&
                    o.Parfrequency_Id == currentParFrequency_Id &&
                    o.ParCargo_Id == currentParCargo_Id &&
                    o.ParLevel1_Id == parLevel1_Id &&
                    o.ParLevel2_Id == parLevel2_Id &&
                    o.ParLevel3_Id == null &&
                    o.Evaluation == currentEvaluationSample.Evaluation &&
                    o.Sample == currentEvaluationSample.Sample &&
                    o.ParHeaderField_Id == parHeaderField_Id;

            })[0];

            if (coletaHeaderFieldL2) {

                if ($(value).is("select")) {

                    $(value).val(coletaHeaderFieldL2.ParHeaderField_Value)

                } else {

                    $(value).val(coletaHeaderFieldL2.ParHeaderField_Value)
                    $(value).text(coletaHeaderFieldL2.ParHeaderField_Value)
                }

                $(value).prop("disabled", true);
            }

        });

    });

}

function setHeaderFieldLevel3() {

    var listaHeaderFieldLevel3 = $('[id=headerFieldLevel3]');

    listaHeaderFieldLevel3.each(function(index, value){

        var inputSelect = $(value).find('input, select');
        var parLevel1_Id = parseInt($(value).attr('parlevel1id'));
        var parLevel2_Id = parseInt($(value).attr('parlevel2id'));
        var parLevel3_Id = parseInt($(value).attr('parlevel3id'));

        inputSelect.each(function (index, value) {

            var parHeaderField_Id = parseInt($(value).attr('parheaderfield_id'));

            var coletaHeaderFieldL3 = $.grep(coletasParciais, function (o, i) {

                return o.ParCompany_Id == currentParCompany_Id &&
                    o.ParDepartment_Id == currentParDepartment_Id &&
                    o.ParCluster_Id == currentParCluster_Id &&
                    o.Parfrequency_Id == currentParFrequency_Id &&
                    o.ParCargo_Id == currentParCargo_Id &&
                    o.ParLevel1_Id == parLevel1_Id &&
                    o.ParLevel2_Id == parLevel2_Id &&
                    o.ParLevel3_Id == parLevel3_Id &&
                    o.Evaluation == currentEvaluationSample.Evaluation &&
                    o.Sample == currentEvaluationSample.Sample &&
                    o.ParHeaderField_Id == parHeaderField_Id;

            })[0];

            if (coletaHeaderFieldL3) {

                if ($(value).is("select")) {

                    $(value).val(coletaHeaderFieldL3.ParHeaderField_Value)

                } else {

                    $(value).val(coletaHeaderFieldL3.ParHeaderField_Value)
                    $(value).text(coletaHeaderFieldL3.ParHeaderField_Value)
                }

                $(value).prop("disabled", true);
            }
        });
    });
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

    $(self).addClass('btn-default');
    $(self).removeClass('btn-secundary');

}