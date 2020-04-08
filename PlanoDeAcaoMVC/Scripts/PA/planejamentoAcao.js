/*Acao*/
var addButton = (function () {
    var counter = 0;

    return function () {
        var objectContainer = $('#obj:hidden');
        var header = objectContainer.clone();
        counter += 1;
        var collapseId = "collapsible_obj_" + counter;

        header.find(".collapse").attr("id", collapseId);
        header.find("button[data-toggle='collapse']").attr("data-target", "#" + collapseId);
        header.find("button[data-toggle='collapse']").html(Resources('new_action'))

        header.appendTo('#final');
        header.show();

        setTimeout(function () {
            header.find("select.select2-hidden-accessible").next('.select2').remove();
            header.find("select.select2-hidden-accessible").removeClass('select2-hidden-accessible');
            header.find("select:visible").select2({
                matcher: matchCustom
            });
            $('.UnidadeMedida').change();

            $('[name="TipoIndicador"]').each(function () {
                $(this).select2({
                    matcher: matchCustom
                });
            });

        }, 1);

        myfunction();
        InitDatePiker();
    };

})();

$('#new').click(addButton);

$('#modalLindo').on('change', '.UnidadeMedida', function (e) {

    let unidadeMedida = $(this).val();
    let valor = $(this).parent().parent().parent().find(".QuantoCusta").val();
    let inputQuantoCusta = $(this).parent().parent().parent().find(".QuantoCusta");

    if (unidadeMedida == 1) {

        $(inputQuantoCusta).get(0).type = 'text';

        let config = { prefix: 'R$ ', allowNegative: true, thousands: '.', decimal: ',', affixesStay: true, allowZero: true }

        $(inputQuantoCusta).maskMoney(config);
        $(inputQuantoCusta).maskMoney('mask');

    } else {

        $(inputQuantoCusta).maskMoney('destroy');

        if (valor != "" && valor != NaN)
            $(inputQuantoCusta).val(parseFloat(valor.replace("R$ ", "")));
        else
            $(inputQuantoCusta).val(valor);

        $(inputQuantoCusta).on("keypress", function (event) {

            if (unidadeMedida == 2 || unidadeMedida == 3) {

                var $this = $(this);

                if (($this.val().indexOf(',') != -1 || $this.val().indexOf('.') != -1) &&
                    ((event.which < 48 || event.which > 57) &&
                        (event.which != 0 && event.which != 8))) {
                    event.preventDefault();
                }
            }
        });
    }
});

$('#modalLindo').on('change', '.UnidadeDeMedidaTatico', function (e) {

    let unidadeMedida = $(this).val();
    let valorDe = $(this).parent().parent().parent().find(".ValorDe").val();
    let valorPara = $(this).parent().parent().parent().find(".ValorPara").val();
    let inputValorDe = $(this).parent().parent().parent().find(".ValorDe");
    let inputValorPara = $(this).parent().parent().parent().find(".ValorPara");

    if (unidadeMedida == 1) {

        let config = { prefix: 'R$ ', allowNegative: true, thousands: '.', decimal: ',', affixesStay: true, allowZero: true }

        $(inputValorDe).maskMoney(config);
        $(inputValorDe).maskMoney('mask');
        $(inputValorPara).maskMoney(config);
        $(inputValorPara).maskMoney('mask');

        $(inputValorDe).prop("disabled", false);
        $(inputValorPara).prop("disabled", false);

    } else if (unidadeMedida == "" || unidadeMedida == null || unidadeMedida == undefined || unidadeMedida == NaN) {

        $(inputValorDe).maskMoney('destroy');
        $(inputValorPara).maskMoney('destroy');


        $(inputValorDe).val("").prop("disabled", true);
        $(inputValorPara).val("").prop("disabled", true);
    }
    else {

        $(inputValorDe).maskMoney('destroy');

        if (valorDe != "" && valorDe != NaN)
            $(inputValorDe).val(parseFloat(valorDe.replace("R$ ", "").replace(",", ".")));
        else
            $(inputValorDe).val(valorDe);

        $(inputValorPara).maskMoney('destroy');

        if (valorPara != "" && valorPara != NaN)
            $(inputValorPara).val(parseFloat(valorPara.replace("R$ ", "").replace(",", ".")));
        else
            $(inputValorPara).val(valorPara);

        $(inputValorDe).prop("disabled", false);
        $(inputValorPara).prop("disabled", false);

        $(inputValorDe).on("keypress", function (event) {

            if (unidadeMedida == 2 || unidadeMedida == 3) {

                var $this = $(this);

                if (event.which == 46 || event.which == 44) {//Caso for ponto ou virgula
                    if ($this.val().indexOf(',') != -1 || $this.val().indexOf('.') != -1) {//se já existir um ponto ou uma virgula
                        event.preventDefault();
                    }
                } else {
                    if ((event.which < 48 || event.which > 57) && (event.which != 0 && event.which != 8)) {//se não for numero
                        event.preventDefault();
                    }
                }
            }
        });

        $(inputValorPara).on("keypress", function (event) {

            if (unidadeMedida == 2 || unidadeMedida == 3) {

                var $this = $(this);

                if (event.which == 46 || event.which == 44) {
                    if ($this.val().indexOf(',') != -1 || $this.val().indexOf('.') != -1) {
                        event.preventDefault();
                    }
                } else {
                    if ((event.which < 48 || event.which > 57) && (event.which != 0 && event.which != 8)) {
                        event.preventDefault();
                    }
                }
            }
        });
    }
});

function ModalAcao(isOpenModal, isFukingNewClicked, isTableClick) {
    if (!$('#body > table').length) {
        ModalOpcoesEstrategico('Planejamento Operacional', planejamentoCorrentId, function () {
            if (isTableClick) {
                $.get(PlanejamentoDetalhes, { id: planejamentoCorrentId }, function (r) {
                    $('#modalLindo').find('.modal-body').empty().append(r);
                });
            }
        });
    }

    if (isFukingNewClicked) {

        if (isOpenModal)
            $('#modalLindo').modal();

        $('#Header').html(Resources('new_action'));

        $.get(urlNovaAcao, function (r) {

            $('#modalLindo').find('.modal-body').append('<hr>');
            $('#modalLindo').find('.modal-body').append(r);
            $('#GrupoCausa_Id').empty().attr('disabled', true);
            $('#ContramedidaGenerica_Id').empty().attr('disabled', true);

            myfunction();
            MoneyMask('R$ ');

            $('.money').each(function () {
                $(this).maskMoney('mask', $(this).val());
            });

            $('#new').click();
            if (isFukingNewClicked) {
                $('#new').show();
            } else {
                $('#new').hide();
            }

        });

        /*Botões*/
        $('#modalLindo').find('.modal-footer button').hide()
        $('#Fechar').show();
        $('#Salvar').show();
        $('#Salvar').off('click').on('click', SalvarAcao);
        /*Fim Botões*/
    }
}

function EditarAcao(html, data) {
    var acao = data.Acao;

    $('#modalLindo').modal();
    $('#Header').html(Resources('edit_action'));
    $('#modalLindo').find('.modal-body').empty().append(html);
    $('#modalLindo').find('.modal-footer button').hide();
    myfunction();

    $('#CausaGenerica_Id').val(acao.CausaGenerica_Id).change();
    $('[name="_QuandoInicio"]').val(acao._QuandoInicio.split(" ")[0]);
    $('[name="_QuandoFim"]').val(acao._QuandoFim.split(" ")[0]);
    $('#Status').val(acao.Status);
    MoneyMask();
    $('#_QuantoCusta').val(acao.QuantoCusta);
    $('#_QuantoCusta').trigger('mask.maskMoney');
    $('#_QuantoCusta').trigger('blur');

    InitDatePiker();

    MoneyMask('R$ ');

    $('.money').each(function () {
        $(this).maskMoney('mask', $(this).val());
    });

}

function myfunction() {
    $('[id=tipoIndicador]').off('change').on('change', function () {

        var valor = $(this).val();
        var form = $(this).parents('form');

        form.find('.IndicadorOutros').hide();
        form.find('.IndicadorSgq').hide();

        if (valor == 1) {
            form.find('.IndicadorOutros').show();
            form.find('.ProbDesv').hide();
            form.find('.ProbDesv').val("");
            form.find('.IndicadorOutros select').select2({
                matcher: matchCustom
            });
        } else if (valor == 2) {
            form.find('.IndicadorSgq').show();
            form.find('.ProbDesv').show();
            form.find('.IndicadorSgq select').select2({
                matcher: matchCustom
            });
        }
    });

    $('[id=CausaGenerica_Id]').off('change').on('change', function () {
        var valor = $(this).val();
        var form = $(this).parents('form');
        if (valor) {
            $.get(GETGrupoCausa, { id: valor }, function (r) {
                form.find('#GrupoCausa').empty().html(r);
                $('.showAsEstrategy select').select2({
                    matcher: matchCustom
                });
            });
            $.get(GETContramedidaGenerica, { id: valor }, function (rr) {
                form.find('#ContramedidaGenerica').empty().html(rr);
                $('.showAsEstrategy select').select2({
                    matcher: matchCustom
                });
            });
        }
        else {
            form.find('#GrupoCausa_Id').empty().attr('disabled', true);
            form.find('#ContramedidaGenerica_Id').empty().attr('disabled', true);
            $('.showAsEstrategy select').select2({
                matcher: matchCustom
            });
        }
    });
}

function SalvarAcao() {
    var listaAcoes = [];
    $('.Acao').not(':first').each(function (c, o) {
        listaAcoes.push(SalvarAcaoAvulsa(o));
    });

    let isValid = $('.Acao').not(':first').find('.error').length == 0

    if (ValidaAcao(listaAcoes) && isValid) {

        $.post(urlSalvarAcao, { "": listaAcoes }, function (r) {

            if (listaAcoes[0].isValid == '0') { return }

            $.post(urlGetPlanejamentoAcao, {}, function (r) {

                jaLiberouCallback = true;
                $('#modalLindo').modal('hide');
                openMessageModal(Resources('registry_successfully_saved'), '');
                $('#btnTop').click();

            }).fail(function (e, h, x) {

                openMessageModal(Resources('could_not_save_record'), e.responseJSON.Message);

            }).always(function () {

            });
        })
            .fail(function (e, h, x) {

                openMessageModal(Resources('could_not_save_record'), e.responseJSON.Message);

            });
    } else
        alert(Resources('required_fields'));

}

function ValidaAcao(listaAcoes) {

    let dataIsValid = true;

    $.each(listaAcoes, function (i, o) {

        let parts = o._QuandoFim.split('/');

        let DataInicio = new Date(parts[2], parts[1], parts[0]);

        parts = o._QuandoInicio.split('/');

        let DataFim = new Date(parts[2], parts[1], parts[0]);

        if (DataInicio < DataFim) {
            alert(Resources('start_date_must_be_less_than_end_date'));
            dataIsValid = false;
            return;
        }
    });

    if (listaAcoes[0] != undefined && dataIsValid) {
        return true;
    } else {
        return false;
    }
}

function SalvarAcaoEditada() {
    var listaAcoes = [];

    $('#Status').prop("disabled", false);

    $('.Acao').each(function (c, o) {

        listaAcoes.push(SalvarAcaoAvulsa(o, true));

    });

    let isValid = $('.Acao').find('.error').length == 0;
    if (!isValid) {
        alert(Resources('required_fields'))
        return;
    }

    if (!ValidaAcao(listaAcoes)) {
        return;
    }

    $.post(urlSalvarAcao, { "": listaAcoes }, function (r) {
        $.post(urlGetPlanejamentoAcao, {}, function (r) {

            jaLiberouCallback = true;
            $('#modalLindo').modal('hide');
            $('.modal-backdrop').remove();
            openMessageModal(Resources('registry_successfully_saved'), '');

            $('#btnTop').click();

        }).fail(function (e, h, x) {

            openMessageModal(Resources('could_not_save_record'), e.responseJSON.Message);

        }).always(function () {

        });
    })
        .fail(function (e, h, x) {

            openMessageModal(Resources('could_not_save_record'), e.responseJSON.Message);

        });

}

var jaLiberouCallback = false;

function SalvarAcaoAvulsa(o, isEdit) {

    var sendObj = $(o).serializeObject();

    if (sendObj != undefined) {
        if (sendObj.TipoIndicador == 1) {

            sendObj.Level1Id = 0;
            sendObj.Level2Id = 0;
            sendObj.Level3Id = 0;

        } else if (sendObj.TipoIndicador == 2) {

            sendObj.Pa_IndicadorSgqAcao_Id = 0;
        }
    }

    if (isEdit == undefined && sendObj != undefined) {
        sendObj['Panejamento_Id'] = planejamentoCorrentId;
    }

    return sendObj;

}

function RemoveAcao(btn) {
    $(btn).parents('#obj').remove();
}
/*Fim Acao*/

function Adicionar(btn) {

    var $tr = $(btn).closest('.tr_clone');
    var $clone;

    $clone = $tr.clone();
    $clone.find(':text').val('');

    $tr.find('#remove').show();
    $tr.find('#adicionar').hide();
    $tr.after($clone);

}

function Remover(btn) {
    $(btn).parents('tr').remove();
}

function getPlanOp(data, a, b) {

    $.get(urlEditPlanOp, { id: data.Id }, function (r) {

        TesteRenan(r, 'Estrategico', data);
        $('#modalNovo > div > div > div.modal-footer > button').show();

        if (data.Estrategico_Id > 0) {
            getPlanEstrat(data, a, b);
        }
    });

}

function getAcao(data) {

    $.get(urlEditAcao, { id: data.Acao.Id }, function (r) {
        var acao = data.Acao;

        $('#Header').html(Resources('edit_action'));

        $('#modalLindo').find('.modal-footer button').hide();
        $('#modalLindo').find('.modal-body .content3').append(r);

        $('#modalLindo').find('.modal-body .content3').find('#CausaGenerica_Id').val(acao.CausaGenerica_Id).change();
        $('#modalLindo').find('.modal-body .content3').find('[name="_QuandoInicio"]').val(acao._QuandoInicio.split(" ")[0]);
        $('#modalLindo').find('.modal-body .content3').find('[name="_QuandoFim"]').val(acao._QuandoFim.split(" ")[0]);
        $('#modalLindo').find('.modal-body .content3').find('#Status').val(acao.Status).prop('disabled', true);;
        $('#modalLindo').find('.modal-body .content3').find('[name="_QuantoCusta"]').val(acao.QuantoCusta);
        $('#modalLindo').find('.modal-body .content3').find('[name="_QuantoCusta"]').trigger('mask.maskMoney');
        $('#modalLindo').find('.modal-body .content3').find('[name="_QuantoCusta"]').trigger('blur');

        setTimeout(function () {
            $('#modalLindo').find('.modal-body .content3').find('#tipoIndicador').val(acao.TipoIndicador).change();
            $('.content3 select').select2({
                matcher: matchCustom
            });
            $('.UnidadeMedida').change();

            InitDatePiker();
        }, 1);

        $(r).attr("id", "formEditAcao");

        MoneyMask();
        myfunction();

        $('#modalLindo').find('.modal-body .content3').append("<button type='button' class='btn btn-primary' id='Salvar' onclick=\"isClickedEstrategico=false;isClickedTaticoVinculado=false;SalvarAcaoEditada();\" >" + Resources('save_operational') + "</button><hr>");
        $('#obj').show(); //Apagar depois

        $('#CausaGenerica_Id').change();
        $('#modalNovo > div > div > div.modal-footer > button').show();

        MoneyMask('R$ ');

        $('.money').each(function () {
            $(this).maskMoney('mask', $(this).val());
        })

        $("input[name='_QuandoInicio']")[0].value = $("input[name='_QuandoInicio']")[0].defaultValue.substring(0, 10);
        $("input[name='_QuandoFim']")[0].value = $("input[name='_QuandoFim']")[0].defaultValue.substring(0, 10);

        $('#Fechar').show();
    });
}

function getAcompanhamento(acaoCorrentId) {

    $('#modalLindo').modal('hide');

    $.get(urlAcompanhamentoDetalhes, { id: acaoCorrentId }, function (r) {
        $('#modalLindo').find('.modal-body').empty().append(r);
        $('#modalLindo').modal();
        $('#Header').html(Resources('operational_planning_follow_up'));
        $('.modal .modal-footer button').hide();
        $('#showAcompnhamentoForm').show();

        $("#SaveAcompanhamento").on("click", function (event) {
            var obj = $("#acompanhamento").serializeObject();
            if (obj.Name == "" || obj.Acao_Id == undefined || obj.MailTo == undefined || obj.MailTo == undefined || !(obj.Status_Id > 0))
                alert(Resources('fields_must_be_completed'))
            else
                SalvarAcompanhamento(obj);
        });

        $('.edit').off('click').on('click', function (data, a, b) {
            var data = selecionado

            $('#modalLindo').find('.modal-body').empty().append('<div class="content1"></div><div class="content2"></div><div class="content3"></div>');

            if (data.Id > 0) {

                getPlanOp(data, a, b);

            } else if (data.Estrategico_Id > 0) {

                getPlanEstrat(data, a, b);

            } else if (data.Acao && data.Acao.Id > 0) {

                getAcao(data, a, b);
            }

            $('#modalLindo').find('.modal-footer button').hide();
            $('#Header').html(Resources('edit'));
            $('#modalLindo').modal();
            $('#Fechar').show();
        });

        setTimeout(function () {
            $('#MailTo').select2({
                matcher: matchCustom
            });
            $('#Status_Id').select2({
                matcher: matchCustom
            });
        }, 4500);
    })
}

function SalvarAcompanhamento(obj) {

    $('#modalLindo').modal('hide');
    $.post(urlSalvarAcompanhamento, obj, function (r) {

        formReset();

        $('#btnTop').click();
        openMessageModal(Resources('registry_successfully_saved'), '');

    }).fail(function (e, h, x) {

        openMessageModal(Resources('could_not_save_record'), e.responseJSON.Message);

    });
}

function formReset() {
    $('#acompanhamento #Name').val("");
    $('#acompanhamento #MailTo').val(0);
    $('#acompanhamento #Status_Id').val(0);
}

function SetaValoresModal(id, item, isActive, isDiretriz, isPriority) {

    $('#btnCancelarNovo').click();

    if (!!parseInt(id)) {
        $("#newIdItem").val(id);
        $("#newItem").val(item);
        $("#newIsActiveItem").prop('checked', isActive);
        $('#btnSalvarNovo').addClass('hide');
        $('#btnEditarNovo').removeClass('hide');
        $('#btnCancelarNovo').removeClass('hide');
        $('#divIsActive').removeClass('hide');

        if (isDiretriz) {
            $('#isPriority').prop('checked', isPriority);
            $('#divIsPriority').removeClass('hide');
        } else {
            $('#isPriority').prop('checked', isPriority);
            $('#divIsPriority').addClass('hide');
        }
    } else {

        $('#divIsActive').removeClass('hide');

        if (!isDiretriz) {
            $('#divIsPriority').addClass('hide');
        }
        else {
            $('#divIsPriority').removeClass('hide');
            $('#isPriority').prop('checked', false);
        }

    }
}

function AbreModalDeAcao(id) {
    Clicked(true, true);

    $('#modalLindo').modal();
    $('#modalLindo').find('.modal-body').empty();
    $('#Header').html(Resources('operational_planning'));

    $.get(PlanejamentoDetalhes, { id: id }, function (r) {
        $('#NovaAcao').show();
        $('#NovaAcao').click();
        $('#modalLindo').find('.modal-body').empty().prepend(r);
    });
}

function isPositiveInteger(n) {
    return n == "0" || ((n | 0) > 0 && n % 1 == 0);
}