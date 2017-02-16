//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dominio
{
    using System;
    using System.Collections.Generic;
    
    public partial class ManColetaDados
    {
        public int id { get; set; }
        public Nullable<int> Base_parCompany_id { get; set; }
        public Nullable<System.DateTime> Base_dateAdd { get; set; }
        public Nullable<System.DateTime> Base_dateAlter { get; set; }
        public Nullable<System.DateTime> Base_dateRef { get; set; }
        public Nullable<decimal> CabAbat_QteBoiAbatidosReal { get; set; }
        public Nullable<decimal> CabAbat_QteBoiAbatidosMeta { get; set; }
        public Nullable<decimal> CabAbat_PrevisaoFechamento { get; set; }
        public Nullable<decimal> CabAbat_SobraAnimaisCurrais { get; set; }
        public Nullable<decimal> CustoMan_Meta { get; set; }
        public Nullable<decimal> CustoMan_OrcDia { get; set; }
        public Nullable<decimal> CustoMan_Real { get; set; }
        public Nullable<decimal> CustoMan_CombinadoMenos5pcnt { get; set; }
        public Nullable<decimal> CustoMan_VarCombinado { get; set; }
        public Nullable<decimal> CustoMan_PrevisaoFechamento { get; set; }
        public Nullable<decimal> EnergiaEletrica_Meta { get; set; }
        public Nullable<decimal> EnergiaEletrica_Real { get; set; }
        public Nullable<decimal> EnergiaEletrica_OrcDia { get; set; }
        public Nullable<decimal> EnergiaEletrica_KWhBoiProcMeta { get; set; }
        public Nullable<decimal> EnergiaEletrica_KWhBoiProcReal { get; set; }
        public Nullable<decimal> EnergiaEletrica_PrevisaoFechamento { get; set; }
        public Nullable<decimal> CombCaldUtil_Meta { get; set; }
        public Nullable<decimal> CombCaldUtil_OrcDia { get; set; }
        public Nullable<decimal> CombCaldUtil_Real { get; set; }
        public Nullable<decimal> CombCaldUtil_McalBoiUtilMeta { get; set; }
        public Nullable<decimal> CombCaldUtil_McalBoiUtilReal { get; set; }
        public Nullable<decimal> CombCaldUtil_PrevFechamento { get; set; }
        public Nullable<decimal> FatorPotencia_Meta { get; set; }
        public Nullable<decimal> FatorPotencia_Real { get; set; }
        public Nullable<decimal> M3AguaBoi_Meta { get; set; }
        public Nullable<decimal> M3AguaBoi_Real { get; set; }
        public Nullable<decimal> M3Lenha_Meta { get; set; }
        public Nullable<decimal> M3Lenha_Real { get; set; }
        public Nullable<decimal> Confiabilidade_AbateMeta { get; set; }
        public Nullable<decimal> Confiabilidade_AbateReal { get; set; }
        public Nullable<decimal> Confiabilidade_DesossaMeta { get; set; }
        public Nullable<decimal> Confiabilidade_DesossaReal { get; set; }
        public Nullable<decimal> KWhMes_Meta { get; set; }
        public Nullable<decimal> KWhMes_Real { get; set; }
        public Nullable<decimal> ParadasAbateMinutos_Meta { get; set; }
        public Nullable<decimal> ParadasAbateMinutos_Real { get; set; }
        public Nullable<decimal> ParadasDesossaMinutos_Meta { get; set; }
        public Nullable<decimal> ParadasDesossaMinutos_Real { get; set; }
        public Nullable<decimal> ObrasEmAndamento_Meta { get; set; }
        public Nullable<decimal> ObrasEmAndamento_Real { get; set; }
        public Nullable<decimal> Moral_CustoHorasExtrasMeta { get; set; }
        public Nullable<decimal> Moral_CustoHorasExtrasReal { get; set; }
        public Nullable<decimal> Moral_CustoHorasExtrasParcial { get; set; }
        public Nullable<decimal> Moral_QteHorasExtras { get; set; }
        public Nullable<decimal> Moral_TaxaFrequenciaAcidentesMeta { get; set; }
        public Nullable<decimal> Moral_TaxaFrequenciaAcidentesReal { get; set; }
        public Nullable<decimal> ApropHoras_Meta { get; set; }
        public Nullable<decimal> ApropHoras_QteHoraAprop { get; set; }
        public Nullable<decimal> ApropHoras_QteManutentores { get; set; }
        public Nullable<decimal> ApropHoras_Real { get; set; }
        public Nullable<decimal> EficienciaProgram_Meta { get; set; }
        public Nullable<decimal> EficienciaProgram_QteOSAberta { get; set; }
        public Nullable<decimal> EficienciaProgram_QteOSEncerrada { get; set; }
        public Nullable<decimal> EficienciaProgram_Real { get; set; }
        public Nullable<decimal> ApropPlanejamento_Meta { get; set; }
        public Nullable<decimal> ApropPlanejamento_TotalHorasAprop { get; set; }
        public Nullable<decimal> ApropPlanejamento_Real { get; set; }
        public Nullable<decimal> Rendimento_Meta { get; set; }
        public Nullable<decimal> Rendimento_Real { get; set; }
        public Nullable<decimal> SeboFlotado_Meta { get; set; }
        public Nullable<decimal> SeboFlotado_Real { get; set; }
        public Nullable<decimal> ScoreCardUnidade_Meta { get; set; }
        public Nullable<decimal> ScoreCardUnidade_Real { get; set; }
        public Nullable<decimal> ScoreCardCheckListEquip_Meta { get; set; }
        public Nullable<decimal> ScoreCardCheckListEquip_Real { get; set; }
        public Nullable<decimal> ScoreCardAnaliseAgua_Meta { get; set; }
        public Nullable<decimal> ScoreCardAnaliseAgua_Real { get; set; }
        public Nullable<decimal> ScoreCardAspersao_Meta { get; set; }
        public Nullable<decimal> ScoreCardAspersao_Real { get; set; }
        public Nullable<decimal> ScoreCardMeioAmbiente_Meta { get; set; }
        public Nullable<decimal> ScoreCardMeioAmbiente_Real { get; set; }
        public Nullable<decimal> ScoreCardEnergia_Meta { get; set; }
        public Nullable<decimal> ScoreCardEnergia_Real { get; set; }
        public Nullable<decimal> ScoreCardVapor_Meta { get; set; }
        public Nullable<decimal> ScoreCardVapor_Real { get; set; }
        public Nullable<decimal> PilarMan_Meta { get; set; }
        public Nullable<decimal> PilarMan_Real { get; set; }
        public Nullable<decimal> Reclamacao_Meta { get; set; }
        public Nullable<decimal> Reclamacao_Real { get; set; }
        public Nullable<decimal> CustoDevolucoes_Meta { get; set; }
        public Nullable<decimal> CustoDevolucoes_Real { get; set; }
        public Nullable<decimal> Absenteismo_Meta { get; set; }
        public Nullable<decimal> Absenteismo_Real { get; set; }
        public Nullable<decimal> Rotatividade_Meta { get; set; }
        public Nullable<decimal> Rotatividade_Real { get; set; }
        public Nullable<decimal> HeadCount_Meta { get; set; }
        public Nullable<decimal> HeadCount_Real { get; set; }
        public Nullable<decimal> ReaisCabeca_Meta { get; set; }
        public Nullable<decimal> ReaisCabeca_Real { get; set; }
        public Nullable<decimal> ReaisCabeca_ManUtilMeta { get; set; }
        public Nullable<decimal> ReaisCabeca_ManUtilReal { get; set; }
        public Nullable<decimal> EnergiaReaisCabeca_Meta { get; set; }
        public Nullable<decimal> EnergiaReaisCabeca_Real { get; set; }
        public Nullable<decimal> Capex_Meta { get; set; }
        public Nullable<decimal> Capex_Real { get; set; }
        public Nullable<decimal> Disponibilidade_GeralMeta { get; set; }
        public Nullable<decimal> Disponibilidade_GeralReal { get; set; }
        public Nullable<decimal> CartaMetas_Meta { get; set; }
        public Nullable<decimal> CartaMetas_Real { get; set; }
        public Nullable<decimal> CartaMetas_Vazio { get; set; }
        public string Comentarios { get; set; }
        public Nullable<decimal> Disponibilidade_AbateMeta { get; set; }
        public Nullable<decimal> Disponibilidade_AbateReal { get; set; }
        public Nullable<decimal> Disponibilidade_DesossaReal { get; set; }
        public Nullable<decimal> Disponibilidade_DesossaMeta { get; set; }
    }
}
