
function keyDuplicatedCheck(level1, level2, evaluation, sample, collectiondate, reaudit, reauditnumber) {
    var key = $('.App').attr('unidadeid');
    key += '-' + $('.App').attr('shift');
    key += '-' + $('.App').attr('period');
    key += '-' + level1.attr('id');
    key += '-' + level2.attr('id');
    key += '-' + evaluation;
    key += '-' + sample;
    key += '-' + dateDataBaseConvert(collectiondate);
    if (reaudit)
        key += '-r'+reauditnumber;

    var keyLevel1Group = $('.ResultsKeys .ResultLevel2Key[parlevel1_id=' + level1.attr('id') + ']');
    if (keyLevel1Group.length) {
        var checkKey = keyLevel1Group.children('.collectionLevel2Key[id="' + key + '"]');
        if (checkKey.length) {
            return true;
        }
    }

    keyCreate(level1, key);
    return false;
}
function keyCreate(level1, key) {

    var keyLevel1Group = $('.ResultsKeys .ResultLevel2Key[parlevel1_id=' + level1.attr('id') + ']');
    if (!keyLevel1Group.length) {
        keyLevel1Group = $('<div parlevel1_id=' + level1.attr('id') + ' class="ResultLevel2Key"></div>');
        appendDevice(keyLevel1Group, $('.ResultsKeys'));
    }

    var key = $('<div class="collectionLevel2Key" id=' + key + '></div>');

    appendDevice(key, keyLevel1Group);

}

function getCollectionKeys(ParCompany_Id) {

    var date = getCollectionDate();
    if (date == undefined) {
        date = dateTimeFormat();
    }

    $.ajax({
        data: JSON.stringify({
            "ParCompany_Id": ParCompany_Id,
            "date": date,
            "ParLevel1_Id": 0
        }),
        url: urlPreffix + '/api/SyncServiceApi/getCollectionLevel2Keys',
        headers: token(),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        success: function (data) {

            $('.ResultsKeys').empty();

            var keys = $(data).text();
            appendDevice(keys, $('.ResultsKeys'));

            // wMessage($('#btnLoginOnline'), getResource('recording_information'));

            // setTimeout(function (e) {
            //     gravaBancoDadosOffLine();
            // }, 500);
        },
        timeout: 600000,
        error: function () {
            contagem++;
            if (contagem > 2) {
                contagem = 0;
                Geral.exibirMensagemErro(getResource("keys_error_try_again"));
                return false;
            }
            wMessage($('#btnLoginOnline'), getResource("trying_again"));
            setTimeout(function (e) {
                wMessage($('#btnLoginOnline'), getResource('verifying_keys'));
                setTimeout(function (e) {
                    getCollectionKeys(ParCompany_Id);
                }, 800);
            }, 1500);
        }
    });
}