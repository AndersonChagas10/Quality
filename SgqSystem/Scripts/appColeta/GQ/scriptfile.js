











































































































/***********************************************************************************************
FINAL DOS METODOS QUE GERAM ALERTA
*********************************************************************************************/




































//metodo que insere a lista de usuarios no banco local
//function insertUserFile(username, password) {
//    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
//        fs.root.getFile("users.txt", { create: true, exclusive: false }, function (fileEntry) {
//            $.ajax({
//                //data: {
//                //    Name: username,
//                //    Password: password,
//                //},
//                url: urlPreffix + '/Services/SyncServices.asmx/getCompanyUsers',
//                data: { "ParCompany_Id": $('.App').attr('unidadeid') },

//                type: 'POST',
//                success: function (data) {
//                    //Se não tem mensagem de erro e tem retorno do método
//                    //  if (!data.MensagemExcecao && data.Retorno) {
//                    //var users = '';
//                    ////Percorre o retorno incrementando a lista de usuários
//                    //for(var i = 0; i < data.Retorno.length; i++){
//                    //    var user = data.Retorno[i];
//                    //    var unidadeid = '';
//                    //    if (user.UnitUser[0]) {
//                    //        unidadeid = user.UnitUser[0].UnitId;
//                    //        unidadename = user.UnitUser[0].Unit.Name;
//                    //    }
//                    //    users +=
//                    //    '<div class="user" userid="' + user.Id + '" username="' + user.FullName+ '" userlogin="' + user.Name.toLowerCase() + '" userpass="' +
//                    //    user.Password + '" userprofile="' + user.Role + '" unidadeid="' + unidadeid + '" unidadename="' + unidadename + '"></div>';
//                    //}
//                    //Grava a lista de usuários
//                    var users = $(data).text();
//                    writeFile(fileEntry, new Blob([users], { type: 'text/plain' }));
//                    //Libera a lista de usuários para a pesquisa
//                    appendDevice(users, $('.Users'));
//                    if (username && password)
//                        getLastSync($('.user[userlogin="' + username.toLowerCase() + '"][userpass="' + password + '"]'), initialLogin);
//                    $('.Users').empty();
//                }
//            });
//        }, onErrorCreateFile);
//    }, onErrorLoadFs);
//}











///Função que verifica se o APP está conectado
///Aceita o um callback sem parametros




$(document).on('click', '.viewModal .close', function (e) {
    $('.viewModal').fadeOut(function (e) {
        $('.viewModal .body').empty();
    });
});










function apagaValores() {

    $('.level3').find('.value:hidden').text("");
    $('.level3').find('.valueDecimal:hidden').text("");



    setTimeout(apagaValores, 500);
}
setTimeout(apagaValores, 1000);

















