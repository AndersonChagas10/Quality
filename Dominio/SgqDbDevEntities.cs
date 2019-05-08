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
                    ((BaseModel)entity.Entity).AlterDate = DateTime.Now;
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
        public virtual DbSet<IntegracaoSistemica> IntegracaoSistemica { get; set; }

        public virtual DbSet<IntegCollectionData> IntegCollectionData { get; set; }
        public virtual DbSet<ParLevel1XRotinaIntegracao> ParLevel1XRotinaIntegracao { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Entity<AreasParticipantes>()
            //    .Property(e => e.nCdCaracteristica)
            //    .HasPrecision(12, 0);

            //modelBuilder.Entity<AreasParticipantes>()
            //    .Property(e => e.cNmCaracteristica)
            //    .IsUnicode(false);

            //modelBuilder.Entity<AreasParticipantes>()
            //    .Property(e => e.cNrCaracteristica)
            //    .IsUnicode(false);

            //modelBuilder.Entity<AreasParticipantes>()
            //    .Property(e => e.cSgCaracteristica)
            //    .IsUnicode(false);

            //modelBuilder.Entity<AreasParticipantes>()
            //    .Property(e => e.cIdentificador)
            //    .IsUnicode(false);

            //modelBuilder.Entity<CaracteristicaTipificacao>()
            //    .Property(e => e.nCdCaracteristica)
            //    .HasPrecision(10, 0);

            //modelBuilder.Entity<CaracteristicaTipificacao>()
            //    .Property(e => e.cNmCaracteristica)
            //    .IsUnicode(false);

            //modelBuilder.Entity<CaracteristicaTipificacao>()
            //    .Property(e => e.cNrCaracteristica)
            //    .IsUnicode(false);

            //modelBuilder.Entity<CaracteristicaTipificacao>()
            //    .Property(e => e.cSgCaracteristica)
            //    .IsUnicode(false);

            //modelBuilder.Entity<CaracteristicaTipificacao>()
            //    .Property(e => e.cIdentificador)
            //    .IsUnicode(false);

            //modelBuilder.Entity<CollectionJson>()
            //    .Property(e => e.Level02HeaderJson)
            //    .IsUnicode(false);

            //modelBuilder.Entity<CollectionJson>()
            //    .Property(e => e.Level03ResultJSon)
            //    .IsUnicode(false);

            //modelBuilder.Entity<CollectionJson>()
            //    .Property(e => e.CorrectiveActionJson)
            //    .IsUnicode(false);

            //modelBuilder.Entity<CollectionLevel2>()
            //    .Property(e => e.WeiEvaluation)
            //    .HasPrecision(38, 10);

            //modelBuilder.Entity<CollectionLevel2>()
            //    .Property(e => e.Defects)
            //    .HasPrecision(38, 10);

            //modelBuilder.Entity<CollectionLevel2>()
            //    .Property(e => e.WeiDefects)
            //    .HasPrecision(15, 5);

            //modelBuilder.Entity<CollectionLevel2>()
            //    .HasMany(e => e.CollectionLevel2XParHeaderField)
            //    .WithRequired(e => e.CollectionLevel2)
            //    .HasForeignKey(e => e.CollectionLevel2_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<CollectionLevel2>()
            //    .HasMany(e => e.CorrectiveAction)
            //    .WithRequired(e => e.CollectionLevel2)
            //    .HasForeignKey(e => e.CollectionLevel02Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<CollectionLevel2>()
            //    .HasMany(e => e.Result_Level3)
            //    .WithRequired(e => e.CollectionLevel2)
            //    .HasForeignKey(e => e.CollectionLevel2_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ConsolidationLevel1>()
            //    .Property(e => e.Evaluation)
            //    .HasPrecision(32, 8);

            //modelBuilder.Entity<ConsolidationLevel1>()
            //    .Property(e => e.WeiEvaluation)
            //    .HasPrecision(32, 8);

            //modelBuilder.Entity<ConsolidationLevel1>()
            //    .Property(e => e.EvaluateTotal)
            //    .HasPrecision(32, 8);

            //modelBuilder.Entity<ConsolidationLevel1>()
            //    .Property(e => e.DefectsTotal)
            //    .HasPrecision(10, 5);

            //modelBuilder.Entity<ConsolidationLevel1>()
            //    .Property(e => e.WeiDefects)
            //    .HasPrecision(30, 8);

            //modelBuilder.Entity<ConsolidationLevel1>()
            //    .HasMany(e => e.ConsolidationLevel2)
            //    .WithRequired(e => e.ConsolidationLevel1)
            //    .HasForeignKey(e => e.ConsolidationLevel1_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ConsolidationLevel2>()
            //    .Property(e => e.WeiEvaluation)
            //    .HasPrecision(10, 5);

            //modelBuilder.Entity<ConsolidationLevel2>()
            //    .Property(e => e.EvaluateTotal)
            //    .HasPrecision(10, 5);

            //modelBuilder.Entity<ConsolidationLevel2>()
            //    .Property(e => e.DefectsTotal)
            //    .HasPrecision(10, 5);

            //modelBuilder.Entity<ConsolidationLevel2>()
            //    .Property(e => e.WeiDefects)
            //    .HasPrecision(30, 8);

            //modelBuilder.Entity<Defect>()
            //    .Property(e => e.Defects)
            //    .HasPrecision(30, 8);

            //modelBuilder.Entity<Departamentos>()
            //    .HasMany(e => e.DepartamentoOperacoes)
            //    .WithRequired(e => e.Departamentos)
            //    .HasForeignKey(e => e.Departamento)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Departamentos>()
            //    .HasMany(e => e.TarefaAvaliacoes)
            //    .WithRequired(e => e.Departamentos)
            //    .HasForeignKey(e => e.Departamento)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Departamentos>()
            //    .HasMany(e => e.Tarefas)
            //    .WithOptional(e => e.Departamentos)
            //    .HasForeignKey(e => e.Departamento);

            //modelBuilder.Entity<Departamentos>()
            //    .HasMany(e => e.VolumeProducao)
            //    .WithRequired(e => e.Departamentos)
            //    .HasForeignKey(e => e.Departamento)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Deviation>()
            //    .Property(e => e.Evaluation)
            //    .HasPrecision(18, 0);

            //modelBuilder.Entity<Deviation>()
            //    .Property(e => e.Sample)
            //    .HasPrecision(18, 0);

            //modelBuilder.Entity<Deviation>()
            //    .Property(e => e.Defects)
            //    .HasPrecision(10, 5);

            //modelBuilder.Entity<EmailContent>()
            //    .HasMany(e => e.CorrectiveAction)
            //    .WithOptional(e => e.EmailContent)
            //    .HasForeignKey(e => e.EmailContent_Id);

            //modelBuilder.Entity<EmailContent>()
            //    .HasMany(e => e.Deviation)
            //    .WithOptional(e => e.EmailContent)
            //    .HasForeignKey(e => e.EmailContent_Id);

            //modelBuilder.Entity<ItemMenu>()
            //    .Property(e => e.Name)
            //    .IsUnicode(false);

            //modelBuilder.Entity<ItemMenu>()
            //    .Property(e => e.Icon)
            //    .IsUnicode(false);

            //modelBuilder.Entity<ItemMenu>()
            //    .Property(e => e.Url)
            //    .IsUnicode(false);

            //modelBuilder.Entity<ItemMenu>()
            //    .Property(e => e.Resource)
            //    .IsUnicode(false);

            //modelBuilder.Entity<LogJson>()
            //    .Property(e => e.result)
            //    .IsUnicode(false);

            //modelBuilder.Entity<LogJson>()
            //    .Property(e => e.log)
            //    .IsUnicode(false);

            //modelBuilder.Entity<LogSgq>()
            //    .Property(e => e.Level)
            //    .IsUnicode(false);

            //modelBuilder.Entity<LogSgq>()
            //    .Property(e => e.Call_Site)
            //    .IsUnicode(false);

            //modelBuilder.Entity<LogSgq>()
            //    .Property(e => e.Exception_Type)
            //    .IsUnicode(false);

            //modelBuilder.Entity<LogSgq>()
            //    .Property(e => e.Exception_Message)
            //    .IsUnicode(false);

            //modelBuilder.Entity<LogSgq>()
            //    .Property(e => e.Stack_Trace)
            //    .IsUnicode(false);

            //modelBuilder.Entity<LogSgq>()
            //    .Property(e => e.Additional_Info)
            //    .IsUnicode(false);

            //modelBuilder.Entity<LogSgq>()
            //    .Property(e => e.Email)
            //    .IsUnicode(false);

            //modelBuilder.Entity<LogSgq>()
            //    .Property(e => e.Second_Log_Path)
            //    .IsUnicode(false);

            //modelBuilder.Entity<LogSgqGlobal>()
            //    .Property(e => e.Level)
            //    .IsUnicode(false);

            //modelBuilder.Entity<LogSgqGlobal>()
            //    .Property(e => e.Call_Site)
            //    .IsUnicode(false);

            //modelBuilder.Entity<LogSgqGlobal>()
            //    .Property(e => e.Exception_Type)
            //    .IsUnicode(false);

            //modelBuilder.Entity<LogSgqGlobal>()
            //    .Property(e => e.Exception_Message)
            //    .IsUnicode(false);

            //modelBuilder.Entity<LogSgqGlobal>()
            //    .Property(e => e.Stack_Trace)
            //    .IsUnicode(false);

            //modelBuilder.Entity<LogSgqGlobal>()
            //    .Property(e => e.Additional_Info)
            //    .IsUnicode(false);

            //modelBuilder.Entity<LogSgqGlobal>()
            //    .Property(e => e.Object)
            //    .IsUnicode(false);

            //modelBuilder.Entity<LogSgqGlobal>()
            //    .Property(e => e.email)
            //    .IsUnicode(false);

            //modelBuilder.Entity<LogSgqGlobal>()
            //    .Property(e => e.Second_Log_Path)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Metas>()
            //    .Property(e => e.Meta)
            //    .HasPrecision(5, 2);

            //modelBuilder.Entity<Monitoramentos>()
            //    .HasMany(e => e.TarefaMonitoramentos)
            //    .WithRequired(e => e.Monitoramentos)
            //    .HasForeignKey(e => e.Monitoramento)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Monitoramentos>()
            //    .HasMany(e => e.VerificacaoTipificacaoTarefaIntegracao)
            //    .WithRequired(e => e.Monitoramentos)
            //    .HasForeignKey(e => e.TarefaId)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Operacoes>()
            //    .HasMany(e => e.DepartamentoOperacoes)
            //    .WithRequired(e => e.Operacoes)
            //    .HasForeignKey(e => e.Operacao)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Operacoes>()
            //    .HasMany(e => e.FamiliaProdutos)
            //    .WithOptional(e => e.Operacoes)
            //    .HasForeignKey(e => e.Operacao);

            //modelBuilder.Entity<Operacoes>()
            //    .HasMany(e => e.Horarios)
            //    .WithRequired(e => e.Operacoes)
            //    .HasForeignKey(e => e.OperacaoId)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Operacoes>()
            //    .HasMany(e => e.Metas)
            //    .WithRequired(e => e.Operacoes)
            //    .HasForeignKey(e => e.Operacao)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Operacoes>()
            //    .HasMany(e => e.TarefaAvaliacoes)
            //    .WithRequired(e => e.Operacoes)
            //    .HasForeignKey(e => e.Operacao)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Operacoes>()
            //    .HasMany(e => e.Tarefas)
            //    .WithRequired(e => e.Operacoes)
            //    .HasForeignKey(e => e.Operacao)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Operacoes>()
            //    .HasMany(e => e.VolumeProducao)
            //    .WithRequired(e => e.Operacoes)
            //    .HasForeignKey(e => e.Operacao)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParCluster>()
            //    .HasMany(e => e.ParClusterXModule)
            //    .WithRequired(e => e.ParCluster)
            //    .HasForeignKey(e => e.ParCluster_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParCluster>()
            //    .HasMany(e => e.ParCompanyCluster)
            //    .WithRequired(e => e.ParCluster)
            //    .HasForeignKey(e => e.ParCluster_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParCluster>()
            //    .HasMany(e => e.ParLevel1XCluster)
            //    .WithRequired(e => e.ParCluster)
            //    .HasForeignKey(e => e.ParCluster_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParClusterGroup>()
            //    .HasMany(e => e.ParCluster)
            //    .WithRequired(e => e.ParClusterGroup)
            //    .HasForeignKey(e => e.ParClusterGroup_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParClusterXModule>()
            //    .Property(e => e.Points)
            //    .HasPrecision(10, 5);

            //modelBuilder.Entity<ParCompany>()
            //    .Property(e => e.IntegrationId)
            //    .HasPrecision(18, 0);

            //modelBuilder.Entity<ParCompany>()
            //    .HasMany(e => e.ConsolidationLevel1)
            //    .WithRequired(e => e.ParCompany)
            //    .HasForeignKey(e => e.UnitId)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParCompany>()
            //    .HasMany(e => e.Defect)
            //    .WithRequired(e => e.ParCompany)
            //    .HasForeignKey(e => e.ParCompany_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParCompany>()
            //    .HasMany(e => e.ParLevel3EvaluationSample)
            //    .WithOptional(e => e.ParCompany)
            //    .HasForeignKey(e => e.ParCompany_Id);

            //modelBuilder.Entity<ParCompany>()
            //    .HasMany(e => e.VolumeCepDesossa)
            //    .WithOptional(e => e.ParCompany)
            //    .HasForeignKey(e => e.ParCompany_id);

            //modelBuilder.Entity<ParCompany>()
            //    .HasMany(e => e.VolumeCepRecortes)
            //    .WithOptional(e => e.ParCompany)
            //    .HasForeignKey(e => e.ParCompany_id);

            //modelBuilder.Entity<ParCompany>()
            //    .HasMany(e => e.VolumePcc1b)
            //    .WithOptional(e => e.ParCompany)
            //    .HasForeignKey(e => e.ParCompany_id);

            //modelBuilder.Entity<ParCompany>()
            //    .HasMany(e => e.VolumeVacuoGRD)
            //    .WithOptional(e => e.ParCompany)
            //    .HasForeignKey(e => e.ParCompany_id);

            //modelBuilder.Entity<ParCompany>()
            //    .HasMany(e => e.ParCompanyCluster)
            //    .WithRequired(e => e.ParCompany)
            //    .HasForeignKey(e => e.ParCompany_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParCompany>()
            //    .HasMany(e => e.ParCompanyXStructure)
            //    .WithRequired(e => e.ParCompany)
            //    .HasForeignKey(e => e.ParCompany_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParCompany>()
            //    .HasMany(e => e.ParCompanyXUserSgq)
            //    .WithRequired(e => e.ParCompany)
            //    .HasForeignKey(e => e.ParCompany_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParCompany>()
            //    .HasMany(e => e.ParEvaluation)
            //    .WithOptional(e => e.ParCompany)
            //    .HasForeignKey(e => e.ParCompany_Id);

            //modelBuilder.Entity<ParCompany>()
            //    .HasMany(e => e.ParGoal)
            //    .WithOptional(e => e.ParCompany)
            //    .HasForeignKey(e => e.ParCompany_Id);

            //modelBuilder.Entity<ParCompany>()
            //    .HasMany(e => e.ParLevel2ControlCompany)
            //    .WithOptional(e => e.ParCompany)
            //    .HasForeignKey(e => e.ParCompany_Id);

            //modelBuilder.Entity<ParCompany>()
            //    .HasMany(e => e.ParLevel3Level2)
            //    .WithOptional(e => e.ParCompany)
            //    .HasForeignKey(e => e.ParCompany_Id);

            //modelBuilder.Entity<ParCompany>()
            //    .HasMany(e => e.ParLevel3Value)
            //    .WithOptional(e => e.ParCompany)
            //    .HasForeignKey(e => e.ParCompany_Id);

            //modelBuilder.Entity<ParCompany>()
            //    .HasMany(e => e.ParNotConformityRuleXLevel)
            //    .WithOptional(e => e.ParCompany)
            //    .HasForeignKey(e => e.ParCompany_Id);

            //modelBuilder.Entity<ParCompany>()
            //    .HasMany(e => e.ParSample)
            //    .WithOptional(e => e.ParCompany)
            //    .HasForeignKey(e => e.ParCompany_Id);

            //modelBuilder.Entity<ParConsolidationType>()
            //    .HasMany(e => e.ParLevel1)
            //    .WithRequired(e => e.ParConsolidationType)
            //    .HasForeignKey(e => e.ParConsolidationType_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParCounter>()
            //    .HasMany(e => e.ParCounterXLocal)
            //    .WithRequired(e => e.ParCounter)
            //    .HasForeignKey(e => e.ParCounter_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParCriticalLevel>()
            //    .HasMany(e => e.ParLevel1XCluster)
            //    .WithOptional(e => e.ParCriticalLevel)
            //    .HasForeignKey(e => e.ParCriticalLevel_Id);

            //modelBuilder.Entity<ParDepartment>()
            //    .HasMany(e => e.ParLevel2)
            //    .WithRequired(e => e.ParDepartment)
            //    .HasForeignKey(e => e.ParDepartment_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParFieldType>()
            //    .HasMany(e => e.CollectionLevel2XParHeaderField)
            //    .WithRequired(e => e.ParFieldType)
            //    .HasForeignKey(e => e.ParFieldType_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParFieldType>()
            //    .HasMany(e => e.ParHeaderField)
            //    .WithRequired(e => e.ParFieldType)
            //    .HasForeignKey(e => e.ParFieldType_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParFrequency>()
            //    .HasMany(e => e.ParLevel1)
            //    .WithRequired(e => e.ParFrequency)
            //    .HasForeignKey(e => e.ParFrequency_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParFrequency>()
            //    .HasMany(e => e.ParLevel2)
            //    .WithRequired(e => e.ParFrequency)
            //    .HasForeignKey(e => e.ParFrequency_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParFrequency>()
            //    .HasMany(e => e.ParRelapse)
            //    .WithRequired(e => e.ParFrequency)
            //    .HasForeignKey(e => e.ParFrequency_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParGoal>()
            //    .Property(e => e.PercentValue)
            //    .HasPrecision(10, 5);

            //modelBuilder.Entity<ParGoalScorecard>()
            //    .Property(e => e.PercentValueMid)
            //    .HasPrecision(25, 7);

            //modelBuilder.Entity<ParGoalScorecard>()
            //    .Property(e => e.PercentValueHigh)
            //    .HasPrecision(25, 7);

            //modelBuilder.Entity<ParHeaderField>()
            //    .HasMany(e => e.CollectionLevel2XParHeaderField)
            //    .WithRequired(e => e.ParHeaderField)
            //    .HasForeignKey(e => e.ParHeaderField_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParHeaderField>()
            //    .HasMany(e => e.ParLevel1XHeaderField)
            //    .WithRequired(e => e.ParHeaderField)
            //    .HasForeignKey(e => e.ParHeaderField_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParHeaderField>()
            //    .HasMany(e => e.ParMultipleValues)
            //    .WithRequired(e => e.ParHeaderField)
            //    .HasForeignKey(e => e.ParHeaderField_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParLataImagens>()
            //    .Property(e => e.PathFile)
            //    .IsUnicode(false);

            //modelBuilder.Entity<ParLataImagens>()
            //    .Property(e => e.FileName)
            //    .IsUnicode(false);

            //modelBuilder.Entity<ParLevel1>()
            //    .HasMany(e => e.ConsolidationLevel1)
            //    .WithRequired(e => e.ParLevel1)
            //    .HasForeignKey(e => e.ParLevel1_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParLevel1>()
            //    .HasMany(e => e.Defect)
            //    .WithRequired(e => e.ParLevel1)
            //    .HasForeignKey(e => e.ParLevel1_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParLevel1>()
            //    .HasMany(e => e.ParCounterXLocal)
            //    .WithOptional(e => e.ParLevel1)
            //    .HasForeignKey(e => e.ParLevel1_Id);

            //modelBuilder.Entity<ParLevel1>()
            //    .HasMany(e => e.ParGoal)
            //    .WithRequired(e => e.ParLevel1)
            //    .HasForeignKey(e => e.ParLevel1_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParLevel1>()
            //    .HasMany(e => e.ParLevel3EvaluationSample)
            //    .WithOptional(e => e.ParLevel1)
            //    .HasForeignKey(e => e.ParLevel1_Id);

            //modelBuilder.Entity<ParLevel1>()
            //    .HasMany(e => e.ParLevel3Value)
            //    .WithOptional(e => e.ParLevel1)
            //    .HasForeignKey(e => e.ParLevel1_Id);

            //modelBuilder.Entity<ParLevel1>()
            //    .HasMany(e => e.VolumeCepDesossa)
            //    .WithOptional(e => e.ParLevel1)
            //    .HasForeignKey(e => e.ParLevel1_id);

            //modelBuilder.Entity<ParLevel1>()
            //    .HasMany(e => e.VolumeCepRecortes)
            //    .WithOptional(e => e.ParLevel1)
            //    .HasForeignKey(e => e.ParLevel1_id);

            //modelBuilder.Entity<ParLevel1>()
            //    .HasMany(e => e.VolumePcc1b)
            //    .WithOptional(e => e.ParLevel1)
            //    .HasForeignKey(e => e.ParLevel1_id);

            //modelBuilder.Entity<ParLevel1>()
            //    .HasMany(e => e.VolumeVacuoGRD)
            //    .WithOptional(e => e.ParLevel1)
            //    .HasForeignKey(e => e.ParLevel1_id);

            //modelBuilder.Entity<ParLevel1>()
            //    .HasMany(e => e.ParLevel2Level1)
            //    .WithRequired(e => e.ParLevel1)
            //    .HasForeignKey(e => e.ParLevel1_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParLevel1>()
            //    .HasMany(e => e.ParLevel1XCluster)
            //    .WithRequired(e => e.ParLevel1)
            //    .HasForeignKey(e => e.ParLevel1_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParLevel1>()
            //    .HasMany(e => e.ParLevel1XHeaderField)
            //    .WithRequired(e => e.ParLevel1)
            //    .HasForeignKey(e => e.ParLevel1_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParLevel1>()
            //    .HasMany(e => e.ParLevel2ControlCompany)
            //    .WithOptional(e => e.ParLevel1)
            //    .HasForeignKey(e => e.ParLevel1_Id);

            //modelBuilder.Entity<ParLevel1>()
            //    .HasMany(e => e.ParLevel3Level2Level1)
            //    .WithRequired(e => e.ParLevel1)
            //    .HasForeignKey(e => e.ParLevel1_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParLevel1>()
            //    .HasMany(e => e.ParNotConformityRuleXLevel)
            //    .WithOptional(e => e.ParLevel1)
            //    .HasForeignKey(e => e.ParLevel1_Id);

            //modelBuilder.Entity<ParLevel1>()
            //    .HasMany(e => e.ParRelapse)
            //    .WithOptional(e => e.ParLevel1)
            //    .HasForeignKey(e => e.ParLevel1_Id);

            //modelBuilder.Entity<ParLevel1XCluster>()
            //    .Property(e => e.Points)
            //    .HasPrecision(10, 5);

            //modelBuilder.Entity<ParLevel1XHeaderField>()
            //    .Property(e => e.HeaderFieldGroup)
            //    .IsUnicode(false);

            //modelBuilder.Entity<ParLevel2>()
            //    .HasMany(e => e.CollectionLevel2)
            //    .WithRequired(e => e.ParLevel2)
            //    .HasForeignKey(e => e.ParLevel2_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParLevel2>()
            //    .HasMany(e => e.ConsolidationLevel2)
            //    .WithRequired(e => e.ParLevel2)
            //    .HasForeignKey(e => e.ParLevel2_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParLevel2>()
            //    .HasMany(e => e.ParCounterXLocal)
            //    .WithOptional(e => e.ParLevel2)
            //    .HasForeignKey(e => e.ParLevel2_Id);

            //modelBuilder.Entity<ParLevel2>()
            //    .HasMany(e => e.ParEvaluation)
            //    .WithRequired(e => e.ParLevel2)
            //    .HasForeignKey(e => e.ParLevel2_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParLevel2>()
            //    .HasMany(e => e.ParLevel3EvaluationSample)
            //    .WithOptional(e => e.ParLevel2)
            //    .HasForeignKey(e => e.ParLevel2_Id);

            //modelBuilder.Entity<ParLevel2>()
            //    .HasMany(e => e.ParLevel3Value)
            //    .WithOptional(e => e.ParLevel2)
            //    .HasForeignKey(e => e.ParLevel2_Id);

            //modelBuilder.Entity<ParLevel2>()
            //    .HasMany(e => e.ParLevel2Level1)
            //    .WithRequired(e => e.ParLevel2)
            //    .HasForeignKey(e => e.ParLevel2_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParLevel2>()
            //    .HasMany(e => e.ParLevel2ControlCompany)
            //    .WithOptional(e => e.ParLevel2)
            //    .HasForeignKey(e => e.ParLevel2_Id);

            //modelBuilder.Entity<ParLevel2>()
            //    .HasMany(e => e.ParLevel3Group)
            //    .WithRequired(e => e.ParLevel2)
            //    .HasForeignKey(e => e.ParLevel2_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParLevel2>()
            //    .HasMany(e => e.ParLevel3Level2)
            //    .WithRequired(e => e.ParLevel2)
            //    .HasForeignKey(e => e.ParLevel2_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParLevel2>()
            //    .HasMany(e => e.ParNotConformityRuleXLevel)
            //    .WithOptional(e => e.ParLevel2)
            //    .HasForeignKey(e => e.ParLevel2_Id);

            //modelBuilder.Entity<ParLevel2>()
            //    .HasMany(e => e.ParRelapse)
            //    .WithOptional(e => e.ParLevel2)
            //    .HasForeignKey(e => e.ParLevel2_Id);

            //modelBuilder.Entity<ParLevel2>()
            //    .HasMany(e => e.ParSample)
            //    .WithRequired(e => e.ParLevel2)
            //    .HasForeignKey(e => e.ParLevel2_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParLevel3>()
            //    .HasMany(e => e.ParCounterXLocal)
            //    .WithOptional(e => e.ParLevel3)
            //    .HasForeignKey(e => e.ParLevel3_Id);

            //modelBuilder.Entity<ParLevel3>()
            //    .HasMany(e => e.ParLevel3EvaluationSample)
            //    .WithRequired(e => e.ParLevel3)
            //    .HasForeignKey(e => e.ParLevel3_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParLevel3>()
            //    .HasMany(e => e.ParLevel3Level2)
            //    .WithRequired(e => e.ParLevel3)
            //    .HasForeignKey(e => e.ParLevel3_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParLevel3>()
            //    .HasMany(e => e.ParLevel3Value)
            //    .WithRequired(e => e.ParLevel3)
            //    .HasForeignKey(e => e.ParLevel3_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParLevel3>()
            //    .HasMany(e => e.ParNotConformityRuleXLevel)
            //    .WithOptional(e => e.ParLevel3)
            //    .HasForeignKey(e => e.ParLevel3_Id);

            //modelBuilder.Entity<ParLevel3>()
            //    .HasMany(e => e.ParRelapse)
            //    .WithOptional(e => e.ParLevel3)
            //    .HasForeignKey(e => e.ParLevel3_Id);

            //modelBuilder.Entity<ParLevel3>()
            //    .HasMany(e => e.Result_Level3)
            //    .WithRequired(e => e.ParLevel3)
            //    .HasForeignKey(e => e.ParLevel3_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParLevel3BoolFalse>()
            //    .HasMany(e => e.ParLevel3Value)
            //    .WithOptional(e => e.ParLevel3BoolFalse)
            //    .HasForeignKey(e => e.ParLevel3BoolFalse_Id);

            //modelBuilder.Entity<ParLevel3BoolTrue>()
            //    .HasMany(e => e.ParLevel3Value)
            //    .WithOptional(e => e.ParLevel3BoolTrue)
            //    .HasForeignKey(e => e.ParLevel3BoolTrue_Id);

            //modelBuilder.Entity<ParLevel3EvaluationSample>()
            //    .Property(e => e.SampleNumber)
            //    .HasPrecision(18, 0);

            //modelBuilder.Entity<ParLevel3EvaluationSample>()
            //    .Property(e => e.EvaluationNumber)
            //    .HasPrecision(18, 0);

            //modelBuilder.Entity<ParLevel3EvaluationSample>()
            //    .Property(e => e.EvaluationInterval)
            //    .IsUnicode(false);

            //modelBuilder.Entity<ParLevel3Group>()
            //    .HasMany(e => e.ParLevel3Level2)
            //    .WithOptional(e => e.ParLevel3Group)
            //    .HasForeignKey(e => e.ParLevel3Group_Id);

            //modelBuilder.Entity<ParLevel3InputType>()
            //    .Property(e => e.Sampling)
            //    .HasPrecision(10, 5);

            //modelBuilder.Entity<ParLevel3InputType>()
            //    .HasMany(e => e.ParLevel3Value)
            //    .WithRequired(e => e.ParLevel3InputType)
            //    .HasForeignKey(e => e.ParLevel3InputType_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParLevel3Level2>()
            //    .Property(e => e.Weight)
            //    .HasPrecision(10, 5);

            //modelBuilder.Entity<ParLevel3Level2>()
            //    .HasMany(e => e.ParLevel3Level2Level1)
            //    .WithRequired(e => e.ParLevel3Level2)
            //    .HasForeignKey(e => e.ParLevel3Level2_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParLevel3Value>()
            //    .Property(e => e.IntervalMin)
            //    .HasPrecision(38, 10);

            //modelBuilder.Entity<ParLevel3Value>()
            //    .Property(e => e.IntervalMax)
            //    .HasPrecision(38, 10);

            //modelBuilder.Entity<ParLevel3Value>()
            //    .Property(e => e.DynamicValue)
            //    .IsUnicode(false);

            //modelBuilder.Entity<ParLevel3Value_Outer>()
            //    .Property(e => e.LimInferior)
            //    .HasPrecision(18, 5);

            //modelBuilder.Entity<ParLevel3Value_Outer>()
            //    .Property(e => e.LimSuperior)
            //    .HasPrecision(18, 5);

            //modelBuilder.Entity<ParLevelDefiniton>()
            //    .HasMany(e => e.ParHeaderField)
            //    .WithRequired(e => e.ParLevelDefiniton)
            //    .HasForeignKey(e => e.ParLevelDefinition_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParLocal>()
            //    .HasMany(e => e.ParCounterXLocal)
            //    .WithRequired(e => e.ParLocal)
            //    .HasForeignKey(e => e.ParLocal_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParMeasurementUnit>()
            //    .HasMany(e => e.ParLevel3Value)
            //    .WithOptional(e => e.ParMeasurementUnit)
            //    .HasForeignKey(e => e.ParMeasurementUnit_Id);

            //modelBuilder.Entity<ParModule>()
            //    .HasMany(e => e.ParClusterXModule)
            //    .WithRequired(e => e.ParModule)
            //    .HasForeignKey(e => e.ParModule_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParModule>()
            //    .HasMany(e => e.ParModuleXModule)
            //    .WithRequired(e => e.ParModule)
            //    .HasForeignKey(e => e.ParModuleChild_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParModule>()
            //    .HasMany(e => e.ParModuleXModule1)
            //    .WithRequired(e => e.ParModule1)
            //    .HasForeignKey(e => e.ParModuleParent_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParMultipleValues>()
            //    .Property(e => e.PunishmentValue)
            //    .HasPrecision(38, 10);

            //modelBuilder.Entity<ParNotConformityRule>()
            //    .HasMany(e => e.ParNotConformityRuleXLevel)
            //    .WithRequired(e => e.ParNotConformityRule)
            //    .HasForeignKey(e => e.ParNotConformityRule_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParNotConformityRuleXLevel>()
            //    .Property(e => e.Value)
            //    .HasPrecision(38, 10);

            //modelBuilder.Entity<ParScoreType>()
            //    .HasMany(e => e.ParLevel1)
            //    .WithOptional(e => e.ParScoreType)
            //    .HasForeignKey(e => e.ParScoreType_Id);

            //modelBuilder.Entity<ParStructure>()
            //    .HasMany(e => e.ParCompanyXStructure)
            //    .WithRequired(e => e.ParStructure)
            //    .HasForeignKey(e => e.ParStructure_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ParStructureGroup>()
            //    .HasMany(e => e.ParStructure)
            //    .WithRequired(e => e.ParStructureGroup)
            //    .HasForeignKey(e => e.ParStructureGroup_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Perfil>()
            //    .Property(e => e.nCdPerfil)
            //    .HasPrecision(10, 0);

            //modelBuilder.Entity<Perfil>()
            //    .Property(e => e.cNmPerfil)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Produtos>()
            //    .HasMany(e => e.FamiliaProdutos)
            //    .WithRequired(e => e.Produtos)
            //    .HasForeignKey(e => e.Produto)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Produtos>()
            //    .HasMany(e => e.Tarefas)
            //    .WithOptional(e => e.Produtos)
            //    .HasForeignKey(e => e.Produto);

            //modelBuilder.Entity<Regionais>()
            //    .Property(e => e.Nome)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Result_Level3>()
            //    .Property(e => e.Weight)
            //    .HasPrecision(10, 5);

            //modelBuilder.Entity<Result_Level3>()
            //    .Property(e => e.Defects)
            //    .HasPrecision(10, 5);

            //modelBuilder.Entity<Result_Level3>()
            //    .Property(e => e.PunishmentValue)
            //    .HasPrecision(10, 5);

            //modelBuilder.Entity<Result_Level3>()
            //    .Property(e => e.WeiEvaluation)
            //    .HasPrecision(10, 5);

            //modelBuilder.Entity<Result_Level3>()
            //    .Property(e => e.Evaluation)
            //    .HasPrecision(10, 5);

            //modelBuilder.Entity<Result_Level3>()
            //    .Property(e => e.WeiDefects)
            //    .HasPrecision(30, 8);

            //modelBuilder.Entity<Result_Level3>()
            //    .Property(e => e.CT4Eva3)
            //    .HasPrecision(10, 5);

            //modelBuilder.Entity<Result_Level3>()
            //    .Property(e => e.Sampling)
            //    .HasPrecision(10, 5);

            //modelBuilder.Entity<Result_Level3_Photos>()
            //    .Property(e => e.Photo_Thumbnaills)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Result_Level3_Photos>()
            //    .Property(e => e.Photo)
            //    .IsUnicode(false);

            //modelBuilder.Entity<RoleSGQ>()
            //    .Property(e => e.Role)
            //    .IsFixedLength();

            //modelBuilder.Entity<RoleUserSgqXItemMenu>()
            //    .Property(e => e.Name)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Tarefas>()
            //    .HasMany(e => e.TarefaAmostras)
            //    .WithRequired(e => e.Tarefas)
            //    .HasForeignKey(e => e.Tarefa)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Tarefas>()
            //    .HasMany(e => e.TarefaAvaliacoes)
            //    .WithRequired(e => e.Tarefas)
            //    .HasForeignKey(e => e.Tarefa)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Tarefas>()
            //    .HasMany(e => e.TarefaMonitoramentos)
            //    .WithRequired(e => e.Tarefas)
            //    .HasForeignKey(e => e.Tarefa)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Unidades>()
            //    .HasMany(e => e.Equipamentos)
            //    .WithRequired(e => e.Unidades)
            //    .HasForeignKey(e => e.Unidade)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Unidades>()
            //    .HasMany(e => e.FamiliaProdutos)
            //    .WithOptional(e => e.Unidades)
            //    .HasForeignKey(e => e.Unidade);

            //modelBuilder.Entity<Unidades>()
            //    .HasMany(e => e.Horarios)
            //    .WithOptional(e => e.Unidades)
            //    .HasForeignKey(e => e.UnidadeId);

            //modelBuilder.Entity<Unidades>()
            //    .HasMany(e => e.TarefaAmostras)
            //    .WithOptional(e => e.Unidades)
            //    .HasForeignKey(e => e.Unidade);

            //modelBuilder.Entity<Unidades>()
            //    .HasMany(e => e.TarefaAvaliacoes)
            //    .WithOptional(e => e.Unidades)
            //    .HasForeignKey(e => e.Unidade);

            //modelBuilder.Entity<Unidades>()
            //    .HasMany(e => e.TarefaMonitoramentos)
            //    .WithOptional(e => e.Unidades)
            //    .HasForeignKey(e => e.Unidade);

            //modelBuilder.Entity<Unidades>()
            //    .HasMany(e => e.UsuarioUnidades)
            //    .WithRequired(e => e.Unidades)
            //    .HasForeignKey(e => e.Unidade)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Unidades>()
            //    .HasMany(e => e.VerificacaoTipificacao)
            //    .WithRequired(e => e.Unidades)
            //    .HasForeignKey(e => e.UnidadeId)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Unidades>()
            //    .HasMany(e => e.VolumeProducao)
            //    .WithRequired(e => e.Unidades)
            //    .HasForeignKey(e => e.Unidade)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Unit>()
            //    .HasMany(e => e.UnitUser)
            //    .WithRequired(e => e.Unit)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<UserSgq>()
            //    .HasMany(e => e.CollectionLevel2)
            //    .WithRequired(e => e.UserSgq)
            //    .HasForeignKey(e => e.AuditorId)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<UserSgq>()
            //    .HasMany(e => e.CorrectiveAction)
            //    .WithRequired(e => e.UserSgq)
            //    .HasForeignKey(e => e.AuditorId)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<UserSgq>()
            //    .HasMany(e => e.CorrectiveAction1)
            //    .WithOptional(e => e.UserSgq1)
            //    .HasForeignKey(e => e.TechinicalId);

            //modelBuilder.Entity<UserSgq>()
            //    .HasMany(e => e.CorrectiveAction2)
            //    .WithOptional(e => e.UserSgq2)
            //    .HasForeignKey(e => e.SlaughterId);

            //modelBuilder.Entity<UserSgq>()
            //    .HasMany(e => e.ParCompanyXUserSgq)
            //    .WithRequired(e => e.UserSgq)
            //    .HasForeignKey(e => e.UserSgq_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<UserSgq>()
            //    .HasMany(e => e.UnitUser)
            //    .WithRequired(e => e.UserSgq)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Usuario>()
            //    .Property(e => e.nCdUsuario)
            //    .HasPrecision(10, 0);

            //modelBuilder.Entity<Usuario>()
            //    .Property(e => e.cNmUsuario)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Usuario>()
            //    .Property(e => e.cSigla)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Usuario>()
            //    .Property(e => e.cEMail)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Usuario>()
            //    .Property(e => e.cTelefone)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Usuario>()
            //    .Property(e => e.cCelular)
            //    .IsUnicode(false);

            //modelBuilder.Entity<UsuarioPerfilEmpresa>()
            //    .Property(e => e.nCdUsuario)
            //    .HasPrecision(10, 0);

            //modelBuilder.Entity<UsuarioPerfilEmpresa>()
            //    .Property(e => e.nCdPerfil)
            //    .HasPrecision(10, 0);

            //modelBuilder.Entity<UsuarioPerfilEmpresa>()
            //    .Property(e => e.nCdEmpresa)
            //    .HasPrecision(10, 0);

            //modelBuilder.Entity<VerificacaoTipificacao>()
            //    .Property(e => e.Chave)
            //    .IsUnicode(false);

            //modelBuilder.Entity<VerificacaoTipificacaoComparacao>()
            //    .Property(e => e.Identificador)
            //    .IsUnicode(false);

            //modelBuilder.Entity<VerificacaoTipificacaoResultados>()
            //    .Property(e => e.Chave)
            //    .IsUnicode(false);

            //modelBuilder.Entity<VerificacaoTipificacaoValidacao>()
            //    .Property(e => e.cIdentificadorTipificacao)
            //    .IsUnicode(false);

            //modelBuilder.Entity<VolumeCepDesossa>()
            //    .Property(e => e.Departamento)
            //    .IsUnicode(false);

            //modelBuilder.Entity<VolumeCepRecortes>()
            //    .Property(e => e.Departamento)
            //    .IsUnicode(false);

            //modelBuilder.Entity<VolumePcc1b>()
            //    .Property(e => e.Departamento)
            //    .IsUnicode(false);

            //modelBuilder.Entity<VolumePcc1b>()
            //    .Property(e => e.Meta)
            //    .HasPrecision(10, 5);

            //modelBuilder.Entity<VolumeProducao>()
            //    .Property(e => e.Meta)
            //    .HasPrecision(5, 2);

            //modelBuilder.Entity<VolumeProducao>()
            //    .Property(e => e.ToleranciaDia)
            //    .HasPrecision(11, 8);

            //modelBuilder.Entity<VolumeProducao>()
            //    .Property(e => e.Nivel1)
            //    .HasPrecision(11, 8);

            //modelBuilder.Entity<VolumeProducao>()
            //    .Property(e => e.Nivel2)
            //    .HasPrecision(11, 8);

            //modelBuilder.Entity<VolumeProducao>()
            //    .Property(e => e.Nivel3)
            //    .HasPrecision(11, 8);

            //modelBuilder.Entity<VolumeVacuoGRD>()
            //    .Property(e => e.Departamento)
            //    .IsUnicode(false);

            //modelBuilder.Entity<VTVerificacaoTipificacaoValidacao>()
            //    .Property(e => e.cIdentificadorTipificacao)
            //    .IsUnicode(false);

            //modelBuilder.Entity<ResultLevel2HeaderField>()
            //    .Property(e => e.PunishmentValue)
            //    .HasPrecision(10, 5);

            //modelBuilder.Entity<RoleJBS>()
            //    .Property(e => e.Role)
            //    .IsFixedLength();
        }

        public System.Data.Entity.DbSet<Dominio.ParVinculoPeso> ParGroupParLevel1XParLevel3 { get; set; }
    }
}
