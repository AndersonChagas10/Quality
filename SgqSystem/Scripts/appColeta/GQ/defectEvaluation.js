function getDefectEvaluationOnline(parLevel1_Id) {

    if(connectionServer){
        var date = getCollectionDate();
        if (date == undefined) {
            date = dateTimeFormat();
        }
        var parCompany_Id = parseInt($('.App').attr('unidadeid'));
        
        if(isNaN(parCompany_Id))
            parCompany_Id = userlogado.attr('unidadeid');

        var request = $.ajax({ 
            data: {
                "parCompany_Id": parCompany_Id,
                "date": date,
                "parLevel1_Id": parLevel1_Id
            },
            url: urlPreffix + '/Services/SyncServices.asmx/getResultEvaluationDefects',
            type: 'POST',
            success: function (data) {

                $('.ResultsDefectsEvaluation').empty();
                appendDevice($(data).text(), $('.ResultsDefectsEvaluation'));
                _writeFile('ResultsDefectsEvaluation.txt', $(data).text());

            },
            timeout: 600000,
            error: function () {

            }
        });
    }else{        
        _readFile('ResultsDefectsEvaluation.txt', function(data){
            $('ResultsDefectsEvaluation').empty();
            appendDevice(data, $('.ResultsDefectsEvaluation'));
        });
    }
}

function setDefectsByEvaluation(ParLevel1_Id, CollectionDate, EvaluationNumber, Sample, Period, Shift, Defects){
    var evaluationAtual = $('.EvaluationDefects[parlevel1_id='+ParLevel1_Id+'][evaluationnumber='+EvaluationNumber+'][sample='+Sample+'][period='+Period+'][shift='+Shift+'][date='+CollectionDate+']');

    if(evaluationAtual.length == 0){
        evaluationAtual = $('<div date="'+CollectionDate+'" evaluationnumber="'+EvaluationNumber+'" sample="'+Sample+'" period="'+Period+'" shift="'+Shift+'" parlevel1_id="'+ParLevel1_Id+'" class="EvaluationDefects"></div>');
        appendDevice(evaluationAtual, $('.ResultsDefectsEvaluation'));
    }else{
        Defects += parseInt(evaluationAtual.attr('defects'));
    }

    evaluationAtual.attr('Defects', Defects);
    
    _writeFile('ResultsDefectsEvaluation.txt', $('.ResultsDefectsEvaluation').html());
}

function getValidCFF(){
    var list = $('.EvaluationDefects[parlevel1_id.includes("9878955")][period='+$('.App').attr('period')+'][shift='+$('.App').attr('shift')+'][date='+getCollectionDate()+']');

    var valid = true;
    var countEvaDef = 0;
    list.each(function(index, self){
        if(parseInt($(self).attr('defects')) > 0){
            countEvaDef = countEvaDef + 1;
        }
        if(parseInt($(self).attr('defects')) > 3){
            valid = false;
        }
    });

    if(countEvaDef >= 6){
        valid = false;
    }

    return valid;
}