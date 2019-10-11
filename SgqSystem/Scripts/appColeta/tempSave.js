var fileTempName = 'temp_save.txt';
var tempLevel2 = [];

function getTempFileLevel2(){
    _readFile(fileTempName, function(r){
        if(r){
            tempLevel2 = JSON.parse(r);            
        }
    });
}

function setTempFileLevel2(){    
    _writeFile(fileTempName, JSON.stringify(tempLevel2));
}

function getTempLevel2(level1id, level2id){

    var period = parseInt($('.App').attr('period'));
    var shift = parseInt($('.App').attr('shift'));
    var unitid = parseInt($('.App').attr('unidadeid'));
    var date = getCollectionDate();
    var reauditnumber = getReauditTemp();

    var level2 = getLastTemp(level1id, level2id, reauditnumber, date);

    if(level2.length > 0){
        $('.level3Group[level1id=' + level1id + '][level2id=' + level2id + ']').empty();
        $('.level3Group[level1id=' + level1id + '][level2id=' + level2id + ']').append(level2[0].level3Group);
        
        $('.level3Group[level1id=' + level1id + '][level2id=' + level2id + '] input, '+
            '.level3Group[level1id=' + level1id + '][level2id=' + level2id + '] select').each(function(i, o){
            $(o).val($(o).attr('tempVal'));
        });
    }

}

function setTempLevel2(level1id, level2id, reaudit, evaluation, sample){

    var period = parseInt($('.App').attr('period'));
    var shift = parseInt($('.App').attr('shift'));
    var unitid = parseInt($('.App').attr('unidadeid'));
    var date = getCollectionDate();
    var reauditnumber = getReauditTemp();
    var defeitos = parseInt($('.painelLevel03 .defects:visible').text());

    if(defeitos == undefined || isNaN(defeitos))
        defeitos = 0;
        
    var level2 = $.grep(tempLevel2, function(o) {
        if(o.level1id == level1id && 
            o.level2id == level2id && 
            o.shift == shift && 
            o.period == period && 
            o.unitid == unitid &&
            o.reauditnumber == reauditnumber &&
            o.evaluation == evaluation &&
            o.sample == sample &&
            o.date == date){
            return o;
        }
    });

    $('.level3Group[level1id=' + level1id + '][level2id=' + level2id + '] input, '+
        '.level3Group[level1id=' + level1id + '][level2id=' + level2id + '] select').each(function(i, o){
        $(o).attr('tempVal', $(o).val())
    });

    var level3Group = $('.level3Group[level1id=' + level1id + '][level2id=' + level2id + ']').html();

    if(level2.length > 0){
        level2[0].level3Group = level3Group;
        level2[0].defeitos = defeitos;
    }
    else
    {
        tempLevel2.push({
            level1id : level1id,
            level2id : level2id,
            shift : shift,
            period : period,
            unitid : unitid,
            evaluation : evaluation,
            sample : sample,
            reauditnumber : reauditnumber,
            date : date,
            level3Group : level3Group,
            defeitos : defeitos
        });
    }

    if(defeitos > 0){
        $('li[id='+level2id+'] .counters .defectstotal').css({'color':'red'}).text(defeitos);
        //$('li[id='+level2id+']').addClass('bgLimitExceeded');
        
    }
    setTempFileLevel2();
}

function removeTempLevel2(obj){
    $.grep(tempLevel2, function(o, i) {
        if(o == obj){            
            tempLevel2.splice(i, 1);
            return;
        }
    });
}

function getLastTemp(level1id, level2id){
    var period = parseInt($('.App').attr('period'));
    var shift = parseInt($('.App').attr('shift'));
    var unitid = parseInt($('.App').attr('unidadeid'));
    var date = getCollectionDate();
    var reauditnumber = getReauditTemp();

    var level2list = $.grep(tempLevel2, function(o) {
        if(o.level1id == level1id && 
            o.level2id == level2id && 
            o.shift == shift && 
            o.period == period && 
            o.unitid == unitid &&
            o.reauditnumber == reauditnumber &&
            o.date == date){
            return o;
        }
    });

    var maxEvaluation = 0;
    var maxSample = 0;

    level2list.map(function(obj){     
        if (obj.evaluation > maxEvaluation) {
            maxEvaluation = obj.evaluation;    
        }
    });

    level2list = $.grep(level2list, function(o) {
        if(o.evaluation == maxEvaluation){
            return o;
        }
    });

    level2list.map(function(obj){     
        if (obj.sample > maxSample) {
            maxSample = obj.sample;    
        }
    });

    level2list = $.grep(level2list, function(o) {
        if(o.sample == maxSample){
            return o;
        }
    });

    return level2list;

}

function hasTempLevel2(level1id, level2id){

    var period = parseInt($('.App').attr('period'));
    var shift = parseInt($('.App').attr('shift'));
    var unitid = parseInt($('.App').attr('unidadeid'));
    var date = getCollectionDate();
    var reauditnumber = getReauditTemp();

    var level2list = $.grep(tempLevel2, function(o) {
        if(o.level1id == level1id && 
            o.level2id == level2id && 
            o.shift == shift && 
            o.period == period && 
            o.unitid == unitid &&
            o.reauditnumber == reauditnumber &&
            o.date == date){
            return o;
        }
    });

    return level2list.length > 0;

}

function hasTempLevel1(level1id){

    var period = parseInt($('.App').attr('period'));
    var shift = parseInt($('.App').attr('shift'));
    var unitid = parseInt($('.App').attr('unidadeid'));
    var date = getCollectionDate();
    var reauditnumber = getReauditTemp();

    var level2list = $.grep(tempLevel2, function(o) {
        if(o.level1id == level1id && 
            o.shift == shift && 
            o.period == period && 
            o.unitid == unitid &&
            o.reauditnumber == reauditnumber &&
            o.date == date){
            return o;
        }
    });

    return level2list.length > 0;

}

$(document).on('click', '#btnSaveTemp', function (e) {

    var level1id = parseInt($('.level1.selected').attr('id'));
    var level2id = parseInt($('.level2.selected').attr('id'));
    var level2 = $('.level2.selected');
    var reauditnumber = getReauditTemp();
    var evaluation = level2.attr('evaluatecurrent') != undefined ? parseInt(level2.attr('evaluatecurrent')) : 1;
    evaluation = parseInt(evaluation);
    var sample = parseInt($('.level3Group[level1id=' + level1id + '][level2id=' + level2id + '] .sampleCurrent').text());

    setTempLevel2(level1id, level2id, reauditnumber, evaluation, sample);

    openLevel2($('.level1.selected'));

});

var ultL2Temp =false;
$(document).on('click', '#btnSaveAllTemp', function (e) {

    if ($(this).is(':disabled')) {
        return false;
    }

    if (validHeader()) {

        var period = parseInt($('.App').attr('period'));
        var shift = parseInt($('.App').attr('shift'));
        var unitid = parseInt($('.App').attr('unidadeid'));
        var level1id = parseInt($('.level1.selected').attr('id'));
        var reauditnumber = getReauditTemp();
        var date = getCollectionDate();

        var _tempLevel2 = $.grep(tempLevel2, function(o) {
            if(o.level1id == level1id && 
                o.shift == shift && 
                o.period == period && 
                o.unitid == unitid &&
                o.reauditnumber == reauditnumber &&
                o.date == date){
                return o;
            }
        });
        ultL2Temp =false;
        _tempLevel2.forEach(function(o, i){ 

            $('.level2').removeClass('selected');
            $('.level2Group[level01id='+o.level1id+'] .level2[id='+o.level2id+']').addClass('selected');        
            _level2 = $('.level2.selected')[0];

            $('.level3Group[level1id=' + o.level1id + '][level2id=' + o.level2id + ']').empty();
            $('.level3Group[level1id=' + o.level1id + '][level2id=' + o.level2id + ']').append(o.level3Group);

            $('.level3Group[level1id=' + o.level1id + '][level2id=' + o.level2id + '] input, '+
            '.level3Group[level1id=' + o.level1id + '][level2id=' + o.level2id + '] select').each(function(i2, o2){
                $(o2).val($(o2).attr('tempVal'));
            });
            if(i == _tempLevel2.length-1)
               ultL2Temp =true; 

            saveResultLevel3();

            $('.level2').removeClass('selected');

            removeTempLevel2(o);

        });    
  
        setTempFileLevel2();

        $('#btnSaveAllTemp').addClass('hide');

        updateTempSave($('.level1.selected'));
    }

    setTimeout(function(){
        createFileResult();
    },100);
        
});

function updateTempSave(level1){
    if(hasTempLevel1(parseInt(level1.attr('id')))) {
        $('#btnSaveAllTemp').removeClass('hide');
    }else{
        $('#btnSaveAllTemp').addClass('hide');
    }

    $('#btnSaveTemp').addClass('hide');

    $('.level2Group[level01id=' + level1.attr('id') + '] .level2').each(function(index, self){
        var reauditnumber = getReauditTemp();

        if(!$(self).attr('completed')){
            if(hasTempLevel2(parseInt(level1.attr('id')), $(self).attr('id'), reauditnumber)){
                $(self).parents('li').addClass('bgCompleted');
                $(self).parent().find('.btnNotAvaliableLevel2').attr('disabled', 'disabled');
                $(self).parent().find('.btnAreaSave').attr('disabled', 'disabled');
            }
            else{
                $(self).parents('li').removeClass('bgCompleted');
                $(self).parent().find('.btnNotAvaliableLevel2').removeAttr('disabled');
                $(self).parent().find('.btnAreaSave').removeAttr('disabled');
            }
        }
        
    });
}

function getReauditTemp(){

    var reaudnumber = 0;
    var reaudMax = $('.Resultlevel2[level1id=' + $('.level1.selected').attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][unitid=' +
        $('.App').attr('unidadeid') + ']:last').attr('reauditnumber')
        
    if (reaudMax > 0)
        reaudnumber = reaudMax;

    if (reaudnumber == 0 && ($('.level1.selected').attr('isreaudit') == 'true' || $('.level1.selected').attr('reaudminlevel') > 0 )){
        reaudnumber = 1;
        reaudMax = 1;
    }

    if($('.level1.selected').attr('reauditnumber') > reaudMax)
        reaudnumber = parseInt($('.level1.selected').attr('reauditnumber'));

    return reaudnumber;
}

function getReauditTempPeriodo(periodo){

    var reaudnumber = 0;
    var reaudMax = $('.Resultlevel2[level1id=' + $('.level1.selected').attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + periodo + '][unitid=' +
        $('.App').attr('unidadeid') + ']:last').attr('reauditnumber')
        
    if (reaudMax > 0)
        reaudnumber = reaudMax;

    if (reaudnumber == 0 && ($('.level1.selected').attr('isreaudit') == 'true' || $('.level1.selected').attr('reaudminlevel') > 0 )){
        reaudnumber = 1;
        reaudMax = 1;
    }

    if($('.level1.selected').attr('reauditnumber') > reaudMax)
        reaudnumber = parseInt($('.level1.selected').attr('reauditnumber'));

    return reaudnumber;
}