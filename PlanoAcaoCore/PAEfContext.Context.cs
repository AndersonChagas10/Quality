﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PlanoAcaoCore
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PlanoDeAcaoEntities : DbContext
    {
        public PlanoDeAcaoEntities()
            : base("name=PlanoDeAcaoEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
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
        public virtual DbSet<Pa_Iniciativa> Pa_Iniciativa { get; set; }
        public virtual DbSet<Pa_Missao> Pa_Missao { get; set; }
        public virtual DbSet<Pa_Objetivo> Pa_Objetivo { get; set; }
        public virtual DbSet<Pa_ObjetivoGeral> Pa_ObjetivoGeral { get; set; }
        public virtual DbSet<Pa_Planejamento> Pa_Planejamento { get; set; }
        public virtual DbSet<Pa_Quem> Pa_Quem { get; set; }
        public virtual DbSet<Pa_Status> Pa_Status { get; set; }
        public virtual DbSet<Pa_TemaAssunto> Pa_TemaAssunto { get; set; }
        public virtual DbSet<Pa_Unidade> Pa_Unidade { get; set; }
        public virtual DbSet<Pa_Visao> Pa_Visao { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Pa_AcaoXQuem> Pa_AcaoXQuem { get; set; }
        public virtual DbSet<Pa_CausaMedidaXAcao> Pa_CausaMedidaXAcao { get; set; }
    }
}
