namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RecravacaoJson")]
    public partial class RecravacaoJson : BaseModel
    {
        public int Id { get; set; }

        public int UserSgqId { get; set; }

        public int ParCompany_Id { get; set; }

        public int Linha_Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string ObjectRecravacaoJson { get; set; }

        public bool? IsActive { get; set; }

        public bool? isValidated { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ValidateLockDate { get; set; }

        public int? ParLevel1_Id { get; set; }

        public int? SalvoParaInserirNovaColeta { get; set; }

        public int? UserFinished_Id { get; set; }

        public int? UserValidated_Id { get; set; }
    }
}
