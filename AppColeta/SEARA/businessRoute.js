function level1BusinessRoute(parlevel1_id){
    if(parlevel1_id == globalDicionarioEstatico.ParLevel1DCA){
        listarFamiliaProdutoDCA();
    }else{
        listarParLevel2();
    }
    return;
}