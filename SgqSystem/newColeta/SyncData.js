var urlServidor = "http://192.168.25.200/SgqMaster";
var urlLocal = "http://localhost:63128"
var urlPreffix = window.location.host.indexOf("host") > 0 ? urlLocal : urlServidor;

function MakeObject2(element, ListName, objectReturn) {
    objectReturn[ListName] = [];
    var elemens = element;
    $.each(elemens, function (counter, object) {
        var temp = {};
        var el = object;
        for (var i = 0, atts = el.attributes, n = atts.length, arr = []; i < n; i++) {
            var name = atts[i].nodeName;
            var value = atts[i].nodeValue;
            if (value == 'undefined' || value == 'null') { continue; }
            temp[atts[i].nodeName] = atts[i].nodeValue
        }
        objectReturn[ListName].push(temp);
        if (!!object.childNodes) {
            MakeObject2(object.childNodes, 'next' + ListName, temp);
        }
    });
}

function MakeResult(Obj) {
    MakeObject2($('.level01Result'), 'Root', Obj);
    console.log('Done.');
    //console.log(Obj);
}

function Sync() {
    var ObjListaLevel1 = {};
    MakeResult(ObjListaLevel1)
    $.each(ObjListaLevel1.Root, function (c, o) {
        var smallerObject = {};
        smallerObject['Root'] = [];
        smallerObject.Root.push(o);
        console.log(smallerObject)
        $.post(urlPreffix + "/api/Sync/SetDataAuditConsolidated", smallerObject, function (r) {
            console.log(r);
        });
    });
}

function GetSync() {
    $.post(urlPreffix + "/api/Sync/GetLastEntry", {}, function (r) {
        console.log(r);
    });
}
