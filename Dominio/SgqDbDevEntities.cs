namespace Dominio
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Threading;
    using Newtonsoft.Json;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using Helper;

    public partial class SgqDbDevEntities : DbContext
    {
        public SgqDbDevEntities()
            : base("name=DefaultConnection")
        {
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }
        public override int SaveChanges()
        {
            AddTimestamps();
            AddDatabaseLog();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps();
            AddDatabaseLog();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseModel && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((BaseModel)entity.Entity).AddDate = DateTime.Now;
                    //((BaseModel)entity.Entity).AlterDate = DateTime.Now;
                }
                else
                {
                    this.Entry(((BaseModel)entity.Entity)).Property(x => x.AddDate).IsModified = false;
                    ((BaseModel)entity.Entity).AlterDate = DateTime.Now;
                }
            }
        }

        private void AddDatabaseLog()
        {
            var entities = ChangeTracker.Entries().Where(x =>
            !(x.Entity is DatabaseLog)
            && !(x.Entity is LogTrack)
            && !(x.Entity is LogError)
            && !(x.Entity is ErrorLog)
            && !(x.Entity is UserSgq)
            && !(x.Entity is Result_Level3)
            && !(x.Entity is LogJson)
            && !(x.Entity is LogSgqGlobal)
            && !(x.Entity is LogAlteracoes)
            && !(x.Entity is CollectionJson)
            && !(x.Entity is CollectionLevel2)
            && !(x.Entity is ConsolidationLevel2)
            && !(x.Entity is ConsolidationLevel1)
            && !(x.Entity is CollectionLevel2XParHeaderField)
            && !(x.Entity is RecravacaoJson)
            && !(x.Entity is EmailContent)
            && !(x.Entity is Deviation)
            && !(x.Entity is IntegCollectionData)
            && !(x.State == EntityState.Detached || x.State == EntityState.Unchanged)
            ).ToList();

            foreach (var entity in entities)
            {
                object objeto = Activator.CreateInstance(entity.Entity.GetType());
                Type t = entity.Entity.GetType();

                foreach (var propInfo in t.GetProperties())
                {
                    object valor = propInfo.GetValue(entity.Entity, null);
                    Type tipo = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;
                    if (tipo.IsPrimitiveAndNullableType())
                    {
                        object valorConvertido = (valor == null) ? null : Convert.ChangeType(valor, tipo);
                        propInfo.SetValue(objeto, valorConvertido, null);
                    }
                }

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(objeto, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    PreserveReferencesHandling = PreserveReferencesHandling.None
                });

                this.DatabaseLog.Add(
                    new DatabaseLog()
                    {
                        AddDate = DateTime.Now,
                        Json = json,
                        Operacao = (int)entity.State,
                        Tabela = entity.Entity.GetType().ToString()
                    }
                 );

                base.SaveChanges();
            }
        }

        public virtual DbSet<DatabaseLog> DatabaseLog { get; set; }
        public virtual DbSet<ErrorLog> ErrorLog { get; set; }

        public virtual DbSet<AreasParticipantes> AreasParticipantes { get; set; }
        public virtual DbSet<BkpCollection> BkpCollection { get; set; }
        public virtual DbSet<CaracteristicaTipificacao> CaracteristicaTipificacao { get; set; }
        public virtual DbSet<ClusterDepartamentos> ClusterDepartamentos { get; set; }
        public virtual DbSet<Clusters> Clusters { get; set; }
        public virtual DbSet<CollectionHtml> CollectionHtml { get; set; }
        public virtual DbSet<CollectionJson> CollectionJson { get; set; }
        public virtual DbSet<CollectionLevel2> CollectionLevel2 { get; set; }
        public virtual DbSet<CollectionLevel2XParHeaderField> CollectionLevel2XParHeaderField { get; set; }
        public virtual DbSet<ConsolidationLevel1> ConsolidationLevel1 { get; set; }
        public virtual DbSet<ConsolidationLevel2> ConsolidationLevel2 { get; set; }
        public virtual DbSet<CorrectiveAction> CorrectiveAction { get; set; }
        public virtual DbSet<Defect> Defect { get; set; }
        public virtual DbSet<DepartamentoOperacoes> DepartamentoOperacoes { get; set; }
        public virtual DbSet<Departamentos> Departamentos { get; set; }
        public virtual DbSet<Deviation> Deviation { get; set; }
        public virtual DbSet<EmailContent> EmailContent { get; set; }
        public virtual DbSet<Empresas> Empresas { get; set; }
        public virtual DbSet<Equipamentos> Equipamentos { get; set; }
        public virtual DbSet<Example> Example { get; set; }
        public virtual DbSet<FamiliaProdutos> FamiliaProdutos { get; set; }
        public virtual DbSet<Horarios> Horarios { get; set; }
        public virtual DbSet<ItemMenu> ItemMenu { get; set; }
        public virtual DbSet<LeftControlRole> LeftControlRole { get; set; }
        public virtual DbSet<LogAlteracoes> LogAlteracoes { get; set; }
        public virtual DbSet<LogJson> LogJson { get; set; }
        public virtual DbSet<LogSgq> LogSgq { get; set; }
        public virtual DbSet<LogSgqGlobal> LogSgqGlobal { get; set; }
        public virtual DbSet<Metas> Metas { get; set; }
        public virtual DbSet<MigrationHistory> MigrationHistory { get; set; }
        public virtual DbSet<Monitoramentos> Monitoramentos { get; set; }
        public virtual DbSet<NQA> NQA { get; set; }
        public virtual DbSet<Operacoes> Operacoes { get; set; }
        public virtual DbSet<Pa_Acao> Pa_Acao { get; set; }
        public virtual DbSet<ParCluster> ParCluster { get; set; }
        public virtual DbSet<ParClusterGroup> ParClusterGroup { get; set; }
        public virtual DbSet<ParClusterXModule> ParClusterXModule { get; set; }
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
        public virtual DbSet<ParDepartmentGroup> ParDepartmentGroup { get; set; }
        public virtual DbSet<ParEvaluation> ParEvaluation { get; set; }
        public virtual DbSet<ParFieldType> ParFieldType { get; set; }
        public virtual DbSet<ParFrequency> ParFrequency { get; set; }
        public virtual DbSet<ParGoal> ParGoal { get; set; }
        public virtual DbSet<ParGoalScorecard> ParGoalScorecard { get; set; }
        public virtual DbSet<ParHeaderField> ParHeaderField { get; set; }
        public virtual DbSet<ParLataImagens> ParLataImagens { get; set; }
        public virtual DbSet<ParLevel1> ParLevel1 { get; set; }
        public virtual DbSet<ParLevel1XCluster> ParLevel1XCluster { get; set; }
        public virtual DbSet<ParLevel1XHeaderField> ParLevel1XHeaderField { get; set; }
        public virtual DbSet<ParLevel2> ParLevel2 { get; set; }
        public virtual DbSet<ParLevel2ControlCompany> ParLevel2ControlCompany { get; set; }
        public virtual DbSet<ParLevel2Level1> ParLevel2Level1 { get; set; }
        public virtual DbSet<ParLevel2XHeaderField> ParLevel2XHeaderField { get; set; }
        public virtual DbSet<ParLevel3> ParLevel3 { get; set; }
        public virtual DbSet<ParLevel3BoolFalse> ParLevel3BoolFalse { get; set; }
        public virtual DbSet<ParLevel3BoolTrue> ParLevel3BoolTrue { get; set; }
        public virtual DbSet<ParLevel3EvaluationSample> ParLevel3EvaluationSample { get; set; }
        public virtual DbSet<ParLevel3Group> ParLevel3Group { get; set; }
        public virtual DbSet<ParLevel3InputType> ParLevel3InputType { get; set; }
        public virtual DbSet<ParLevel3Level2> ParLevel3Level2 { get; set; }
        public virtual DbSet<ParLevel3Level2Level1> ParLevel3Level2Level1 { get; set; }
        public virtual DbSet<ParLevel3Value> ParLevel3Value { get; set; }
        public virtual DbSet<ParLevel3Value_Outer> ParLevel3Value_Outer { get; set; }
        public virtual DbSet<ParLevelDefiniton> ParLevelDefiniton { get; set; }
        public virtual DbSet<ParLevel3XParDepartment> ParLevel3XParDepartment { get; set; }
        public virtual DbSet<ParLocal> ParLocal { get; set; }
        public virtual DbSet<ParMeasurementUnit> ParMeasurementUnit { get; set; }
        public virtual DbSet<ParRiskCharacteristicType> ParRiskCharacteristicType { get; set; }
        public virtual DbSet<ParModule> ParModule { get; set; }
        public virtual DbSet<ParModuleXModule> ParModuleXModule { get; set; }
        public virtual DbSet<ParMultipleValues> ParMultipleValues { get; set; }
        public virtual DbSet<ParNotConformityRule> ParNotConformityRule { get; set; }
        public virtual DbSet<ParNotConformityRuleXLevel> ParNotConformityRuleXLevel { get; set; }
        public virtual DbSet<ParRelapse> ParRelapse { get; set; }
        public virtual DbSet<ParSample> ParSample { get; set; }
        public virtual DbSet<ParScoreType> ParScoreType { get; set; }
        public virtual DbSet<ParStructure> ParStructure { get; set; }
        public virtual DbSet<ParStructureGroup> ParStructureGroup { get; set; }
        public virtual DbSet<PenalidadeReincidencia> PenalidadeReincidencia { get; set; }
        public virtual DbSet<Perfil> Perfil { get; set; }
        public virtual DbSet<Period> Period { get; set; }
        public virtual DbSet<Produtos> Produtos { get; set; }
        public virtual DbSet<RecravacaoJson> RecravacaoJson { get; set; }
        public virtual DbSet<Regionais> Regionais { get; set; }
        public virtual DbSet<Result_Level3> Result_Level3 { get; set; }
        public virtual DbSet<Result_Level3_Photos> Result_Level3_Photos { get; set; }
        public virtual DbSet<RoleSGQ> RoleSGQ { get; set; }
        public virtual DbSet<RoleUserSgq> RoleUserSgq { get; set; }
        public virtual DbSet<RoleUserSgqXItemMenu> RoleUserSgqXItemMenu { get; set; }
        public virtual DbSet<Shift> Shift { get; set; }
        public virtual DbSet<TarefaAmostras> TarefaAmostras { get; set; }
        public virtual DbSet<TarefaAvaliacoes> TarefaAvaliacoes { get; set; }
        public virtual DbSet<TarefaMonitoramentos> TarefaMonitoramentos { get; set; }
        public virtual DbSet<Tarefas> Tarefas { get; set; }
        public virtual DbSet<Unidades> Unidades { get; set; }
        public virtual DbSet<Unit> Unit { get; set; }
        public virtual DbSet<UnitUser> UnitUser { get; set; }
        public virtual DbSet<UserSgq> UserSgq { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioPerfilEmpresa> UsuarioPerfilEmpresa { get; set; }
        public virtual DbSet<UsuarioUnidades> UsuarioUnidades { get; set; }
        public virtual DbSet<VerificacaoTipificacao> VerificacaoTipificacao { get; set; }
        public virtual DbSet<VerificacaoTipificacaoComparacao> VerificacaoTipificacaoComparacao { get; set; }
        public virtual DbSet<VerificacaoTipificacaoResultados> VerificacaoTipificacaoResultados { get; set; }
        public virtual DbSet<VerificacaoTipificacaoTarefaIntegracao> VerificacaoTipificacaoTarefaIntegracao { get; set; }
        public virtual DbSet<VerificacaoTipificacaoV2> VerificacaoTipificacaoV2 { get; set; }
        public virtual DbSet<VerificacaoTipificacaoValidacao> VerificacaoTipificacaoValidacao { get; set; }
        public virtual DbSet<VolumeCepDesossa> VolumeCepDesossa { get; set; }
        public virtual DbSet<VolumeCepRecortes> VolumeCepRecortes { get; set; }
        public virtual DbSet<VolumePcc1b> VolumePcc1b { get; set; }
        public virtual DbSet<VolumeProducao> VolumeProducao { get; set; }
        public virtual DbSet<VolumeVacuoGRD> VolumeVacuoGRD { get; set; }
        public virtual DbSet<VTVerificacaoTipificacao> VTVerificacaoTipificacao { get; set; }
        public virtual DbSet<VTVerificacaoTipificacaoResultados> VTVerificacaoTipificacaoResultados { get; set; }
        public virtual DbSet<VTVerificacaoTipificacaoValidacao> VTVerificacaoTipificacaoValidacao { get; set; }
        public virtual DbSet<ParLevel1VariableProduction> ParLevel1VariableProduction { get; set; }
        public virtual DbSet<ParLevel1VariableProductionXLevel1> ParLevel1VariableProductionXLevel1 { get; set; }
        public virtual DbSet<ResultLevel2HeaderField> ResultLevel2HeaderField { get; set; }
        public virtual DbSet<RoleJBS> RoleJBS { get; set; }
        public virtual DbSet<RoleType> RoleType { get; set; }
        public virtual DbSet<ScreenComponent> ScreenComponent { get; set; }
        public virtual DbSet<ParLevel1XModule> ParLevel1XModule { get; set; }
        public virtual DbSet<ImportFormat> ImportFormat { get; set; }
        public virtual DbSet<ImportFormatItem> ImportFormatItem { get; set; }
        public virtual DbSet<RotinaIntegracao> RotinaIntegracao { get; set; }
        public virtual DbSet<ReportXUserSgq> ReportXUserSgq { get; set; }
        public virtual DbSet<ParReportLayoutXReportXUser> ParReportLayoutXReportXUser { get; set; }
        public virtual DbSet<ParReportXFilter> ParReportXFilter { get; set; }
        public virtual DbSet<ReportLayoutItens> ReportLayoutItens { get; set; }
        public virtual DbSet<ParInputTypeValues> ParInputTypeValues { get; set; }
        public System.Data.Entity.DbSet<Dominio.ParGroupParLevel1> ParGroupParLevel1 { get; set; }
        public System.Data.Entity.DbSet<Dominio.ParGroupParLevel1Type> ParGroupParLevel1Type { get; set; }
        public System.Data.Entity.DbSet<Dominio.ParGroupParLevel1XParLevel1> ParGroupParLevel1XParLevel1 { get; set; }
        public virtual DbSet<CollectionLevel2XParReason> CollectionLevel2XParReason { get; set; }
        public virtual DbSet<CollectionLevel2XParDepartment> CollectionLevel2XParDepartment { get; set; }
        public virtual DbSet<ParEvaluationSchedule> ParEvaluationSchedule { get; set; }
        public virtual DbSet<ParReasonType> ParReasonType { get; set; }
        public virtual DbSet<ParReason> ParReason { get; set; }
        public virtual DbSet<DicionarioEstatico> DicionarioEstatico { get; set; }
        public virtual DbSet<ParLevel1XRotinaIntegracao> ParLevel1XRotinaIntegracao { get; set; }
        public virtual DbSet<ParCargo> ParCargo { get; set; }
        public virtual DbSet<ParColaborador> ParColaborador { get; set; }
        public virtual DbSet<ParColaboradorXCargo> ParColaboradorXCargo { get; set; }
        public virtual DbSet<ParCargoXDepartment> ParCargoXDepartment { get; set; }
        public virtual DbSet<ParEvaluationXDepartmentXCargo> ParEvaluationXDepartmentXCargo { get; set; }
        public System.Data.Entity.DbSet<Dominio.ParVinculoPeso> ParVinculoPeso { get; set; }
        public virtual DbSet<IntegracaoSistemica> IntegracaoSistemica { get; set; }
        public virtual DbSet<Collection> Collection { get; set; }
        public virtual DbSet<CollectionPartial> CollectionPartial { get; set; }
        public virtual DbSet<ParLevel3XHelp> ParLevel3XHelp { get; set; }
        public virtual DbSet<ParAlertType> ParAlertType { get; set; }
        public virtual DbSet<ParAlert> ParAlert { get; set; }
        public virtual DbSet<ParDepartmentXRotinaIntegracao> ParDepartmentXRotinaIntegracao { get; set; }
        public virtual DbSet<ParDepartmentXHeaderField> ParDepartmentXHeaderField { get; set; }
        public virtual DbSet<ResourcePT> ResourcePT { get; set; }
        public virtual DbSet<ResourceEN> ResourceEN { get; set; }
        public virtual DbSet<AppScript> AppScript { get; set; }
        public virtual DbSet<CollectionLevel2XParCargo> CollectionLevel2XParCargo { get; set; }
        public virtual DbSet<CollectionLevel2XCluster> CollectionLevel2XCluster { get; set; }
        public virtual DbSet<ParLevel3XModule> ParLevel3XModule { get; set; }
        public virtual DbSet<ParHeaderFieldGeral> ParHeaderFieldGeral { get; set; }
        public virtual DbSet<ParLevelHeaderField> ParLevelHeaderField { get; set; }
        public virtual DbSet<IntegCollectionData> IntegCollectionData { get; set; }
        public virtual DbSet<ParMultipleValuesGeral> ParMultipleValuesGeral { get; set; }
        public virtual DbSet<LogTrack> LogTrack { get; set; }
        public virtual DbSet<Pa_Acompanhamento> Pa_Acompanhamento { get; set; }
        public virtual DbSet<Pa_AcompanhamentoXQuem> Pa_AcompanhamentoXQuem { get; set; }
        public virtual DbSet<Pa_FTA> Pa_FTA { get; set; }
        public virtual DbSet<Pa_Planejamento> Pa_Planejamento { get; set; }
        public virtual DbSet<CollectionLevel2XParHeaderFieldGeral> CollectionLevel2XParHeaderFieldGeral { get; set; }
        public virtual DbSet<ComponenteGenerico> ComponenteGenerico { get; set; }
        public virtual DbSet<ComponenteGenericoTipoColuna> ComponenteGenericoTipoColuna { get; set; }
        public virtual DbSet<ComponenteGenericoColuna> ComponenteGenericoColuna { get; set; }
        public virtual DbSet<ComponenteGenericoValor> ComponenteGenericoValor { get; set; }
        public virtual DbSet<ParHeaderFieldXComponenteGenerico> ParHeaderFieldXComponenteGenerico { get; set; }
        public virtual DbSet<LogError> LogError { get; set; }
        public virtual DbSet<ParAlertXUser> ParAlertXUser { get; set; }
        public virtual DbSet<ParQualification> ParQualification { get; set; }
        public virtual DbSet<PargroupQualification> PargroupQualification { get; set; }
        public virtual DbSet<PargroupQualificationXParQualification> PargroupQualificationXParQualification { get; set; }
        public virtual DbSet<PargroupQualificationXParLevel3Value> PargroupQualificationXParLevel3Value { get; set; }
        public virtual DbSet<Result_Level3XParHeaderFieldGeral> Result_Level3XParHeaderFieldGeral { get; set; }
        public virtual DbSet<ResultLevel3XParQualification> ResultLevel3XParQualification { get; set; }

        public virtual DbSet<Seara.ParFamiliaProduto> ParFamiliaProduto { get; set; }
        public virtual DbSet<Seara.ParProduto> ParProduto { get; set; }
        public virtual DbSet<Seara.ParFamiliaProdutoXParProduto> ParFamiliaProdutoXParProduto { get; set; }
        public virtual DbSet<Seara.CollectionLevel2XParFamiliaProdutoXParProduto> CollectionLevel2XParFamiliaProdutoXParProduto { get; set; }
        public virtual DbSet<Seara.ParLevel1XParFamiliaProduto> ParLevel1XParFamiliaProduto { get; set; }

        public virtual DbSet<Seara.ParVinculoPesoParLevel2> ParVinculoPesoParLevel2 { get; set; }

        public virtual DbSet<Acao> Acao { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
