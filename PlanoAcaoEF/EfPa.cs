namespace PlanoAcaoEF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EfPa : DbContext
    {
        public EfPa()
            : base("name=EfPa")
        {
        }

        public virtual DbSet<Pa_Acao> Pa_Acao { get; set; }
        public virtual DbSet<Pa_AcaoXQuem> Pa_AcaoXQuem { get; set; }
        public virtual DbSet<Pa_CausaMedidaXAcao> Pa_CausaMedidaXAcao { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pa_Acao>()
                .Property(e => e.QuantoCusta)
                .HasPrecision(35, 10);
        }
    }
}
