
//$('.level02:visible').parents('.row').find('.btnAreaSaveConfirm').click();

function MakeObject(JquerySelector, ListName) {
    var objectReturn = {};
    objectReturn[ListName] = [];
    var elemens = $(JquerySelector);
    $.each(elemens, function (counter, object) {
        var temp = {};
        var el = object;
        for (var i = 0, atts = el.attributes, n = atts.length, arr = []; i < n; i++) {
            var name = atts[i].nodeName;
            var value = atts[i].nodeValue;
            temp[atts[i].nodeName] = atts[i].nodeValue
        }
        objectReturn[ListName].push(temp);
    });
    return objectReturn
}

var ObjListaLevel1 = {};

function MakeResult() {

    ObjListaLevel1 = MakeObject('.level01Result', 'level1');

    //Para cada objeto da lista de level 2, pegua os objetos referentes a este do level 2.
    $.each(ObjListaLevel1.level1, function (counter, object) {

        var seletorLevel1 = '.level01Result[shift=' + object['shift'] + '][period=' + object['period'] + '][reauditnumber=' + object['reauditnumber'] + ']';
        var seletorLevel2 = '.level02Result[level01id=' + object.level01id + ']';

        var objListaLevel2 = MakeObject(seletorLevel1 + " " + seletorLevel2, 'list02');
        jQuery.extend(object, objListaLevel2);

        //Para cada objeto da lista de level 2, pegua os objetos referentes a este do level 3.
        $.each(object.list02, function (counter2, object2) {
            var objListaLevel3 = MakeObject(seletorLevel1 + " " + seletorLevel2 + '[level02id = ' + object2["level02id"] + '] .level03Result', 'list03');
            jQuery.extend(object2, objListaLevel3);
        });
    });
    console.log('Done.');
    console.log(ObjListaLevel1);
}

function ConsildateCarcass() {
    var enviar = CreateCCAObject();
    $.post("http://192.168.25.200/SgqMaster/api/Sync/SetDataAuditConsolidated", ObjListaLevel1,
    function (r) {//Callback.
        console.log(r);
    });
}