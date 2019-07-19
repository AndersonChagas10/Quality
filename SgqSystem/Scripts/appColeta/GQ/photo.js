var level3Photos = [];
var level3PhotoTemp = {};
var algumaFotoEstaSendoEnviada = false;

function displayPhotoButtons() {
    if (permiteTirarFotos == true) {

        if (device.platform.toLowerCase() == "android" || device.platform.toLowerCase() == "windows") {

            var level1 = $('.level1.selected');
            var level2 = $('.level2.selected');

            $('.level3:visible, .panel-group:visible .panel:visible .level3').each(function (index, element) {

                if (level1.attr('hastakephoto') == 'true'
                    || level2.attr('hastakephoto') == 'true'
                    || $(element).attr('hastakephoto') == 'true') {

                    $(element).find('.levelName').parent().removeClass('col-xs-4').addClass('col-xs-3');
                    var btn = $(element).find('.camera-button');
                    if (btn.length == 0) {
                        btn = $('<div class="col-xs-1"><button class="btn btn-sm btn-danger camera-button"><i class="fa fa-camera"></i></button></div>');
                        $(element).append(btn);
                    }
                }

            }, this);

        }
    }
}

function cameraSuccess(data) {

    $('.message, .overlay').hide();

    //level3PhotoTemp.path = data;
    var imageBase64Name = new Date().getTime();
    level3PhotoTemp.photo = imageBase64Name+".base64";
    _writeFile(level3PhotoTemp.photo, data);

    //Insere valor padrão zerado
    level3PhotoTemp.latitude = 0;
    level3PhotoTemp.longitude = 0;

    //CONVERTE O PATH PARA IMAGEM
    salvaFoto(level3PhotoTemp);
}

function hasPhoto(level1id, level2id, level3id, evaluation, sample, date, unitid, period, shift) {
    var temp = [];

    level3Photos.forEach(function (obj) {
        if (obj.level1id == level1id
            && obj.level2id == level2id
            && obj.level3id == level3id
            && obj.evaluation == evaluation
            && obj.unitid == unitid
            && obj.period == period
            && obj.shift == shift
            && obj.sample == sample
            && obj.date == date
        )
            temp.push(obj);
    }, this);

    if (temp.length == 0) {
        return false;
    } else {
        return true;
    }
}

function cameraError(e) {
    console.log("camera error");
}

var cameraOptions = {
    quality: 75,
    destinationType: Camera.DestinationType.DATA_URL,
    targetWidth: 2048,
    targetHeight: 1152,
    correctOrientation: true //Corrects Android orientation quirks
};

$(document).on('click', '.camera-button', function (e) {

    //openMessageModal("Conectando ao GPS...", null);
    //openMessageModal(!!_level2 + '-' + $(_level2).attr('sampleCurrent') + '--' +
    //    parseInt($(_level2).attr('sampleCurrent')) + '---' + $(_level2).attr('samplecurrent')
    //    , null);

    level3PhotoTemp = {
        level1id: $('.level1.selected').attr('id'),
        level2id: $('.level2.selected').attr('id'),
        level3id: $(this).parent().parent().attr('id'),
        //evaluation: parseInt($('.evaluateCurrent:visible').text()),
        //sample: parseInt($('.sampleCurrent:visible').text()),
        evaluation: !!$(_level2).attr('evaluateCurrent') ? parseInt($(_level2).attr('evaluateCurrent')) : 1,
        sample: !!$(_level2).attr('sampleCurrent') ? parseInt($(_level2).attr('sampleCurrent')) + 1 : 1,
        date: getCollectionDate(),
        unitid: $('.App').attr('unidadeid'),
        period: $('.App').attr('period'),
        shift: $('.App').attr('shift'),
        isactive: false
    };

    //getLatLong();
    abrirCamera();

});

function sendResultLevel3Photo() {
    if ($('.level02Result[sync=false]').length == 0 && algumaFotoEstaSendoEnviada != true)
        setTimeout(function () {
            var listaFotosSalvas = level3Photos.filter(function (o, i) { return o.isactive == true });
            if (listaFotosSalvas.length > 0) {
                algumaFotoEstaSendoEnviada = true;
                preparaFotos(listaFotosSalvas);
            }
        }, 1000);
}

function preparaFotos(listaFotosSalvas) {
    
    var listaFotos = [];
    for (i = 0; i < (listaFotosSalvas.length > 1 ? 1 : listaFotosSalvas.length); i++) {
        var temp = listaFotosSalvas[i];
        listaFotos.push(
            {
                "ID": 0,
                "Result_Level3_Id": 0,
                "Photo_Thumbnaills": "",
                "Photo": temp.photo,
                "Latitude": temp.latitude,
                "Longitude": temp.longitude,
                "Level1Id": temp.level1id,
                "Level2Id": temp.level2id,
                "Level3Id": temp.level3id,
                "UnitId": temp.unitid,
                "Period": temp.period,
                "Shift": temp.shift,
                "Evaluation": temp.evaluation,
                "Sample": temp.sample,
                "ResultDate": temp.date
            }
        );
    }
    
    lastFotoEnviada = temp.photo;
    _readFile(temp.photo, function (data) {
        if(typeof(data) != 'undefined' && data.length > 0){
            listaFotos[0]["Photo"] = data;
            enviaFotos(listaFotos);
        }else{
            level3Photos.splice(0, data.count);
            _writeFile("level3Photos.json", level3Photos);
            algumaFotoEstaSendoEnviada = false;
            sendResultLevel3Photo();
        }
    });
}

function salvaFoto(level3PhotoTemp) {
    var canvasPhoto = document.createElement("canvas");
    var contextPhoto = canvasPhoto.getContext('2d');

    var img = new Image();
    img.src = level3PhotoTemp.path;

    level3Photos.push(level3PhotoTemp);
    _writeFile("level3Photos.json", level3Photos);

    $('.level3[id=' + level3PhotoTemp.level3id + ']').find('.camera-button').removeClass('btn-danger').addClass('btn-default');
}

var lastFotoEnviada = "";

function enviaFotos(listaFotos) {
    $.ajax({
        data: JSON.stringify(listaFotos),
        contentType: "application/json; charset=utf-8",
        url: urlPreffix + '/api/ResultLevel3PhotosApi',
        type: 'POST',
        headers: token(),
        success: function (data) {
            algumaFotoEstaSendoEnviada = false;
            _writeFile(lastFotoEnviada, '');
            level3Photos.splice(0, data.count);
            _writeFile("level3Photos.json", level3Photos);
            sendResultLevel3Photo();
        },
        error: function (e) {
            //alert(JSON.stringify(e));
            setTimeout(function () {
                sendResultLevel3Photo();
            }, 20000);
            algumaFotoEstaSendoEnviada = false;
        }
    });
}

function resetaBotaoCamera() {
    $('.camera-button').removeClass('btn-default').addClass('btn-danger');
}

function ativaFotosSalvasRelacionadasAColeta() {
    var listaFotosAtualizadas = []
    level3Photos.forEach(function (o, i) {
        o.isactive = true;
        listaFotosAtualizadas.push(o)
    });
    level3Photos = listaFotosAtualizadas;
}

function removeFotosNaoSalvas() {
    resetaBotaoCamera();
    var listaFotosAtualizadas = []
    level3Photos.forEach(function (o, i) {
        //Adiciona fotos que estão ativas, ou seja, foram salvas
        if (o.isactive == true)
            listaFotosAtualizadas.push(o);
    });
    level3Photos = listaFotosAtualizadas;
}