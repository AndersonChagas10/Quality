/**
* Configura os valores padrão do pluguin utilizado para mascarar caixas de texto.
*
*
*/
jQuery.mask.options = jQuery.extend(jQuery.mask.options, {
    attr: 'alt', // Define o atribuito que pode ser adicionado a elementos que devem ser mascarados.
    mask: null, // Máscara padrão usada nos campos.
    type: 'fixed', // A máscara das máscaras.
    maxLength: -1, // Tamanho máximo das mascaras
    defaultValue: '', // Valor padrão dos campos.
    textAlign: true, // Alinhar o texto no campo.
    selectCharsOnFocus: false, // Selecionar o valor do campo quando ele for focado.
    setSize: false, // Define o tamanho do campo com base no tamanho da máscara (Funciona com máscaras fixas e reversas apenas).
    autoTab: false, // Foco automático no próximo campo.
    fixedChars: '[(),.:/ -]', // Define os caracteres usados nas máscaras.
    onInvalid: function () { },
    onValid: function () { },
    onOverflow: function () { }
});