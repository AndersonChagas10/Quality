using Conformity.Domain.Core.Entities.PlanoDeAcao;
using System.Data.Entity;

namespace Conformity.Infra.Data.Core
{
    public class PlanoDeAcaoEntityContext : EntityContext
    {
        public virtual DbSet<Acao> Acao { get; set; }
        public virtual DbSet<AcompanhamentoAcao> AcompanhamentoAcao { get; set; }
        public virtual DbSet<ParLevel1> ParLevel1 { get; set; }
        public virtual DbSet<ParLevel2> ParLevel2 { get; set; }
        public virtual DbSet<ParLevel3> ParLevel3 { get; set; }
        public virtual DbSet<ParCargo> ParCargo { get; set; }
        public virtual DbSet<ParDepartment> ParDepartment { get; set; }
        public virtual DbSet<UserSgq> UserSgq { get; set; }
        public virtual DbSet<ParCluster> ParCluster { get; set; }
        public virtual DbSet<ParClusterGroup> ParClusterGroup { get; set; }
        public virtual DbSet<Conformity.Domain.Core.Entities.PlanoDeAcao.ParCompany> PA_ParCompany { get; set; }
    }
}
