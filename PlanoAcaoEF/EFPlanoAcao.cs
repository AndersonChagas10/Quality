namespace PlanoAcaoEF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EFPlanoAcao : DbContext
    {
        public EFPlanoAcao()
            : base("name=EFPlanoAcao")
        {
        }

        public virtual DbSet<Email_ConfiguracaoEmailSgq> Email_ConfiguracaoEmailSgq { get; set; }
        public virtual DbSet<EmailContent> EmailContent { get; set; }
        public virtual DbSet<Pa_Acao> Pa_Acao { get; set; }
        public virtual DbSet<Pa_CausaEspecifica> Pa_CausaEspecifica { get; set; }
        public virtual DbSet<Pa_CausaGenerica> Pa_CausaGenerica { get; set; }
        public virtual DbSet<Pa_ContramedidaEspecifica> Pa_ContramedidaEspecifica { get; set; }
        public virtual DbSet<Pa_ContramedidaGenerica> Pa_ContramedidaGenerica { get; set; }
        public virtual DbSet<Pa_Coordenacao> Pa_Coordenacao { get; set; }
        public virtual DbSet<Pa_Departamento> Pa_Departamento { get; set; }
        public virtual DbSet<Pa_Dimensao> Pa_Dimensao { get; set; }
        public virtual DbSet<Pa_Diretoria> Pa_Diretoria { get; set; }
        public virtual DbSet<Pa_Gerencia> Pa_Gerencia { get; set; }
        public virtual DbSet<Pa_GrupoCausa> Pa_GrupoCausa { get; set; }
        public virtual DbSet<Pa_Indicadores> Pa_Indicadores { get; set; }
        public virtual DbSet<Pa_IndicadoresDeProjeto> Pa_IndicadoresDeProjeto { get; set; }
        public virtual DbSet<Pa_IndicadoresDiretriz> Pa_IndicadoresDiretriz { get; set; }
        public virtual DbSet<Pa_IndicadorSgqAcao> Pa_IndicadorSgqAcao { get; set; }
        public virtual DbSet<Pa_Iniciativa> Pa_Iniciativa { get; set; }
        public virtual DbSet<Pa_Missao> Pa_Missao { get; set; }
        public virtual DbSet<Pa_Objetivo> Pa_Objetivo { get; set; }
        public virtual DbSet<Pa_ObjetivoGeral> Pa_ObjetivoGeral { get; set; }
        public virtual DbSet<Pa_Planejamento> Pa_Planejamento { get; set; }
        public virtual DbSet<Pa_Problema_Desvio> Pa_Problema_Desvio { get; set; }
        public virtual DbSet<Pa_Quem> Pa_Quem { get; set; }
        public virtual DbSet<Pa_Query> Pa_Query { get; set; }
        public virtual DbSet<Pa_Status> Pa_Status { get; set; }
        public virtual DbSet<Pa_TemaAssunto> Pa_TemaAssunto { get; set; }
        public virtual DbSet<Pa_Unidade> Pa_Unidade { get; set; }
        public virtual DbSet<Pa_UnidadeMedida> Pa_UnidadeMedida { get; set; }
        public virtual DbSet<Pa_Visao> Pa_Visao { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Pa_AcaoXQuem> Pa_AcaoXQuem { get; set; }
        public virtual DbSet<Pa_CausaMedidaXAcao> Pa_CausaMedidaXAcao { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pa_Acao>()
                .Property(e => e.QuantoCusta)
                .HasPrecision(35, 10);

            modelBuilder.Entity<Pa_CausaEspecifica>()
                .Property(e => e.Text)
                .IsFixedLength();

            modelBuilder.Entity<Pa_ContramedidaEspecifica>()
                .Property(e => e.Text)
                .IsFixedLength();

            modelBuilder.Entity<Pa_Planejamento>()
                .Property(e => e.ValorDe)
                .HasPrecision(32, 10);

            modelBuilder.Entity<Pa_Planejamento>()
                .Property(e => e.ValorPara)
                .HasPrecision(32, 10);

            
        }
    }
}
