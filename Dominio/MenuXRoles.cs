namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MenuXRoles
    {
        public int Id { get; set; }

        public int Menu_Id { get; set; }

        public int Role_Id { get; set; }

        public virtual Menu Menu { get; set; }

        public virtual RoleUserSgq RoleUserSgq { get; set; }
    }
}
