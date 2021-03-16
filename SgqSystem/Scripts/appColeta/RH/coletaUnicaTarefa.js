
$(document)
.off('click', 'form[data-form-coleta] div[data-linha-coleta] button')
.on('click', 'form[data-form-coleta] div[data-linha-coleta] button', function () {
    var linhaColeta = $(this).parents('[data-linha-coleta]')[0];

    if (globalDicionarioEstatico) {
        if (globalDicionarioEstatico.clustersIdsColetaUnicaTarefa == undefined) {
            globalDicionarioEstatico = JSON.parse(globalDicionarioEstatico);
        }
    } else {
        openMessageConfirm(
            "Objeto de coleta inválido",
            "Entre em contato com o responsável do SESMT da sua Unidade! Não atualizar a página e tirar print da tela por favor.",
            closeMensagemImediatamente,
            closeMensagemImediatamente,
            "red",
            "white");
    }

    if ($(linhaColeta).hasClass('naoSalvar')
        || globalDicionarioEstatico.clustersIdsColetaUnicaTarefa.indexOf('|' + currentParCluster_Id + '|') < 0)
        return;

    var isNA = $(linhaColeta).attr('data-conforme-na') == "";

    if (($(linhaColeta).attr('data-input-type') == "11" && $(linhaColeta).find('input[data-texto]').val() && $(linhaColeta).find('input[data-texto]').val() != '')
        || ($(linhaColeta).find('input[data-valor]').length > 0 && $(linhaColeta).find('input[data-valor]').val() && $(linhaColeta).find('input[data-valor]').val() != '')
        || ($(linhaColeta).find('button[data-binario]').length > 0 && $(linhaColeta).attr('data-conforme') != $(linhaColeta).attr('data-default-answer'))
        || isNA) {

        $('form[data-form-coleta] div[data-linha-coleta]')
            .not(linhaColeta)
            .addClass('naoSalvar');

        $('form[data-form-coleta] div[data-linha-coleta]')
            .not(linhaColeta)
            .css("background-color", "#999")
            
        $('form[data-form-coleta] div[data-linha-coleta]')
            .not(linhaColeta)
            .find('input, button')
            .prop("disabled", true);

        $('.headerFieldL3, .headerFieldL3 input, .headerFieldL3 select')
            .not('[parlevel1id="' + $(linhaColeta).attr('data-level1') + '"][parlevel2id="' + $(linhaColeta).attr('data-level2') + '"][parlevel3id="' + $(linhaColeta).attr('data-level3') + '"]')
            .addClass('naoSalvar')
            .not(linhaColeta)
            .css("background-color", "#999")
            .find('input, button')
            .prop("disabled", true);

    } else {

        $('form[data-form-coleta] div[data-linha-coleta]')
            .removeClass('naoSalvar');

        $('form[data-form-coleta] div[data-linha-coleta]')
            .css("background-color", "#FFFFFF")
            
        $('form[data-form-coleta] div[data-linha-coleta]')
            .find('input, button')
            .prop("disabled", false);

        $('.headerFieldL3, .headerFieldL3 input, .headerFieldL3 select')
            .removeClass('naoSalvar')
            .css("background-color", "#FFFFFF")
            .find('input, button')
            .prop("disabled", false);

    }
});
