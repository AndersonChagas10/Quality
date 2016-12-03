//methodoo padrao para criacao de arquivo
function createFile() {
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {

        //console.log('file system open: ' + fs.name);
        fs.root.getFile("database.txt", { create: true, exclusive: false }, function (fileEntry) {
            //console.log("fileEntry is file?" + fileEntry.isFile.toString());
            writeFile(fileEntry, new Blob([$('.Results').html()], { type: 'text/plain' }));
        }, onErrorCreateFile);

    }, onErrorLoadFs);
}
//metodo que insere a lista de usuarios no banco local
function insertUserFile(username, password){
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
        fs.root.getFile("users.txt", { create: true, exclusive: false }, function (fileEntry) {
            $.ajax({
                data: {
                    Name: username,
                    Password: password,
                    },
                url: urlPreffix + '/api/User/GetAllUserValidationAd',
                type: 'POST',
                success: function (data) {
                    //Se não tem mensagem de erro e tem retorno do método
                    if (!data.MensagemExcecao && data.Retorno) {                        
                        var users = '';
                        //Percorre o retorno incrementando a lista de usuários
                        for(var i = 0; i < data.Retorno.length; i++){
                            var user = data.Retorno[i];
                            var unidadeid = '';
                            if (user.UnitUser[0]) {
                                unidadeid = user.UnitUser[0].UnitId;
                                unidadename = user.UnitUser[0].Unit.Name;
                            }                                
                            users += 
                            '<div class="user" userid="' + user.Id + '" username="' + user.FullName+ '" userlogin="' + user.Name.toLowerCase() + '" userpass="' +
                            user.Password + '" userprofile="' + user.Role + '" unidadeid="' + unidadeid + '" unidadename="' + unidadename + '"></div>';
                        }
                        //Grava a lista de usuários
                        writeFile(fileEntry, new Blob([users], { type: 'text/plain' }));
                        //Libera a lista de usuários para a pesquisa
                        appendDevice(users, $('.Users'));
                        if (username && password)
                            getLastSync($('.user[userlogin="' + username.toLowerCase() + '"][userpass="' + password + '"]'), initialLogin);
                        $('.Users').empty();
                    }
                }
            });
        }, onErrorCreateFile);
    }, onErrorLoadFs);
}
//metodo que grava o resultado do html no bnco local
function createFileResult() {
    _writeFile("database.txt", $('.Results').html());
}//metodo padrao que grava dados no arquivo
function writeFile(fileEntry, dataObj) {
    // Create a FileWriter object for our FileEntry (log.txt).
    fileEntry.createWriter(function (fileWriter) {
        fileWriter.onwriteend = function () {
        };
        fileWriter.onerror = function (e) {
        };
        fileWriter.write(dataObj);
    });
}
//metodo padrao que le o arquivo no hmtl de results
function readFile(fileEntry) {
    fileEntry.file(function (file) {
        var reader = new FileReader();
        reader.onloadend = function() {
            if(this.result){
                $('.Results').empty();
                appendDevice($(this.result), $('.Results'));
                cleanResults();
            }
        };
        reader.readAsText(file);
    }, onErrorReadFile);
}
/// <summary>Método que faz o Login</summary>
/// username: Nome de  usuário
/// password: Senha
/// method: call-back (função que vai ser chamada após a execução do método)
function getlogin(username, password, method) {
    //Abre uma requisição de Arquivo.
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
        //Busca o arquivo de usuários que está gravado local no dispositivo
        fs.root.getFile("users.txt", { create: true, exclusive: false }, function (fileEntry) {
            //Abre o arquivo 
            fileEntry.file(function (file) {
                var reader = new FileReader();
                reader.onloadend = function () {
                    //Se existe resultado
                    if (this.result) {
                        //Atualiza a base de usuário no dispositivo para verificar o login digitado
                        appendDevice(this.result, $('.Users'));
                        //Instancia o nome do usuário e a senha criptografada para pesquisa.
                        var u = username;
                        var p = AES.Encrypt(password);
                        //Faz a pesquisa na base de usuários
                        var user = $('.user[userlogin="' + u.toLowerCase() + '"][userpass="' + p + '"]');
                        //Se o usuário existe
                        if (user.length) {
                            //Busca a ultima sincronização
                            getLastSync(user, method);
                        }
                        else {
                            //Se o método callback passado for initialLogin
                            if (method == initialLogin) {
                                //Se o dispositivo estiver conectado
                                if (connectionServer == true) {
                                    //Verifico o Login On Line na API
                                    onlineLogin(username, password);
                                    return;
                                }
                            }
                            //Usuário e senha inválidos
                            Geral.exibirMensagemErro("Username or password is invalid");
                            $('#btnLogin').button('reset');
                        }
                        //Limpa a base de usuários de pesquisa temporária
                        $('.Users').empty();
                    } else {
                        //Se não existe resultado na tabela de usuários e o dispositivo não estiver conectado
                        if (connectionServer == false) {
                            //Informa mensagem que necessita estar on line
                            Geral.exibirMensagemErro("The first login needs to be connected the unit. "  + $('#selectUnit :selected').text());
                            $('#btnLogin').button('reset');
                        } else
                            //Se existe conexão e não tem arquivo, verifica o login online
                            onlineLogin(username, password);
                    }
                };
                reader.readAsText(file);
            }, onErrorReadFile);
        }, onErrorCreateFile);
    });
}
function getloginSignature(username, password, method) {
    //Abre uma requisição de Arquivo.
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
        //Busca o arquivo de usuários que está gravado local no dispositivo
        fs.root.getFile('users.txt', { create: false, exclusive: false }, function (fileEntry) {
            //Abre o arquivo 
            fileEntry.file(function (file) {
                var reader = new FileReader();
                reader.onloadend = function () {
                    //Se existe resultado
                    if (this.result) {
                        //Atualiza a base de usuário no dispositivo para verificar o login digitado
                        appendDevice(this.result, $('.Users'));
                        //Instancia o nome do usuário e a senha criptografada para pesquisa.
                        var u = username;
                        var p = AES.Encrypt(password);
                        //Faz a pesquisa na base de usuários
                        var user = $('.user[userlogin="' + u.toLowerCase() + '"][userpass="' + p + '"]');
                        //Se o usuário existe
                        if (user.length) {
                            //Busca a ultima sincronização
                            method(user);
                        }
                        else {
                            //Se o método callback passado for initialLogin
                         
                            //Usuário e senha inválidos
                            Geral.exibirMensagemErro("Username or password is invalid");
                            $('#btnLogin').button('reset');
                        }
                        //Limpa a base de usuários de pesquisa temporária
                        $('.Users').empty();
                    } 
                };
                reader.readAsText(file);
            }, onErrorReadFile);
        }, onErrorCreateFile);
    });
}

//cria o arquivo de database se ainda nao existir
function loginFile(){
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
        fs.root.getFile("database.txt", { create: false, exclusive: false }, function (fileEntry) {
            readFile(fileEntry);
        }, onErrorCreateFile);
    }, onErrorLoadFs);
}
//pegar os dados do arquivo de database colocar no html
function getOffLineData(firstOnLineSync) {
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
        fs.root.getFile("database.txt", { create: false, exclusive: false }, function (fileEntry) {
            fileEntry.file(function (file) {
                var reader = new FileReader();
                reader.onloadend = function () {
                    if (this.result) {
                        var offlineResults = $(this.result);
                        offlineResults.children('div[sync="true"]').remove();
                        appendDevice(offlineResults, $('.Results'));
                        cleanResults();
                    }
                    if (firstOnLineSync == true) {
                        if (connectionServer == true) {
                            $('#btnSync').click();
                        }
                        else {
                            GetToSyncNoOffLineData("getOffLineData");
                        }
                        //setTimeout(startAutomaticSync, 30000);
                    }
                };
                reader.readAsText(file);
            }, firstLoginWithoutFile);
        }, firstLoginWithoutFile);
    }, firstLoginWithoutFile);
}//gravar o ultimo sincronismo

function firstLoginWithoutFile() {
    if (connectionServer == true) {
        $('#btnSync').click();
    }
}

//ler o arquivo e executa um callback se houver um
function _readFile(filename, callback) {
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
        fs.root.getFile(filename, { create: false, exclusive: false }, function (fileEntry) {
            fileEntry.file(function (file) {
                var reader = new FileReader();
                reader.onloadend = function () {
                    console.log("Read file: " + this.result);
                    if (callback)
                        callback(this.result);
                };
                reader.readAsText(file);
            }, onErrorReadFile);
        }, function () {
            _writeFile(filename, "",  function () {
                if (callback)
                    callback("");
            })
        });
    }, onErrorLoadFs);
};

//deleta o arquivo existente e cria um novo com os novos dados
//se não houver um arquivo para ser deletado, cria um novo arquivo com os novos dados
function _writeFile(filename, obj) {

    var dir = "c:/";

    window.requestFileSystem(dir, 0, function (fs) {
        fs.root.getFile(filename, { create: false }, function (fileEntry) {
            fileEntry.remove(function () {

                window.requestFileSystem(dir, 0, function (fs) {
                    fs.root.getFile(filename, { create: true, exclusive: false }, function (fileEntry) {
                        fileEntry.createWriter(function (fileWriter) {
                            fileWriter.onwriteend = function () {
                            };
                            fileWriter.onerror = function (e) {
                            };
                            fileWriter.write(obj);
                        });
                    }, onErrorCreateFile);
                }, onErrorLoadFs);

            });
        }, function () {

            window.requestFileSystem(dir, 0, function (fs) {
                fs.root.getFile(filename, { create: true, exclusive: false }, function (fileEntry) {
                    fileEntry.createWriter(function (fileWriter) {
                        fileWriter.onwriteend = function () {
                        };
                        fileWriter.onerror = function (e) {
                        };
                        fileWriter.write(obj);
                    });
                }, onErrorCreateFile);
            }, onErrorLoadFs);

        });
    }, onErrorLoadFs);
}

function setLastSync() {
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
        fs.root.getFile("sync.txt", { create: true, exclusive: false }, function (fileEntry) {
            var div = '<div class="LastSync" date="' + dateFormat() + '" datetime="' + dateTimeFormat() + '" userid="' + $('.App').attr('userid') + '" unitid="' + $('#selectUnit :selected').val() + '"></div>';
            writeFile(fileEntry, new Blob([div], { type: 'text/plain' }));
        }, onErrorCreateFile);
    }, onErrorLoadFs);
}

function clearLastSync() {
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
        fs.root.getFile("sync.txt", { create: false }, function (fileEntry) {
            fileEntry.remove(function () {
                console.log('File removed.');
            });
            //var div = "123";
            //writeFile(fileEntry, new Blob([div], { type: 'text/plain' }));
        }, onErrorCreateFile);
    }, onErrorLoadFs);
}

//pega a ultima data de sincronismo e decide qual callback executar
function getLastSync(user, callback) {
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
        fs.root.getFile("sync.txt", { create: false, exclusive: false }, function (fileEntry) {
            fileEntry.file(function (file) {
                var reader = new FileReader();
                reader.onloadend = function () {
                    var lastSync = $(this.result);
                    if ($(user).attr('unidadeid'))
                        if (lastSync.length) {

                            if (dateFormat() > lastSync.attr('date') && (connectionServer == true) || lastSync.attr('unitid') != $('#selectUnit :selected').val()) {
                                Geral.exibirMensagemErro("Sync for the first time today.");
                                $('#btnLogin').button('reset');
                            } else {
                                callback(user);
                            }
                        }
                        else {
                            callback(user);
                        }
                    else {
                        if ($('#messageError:visible').length > 0)
                            Geral.exibirMensagemErro("This user has no access permission on this unit.");                            
                        $('#btnLogin').button('reset');
                    }
                };
                reader.readAsText(file);
            }, onErrorReadFile);
        }, onErrorCreateFile);
    }, onErrorLoadFs);
}
//arquivo de erro para leitura
function onErrorLoadFs(e) {
}
    //cria o arquivo de sincronizacao se nao existir
    function onErrorCreateSyncFile(callback, user) {
        window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
            fs.root.getFile("sync.txt", { create: true, exclusive: false }, function (fileEntry) {
                callback(user);
            }, onErrorCreateFile);
        }, onErrorLoadFs);
    }
    //metodo padrao para erro de criacao de arquivo
    function onErrorCreateFile(e) {
        console.log(e);
    }
    //metodo padrao para erro de leitura de arquivo
    function onErrorReadFile(e) {
        console.log(e);
    }