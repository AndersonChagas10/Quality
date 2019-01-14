namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParCompanyXUserSgq")]
    public partial class ParCompanyXUserSgq
    {
        public int Id { get; set; }

        public int UserSgq_Id { get; set; }

        public int ParCompany_Id { get; set; }

        public string Role { get; set; }

        [ForeignKey("ParCompany_Id")]
        public virtual ParCompany ParCompany { get; set; }

        [ForeignKey("UserSgq_Id")]
        public virtual UserSgq UserSgq { get; set; }
    }
}
