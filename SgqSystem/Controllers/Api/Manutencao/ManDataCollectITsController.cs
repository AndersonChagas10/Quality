using Dominio;
using DTO.Helpers;
using SgqSystem.Handlres;
using System;
using System.Web.Http;


namespace SgqSystem.Controllers.Api.Manutencao
{
    [HandleApi()]
    [RoutePrefix("api/Manutencao")]
    public class ManDataCollectITsController : ApiController
    {
        //[HttpPost]
        //[HandleApi()]
        //[Route("SaveCreate")]
        //public int SaveCreate(Obj obj)
        //{
        //    string sql = "";

        //    //string sql = "INSERT INTO [dbo].[ManDataCollectIT]" +
        //    //  "([AddDate]" +
        //    //  ",[ReferenceDatetime]" +
        //    //  ",[UserSGQ_Id]" +
        //    //  ",[ParCompany_Id]" +
        //    //  ",[DimManutencaoColetaITs_id]" +
        //    //  ",[AmountData]" +
        //    //  ",[Comments]" +
        //    //  ",[IsActive])" +
        //    //  "VALUES" +
        //    //  "(" + DateTime.Now + "," +
        //      //"," + manDataCollectITs.ReferenceDatetime +
        //      ////"," + Guard.GetUsuarioLogado_Id(System.Web.HttpContext) +
        //      //"," + manDataCollectITs.ParCompany_Id +
        //      //"," + manDataCollectITs.DimManutencaoColetaITs_id +
        //      //"," + manDataCollectITs.AmountData +
        //      //"," + manDataCollectITs.Comments +
        //      //"," + true + ")";

        //    using (var db = new SgqDbDevEntities())
        //    {
        //        var d = db.Database.ExecuteSqlCommand(sql);
        //        return d;
        //    }

        //}

        [HttpPost]
        [HandleApi()]
        [Route("SaveCreate")]
        public int SaveCreate(Obj obj)
        {
            string sql = "";

            sql = "INSERT INTO[dbo].[ManColetaDados] " +
            "([Base_parCompany_id] " +
            ",[Base_dateAdd] " +
            ",[Base_dateAlter] " +
            ",[Base_dateRef] " +
            ",[Comentarios]) " +

             //,[CabAbat_QteBoiAbatidosReal]
             //,[CabAbat_QteBoiAbatidosMeta]
             //,[CabAbat_PrevisaoFechamento]
             //,[CabAbat_SobraAnimaisCurrais]
             //,[CustoMan_Meta]
             //,[CustoMan_OrcDia]
             //,[CustoMan_Real]
             //,[CustoMan_CombinadoMenos5pcnt]
             //,[CustoMan_VarCombinado]
             //,[CustoMan_PrevisaoFechamento]
             //,[EnergiaEletrica_Meta]
             //,[EnergiaEletrica_Real]
             //,[EnergiaEletrica_OrcDia]
             //,[EnergiaEletrica_KWhBoiProcMeta]
             //,[EnergiaEletrica_KWhBoiProcReal]
             //,[EnergiaEletrica_PrevisaoFechamento]
             //,[CombCaldUtil_Meta]
             //,[CombCaldUtil_OrcDia]
             //,[CombCaldUtil_Real]
             //,[CombCaldUtil_McalBoiUtilMeta]
             //,[CombCaldUtil_McalBoiUtilReal]
             //,[CombCaldUtil_PrevFechamento]
             //,[FatorPotencia_Meta]
             //,[FatorPotencia_Real]
             //,[M3AguaBoi_Meta]
             //,[M3AguaBoi_Real]
             //,[M3Lenha_Meta]
             //,[M3Lenha_Real]
             //,[Confiabilidade_AbateMeta]
             //,[Confiabilidade_AbateReal]
             //,[Confiabilidade_DesossaMeta]
             //,[Confiabilidade_DesossaReal]
             //,[KWhMes_Meta]
             //,[KWhMes_Real]
             //,[ParadasAbateMinutos_Meta]
             //,[ParadasAbateMinutos_Real]
             //,[ParadasDesossaMinutos_Meta]
             //,[ParadasDesossaMinutos_Real]
             //,[ObrasEmAndamento_Meta]
             //,[ObrasEmAndamento_Real]
             //,[Moral_CustoHorasExtrasMeta]
             //,[Moral_CustoHorasExtrasReal]
             //,[Moral_CustoHorasExtrasParcial]
             //,[Moral_QteHorasExtras]
             //,[Moral_TaxaFrequenciaAcidentesMeta]
             //,[Moral_TaxaFrequenciaAcidentesReal]
             //,[ApropHoras_Meta]
             //,[ApropHoras_QteHoraAprop]
             //,[ApropHoras_QteManutentores]
             //,[ApropHoras_Real]
             //,[EficienciaProgram_Meta]
             //,[EficienciaProgram_QteOSAberta]
             //,[EficienciaProgram_QteOSEncerrada]
             //,[EficienciaProgram_Real]
             //,[ApropPlanejamento_Meta]
             //,[ApropPlanejamento_TotalHorasAprop]
             //,[ApropPlanejamento_Real]
             //,[Rendimento_Meta]
             //,[Rendimento_Real]
             //,[SeboFlotado_Meta]
             //,[SeboFlotado_Real]
             //,[ScoreCardUnidade_Meta]
             //,[ScoreCardUnidade_Real]
             //,[ScoreCardCheckListEquip_Meta]
             //,[ScoreCardCheckListEquip_Real]
             //,[ScoreCardAnaliseAgua_Meta]
             //,[ScoreCardAnaliseAgua_Real]
             //,[ScoreCardAspersao_Meta]
             //,[ScoreCardAspersao_Real]
             //,[ScoreCardMeioAmbiente_Meta]
             //,[ScoreCardMeioAmbiente_Real]
             //,[ScoreCardEnergia_Meta]
             //,[ScoreCardEnergia_Real]
             //,[ScoreCardVapor_Meta]
             //,[ScoreCardVapor_Real]
             //,[PilarMan_Meta]
             //,[PilarMan_Real]
             //,[Reclamacao_Meta]
             //,[Reclamacao_Real]
             //,[CustoDevolucoes_Meta]
             //,[CustoDevolucoes_Real]
             //,[Absenteismo_Meta]
             //,[Absenteismo_Real]
             //,[Rotatividade_Meta]
             //,[Rotatividade_Real]
             //,[HeadCount_Meta]
             //,[HeadCount_Real]
             //,[ReaisCabeca_Meta]
             //,[ReaisCabeca_Real]
             //,[ReaisCabeca_ManUtilMeta]
             //,[ReaisCabeca_ManUtilReal]
             //,[EnergiaReaisCabeca_Meta]
             //,[EnergiaReaisCabeca_Real]
             //,[Capex_Meta]
             //,[Capex_Real]
             //,[Disponibilidade_GeralMeta]
             //,[Disponibilidade_GeralReal]
             //,[CartaMetas_Meta]
             //,[CartaMetas_Real]
             //,[CartaMetas_Vazio]

             "VALUES ";
           //(<Base_parCompany_id, int,>
           //,<Base_dateAdd, datetime,>
           //,<Base_dateAlter, datetime,>
           //,<Base_dateRef, date,>
           //,<CabAbat_QteBoiAbatidosReal, decimal(30,10),>
           //,<CabAbat_QteBoiAbatidosMeta, decimal(30,10),>
           //,<CabAbat_PrevisaoFechamento, decimal(30,10),>
           //,<CabAbat_SobraAnimaisCurrais, decimal(30,10),>
           //,<CustoMan_Meta, decimal(30,10),>
           //,<CustoMan_OrcDia, decimal(30,10),>
           //,<CustoMan_Real, decimal(30,10),>
           //,<CustoMan_CombinadoMenos5pcnt, decimal(30,10),>
           //,<CustoMan_VarCombinado, decimal(30,10),>
           //,<CustoMan_PrevisaoFechamento, decimal(30,10),>
           //,<EnergiaEletrica_Meta, decimal(30,10),>
           //,<EnergiaEletrica_Real, decimal(30,10),>
           //,<EnergiaEletrica_OrcDia, decimal(30,10),>
           //,<EnergiaEletrica_KWhBoiProcMeta, decimal(30,10),>
           //,<EnergiaEletrica_KWhBoiProcReal, decimal(30,10),>
           //,<EnergiaEletrica_PrevisaoFechamento, decimal(30,10),>
           //,<CombCaldUtil_Meta, decimal(30,10),>
           //,<CombCaldUtil_OrcDia, decimal(30,10),>
           //,<CombCaldUtil_Real, decimal(30,10),>
           //,<CombCaldUtil_McalBoiUtilMeta, decimal(30,10),>
           //,<CombCaldUtil_McalBoiUtilReal, decimal(30,10),>
           //,<CombCaldUtil_PrevFechamento, decimal(30,10),>
           //,<FatorPotencia_Meta, decimal(30,10),>
           //,<FatorPotencia_Real, decimal(30,10),>
           //,<M3AguaBoi_Meta, decimal(30,10),>
           //,<M3AguaBoi_Real, decimal(30,10),>
           //,<M3Lenha_Meta, decimal(30,10),>
           //,<M3Lenha_Real, decimal(30,10),>
           //,<Confiabilidade_AbateMeta, decimal(30,10),>
           //,<Confiabilidade_AbateReal, decimal(30,10),>
           //,<Confiabilidade_DesossaMeta, decimal(30,10),>
           //,<Confiabilidade_DesossaReal, decimal(30,10),>
           //,<KWhMes_Meta, decimal(30,10),>
           //,<KWhMes_Real, decimal(30,10),>
           //,<ParadasAbateMinutos_Meta, decimal(30,10),>
           //,<ParadasAbateMinutos_Real, decimal(30,10),>
           //,<ParadasDesossaMinutos_Meta, decimal(30,10),>
           //,<ParadasDesossaMinutos_Real, decimal(30,10),>
           //,<ObrasEmAndamento_Meta, decimal(30,10),>
           //,<ObrasEmAndamento_Real, decimal(30,10),>
           //,<Moral_CustoHorasExtrasMeta, decimal(30,10),>
           //,<Moral_CustoHorasExtrasReal, decimal(30,10),>
           //,<Moral_CustoHorasExtrasParcial, decimal(30,10),>
           //,<Moral_QteHorasExtras, decimal(30,10),>
           //,<Moral_TaxaFrequenciaAcidentesMeta, decimal(30,10),>
           //,<Moral_TaxaFrequenciaAcidentesReal, decimal(30,10),>
           //,<ApropHoras_Meta, decimal(30,10),>
           //,<ApropHoras_QteHoraAprop, decimal(30,10),>
           //,<ApropHoras_QteManutentores, decimal(30,10),>
           //,<ApropHoras_Real, decimal(30,10),>
           //,<EficienciaProgram_Meta, decimal(30,10),>
           //,<EficienciaProgram_QteOSAberta, decimal(30,10),>
           //,<EficienciaProgram_QteOSEncerrada, decimal(30,10),>
           //,<EficienciaProgram_Real, decimal(30,10),>
           //,<ApropPlanejamento_Meta, decimal(30,10),>
           //,<ApropPlanejamento_TotalHorasAprop, decimal(30,10),>
           //,<ApropPlanejamento_Real, decimal(30,10),>
           //,<Rendimento_Meta, decimal(30,10),>
           //,<Rendimento_Real, decimal(30,10),>
           //,<SeboFlotado_Meta, decimal(30,10),>
           //,<SeboFlotado_Real, decimal(30,10),>
           //,<ScoreCardUnidade_Meta, decimal(30,10),>
           //,<ScoreCardUnidade_Real, decimal(30,10),>
           //,<ScoreCardCheckListEquip_Meta, decimal(30,10),>
           //,<ScoreCardCheckListEquip_Real, decimal(30,10),>
           //,<ScoreCardAnaliseAgua_Meta, decimal(30,10),>
           //,<ScoreCardAnaliseAgua_Real, decimal(30,10),>
           //,<ScoreCardAspersao_Meta, decimal(30,10),>
           //,<ScoreCardAspersao_Real, decimal(30,10),>
           //,<ScoreCardMeioAmbiente_Meta, decimal(30,10),>
           //,<ScoreCardMeioAmbiente_Real, decimal(30,10),>
           //,<ScoreCardEnergia_Meta, decimal(30,10),>
           //,<ScoreCardEnergia_Real, decimal(30,10),>
           //,<ScoreCardVapor_Meta, decimal(30,10),>
           //,<ScoreCardVapor_Real, decimal(30,10),>
           //,<PilarMan_Meta, decimal(30,10),>
           //,<PilarMan_Real, decimal(30,10),>
           //,<Reclamacao_Meta, decimal(30,10),>
           //,<Reclamacao_Real, decimal(30,10),>
           //,<CustoDevolucoes_Meta, decimal(30,10),>
           //,<CustoDevolucoes_Real, decimal(30,10),>
           //,<Absenteismo_Meta, decimal(30,10),>
           //,<Absenteismo_Real, decimal(30,10),>
           //,<Rotatividade_Meta, decimal(30,10),>
           //,<Rotatividade_Real, decimal(30,10),>
           //,<HeadCount_Meta, decimal(30,10),>
           //,<HeadCount_Real, decimal(30,10),>
           //,<ReaisCabeca_Meta, decimal(30,10),>
           //,<ReaisCabeca_Real, decimal(30,10),>
           //,<ReaisCabeca_ManUtilMeta, decimal(30,10),>
           //,<ReaisCabeca_ManUtilReal, decimal(30,10),>
           //,<EnergiaReaisCabeca_Meta, decimal(30,10),>
           //,<EnergiaReaisCabeca_Real, decimal(30,10),>
           //,<Capex_Meta, decimal(30,10),>
           //,<Capex_Real, decimal(30,10),>
           //,<Disponibilidade_GeralMeta, decimal(30,10),>
           //,<Disponibilidade_GeralReal, decimal(30,10),>
           //,<CartaMetas_Meta, decimal(30,10),>
           //,<CartaMetas_Real, decimal(30,10),>
           //,<CartaMetas_Vazio, decimal(30,10),>
           //,<Comentarios, varchar(900),>)

            using (var db = new SgqDbDevEntities())
            {
                var d = db.Database.ExecuteSqlCommand(sql);
                return d;
            }

        }

        [HttpPost]
        [HandleApi()]
        [Route("SaveCreateAll")]
        public int SaveCreateAll(Obj obj)
        {
            string sql = "";

            //string sql = "INSERT INTO [dbo].[ManDataCollectIT]" +
            //  "([AddDate]" +
            //  ",[ReferenceDatetime]" +
            //  ",[UserSGQ_Id]" +
            //  ",[ParCompany_Id]" +
            //  ",[DimManutencaoColetaITs_id]" +
            //  ",[AmountData]" +
            //  ",[Comments]" +
            //  ",[IsActive])" +
            //  "VALUES" +
            //  "(" + DateTime.Now + "," +
            //"," + manDataCollectITs.ReferenceDatetime +
            ////"," + Guard.GetUsuarioLogado_Id(System.Web.HttpContext) +
            //"," + manDataCollectITs.ParCompany_Id +
            //"," + manDataCollectITs.DimManutencaoColetaITs_id +
            //"," + manDataCollectITs.AmountData +
            //"," + manDataCollectITs.Comments +
            //"," + true + ")";

            using (var db = new SgqDbDevEntities())
            {
                var d = db.Database.ExecuteSqlCommand(sql);
                return d;
            }

        }

    }

    public class Obj
    { 
        private string descricao { get; set; }
        private decimal quantidade { get; set; }
        private string comentarios { get; set; }
        private DateTime data { get; set; }
        private int parCompany { get; set; }
    }

}
