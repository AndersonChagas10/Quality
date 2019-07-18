var listHeaders = [];
var listHeadersTemp = [];
var headerResultList = [];

function validHeader() {
    var valid = true;
    $('.header:visible[required]').removeClass("has-warning");
    $('.header:visible[required], .header:visible:has([linknumberevaluetion=true])').each(function (index, self) {
        var value = $(self).children("input").val();

        if (value == undefined)
            value = parseInt($(self).children("select").val());

        if($(self).children("select[checkbox=true]").length)
            value = $(self).children("select").find('option.selected').length;

        if (!value) {
            $(self).addClass("has-warning");
            openMessageModal(getResource("warning"), getResource("fill_header_fields"));
            valid = false;
            return false;
        }
    });
    return valid;
}

function loadHeaders() {
    showNoParents();
}

$(document).on('change', '.header:visible select', function(){

    var id = parseInt($(this).attr('parheaderfield_id'));
    var selects = [];
    var that = $(this);

    var optionSelected = that.children('option:selected');

    that.children('option[selected]').removeAttr('selected');

    var panelLevel3;
    if($('.level2.selected').length > 0) {
        panelLevel3 = $('.level3Group[level1id='+$('.level1.selected').attr('id')+'][level2id='+$('.level2.selected').attr('id')+'] .painelLevel03').clone();
    } else {
        panelLevel3 = $('.painelLevel03:visible').clone();
    }

    if(hasMultipleList(id)){
        var value = parseInt($(this).val());
        var hashkey = optionSelected.attr('hashkey');
        var parheaderfield_id = parseInt($(this).attr('parheaderfield_id'));

        panelLevel3.find('option[hashkey='+hashkey+']').attr('selected', 'selected');

        if(value > 0 && !!hashkey)
        {
            var headers = panelLevel3.find('.header');

            headers.each(function(index1, elem1){
                if(!$(elem1).parent().hasClass('hide')){
                    $.grep(listHeadersTemp, function(o) { return o.ParHeaderField_Id == $(elem1).children('select').attr('parheaderfield_id')})
                        .forEach(function(o, i){
                            if(!$(elem1).parent().hasClass('hide')){
                                if(parseInt(o.ParMultipleValues_Id) > 0 
                                && o.HashKey.split('-').length > hashkey.split('-').length) {
                                    if(hashkey.indexOf('-') < 0){
                                        if(hashkey == o.HashKey.substring(0, hashkey.length)){
                                            $(elem1).parent().addClass('hide');
                                        }
                                    } 
                                    else {
                                        $(elem1).parent().addClass('hide');
                                    }
                                }
                            }
                        });

                }
            });

            var elem = null;
            $.grep(listHeadersTemp, function(o, i){
                if(o.HashKey.indexOf(hashkey) == 0 && o.ParHeaderField_Id != parheaderfield_id){

                    if(elem == null || elem.attr('parheaderfield_id') != o.ParHeaderField_Id){
                        elem = panelLevel3.find('.header select[parheaderfield_id='+o.ParHeaderField_Id+']');
                    } 
                    
                    if((o.HashKey.split("-").length - 1) == (hashkey.split("-").length)){
                        if(selects.indexOf(o.ParHeaderField_Id) < 0){
                            $(elem).empty();
                            $(elem).append($('<option value="0">'+getResource('select')+'...'+'</option>'));
                            $(elem).parent().parent().removeClass('hide');
                            selects.push(o.ParHeaderField_Id);
                        }
                        $(elem).append(optionHtml(o));
                    }else if((o.HashKey.split("-").length - 1) > (hashkey.split("-").length)){
                        if(!$(elem).parent().parent().hasClass('hide')){
                            $(elem).empty();
                            $(elem).parent().parent().addClass('hide');
                        }                    
                    }
                } 
            });
        } else {
            var elem = null;
            hashkey = that.children('option:last:not(".hide")').attr('hashkey') ? that.children('option:last:not(".hide")').attr('hashkey') : "";
            $.grep(listHeadersTemp, function(o, i){
                if(o.ParHeaderField_Id != parheaderfield_id){

                    if(elem == null || elem.attr('parheaderfield_id') != o.ParHeaderField_Id){
                        elem = panelLevel3.find('.header select[parheaderfield_id='+o.ParHeaderField_Id+']');

                        var list = $.grep(listHeadersTemp, function(o) { return o.ParHeaderField_Id == $(elem).attr('parheaderfield_id')})

                        if(list.length > 0)
                            if((hashkey.split("-").length - 1) < ((list[0].HashKey ? list[0].HashKey : "").split("-").length -1)){
                                if(!$(elem).parent().parent().hasClass('hide')){
                                    $(elem).empty();
                                    $(elem).parent().parent().addClass('hide');
                                }                    
                            }
                    } 
                                        
                }
            });
        }
        
        var options = panelLevel3.find('.header .form-control[parfieldtype_id=1] option[value!=0]:selected');
        panelLevel3.find('.header .form-control[parfieldtype_id!=1]').parent().parent().addClass('hide');
        
        options.each(function(i1, o1){
            var headers = panelLevel3.find('.header');

            headers.each(function(i2, o2){
                var haskeys = $(o2).attr('hashkeys');
                if(!!haskeys){
                    var valid = false;
                    haskeys.split(';').forEach(function(element) {
                        if(element.indexOf($(o1).attr('hashkey')+"-i") == 0){
                            $(o2).parent().removeClass('hide');
                            $(o2).children('.form-control').val('');
                        }
                    }, this);
                } else if ($(o2).children('.form-control[parfieldtype_id!=1]').length > 0){
                    $(o2).parent().removeClass('hide');
                }  
                
            });
        });

    }

    if(that.children('option[selected]').length == 0 && parseInt(optionSelected.val()) > 0){
        panelLevel3.find('select[parheaderfield_id='+id+'] option[value='+optionSelected.val()+']').attr('selected', 'selected');
    }

    panelLevel3.find('.header .form-control[parfieldtype_id!=1]').each(function(index, elem){
        if(!$(elem).parent().attr('hashkeys') || $(elem).parent().attr('hashkeys').indexOf('i') == 0)
            $(elem).parent().parent().removeClass('hide');
    });

    if($('.level2.selected').length > 0) {
        $('.level3Group[level1id='+$('.level1.selected').attr('id')+'][level2id='+$('.level2.selected').attr('id')+'] .painelLevel03').replaceWith(panelLevel3);
    } else {
        $('.painelLevel03:visible').replaceWith(panelLevel3);
    }

    window.localStorage.setItem("cb-"+$('.level1.selected').attr('id')+"-"+this.id, this.value);

    setEvaluationByHeadersSelection();

});

function optionHtml(o){
    return $('<option value="'+o.ParMultipleValues_Id
        +'" punishmentvalue="'+o.PunishmentValue
        +'" parent_id="'+o.Parent_ParMultipleValues_Id
        +'" parcompany_id="'+o.ParCompany_Id
        +'" hashkey="'+o.HashKey
        +'">'+o.Name
        +'</option>');
}

function hasMultipleListParent(id){
    var v = $.hasElem(listHeadersTemp, function(o){
        return o.Parent_ParMultipleValues_Id == null && o.ParHeaderField_Id == id;
    });

    return v;
}

function hasMultipleList(id){
    var v = $.hasElem(listHeadersTemp, function(o){
        return o.ParHeaderField_Id == id;
    });

    return v;
}

$.extend({
    hasElem: function(elems, validateCb){
        var i;
        for( i=0 ; i < elems.length ; ++i ) {
            if( validateCb( elems[i], i ) )
                return true;
        }
        return false;
    }
});

function showNoParents(){

    $('.level3Group[level1id='+$('.level1.selected').attr('id')+'][level2id='+$('.level2.selected').attr('id')+'] .painelLevel03 .header .form-control[parfieldtype_id=1]').each(function(index, elem){
        var id = parseInt($(elem).attr('parheaderfield_id'));
        var cache = window.localStorage.getItem("cb-"+$('.level1.selected').attr('id')+"-"+id);
                
        if(hasMultipleListParent(id)) {
            $(elem).empty();
            $(elem).append($('<option value="0">'+getResource('select')+'...'+'</option>'));
            $.grep(listHeadersTemp, function(o, i){
                if(o.ParHeaderField_Id == id){
                    $(elem).append(optionHtml(o));
                }
            });
            $('select[id='+id+']:visible').replaceWith($(elem));
            if(!!cache){
                $(elem).val(cache).change();
            }
        }else{
            if(hasMultipleList(id)){
                if($(elem).children('option').length == 0){
                    $(elem).parent().parent().addClass('hide');
                }
                else if(!!cache){
                    $(elem).val(cache).change();
                }
            } else {
                $(elem).val($(elem).children('option[selected]').val());
            }
        }
    });

    $('.level3Group[level1id='+$('.level1.selected').attr('id')+'][level2id='+$('.level2.selected').attr('id')+'] .painelLevel03 .header .form-control[parfieldtype_id=2]')
    .each(function(index, elem){
        $(elem).val($(elem).children('option[selected]').val());
    });

    $('.level3Group[level1id='+$('.level1.selected').attr('id')+'][level2id='+$('.level2.selected').attr('id')+'] .painelLevel03 .header .form-control[parfieldtype_id!=1]').each(function(index, elem){
        if(!$(elem).parent().attr('hashkeys') || $(elem).parent().attr('hashkeys').indexOf('i') == 0)
            $(elem).parent().parent().removeClass('hide');
    });

}

function getHeaderResultList(){
    var unitid = parseInt($('.App').attr('unidadeid'));
    var date = getCollectionDate();

    if($('.level02Result[sync=false]').length == 0 && unitid != undefined) {
        $.ajax({
            url: urlPreffix+"/api/ParHeader/GetCollectionLevel2XHeaderField/"+unitid+"/"+date,
            contentType: 'application/json; charset=utf-8',
            headers: token(),
            dataType: 'json',
            type: 'GET',
            success: function (data) {
                headerResultList = data;
                saveHeaderResultList();
            },
            timeout: 600000,
            error: function (e) {
                _readFile("HeaderResultList.json", function(result){
                    if(result){
                        headerResultList = JSON.parse(result);
                    }
                });
            }
        });
    } else {
        _readFile("HeaderResultList.json", function(result){
            if(result){
                headerResultList = JSON.parse(result);
            }
        });
    }
}

function getListParMultipleValuesXParCompany(){
    listHeaders = [];
    var unitid = parseInt($('.App').attr('unidadeid'));

    $('.level1').each(function(index, element){
        if($('.level02Result[sync=false]').length == 0 && unitid != undefined) {
            $.ajax({
                url: urlPreffix+"/api/ParHeader/GetListParMultipleValuesXParCompany/"+unitid+"/"+$(element).attr('id'),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                headers: token(),
                type: 'GET',
                success: function (result) {
                    listHeaders.push({level1id: $(element).attr('id'), list: result });
                    _writeFile("ListHeaders_"+$(element).attr('id')+".json", JSON.stringify(result));
                },
                error: function (e) {
                    _readFile("ListHeaders_"+$(element).attr('id')+".json", function(result){
                        if(result){
                            listHeaders.push({level1id: JSON.parse(result)[0].ParLevel1_Id, list: JSON.parse(result) });
                        }
                    });
                }
            });
        } else {
            _readFile("ListHeaders_"+$(element).attr('id')+".json", function(result){
                if(result){
                    try{
                    listHeaders.push({level1id: JSON.parse(result)[0].ParLevel1_Id, list: JSON.parse(result) });
                    }catch(e){}
                }
            });
        }
    });
}

function saveListHeaders(){
    _writeFile("ListHeaders.json", JSON.stringify(listHeaders));
}

function saveHeaderResultList(){
    _writeFile("HeaderResultList.json", JSON.stringify(headerResultList));
}

function setEvaluationByHeadersSelection(){

    $('.form-control[linknumberevaluetion=true]').css("border", "#a7b018 solid 1px");
    $('.form-control[linknumberevaluetion=true]').parent().children('label').css("color", "#a7b018");

    var selectedHeaders = [];
    var triggerChange = true;
    $('select[linknumberevaluetion=true]:visible option:selected').each(function(i, e){
        if($(e).attr('hashkey')){
            selectedHeaders.push($(e).attr('hashkey'));
        } if(!!parseInt($(e).val())) { 
            selectedHeaders.push($(e).val());
        } else {
            triggerChange = false;
        }
    });

    if(triggerChange && selectedHeaders.length) {
        var period = parseInt($('.App').attr('period'));
        var shift = parseInt($('.App').attr('shift'));
        var level1id = $('.level1.selected').attr('id');
        var values = $.grep(headerResultList, function(o, i){
            return selectedHeaders.indexOf(o.Value) >= 0 && o.ParLevel1_Id == level1id 
                && o.Shift == shift && o.Period == period 
                && (!isEUA || (isEUA && o.ReauditNumber == ReauditByHeader.CurrentReauditNumber));
        });

        var maxEvaluation = 1;
        var maxSample = 1;
        for(var i = 0; i < values.length; i++){
            if(values[i].Evaluation > maxEvaluation)
                maxEvaluation = values[i].Evaluation;
            if(values[i].Sample > maxSample)
                maxSample = values[i].Sample;
        }
        
        if($('.level2.selected').length > 0){
            var level2id = $('.level2.selected').attr('id');

            var compare = [];
            for(var i = 1; i <= maxEvaluation; i++){
                for(var j = 1; j <= maxSample; j++){
                    compare.push($.grep(values, function(o){
                        return o.Evaluation == i && o.Sample == j && o.ParLevel2_Id == level2id;
                    }));
                }
            }
    
            var matchHeaders = [];
            for(var i = 0; i < compare.length; i++){
                if($('select[linknumberevaluetion="true"]:visible').length > 0 ||  selectedHeaders.length == compare[i].length){
					if(!!compare[i][0])
						matchHeaders.push(compare[i][0]);
                }
            }

            var evaluation = 1;
            var sample = 1;
            var evaluateTotal = parseInt($('.level3Group[level1id='+level1id+'][level2id='+level2id+'] .evaluateTotal').text());
            var sampleTotal = parseInt($('.level3Group[level1id='+level1id+'][level2id='+level2id+'] .sampleTotal').text());
            $.grep(matchHeaders, function(o){
                if(o.ParLevel2_Id == level2id){
                    if(sampleTotal == sample){
                        evaluation++;
                        sample = 1;
                    } else {
                        sample++;
                    }
                }
            });
        
            setHeaderSampleOnLevel3(sample, sampleTotal);
            setHeaderEvaluationOnLevel3(evaluation, evaluateTotal);

            var defects = 0;
            $.grep(values, function(o){
                if(o.ParLevel2_Id == level2id)
                    defects += parseInt(o.Defects);
            })

            $('.defects:visible').text(defects);

        } else {

            $('.level2Group[level01id='+level1id+'] .level2').each(function(i, e){
                
                var evaluateTotal = parseInt($(e).attr('evaluate'));
                var sampleTotal = evaluateTotal * parseInt($(e).attr('sample'));
                var parlevel2id = parseInt($(e).attr('id'));
                var evaluation = 0;
                var sample = 0;
                var sampleCount = 0;
                var defects = 0;

                var compare = [];
                for(var i = 1; i <= maxEvaluation; i++){
                    for(var j = 1; j <= maxSample; j++){
                        compare.push($.grep(values, function(o){
                            return o.Evaluation == i && o.Sample == j && o.ParLevel2_Id == parlevel2id;
                        }));
                    }
                }
        
                var matchHeaders = [];
                for(var i = 0; i < compare.length; i++){
                    if($('select[linknumberevaluetion="true"]:visible').length > 0 || selectedHeaders.length == compare[i].length){
						if(!!compare[i][0])
							matchHeaders.push(compare[i][0]);
                    }
                }

                $.grep(matchHeaders, function(o){
                    if(o.ParLevel2_Id == parlevel2id){
                        sample++;
                        if(parseInt($(e).attr('sample')) == sample){
                            evaluation++;
                            sample = 0;
                        }
                        sampleCount++;
                    }
                });

                $(e).parent().find('.evaluateCurrent').text(evaluation);
                $(e).parent().find('.sampleCurrentTotal').text(sampleCount);

                if(sampleCount >= sampleTotal){
                    $(e).parent().addClass('bgCompleted');
                }else{
                    $(e).parent().removeClass('bgCompleted');
                }

                $.grep(values, function(o){
                    if(o.ParLevel2_Id == parlevel2id)
                        defects += parseInt(o.Defects);
                });

                $(e).parent().find('.defectstotal').text(defects);
                $(e).attr('totaldefeitos', defects);

                if(defects == 0){
                    $(e).parent().removeClass('bgLimitExceeded');
                } else {
                    $(e).parent().addClass('bgLimitExceeded');
                }

            });
        }      
    }
}

function setHeaderEvaluationOnLevel3(evaluation, evaluationTotal){
    var div = $("<div class='col-xs-6 col-sm-4 col-md-3 col-lg-2 headerEvaluation' style='padding-right: 4px !important; padding-left: 4px !important;'><div class='form-group' style='margin-bottom: 4px;'><label class='font-small' style='display:inherit'>"+getResource('evaluation')+" </label><label style='display:inline-block; font-size: 20px;'><span class='headerEvaluationCount'>"+evaluation+"</span><span class='separator'> / </span><span>"+evaluationTotal+"</span></label></div></div>");

    if($('.painelLevel03:visible .headerEvaluation').length == 0){
        $('.painelLevel03:visible').prepend(div);
    } else {
        $('.painelLevel03:visible .headerEvaluationCount').text(evaluation);
    }
    
    if($('.evaluateCurrent:visible').length > 0){
        $('.evaluateCurrent:visible').parent().parent().parent().addClass('hide');
    }
}

function setHeaderSampleOnLevel3(sample, sampleTotal){
    var div = $("<div class='col-xs-6 col-sm-4 col-md-3 col-lg-2 headerSample' style='padding-right: 4px !important; padding-left: 4px !important;'><div class='form-group' style='margin-bottom: 4px;'><label class='font-small' style='display:inherit'>"+getResource('sample')+" </label><label style='display:inline-block; font-size: 20px;'><span class='headerSampleCount'>"+sample+"</span><span class='separator'> / </span><span>"+sampleTotal+"</span></label></div></div>");

    if($('.painelLevel03:visible .headerSample').length == 0){
        $('.painelLevel03:visible').prepend(div);
    } else {
        $('.painelLevel03:visible .headerSampleCount').text(sample);
    }

    if($('.sampleCurrent:visible').length > 0){
        $('.sampleCurrent:visible').parent().parent().parent().addClass('hide');
    }
}

function updateHeaderCollection(){
    var level1id = parseInt($('.level1.selected').attr('id'));
    var level2id = parseInt($('.level2.selected').attr('id'));
    var period = parseInt($('.App').attr('period'));
    var shift = parseInt($('.App').attr('shift'));
    var evaluation = parseInt($('.level3Group[level1id='+level1id+'][level2id='+level2id+'] .headerEvaluationCount').text());
    var sample = parseInt($('.level3Group[level1id='+level1id+'][level2id='+level2id+'] .headerSampleCount').text());
    var reauditNumber = ReauditByHeader.CurrentReauditNumber;

    $('.painelLevel02:visible select[linknumberevaluetion=true],.header:visible select[linknumberevaluetion=true]')
        .each(function(i, e){
            var defects = 0;
            var value = $(e).children('option:selected').attr('hashkey') ? 
                        $(e).children('option:selected').attr('hashkey') : 
                        $(e).children('option:selected').val();

            headerResultList.forEach(function(o){
                if(o.ParLevel1_Id == level1id &&
                    o.ParLevel2_Id == level2id &&
                    o.Period == period &&
                    o.Shift == shift &&
                    o.Value == value &&
                    o.ReauditNumber == reauditNumber
                )
                defects += parseInt(o.Defects);
            });

            defects = parseInt($('.level3Group[level1id='+$('.level1.selected').attr('id')+'][level2id='+$('.level2.selected').attr('id')+'] .defects').text()) - defects;

            if(parseInt($(e).val())){
                var collectedHeader = {
                    Evaluation : evaluation,
                    ParHeaderField_Id : parseInt($(e).attr("ParHeaderField_Id")),
                    ParLevel1_Id : level1id,
                    ParLevel2_Id : level2id,
                    Period : period,
                    Sample : sample,
                    Shift : shift,
                    Defects: defects,
                    Value : value,
                    ReauditNumber : reauditNumber
                };
                headerResultList.push(collectedHeader);
            }
        
    });
    
    saveHeaderResultList();
}

function headerWithCheckbox(){
	var headers = $('select[checkbox=true]:visible');

	headers.each(function(index, elem){
		
		$(elem).addClass('hide');
        var btnSelect = $('<button class="btn btn-default btn-block btn-sm" style="padding-right: 4px; padding-left: 4px; text-overflow: ellipsis; overflow: hidden;white-space: nowrap">' + 
            getResource('select') + '...</button>');

        btnSelect.click(function(){
            var listGroup = $("<ul class='list-group header-checkbox'></ul>");
            var title = $("<li class='list-group-item active'>"+$(elem).parent().find('label').text()+"</li>");
            var btnClose = $("<i class='fa fa-close pull-right'></i>");
            btnClose.click(function(){
                $('.header-checkbox').remove();
            })
            title.append(btnClose);
            listGroup.append(title);
            $(elem).children('option').each(function(index2, option){
                if(parseInt($(option).val()))
                    listGroup.append(
                        "<li class='list-group-item' id='"+$(option).val()+"'>"+
                            "<input type='checkbox' "+($(this).hasClass('selected') ? "checked" : "")+"/>"+
                            "<span>" + $(option).text()+"</span>"
                        +"</li>");
            });
            listGroup.append("<li class='list-group-item'></li>");

            var btnConfirm = $("<button class='btn btn-default btn-block'>"+getResource('save')+"</button>");

            btnConfirm.click(function(){
                var selectedValues = $('.header-checkbox [type=checkbox]:checked').parent();
                var text = getResource('select')+'...';
                $(elem).children('option').removeClass('selected');

                if(selectedValues.length > 0){
                    text = "";
                    selectedValues.each(function(index, select){
                        text += $(select).text() + ", ";
                        $(elem).children('option[value='+$(select).attr('id')+']').addClass('selected');
                    });
                }

                btnSelect.text(text);                

                $('.header-checkbox').remove();
            });

            listGroup = $(listGroup);

            listGroup.children('.list-group-item:last').append(btnConfirm);
            
            $('body').append(listGroup);
		})

		$(elem).after(btnSelect);
	})
}

function getFilteredHeaderResultList(){

    var _list = [];

    headerResultList.forEach(function(o){
        var _l = [];
        _list.forEach(function(t) {
            if(t.Period == o.Period &&
                t.Shift == o.Shift &&
                t.ParLevel1_Id == o.ParLevel1_Id &&
                t.ParLevel2_Id == o.ParLevel2_Id &&
                t.Evaluation == o.Evaluation &&
                t.Sample == o.Sample &&
                t.ReauditNumber == o.ReauditNumber
            )
                _l.push(t)
        });
        if(_l.length == 0){
            _list.push(o);
        }
    });

    return _list;
}

$(document).on('click', '.btnCloseHeaderCheckbox', function(){
    $('.header-checkbox').remove();
});

$(document).on('click', '.header-checkbox .list-group-item span', function(){
    $(this).siblings('input').click();
});