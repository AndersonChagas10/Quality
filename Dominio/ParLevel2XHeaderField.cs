namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParLevel2XHeaderField : BaseModel 
    {
        public int Id { get; set; }

        public int ParLevel2_Id { get; set; }

        public int ParLevel1_Id { get; set; }

        public int ParHeaderField_Id { get; set; }

        public bool IsActive { get; set; }
    }
}
