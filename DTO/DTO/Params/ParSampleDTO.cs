using DTO.BaseEntity;
using System;
using System.ComponentModel.DataAnnotations;

namespace DTO.DTO.Params
{
    public class ParSampleDTO : EntityBase
    {
        public Nullable<int> ParCompany_Id { get; set; }
        public int ParLevel2_Id { get; set; }

        [Required(ErrorMessage = "Amostra deve ser maior que 0.")]
        [Range(1, 999, ErrorMessage = "Amostra deve ser maior que 0.")]
        public int Number { get; set; }
        public bool IsActive { get; set; } = true;
    }
}