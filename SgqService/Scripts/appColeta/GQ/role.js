
function applyRole() {

    attrRoleHide($('#btnSync'), 'COMP001');
    attrRoleHide($('#btnSync'), 'COMP001');
    attrRoleHide($('#btnSyncParam'), 'COMP002');
    attrRoleHide($('#btnLog'), 'COMP003');
    attrRoleHide($('#btnCollectDB'), 'COMP004');
    attrRoleHide($('#btnClearDatabase'), 'COMP005');
    attrRoleHide($('#btnLogout'), 'COMP006');
    attrRoleHide($('#btnMostrarContadores'), 'COMP007');
    attrRoleHide($('#btnDate'), 'COMP008');

    $(".Users .user[userid=" + $('.App').attr('userid') + "] .role").each(function (index, self) {
        $('.' + $(self).text()).removeClass('hide').removeClass('disabled');
    });
    
    if(isEUA)
        $('#btnAutoSend').hide();

}
function attrRoleHide(element, hashkey) {
    $(element).addClass(hashkey).addClass('hasRole').addClass('hide');
}
function attrRoleDisabled(element, hashkey) {
    $(element).addClass(hashkey).addClass('hasRole').addClass('disabled');
}
