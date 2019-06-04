var ParGroupLevel1_Id = 0;

$(document).on('click', '#btnConfirmModule', function(){

    if($('.module input:checked').length == 0){
        openMessageModal("Selecione um módulo para continuar.", "");
        return;
    } else {
        ParGroupLevel1_Id = parseInt($('.module input:checked').attr('id'));
    }

    ping(paramsUpdate_OnLine, paramsUpdate_OffLine);

    $('.modalSyncInd, .overlay').fadeOut('fast');

    loadFirst = false;
});