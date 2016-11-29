var crudNxN = {

    adiciona: function (o, idTable, tdsName) {

        var tr = "";
        var objData = {};

        if (o["IsActive"] == true) {
            tr += "<tr pos=" + crudNxN.inifnito + ">"
        } else {
            tr += "<tr pos=" + crudNxN.inifnito + " class='inativeTr'>";
        }

        tdsName.forEach(function (oo, cc) {/*Para cada name de tdName monta uma TD, coloca valor a ser mostrado na TD a prop do objeto[name].*/
            if (oo == "btn") {
                if (o["Id"] != null && o["Id"] != undefined && o["Id"] > 0) {
                    if (o["IsActive"]) {
                        o[oo] = crudNxN.btnInactive + crudNxN.btnEdit;
                    } else {
                        o[oo] = crudNxN.btnReActive + crudNxN.btnEdit;
                    }
                } else {
                    o[oo] = crudNxN.btnRemove + crudNxN.btnEdit;
                }
            }
            tr += '<td>' + o[oo] + '</td>';
        });

        tr += '</tr>'
        $(tr)
        $('#' + idTable + ' > tbody').append($(tr).data(o));
        crudNxN.verificaSeMostraTable(idTable);
        reloadPopovers();

    },
    verificaSeMostraTable: function (idTable) {
        if ($('#' + idTable + ' > tbody > tr').length > 0) {
            $('#' + idTable).fadeIn();
        } else {
            $('#' + idTable).fadeOut();
        }
    },
    btnRemove: '<button type="button" class="btn btn-danger btn-xs popovers" data-content="Remover" data-trigger="hover" data-placement="right" name="" onclick="crudNxN.funcRemove($(this));"><i class="fa fa-times" aria-hidden="true"></i></button>',
    funcRemove: function (e) {
        $(e).parents('tr').remove();
    },
    btnInactive: '<button type="button" class="btn btn-danger btn-xs popovers" data-content="Inativar" data-trigger="hover" data-placement="right" name="" onclick="crudNxN.funcInactive($(this));"><i class="fa fa-times" aria-hidden="true"></i></button>',
    funcInactive: function (e) {
        var obj = $(e).parents('tr').data();
        obj["IsActive"] = false;
        $(e).parents('tr').removeData();
        $(e).parents('tr').data(obj);
        var element = $(e).parents('tr');
        element.find('td:last').empty();
        element.find('td:last').append(crudNxN.btnReActive, crudNxN.btnEdit);
        reloadPopovers();
    },
    btnReActive: '<button type="button" class="btn btn-danger btn-xs popovers" data-content="Reativar" data-trigger="hover" data-placement="right" name="" onclick="crudNxN.funcReActive($(this));"><i class="fa fa-undo" aria-hidden="true"></i></button>',
    funcReActive: function (e) {
        var obj = $(e).parents('tr').data();
        obj["IsActive"] = true;
        $(e).parents('tr').removeData();
        $(e).parents('tr').data(obj);
        var element = $(e).parents('tr');
        element.find('td:last').empty();
        element.find('td:last').append(crudNxN.btnInactive, crudNxN.btnEdit);
        reloadPopovers();
    },
    btnEdit: '<button type="button" class="btn btn-danger btn-xs popovers" data-content="Alterar" data-trigger="hover" data-placement="right" name="" onclick="crudNxN.funcEdit($(this));"><i class="fa fa-pencil-square-o" aria-hidden="true"></i></button>',
    funcEdit: function (e) {

        $('#crudNxNEdit > div > div > div.modal-body').empty();
        $('#crudNxNEdit > div > div > div.modal-footer > button.btn.btn-primary').off('click');
        var obj = $(e).parents('tr').data();

        /*RegraNc*/
        if ($(e).parents('#level1_nc_accordion').length || $(e).parents('#level2_nc_accordion').length || $(e).parents('#level3_nc_accordion').length) {
            crudNxN.modalAlterarRegraNc(obj);
            $('#crudNxNEdit > div > div > div.modal-footer > button.btn.btn-primary').on('click', function () {
                var editado = crudNxN.retornaObjetoAlteradoRegraNc(obj);
                $(e).parents('tr').removeData();
                $(e).parents('tr').data(editado);
                $($(e).parents('tr').find('td')[0]).html(editado.ParNotConformityRule_Name)
                $($(e).parents('tr').find('td')[1]).html(editado.Value)
                $($(e).parents('tr').find('td')[2]).html(editado.IsReauditShowTable)
                $('#crudNxNEdit').modal('hide');
            });
        }
            /*Contadores*/
        else if ($(e).parents('#level1_counter_accordion').length || $(e).parents('#level2_counter_accordion').length || $(e).parents('#level3_counter_accordion').length) {
            crudNxN.modalAlterarContadores(obj);//
            $('#crudNxNEdit > div > div > div.modal-footer > button.btn.btn-primary').on('click', function () {
                var editado = crudNxN.retornaObjetoAlteradoContadores(obj);//
                $(e).parents('tr').removeData();
                $(e).parents('tr').data(editado);
                $($(e).parents('tr').find('td')[0]).html(editado.ParCounterName)//
                $($(e).parents('tr').find('td')[1]).html(editado.ParLocalName)//
                $('#crudNxNEdit').modal('hide');
            });
        }
            /*Reincidencia*/
        else if ($(e).parents('#level1_reincidencia_accordion').length || $(e).parents('#level2_reincidencia_accordion').length || $(e).parents('#level3_reincidencia_accordion').length) {
            crudNxN.modalAlterarReincidencia(obj);//
            $('#crudNxNEdit > div > div > div.modal-footer > button.btn.btn-primary').on('click', function () {
                var editado = crudNxN.retornaObjetoAlteradoReincidencia(obj);//
                $(e).parents('tr').removeData();
                $(e).parents('tr').data(editado);
                $($(e).parents('tr').find('td')[0]).html(editado.NcNumber)//
                $($(e).parents('tr').find('td')[1]).html(editado.EffectiveLength)//
                $($(e).parents('tr').find('td')[2]).html(editado.parFrequencyName)//
                $('#crudNxNEdit').modal('hide');
            });
        }
        else if ($(e).parents('#level1_cluster_accordion').length) {
            crudNxN.modalAlterarCluster(obj);//
            $('#crudNxNEdit > div > div > div.modal-footer > button.btn.btn-primary').on('click', function () {
                var editado = crudNxN.retornaObjetoAlteradoCluster(obj);//
                $(e).parents('tr').removeData();
                $(e).parents('tr').data(editado);
                $($(e).parents('tr').find('td')[0]).html(editado.Cluster_Name)//
                $($(e).parents('tr').find('td')[1]).html(editado.ParCriticalLevel_Name)//
                $($(e).parents('tr').find('td')[2]).html(editado.Points)//
                $('#crudNxNEdit').modal('hide');
            });
        }
        else if ($(e).parents('#level2_GroupLevel_accordion').length) {
            crudNxN.modalAlterarGroupLevel2(obj);//
            $('#crudNxNEdit > div > div > div.modal-footer > button.btn.btn-primary').on('click', function () {
                var editado = crudNxN.retornaObjetoAlteradoGroupLevel2(obj);//
                $(e).parents('tr').removeData();
                $(e).parents('tr').data(editado);
                $($(e).parents('tr').find('td')[0]).html(editado.Name)//
                $('#crudNxNEdit').modal('hide');
            });
        }
        $('#crudNxNEdit').modal();

    },

    modalAlterarRegraNc: function (obj) {

        $("#paramsDto_parLevel1Dto_parNotConformityRuleXLevelDto_IsReaudit").bootstrapSwitch('destroy', true);

        var divDeEdicao = $('#level1_nc_collapse > div > table.table-erp').clone();
        $('#crudNxNEdit > div > div > div.modal-body').append(divDeEdicao)/*append*/

        $('#crudNxNEdit #selectNotConformityRule').val(obj.ParNotConformityRule_Id);/*Preenche*/
        $("#crudNxNEdit .check-box").bootstrapSwitch('state', obj.IsReaudit);
        $('#crudNxNEdit #paramsDto_parLevel1Dto_parNotConformityRuleXLevelDto_Value').val(obj.Value);
        $('#crudNxNEdit button').not('#save').not('.close').not('.btn-default').remove();
        $("#paramsDto_parLevel1Dto_parNotConformityRuleXLevelDto_IsReaudit").bootstrapSwitch();

    },
    retornaObjetoAlteradoRegraNc: function (obj) {

        /*Valida se pode criar o objeto*/
        if ($('#crudNxNEdit #selectNotConformityRule :selected').val() <= 0) {
            alert("Por favor Selecione Regra de Não conformidade.");
            return;
        }

        if ($('#crudNxNEdit #paramsDto_parLevel1Dto_parNotConformityRuleXLevelDto_Value').val().replace(/[^0-9.]/g, '') == "") {
            alert("É necessário preencher o campo Valor da Regra de alerta.");
            return;
        }

        obj.ParNotConformityRule_Id = $('#crudNxNEdit #selectNotConformityRule :selected').val();
        obj.ParNotConformityRule_Name = $('#crudNxNEdit #selectNotConformityRule :selected').text();
        obj.Value = $('#crudNxNEdit #paramsDto_parLevel1Dto_parNotConformityRuleXLevelDto_Value').val();
        obj.IsReaudit = $('#crudNxNEdit #paramsDto_parLevel1Dto_parNotConformityRuleXLevelDto_IsReaudit').is(":checked");
        obj.IsReauditShowTable = $('#crudNxNEdit #paramsDto_parLevel1Dto_parNotConformityRuleXLevelDto_IsReaudit').is(":checked") ? "Sim" : "Não";

        return obj;
    },

    modalAlterarContadores: function (obj) {

        var divDeEdicao = $('#camposContador').clone();
        $('#crudNxNEdit > div > div > div.modal-body').append(divDeEdicao)/*append*/
        $('#crudNxNEdit #myModalLabel').html($('#tableContadores').attr('nameModal'));

        $('#crudNxNEdit #parCounter').val(obj.ParCounter_Id);/*Preenche*/
        $('#crudNxNEdit #parLocal').val(obj.ParLocal_Id);/*Preenche*/
        $('#crudNxNEdit button').not('#save').not('.close').not('.btn-default').remove();
    },
    retornaObjetoAlteradoContadores: function (obj) {

        /*Valida se pode criar o objeto*/
        if ($('#crudNxNEdit #parCounter :selected').val() <= 0) {
            alert("Por favor selecione o contador.");
            return;
        }

        if ($('#crudNxNEdit #parLocal').val() <= 0) {
            alert("Por favor selecione o local.");
            return;
        }

        obj.ParLocal_Id = $('#crudNxNEdit #parLocal :selected').val();
        obj.ParLocalName = $('#crudNxNEdit #parLocal :selected').text();
        obj.ParCounter_Id = $('#crudNxNEdit #parCounter :selected').val();
        obj.ParCounterName = $('#crudNxNEdit #parCounter :selected').text();

        return obj;
    },

    modalAlterarReincidencia: function (obj) {

        var divDeEdicao = $('#camposRencidencia').clone();
        $('#crudNxNEdit > div > div > div.modal-body').append(divDeEdicao)/*append*/
        $('#crudNxNEdit #myModalLabel').html($('#tableReincidencia').attr('nameModal'));

        $('#crudNxNEdit #selectFrequenciaReincidencia').val(obj.ParFrequency_Id);/*Preenche*/
        $('#crudNxNEdit #inputNumeroNC').val(obj.NcNumber);/*Preenche*/
        $('#crudNxNEdit #inputVigencia').val(obj.EffectiveLength);/*Preenche*/

    },
    retornaObjetoAlteradoReincidencia: function (obj) {
        /*Valida se pode criar o objeto*/
        ReincidenciaL1.veifyAdd('crudNxNEdit')
        ReincidenciaL1.getObjAdd('crudNxNEdit', obj);
        return obj;
    },

    modalAlterarCluster: function (obj) {

        var divDeEdicao = $('#camposCluster').clone();
        $('#crudNxNEdit > div > div > div.modal-body').append(divDeEdicao)/*append*/
        $('#crudNxNEdit #myModalLabel').html($('#camposCluster').attr('nameModal'));

        $('#crudNxNEdit #valueCluster').val(obj.ParCluster_Id).attr('disabled', true);/*Preenche*/
        $('#crudNxNEdit #valCrit').val(obj.ParCriticalLevel_Id);/*Preenche*/
        $('#crudNxNEdit #pontosCluster').val(obj.Points);/*Preenche*/
        $('#crudNxNEdit button').not('#save').not('.close').not('.btn-default').remove();
    },
    retornaObjetoAlteradoCluster: function (obj) {
        /*Valida se pode criar o objeto*/
        ClusterL1.veifyAdd('crudNxNEdit')
        ClusterL1.getObjAdd('crudNxNEdit', obj);
        return obj;
    },

    modalAlterarGroupLevel2: function (obj) {

        var divDeEdicao = $('#camposGroup').clone();
        $('#crudNxNEdit > div > div > div.modal-body').append(divDeEdicao)/*append*/
        $('#crudNxNEdit #myModalLabel').html($('#tableGroupLevel').attr('nameModal'));

        $('#crudNxNEdit #inputParLevel03Name').val(obj.Name);/*Preenche*/
        $('#crudNxNEdit button').not('#save').not('.close').not('.btn-default').remove();

    },
    retornaObjetoAlteradoGroupLevel2: function (obj) {
        /*Valida se pode criar o objeto*/
        GroupL2.veifyAdd('crudNxNEdit')
        GroupL2.getObjAdd('crudNxNEdit', obj);
        return obj;
    },

}