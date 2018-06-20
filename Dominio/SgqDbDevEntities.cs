namespace Dominio
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SgqDbDevEntities : DbContext
    {
        public SgqDbDevEntities()
            : base("name=DefaultConnection")
        {
        }

        public virtual DbSet<Acoes> Acoes { get; set; }
        public virtual DbSet<AcoesCorretivas> AcoesCorretivas { get; set; }
        public virtual DbSet<AcoesPreventivas> AcoesPreventivas { get; set; }
        public virtual DbSet<AcompanhamentoTarefa> AcompanhamentoTarefa { get; set; }
        public virtual DbSet<AggregatedCounter> AggregatedCounter { get; set; }
        public virtual DbSet<Alertas> Alertas { get; set; }
        public virtual DbSet<AreasParticipantes> AreasParticipantes { get; set; }
        public virtual DbSet<BkpCollection> BkpCollection { get; set; }
        public virtual DbSet<Cabecalho> Cabecalho { get; set; }
        public virtual DbSet<Campo> Campo { get; set; }
        public virtual DbSet<CaracteristicaTipificacao> CaracteristicaTipificacao { get; set; }
        public virtual DbSet<CaracteristicaTipificacaoSequencial> CaracteristicaTipificacaoSequencial { get; set; }
        public virtual DbSet<CategoriaProdutos> CategoriaProdutos { get; set; }
        public virtual DbSet<Categorias> Categorias { get; set; }
        public virtual DbSet<CausaEspecifica> CausaEspecifica { get; set; }
        public virtual DbSet<CausaGenerica> CausaGenerica { get; set; }
        public virtual DbSet<Classificacao> Classificacao { get; set; }
        public virtual DbSet<ClassificacaoProduto> ClassificacaoProduto { get; set; }
        public virtual DbSet<ClusterDepartamentos> ClusterDepartamentos { get; set; }
        public virtual DbSet<Clusters> Clusters { get; set; }
        public virtual DbSet<CollectionHtml> CollectionHtml { get; set; }
        public virtual DbSet<CollectionJson> CollectionJson { get; set; }
        public virtual DbSet<CollectionLevel02> CollectionLevel02 { get; set; }
        public virtual DbSet<CollectionLevel03> CollectionLevel03 { get; set; }
        public virtual DbSet<CollectionLevel2> CollectionLevel2 { get; set; }
        public virtual DbSet<CollectionLevel2XCluster> CollectionLevel2XCluster { get; set; }
        public virtual DbSet<CollectionLevel2XCollectionJson> CollectionLevel2XCollectionJson { get; set; }
        public virtual DbSet<CollectionLevel2XParHeaderField> CollectionLevel2XParHeaderField { get; set; }
        public virtual DbSet<ConfiguracaoEmail> ConfiguracaoEmail { get; set; }
        public virtual DbSet<ConfiguracaoEmailPA> ConfiguracaoEmailPA { get; set; }
        public virtual DbSet<ConsolidationLevel01> ConsolidationLevel01 { get; set; }
        public virtual DbSet<ConsolidationLevel02> ConsolidationLevel02 { get; set; }
        public virtual DbSet<ConsolidationLevel1> ConsolidationLevel1 { get; set; }
        public virtual DbSet<ConsolidationLevel1XCluster> ConsolidationLevel1XCluster { get; set; }
        public virtual DbSet<ConsolidationLevel2> ConsolidationLevel2 { get; set; }
        public virtual DbSet<ConsolidationLevel2XCluster> ConsolidationLevel2XCluster { get; set; }
        public virtual DbSet<ContramedidaEspecifica> ContramedidaEspecifica { get; set; }
        public virtual DbSet<ContramedidaGenerica> ContramedidaGenerica { get; set; }
        public virtual DbSet<CorrectiveAction> CorrectiveAction { get; set; }
        public virtual DbSet<Counter> Counter { get; set; }
        public virtual DbSet<Defect> Defect { get; set; }
        public virtual DbSet<DepartamentoOperacoes> DepartamentoOperacoes { get; set; }
        public virtual DbSet<DepartamentoProdutos> DepartamentoProdutos { get; set; }
        public virtual DbSet<Departamentos> Departamentos { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<DesvioNiveis> DesvioNiveis { get; set; }
        public virtual DbSet<Desvios> Desvios { get; set; }
        public virtual DbSet<Deviation> Deviation { get; set; }
        public virtual DbSet<Email_ConfiguracaoEmailSgq> Email_ConfiguracaoEmailSgq { get; set; }
        public virtual DbSet<EmailContent> EmailContent { get; set; }
        public virtual DbSet<Empresa> Empresa { get; set; }
        public virtual DbSet<Empresas> Empresas { get; set; }
        public virtual DbSet<Equipamentos> Equipamentos { get; set; }
        public virtual DbSet<EquipamentosAvaliados> EquipamentosAvaliados { get; set; }
        public virtual DbSet<Estados> Estados { get; set; }
        public virtual DbSet<Example> Example { get; set; }
        public virtual DbSet<fa_CausaEspecifica> fa_CausaEspecifica { get; set; }
        public virtual DbSet<fa_CausaGenerica> fa_CausaGenerica { get; set; }
        public virtual DbSet<fa_ContramedidaEspecifica> fa_ContramedidaEspecifica { get; set; }
        public virtual DbSet<fa_ContramedidaGenerica> fa_ContramedidaGenerica { get; set; }
        public virtual DbSet<fa_GrupoCausa> fa_GrupoCausa { get; set; }
        public virtual DbSet<fa_PlanoDeAcaoQuemQuandoComo> fa_PlanoDeAcaoQuemQuandoComo { get; set; }
        public virtual DbSet<FamiliaProdutos> FamiliaProdutos { get; set; }
        public virtual DbSet<FormularioTratamentoAnomalia> FormularioTratamentoAnomalia { get; set; }
        public virtual DbSet<GroupMenu> GroupMenu { get; set; }
        public virtual DbSet<GrupoCabecalho> GrupoCabecalho { get; set; }
        public virtual DbSet<GrupoCausa> GrupoCausa { get; set; }
        public virtual DbSet<GrupoProjeto> GrupoProjeto { get; set; }
        public virtual DbSet<GrupoTipoAvaliacaoMonitoramentos> GrupoTipoAvaliacaoMonitoramentos { get; set; }
        public virtual DbSet<GrupoTipoAvaliacoes> GrupoTipoAvaliacoes { get; set; }
        public virtual DbSet<Hash> Hash { get; set; }
        public virtual DbSet<Horarios> Horarios { get; set; }
        public virtual DbSet<ItemMenu> ItemMenu { get; set; }
        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<JobParameter> JobParameter { get; set; }
        public virtual DbSet<JobQueue> JobQueue { get; set; }
        public virtual DbSet<LeftControlRole> LeftControlRole { get; set; }
        public virtual DbSet<Level01> Level01 { get; set; }
        public virtual DbSet<Level02> Level02 { get; set; }
        public virtual DbSet<Level03> Level03 { get; set; }
        public virtual DbSet<List> List { get; set; }
        public virtual DbSet<LogAlteracoes> LogAlteracoes { get; set; }
        public virtual DbSet<LogJson> LogJson { get; set; }
        public virtual DbSet<LogOperacaoPA> LogOperacaoPA { get; set; }
        public virtual DbSet<LogSgq> LogSgq { get; set; }
        public virtual DbSet<LogSgqGlobal> LogSgqGlobal { get; set; }
        public virtual DbSet<manDataCollectIT> manDataCollectIT { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<MenuXRoles> MenuXRoles { get; set; }
        public virtual DbSet<Metas> Metas { get; set; }
        public virtual DbSet<MigrationHistory> MigrationHistory { get; set; }
        public virtual DbSet<MonitoramentoEquipamentos> MonitoramentoEquipamentos { get; set; }
        public virtual DbSet<Monitoramentos> Monitoramentos { get; set; }
        public virtual DbSet<MonitoramentosConcorrentes> MonitoramentosConcorrentes { get; set; }
        public virtual DbSet<MultiplaEscolha> MultiplaEscolha { get; set; }
        public virtual DbSet<Niveis> Niveis { get; set; }
        public virtual DbSet<NiveisUsuarios> NiveisUsuarios { get; set; }
        public virtual DbSet<NQA> NQA { get; set; }
        public virtual DbSet<Observacoes> Observacoes { get; set; }
        public virtual DbSet<ObservacoesPadroes> ObservacoesPadroes { get; set; }
        public virtual DbSet<Operacoes> Operacoes { get; set; }
        public virtual DbSet<Pa_Acao> Pa_Acao { get; set; }
        public virtual DbSet<Pa_Acompanhamento> Pa_Acompanhamento { get; set; }
        public virtual DbSet<pa_acompanhamento_tarefa> pa_acompanhamento_tarefa { get; set; }
        public virtual DbSet<pa_campo> pa_campo { get; set; }
        public virtual DbSet<Pa_CausaEspecifica> Pa_CausaEspecifica { get; set; }
        public virtual DbSet<Pa_CausaGenerica> Pa_CausaGenerica { get; set; }
        public virtual DbSet<Pa_Colvis> Pa_Colvis { get; set; }
        public virtual DbSet<pa_configuracao_email> pa_configuracao_email { get; set; }
        public virtual DbSet<Pa_ContramedidaEspecifica> Pa_ContramedidaEspecifica { get; set; }
        public virtual DbSet<Pa_ContramedidaGenerica> Pa_ContramedidaGenerica { get; set; }
        public virtual DbSet<Pa_Coordenacao> Pa_Coordenacao { get; set; }
        public virtual DbSet<Pa_Departamento> Pa_Departamento { get; set; }
        public virtual DbSet<Pa_Dimensao> Pa_Dimensao { get; set; }
        public virtual DbSet<Pa_Diretoria> Pa_Diretoria { get; set; }
        public virtual DbSet<pa_empresa> pa_empresa { get; set; }
        public virtual DbSet<Pa_Gerencia> Pa_Gerencia { get; set; }
        public virtual DbSet<pa_grupo_projeto> pa_grupo_projeto { get; set; }
        public virtual DbSet<Pa_GrupoCausa> Pa_GrupoCausa { get; set; }
        public virtual DbSet<Pa_Indicadores> Pa_Indicadores { get; set; }
        public virtual DbSet<Pa_IndicadoresDeProjeto> Pa_IndicadoresDeProjeto { get; set; }
        public virtual DbSet<Pa_IndicadoresDiretriz> Pa_IndicadoresDiretriz { get; set; }
        public virtual DbSet<Pa_IndicadorSgqAcao> Pa_IndicadorSgqAcao { get; set; }
        public virtual DbSet<Pa_Iniciativa> Pa_Iniciativa { get; set; }
        public virtual DbSet<pa_log_operacao> pa_log_operacao { get; set; }
        public virtual DbSet<Pa_Missao> Pa_Missao { get; set; }
        public virtual DbSet<pa_multipla_escolha> pa_multipla_escolha { get; set; }
        public virtual DbSet<Pa_Objetivo> Pa_Objetivo { get; set; }
        public virtual DbSet<Pa_ObjetivoGeral> Pa_ObjetivoGeral { get; set; }
        public virtual DbSet<pa_participante> pa_participante { get; set; }
        public virtual DbSet<Pa_Planejamento> Pa_Planejamento { get; set; }
        public virtual DbSet<Pa_Problema_Desvio> Pa_Problema_Desvio { get; set; }
        public virtual DbSet<pa_projeto> pa_projeto { get; set; }
        public virtual DbSet<Pa_Quem> Pa_Quem { get; set; }
        public virtual DbSet<Pa_Query> Pa_Query { get; set; }
        public virtual DbSet<Pa_Status> Pa_Status { get; set; }
        public virtual DbSet<pa_tarefa> pa_tarefa { get; set; }
        public virtual DbSet<Pa_TemaAssunto> Pa_TemaAssunto { get; set; }
        public virtual DbSet<Pa_TemaProjeto> Pa_TemaProjeto { get; set; }
        public virtual DbSet<Pa_TipoProjeto> Pa_TipoProjeto { get; set; }
        public virtual DbSet<Pa_Unidade> Pa_Unidade { get; set; }
        public virtual DbSet<Pa_UnidadeMedida> Pa_UnidadeMedida { get; set; }
        public virtual DbSet<pa_vinculo_campo_tarefa> pa_vinculo_campo_tarefa { get; set; }
        public virtual DbSet<pa_vinculo_participante_multipla_escolha> pa_vinculo_participante_multipla_escolha { get; set; }
        public virtual DbSet<pa_vinculo_participante_projeto> pa_vinculo_participante_projeto { get; set; }
        public virtual DbSet<Pa_Visao> Pa_Visao { get; set; }
        public virtual DbSet<Pacotes> Pacotes { get; set; }
        public virtual DbSet<PacotesOperacoes> PacotesOperacoes { get; set; }
        public virtual DbSet<PadraoMonitoramentos> PadraoMonitoramentos { get; set; }
        public virtual DbSet<PadraoTolerancias> PadraoTolerancias { get; set; }
        public virtual DbSet<Padroes> Padroes { get; set; }
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
        public virtual DbSet<ParLocal> ParLocal { get; set; }
        public virtual DbSet<ParMeasurementUnit> ParMeasurementUnit { get; set; }
        public virtual DbSet<ParModule> ParModule { get; set; }
        public virtual DbSet<ParModuleXModule> ParModuleXModule { get; set; }
        public virtual DbSet<ParMultipleValues> ParMultipleValues { get; set; }
        public virtual DbSet<ParMultipleValuesXParCompany> ParMultipleValuesXParCompany { get; set; }
        public virtual DbSet<ParNotConformityRule> ParNotConformityRule { get; set; }
        public virtual DbSet<ParNotConformityRuleXLevel> ParNotConformityRuleXLevel { get; set; }
        public virtual DbSet<ParRecravacao_Linhas> ParRecravacao_Linhas { get; set; }
        public virtual DbSet<ParRecravacao_TipoLata> ParRecravacao_TipoLata { get; set; }
        public virtual DbSet<ParRelapse> ParRelapse { get; set; }
        public virtual DbSet<ParSample> ParSample { get; set; }
        public virtual DbSet<ParScoreType> ParScoreType { get; set; }
        public virtual DbSet<ParStructure> ParStructure { get; set; }
        public virtual DbSet<ParStructureGroup> ParStructureGroup { get; set; }
        public virtual DbSet<Pcc1b> Pcc1b { get; set; }
        public virtual DbSet<PenalidadeReincidencia> PenalidadeReincidencia { get; set; }
        public virtual DbSet<Perfil> Perfil { get; set; }
        public virtual DbSet<Period> Period { get; set; }
        public virtual DbSet<PlanoDeAcaoQuemQuandoComo> PlanoDeAcaoQuemQuandoComo { get; set; }
        public virtual DbSet<Pontos> Pontos { get; set; }
        public virtual DbSet<Produto> Produto { get; set; }
        public virtual DbSet<ProdutoInNatura> ProdutoInNatura { get; set; }
        public virtual DbSet<Produtos> Produtos { get; set; }
        public virtual DbSet<ProdutosAvaliados> ProdutosAvaliados { get; set; }
        public virtual DbSet<Projeto> Projeto { get; set; }
        public virtual DbSet<RecravacaoJson> RecravacaoJson { get; set; }
        public virtual DbSet<Regionais> Regionais { get; set; }
        public virtual DbSet<Result_Level3> Result_Level3 { get; set; }
        public virtual DbSet<Result_Level3_Photos> Result_Level3_Photos { get; set; }
        public virtual DbSet<Resultados> Resultados { get; set; }
        public virtual DbSet<ResultadosData> ResultadosData { get; set; }
        public virtual DbSet<ResultadosPCC> ResultadosPCC { get; set; }
        public virtual DbSet<RoleSGQ> RoleSGQ { get; set; }
        public virtual DbSet<RoleUserSgq> RoleUserSgq { get; set; }
        public virtual DbSet<RoleUserSgqXItemMenu> RoleUserSgqXItemMenu { get; set; }
        public virtual DbSet<Schema> Schema { get; set; }
        public virtual DbSet<Server> Server { get; set; }
        public virtual DbSet<Set> Set { get; set; }
        public virtual DbSet<Shift> Shift { get; set; }
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<Sugestoes> Sugestoes { get; set; }
        public virtual DbSet<TarefaAmostras> TarefaAmostras { get; set; }
        public virtual DbSet<TarefaAvaliacoes> TarefaAvaliacoes { get; set; }
        public virtual DbSet<TarefaCategorias> TarefaCategorias { get; set; }
        public virtual DbSet<TarefaMonitoramentos> TarefaMonitoramentos { get; set; }
        public virtual DbSet<TarefaPA> TarefaPA { get; set; }
        public virtual DbSet<Tarefas> Tarefas { get; set; }
        public virtual DbSet<Terceiro> Terceiro { get; set; }
        public virtual DbSet<TipificacaoReal> TipificacaoReal { get; set; }
        public virtual DbSet<TipoAvaliacoes> TipoAvaliacoes { get; set; }
        public virtual DbSet<Unidades> Unidades { get; set; }
        public virtual DbSet<UnidadesMedidas> UnidadesMedidas { get; set; }
        public virtual DbSet<Unit> Unit { get; set; }
        public virtual DbSet<UnitUser> UnitUser { get; set; }
        public virtual DbSet<UserSgq> UserSgq { get; set; }
        public virtual DbSet<UserXRoles> UserXRoles { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioPerfilEmpresa> UsuarioPerfilEmpresa { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
        public virtual DbSet<UsuarioUnidades> UsuarioUnidades { get; set; }
        public virtual DbSet<VacuoGRD> VacuoGRD { get; set; }
        public virtual DbSet<VerificacaoContagem> VerificacaoContagem { get; set; }
        public virtual DbSet<VerificacaoTipificacao> VerificacaoTipificacao { get; set; }
        public virtual DbSet<VerificacaoTipificacaoComparacao> VerificacaoTipificacaoComparacao { get; set; }
        public virtual DbSet<VerificacaoTipificacaoResultados> VerificacaoTipificacaoResultados { get; set; }
        public virtual DbSet<VerificacaoTipificacaoTarefaIntegracao> VerificacaoTipificacaoTarefaIntegracao { get; set; }
        public virtual DbSet<VerificacaoTipificacaoV2> VerificacaoTipificacaoV2 { get; set; }
        public virtual DbSet<VerificacaoTipificacaoValidacao> VerificacaoTipificacaoValidacao { get; set; }
        public virtual DbSet<VinculoCampoCabecalho> VinculoCampoCabecalho { get; set; }
        public virtual DbSet<VinculoCampoTarefa> VinculoCampoTarefa { get; set; }
        public virtual DbSet<VinculoParticipanteMultiplaEscolha> VinculoParticipanteMultiplaEscolha { get; set; }
        public virtual DbSet<VinculoParticipanteProjeto> VinculoParticipanteProjeto { get; set; }
        public virtual DbSet<VolumeAbate> VolumeAbate { get; set; }
        public virtual DbSet<VolumeCepDesossa> VolumeCepDesossa { get; set; }
        public virtual DbSet<VolumeCepRecortes> VolumeCepRecortes { get; set; }
        public virtual DbSet<VolumePcc1b> VolumePcc1b { get; set; }
        public virtual DbSet<VolumeProducao> VolumeProducao { get; set; }
        public virtual DbSet<VolumeVacuoGRD> VolumeVacuoGRD { get; set; }
        public virtual DbSet<VTVerificacaoContagem> VTVerificacaoContagem { get; set; }
        public virtual DbSet<VTVerificacaoTipificacao> VTVerificacaoTipificacao { get; set; }
        public virtual DbSet<VTVerificacaoTipificacaoComparacao> VTVerificacaoTipificacaoComparacao { get; set; }
        public virtual DbSet<VTVerificacaoTipificacaoResultados> VTVerificacaoTipificacaoResultados { get; set; }
        public virtual DbSet<VTVerificacaoTipificacaoTarefaIntegracao> VTVerificacaoTipificacaoTarefaIntegracao { get; set; }
        public virtual DbSet<VTVerificacaoTipificacaoValidacao> VTVerificacaoTipificacaoValidacao { get; set; }
        public virtual DbSet<Z_Sistema> Z_Sistema { get; set; }
        public virtual DbSet<AggregatedCounter1> AggregatedCounter1 { get; set; }
        public virtual DbSet<Counter1> Counter1 { get; set; }
        public virtual DbSet<Hash1> Hash1 { get; set; }
        public virtual DbSet<Job1> Job1 { get; set; }
        public virtual DbSet<JobParameter1> JobParameter1 { get; set; }
        public virtual DbSet<JobQueue1> JobQueue1 { get; set; }
        public virtual DbSet<List1> List1 { get; set; }
        public virtual DbSet<Schema1> Schema1 { get; set; }
        public virtual DbSet<Server1> Server1 { get; set; }
        public virtual DbSet<Set1> Set1 { get; set; }
        public virtual DbSet<State1> State1 { get; set; }
        public virtual DbSet<CACHORRO> CACHORRO { get; set; }
        public virtual DbSet<ControleMetaTolerancia> ControleMetaTolerancia { get; set; }
        public virtual DbSet<Pa_AcaoXQuem> Pa_AcaoXQuem { get; set; }
        public virtual DbSet<Pa_CausaMedidaXAcao> Pa_CausaMedidaXAcao { get; set; }
        public virtual DbSet<ParCalendar> ParCalendar { get; set; }
        public virtual DbSet<ParGroupParLevel1> ParGroupParLevel1 { get; set; }
        public virtual DbSet<ParGroupParLevel1Type> ParGroupParLevel1Type { get; set; }
        public virtual DbSet<ParGroupParLevel1XParLevel1> ParGroupParLevel1XParLevel1 { get; set; }
        public virtual DbSet<ParLevel1VariableProduction> ParLevel1VariableProduction { get; set; }
        public virtual DbSet<ParLevel1VariableProductionXLevel1> ParLevel1VariableProductionXLevel1 { get; set; }
        public virtual DbSet<ParReports> ParReports { get; set; }
        public virtual DbSet<ResultLevel2HeaderField> ResultLevel2HeaderField { get; set; }
        public virtual DbSet<RetornoParaTablet> RetornoParaTablet { get; set; }
        public virtual DbSet<RoleJBS> RoleJBS { get; set; }
        public virtual DbSet<RoleType> RoleType { get; set; }
        public virtual DbSet<ScorecardConsolidadoDia> ScorecardConsolidadoDia { get; set; }
        public virtual DbSet<ScreenComponent> ScreenComponent { get; set; }
        public virtual DbSet<SgqConfig> SgqConfig { get; set; }
        public virtual DbSet<SgqConfig2> SgqConfig2 { get; set; }
        public virtual DbSet<Tipificacao> Tipificacao { get; set; }
        public virtual DbSet<VerificacaoTipificacaoJBS> VerificacaoTipificacaoJBS { get; set; }
        public virtual DbSet<VTVerificacaoTipificacaoJBS> VTVerificacaoTipificacaoJBS { get; set; }
        public virtual DbSet<DelDados> DelDados { get; set; }
        public virtual DbSet<Reports_CCA_Audit> Reports_CCA_Audit { get; set; }
        public virtual DbSet<Reports_CFF_Audit> Reports_CFF_Audit { get; set; }
        public virtual DbSet<Reports_HTP_Audit> Reports_HTP_Audit { get; set; }
        public virtual DbSet<ScorecardJBS_V> ScorecardJBS_V { get; set; }
        public virtual DbSet<VWCFFResults> VWCFFResults { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AreasParticipantes>()
                .Property(e => e.nCdCaracteristica)
                .HasPrecision(12, 0);

            modelBuilder.Entity<AreasParticipantes>()
                .Property(e => e.cNmCaracteristica)
                .IsUnicode(false);

            modelBuilder.Entity<AreasParticipantes>()
                .Property(e => e.cNrCaracteristica)
                .IsUnicode(false);

            modelBuilder.Entity<AreasParticipantes>()
                .Property(e => e.cSgCaracteristica)
                .IsUnicode(false);

            modelBuilder.Entity<AreasParticipantes>()
                .Property(e => e.cIdentificador)
                .IsUnicode(false);

            modelBuilder.Entity<Cabecalho>()
                .HasMany(e => e.TarefaPA)
                .WithOptional(e => e.Cabecalho)
                .HasForeignKey(e => e.IdCabecalho);

            modelBuilder.Entity<Cabecalho>()
                .HasMany(e => e.VinculoCampoCabecalho)
                .WithRequired(e => e.Cabecalho)
                .HasForeignKey(e => e.IdCabecalho)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Campo>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Campo>()
                .Property(e => e.Tipo)
                .IsUnicode(false);

            modelBuilder.Entity<Campo>()
                .HasMany(e => e.MultiplaEscolha)
                .WithRequired(e => e.Campo)
                .HasForeignKey(e => e.IdCampo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Campo>()
                .HasMany(e => e.VinculoCampoCabecalho)
                .WithRequired(e => e.Campo)
                .HasForeignKey(e => e.IdCampo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Campo>()
                .HasMany(e => e.VinculoCampoTarefa)
                .WithRequired(e => e.Campo)
                .HasForeignKey(e => e.IdCampo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CaracteristicaTipificacao>()
                .Property(e => e.nCdCaracteristica)
                .HasPrecision(10, 0);

            modelBuilder.Entity<CaracteristicaTipificacao>()
                .Property(e => e.cNmCaracteristica)
                .IsUnicode(false);

            modelBuilder.Entity<CaracteristicaTipificacao>()
                .Property(e => e.cNrCaracteristica)
                .IsUnicode(false);

            modelBuilder.Entity<CaracteristicaTipificacao>()
                .Property(e => e.cSgCaracteristica)
                .IsUnicode(false);

            modelBuilder.Entity<CaracteristicaTipificacao>()
                .Property(e => e.cIdentificador)
                .IsUnicode(false);

            modelBuilder.Entity<CaracteristicaTipificacao>()
                .HasMany(e => e.CaracteristicaTipificacaoSequencial)
                .WithOptional(e => e.CaracteristicaTipificacao)
                .HasForeignKey(e => e.nCdCaracteristica_Id);

            modelBuilder.Entity<CaracteristicaTipificacaoSequencial>()
                .Property(e => e.nCdCaracteristica_Id)
                .HasPrecision(10, 0);

            modelBuilder.Entity<Categorias>()
                .HasMany(e => e.CategoriaProdutos)
                .WithRequired(e => e.Categorias)
                .HasForeignKey(e => e.Categoria)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Categorias>()
                .HasMany(e => e.TarefaCategorias)
                .WithRequired(e => e.Categorias)
                .HasForeignKey(e => e.Categoria)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CausaEspecifica>()
                .Property(e => e.Text)
                .IsFixedLength();

            modelBuilder.Entity<CausaEspecifica>()
                .HasMany(e => e.fa_CausaEspecifica)
                .WithRequired(e => e.CausaEspecifica)
                .HasForeignKey(e => e.IdCausaEspecifica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CausaGenerica>()
                .HasMany(e => e.fa_CausaGenerica)
                .WithRequired(e => e.CausaGenerica)
                .HasForeignKey(e => e.IdCausaGenerica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Classificacao>()
                .Property(e => e.nCdClassificacao)
                .HasPrecision(10, 0);

            modelBuilder.Entity<Classificacao>()
                .Property(e => e.cNmClassificacao)
                .IsUnicode(false);

            modelBuilder.Entity<Classificacao>()
                .Property(e => e.cNrClassificacao)
                .IsUnicode(false);

            modelBuilder.Entity<ClassificacaoProduto>()
                .Property(e => e.nCdProduto)
                .HasPrecision(10, 0);

            modelBuilder.Entity<ClassificacaoProduto>()
                .Property(e => e.nCdClassificacao)
                .HasPrecision(10, 0);

            modelBuilder.Entity<Clusters>()
                .HasMany(e => e.Pontos)
                .WithRequired(e => e.Clusters)
                .HasForeignKey(e => e.Cluster)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CollectionJson>()
                .Property(e => e.Level02HeaderJson)
                .IsUnicode(false);

            modelBuilder.Entity<CollectionJson>()
                .Property(e => e.Level03ResultJSon)
                .IsUnicode(false);

            modelBuilder.Entity<CollectionJson>()
                .Property(e => e.CorrectiveActionJson)
                .IsUnicode(false);

            modelBuilder.Entity<CollectionJson>()
                .HasMany(e => e.CollectionLevel2XCollectionJson)
                .WithRequired(e => e.CollectionJson)
                .HasForeignKey(e => e.CollectionJson_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CollectionLevel02>()
                .Property(e => e.Chainspeed)
                .HasPrecision(20, 5);

            modelBuilder.Entity<CollectionLevel02>()
                .Property(e => e.LotNumber)
                .HasPrecision(20, 5);

            modelBuilder.Entity<CollectionLevel02>()
                .Property(e => e.Mudscore)
                .HasPrecision(20, 5);

            modelBuilder.Entity<CollectionLevel02>()
                .HasMany(e => e.CollectionLevel03)
                .WithRequired(e => e.CollectionLevel02)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CollectionLevel03>()
                .Property(e => e.Value)
                .HasPrecision(10, 5);

            modelBuilder.Entity<CollectionLevel03>()
                .Property(e => e.ValueText)
                .IsUnicode(false);

            modelBuilder.Entity<CollectionLevel2>()
                .Property(e => e.WeiEvaluation)
                .HasPrecision(38, 10);

            modelBuilder.Entity<CollectionLevel2>()
                .Property(e => e.Defects)
                .HasPrecision(38, 10);

            modelBuilder.Entity<CollectionLevel2>()
                .Property(e => e.WeiDefects)
                .HasPrecision(15, 5);

            modelBuilder.Entity<CollectionLevel2>()
                .HasMany(e => e.CollectionLevel2XCollectionJson)
                .WithRequired(e => e.CollectionLevel2)
                .HasForeignKey(e => e.CollectionLevel2_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CollectionLevel2>()
                .HasOptional(e => e.CollectionLevel21)
                .WithRequired(e => e.CollectionLevel22);

            modelBuilder.Entity<CollectionLevel2>()
                .HasMany(e => e.CollectionLevel2XParHeaderField)
                .WithRequired(e => e.CollectionLevel2)
                .HasForeignKey(e => e.CollectionLevel2_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CollectionLevel2>()
                .HasMany(e => e.CorrectiveAction)
                .WithRequired(e => e.CollectionLevel2)
                .HasForeignKey(e => e.CollectionLevel02Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CollectionLevel2>()
                .HasMany(e => e.Result_Level3)
                .WithRequired(e => e.CollectionLevel2)
                .HasForeignKey(e => e.CollectionLevel2_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ConsolidationLevel01>()
                .HasMany(e => e.ConsolidationLevel02)
                .WithRequired(e => e.ConsolidationLevel01)
                .HasForeignKey(e => e.Level01ConsolidationId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ConsolidationLevel02>()
                .HasMany(e => e.CollectionLevel02)
                .WithRequired(e => e.ConsolidationLevel02)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ConsolidationLevel1>()
                .Property(e => e.Evaluation)
                .HasPrecision(32, 8);

            modelBuilder.Entity<ConsolidationLevel1>()
                .Property(e => e.WeiEvaluation)
                .HasPrecision(32, 8);

            modelBuilder.Entity<ConsolidationLevel1>()
                .Property(e => e.EvaluateTotal)
                .HasPrecision(32, 8);

            modelBuilder.Entity<ConsolidationLevel1>()
                .Property(e => e.DefectsTotal)
                .HasPrecision(10, 5);

            modelBuilder.Entity<ConsolidationLevel1>()
                .Property(e => e.WeiDefects)
                .HasPrecision(30, 8);

            modelBuilder.Entity<ConsolidationLevel1>()
                .HasMany(e => e.ConsolidationLevel2)
                .WithRequired(e => e.ConsolidationLevel1)
                .HasForeignKey(e => e.ConsolidationLevel1_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ConsolidationLevel2>()
                .Property(e => e.WeiEvaluation)
                .HasPrecision(10, 5);

            modelBuilder.Entity<ConsolidationLevel2>()
                .Property(e => e.EvaluateTotal)
                .HasPrecision(10, 5);

            modelBuilder.Entity<ConsolidationLevel2>()
                .Property(e => e.DefectsTotal)
                .HasPrecision(10, 5);

            modelBuilder.Entity<ConsolidationLevel2>()
                .Property(e => e.WeiDefects)
                .HasPrecision(30, 8);

            modelBuilder.Entity<ConsolidationLevel2>()
                .HasMany(e => e.CollectionLevel2)
                .WithRequired(e => e.ConsolidationLevel2)
                .HasForeignKey(e => e.ConsolidationLevel2_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ContramedidaEspecifica>()
                .Property(e => e.Text)
                .IsFixedLength();

            modelBuilder.Entity<ContramedidaEspecifica>()
                .HasMany(e => e.fa_ContramedidaEspecifica)
                .WithRequired(e => e.ContramedidaEspecifica)
                .HasForeignKey(e => e.IdContramedidaEspecifica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ContramedidaGenerica>()
                .HasMany(e => e.fa_ContramedidaGenerica)
                .WithRequired(e => e.ContramedidaGenerica)
                .HasForeignKey(e => e.IdContramedidaGenerica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Defect>()
                .Property(e => e.Defects)
                .HasPrecision(30, 8);

            modelBuilder.Entity<Departamentos>()
                .HasMany(e => e.DepartamentoOperacoes)
                .WithRequired(e => e.Departamentos)
                .HasForeignKey(e => e.Departamento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Departamentos>()
                .HasMany(e => e.DepartamentoProdutos)
                .WithRequired(e => e.Departamentos)
                .HasForeignKey(e => e.Departamento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Departamentos>()
                .HasMany(e => e.ObservacoesPadroes)
                .WithRequired(e => e.Departamentos)
                .HasForeignKey(e => e.Departamento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Departamentos>()
                .HasMany(e => e.TarefaAvaliacoes)
                .WithRequired(e => e.Departamentos)
                .HasForeignKey(e => e.Departamento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Departamentos>()
                .HasMany(e => e.Tarefas)
                .WithOptional(e => e.Departamentos)
                .HasForeignKey(e => e.Departamento);

            modelBuilder.Entity<Departamentos>()
                .HasMany(e => e.VolumeProducao)
                .WithRequired(e => e.Departamentos)
                .HasForeignKey(e => e.Departamento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Department>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.ConsolidationLevel01)
                .WithRequired(e => e.Department)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.ConsolidationLevel1)
                .WithRequired(e => e.Department)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Desvios>()
                .Property(e => e.Meta)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Desvios>()
                .Property(e => e.Real)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Deviation>()
                .Property(e => e.Evaluation)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Deviation>()
                .Property(e => e.Sample)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Deviation>()
                .Property(e => e.Defects)
                .HasPrecision(10, 5);

            modelBuilder.Entity<EmailContent>()
                .HasMany(e => e.CorrectiveAction)
                .WithOptional(e => e.EmailContent)
                .HasForeignKey(e => e.EmailContent_Id);

            modelBuilder.Entity<EmailContent>()
                .HasMany(e => e.Deviation)
                .WithOptional(e => e.EmailContent)
                .HasForeignKey(e => e.EmailContent_Id);

            modelBuilder.Entity<Empresa>()
                .Property(e => e.nCdEmpresa)
                .HasPrecision(10, 0);

            modelBuilder.Entity<Empresa>()
                .Property(e => e.cNmEmpresa)
                .IsUnicode(false);

            modelBuilder.Entity<Empresa>()
                .Property(e => e.cSgEmpresa)
                .IsUnicode(false);

            modelBuilder.Entity<Empresa>()
                .Property(e => e.cCdOrgaoRegulador)
                .IsUnicode(false);

            modelBuilder.Entity<fa_CausaEspecifica>()
                .Property(e => e.Versao)
                .IsFixedLength();

            modelBuilder.Entity<fa_CausaGenerica>()
                .Property(e => e.Versao)
                .IsFixedLength();

            modelBuilder.Entity<fa_ContramedidaEspecifica>()
                .Property(e => e.Versao)
                .IsFixedLength();

            modelBuilder.Entity<fa_ContramedidaGenerica>()
                .Property(e => e.Versao)
                .IsFixedLength();

            modelBuilder.Entity<fa_GrupoCausa>()
                .Property(e => e.Versao)
                .IsFixedLength();

            modelBuilder.Entity<fa_PlanoDeAcaoQuemQuandoComo>()
                .Property(e => e.Versao)
                .IsFixedLength();

            modelBuilder.Entity<FormularioTratamentoAnomalia>()
                .Property(e => e.Versao)
                .IsFixedLength();

            modelBuilder.Entity<GroupMenu>()
                .HasMany(e => e.Menu)
                .WithRequired(e => e.GroupMenu)
                .HasForeignKey(e => e.GroupMenu_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GrupoCabecalho>()
                .HasMany(e => e.Campo)
                .WithOptional(e => e.GrupoCabecalho)
                .HasForeignKey(e => e.IdGrupoCabecalho);

            modelBuilder.Entity<GrupoCabecalho>()
                .HasMany(e => e.VinculoCampoCabecalho)
                .WithOptional(e => e.GrupoCabecalho)
                .HasForeignKey(e => e.IdGrupoCabecalho);

            modelBuilder.Entity<GrupoCausa>()
                .HasMany(e => e.fa_GrupoCausa)
                .WithRequired(e => e.GrupoCausa)
                .HasForeignKey(e => e.IdGrupoCausa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GrupoProjeto>()
                .HasMany(e => e.Projeto)
                .WithOptional(e => e.GrupoProjeto)
                .HasForeignKey(e => e.IdGrupoProjeto);

            modelBuilder.Entity<GrupoTipoAvaliacoes>()
                .HasMany(e => e.GrupoTipoAvaliacaoMonitoramentos)
                .WithRequired(e => e.GrupoTipoAvaliacoes)
                .HasForeignKey(e => e.GrupoTipoAvaliacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ItemMenu>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ItemMenu>()
                .Property(e => e.Icon)
                .IsUnicode(false);

            modelBuilder.Entity<ItemMenu>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<ItemMenu>()
                .Property(e => e.Resource)
                .IsUnicode(false);

            modelBuilder.Entity<Level01>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Level01>()
                .Property(e => e.Alias)
                .IsUnicode(false);

            modelBuilder.Entity<Level01>()
                .HasMany(e => e.CollectionLevel02)
                .WithRequired(e => e.Level01)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Level01>()
                .HasMany(e => e.ConsolidationLevel01)
                .WithRequired(e => e.Level01)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Level02>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Level02>()
                .Property(e => e.Alias)
                .IsUnicode(false);

            modelBuilder.Entity<Level02>()
                .HasMany(e => e.CollectionLevel02)
                .WithRequired(e => e.Level02)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Level02>()
                .HasMany(e => e.ConsolidationLevel02)
                .WithRequired(e => e.Level02)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Level03>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Level03>()
                .Property(e => e.Alias)
                .IsUnicode(false);

            modelBuilder.Entity<LogJson>()
                .Property(e => e.result)
                .IsUnicode(false);

            modelBuilder.Entity<LogJson>()
                .Property(e => e.log)
                .IsUnicode(false);

            modelBuilder.Entity<LogOperacaoPA>()
                .Property(e => e.MensagemOperacao)
                .IsUnicode(false);

            modelBuilder.Entity<LogOperacaoPA>()
                .Property(e => e.NomeUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<LogOperacaoPA>()
                .Property(e => e.TextoExcecao)
                .IsUnicode(false);

            modelBuilder.Entity<LogOperacaoPA>()
                .Property(e => e.DescricaoLanIp)
                .IsUnicode(false);

            modelBuilder.Entity<LogOperacaoPA>()
                .Property(e => e.DescricaoInternetIp)
                .IsUnicode(false);

            modelBuilder.Entity<LogOperacaoPA>()
                .Property(e => e.NomeMetodo)
                .IsUnicode(false);

            modelBuilder.Entity<LogOperacaoPA>()
                .Property(e => e.UrlTela)
                .IsUnicode(false);

            modelBuilder.Entity<LogSgq>()
                .Property(e => e.Level)
                .IsUnicode(false);

            modelBuilder.Entity<LogSgq>()
                .Property(e => e.Call_Site)
                .IsUnicode(false);

            modelBuilder.Entity<LogSgq>()
                .Property(e => e.Exception_Type)
                .IsUnicode(false);

            modelBuilder.Entity<LogSgq>()
                .Property(e => e.Exception_Message)
                .IsUnicode(false);

            modelBuilder.Entity<LogSgq>()
                .Property(e => e.Stack_Trace)
                .IsUnicode(false);

            modelBuilder.Entity<LogSgq>()
                .Property(e => e.Additional_Info)
                .IsUnicode(false);

            modelBuilder.Entity<LogSgq>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<LogSgq>()
                .Property(e => e.Second_Log_Path)
                .IsUnicode(false);

            modelBuilder.Entity<LogSgqGlobal>()
                .Property(e => e.Level)
                .IsUnicode(false);

            modelBuilder.Entity<LogSgqGlobal>()
                .Property(e => e.Call_Site)
                .IsUnicode(false);

            modelBuilder.Entity<LogSgqGlobal>()
                .Property(e => e.Exception_Type)
                .IsUnicode(false);

            modelBuilder.Entity<LogSgqGlobal>()
                .Property(e => e.Exception_Message)
                .IsUnicode(false);

            modelBuilder.Entity<LogSgqGlobal>()
                .Property(e => e.Stack_Trace)
                .IsUnicode(false);

            modelBuilder.Entity<LogSgqGlobal>()
                .Property(e => e.Additional_Info)
                .IsUnicode(false);

            modelBuilder.Entity<LogSgqGlobal>()
                .Property(e => e.Object)
                .IsUnicode(false);

            modelBuilder.Entity<LogSgqGlobal>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<LogSgqGlobal>()
                .Property(e => e.Second_Log_Path)
                .IsUnicode(false);

            modelBuilder.Entity<manDataCollectIT>()
                .Property(e => e.amountData)
                .HasPrecision(32, 10);

            modelBuilder.Entity<manDataCollectIT>()
                .Property(e => e.comments)
                .IsUnicode(false);

            modelBuilder.Entity<Menu>()
                .HasMany(e => e.MenuXRoles)
                .WithRequired(e => e.Menu)
                .HasForeignKey(e => e.Menu_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Metas>()
                .Property(e => e.Meta)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Monitoramentos>()
                .HasMany(e => e.GrupoTipoAvaliacaoMonitoramentos)
                .WithRequired(e => e.Monitoramentos)
                .HasForeignKey(e => e.Monitoramento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Monitoramentos>()
                .HasMany(e => e.MonitoramentoEquipamentos)
                .WithRequired(e => e.Monitoramentos)
                .HasForeignKey(e => e.Monitoramento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Monitoramentos>()
                .HasMany(e => e.MonitoramentosConcorrentes)
                .WithRequired(e => e.Monitoramentos)
                .HasForeignKey(e => e.ConcorrenteId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Monitoramentos>()
                .HasMany(e => e.MonitoramentosConcorrentes1)
                .WithRequired(e => e.Monitoramentos1)
                .HasForeignKey(e => e.MonitoramentoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Monitoramentos>()
                .HasMany(e => e.PadraoMonitoramentos)
                .WithRequired(e => e.Monitoramentos)
                .HasForeignKey(e => e.Monitoramento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Monitoramentos>()
                .HasMany(e => e.PadraoTolerancias)
                .WithRequired(e => e.Monitoramentos)
                .HasForeignKey(e => e.Monitoramento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Monitoramentos>()
                .HasMany(e => e.TarefaMonitoramentos)
                .WithRequired(e => e.Monitoramentos)
                .HasForeignKey(e => e.Monitoramento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Monitoramentos>()
                .HasMany(e => e.VerificacaoTipificacaoTarefaIntegracao)
                .WithRequired(e => e.Monitoramentos)
                .HasForeignKey(e => e.TarefaId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MultiplaEscolha>()
                .HasMany(e => e.VinculoCampoCabecalho)
                .WithOptional(e => e.MultiplaEscolha)
                .HasForeignKey(e => e.IdMultiplaEscolha);

            modelBuilder.Entity<MultiplaEscolha>()
                .HasMany(e => e.VinculoCampoTarefa)
                .WithOptional(e => e.MultiplaEscolha)
                .HasForeignKey(e => e.IdMultiplaEscolha);

            modelBuilder.Entity<MultiplaEscolha>()
                .HasMany(e => e.VinculoParticipanteMultiplaEscolha)
                .WithRequired(e => e.MultiplaEscolha)
                .HasForeignKey(e => e.IdMultiplaEscolha)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Niveis>()
                .HasMany(e => e.NiveisUsuarios)
                .WithRequired(e => e.Niveis)
                .HasForeignKey(e => e.Nivel)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Operacoes>()
                .HasMany(e => e.AcoesCorretivas)
                .WithRequired(e => e.Operacoes)
                .HasForeignKey(e => e.Operacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Operacoes>()
                .HasMany(e => e.Alertas)
                .WithOptional(e => e.Operacoes)
                .HasForeignKey(e => e.Operacao);

            modelBuilder.Entity<Operacoes>()
                .HasMany(e => e.DepartamentoOperacoes)
                .WithRequired(e => e.Operacoes)
                .HasForeignKey(e => e.Operacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Operacoes>()
                .HasMany(e => e.FamiliaProdutos)
                .WithOptional(e => e.Operacoes)
                .HasForeignKey(e => e.Operacao);

            modelBuilder.Entity<Operacoes>()
                .HasMany(e => e.Horarios)
                .WithRequired(e => e.Operacoes)
                .HasForeignKey(e => e.OperacaoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Operacoes>()
                .HasMany(e => e.Metas)
                .WithRequired(e => e.Operacoes)
                .HasForeignKey(e => e.Operacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Operacoes>()
                .HasMany(e => e.MonitoramentosConcorrentes)
                .WithRequired(e => e.Operacoes)
                .HasForeignKey(e => e.OperacaoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Operacoes>()
                .HasMany(e => e.ObservacoesPadroes)
                .WithRequired(e => e.Operacoes)
                .HasForeignKey(e => e.Operacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Operacoes>()
                .HasMany(e => e.PacotesOperacoes)
                .WithOptional(e => e.Operacoes)
                .HasForeignKey(e => e.Operacao);

            modelBuilder.Entity<Operacoes>()
                .HasMany(e => e.Pontos)
                .WithRequired(e => e.Operacoes)
                .HasForeignKey(e => e.Operacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Operacoes>()
                .HasMany(e => e.TarefaAvaliacoes)
                .WithRequired(e => e.Operacoes)
                .HasForeignKey(e => e.Operacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Operacoes>()
                .HasMany(e => e.Tarefas)
                .WithRequired(e => e.Operacoes)
                .HasForeignKey(e => e.Operacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Operacoes>()
                .HasMany(e => e.TipificacaoReal)
                .WithRequired(e => e.Operacoes)
                .HasForeignKey(e => e.Operacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Operacoes>()
                .HasMany(e => e.VolumeProducao)
                .WithRequired(e => e.Operacoes)
                .HasForeignKey(e => e.Operacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Pa_Acao>()
                .Property(e => e.QuantoCusta)
                .HasPrecision(35, 10);

            modelBuilder.Entity<Pa_Acao>()
                .Property(e => e.Observacao)
                .IsUnicode(false);

            modelBuilder.Entity<Pa_Acao>()
                .Property(e => e.Level1Name)
                .IsUnicode(false);

            modelBuilder.Entity<Pa_Acao>()
                .Property(e => e.Level2Name)
                .IsUnicode(false);

            modelBuilder.Entity<Pa_Acao>()
                .Property(e => e.Level3Name)
                .IsUnicode(false);

            modelBuilder.Entity<Pa_Acao>()
                .Property(e => e.Regional)
                .IsUnicode(false);

            modelBuilder.Entity<Pa_Acao>()
                .Property(e => e.UnidadeName)
                .IsUnicode(false);

            modelBuilder.Entity<pa_campo>()
                .Property(e => e.nome)
                .IsUnicode(false);

            modelBuilder.Entity<pa_campo>()
                .Property(e => e.tipo)
                .IsUnicode(false);

            modelBuilder.Entity<pa_campo>()
                .HasMany(e => e.pa_multipla_escolha)
                .WithRequired(e => e.pa_campo)
                .HasForeignKey(e => e.id_campo);

            modelBuilder.Entity<pa_campo>()
                .HasMany(e => e.pa_vinculo_campo_tarefa)
                .WithRequired(e => e.pa_campo)
                .HasForeignKey(e => e.id_campo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Pa_CausaEspecifica>()
                .Property(e => e.Text)
                .IsFixedLength();

            modelBuilder.Entity<Pa_Colvis>()
                .Property(e => e.ColVisShow)
                .IsUnicode(false);

            modelBuilder.Entity<Pa_Colvis>()
                .Property(e => e.ColVisHide)
                .IsUnicode(false);

            modelBuilder.Entity<Pa_ContramedidaEspecifica>()
                .Property(e => e.Text)
                .IsFixedLength();

            modelBuilder.Entity<pa_empresa>()
                .HasMany(e => e.pa_grupo_projeto)
                .WithOptional(e => e.pa_empresa)
                .HasForeignKey(e => e.id_empresa);

            modelBuilder.Entity<pa_empresa>()
                .HasMany(e => e.pa_participante)
                .WithRequired(e => e.pa_empresa)
                .HasForeignKey(e => e.id_empresa);

            modelBuilder.Entity<pa_empresa>()
                .HasMany(e => e.pa_projeto)
                .WithRequired(e => e.pa_empresa)
                .HasForeignKey(e => e.id_empresa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<pa_grupo_projeto>()
                .HasMany(e => e.pa_projeto)
                .WithOptional(e => e.pa_grupo_projeto)
                .HasForeignKey(e => e.id_grupo_projeto);

            modelBuilder.Entity<pa_log_operacao>()
                .Property(e => e.mensagem_operacao)
                .IsUnicode(false);

            modelBuilder.Entity<pa_log_operacao>()
                .Property(e => e.nm_usuario)
                .IsUnicode(false);

            modelBuilder.Entity<pa_log_operacao>()
                .Property(e => e.tx_excecao)
                .IsUnicode(false);

            modelBuilder.Entity<pa_log_operacao>()
                .Property(e => e.dc_lan_ip)
                .IsUnicode(false);

            modelBuilder.Entity<pa_log_operacao>()
                .Property(e => e.dc_internet_ip)
                .IsUnicode(false);

            modelBuilder.Entity<pa_multipla_escolha>()
                .HasMany(e => e.pa_vinculo_campo_tarefa)
                .WithOptional(e => e.pa_multipla_escolha)
                .HasForeignKey(e => e.id_multipla_escolha);

            modelBuilder.Entity<pa_multipla_escolha>()
                .HasMany(e => e.pa_vinculo_participante_multipla_escolha)
                .WithRequired(e => e.pa_multipla_escolha)
                .HasForeignKey(e => e.id_multipla_escolha);

            modelBuilder.Entity<pa_participante>()
                .Property(e => e.nome)
                .IsUnicode(false);

            modelBuilder.Entity<pa_participante>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<pa_participante>()
                .Property(e => e.telefone)
                .IsUnicode(false);

            modelBuilder.Entity<pa_participante>()
                .HasMany(e => e.pa_tarefa)
                .WithOptional(e => e.pa_participante)
                .HasForeignKey(e => e.id_participante_criador);

            modelBuilder.Entity<pa_participante>()
                .HasMany(e => e.pa_vinculo_campo_tarefa)
                .WithOptional(e => e.pa_participante)
                .HasForeignKey(e => e.id_participante);

            modelBuilder.Entity<pa_participante>()
                .HasMany(e => e.pa_vinculo_participante_multipla_escolha)
                .WithRequired(e => e.pa_participante)
                .HasForeignKey(e => e.id_participante);

            modelBuilder.Entity<pa_participante>()
                .HasMany(e => e.pa_vinculo_participante_projeto)
                .WithRequired(e => e.pa_participante)
                .HasForeignKey(e => e.id_participante);

            modelBuilder.Entity<Pa_Planejamento>()
                .Property(e => e.ValorDe)
                .HasPrecision(38, 10);

            modelBuilder.Entity<Pa_Planejamento>()
                .Property(e => e.ValorPara)
                .HasPrecision(38, 10);

            modelBuilder.Entity<pa_projeto>()
                .HasMany(e => e.pa_campo)
                .WithRequired(e => e.pa_projeto)
                .HasForeignKey(e => e.id_projeto);

            modelBuilder.Entity<pa_projeto>()
                .HasMany(e => e.pa_tarefa)
                .WithRequired(e => e.pa_projeto)
                .HasForeignKey(e => e.id_projeto);

            modelBuilder.Entity<pa_projeto>()
                .HasMany(e => e.pa_vinculo_participante_projeto)
                .WithRequired(e => e.pa_projeto)
                .HasForeignKey(e => e.id_projeto);

            modelBuilder.Entity<pa_tarefa>()
                .HasMany(e => e.pa_acompanhamento_tarefa)
                .WithRequired(e => e.pa_tarefa)
                .HasForeignKey(e => e.id_tarefa);

            modelBuilder.Entity<pa_tarefa>()
                .HasMany(e => e.pa_vinculo_campo_tarefa)
                .WithRequired(e => e.pa_tarefa)
                .HasForeignKey(e => e.id_tarefa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<pa_vinculo_campo_tarefa>()
                .Property(e => e.valor)
                .IsUnicode(false);

            modelBuilder.Entity<Pacotes>()
                .HasMany(e => e.PacotesOperacoes)
                .WithOptional(e => e.Pacotes)
                .HasForeignKey(e => e.Pacote);

            modelBuilder.Entity<Padroes>()
                .HasMany(e => e.PadraoMonitoramentos)
                .WithRequired(e => e.Padroes)
                .HasForeignKey(e => e.Padrao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Padroes>()
                .HasMany(e => e.PadraoTolerancias)
                .WithOptional(e => e.Padroes)
                .HasForeignKey(e => e.PadraoNivel1);

            modelBuilder.Entity<Padroes>()
                .HasMany(e => e.PadraoTolerancias1)
                .WithOptional(e => e.Padroes1)
                .HasForeignKey(e => e.PadraoNivel3);

            modelBuilder.Entity<ParCluster>()
                .HasMany(e => e.ParClusterXModule)
                .WithRequired(e => e.ParCluster)
                .HasForeignKey(e => e.ParCluster_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParCluster>()
                .HasMany(e => e.ParCompanyCluster)
                .WithRequired(e => e.ParCluster)
                .HasForeignKey(e => e.ParCluster_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParCluster>()
                .HasMany(e => e.ParLevel1XCluster)
                .WithRequired(e => e.ParCluster)
                .HasForeignKey(e => e.ParCluster_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParClusterGroup>()
                .HasMany(e => e.ParCluster)
                .WithRequired(e => e.ParClusterGroup)
                .HasForeignKey(e => e.ParClusterGroup_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParClusterXModule>()
                .Property(e => e.Points)
                .HasPrecision(10, 5);

            modelBuilder.Entity<ParCompany>()
                .Property(e => e.IntegrationId)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ParCompany>()
                .HasMany(e => e.ConsolidationLevel1)
                .WithRequired(e => e.ParCompany)
                .HasForeignKey(e => e.UnitId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParCompany>()
                .HasMany(e => e.Defect)
                .WithRequired(e => e.ParCompany)
                .HasForeignKey(e => e.ParCompany_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParCompany>()
                .HasMany(e => e.ParLevel3EvaluationSample)
                .WithOptional(e => e.ParCompany)
                .HasForeignKey(e => e.ParCompany_Id);

            modelBuilder.Entity<ParCompany>()
                .HasMany(e => e.ParMultipleValuesXParCompany)
                .WithRequired(e => e.ParCompany)
                .HasForeignKey(e => e.ParCompany_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParCompany>()
                .HasMany(e => e.VolumeCepDesossa)
                .WithOptional(e => e.ParCompany)
                .HasForeignKey(e => e.ParCompany_id);

            modelBuilder.Entity<ParCompany>()
                .HasMany(e => e.VolumeCepRecortes)
                .WithOptional(e => e.ParCompany)
                .HasForeignKey(e => e.ParCompany_id);

            modelBuilder.Entity<ParCompany>()
                .HasMany(e => e.VolumePcc1b)
                .WithOptional(e => e.ParCompany)
                .HasForeignKey(e => e.ParCompany_id);

            modelBuilder.Entity<ParCompany>()
                .HasMany(e => e.VolumeVacuoGRD)
                .WithOptional(e => e.ParCompany)
                .HasForeignKey(e => e.ParCompany_id);

            modelBuilder.Entity<ParCompany>()
                .HasMany(e => e.ParCompanyCluster)
                .WithRequired(e => e.ParCompany)
                .HasForeignKey(e => e.ParCompany_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParCompany>()
                .HasMany(e => e.ParCompanyXStructure)
                .WithRequired(e => e.ParCompany)
                .HasForeignKey(e => e.ParCompany_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParCompany>()
                .HasMany(e => e.ParCompanyXUserSgq)
                .WithRequired(e => e.ParCompany)
                .HasForeignKey(e => e.ParCompany_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParCompany>()
                .HasMany(e => e.ParEvaluation)
                .WithOptional(e => e.ParCompany)
                .HasForeignKey(e => e.ParCompany_Id);

            modelBuilder.Entity<ParCompany>()
                .HasMany(e => e.ParGoal)
                .WithOptional(e => e.ParCompany)
                .HasForeignKey(e => e.ParCompany_Id);

            modelBuilder.Entity<ParCompany>()
                .HasMany(e => e.ParLevel2ControlCompany)
                .WithOptional(e => e.ParCompany)
                .HasForeignKey(e => e.ParCompany_Id);

            modelBuilder.Entity<ParCompany>()
                .HasMany(e => e.ParLevel3Level2)
                .WithOptional(e => e.ParCompany)
                .HasForeignKey(e => e.ParCompany_Id);

            modelBuilder.Entity<ParCompany>()
                .HasMany(e => e.ParLevel3Value)
                .WithOptional(e => e.ParCompany)
                .HasForeignKey(e => e.ParCompany_Id);

            modelBuilder.Entity<ParCompany>()
                .HasMany(e => e.ParNotConformityRuleXLevel)
                .WithOptional(e => e.ParCompany)
                .HasForeignKey(e => e.ParCompany_Id);

            modelBuilder.Entity<ParCompany>()
                .HasMany(e => e.ParSample)
                .WithOptional(e => e.ParCompany)
                .HasForeignKey(e => e.ParCompany_Id);

            modelBuilder.Entity<ParConsolidationType>()
                .HasMany(e => e.ParLevel1)
                .WithRequired(e => e.ParConsolidationType)
                .HasForeignKey(e => e.ParConsolidationType_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParCounter>()
                .HasMany(e => e.ParCounterXLocal)
                .WithRequired(e => e.ParCounter)
                .HasForeignKey(e => e.ParCounter_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParCriticalLevel>()
                .HasMany(e => e.ParLevel1XCluster)
                .WithOptional(e => e.ParCriticalLevel)
                .HasForeignKey(e => e.ParCriticalLevel_Id);

            modelBuilder.Entity<ParDepartment>()
                .HasMany(e => e.ParLevel2)
                .WithRequired(e => e.ParDepartment)
                .HasForeignKey(e => e.ParDepartment_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParFieldType>()
                .HasMany(e => e.CollectionLevel2XParHeaderField)
                .WithRequired(e => e.ParFieldType)
                .HasForeignKey(e => e.ParFieldType_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParFieldType>()
                .HasMany(e => e.ParHeaderField)
                .WithRequired(e => e.ParFieldType)
                .HasForeignKey(e => e.ParFieldType_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParFrequency>()
                .HasMany(e => e.ParLevel1)
                .WithRequired(e => e.ParFrequency)
                .HasForeignKey(e => e.ParFrequency_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParFrequency>()
                .HasMany(e => e.ParLevel2)
                .WithRequired(e => e.ParFrequency)
                .HasForeignKey(e => e.ParFrequency_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParFrequency>()
                .HasMany(e => e.ParRelapse)
                .WithRequired(e => e.ParFrequency)
                .HasForeignKey(e => e.ParFrequency_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParGoal>()
                .Property(e => e.PercentValue)
                .HasPrecision(10, 5);

            modelBuilder.Entity<ParGoalScorecard>()
                .Property(e => e.PercentValueMid)
                .HasPrecision(25, 7);

            modelBuilder.Entity<ParGoalScorecard>()
                .Property(e => e.PercentValueHigh)
                .HasPrecision(25, 7);

            modelBuilder.Entity<ParHeaderField>()
                .HasMany(e => e.CollectionLevel2XParHeaderField)
                .WithRequired(e => e.ParHeaderField)
                .HasForeignKey(e => e.ParHeaderField_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParHeaderField>()
                .HasMany(e => e.ParMultipleValuesXParCompany)
                .WithOptional(e => e.ParHeaderField)
                .HasForeignKey(e => e.ParHeaderField_Id);

            modelBuilder.Entity<ParHeaderField>()
                .HasMany(e => e.ParLevel1XHeaderField)
                .WithRequired(e => e.ParHeaderField)
                .HasForeignKey(e => e.ParHeaderField_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParHeaderField>()
                .HasMany(e => e.ParMultipleValues)
                .WithRequired(e => e.ParHeaderField)
                .HasForeignKey(e => e.ParHeaderField_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLataImagens>()
                .Property(e => e.PathFile)
                .IsUnicode(false);

            modelBuilder.Entity<ParLataImagens>()
                .Property(e => e.FileName)
                .IsUnicode(false);

            modelBuilder.Entity<ParLevel1>()
                .HasMany(e => e.CollectionLevel2)
                .WithRequired(e => e.ParLevel1)
                .HasForeignKey(e => e.ParLevel1_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLevel1>()
                .HasMany(e => e.ConsolidationLevel1)
                .WithRequired(e => e.ParLevel1)
                .HasForeignKey(e => e.ParLevel1_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLevel1>()
                .HasMany(e => e.Defect)
                .WithRequired(e => e.ParLevel1)
                .HasForeignKey(e => e.ParLevel1_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLevel1>()
                .HasMany(e => e.ParCounterXLocal)
                .WithOptional(e => e.ParLevel1)
                .HasForeignKey(e => e.ParLevel1_Id);

            modelBuilder.Entity<ParLevel1>()
                .HasMany(e => e.ParGoal)
                .WithRequired(e => e.ParLevel1)
                .HasForeignKey(e => e.ParLevel1_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLevel1>()
                .HasMany(e => e.ParLevel3EvaluationSample)
                .WithOptional(e => e.ParLevel1)
                .HasForeignKey(e => e.ParLevel1_Id);

            modelBuilder.Entity<ParLevel1>()
                .HasMany(e => e.ParLevel3Value)
                .WithOptional(e => e.ParLevel1)
                .HasForeignKey(e => e.ParLevel1_Id);

            modelBuilder.Entity<ParLevel1>()
                .HasMany(e => e.ParMultipleValuesXParCompany)
                .WithRequired(e => e.ParLevel1)
                .HasForeignKey(e => e.ParLevel1_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLevel1>()
                .HasMany(e => e.VolumeCepDesossa)
                .WithOptional(e => e.ParLevel1)
                .HasForeignKey(e => e.ParLevel1_id);

            modelBuilder.Entity<ParLevel1>()
                .HasMany(e => e.VolumeCepRecortes)
                .WithOptional(e => e.ParLevel1)
                .HasForeignKey(e => e.ParLevel1_id);

            modelBuilder.Entity<ParLevel1>()
                .HasMany(e => e.VolumePcc1b)
                .WithOptional(e => e.ParLevel1)
                .HasForeignKey(e => e.ParLevel1_id);

            modelBuilder.Entity<ParLevel1>()
                .HasMany(e => e.VolumeVacuoGRD)
                .WithOptional(e => e.ParLevel1)
                .HasForeignKey(e => e.ParLevel1_id);

            modelBuilder.Entity<ParLevel1>()
                .HasMany(e => e.ParLevel2Level1)
                .WithRequired(e => e.ParLevel1)
                .HasForeignKey(e => e.ParLevel1_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLevel1>()
                .HasMany(e => e.ParLevel1XCluster)
                .WithRequired(e => e.ParLevel1)
                .HasForeignKey(e => e.ParLevel1_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLevel1>()
                .HasMany(e => e.ParLevel1XHeaderField)
                .WithRequired(e => e.ParLevel1)
                .HasForeignKey(e => e.ParLevel1_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLevel1>()
                .HasMany(e => e.ParLevel2ControlCompany)
                .WithOptional(e => e.ParLevel1)
                .HasForeignKey(e => e.ParLevel1_Id);

            modelBuilder.Entity<ParLevel1>()
                .HasMany(e => e.ParLevel3Level2Level1)
                .WithRequired(e => e.ParLevel1)
                .HasForeignKey(e => e.ParLevel1_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLevel1>()
                .HasMany(e => e.ParNotConformityRuleXLevel)
                .WithOptional(e => e.ParLevel1)
                .HasForeignKey(e => e.ParLevel1_Id);

            modelBuilder.Entity<ParLevel1>()
                .HasMany(e => e.ParRelapse)
                .WithOptional(e => e.ParLevel1)
                .HasForeignKey(e => e.ParLevel1_Id);

            modelBuilder.Entity<ParLevel1XCluster>()
                .Property(e => e.Points)
                .HasPrecision(10, 5);

            modelBuilder.Entity<ParLevel1XHeaderField>()
                .Property(e => e.HeaderFieldGroup)
                .IsUnicode(false);

            modelBuilder.Entity<ParLevel2>()
                .HasMany(e => e.CollectionLevel2)
                .WithRequired(e => e.ParLevel2)
                .HasForeignKey(e => e.ParLevel2_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLevel2>()
                .HasMany(e => e.ConsolidationLevel2)
                .WithRequired(e => e.ParLevel2)
                .HasForeignKey(e => e.ParLevel2_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLevel2>()
                .HasMany(e => e.ParCounterXLocal)
                .WithOptional(e => e.ParLevel2)
                .HasForeignKey(e => e.ParLevel2_Id);

            modelBuilder.Entity<ParLevel2>()
                .HasMany(e => e.ParEvaluation)
                .WithRequired(e => e.ParLevel2)
                .HasForeignKey(e => e.ParLevel2_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLevel2>()
                .HasMany(e => e.ParLevel3EvaluationSample)
                .WithOptional(e => e.ParLevel2)
                .HasForeignKey(e => e.ParLevel2_Id);

            modelBuilder.Entity<ParLevel2>()
                .HasMany(e => e.ParLevel3Value)
                .WithOptional(e => e.ParLevel2)
                .HasForeignKey(e => e.ParLevel2_Id);

            modelBuilder.Entity<ParLevel2>()
                .HasMany(e => e.ParLevel2Level1)
                .WithRequired(e => e.ParLevel2)
                .HasForeignKey(e => e.ParLevel2_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLevel2>()
                .HasMany(e => e.ParLevel2ControlCompany)
                .WithOptional(e => e.ParLevel2)
                .HasForeignKey(e => e.ParLevel2_Id);

            modelBuilder.Entity<ParLevel2>()
                .HasMany(e => e.ParLevel3Group)
                .WithRequired(e => e.ParLevel2)
                .HasForeignKey(e => e.ParLevel2_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLevel2>()
                .HasMany(e => e.ParLevel3Level2)
                .WithRequired(e => e.ParLevel2)
                .HasForeignKey(e => e.ParLevel2_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLevel2>()
                .HasMany(e => e.ParNotConformityRuleXLevel)
                .WithOptional(e => e.ParLevel2)
                .HasForeignKey(e => e.ParLevel2_Id);

            modelBuilder.Entity<ParLevel2>()
                .HasMany(e => e.ParRelapse)
                .WithOptional(e => e.ParLevel2)
                .HasForeignKey(e => e.ParLevel2_Id);

            modelBuilder.Entity<ParLevel2>()
                .HasMany(e => e.ParSample)
                .WithRequired(e => e.ParLevel2)
                .HasForeignKey(e => e.ParLevel2_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLevel3>()
                .HasMany(e => e.ParCounterXLocal)
                .WithOptional(e => e.ParLevel3)
                .HasForeignKey(e => e.ParLevel3_Id);

            modelBuilder.Entity<ParLevel3>()
                .HasMany(e => e.ParLevel3EvaluationSample)
                .WithRequired(e => e.ParLevel3)
                .HasForeignKey(e => e.ParLevel3_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLevel3>()
                .HasMany(e => e.ParLevel3Level2)
                .WithRequired(e => e.ParLevel3)
                .HasForeignKey(e => e.ParLevel3_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLevel3>()
                .HasMany(e => e.ParLevel3Value)
                .WithRequired(e => e.ParLevel3)
                .HasForeignKey(e => e.ParLevel3_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLevel3>()
                .HasMany(e => e.ParNotConformityRuleXLevel)
                .WithOptional(e => e.ParLevel3)
                .HasForeignKey(e => e.ParLevel3_Id);

            modelBuilder.Entity<ParLevel3>()
                .HasMany(e => e.ParRelapse)
                .WithOptional(e => e.ParLevel3)
                .HasForeignKey(e => e.ParLevel3_Id);

            modelBuilder.Entity<ParLevel3>()
                .HasMany(e => e.Result_Level3)
                .WithRequired(e => e.ParLevel3)
                .HasForeignKey(e => e.ParLevel3_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLevel3BoolFalse>()
                .HasMany(e => e.ParLevel3Value)
                .WithOptional(e => e.ParLevel3BoolFalse)
                .HasForeignKey(e => e.ParLevel3BoolFalse_Id);

            modelBuilder.Entity<ParLevel3BoolTrue>()
                .HasMany(e => e.ParLevel3Value)
                .WithOptional(e => e.ParLevel3BoolTrue)
                .HasForeignKey(e => e.ParLevel3BoolTrue_Id);

            modelBuilder.Entity<ParLevel3EvaluationSample>()
                .Property(e => e.SampleNumber)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ParLevel3EvaluationSample>()
                .Property(e => e.EvaluationNumber)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ParLevel3EvaluationSample>()
                .Property(e => e.EvaluationInterval)
                .IsUnicode(false);

            modelBuilder.Entity<ParLevel3Group>()
                .HasMany(e => e.ParLevel3Level2)
                .WithOptional(e => e.ParLevel3Group)
                .HasForeignKey(e => e.ParLevel3Group_Id);

            modelBuilder.Entity<ParLevel3InputType>()
                .Property(e => e.Sampling)
                .HasPrecision(10, 5);

            modelBuilder.Entity<ParLevel3InputType>()
                .HasMany(e => e.ParLevel3Value)
                .WithRequired(e => e.ParLevel3InputType)
                .HasForeignKey(e => e.ParLevel3InputType_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLevel3Level2>()
                .Property(e => e.Weight)
                .HasPrecision(10, 5);

            modelBuilder.Entity<ParLevel3Level2>()
                .HasMany(e => e.ParLevel3Level2Level1)
                .WithRequired(e => e.ParLevel3Level2)
                .HasForeignKey(e => e.ParLevel3Level2_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLevel3Value>()
                .Property(e => e.IntervalMin)
                .HasPrecision(38, 10);

            modelBuilder.Entity<ParLevel3Value>()
                .Property(e => e.IntervalMax)
                .HasPrecision(38, 10);

            modelBuilder.Entity<ParLevel3Value>()
                .Property(e => e.DynamicValue)
                .IsUnicode(false);

            modelBuilder.Entity<ParLevel3Value_Outer>()
                .Property(e => e.LimInferior)
                .HasPrecision(18, 5);

            modelBuilder.Entity<ParLevel3Value_Outer>()
                .Property(e => e.LimSuperior)
                .HasPrecision(18, 5);

            modelBuilder.Entity<ParLevelDefiniton>()
                .HasMany(e => e.ParHeaderField)
                .WithRequired(e => e.ParLevelDefiniton)
                .HasForeignKey(e => e.ParLevelDefinition_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParLocal>()
                .HasMany(e => e.ParCounterXLocal)
                .WithRequired(e => e.ParLocal)
                .HasForeignKey(e => e.ParLocal_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParMeasurementUnit>()
                .HasMany(e => e.ParLevel3Value)
                .WithOptional(e => e.ParMeasurementUnit)
                .HasForeignKey(e => e.ParMeasurementUnit_Id);

            modelBuilder.Entity<ParModule>()
                .HasMany(e => e.ParClusterXModule)
                .WithRequired(e => e.ParModule)
                .HasForeignKey(e => e.ParModule_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParModule>()
                .HasMany(e => e.ParModuleXModule)
                .WithRequired(e => e.ParModule)
                .HasForeignKey(e => e.ParModuleChild_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParModule>()
                .HasMany(e => e.ParModuleXModule1)
                .WithRequired(e => e.ParModule1)
                .HasForeignKey(e => e.ParModuleParent_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParMultipleValues>()
                .Property(e => e.PunishmentValue)
                .HasPrecision(38, 10);

            modelBuilder.Entity<ParMultipleValues>()
                .HasMany(e => e.ParMultipleValuesXParCompany)
                .WithOptional(e => e.ParMultipleValues)
                .HasForeignKey(e => e.Parent_ParMultipleValues_Id);

            modelBuilder.Entity<ParMultipleValues>()
                .HasMany(e => e.ParMultipleValuesXParCompany1)
                .WithOptional(e => e.ParMultipleValues1)
                .HasForeignKey(e => e.ParMultipleValues_Id);

            modelBuilder.Entity<ParMultipleValuesXParCompany>()
                .Property(e => e.HashKey)
                .IsUnicode(false);

            modelBuilder.Entity<ParNotConformityRule>()
                .HasMany(e => e.ParNotConformityRuleXLevel)
                .WithRequired(e => e.ParNotConformityRule)
                .HasForeignKey(e => e.ParNotConformityRule_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParNotConformityRuleXLevel>()
                .Property(e => e.Value)
                .HasPrecision(38, 10);

            modelBuilder.Entity<ParScoreType>()
                .HasMany(e => e.ParLevel1)
                .WithOptional(e => e.ParScoreType)
                .HasForeignKey(e => e.ParScoreType_Id);

            modelBuilder.Entity<ParStructure>()
                .HasMany(e => e.ParCompanyXStructure)
                .WithRequired(e => e.ParStructure)
                .HasForeignKey(e => e.ParStructure_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParStructureGroup>()
                .HasMany(e => e.ParStructure)
                .WithRequired(e => e.ParStructureGroup)
                .HasForeignKey(e => e.ParStructureGroup_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Pcc1b>()
                .Property(e => e.Departamento)
                .IsUnicode(false);

            modelBuilder.Entity<Pcc1b>()
                .Property(e => e.Meta)
                .HasPrecision(2, 0);

            modelBuilder.Entity<Perfil>()
                .Property(e => e.nCdPerfil)
                .HasPrecision(10, 0);

            modelBuilder.Entity<Perfil>()
                .Property(e => e.cNmPerfil)
                .IsUnicode(false);

            modelBuilder.Entity<PlanoDeAcaoQuemQuandoComo>()
                .Property(e => e.Quem)
                .IsFixedLength();

            modelBuilder.Entity<PlanoDeAcaoQuemQuandoComo>()
                .HasMany(e => e.fa_PlanoDeAcaoQuemQuandoComo)
                .WithRequired(e => e.PlanoDeAcaoQuemQuandoComo)
                .HasForeignKey(e => e.IdPlanoDeAcaoQuemQuandoComo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Pontos>()
                .Property(e => e.Pontos1)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Produto>()
                .Property(e => e.nCdProduto)
                .HasPrecision(10, 0);

            modelBuilder.Entity<Produto>()
                .Property(e => e.cNmProduto)
                .IsUnicode(false);

            modelBuilder.Entity<Produto>()
                .Property(e => e.cDescricaoDetalhada)
                .IsUnicode(false);

            modelBuilder.Entity<ProdutoInNatura>()
                .Property(e => e.nCdProduto)
                .HasPrecision(10, 0);

            modelBuilder.Entity<ProdutoInNatura>()
                .Property(e => e.cNmProduto)
                .IsUnicode(false);

            modelBuilder.Entity<Produtos>()
                .HasMany(e => e.CategoriaProdutos)
                .WithRequired(e => e.Produtos)
                .HasForeignKey(e => e.Produto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Produtos>()
                .HasMany(e => e.DepartamentoProdutos)
                .WithRequired(e => e.Produtos)
                .HasForeignKey(e => e.Produto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Produtos>()
                .HasMany(e => e.FamiliaProdutos)
                .WithRequired(e => e.Produtos)
                .HasForeignKey(e => e.Produto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Produtos>()
                .HasMany(e => e.Tarefas)
                .WithOptional(e => e.Produtos)
                .HasForeignKey(e => e.Produto);

            modelBuilder.Entity<Projeto>()
                .HasMany(e => e.Cabecalho)
                .WithRequired(e => e.Projeto)
                .HasForeignKey(e => e.IdProjeto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Projeto>()
                .HasMany(e => e.Campo)
                .WithRequired(e => e.Projeto)
                .HasForeignKey(e => e.IdProjeto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Projeto>()
                .HasMany(e => e.GrupoCabecalho)
                .WithOptional(e => e.Projeto)
                .HasForeignKey(e => e.IdProjeto);

            modelBuilder.Entity<Projeto>()
                .HasMany(e => e.TarefaPA)
                .WithRequired(e => e.Projeto)
                .HasForeignKey(e => e.IdProjeto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Projeto>()
                .HasMany(e => e.VinculoParticipanteProjeto)
                .WithRequired(e => e.Projeto)
                .HasForeignKey(e => e.IdProjeto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Regionais>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Regionais>()
                .HasMany(e => e.Usuarios)
                .WithOptional(e => e.Regionais)
                .HasForeignKey(e => e.Regional);

            modelBuilder.Entity<Result_Level3>()
                .Property(e => e.Weight)
                .HasPrecision(10, 5);

            modelBuilder.Entity<Result_Level3>()
                .Property(e => e.Defects)
                .HasPrecision(10, 5);

            modelBuilder.Entity<Result_Level3>()
                .Property(e => e.PunishmentValue)
                .HasPrecision(10, 5);

            modelBuilder.Entity<Result_Level3>()
                .Property(e => e.WeiEvaluation)
                .HasPrecision(10, 5);

            modelBuilder.Entity<Result_Level3>()
                .Property(e => e.Evaluation)
                .HasPrecision(10, 5);

            modelBuilder.Entity<Result_Level3>()
                .Property(e => e.WeiDefects)
                .HasPrecision(30, 8);

            modelBuilder.Entity<Result_Level3>()
                .Property(e => e.CT4Eva3)
                .HasPrecision(10, 5);

            modelBuilder.Entity<Result_Level3>()
                .Property(e => e.Sampling)
                .HasPrecision(10, 5);

            modelBuilder.Entity<Result_Level3_Photos>()
                .Property(e => e.Photo_Thumbnaills)
                .IsUnicode(false);

            modelBuilder.Entity<Result_Level3_Photos>()
                .Property(e => e.Photo)
                .IsUnicode(false);

            modelBuilder.Entity<Resultados>()
                .Property(e => e.Meta)
                .HasPrecision(5, 2);

            modelBuilder.Entity<ResultadosData>()
                .Property(e => e.Chave)
                .IsUnicode(false);

            modelBuilder.Entity<ResultadosData>()
                .Property(e => e.Campo)
                .IsUnicode(false);

            modelBuilder.Entity<ResultadosPCC>()
                .Property(e => e.Meta)
                .HasPrecision(5, 2);

            modelBuilder.Entity<RoleSGQ>()
                .Property(e => e.Role)
                .IsFixedLength();

            modelBuilder.Entity<RoleUserSgq>()
                .HasMany(e => e.MenuXRoles)
                .WithRequired(e => e.RoleUserSgq)
                .HasForeignKey(e => e.Role_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RoleUserSgq>()
                .HasMany(e => e.UserXRoles)
                .WithRequired(e => e.RoleUserSgq)
                .HasForeignKey(e => e.Role_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RoleUserSgqXItemMenu>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<TarefaPA>()
                .HasMany(e => e.AcompanhamentoTarefa)
                .WithRequired(e => e.TarefaPA)
                .HasForeignKey(e => e.IdTarefa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TarefaPA>()
                .HasMany(e => e.VinculoCampoTarefa)
                .WithRequired(e => e.TarefaPA)
                .HasForeignKey(e => e.IdTarefa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tarefas>()
                .HasMany(e => e.AcoesCorretivas)
                .WithRequired(e => e.Tarefas)
                .HasForeignKey(e => e.Tarefa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tarefas>()
                .HasMany(e => e.Alertas)
                .WithOptional(e => e.Tarefas)
                .HasForeignKey(e => e.Tarefa);

            modelBuilder.Entity<Tarefas>()
                .HasMany(e => e.MonitoramentosConcorrentes)
                .WithRequired(e => e.Tarefas)
                .HasForeignKey(e => e.TarefaId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tarefas>()
                .HasMany(e => e.ObservacoesPadroes)
                .WithRequired(e => e.Tarefas)
                .HasForeignKey(e => e.Tarefa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tarefas>()
                .HasMany(e => e.TarefaAmostras)
                .WithRequired(e => e.Tarefas)
                .HasForeignKey(e => e.Tarefa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tarefas>()
                .HasMany(e => e.TarefaAvaliacoes)
                .WithRequired(e => e.Tarefas)
                .HasForeignKey(e => e.Tarefa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tarefas>()
                .HasMany(e => e.TarefaCategorias)
                .WithRequired(e => e.Tarefas)
                .HasForeignKey(e => e.Tarefa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tarefas>()
                .HasMany(e => e.TarefaMonitoramentos)
                .WithRequired(e => e.Tarefas)
                .HasForeignKey(e => e.Tarefa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Terceiro>()
                .Property(e => e.nCdTerceiro)
                .HasPrecision(10, 0);

            modelBuilder.Entity<Terceiro>()
                .Property(e => e.cNmTerceiro)
                .IsUnicode(false);

            modelBuilder.Entity<TipificacaoReal>()
                .Property(e => e.PercReal)
                .HasPrecision(5, 2);

            modelBuilder.Entity<TipoAvaliacoes>()
                .HasMany(e => e.GrupoTipoAvaliacoes)
                .WithRequired(e => e.TipoAvaliacoes)
                .HasForeignKey(e => e.Negativo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TipoAvaliacoes>()
                .HasMany(e => e.GrupoTipoAvaliacoes1)
                .WithRequired(e => e.TipoAvaliacoes1)
                .HasForeignKey(e => e.Positivo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Unidades>()
                .HasMany(e => e.Alertas)
                .WithOptional(e => e.Unidades)
                .HasForeignKey(e => e.Unidade);

            modelBuilder.Entity<Unidades>()
                .HasMany(e => e.Equipamentos)
                .WithRequired(e => e.Unidades)
                .HasForeignKey(e => e.Unidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Unidades>()
                .HasMany(e => e.FamiliaProdutos)
                .WithOptional(e => e.Unidades)
                .HasForeignKey(e => e.Unidade);

            modelBuilder.Entity<Unidades>()
                .HasMany(e => e.GrupoProjeto)
                .WithOptional(e => e.Unidades)
                .HasForeignKey(e => e.IdEmpresa);

            modelBuilder.Entity<Unidades>()
                .HasMany(e => e.Horarios)
                .WithOptional(e => e.Unidades)
                .HasForeignKey(e => e.UnidadeId);

            modelBuilder.Entity<Unidades>()
                .HasMany(e => e.MonitoramentosConcorrentes)
                .WithOptional(e => e.Unidades)
                .HasForeignKey(e => e.UnidadeId);

            modelBuilder.Entity<Unidades>()
                .HasMany(e => e.PadraoMonitoramentos)
                .WithOptional(e => e.Unidades)
                .HasForeignKey(e => e.Unidade);

            modelBuilder.Entity<Unidades>()
                .HasMany(e => e.TarefaAmostras)
                .WithOptional(e => e.Unidades)
                .HasForeignKey(e => e.Unidade);

            modelBuilder.Entity<Unidades>()
                .HasMany(e => e.TarefaAvaliacoes)
                .WithOptional(e => e.Unidades)
                .HasForeignKey(e => e.Unidade);

            modelBuilder.Entity<Unidades>()
                .HasMany(e => e.TarefaMonitoramentos)
                .WithOptional(e => e.Unidades)
                .HasForeignKey(e => e.Unidade);

            modelBuilder.Entity<Unidades>()
                .HasMany(e => e.TipificacaoReal)
                .WithRequired(e => e.Unidades)
                .HasForeignKey(e => e.Unidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Unidades>()
                .HasMany(e => e.Usuarios)
                .WithRequired(e => e.Unidades)
                .HasForeignKey(e => e.Unidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Unidades>()
                .HasMany(e => e.UsuarioUnidades)
                .WithRequired(e => e.Unidades)
                .HasForeignKey(e => e.Unidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Unidades>()
                .HasMany(e => e.VerificacaoTipificacao)
                .WithRequired(e => e.Unidades)
                .HasForeignKey(e => e.UnidadeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Unidades>()
                .HasMany(e => e.VolumeProducao)
                .WithRequired(e => e.Unidades)
                .HasForeignKey(e => e.Unidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UnidadesMedidas>()
                .HasMany(e => e.PadraoMonitoramentos)
                .WithOptional(e => e.UnidadesMedidas)
                .HasForeignKey(e => e.UnidadeMedidaLegenda);

            modelBuilder.Entity<UnidadesMedidas>()
                .HasMany(e => e.PadraoMonitoramentos1)
                .WithOptional(e => e.UnidadesMedidas1)
                .HasForeignKey(e => e.UnidadeMedida);

            modelBuilder.Entity<Unit>()
                .HasMany(e => e.ConsolidationLevel01)
                .WithRequired(e => e.Unit)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Unit>()
                .HasMany(e => e.UnitUser)
                .WithRequired(e => e.Unit)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserSgq>()
                .HasMany(e => e.CollectionLevel02)
                .WithRequired(e => e.UserSgq)
                .HasForeignKey(e => e.AuditorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserSgq>()
                .HasMany(e => e.CollectionLevel2)
                .WithRequired(e => e.UserSgq)
                .HasForeignKey(e => e.AuditorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserSgq>()
                .HasMany(e => e.CorrectiveAction)
                .WithRequired(e => e.UserSgq)
                .HasForeignKey(e => e.AuditorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserSgq>()
                .HasMany(e => e.CorrectiveAction1)
                .WithOptional(e => e.UserSgq1)
                .HasForeignKey(e => e.TechinicalId);

            modelBuilder.Entity<UserSgq>()
                .HasMany(e => e.CorrectiveAction2)
                .WithOptional(e => e.UserSgq2)
                .HasForeignKey(e => e.SlaughterId);

            modelBuilder.Entity<UserSgq>()
                .HasMany(e => e.ParCompanyXUserSgq)
                .WithRequired(e => e.UserSgq)
                .HasForeignKey(e => e.UserSgq_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserSgq>()
                .HasMany(e => e.UnitUser)
                .WithRequired(e => e.UserSgq)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserSgq>()
                .HasMany(e => e.UserXRoles)
                .WithRequired(e => e.UserSgq)
                .HasForeignKey(e => e.User_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.nCdUsuario)
                .HasPrecision(10, 0);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.cNmUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.cSigla)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.cEMail)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.cTelefone)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.cCelular)
                .IsUnicode(false);

            modelBuilder.Entity<UsuarioPerfilEmpresa>()
                .Property(e => e.nCdUsuario)
                .HasPrecision(10, 0);

            modelBuilder.Entity<UsuarioPerfilEmpresa>()
                .Property(e => e.nCdPerfil)
                .HasPrecision(10, 0);

            modelBuilder.Entity<UsuarioPerfilEmpresa>()
                .Property(e => e.nCdEmpresa)
                .HasPrecision(10, 0);

            modelBuilder.Entity<Usuarios>()
                .HasMany(e => e.AcompanhamentoTarefa)
                .WithRequired(e => e.Usuarios)
                .HasForeignKey(e => e.IdParticipanteEnvio)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuarios>()
                .HasMany(e => e.Cabecalho)
                .WithOptional(e => e.Usuarios)
                .HasForeignKey(e => e.IdParticipanteCriador);

            modelBuilder.Entity<Usuarios>()
                .HasMany(e => e.NiveisUsuarios)
                .WithRequired(e => e.Usuarios)
                .HasForeignKey(e => e.Usuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuarios>()
                .HasMany(e => e.PlanoDeAcaoQuemQuandoComo)
                .WithOptional(e => e.Usuarios)
                .HasForeignKey(e => e.IdUsuario);

            modelBuilder.Entity<Usuarios>()
                .HasMany(e => e.TarefaPA)
                .WithOptional(e => e.Usuarios)
                .HasForeignKey(e => e.IdParticipanteCriador);

            modelBuilder.Entity<Usuarios>()
                .HasMany(e => e.UsuarioUnidades)
                .WithRequired(e => e.Usuarios)
                .HasForeignKey(e => e.Usuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuarios>()
                .HasMany(e => e.VinculoCampoCabecalho)
                .WithOptional(e => e.Usuarios)
                .HasForeignKey(e => e.IdParticipante);

            modelBuilder.Entity<Usuarios>()
                .HasMany(e => e.VinculoCampoTarefa)
                .WithOptional(e => e.Usuarios)
                .HasForeignKey(e => e.IdParticipante);

            modelBuilder.Entity<Usuarios>()
                .HasMany(e => e.VinculoParticipanteMultiplaEscolha)
                .WithRequired(e => e.Usuarios)
                .HasForeignKey(e => e.IdParticipante)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuarios>()
                .HasMany(e => e.VinculoParticipanteProjeto)
                .WithRequired(e => e.Usuarios)
                .HasForeignKey(e => e.IdParticipante)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VacuoGRD>()
                .Property(e => e.Departamento)
                .IsUnicode(false);

            modelBuilder.Entity<VerificacaoContagem>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<VerificacaoTipificacao>()
                .Property(e => e.Chave)
                .IsUnicode(false);

            modelBuilder.Entity<VerificacaoTipificacaoComparacao>()
                .Property(e => e.Identificador)
                .IsUnicode(false);

            modelBuilder.Entity<VerificacaoTipificacaoResultados>()
                .Property(e => e.Chave)
                .IsUnicode(false);

            modelBuilder.Entity<VerificacaoTipificacaoValidacao>()
                .Property(e => e.cIdentificadorTipificacao)
                .IsUnicode(false);

            modelBuilder.Entity<VinculoCampoCabecalho>()
                .Property(e => e.Valor)
                .IsUnicode(false);

            modelBuilder.Entity<VinculoCampoTarefa>()
                .Property(e => e.Valor)
                .IsUnicode(false);

            modelBuilder.Entity<VolumeCepDesossa>()
                .Property(e => e.Departamento)
                .IsUnicode(false);

            modelBuilder.Entity<VolumeCepRecortes>()
                .Property(e => e.Departamento)
                .IsUnicode(false);

            modelBuilder.Entity<VolumePcc1b>()
                .Property(e => e.Departamento)
                .IsUnicode(false);

            modelBuilder.Entity<VolumePcc1b>()
                .Property(e => e.Meta)
                .HasPrecision(10, 5);

            modelBuilder.Entity<VolumeProducao>()
                .Property(e => e.Meta)
                .HasPrecision(5, 2);

            modelBuilder.Entity<VolumeProducao>()
                .Property(e => e.ToleranciaDia)
                .HasPrecision(11, 8);

            modelBuilder.Entity<VolumeProducao>()
                .Property(e => e.Nivel1)
                .HasPrecision(11, 8);

            modelBuilder.Entity<VolumeProducao>()
                .Property(e => e.Nivel2)
                .HasPrecision(11, 8);

            modelBuilder.Entity<VolumeProducao>()
                .Property(e => e.Nivel3)
                .HasPrecision(11, 8);

            modelBuilder.Entity<VolumeVacuoGRD>()
                .Property(e => e.Departamento)
                .IsUnicode(false);

            modelBuilder.Entity<VTVerificacaoContagem>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<VTVerificacaoTipificacaoComparacao>()
                .Property(e => e.Identificador)
                .IsUnicode(false);

            modelBuilder.Entity<VTVerificacaoTipificacaoValidacao>()
                .Property(e => e.cIdentificadorTipificacao)
                .IsUnicode(false);

            modelBuilder.Entity<Z_Sistema>()
                .Property(e => e.VersaoAPP)
                .IsUnicode(false);

            modelBuilder.Entity<Job1>()
                .HasMany(e => e.JobParameter1)
                .WithRequired(e => e.Job1)
                .HasForeignKey(e => e.JobId);

            modelBuilder.Entity<Job1>()
                .HasMany(e => e.State1)
                .WithRequired(e => e.Job1)
                .HasForeignKey(e => e.JobId);

            modelBuilder.Entity<ControleMetaTolerancia>()
                .Property(e => e.ProximaMetaTolerancia)
                .HasPrecision(11, 8);

            modelBuilder.Entity<ControleMetaTolerancia>()
                .Property(e => e.UltimoNumeroNC)
                .HasPrecision(11, 8);

            modelBuilder.Entity<ControleMetaTolerancia>()
                .Property(e => e.TotalNC)
                .HasPrecision(11, 8);

            modelBuilder.Entity<ParGroupParLevel1>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ParGroupParLevel1Type>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ParReports>()
                .Property(e => e.groupReport)
                .IsUnicode(false);

            modelBuilder.Entity<ParReports>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<ParReports>()
                .Property(e => e.query)
                .IsUnicode(false);

            modelBuilder.Entity<ResultLevel2HeaderField>()
                .Property(e => e.PunishmentValue)
                .HasPrecision(10, 5);

            modelBuilder.Entity<RoleJBS>()
                .Property(e => e.Role)
                .IsFixedLength();

            modelBuilder.Entity<ScorecardConsolidadoDia>()
                .Property(e => e.TotalAvaliado)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ScorecardConsolidadoDia>()
                .Property(e => e.TotalForaPadrao)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ScorecardConsolidadoDia>()
                .Property(e => e.Meta)
                .HasPrecision(5, 2);

            modelBuilder.Entity<ScorecardConsolidadoDia>()
                .Property(e => e.Pontos)
                .HasPrecision(5, 2);

            modelBuilder.Entity<VerificacaoTipificacaoJBS>()
                .Property(e => e.cIdentificadorTipificacao)
                .IsUnicode(false);

            modelBuilder.Entity<VTVerificacaoTipificacaoJBS>()
                .Property(e => e.cIdentificadorTipificacao)
                .IsUnicode(false);

            modelBuilder.Entity<Reports_CCA_Audit>()
                .Property(e => e.Audit_Area)
                .IsUnicode(false);

            modelBuilder.Entity<Reports_CCA_Audit>()
                .Property(e => e.Set_Start_Time)
                .IsUnicode(false);

            modelBuilder.Entity<Reports_CCA_Audit>()
                .Property(e => e.Audit_Reaudit)
                .IsUnicode(false);

            modelBuilder.Entity<Reports_CCA_Audit>()
                .Property(e => e.Specks)
                .HasPrecision(10, 5);

            modelBuilder.Entity<Reports_CCA_Audit>()
                .Property(e => e.Dressing)
                .HasPrecision(10, 5);

            modelBuilder.Entity<Reports_CCA_Audit>()
                .Property(e => e.Single_Hairs)
                .HasPrecision(10, 5);

            modelBuilder.Entity<Reports_CCA_Audit>()
                .Property(e => e.Clusters)
                .HasPrecision(10, 5);

            modelBuilder.Entity<Reports_CCA_Audit>()
                .Property(e => e.Hide)
                .HasPrecision(10, 5);

            modelBuilder.Entity<Reports_CCA_Audit>()
                .Property(e => e.Defects)
                .HasPrecision(38, 5);

            modelBuilder.Entity<Reports_CCA_Audit>()
                .Property(e => e.No_of_Def)
                .IsUnicode(false);

            modelBuilder.Entity<Reports_CCA_Audit>()
                .Property(e => e.Cattle_Type)
                .IsUnicode(false);

            modelBuilder.Entity<Reports_CCA_Audit>()
                .Property(e => e.Chain_Speed)
                .HasPrecision(20, 5);

            modelBuilder.Entity<Reports_CCA_Audit>()
                .Property(e => e.Lot__)
                .HasPrecision(20, 5);

            modelBuilder.Entity<Reports_CCA_Audit>()
                .Property(e => e.Mud_Score)
                .HasPrecision(20, 5);

            modelBuilder.Entity<Reports_CCA_Audit>()
                .Property(e => e.Set_End_Time)
                .IsUnicode(false);

            modelBuilder.Entity<Reports_CFF_Audit>()
                .Property(e => e.Audit_Area)
                .IsUnicode(false);

            modelBuilder.Entity<Reports_CFF_Audit>()
                .Property(e => e.Set_Start_Time)
                .IsUnicode(false);

            modelBuilder.Entity<Reports_CFF_Audit>()
                .Property(e => e.Set_End_Time)
                .IsUnicode(false);

            modelBuilder.Entity<Reports_CFF_Audit>()
                .Property(e => e.Reaudit)
                .IsUnicode(false);

            modelBuilder.Entity<Reports_CFF_Audit>()
                .Property(e => e.Defects)
                .HasPrecision(38, 5);

            modelBuilder.Entity<Reports_CFF_Audit>()
                .Property(e => e.Cut)
                .HasPrecision(38, 5);

            modelBuilder.Entity<Reports_CFF_Audit>()
                .Property(e => e.Fold_Flap)
                .HasPrecision(38, 5);

            modelBuilder.Entity<Reports_CFF_Audit>()
                .Property(e => e.Puncture)
                .HasPrecision(38, 5);

            modelBuilder.Entity<Reports_HTP_Audit>()
                .Property(e => e.Start_Time)
                .IsUnicode(false);

            modelBuilder.Entity<Reports_HTP_Audit>()
                .Property(e => e.Job_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Reports_HTP_Audit>()
                .Property(e => e.Audit_ReAudit)
                .IsUnicode(false);

            modelBuilder.Entity<Reports_HTP_Audit>()
                .Property(e => e.Baised_Unbiased)
                .IsUnicode(false);

            modelBuilder.Entity<Reports_HTP_Audit>()
                .Property(e => e.Job_Time)
                .IsUnicode(false);

            modelBuilder.Entity<Reports_HTP_Audit>()
                .Property(e => e.Core_Practice_HTP_Deviation)
                .IsUnicode(false);

            modelBuilder.Entity<Reports_HTP_Audit>()
                .Property(e => e.Other_Deviaton)
                .IsUnicode(false);

            modelBuilder.Entity<Reports_HTP_Audit>()
                .Property(e => e.Reaudit)
                .IsUnicode(false);

            modelBuilder.Entity<Reports_HTP_Audit>()
                .Property(e => e.End_Time)
                .IsUnicode(false);

            modelBuilder.Entity<ScorecardJBS_V>()
                .Property(e => e.TipoIndicadorName)
                .IsUnicode(false);

            modelBuilder.Entity<ScorecardJBS_V>()
                .Property(e => e.AV)
                .HasPrecision(32, 8);

            modelBuilder.Entity<ScorecardJBS_V>()
                .Property(e => e.NC)
                .HasPrecision(30, 8);

            modelBuilder.Entity<ScorecardJBS_V>()
                .Property(e => e.Pontos)
                .HasPrecision(10, 5);

            modelBuilder.Entity<ScorecardJBS_V>()
                .Property(e => e.Meta)
                .HasPrecision(10, 5);

            modelBuilder.Entity<ScorecardJBS_V>()
                .Property(e => e.Real)
                .HasPrecision(38, 6);

            modelBuilder.Entity<ScorecardJBS_V>()
                .Property(e => e.PontosAtingidos)
                .HasPrecision(38, 6);

            modelBuilder.Entity<ScorecardJBS_V>()
                .Property(e => e.Scorecard)
                .HasPrecision(38, 6);

            modelBuilder.Entity<VWCFFResults>()
                .Property(e => e.Audit_Area)
                .IsUnicode(false);

            modelBuilder.Entity<VWCFFResults>()
                .Property(e => e.Set_Start_Time)
                .IsUnicode(false);

            modelBuilder.Entity<VWCFFResults>()
                .Property(e => e.Set_End_Time)
                .IsUnicode(false);

            modelBuilder.Entity<VWCFFResults>()
                .Property(e => e.Reaudit)
                .IsUnicode(false);

            modelBuilder.Entity<VWCFFResults>()
                .Property(e => e.Defects)
                .HasPrecision(38, 5);

            modelBuilder.Entity<VWCFFResults>()
                .Property(e => e.Cut)
                .HasPrecision(38, 5);

            modelBuilder.Entity<VWCFFResults>()
                .Property(e => e.Fold_Flap)
                .HasPrecision(38, 5);

            modelBuilder.Entity<VWCFFResults>()
                .Property(e => e.Puncture)
                .HasPrecision(38, 5);
        }
    }
}
