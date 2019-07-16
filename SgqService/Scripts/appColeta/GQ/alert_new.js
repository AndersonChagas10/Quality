var cacheGlobalAlerta = {
    reauditNumber: 0,
    resultLevel2: {}
};

function emitirAlerta(parLevel1, period, shift) {

    var DefeitosLevel1 = {};
    var obj;
    var reauditNumber = 0;
    var ultRNumber = $('.Resultlevel2[level1id='+parLevel1+'][shift='+$('.App').attr('shift')+'][period='+$('.App').attr('period')+']:last').attr('reauditnumber');
    //var ultRNumber = cacheGlobalAlerta.reauditNumber;

    if(ultRNumber > 0)
        reauditNumber = ultRNumber;

    var result = $('.Resultlevel2[level1id=' + parLevel1 + '][period=' + period + '][shift=' + shift + '][reauditnumber=' + reauditNumber + ']');
    MakeObject2(result, 'DefeitosLevel2', DefeitosLevel1);

    //DefeitosLevel1 = cacheGlobalAlerta.resultLevel2;

    DefeitosLevel1['defectsl2'] = 0;
    DefeitosLevel1['defectsresultl2'] = 0;
    DefeitosLevel1['totalavaliado'] = 0;
    DefeitosLevel1['totallevel3evaluationl2'] = 0;
    DefeitosLevel1['totallevel3withdefectsl2'] = 0;
    DefeitosLevel1['weidefectsl2'] = 0;
    DefeitosLevel1['weievaluationl2'] = 0;

    /*Gambiarra para o CT6 */
    DefeitosLevel1.DefeitosLevel2.forEach(function (o, i) {

        DefeitosLevel1['defectsl2'] += o.defectsl2 == undefined ? 0 : parseFloat(o.defectsl2.replace(',', '.'));
        DefeitosLevel1['defectsresultl2'] += o.defectsresultl2 == undefined ? 0 : parseFloat(o.defectsresultl2.replace(',', '.'));
        DefeitosLevel1['totalavaliado'] += o.totalavaliado == undefined ? 0 : parseFloat(o.totalavaliado.replace(',', '.'));
        DefeitosLevel1['totallevel3evaluationl2'] += o.totallevel3evaluationl2 == undefined ? 0 : parseFloat(o.totallevel3evaluationl2.replace(',', '.'));
        DefeitosLevel1['totallevel3withdefectsl2'] += o.totallevel3withdefectsl2 == undefined ? 0 : parseFloat(o.totallevel3withdefectsl2.replace(',', '.'));
        DefeitosLevel1['weidefectsl2'] += o.totallevel3withdefectsl2 == undefined ? 0 : parseFloat(o.weidefectsl2.replace(',', '.'));
        DefeitosLevel1['weievaluationl2'] += o.weievaluationl2 == undefined ? 0 : parseFloat(o.weievaluationl2.replace(',', '.'));

    });

    return DefeitosLevel1;
}

function compararObjetos(obj1, obj2) {
    var r = {};

    Object.keys(obj2).forEach(function (o) {
        if (obj1[o] == undefined) {
            r[o] = 'undefined';
        }
        else if (obj2[o] != obj1[o]) {
            r[o] = 'diff: ' + obj1[o] + ' ' + obj2[o];
        }
    });

    return r;
}

