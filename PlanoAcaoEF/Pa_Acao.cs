//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PlanoAcaoEF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pa_Acao
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> AddDate { get; set; }
        public Nullable<System.DateTime> AlterDate { get; set; }
        public Nullable<int> Unidade_Id { get; set; }
        public Nullable<int> Departamento_Id { get; set; }
        public Nullable<System.DateTime> QuandoInicio { get; set; }
        public Nullable<int> DuracaoDias { get; set; }
        public Nullable<System.DateTime> QuandoFim { get; set; }
        public string ComoPontosimportantes { get; set; }
        public Nullable<int> Predecessora_Id { get; set; }
        public string PraQue { get; set; }
        public Nullable<decimal> QuantoCusta { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> Panejamento_Id { get; set; }
        public Nullable<int> Pa_IndicadorSgqAcao_Id { get; set; }
        public Nullable<int> Pa_Problema_Desvio_Id { get; set; }
        public Nullable<int> Level1Id { get; set; }
        public Nullable<int> Level2Id { get; set; }
        public Nullable<int> Level3Id { get; set; }
        public Nullable<int> Quem_Id { get; set; }
        public Nullable<int> CausaGenerica_Id { get; set; }
        public Nullable<int> ContramedidaGenerica_Id { get; set; }
        public Nullable<int> GrupoCausa_Id { get; set; }
        public string CausaEspecifica { get; set; }
        public string ContramedidaEspecifica { get; set; }
        public Nullable<int> TipoIndicador { get; set; }
        public Nullable<int> Fta_Id { get; set; }
        public string Observacao { get; set; }
        public string Level1Name { get; set; }
        public string Level2Name { get; set; }
        public string Level3Name { get; set; }
        public string Regional { get; set; }
        public string UnidadeName { get; set; }
        public Nullable<int> UnidadeDeMedida_Id { get; set; }
    }
}
