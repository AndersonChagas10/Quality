var counterLevel1 = [];

function addCounterLevel1(parLevel1, parLevel2, total, evaluated, shift, period){

    var obj = $.grep(counterLevel1, function(o) {
        if(o.parlevel1 == parLevel1 && o.parlevel2 == parLevel2){
            return o;
        }
    });

    if(!obj[0]){
        counterLevel1.push({
            parlevel1: parLevel1,
            parlevel2: parLevel2,
            shift: shift,
            period: period,
            total: total,
            evaluated: evaluated    
        });
    }else{
        var i = counterLevel1.indexOf(obj[0]);
        counterLevel1[i] = {
                                parlevel1: parLevel1,
                                parlevel2: parLevel2,
                                shift: shift,
                                period: period,
                                total: total,
                                evaluated: evaluated    
                            };
    }

}

function emptyEvaluatedCounterLevel1(){
    $.grep(counterLevel1, function(o, i) {
        counterLevel1[i].evaluated = 0;
    });
}

function setEvaluatedCounterLevel1(parLevel1, parLevel2, evaluated, shift, period){
    
    $.grep(counterLevel1, function(o, i) {
        if(o.parlevel1 == parLevel1 && o.parlevel2 == parLevel2 && o.shift == shift && o.period == period){
            counterLevel1[i].evaluated = evaluated;
        }
    });

}

function setTotalCounterLevel1(parLevel1, parLevel2, total, shift, period){
    
    var obj = $.grep(counterLevel1, function(o) {
        if(o.parlevel1 == parLevel1 && o.shift == shift && o.period == period && o.parlevel2 == parLevel2){
            return o;
        }
    });

    if(!obj[0]){
        counterLevel1.push({
            parlevel1: parLevel1,
            parlevel2: parLevel2,
            shift: shift,
            period: period,
            total: total,
            evaluated: 0    
        });
    }else{
        var i = counterLevel1.indexOf(obj[0]);
        counterLevel1[i] = {
                                parlevel1: parLevel1,
                                parlevel2: parLevel2,
                                shift: shift,
                                period: period,
                                total: total,
                                evaluated: obj[0].evaluated    
                            };
    }

}

function getCompletedLevel1(parLevel1, shift, period,unidade){

    var level1 = $.grep(counterLevel1, function(o) {
        if(o.parlevel1 == parLevel1 && o.shift == shift && o.period == period){
            return o;
        }
    });

    var list = $.grep(counterLevel1, function(o) {
        if(o.parlevel1 == parLevel1 && o.shift == shift && o.period == period && o.evaluated >= o.total && o.total != 0){
            return o;
        }
    });

    if(level1.length == 0)
        return false;
        
    if(list.length == level1.length)
        return true;
    else
        return false;
}