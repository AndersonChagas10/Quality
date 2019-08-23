function openModal(title, body, callback) {

    createModal(title, body, callback);

    $('#modal-mensagem').modal('show');

}

function createModal(title, body, callback) {

    var modal =
        '<div class="modal fade" id="modal-mensagem" style="top:5% !important; width: auto;"> ' +
        '   <div class="modal-dialog modal-lg"> ' +
        '       <div class=""> ' +
        '           <div class="modal-header"> ' +
        '           <button type="button" class="btn btn-default" data-dismiss="modal" onclick="closeModal(' + callback + ')" style="float:right">' + getResource('close') + '</button> ' +
        '           <h4 class="modal-title">' + title + '</h4> ' +
        '           </div> ' +
        '           <div class="modal-body" style="overflow-y: scroll;"> ' +
        '               <p>' + body + '</p> ' +
        '           </div> ' +
        // '           <div class="modal-footer"> ' +
        // '           </div> ' +
        '       </div> ' +
        '   </div> ' +
        '</div>';

    $('body').prepend(modal);
}


function closeModal(callback) {

    if (callback) {
        callback();
    }

    deleteModal();
}

function deleteModal() {
    setTimeout(function() {
        $('body').find('#modal-mensagem').remove();
    }, 2000);
}
