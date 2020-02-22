function level1BusinessRoute(parlevel1_id){
    var parLevel1XParFamiliaProduto = existParLevel1XParFamiliaProduto(parlevel1_id);
    if (parLevel1XParFamiliaProduto != null) {
        //listarFamiliaProdutoDCA();
        currentFamiliaProdutoDCA_Id = parLevel1XParFamiliaProduto.ParFamiliaProduto_Id;
        listarParLevel2DCA();
    }else{
        listarParLevel2();
    }
    return;
}

function getParLevel1XParFamiliaProduto(parlevel1_id) {
    var _parlevel1_id = parlevel1_id;
    var exists = $.grep(parametrization.listaParLevel1XParFamiliaProduto, function (item) {
        return item.ParLevel1_Id == _parlevel1_id;
    })
    if (exists.length > 0) {
        return exists[0];
    } else {
        return null;
    }
}