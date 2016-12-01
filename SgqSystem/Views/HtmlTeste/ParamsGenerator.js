
level1.forEach(function(o, c){

    o.DdlLevel2Vinculados = null;
    //o.listParLevel3Level2Level1Dto = null;


});

function CreateHeader(levelHeader, cabecalhosInclusos, contadoresIncluidos){

    var header = '<div class="row">                                                      ';

    if(cabecalhosInclusos){
        cabecalhosInclusos.forEach(function(o, c){

            switch(o.ParHeaderField.ParFieldType.Id){
                case 1:
                    //Multipla Escolha
                    header += '<div class="col-xs-3"><select class="form-control input-sm" name="DropDownList">';
                    var nameHeader = o.ParHeaderField.Name;
                    o.ParHeaderField.DropDownList.forEach(function(p, i){
                        if(p.Value <= 0 ){
                            header += '<option value="'+p.Value+'"> @Resources.Resource.select '+nameHeader+'</option>';
                        }else{
                            header += '<option value="'+p.Value+'">'+p.Text+'</option>';
                        }

                    });
                    header += '</select></div>';
                    break;
                case 2:
                    //Integrações

                    break;
                case 3:
                    //Binario
                    header += '<div class="col-xs-3"><select class="form-control input-sm" name="DropDownList">';
                    o.ParHeaderField.DropDownList.forEach(function(p, i){
                        if(p.Value > 0)
                            header += '<option value="'+p.Value+'">'+p.Text+'</option>';
                    });
                    header += '</select></div>';
                    break;
                case 4:
                    //Texto
                    header +=   '<div class="col-xs-3"><input type="text" placeholder="'+o.ParHeaderField.Name+'" class="form-control input-sm" id="'+o.ParHeaderField.Id+'"/></div>';
                    break;
                case 5:
                    //Numerico
                    header +=   '<div class="col-xs-3"><input type="number" placeholder="'+o.ParHeaderField.Name+'" class="form-control input-sm" id="'+o.ParHeaderField.Id+'"/></div>';
                    break;
                case 6:
                    //Data
                    header +=   '<div class="col-xs-3"><input type="date" placeholder="'+o.ParHeaderField.Name+'" class="form-control input-sm" id="'+o.ParHeaderField.Id+'"/></div>';
                    break;
            }
        });
    }

    if(contadoresIncluidos){
        contadoresIncluidos.forEach(function(o, c){

            var countersLevel2 = "";
            var parCounterName = o.ParCounter.Name;
            var title = o.ParCounter.Name;
            var valueCounter = "0";

            switch (o.ParCounter.Id) {
                case 21:
                    parCounterName = "Nº AV PL";
                    //valueCounter = o.ParamEvaluation.Number;
                    break;
                case 22:
                    parCounterName = "Nº AM PL"
                    //valueCounter = o.ParamSample.Number;
                    break;
                case 23:
                    parCounterName = "TOT AV"
                    break;
                case 24:
                    parCounterName = "TS AV"
                    break;
                case 25:
                    parCounterName = "TOT NC"
                    break;
                case 27:
                    parCounterName = "Nº AM."
                    break;
                case 29:
                    parCounterName = "Nº AV."
                    break;
                case 30:
                    parCounterName = "AV RL/AV PL"
                    //valueCounter = "0/" + o.ParamEvaluation.Number;///precisa ser retornado do banco

                    break;
                case 31:
                    parCounterName = "AM RL/AV PL"
                    //valueCounter =  "0/" + o.ParamSample.Number;///precisa ser retornado do banco

                    break;
                default:

            }


            countersLevel2 += " <div class='col-xs-3'><span style='cursor:help;'><b title='" + title + "'> " + parCounterName + ":</b><span> " + valueCounter + " </span></span></div>";

            header +=   countersLevel2;
        });
    }

    header += '</div>';

    $(levelHeader).empty();
    appendDevice($(header), $(levelHeader));

}

function CreateLevel1Group(listLevel1) {

    //As colunhas do Level1 estão relacionadas as @*Heder do Level1*@ no Html. Caso alterar o tanho das divs aqui deve ser alterada no HTML também
    listLevel1.forEach(function(o, c){



        var itemGroupLevel1 = '<li class="list-group-item" style="padding-bottom:0">                                                                                                                     '+
          '   <div class="row">                                                                                                                                                 '+
          //Level Button
          '       <a href="#" id="' + o.Id + '" class="level01 col-xs-6 col-sm-6 col-md-6 col-lg-8" savelevel02="saveLevel02" minreauditnumber="1" consecutivefailure="false">  '+
          '           <span class="icons"><i class="fa fa-check fa-3 font22 iconsArea green areaComplete hide" aria-hidden="true" title="Complete"></i></span>                  '+
          '           <span class="levelName" style="font-size:17px; font-family: Verdana">' + o.Name + '</span>                                                                                                             '+
          '       </a>                                                                                                                                                          '+
          //Buttons Level1
          '       <div class="col-xs-6 col-sm-6 col-md-6 col-lg-4 userInfo">                                                                                                    '+
          '           <div class="pull-right">                                                                                                                                  '+
          '               <span class="btnCorrectiveAction iconsArea font22 hide red"><button class="btn btn-sm btn-danger">Corrective Action</button></span>                          '+
          '               <button class="btn btn-primary btn-sm btnReaudit hide">Reaudit <span class="reauditPeriod fontBold"></span></button>                                         '+
          '               <!--<span class=" cursorPointer   "><button class=""></span></button></span>-->                                                                       '+
          '               <!--<span class="reauditCount iconsArea font22 hide" count="0"><button class="btn btn-default">0</button></span>-->                                   '+
          '                                                                                                                                                                     '+
          '           </div>                                                                                                                                                    '+
          '       </div>                                                                                                                                                        '+
          '   </div>                                                                                                                                                            '+
          '<div class="row"><div class="col-xs-12" style="background-color: #f5f5f5;height: 30px;margin-top:5px; line-height: 30px;">';
        o.contadoresIncluidos.forEach(function(o, c){
            itemGroupLevel1 += " <b> " + o.ParCounter.Name + ":</b><span> 0 </span>";
        });

        itemGroupLevel1 += '</div></div></li>';

        appendDevice(itemGroupLevel1, $('.level01List .list-group'));
        //$('.level01List .list-group').append(itemGroupLevel1);


        CreateLevel2Listgroup(o.listParLevel2Colleta, o.Id, o.Name, o.listParLevel3Level2Level1Dto, o.HasSaveLevel2, o.HasNoApplicableLevel2);

    });
}

function CreateLevel2Listgroup(listLevel2Item, idLevel1, level1Name, listParLevel3Level2Level1Dto, saveLevel2, naLevel2) {

    var level2Item = '<div class="list-group level02Group hide" level01Id="' + idLevel1 + '">       ';

    var isCreated = false;
    listLevel2Item.forEach(function(o, c){

        var countersLevel2 = "";
        o.listParCounterXLocal.forEach(function(p, c){

            var parCounterName = "";
            var title = p.ParCounter.Name;
            var valueCounter = "0";

            switch (p.ParCounter.Id) {
                case 21:
                    parCounterName = "Nº AV PL";
                    valueCounter = o.ParamEvaluation.Number;
                    break;
                case 22:
                    parCounterName = "Nº AM PL"
                    valueCounter = o.ParamSample.Number;
                    break;
                case 23:
                    parCounterName = "TOT AV"
                    break;
                case 24:
                    parCounterName = "TS AV"
                    break;
                case 25:
                    parCounterName = "TOT NC"
                    break;
                case 27:
                    parCounterName = "Nº AM."
                    break;
                case 29:
                    parCounterName = "Nº AV."
                    break;
                case 30:
                    parCounterName = "AV RL/AV PL"
                    valueCounter = "0/" + o.ParamEvaluation.Number;

                    break;
                case 31:
                    parCounterName = "AM RL/AV PL"
                    valueCounter =  "0/" + o.ParamSample.Number;

                    break;
                default:

            }


            countersLevel2 += " <span style='cursor:help;'><b title='" + title + "'> " + parCounterName + ":</b><span> " + valueCounter + " </span></span>";
        });

        if(o.HasShowLevel03){/*Ex CFF*/

            var cabecalhoLevel3 = '';
            if (!isCreated) {

                level2Item += '<li class="list-group-item">                                                                                                 ' +
              '   <div class="row">                                                                                                                         ' +
              '       <a href="#" id="" class="level02 col-xs-3 col-sm-3 col-md-3 col-lg-4" levelerrorlimit="1" defects="0" reauditnumber="0">              ' +
              '           <span class="icons">                                                                                                              ' +
              '               <i class="fa fa-check fa-3 font22 iconsArea green hide areaComplete" aria-hidden="true" title="Complete"></i>                 ' +
              '               <i class="fa fa-exclamation font22 iconsArea red areaNotComplete" aria-hidden="true" title="Not Complete"></i>                ' +
              '           </span>                                                                                                                           ' +
              '           <span class="levelName">' + level1Name + '</span>                                                                                 ' +
              '       </a>                                                                                                                                  ' +

              '       <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1 userInfo">                                                                            ' +
              '           <div class="hide">                                                                                                                ' +
              '               <a href="#" class="consecutiveFailure iconsArea font22  pull-right">0</a>                                                     ' +
              '           </div>                                                                                                                            ' +
              '       </div>                                                                                                                                ' +

              '       <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1 userInfo">                                                                            ' +
              '           <div>                                                                                                                             ' +
              '               <a href="#" class="defects iconsArea font22">0</a>                                                                            ' +
              '           </div>                                                                                                                            ' +
              '       </div>                                                                                                                                ' +

              '       <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4 userInfo">                                                                            ' +
              '           <div class="pull-right">                                                                                                          ' +
              '               <button class="btn btn-success hide btnAreaSaveConfirm">                                                                      ' +
              '                   <span class="cursorPointer">Confirm? <i class="fa fa-check font22" aria-hidden="true"></i></span>                         ' +
              '               </button>                                                                                                                     ' +
              '               <button class="btn btn-primary btnAreaSave">                                                                                  ' +
              '                   <span class="cursorPointer iconsArea"><i class="fa fa-floppy-o font22" aria-hidden="true"></i></span>                     ' +
              '               </button>                                                                                                                     ' +
              '               <button class="btn btn-warning btnNotAvaliable na">                                                                           ' +
              '                   <span class="cursorPointer iconsArea font18">N/A</span>                                                                   ' +
              '               </button>                                                                                                                     ' +
              '               <span class="btnReaudit cursorPointer iconsArea font22 hide"><button class="btn btn-primary">Reaudit</button></span>          ' +
              '           </div>                                                                                                                            ' +
              '       </div>                                                                                                                                ' +
              '   </div>                                                                                                                                    ' ;

                level2Item += countersLevel2;


                level2Item += ' </li>';

                cabecalhoLevel3 += '<div class="level03Group" level01id="' + idLevel1 + '">                                                                                                                              ' +
                                       '<div class="marginBottom10">                                                                                                                                            ' +
                                       '    <button class="btn btn-default button-expand marginRight10"><i class="fa fa-expand" aria-hidden="true"></i> Expand All</button>                                     ' +
                                       '    <button class="btn btn-default button-collapse"><i class="fa fa-compress" aria-hidden="true"></i> Collapse All</button>                                             ' +
                                       '</div>';
                cabecalhoLevel3 += '</div>';
                appendDevice(cabecalhoLevel3, $('.level03List'));
                //$('.level03List').append(cabecalhoLevel3)
                isCreated = true;
            }
            CriaLevel3AgrupadoPorLevel2(o.listaParLevel3Colleta, o.Name, o.Id, idLevel1, cabecalhoLevel3);

        } else {/*Ex CCA*/

            level2Item += CriaItemLevel2TipoCCA(o, saveLevel2, naLevel2);

            if(o.ParLevel3Group.length == 0){
                CreateLevel3(o.listaParLevel3Colleta, idLevel1, o.Id);
            }
            else {

                var listParLevel3Level2Level1 = jQuery.grep(listParLevel3Level2Level1Dto, function( n ) {
                    return ( n.ParLevel3Level2.ParLevel2_Id ==  o.Id );
                });

                var level03Group = '<div class="level03Group" level01id="' + idLevel1 + '" level02id="' + o.Id + '">                                                         ' +
                                      '<div class="marginBottom10">                                                                                                          ' +
                                      '    <button class="btn btn-default button-expand marginRight10"><i class="fa fa-expand" aria-hidden="true"></i> Expand All</button>   ' +
                                      '    <button class="btn btn-default button-collapse"><i class="fa fa-compress" aria-hidden="true"></i> Collapse All</button>           ' +
                                      '</div>                                                                                                                                ' +
                                      '<button id="btnSalvarHTP" class="btn btn-default level03Confirm hide">Salvar</button>                                                 ' +
                                   '</div> ';

                appendDevice(level03Group, $('.level03List'));
                //$('.level03List').append(level03Group);

                var groupLevel3 = '';

                /*Adiciono os que possuem group*/
                listParLevel3Level2Level1.forEach(function(level3Level2Leve1, counter){

                    var group = level3Level2Leve1.ParLevel3Level2.ParLevel3Group;

                    if(group != null){

                        var level3 = jQuery.grep(listParLevel3Level2Level1, function( n ) {
                            if(n.ParLevel3Level2.ParLevel3Group != null){
                                return (n.ParLevel3Level2.ParLevel3Group.Id == group.Id);
                            }
                        });
                        var groupSearch  = $(groupLevel3).find('.panelGroupLevel3'+ group.Id).length;
                        if(!groupSearch){


                            groupLevel3 +=  '<div class="panel panel-default panelGroupLevel3'+ group.Id +'" >                                                                                                                                                ' +
                                            '      <div class="panel-heading level02" role="tab" id="headingOne" level02id="40" reauditnumber="0" defects="0">                                                                 ' +
                                            '          <h4 class="panel-title">                                                                                                                                                ' +
                                            '              <a role="button" data-toggle="collapse" data-parent="#accordion40" href="#collapsehOne' + group.Id + '" aria-expanded="true" aria-controls="collapsehOne" class=""> ' +
                                            '                  ' + group.Name + '                                                                                                                                              ' +
                                            '              </a>                                                                                                                                                                ' +
                                            '          </h4>                                                                                                                                                                   ' +
                                            '      </div>                                                                                                                                                                      ' +
                                            '      <div id="collapsehOne' + group.Id + '" class="panel-collapse in" role="tabpanel" aria-labelledby="headingOne" style="height: auto;">                                        ' +
                                            '          <div class="panel-body panelGroupLevel3'+ group.Id +'" id="">';

                            level3.forEach(function(obj, counterL3){
                                var l3 = obj.ParLevel3Level2.ParLevel3;

                                //intervalos
                                if(l3.ParLevel3Value[0].ParLevel3InputType.Id == 3){

                                    groupLevel3 +=   ' <div id="' + l3.Id + '" class="level03">' +
                                                    '   <div class="row">' +
                                                    '        <div class="col-xs-9">' + l3.Name + '</div>' +
                                                    '        <div class="col-xs-3 text-center">' +
                                                    '          <div class="input-group input-group-sm width200 pull-right">'+
                                                    '           <span class="input-group-btn btn-minus"><button class="btn btn-default" type="button"><i class="fa fa-minus" aria-hidden="true"></i></button></span><input value="0" min="0" type="number" class="form-control text-center levelValue"><span class="input-group-btn btn-plus"><button class="btn btn-default" type="button"><i class="fa fa-plus" aria-hidden="true"></i></button></span>'+
                                                    '          </div>' +
                                                    '        </div>' +
                                                    '    </div>' +
                                                    '</div>';

                                }
                                    //binarios
                                else if (l3.ParLevel3Value[0].ParLevel3InputType.Id == 1){

                                    groupLevel3 +=   ' <div id="' + l3.Id + '" class="level03 boolean">' +
                                                     '   <div class="row">' +
                                                     '        <div class="col-xs-9">' + l3.Name + '</div>' +
                                                     '        <div class="col-xs-3 text-center">' +
                                                     '            <span value="1" booltrueName="' + l3.ParLevel3Value[0].ParLevel3BoolTrue.Name + '" boolfalseName="' + l3.ParLevel3Value[0].ParLevel3BoolFalse.Name + '" class="pull-right marginRight30 response">' + l3.ParLevel3Value[0].ParLevel3BoolTrue.Name + '</span>' +
                                                     '        </div>' +
                                                     '    </div>' +
                                                     '</div>';
                                }
                            });

                            groupLevel3 +=    '           </div>                                                                                                                                                               ' +
                                            '      </div>                                                                                                                                                                      ' +
                                            '</div>';

                        }

                    }

                });

                appendDevice($(groupLevel3), $('.level03List > [level01id=' + idLevel1 + '][level02id=' + o.Id + ']'));
                //$('.level03List > [level01id=' + idLevel1 + '][level02id=' + o.Id + ']').append($(groupLevel3));
            }
        }

    });

    level2Item += '<button id="btnSalvarLevel02CCA" class="btn btn-default level02Confirm hide">Salvar</button></div>';

    appendDevice(level2Item, $('.level02List'));
    //$('.level02List').append(level2Item);
}

function CriaItemLevel2TipoCCA(o, saveLevel2, naLevel2) {

    var btnAreaSave = '';
    var level2Item  = '';

    if(saveLevel2 ==  true)
    {
        btnAreaSave =    '               <button class="btn btn-success hide btnAreaSaveConfirm">                                                                      ' +
                         '                   <span class="cursorPointer">Confirm? <i class="fa fa-check font22" aria-hidden="true"></i></span>                         ' +
                         '               </button>                                                                                                                     ' +
                         '               <button class="btn btn-primary btnAreaSave">                                                                                  ' +
                         '                   <span class="cursorPointer iconsArea"><i class="fa fa-floppy-o font22" aria-hidden="true"></i></span>                     ' +
                         '               </button>                                                                                                                     ';
    }

    var btnNA = '';

    if(naLevel2 ==  true)
    {
        var btnNA =          '               <button class="btn btn-warning btnNotAvaliable na">                                                                           ' +
                             '                   <span class="cursorPointer iconsArea font18">N/A</span>                                                                   ' +
                             '               </button>                                                                                                                     ' ;

    }

    var countersLevel2 = "";
    o.listParCounterXLocal.forEach(function(p, c){

        var parCounterName = "";
        var title = p.ParCounter.Name;
        var valueCounter = "0";

        switch (p.ParCounter.Id) {
            case 21:
                parCounterName = "Nº AV PL";
                valueCounter = o.ParamEvaluation.Number;
                break;
            case 22:
                parCounterName = "Nº AM PL"
                valueCounter = o.ParamSample.Number;
                break;
            case 23:
                parCounterName = "TOT AV"
                break;
            case 24:
                parCounterName = "TS AV"
                break;
            case 25:
                parCounterName = "TOT NC"
                break;
            case 27:
                parCounterName = "Nº AM."
                break;
            case 29:
                parCounterName = "Nº AV."
                break;
            case 30:
                parCounterName = "AV RL/AV PL"
                valueCounter = "0/" + o.ParamEvaluation.Number;

                break;
            case 31:
                parCounterName = "AM RL/AV PL"
                valueCounter =  "0/" + o.ParamSample.Number;

                break;
            default:

        }


        countersLevel2 += " <span style='cursor:help;'><b title='" + title + "'> " + parCounterName + ":</b><span> " + valueCounter + " </span></span>";
    });

    level2Item +=  '<li class="list-group-item" style="padding-bottom:0">                                                                                                               ' +
         '   <div class="row">                                                                                                                         ' +
         '       <a href="#" id="' + o.Id + '" class="level02 col-xs-8 col-sm-8 col-md-8 col-lg-8" levelerrorlimit="1" defects="0" reauditnumber="0">  ' +
         '           <span class="icons">                                                                                                              ' +
         '               <i class="fa fa-check fa-3 font22 iconsArea green hide areaComplete" aria-hidden="true" title="Complete"></i>                 ' +
         '               <i class="fa fa-exclamation font22 iconsArea red areaNotComplete" aria-hidden="true" title="Not Complete"></i>                ' +
         '           </span>                                                                                                                           ' +
         '           <span class="levelName" style="font-size:24px;font-family:Verdana !important">' + o.Name + '</span>                                                                                     ' +
         '       </a>                                                                                                                                  ' +
         '       <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4 userInfo">                                                                            ' +
         '           <div class="pull-right">                                                                                                          ' +
          btnAreaSave                                                                                                                                    +
          btnNA                                                                                                                                          +
         '               <span class="btnReaudit cursorPointer iconsArea font22 hide"><button class="btn btn-primary">Reaudit</button></span>          ' +
         '           </div>                                                                                                                            ' +
         '       </div>                                                                                                                                ' +
         '   </div>                                                                                                                                    ' +
        '<div class="row"><div class="col-xs-12" style="background-color: #f5f5f5;height: 30px;margin-top:5px; line-height: 30px;font-size:12px;">' +
          countersLevel2 +

        '</div></div></li>';

    return level2Item;
}

/*Ex CCA*/
function CreateLevel3(listLevel3Item, idLevel1, idLevel2, groupName) {

    var htmlLevel3 = '<div class="level03Group hide" level01Id="' + idLevel1 + '" level02Id="' + idLevel2 + '"><ul class="list-group">';


    listLevel3Item.forEach(function(o, c){

        if(o.ParLevel3Value[0].ParLevel3InputType.Id == 1)//Binario
        {
            htmlLevel3 +=   '<li id="' + o.Id + '" class="list-group-item level03 boolean">' +
                            '   <div class="row">' +
                            '        <div class="col-xs-9">' + o.Name + '</div>' +
                            '        <div class="col-xs-3 text-center">' +
                            '            <span value="1" booltrueName="' + o.ParLevel3Value[0].ParLevel3BoolTrue.Name + '" boolfalseName="' + o.ParLevel3Value[0].ParLevel3BoolFalse.Name + '" class="pull-right marginRight30 response">' + o.ParLevel3Value[0].ParLevel3BoolTrue.Name + '</span>' +
                            '        </div>' +
                            '    </div>' +
                            '</li>';

        }else{

            htmlLevel3 +=   '<li id="' + o.Id + '" class="list-group-item level03">                                                              '+
                            '     <div class="row">                                                                                                           '+
                            '         <div class="col-xs-6">' + o.Name + '</div>                                                                              '+
                            '         <div class="col-xs-6 text-center">                                                                                      '+
                            '             <div class="input-group input-group-sm width200 pull-right">                                                        '+
                            '                 <span class="input-group-btn btn-minus">                                                                        '+
                            '                      <button class="btn btn-default" type="button">                                                             '+
                            '                          <i class="fa fa-minus" aria-hidden="true"></i>                                                         '+
                            '                          </button></span><input value="0" min="0" type="number" class="form-control text-center levelValue">    '+
                            '                          <span class="input-group-btn btn-plus"><button class="btn btn-default" type="button">                  '+
                            '                          <i class="fa fa-plus" aria-hidden="true"></i>                                                          '+
                            '                      </button>                                                                                                  '+
                            '                  </span>                                                                                                        '+
                            '             </div>                                                                                                              '+
                            '         </div>                                                                                                                  '+
                            '     </div>                                                                                                                      '+
                            ' </li>';

        }

    });

    htmlLevel3 += '</ul><button id="btnSalvarCCA" class="btn btn-default level03Confirm hide">Salvar</button></div>';

    appendDevice(htmlLevel3, $('.level03List'));
    //$('.level03List').append(htmlLevel3)

}

function CriaLevel3BodyAgrupado(listLevel3Item, group, idGroup) {



    listLevel3Item.forEach(function(o, c){

        //if(o.ParLevel3Value.ParLevel3InputType.Id == 1)//Binario
        //{
        panel +=   ' <div id="' + o.Id + '" class="level03">' +
                        '   <div class="row">' +
                        '        <div class="col-xs-9">' + o.Name + '</div>' +
                        '        <div class="col-xs-3 text-center">' +
                        '            <span value="1" class="pull-right marginRight30 response">Yes</span>' +
                        '        </div>' +
                        '    </div>' +
                        '</div>';
        //}
    });



    return panel;

}

/*Ex CFF*/
function CriaLevel3AgrupadoPorLevel2(listLevel3, Level2Name, level2Id, level1Id, cabecalho) {

    var level2Item = '<div class="panel-group" id="accordion' + level2Id + '" role="tablist" aria-multiselectable="true">                                                                     ' +
                    '    <div class="panel panel-default">                                                                                                                                   ' +
                    '        <div class="panel-heading level02" role="tab" id="headingOne" level02id=" ' + level2Id + ' " reauditnumber="0" defects="0">                                     ' +
                    '            <h4 class="panel-title">                                                                                                                                    ' +
                    '                <a role="button" data-toggle="collapse" data-parent="#accordion' + level2Id + '" href="#collapseOne' + level2Id + '" aria-expanded="true" aria-controls="collapseOne">  ' +
                    '                    <span class="levelName"> ' + Level2Name + ' </span>                                                                                                 ' +
                    '                </a>                                                                                                                                                    ' +
                    '            </h4>                                                                                                                                                       ' +
                    '        </div>                                                                                                                                                          ' +
                    '        <div id="collapseOne' + level2Id + '" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">                                                             ' +
                    '            <div class="panel-body">';

    listLevel3.forEach(function(o, c){

        level2Item +=  '<div id="' + o.Id + '" class="level03">                                                                                     ' +
                       '     <div class="row">                                                                                                      ' +
                       '         <div class="col-xs-6">                                                                                             ' +
                       '             <span class="levelName">'+ o.Name +'</span>                                                                    ' +
                       '         </div>                                                                                                             ' +
                       '         <div class="col-xs-6 text-center">                                                                                 ' +
                       '             <div class="input-group input-group-sm width200 pull-right">                                                   ' +
                       '                 <span class="input-group-btn btn-minus hide">                                                              ' +
                       '                     <button class="btn btn-default" type="button"><i class="fa fa-minus" aria-hidden="true"></i></button>  ' +
                       '                 </span>                                                                                                    ' +
                       '                 <input value="0" min="0" type="number" class="form-control text-center levelValue" disabled="disabled">    ' +
                       '                 <span class="input-group-btn btn-plus hide">                                                               ' +
                       '                     <button class="btn btn-default" type="button"><i class="fa fa-plus" aria-hidden="true"></i></button>   ' +
                       '                 </span>                                                                                                    ' +
                       '             </div>                                                                                                         ' +
                       '         </div>                                                                                                             ' +
                       '     </div>                                                                                                                 ' +
                       ' </div>';

    });

    level2Item += '              </div>'+
        '                </div>'+
        '           </div>'+
        '       </div>';

    appendDevice($(level2Item), $('.level03List > [level01id=' + level1Id + ']'));
    //$('.level03List > [level01id=' + level1Id + ']').append($(level2Item));


}

function getLevel1(id){
    for(var i = 0; i < level1.length; i++){
        if(level1[i].Id == id)
            return level1[i];
    }
    return {};
}

function getLevel2(level01id, level02id){
    for(var i = 0; i < level1.length; i++){
        var level2list = level1[i].listParLevel2Colleta;
        for(var j = 0; j < level2list.length; j++){
            if(level2list[j].Id == level02id)
                return level2list[j];
        }
    }
    return {};
}
