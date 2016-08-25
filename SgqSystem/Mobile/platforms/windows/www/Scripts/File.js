function createFile(){
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {

        console.log('file system open: ' + fs.name);
        fs.root.getFile("database.txt", { create: true, exclusive: false }, function (fileEntry) {
            console.log("fileEntry is file?" + fileEntry.isFile.toString());
            writeFile(fileEntry, new Blob([$('.Results').html()], { type: 'text/plain' }));
        }, onErrorCreateFile);

        fs.root.getFile("users.txt", { create: true, exclusive: false }, function (fileEntry) {
            var users = 
            '<div class="user" userid="1" username="Antonio" userlogin="tony.colorado" userpass="123" userprofile="auditor"></div>'+
            '<div class="user" userid="2" username="Jelsafa" userlogin="jelsafa.colorado" userpass="345" userprofile="techinical"></div>'+
            '<div class="user" userid="3" username="Lucas" userlogin="lucas.colorado" userpass="123" userprofile="audit"></div>';
            writeFile(fileEntry, new Blob([users], { type: 'text/plain' }));
        }, onErrorCreateFile);


    }, onErrorLoadFs);
}

function createFileResult() {
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {

        console.log('file system open: ' + fs.name);
        fs.root.getFile("database.txt", { create: true, exclusive: false }, function (fileEntry) {
            console.log("fileEntry is file?" + fileEntry.isFile.toString());
            writeFile(fileEntry, new Blob([$('.Results').html()], { type: 'text/plain' }));
        }, onErrorCreateFile);
    }, onErrorLoadFs);
}


function writeFile(fileEntry, dataObj) {
    // Create a FileWriter object for our FileEntry (log.txt).
    fileEntry.createWriter(function (fileWriter) {

        fileWriter.onwriteend = function() {
            console.log("Result saved with success");
            //readFile(fileEntry);
        };

        fileWriter.onerror = function (e) {
            console.log("Failed file write: " + e.toString());
        };

        fileWriter.write(dataObj);
    });
}   


function readFile(fileEntry) {

    fileEntry.file(function (file) {
        var reader = new FileReader();

        reader.onloadend = function() {
            //console.log("Successful file read: " + this.result);
            //openMessageModal('leitura', 'Arquivo lido com sucesso.');
            if(this.result){
                $('.Results').empty();
                appendDevice(this.result, $('.Results'));

            }
        };

        reader.readAsText(file);

    }, onErrorReadFile);
}
function getlogin(username, password, method) {
   
   $('.Users').empty();

    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
        fs.root.getFile("users.txt", { create: true, exclusive: false }, function (fileEntry) {
            fileEntry.file(function (file) {
                var reader = new FileReader();

                reader.onloadend = function() {
                    //console.log("Successful file read: " + this.result);
                    //openMessageModal('leitura', 'Arquivo lido com sucesso.');
                    if(this.result){
                        appendDevice(this.result, $('.Users'));
                        

                        var user  = $('.user[userlogin="' + username + '"][userpass="' + password + '"]');
                        // var user = $('.user[userlogin="' + username + '"][userpass="' + password + ']');
                        
                        if(user.length)
                        {
                            method(user);
                        }
                        else
                        {
                           Geral.exibirMensagemErro("Username or password is invalid");
                        }

                         $('.Users').empty();

                         return true;
                       
                    }
                };

                 reader.readAsText(file);

            }, onErrorReadFile);
        }, onErrorCreateFile);
    });
}

function loginFile(){
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
        fs.root.getFile("database.txt", { create: false, exclusive: false }, function (fileEntry) {
            readFile(fileEntry);
        }, onErrorCreateFile);
    }, onErrorLoadFs);
}

function onErrorLoadFs(e) {
    //openMessageModal( "FileSystem Error: "+e);
    console.log(e);
}

function onErrorCreateFile(e){
    //openMessageModal( "FileSystem Error: "+e);
    console.log(e);
}

function onErrorReadFile(e){
    //openMessageModal( "FileSystem Error: "+e);
    console.log(e);
}