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
    
    
    this.Database.CommandTimeout = 9600;
    this.Database.Log = s => System.Diagnostics.Debug.Write(s);
    
    
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CollectionLevel02> CollectionLevel02 { get; set; }
        public virtual DbSet<CollectionLevel03> CollectionLevel03 { get; set; }
        public virtual DbSet<ConsolidationLevel01> ConsolidationLevel01 { get; set; }
        public virtual DbSet<ConsolidationLevel02> ConsolidationLevel02 { get; set; }
        public virtual DbSet<CorrectiveAction> CorrectiveAction { get; set; }
        public virtual DbSet<CorrectiveActionLevels> CorrectiveActionLevels { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Level01> Level01 { get; set; }
        public virtual DbSet<Level02> Level02 { get; set; }
        public virtual DbSet<Level03> Level03 { get; set; }
        public virtual DbSet<Period> Period { get; set; }
        public virtual DbSet<Shift> Shift { get; set; }
        public virtual DbSet<Unit> Unit { get; set; }
        public virtual DbSet<UnitUser> UnitUser { get; set; }
        public virtual DbSet<UserSgq> UserSgq { get; set; }
    }
}
