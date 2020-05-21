function ModalOpcoesEstrategico(title, IdParaNovaAcao, callback) {

    IdBotaoClicado = IdParaNovaAcao;

    $('#modalLindo').modal();
    $('#modalLindo').find('.modal-body').empty();

    if (title != undefined) {
        $('#Header').html(title);
    } else {
        $('#Header').html(Resources('planning'));
    }

    if (callback != undefined) {
        callback();
        return false;
    }

    /*Botões*/
    $('#modalLindo').find('.modal-footer button').hide();
    $('#Fechar').show();

    if (isTaticoClicked) {
        $('#TaticoVinculado').show();
    }
    else if (isNovaAcao) {

        $('#NovaAcao').show();

        if (planejamentoCorrentId > 0) {
            $('#BuscarPlanejamentos').show();
        }

        if (IdParaNovaAcao != undefined || planejamentoCorrentId > 0) { // Mostra Detalhes + BTN nova acao
            $.get(PlanejamentoDetalhes, { id: planejamentoCorrentId }, function (r) {

                var modal = $('#modalLindo').find('.modal-body').html();

                $('#modalLindo').find('.modal-body').empty();
                $('#modalLindo').append(r);
                $('#NovaAcao').show();

            });
        } else {
            $('#modalLindo').find('.modal-body').empty();
        }
    }
    else {
        $('#NovoPlanejamento').show();
    }

    $('#Fechar').show();
    $('#Salvar').show();

    /*Fim Botões*/

}

function NovoPlanejamento() {//Pos Modal2

    var title = $('#Header').html();
    var title2 = " > " + Resources('_new');

    $('#Header').html(title + title2);

    $.get(urlNovoPlanejamento, function (r) {
        $('#modalLindo').find('.modal-body').empty().append(r);
        if (isTaticoClicked) {
            $('.novoItem ').hide();

            DdlChangeTatico();

        } else {
            $('.novoItem ').show();

            $('#Planejamento > table > tbody > tr .tatico').parents("tr").hide();

            //$('#Planejamento > table > tbody > tr:nth-child(2)').hide();
            //$('#Planejamento > table > tbody > tr:nth-child(3)').hide();
            //$('#Planejamento > table > tbody > tr:nth-child(11)').hide();
            //$('#Planejamento > table > tbody > tr:nth-child(12)').hide();
            //$('#Planejamento > table > tbody > tr:nth-child(13)').hide();
            //$('#Planejamento > table > tbody > tr:nth-child(14)').hide();
            //$('#Planejamento > table > tbody > tr:nth-child(15)').hide();
            //$('#Planejamento > table > tbody > tr:nth-child(16)').hide();
            //$('#Planejamento > table > tbody > tr:nth-child(17)').hide();
            //$('#Planejamento > table > tbody > tr:nth-child(18)').hide();
            //$('#Planejamento > table > tbody > tr:nth-child(19)').hide();
            //$('#Planejamento > table > tbody > tr:nth-child(20)').hide();
            //$('#Planejamento > table > tbody > tr:nth-child(21)').hide();

            DdlChangeEstrategico();

            setTimeout(function () {
                $('#Planejamento select').select2(configSelect2);
            }, 500);

        }

        InitDatePiker();
        MoneyMask();

        MoneyMask('R$ ');

        $('.money').each(function () {
            $(this).maskMoney('mask', $(this).val());
        });

    });

    /*Botões*/
    $('#modalLindo').find('.modal-footer button').hide()
    $('#Fechar').show();
    $('#Salvar').show();

    $('#Salvar').off('click').on('click', SalvarPlanejamento);

    /*Fim Botões*/
}

function SalvarPlanejamento(form) {

    if (!isClickedTaticoVinculado) {
        isClickedEstrategico = true;
    }

    var sendObj = $('#Planejamento').serializeObject();

    sendObj.IsActive = true;

    let isValid = $('#Planejamento').find('.error').length === 0;
    if (!isValid) {
        alert(Resources('required_fields'));
        return
    }

    if (isClickedTaticoVinculado)
        sendObj['IsTAtico'] = true;

    if (sendObj !== undefined) {
        $.post(urlSalvarPlanejamento, sendObj, function (r) {

            planejamentoCorrentId = parseInt(r.Id);
            openMessageModal(Resources('registry_successfully_saved'), '');
            $('#modalLindo').modal('hide');
            $('#btnTop').click();

        }).fail(function (e, h, x) {
            openMessageModal(Resources('could_not_save_record'), e.responseJSON.Message);
        });
    }

}

function SalvarPlanejamentoEditado(form) {
    
    var sendObj = {};

    sendObj = $(form).serializeObject();

    if (isClickedTaticoVinculado) {
        sendObj['IsTAtico'] = true;
        sendObj.IsActive = sendObj.IsActive_Tatico;
    }


    let isValid = ($(form).find('.error:visible').length === 0);

    if (!isValid) {
        alert(Resources('required_fields'))
        return;
    }

    $.post(urlSalvarPlanejamento, sendObj, function (r) {

        planejamentoCorrentId = parseInt(r.Id);
        openMessageModal(Resources('registry_successfully_saved'), '');
        $('#modalLindo').modal('hide');
        $('#btnTop').click();


    }).fail(function (e, h, x) {

        openMessageModal(Resources('could_not_save_record'), e.responseJSON.Message);
    });

}

function EditarPlanejamento(model) {

    var title = $('#Header').html();
    var title2 = " > Novo";

    if (isTaticoClicked) {
        $('#Header').html(Resources('tactical_planning'));
    } else {
        $('#Header').html(title + title2);
    }

    $.get(urlNovoPlanejamento, function (r) {

        $('#modalLindo').find('.modal-body').empty().append(r);

        if (isTaticoClicked) {

            $('.novoItem ').hide();

            if (isClickedTaticoVinculado) {

                $('#Planejamento > #IsTatico').val(true);
                $('#Planejamento > table > tbody > tr .estrategico').attr('disabled', true);

                if (model !== undefined) {

                    $('#Estrategico_Id').val(model.Id);
                    if (model.Diretoria_Id > 0)
                        $('#Planejamento #Diretoria_Id').val(model.Diretoria_Id);
                    if (model.Missao_Id > 0)
                        $('#Planejamento #Missao_Id').val(model.Missao_Id);
                    if (model.Visao_Id > 0)
                        $('#Planejamento #Visao_Id').val(model.Visao_Id);
                    if (model.TemaAssunto_Id > 0)
                        $('#Planejamento #TemaAssunto_Id').val(model.TemaAssunto_Id);
                    if (model.Dimensao_Id > 0)
                        $('#Planejamento #Dimensao_Id').val(model.Dimensao_Id);
                    if (model.Objetivo_Id > 0)
                        $('#Planejamento #Objetivo_Id').val(model.Objetivo_Id);
                    if (model.IndicadoresDiretriz_Id > 0)
                        $('#Planejamento #IndicadoresDiretriz_Id').val(model.IndicadoresDiretriz_Id);
                    if (model.Responsavel_Diretriz > 0)
                        $('#Planejamento #Responsavel_Diretriz').val(model.Responsavel_Diretriz);

                    $('#Planejamento #IsActive').val(model.IsActive);
                    $('#Planejamento #IsActive').prop("checked", model.IsActive);

                }

                $('.novoTatico').show();
                DdlChangeTatico(model);

            } else {

                $('#Planejamento > table > tbody > tr .estrategico').parents("tr").hide();
                DdlChangeEstrategico();
            }

        } else {

            $('.novoItem ').show();
            $('#Planejamento > table > tbody > tr .tatico').parents("tr").hide();
        }

        InitDatePiker();
        MoneyMask();

        MoneyMask('R$ ');

        $('.money').each(function () {
            $(this).maskMoney('mask', $(this).val());
        });

        $('#UnidadeDeMedida_Id').trigger('change');

    });

    /*Botões*/

    $('#modalLindo').find('.modal-footer button').hide()
    $('#Fechar').show();
    $('#Salvar').show();
    $('#Salvar').off('click').on('click', SalvarPlanejamento);
    /*Fim Botões*/
}

function DdlChangeEstrategico(v1, v2, v3, edit) {

    var change1 = v1 != undefined ? v1 : "";
    var change2 = v2 != undefined ? v2 : "";

    change1 = change1 == 0 ? "" : change1;
    change2 = change2 == 0 ? "" : change2;

    v3 = v3 == 0 ? "" : v3;

    let Dimensao_Id = $('#Dimensao_Id')
    let Objetivo_Id = $('#Objetivo_Id')

    var rdy = true;

    Dimensao_Id.off('change').on('change', function () {

        var valor = $(this).val();
        var form = $(this).parents('form');

        rdy = false;

        if (valor) {

            $.get(GETObjetivo, { id: valor }, function (r) {
                $('#Objetivo_Id').empty().html(r).attr('disabled', false);
                $('#Objetivo_Id').parent().parent().find('.novoItem').attr('disabled', false);
                rdy = true;
            })
        } else {

            $('#Objetivo_Id').val("").change().attr('disabled', true);
            $('#Objetivo_Id').parent().parent().find('.novoItem').attr('disabled', true);
            $('#IndicadoresDiretriz_Id').val("").change().attr('disabled', true);
            $('#IndicadoresDiretriz_Id').parent().parent().find('.novoItem').attr('disabled', true);
        }
    });

    Objetivo_Id.off('change').on('change', function () {

        var valor = $(this).val();
        var form = $(this).parents('form');

        if (valor) {
            $.get(GETIndicadoresDiretriz, { id: valor }, function (r) {
                $('#IndicadoresDiretriz_Id').empty().html(r).attr('disabled', false);
                $('#IndicadoresDiretriz_Id').parent().parent().find('.novoItem').attr('disabled', false);
            });
        }
        else {
            $('#IndicadoresDiretriz_Id').val("").change().attr('disabled', true);
            $('#IndicadoresDiretriz_Id').parent().parent().find('.novoItem').attr('disabled', true);
        }
    });

    $('#Dimensao_Id').val(change1).change();

    if (v2 != undefined) {
        setTimeout(function () { $('#Objetivo_Id').val(v2).change(); }, 1500)
    } else {

        setTimeout(function () { $('#Objetivo_Id').val(change2).change(); }, 2000)
    }

    if (v3 != undefined) {
        setTimeout(function () { $('#IndicadoresDiretriz_Id').val(v3).change(); }, 3000)
    }

}

function DdlChangeTatico(model) {
    changeChainJsWithHTMLResponseGET('#formEditTatico #Gerencia_Id', GETCoordenacaoByGerencia, '#formEditTatico #Coordenacao_Id', function () {

    })
    changeChainJsWithHTMLResponseGET('#Planejamento #Gerencia_Id', GETCoordenacaoByGerencia, '#Planejamento #Coordenacao_Id', function () {
    })

    $('#Iniciativa_Id').off('change').on('change', function () {
        var Iniciativa_Id = $('#Iniciativa_Id').val() > 0 ? $('#Iniciativa_Id').val() : 0;
        manageButton(Iniciativa_Id, 'IndicadoresDeProjeto_Id', 'Selecione uma predecessor em Projeto / Iniciativa para Inserir / Gerenciar os Indicadores do Projeto / Iniciativa', 'Gerenciar Indicadores do Projeto / Iniciativa');

        $.get(GETIndicadoresProjetoIniciativa, { id: Iniciativa_Id }, function (r) {
            $('#IndicadoresDeProjeto_Id').parent().html('').append(r);
            $('#IndicadoresDeProjeto_Id').off('change').on('change', function () {

                var Objetivo_Id = $('#IndicadoresDeProjeto_Id').val() > 0 ? $('#IndicadoresDeProjeto_Id').val() : 0;
                manageButton(Objetivo_Id, 'ObjetivoGerencial_Id', 'Selecione uma predecessor em Indicadores do Projeto / Iniciativa para Inserir / Gerenciar os Indicadores do Objetivo Gerencial', 'Gerenciar Objetivo Gerencial');

                $.get(GETObjetivosGerenciais, { id: Objetivo_Id }, function (r) {

                    $('#ObjetivoGerencial_Id').parent().html('').append(r);
                    $('#ObjetivoGerencial_Id').addClass('tatico');

                    try {
                        $('#modalLindo span.select2').prev('select').select2('destroy');
                    } catch (e) {
                    }
                    finally {
                        $('#modalLindo select').select2(configSelect2);
                    }

                });
            });

            $('#IndicadoresDeProjeto_Id').change();

        });
    });
    $('#Iniciativa_Id').change();


}

function changeChainJsWithHTMLResponseGET(idMaster, urlGETChildren, idChildren, masterCB, childrenSelected) {
    let masterEl = $(idMaster)
    let masterElVal = $(idMaster).val()
    manageButtonDisabledWhenHaveParentSelect(masterElVal, idChildren, Resources('select_a_predecessor_above'), '');
    masterEl.off('change').on('change', function () {
        let seletedId = masterEl.val() > 0 ? masterEl.val() : 0;
        manageButtonDisabledWhenHaveParentSelect(seletedId, idChildren, Resources('select_a_predecessor_above'), '');
        $.get(urlGETChildren, { id: seletedId }, function (response) {
            let childrenEl = $(idChildren)
            childrenEl.parent().html('').append(response);
            childrenEl.remove();
            $(idChildren).select2(configSelect2);

            if (masterCB)
                masterCB()
        })
    })
}

function manageButtonDisabledWhenHaveParentSelect(parentSelectedValue, elementChild, messageForDisabled, messageForEnabled) {
    let elem = $(elementChild)
    if (elem.length > 0)
        if (parentSelectedValue == 0) {
            elem.parents('tr').find('.novoItem').attr('disabled', true)
            elem.parents('tr').find('.novoItem').attr('title', messageForDisabled)
        }
        else {
            elem.parents('tr').find('.novoItem').attr('disabled', false)
            elem.parents('tr').find('.novoItem').attr('title', messageForEnabled)
        }
}

function manageButton(id, idChildren, messageDisabled, messageEnabled) {
    if (id == 0) {
        $('#' + idChildren).parents('tr').find('.novoItem').attr('disabled', true)
        $('#' + idChildren).parents('tr').find('.novoItem').attr('title', messageDisabled)
    }
    else {
        $('#' + idChildren).parents('tr').find('.novoItem').attr('disabled', false)
        $('#' + idChildren).parents('tr').find('.novoItem').attr('title', messageEnabled)
    }
}

function getPlanEstrat(data, a, b) {
    $.get(urlEditPlanEstrat, { id: data.Tatico_Id, isTatico: true }, function (r) {
        TesteRenan(r, 'Tatico', data);
        $('#modalNovo > div > div > div.modal-footer > button').show();

        //DDLs ENCADEADAS da PARTIAL VIEW
        changeChainJsWithHTMLResponseGET('#formEditTatico #Gerencia_Id', GETCoordenacaoByGerencia, '#formEditTatico #Coordenacao_Id', function () { })

        changeIniciativa();

        changeIndicadoresProjetos();

        $('.UnidadeDeMedidaTatico').change();

        if (data.Acao && data.Acao.Id > 0) {
            getAcao(data, a, b);
        }

    });
}

function changeIniciativa() {

    $('#formEditTatico').on('change', '#Iniciativa_Id', function () {

        var Iniciativa_Id = $('#formEditTatico #Iniciativa_Id').val() > 0 ? $('#formEditTatico #Iniciativa_Id').val() : 0;

        manageButton(Iniciativa_Id, 'formEditTatico #IndicadoresDeProjeto_Id', 'Selecione uma predecessor em Projeto / Iniciativa para Inserir / Gerenciar os Indicadores do Projeto / Iniciativa', 'Gerenciar Indicadores do Projeto / Iniciativa');

        $.get(GETIndicadoresProjetoIniciativa, { id: Iniciativa_Id }, function (r) {

            $('#formEditTatico #IndicadoresDeProjeto_Id').parent().html('').append(r);

             $('#formEditTatico #IndicadoresDeProjeto_Id').select2(configSelect2);

            $('#formEditTatico #IndicadoresDeProjeto_Id').change();
        });
    });
}

function changeIndicadoresProjetos() {

    $('#formEditTatico').on('change', '#IndicadoresDeProjeto_Id', function () {

        var Objetivo_Id = $('#formEditTatico #IndicadoresDeProjeto_Id').val() > 0 ? $('#formEditTatico #IndicadoresDeProjeto_Id').val() : 0;

        manageButton(Objetivo_Id, 'formEditTatico #ObjetivoGerencial_Id', 'Selecione uma predecessor em Indicadores do Projeto / Iniciativa para Inserir / Gerenciar os Indicadores do Objetivo Gerencial', 'Gerenciar Objetivo Gerencial');

        $.get(GETObjetivosGerenciais, { id: Objetivo_Id }, function (r) {

            $('#formEditTatico #ObjetivoGerencial_Id').parent().html('').append(r);

            $('#formEditTatico #ObjetivoGerencial_Id').addClass('tatico');

        });
    });
}

//Editar Planejamento
function TesteRenan(r, tipo, model) {

    var form = $(r);

    if (tipo === 'Tatico') {

        form.find("table > tbody > tr .estrategico").attr('disabled', true);

        form.attr('id', 'formEditTatico');

        form.find('button').each(function (o, c) {
            if ($(c).attr('onclick').split(',')[1]) {
                let before = $(c).attr('onclick').split(',')[1]
                before = before.slice(2, before.length)
                let after = `Novo(this, 'formEditTatico #${before}`
                $(c).attr('onclick', after)
            }
        });

        $('#modalLindo').find('.modal-body .content2').append(form);

        let btnChangeTatico = "";
        let btnSaveTatico = `<button type='button' class='btn btn-primary showAsEstrategy' id='save' onclick="isClickedEstrategico=false;isClickedTaticoVinculado=true;SalvarPlanejamentoEditado($('#formEditTatico'))">${Resources('save_tactical')}</button> | `;

        if (model.Acao && model.Acao.Id > 0)
            btnChangeTatico = `<button type='button' class='btn btn-warning' onclick="changePlanejamento('acao',$('#Acao'))" data-toggle='modal' data-target='#modalChangeEstrategico'>${Resources('change_tactical_link')}</button>`;

        $('#modalLindo').find('.modal-body .content2').append(btnSaveTatico + btnChangeTatico + "<hr>");

        myfunction();

    } else {

        form.find('#Estrategico_Id').remove();
        form.find('#IsTatico').val(false).change();

        form.find('table > tbody > tr .tatico').parents("tr").hide();

        form.attr('id', 'formEditEstrategy');

        form.find('button').each(function (o, c) {
            if ($(c).attr('onclick').split(',')[1]) {
                let before = $(c).attr('onclick').split(',')[1]
                before = before.slice(2, before.length)
                let after = `Novo(this, 'formEditEstrategy #${before}`
                $(c).attr('onclick', after)
            }
        })

        if (IsAdmin) {
            $('#modalLindo').find('.modal-body .content1').append(form);
        }

        let btnChangeEstrategico = "";
        let btnSalvar = "";

        if (model.Tatico_Id > 0)
            btnChangeEstrategico = `<button type='button' class='btn btn-warning' onclick="changePlanejamento('tatico',$('#formEditTatico'))" data-toggle='modal' data-target='#modalChangeEstrategico'>${Resources('change_strategic_link')}</button>`;

        if (IsAdmin) {
            btnSalvar = `<button type="button" class="btn btn-primary showAsEstrategy" id="Salvar" onclick="isClickedEstrategico=true;isClickedTaticoVinculado=false;SalvarPlanejamentoEditado($('#formEditEstrategy'))">${Resources("save_strategic")}</button> | `;
        }

        $('#modalLindo').find('.modal-body .content2').append(btnSalvar + btnChangeEstrategico + "<hr>");

        DdlChangeEstrategico(model.Dimensao_Id, model.Objetivo_Id, model.IndicadoresDiretriz_Id, true);

    }

    InitDatePiker();
    MoneyMask();

    MoneyMask('R$ ');

    $('.money').each(function () {
        $(this).maskMoney('mask', $(this).val());
    });

    DisabilitaBotaoGerenciar();

    setTimeout(function () {
        $('#modalLindo select').select2(configSelect2);
    }, 500)
}

function changePlanejamento(tipo, form) {

    $.post(urlGetListPlanejamento + '/' + tipo, function (r) {

        var col = r.columns;

        col.unshift({
            "render": function (data, type, row) {
                return '<button data-id="' + row.Id + '" class="btn btn-large btn-primary">' + Resources('select') + '</button>';
            }
        });

        datatableGRT.Inicializar({
            idTabela: "tablePessego",
            listaDeDados: r.datas,
            colunaDosDados: r.columns,
            initComplete: function () {

                $('#tablePessego').off().on('click', 'button[data-id]', function () {

                    if (confirm("Deseja alterar o vinculo!?")) {
                        var idParaMudar = $(this).attr("data-id");
                        var id = $(form).find('#Id').val();

                        $.post('@Url.Action("Cereja", "api/Pa_Planejamento")/' + tipo + '/' + id + '/' + idParaMudar, function (r) {
                            openMessageModal(Resources('updated_link'));
                            location.reload();
                        });
                    }
                });

                setTimeout(function (e) {
                    var oTable = $('#tablePessego').dataTable();
                    if (oTable.length > 0) {
                        oTable.fnAdjustColumnSizing();
                    }
                }, 1400);
            }
        });
    });
}

function AbreModalDePlanejamento(id) {

    Clicked(true, false, true);

    $.get(urlGetPlanejamento, { id: id }, function (r) {

        ModalOpcoesEstrategico("Novo Planejamento Tático Vinculado", 0, function () {
            EditarPlanejamento(r);
        });

    });
}