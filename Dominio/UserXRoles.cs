namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserXRoles
    {
        public int Id { get; set; }

        public int User_Id { get; set; }

        public int Role_Id { get; set; }

        public virtual RoleUserSgq RoleUserSgq { get; set; }

        public virtual UserSgq UserSgq { get; set; }
    }
}
