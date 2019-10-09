var ReauditByHeader = {
    ShowReauditPanel: function(ReauditNumber, Level1Id, Level2Id) {
        var reauditTag = $('.level2Group[level01id='+Level1Id+'] .reauditFlag');        
        if(!!Level2Id)
            reauditTag = $('.level3Group[level1id='+Level1Id+'][level2id='+Level2Id+'] .reauditFlag'); 

        reauditTag.removeClass('hide');
        reauditTag.find('.reauditnumber').text(ReauditNumber);
    },
    HideReauditPanel: function(Level1Id, Level2Id){
        var reauditTag = $('.level2Group[level01id='+Level1Id+'] .reauditFlag');
        if(!!Level2Id)
            reauditTag = $('.level3Group[level1id='+Level1Id+'][level2id='+Level2Id+'] .reauditFlag'); 

        reauditTag.addClass('hide');
    },
    EnableCurrentHeader: function(ParHeaderFieldId, Value, Level1Id, Level2Id){
        if(!Level2Id){
            $('.level2Group[level01id='+Level1Id+'] select:visible[parheaderfield_id='+ParHeaderFieldId+']').removeAttr('disabled');
            $('.level2Group[level01id='+Level1Id+'] select:visible[parheaderfield_id='+ParHeaderFieldId+']').val(Value);
        } else {
            $('.level3Group[level1id='+Level1Id+'][level2id='+Level2Id+'] select:visible[parheaderfield_id='+ParHeaderFieldId+']').removeAttr('disabled');
            $('.level3Group[level1id='+Level1Id+'][level2id='+Level2Id+'] select:visible[parheaderfield_id='+ParHeaderFieldId+']').val(Value);
        }
    },
    DisableCurrentHeader: function(ParHeaderFieldId, Level1Id, Level2Id){
        if(!Level2Id){
            $('.level2Group[level01id='+Level1Id+'] select:visible[parheaderfield_id='+ParHeaderFieldId+']').attr('disabled', true);
        } else {
            $('.level3Group[level1id='+Level1Id+'][level2id='+Level2Id+'] select:visible[parheaderfield_id='+ParHeaderFieldId+']').attr('disabled', true);
        }
    },
    IsCollectionByHeader: function(Level1Id){
        for(var i = 0; i < headerResultList.length; i++)
            if(headerResultList[i].ParLevel1_Id == Level1Id)
                return true;
        return false;
    },
    SetReaudit: function(ParHeaderFieldId, Value, Evaluation, Sample, ReauditLevel, level2Result){
        var period = parseInt($('.App').attr('period'));
        var shift = parseInt($('.App').attr('shift'));
        var level1id = parseInt($('.level1.selected').attr('id'));
        var level2id = parseInt($('.level2.selected').attr('id'));

        for(var i = 0; i < headerResultList.length; i++){
            if(headerResultList[i].ParLevel1_Id == level1id &&
                headerResultList[i].ParLevel2_Id == level2id &&
                headerResultList[i].Evaluation == Evaluation &&
                headerResultList[i].Sample == Sample &&
                headerResultList[i].Period == period &&
                headerResultList[i].Shift == shift) {
                    headerResultList[i].HaveReaudit = true;
                    headerResultList[i].ReauditLevel = ReauditLevel;
                    this.SetHeaveReaudit(headerResultList[i], level2Result);
            }
        }
    },
    SetHeaveReaudit: function(headerResult, level2Result){

        var headers = "";

        $(level2Result).attr('headerlist').split('<header>').forEach(function(element){
            if(element){
                headers += '<header>';
                element.replace('<header>', '').replace('</header>', '').split(',').forEach(
                    function(_element, _index){
                        if(_index == 8)
                            headers += "1,";
                        else
                            headers += _element + ",";
                    }
                )
                headers += '</header>';
            }
        });

        level2Result.attr('headerlist', headers);

    },
    triggerReaudit: false,
    CurrentReauditNumber: 0,
    SetupReaudit: function(Level1Id){

        if(this.triggerReaudit){
            var period = parseInt($('.App').attr('period'));
            var shift = parseInt($('.App').attr('shift'));

            var listHeaders = $.grep(headerResultList, function(o, i){
                return o.ParLevel1_Id == Level1Id && o.Period == period && o.Shift == shift;
            });

            this.CurrentReauditNumber = 0;

            var result;

            if(listHeaders.length > 0){
                var notEnded = true;
                var groups = _.groupBy(listHeaders, 'ReauditNumber');
                while(notEnded){
                    var group = groups[this.CurrentReauditNumber];
                    if(group){
                        var haveReaudit = $.grep(group, function(o){ return o.HaveReaudit == true; }).length > 0;
                        
                        if (haveReaudit) {
                            result = $.grep(group, function(o){ return o.HaveReaudit == true; })[0];
                            this.CurrentReauditNumber++;
                            notEnded = false;
                        } else { 
                            this.CurrentReauditNumber++;
                            notEnded = false;
                        }
                    } else {
                        notEnded = false;
                    }
                }

                if(!!result) {
                    this.ShowReauditPanel(this.CurrentReauditNumber, Level1Id);
                    this.EnableCurrentHeader(result.ParHeaderField_Id, result.Value, Level1Id);
                }
            }
        }
        
    } 
}