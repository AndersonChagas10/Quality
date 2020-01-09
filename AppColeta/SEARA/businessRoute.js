function level1BusinessRoute(parlevel1_id){
    if(parlevel1_id == globalDicionarioEstatico.ParLevel1DCA){
        listarParLevel2DCA();
    }else{
        listarParLevel2();
    }
    return;
}