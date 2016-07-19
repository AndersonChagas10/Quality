var d = new Date();
var storage = window.localStorage;
var date = d.getDate()  + "/" + (d.getMonth()+1) + "/" + d.getFullYear();

if(storage.getItem("dateCurrent") != date){  
    window.localStorage.clear();
    storage.setItem("dateCurrent", date);
}

function openIndicator(indicatorId){
    var storage = window.localStorage;
    storage.setItem("indicatorId", indicatorId);
    storage.setItem("indicatorName", getIndicator(indicatorId).Name);
    storage.setItem("indicatorImage", getIndicator(indicatorId).Image);
    $(location).attr('href', '../view/monitorings.html');
}

function showIndicators(indicators) {
    var out = "";
    var i;
    for(i = 0; i<indicators.length; i++) {
        out += lineIndicator(indicators[i]);
    }
    $( ".indicators" ).append(out);
}

function getIndicator(Id){
    for(var i = 0; i < indicators.length; i++){
        if(indicators[i].Id == Id){
            return indicators[i];
        }
    }
}

