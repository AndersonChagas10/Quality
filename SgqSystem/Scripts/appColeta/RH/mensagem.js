function criarMensagem() {
	var html = '';
    html += '<div class="hide" style="text-align: center;margin: 150px 0px 0px 0px;background-color: rgba(255,255,255,0.8);z-index: 9999;position: fixed;color: #000;width: 100%;height:100%" data-mensagem>';
	html += '<div style="font-size:22px;text-align: left;margin: 150px 0px;background-color: #fff;z-index: 9999;position: fixed;color: #000;width: 100%;padding: 60px 40px">';
	html += '	Carregando, por favor, aguarde.';
	html += '</div>';
	html += '</div>';

	$('body').prepend(html);
}

function openMensagem(mensagem, color, textColor) {
	$('div[data-mensagem] > div').css('color', textColor);
	$('div[data-mensagem] > div').css('background-color', color);
	$('div[data-mensagem] > div').text(mensagem);
	$('div[data-mensagem]').removeClass('hide');
}

function closeMensagem(timer) {
	if (typeof (timer) == "undefined") timer = 0;
	setTimeout(function () {
		$('div[data-mensagem]').addClass('hide');
	}, timer)
}

function criarModal() {
	var html = '';
	html += '<div class="hide" style="text-align: center;margin: 150px 0px 0px 0px;background-color: rgba(255,255,255,0.8);z-index: 9999;position: fixed;color: #000;width: 100%;height:100%" data-html>';
	html += '<div style="font-size:22px;text-align: left;margin: 0px 0px;background-color: #fff;z-index: 9999;position: fixed;color: #000;width: 100%;padding: 60px 40px; col-sm-12;overflow: auto;max-height: 100%;">';
	html += '</div>';
	html += '</div>';

	$('body').prepend(html);
}

function openModal(html, color, textColor) {
	$('div[data-html] > div').css('color', textColor);
    $('div[data-html] > div').css('background-color', color);
	$('div[data-html] > div').html(html);
	$('div[data-html]').removeClass('hide');
}

function closeModal(timer) {
	if (typeof (timer) == "undefined") timer = 0;
	setTimeout(function () {
		$('div[data-html]').addClass('hide');
	}, timer)
}

function openMessageConfirm(title, messagem, callbackYes, callbackNo, color, textColor) {

	var html =
		'<div class="container">' +
		'<div class="row">' +
		'<h4>' + title + '</h4>' +
		'<p>' + messagem + '</p>' +
		'<button class="btn btn-primary pull-right" onclick="' + callbackYes + ';closeModal();">Sim</button>' +
		'<button class="btn btn-default pull-right" onclick="' + callbackNo + ';closeModal();">Não</button>' +
		'</div>' +
		'</div>';

	openModal(html, color, textColor);
}