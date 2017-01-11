﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SgqDbDevEntities : DbContext
    {
        public SgqDbDevEntities()
            : base("name=SgqDbDevEntities")
        {
    
    
    this.Database.CommandTimeout = 9600;
    this.Database.Log = s => System.Diagnostics.Debug.Write(s);
    
    
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<DelDados> DelDados { get; set; }
        public virtual DbSet<Reports_CCA_Audit> Reports_CCA_Audit { get; set; }
        public virtual DbSet<Reports_CFF_Audit> Reports_CFF_Audit { get; set; }
        public virtual DbSet<Reports_HTP_Audit> Reports_HTP_Audit { get; set; }
        public virtual DbSet<VWCFFResults> VWCFFResults { get; set; }
        public virtual DbSet<BkpCollection> BkpCollection { get; set; }
        public virtual DbSet<CollectionHtml> CollectionHtml { get; set; }
        public virtual DbSet<CollectionJson> CollectionJson { get; set; }
        public virtual DbSet<CollectionLevel02> CollectionLevel02 { get; set; }
        public virtual DbSet<CollectionLevel03> CollectionLevel03 { get; set; }
        public virtual DbSet<CollectionLevel2> CollectionLevel2 { get; set; }
        public virtual DbSet<CollectionLevel2XParHeaderField> CollectionLevel2XParHeaderField { get; set; }
        public virtual DbSet<ConsolidationLevel01> ConsolidationLevel01 { get; set; }
        public virtual DbSet<ConsolidationLevel02> ConsolidationLevel02 { get; set; }
        public virtual DbSet<ConsolidationLevel1> ConsolidationLevel1 { get; set; }
        public virtual DbSet<ConsolidationLevel2> ConsolidationLevel2 { get; set; }
        public virtual DbSet<CorrectiveAction> CorrectiveAction { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Example> Example { get; set; }
        public virtual DbSet<Level01> Level01 { get; set; }
        public virtual DbSet<Level02> Level02 { get; set; }
        public virtual DbSet<Level03> Level03 { get; set; }
        public virtual DbSet<LogAlteracoes> LogAlteracoes { get; set; }
        public virtual DbSet<LogJson> LogJson { get; set; }
        public virtual DbSet<LogSgq> LogSgq { get; set; }
        public virtual DbSet<LogSgqGlobal> LogSgqGlobal { get; set; }
        public virtual DbSet<NQA> NQA { get; set; }
        public virtual DbSet<ParCluster> ParCluster { get; set; }
        public virtual DbSet<ParClusterGroup> ParClusterGroup { get; set; }
        public virtual DbSet<ParCompany> ParCompany { get; set; }
        public virtual DbSet<ParCompanyCluster> ParCompanyCluster { get; set; }
        public virtual DbSet<ParCompanyXStructure> ParCompanyXStructure { get; set; }
        public virtual DbSet<ParCompanyXUserSgq> ParCompanyXUserSgq { get; set; }
        public virtual DbSet<ParConfSGQ> ParConfSGQ { get; set; }
        public virtual DbSet<ParConsolidationType> ParConsolidationType { get; set; }
        public virtual DbSet<ParCounter> ParCounter { get; set; }
        public virtual DbSet<ParCounterXLocal> ParCounterXLocal { get; set; }
        public virtual DbSet<ParCriticalLevel> ParCriticalLevel { get; set; }
        public virtual DbSet<ParDepartment> ParDepartment { get; set; }
        public virtual DbSet<ParEvaluation> ParEvaluation { get; set; }
        public virtual DbSet<ParFieldType> ParFieldType { get; set; }
        public virtual DbSet<ParFrequency> ParFrequency { get; set; }
        public virtual DbSet<ParGoal> ParGoal { get; set; }
        public virtual DbSet<ParHeaderField> ParHeaderField { get; set; }
        public virtual DbSet<ParLevel1> ParLevel1 { get; set; }
        public virtual DbSet<ParLevel1XCluster> ParLevel1XCluster { get; set; }
        public virtual DbSet<ParLevel1XHeaderField> ParLevel1XHeaderField { get; set; }
        public virtual DbSet<ParLevel2> ParLevel2 { get; set; }
        public virtual DbSet<ParLevel2ControlCompany> ParLevel2ControlCompany { get; set; }
        public virtual DbSet<ParLevel2Level1> ParLevel2Level1 { get; set; }
        public virtual DbSet<ParLevel3> ParLevel3 { get; set; }
        public virtual DbSet<ParLevel3BoolFalse> ParLevel3BoolFalse { get; set; }
        public virtual DbSet<ParLevel3BoolTrue> ParLevel3BoolTrue { get; set; }
        public virtual DbSet<ParLevel3Group> ParLevel3Group { get; set; }
        public virtual DbSet<ParLevel3InputType> ParLevel3InputType { get; set; }
        public virtual DbSet<ParLevel3Level2> ParLevel3Level2 { get; set; }
        public virtual DbSet<ParLevel3Level2Level1> ParLevel3Level2Level1 { get; set; }
        public virtual DbSet<ParLevel3Value> ParLevel3Value { get; set; }
        public virtual DbSet<ParLevelDefiniton> ParLevelDefiniton { get; set; }
        public virtual DbSet<ParLocal> ParLocal { get; set; }
        public virtual DbSet<ParMeasurementUnit> ParMeasurementUnit { get; set; }
        public virtual DbSet<ParMultipleValues> ParMultipleValues { get; set; }
        public virtual DbSet<ParNotConformityRule> ParNotConformityRule { get; set; }
        public virtual DbSet<ParNotConformityRuleXLevel> ParNotConformityRuleXLevel { get; set; }
        public virtual DbSet<ParRelapse> ParRelapse { get; set; }
        public virtual DbSet<ParSample> ParSample { get; set; }
        public virtual DbSet<ParStructure> ParStructure { get; set; }
        public virtual DbSet<ParStructureGroup> ParStructureGroup { get; set; }
        public virtual DbSet<Period> Period { get; set; }
        public virtual DbSet<Result_Level3> Result_Level3 { get; set; }
        public virtual DbSet<Shift> Shift { get; set; }
        public virtual DbSet<Unit> Unit { get; set; }
        public virtual DbSet<UnitUser> UnitUser { get; set; }
        public virtual DbSet<UserSgq> UserSgq { get; set; }
        public virtual DbSet<VolumeCepDesossa> VolumeCepDesossa { get; set; }
        public virtual DbSet<VolumeCepRecortes> VolumeCepRecortes { get; set; }
        public virtual DbSet<VolumePcc1b> VolumePcc1b { get; set; }
        public virtual DbSet<VolumeVacuoGRD> VolumeVacuoGRD { get; set; }
        public virtual DbSet<budgets> budgets { get; set; }
        public virtual DbSet<Deviation> Deviation { get; set; }
        public virtual DbSet<ParLevel1VariableProduction> ParLevel1VariableProduction { get; set; }
        public virtual DbSet<ParLevel1VariableProductionXLevel1> ParLevel1VariableProductionXLevel1 { get; set; }
        public virtual DbSet<ParLevel2XHeaderField> ParLevel2XHeaderField { get; set; }
        public virtual DbSet<ResultLevel2HeaderField> ResultLevel2HeaderField { get; set; }
        public virtual DbSet<manDataCollectIT> manDataCollectIT { get; set; }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    
        [DbFunction("SgqDbDevEntities", "fn_getsubtree")]
        public virtual IQueryable<fn_getsubtree_Result> fn_getsubtree(Nullable<int> empid)
        {
            var empidParameter = empid.HasValue ?
                new ObjectParameter("empid", empid) :
                new ObjectParameter("empid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<fn_getsubtree_Result>("[SgqDbDevEntities].[fn_getsubtree](@empid)", empidParameter);
        }
    }
}
