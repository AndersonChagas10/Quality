function MakeObject2(element, ListName, objectReturn) {
    objectReturn[ListName] = [];
    var elemens = element;
    $.each(elemens, function (counter, object) {
        var temp = {};
        var el = object;
        for (var i = 0, atts = el.attributes, n = atts.length, arr = []; i < n; i++) {
            var name = atts[i].nodeName;
            var value = atts[i].nodeValue;
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
    console.log(Obj);
}

function Sync() {
    var ObjListaLevel1 = {};
    MakeResult(ObjListaLevel1)
    //window.location.host + window.location.pathname // COLOCAR NO ROOT
    var url = window.location.host 
    //$.post("http://192.168.25.200/SgqMaster/api/Sync/SetDataAuditConsolidated", ObjListaLevel1, function (r) { console.log(r); });
    $.post("http://localhost:63128/api/Sync/SetDataAuditConsolidated", ObjListaLevel1, function (r) { console.log(r); });
}

