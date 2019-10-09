//criptografia para a senha
var key = CryptoJS.enc.Utf8.parse('JDS438FDSSJHLWEQ');
var iv = CryptoJS.enc.Utf8.parse('679FDM329IFD23HJ');
var AES = {
    Encrypt: function (text) {
        var encrypted = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(text), key,
            {
                keySize: 128 / 8,
                iv: iv,
                mode: CryptoJS.mode.CBC,
                padding: CryptoJS.pad.Pkcs7
            });
        return encrypted.toString();
    },
};