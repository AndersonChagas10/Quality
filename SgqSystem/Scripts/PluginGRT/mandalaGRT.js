var mandalaGRT = {

    /*
        Para centralizar:
        body {
          text-align: center;
        }
    */

    listaDeDados: [],
    margemDoCentro: 50,
    tamanhoDoElemento: 15,
    idElemento: "",
    elementoDeCentro: "",
    estiloDoElemento: "background: steelblue;border-radius: 100%;",

    SubstituirVariaveis(texto, posicao) {
        var regExp = new RegExp('({([^{]|)*})', 'g');
        var match = texto.match(regExp);
        for (var j = 0; j < match.length; j++) {
            var valorTemp = this.listaDeDados[posicao][match[j].replace("{", "").replace("}", "")];
            texto = texto.replace(match[j], valorTemp != undefined ? valorTemp : undefined);
        }
        return texto;
    },

    PosicionarRaio(raio, angulo) {

        ang = (angulo * Math.PI) / 180;

        var seno = Math.sin(ang);
        var cosseno = -Math.cos(ang);

        var x = raio * seno;
        var y = raio * cosseno;

        return [x, y];
    },

    PlotaBolas(lista, raio) {

        var quantidadeElementos = lista.length;
        var graus = 360 / quantidadeElementos;
        var angulo = 0;
        var arrayPos = this.PosicionarRaio(raio, angulo);

        for (var i = 1; i <= quantidadeElementos; i++) {
            this.CriaElementos(i);
            $('#' + this.idElemento).find('a.mandala-item.item-' + i).css('-webkit-transform', 'translate3d(' + arrayPos[0] + 'px, ' + arrayPos[1] + 'px, 0)')
            $('#' + this.idElemento).find('a.mandala-item.item-' + i).css('transform', 'translate3d(' + arrayPos[0] + 'px, ' + arrayPos[1] + 'px, 0)')

            angulo = angulo + graus;
            arrayPos = this.PosicionarRaio(raio, angulo)
        }
    },

    CriaElementos(posicao) {
        var estiloAuxiliar = this.SubstituirVariaveis(this.estiloDoElemento, posicao - 1);
        var atributosHtml = this.SubstituirVariaveis(this.atributosElemento, posicao - 1);
        $('#' + this.idElemento).prepend('<a href="#" class="mandala-item item-' + posicao + ' item" ' + atributosHtml + ' style="position: absolute;width:' + this.tamanhoDoElemento + 'px;height:' + this.tamanhoDoElemento + 'px;' + estiloAuxiliar + '"></a>');
    },

    Inicializar(config) {

        if (config.listaDeDados != undefined)
            this.listaDeDados = config.listaDeDados;
        if (config.margemDoCentro != undefined)
            this.margemDoCentro = config.margemDoCentro;
        if (config.idElemento != undefined)
            this.idElemento = config.idElemento;
        if (config.tamanhoDoElemento != undefined)
            this.tamanhoDoElemento = config.tamanhoDoElemento;
        if (config.posicaoNaTela != undefined)
            this.posicaoNaTela = config.posicaoNaTela;
        if (config.estiloDoElemento != undefined)
            this.estiloDoElemento = config.estiloDoElemento;
        if (config.atributosElemento != undefined)
            this.atributosElemento = config.atributosElemento;
        if (config.elementoDeCentro != undefined)
            this.elementoDeCentro = config.elementoDeCentro;

        var raioElemento = this.tamanhoDoElemento / 2;
        if (config.margemElemento != undefined)
            this.margemElemento = config.margemElemento;
        else
            this.margemElemento = -raioElemento + 'px ' + raioElemento + 'px ' + raioElemento + 'px -' + raioElemento + 'px';

        //Reseta Mandala
        $('#' + this.idElemento).empty();
        $('#' + this.idElemento).css('position', 'fixed');
        $('#' + this.idElemento).css('z-index', '9999999');
        $('#' + this.idElemento).css('margin', this.margemElemento);
        $(this.posicaoNaTela.split(' ')).each(function (i, o) {
            $('#' + config.idElemento).css(o, '0');
        });
        $('#' + this.idElemento).css('padding', (this.margemDoCentro + raioElemento) + 'px');

        if (this.elementoDeCentro.length > 0)
            $('#' + this.idElemento).append(this.elementoDeCentro);

        this.PlotaBolas(this.listaDeDados, this.margemDoCentro);

        $('#' + this.idElemento+ ' a').hover(function () {
            $(this).css("opacity", "0.5");
            $(this).css("filter", "alpha(opacity = 50)");
        }, function () {
            $(this).css("opacity", "1");
            $(this).css("filter", "alpha(opacity = 100)");
        });

    }
};