using Conformity.Domain.Core.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conformity.Domain.Core.Entities.PlanoDeAcao
{
    [Table("UserSgq")]
    public partial class UserSgq : BaseModel, IEntity
    {

        [Required(AllowEmptyStrings = true)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Password { get; set; }

        public DateTime? AcessDate { get; set; }

        public string Role { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public int? ParCompany_Id { get; set; }

        public DateTime? PasswordDate { get; set; }

        public bool? IsActive { get; set; }
    }
}
