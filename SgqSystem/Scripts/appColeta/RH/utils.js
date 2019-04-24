// function openModal(title, body, callback) {

//     var modal =
//         '<div class="modal modal-lg fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="...">' +
//         '<div class="modal-dialog" role="document">' +
//         '<div class="modal-content">' +
//         '<div class="modal-header">' +
//         '<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>' +
//         '<h4 class="modal-title">' + title + '</h4>' +
//         '</div>' +
//         '<div class="modal-body">' +
//         '</div>' +
//         '<div class="modal-footer">' +
//         '<button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>' +
//         '</div>' +
//         '</div>' +
//         '</div>' +
//         '</div>';

//     $('body').append(modal);

//     $('#myModal .modal-body').html(body);

//     $('#myModal').on('hidden.bs.modal', function (e) {
//         $('#myModal').remove();
//     });

//     // $('#myModal').on('hidden.bs.modal', function (e) {
//     //     callback();
//     // });

//     $('#myModal').modal('show');

// }