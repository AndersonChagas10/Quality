function criarMensagem() {

	var html = '';
	html += '<div class="hide" style="text-align: center;background-color: rgba(255,255,255,0.8);z-index: 9999;position: fixed;color: #000;width: 100%;height:100%" data-mensagem>';
	html += '<div style="font-size:22px;text-align: left;margin: 150px 0px;background-color: #fff;top: 20%;z-index: 9999;position: fixed;color: #000;width: 100%;padding: 60px 40px">';
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

	if (typeof (timer) == "undefined")
		timer = 0;

	setTimeout(function () {
		$('div[data-mensagem]').addClass('hide');
		$('div[data-mensagem] > div').css('color', 'initial');
		$('div[data-mensagem] > div').css('background-color', 'initial');
	}, timer);

}

function closeMensagemImediatamente(){
	closeMensagem(0);
}

function criarModal() {
	var html = '';
	html += '<div class="hide" style="text-align: center;margin: 50px 0px 0px 0px;background-color: rgba(255,255,255,0.8);z-index: 9999;position: fixed;color: #000;width: 100%;height:100%" data-html>';
	html += '<div style="font-size:22px;text-align: left;margin: 0px 0px;background-color: #fff;z-index: 9999;position: fixed;color: #000;width: 100%;padding: 60px 40px; col-sm-12;overflow: auto;max-height: 90%;">';
	html += '</div>';
	html += '</div>';

	$('body').prepend(html);
}

function openModal(html, color, textColor) {
	closeModalImediatamente();
	
	$('div[data-html] > div').css('color', textColor);
	$('div[data-html] > div').css('background-color', color);
	$('div[data-html] > div').html(html);
	$('div[data-html]').removeClass('hide');
	$('body').addClass('modal-open');
}

function closeModal(timer) {

	if (typeof (timer) == "undefined")
		timer = 0;

	setTimeout(function () {
		closeModalImediatamente();
	}, timer);

}

function closeModalImediatamente(){
	$('div[data-html]').addClass('hide');
	$('div[data-html] > div').css('color', 'initial');
	$('div[data-html] > div').css('background-color', 'initial');
	$('div[data-html] > div').html('');
	$('body').removeClass('modal-open');
}

function openMessageConfirm(title, messagem, callbackYes, callbackNo, color, textColor) {
	closeModalImediatamente();
	var html =
		'<div class="container">' +
		'<div class="row">' +
		'<h3>' + title + '</h3>' +
		'<h4>' + messagem + '</h4>' +
		'<button class="btn btn-primary pull-right" onclick="closeModal();' + callbackYes.name + '();">Sim</button>' +
		'<button class="btn btn-default pull-right" onclick="closeModal();' + callbackNo.name + '();">Não</button>' +
		'</div>' +
		'</div>';

	openModal(html, color, textColor);
}