function getCollectionPhase() {

    if(connectionServer && $('.App').attr('unidadeid') != undefined){
        var date = getCollectionDate();
        if (date == undefined) {
            date = dateTimeFormat();
        }
        var ParCompany_Id = parseInt($('.App').attr('unidadeid'));

        $.ajax({
            url: urlPreffix + '/api/SyncServiceApi/getPhaseLevel2?ParCompany_Id=' + ParCompany_Id + '&date=' + date,
            headers: token(),
            type: 'POST',
            success: function (data) {

                $('.ResultsPhase').empty();
                appendDevice($(data), $('.ResultsPhase'));
                _writeFile('ResultsPhase.txt', $(data));

            },
            timeout: 600000,
            error: function () {

            }
        });
    }else{        
        _readFile('ResultsPhase.txt', function(data){
            $('.ResultsPhase').empty();
            appendDevice(data, $('.ResultsPhase'));
        });
    }
}

function getPhaseLevel2(level1, level2){
    
    var phaseAtual = $('.PhaseResultlevel2[parlevel1_id='+$(level1).attr('id')+'][parlevel2_id='+$(level2).attr('id')+'][shift='+$('.App').attr('shift')+']');
    if(phaseAtual.length > 0){
        var frequency = parseInt(level1.attr('phase'+phaseAtual.attr('phase')).split(';')[0]);
        var effective = parseInt(level1.attr('phase'+phaseAtual.attr('phase')).split(';')[1]);
        //var date = new Date(phaseAtual.attr('collectiondate').insertAt(4, '-').insertAt(2, '-'));
        var date = new Date(insertAt(insertAt(phaseAtual.attr('collectiondate'), 4, '-'), 2, '-'));
        var periodos = parseInt(phaseAtual.attr('period'));
        var turnos = parseInt(phaseAtual.attr('shift'));
        var periodosPassados = $(phaseAtual).children('.countPeriod').length;
        var turnosPassados = parseInt(phaseAtual.attr('countshift'));

        if(isInsideFrequency(date, frequency, effective, periodos, turnos, periodosPassados, turnosPassados)){
            $(level2).attr('phase', phaseAtual.attr('phase'));
            return " ("+getResource('phase')+" "+phaseAtual.attr('phase')+")";
        }else{
            $(level2).attr('phase', 0);
        }
    }
    return "";
}

function getPhaseNumber(){
    var level1 = $('.level1.selected');
    var level2 = $('.level2.selected');

    var phaseAtual = $('.PhaseResultlevel2[parlevel1_id='+$(level1).attr('id')+'][parlevel2_id='+$(level2).attr('id')+'][shift='+$('.App').attr('shift')+']:first');

    if(phaseAtual.length > 0 && level1.attr('phase'+phaseAtual.attr('phase')) != undefined){
        var frequency = parseInt(level1.attr('phase'+phaseAtual.attr('phase')).split(';')[0]);
        var effective = parseInt(level1.attr('phase'+phaseAtual.attr('phase')).split(';')[1]);
        //var date = new Date(phaseAtual.attr('collectiondate').insertAt(4, '-').insertAt(2, '-'));
        var date = new Date(insertAt(insertAt(phaseAtual.attr('collectiondate'), 4, '-'), 2, '-'));
        var periodos = parseInt(phaseAtual.attr('period'));
        var turnos = parseInt(phaseAtual.attr('shift'));
        var periodosPassados = $(phaseAtual).children('.countPeriod').length;
        var turnosPassados = parseInt(phaseAtual.attr('countshift'));

        if(isInsideFrequency(date, frequency, effective, periodos, turnos, periodosPassados, turnosPassados)){
            return parseInt(phaseAtual.attr('phase'));
        }else{
            return 0;
        }        
    }else{
        return 0;
    }
}

function getAlertNumber(){
    var level1 = $('.level1.selected');
    var level2 = $('.level2.selected');

    var phaseAtual = $('.PhaseResultlevel2[parlevel1_id='+$(level1).attr('id')+'][parlevel2_id='+$(level2).attr('id')+'][shift='+$('.App').attr('shift')+']');
    if(phaseAtual.length > 0){
        return parseInt(phaseAtual.attr('evaluationnumber'));
    }else{
        return 0;
    }
}

function setPhase(ParLevel1_Id, ParLevel2_Id, CollectionDate, EvaluationNumber, Phase, Period, Shift){
    var phaseAtual = $('.PhaseResultlevel2[parlevel1_id='+ParLevel1_Id+'][parlevel2_id='+ParLevel2_Id+'][shift='+$('.App').attr('shift')+']');

    if(phaseAtual.length == 0){
        phaseAtual = $('<div class="PhaseResultlevel2" parlevel1_id='+ParLevel1_Id+' parlevel2_id='+ParLevel2_Id+' countperiod="0" countshift="0"></div>');
        appendDevice(phaseAtual, $('.ResultsPhase'));
    }else{
        if(Phase != parseInt(phaseAtual.attr('phase'))){
            phaseAtual.empty();
        }
    }

    phaseAtual.attr('collectiondate', CollectionDate);
    phaseAtual.attr('evaluationnumber', EvaluationNumber);
    phaseAtual.attr('period', Period);
    phaseAtual.attr('phase', Phase);
    phaseAtual.attr('shift', Shift);
    phaseAtual.attr('countperiod', 0);
    phaseAtual.attr('countshift', 0);
    
    _writeFile('ResultsPhase.txt', $('.ResultsPhase').html());
}

function checkPhase(level1_id, level2_id, shift, period, phase){
    if(phase){
        var phasePeriods = $('.PhaseResultlevel2[parlevel1_id=' + level1_id + '][parlevel2_id=' + level2_id + '][shift='+$('.App').attr('shift')+']').children('.countPeriod[date='+getCollectionDate()+'][period='+$('.App').attr('period')+']');

        if(phasePeriods.length == 0){
            appendDevice($("<div class='countPeriod' period='"+ period + "' date='"+getCollectionDate()+"'></div>"), 
            $('.PhaseResultlevel2[parlevel1_id=' + level1_id + '][parlevel2_id=' + level2_id + '][shift='+$('.App').attr('shift')+']'));
        }
        
        _writeFile('ResultsPhase.txt', $('.ResultsPhase').html());
    }
    
}

function setStartPhase(result){
    var startPhase = $('.level2.selected').attr('phase') == undefined ? 0 : $('.level2.selected').attr('phase');
    $(result).attr('startphaseevaluation', startPhase);
}

function setEndPhase(result){
    getPhaseLevel2($('.level1.selected'), $('.level2.selected'));
    var endPhase = $('.level2.selected').attr('phase') == undefined ? 0 : $('.level2.selected').attr('phase');
    $(result).attr('endphaseevaluation', endPhase);
}