var urlServidor = "http://192.168.25.200/SgqMaster";
var urlLocal = "http://localhost:63128"
var urlPreffix = window.location.host.indexOf("host") > 0 ? urlServidor : urlServidor;

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
                temp[name] = value;
        }
        objectReturn[ListName].push(temp);
        if (!!object.childNodes) {
            if (object.childNodes.length > 0)
                MakeObject2(object.childNodes, 'next' + ListName, temp);
        }
    });
}

function MakeResult(Obj, selector) {
    MakeObject2(selector, 'Root', Obj);
    console.log('Done.');
    //console.log(Obj);
}

function Sync() {
    var ObjListaLevel1 = {};
    MakeResult(ObjListaLevel1, $('.level01Result'));
    $.each(ObjListaLevel1.Root, function (c, o) {
        var smallerObject = {};
        smallerObject['Root'] = [];
        if (!!o.correctiveactioncomplete) {
            var tempCA = {}
            MakeObject2($('.Results > .correctiveAction'), 'a', tempCA)
            $.each(tempCA.a, function (cc, oo) {
                if (oo.level01id == o.level01id)
                    o['correctiveactioncomplete'] = oo;
            });
        }
        smallerObject.Root.push(o);
        console.log(smallerObject)
        $.post(urlPreffix + "/api/Sync/SetDataAuditConsolidated", smallerObject, function (r) {
            console.log(r);
        });
    });

    //var ObjListaCa = {};
    //MakeResult(ObjListaCa, $('.Results > .correctiveAction'));


    var htmlObject = {};
    htmlObject['html'] = $('.Results').html();
    $.post(urlPreffix + "/api/Sync/SaveHtml", htmlObject, function (r) {
        console.log(r);
    });
}
function GetSync() {
    $.post(urlPreffix + "/api/Sync/GetLastEntry", {}, function (r) {
        console.log(r);
    });
}

function GetToSync() {
    $.post(urlPreffix + "/api/Sync/GetHtmlLastEntry", {}, function (r) {
        console.log(r);
    });
}