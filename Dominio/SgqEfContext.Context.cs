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
    
    public partial class SgqDbDevEntities : DbContext
    {
        public SgqDbDevEntities()
            : base("name=SgqDbDevEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Coleta> Coleta { get; set; }
        public virtual DbSet<DataCollection> DataCollection { get; set; }
        public virtual DbSet<DataCollectionResult> DataCollectionResult { get; set; }
        public virtual DbSet<Level01> Level01 { get; set; }
        public virtual DbSet<Level01Consolidation> Level01Consolidation { get; set; }
        public virtual DbSet<Level02> Level02 { get; set; }
        public virtual DbSet<Level02Consolidation> Level02Consolidation { get; set; }
        public virtual DbSet<Level03Consolidation> Level03Consolidation { get; set; }
        public virtual DbSet<Level1> Level1 { get; set; }
        public virtual DbSet<Level2> Level2 { get; set; }
        public virtual DbSet<Level3> Level3 { get; set; }
        public virtual DbSet<teste> teste { get; set; }
        public virtual DbSet<UserSgq> UserSgq { get; set; }
        public virtual DbSet<Level03> Level03 { get; set; }
        public virtual DbSet<Period> Period { get; set; }
        public virtual DbSet<Shift> Shift { get; set; }
        public virtual DbSet<CorrectiveAction> CorrectiveAction { get; set; }
        public virtual DbSet<CorrectiveActionLevels> CorrectiveActionLevels { get; set; }
    }
}
