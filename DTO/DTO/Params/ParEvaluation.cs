using DTO.BaseEntity;
using System.ComponentModel.DataAnnotations;

namespace DTO.DTO.Params
{
    public class ParEvaluationDTO : EntityBase
    {
        public int ParCompany_Id { get; set; }
        public int ParLevel2_Id { get; set; }

        [Required(ErrorMessage = "Avaliação deve ser maior que 0.")]
        [Range(1, 999, ErrorMessage = "Avaliação deve ser mair que 0.")]
        public int Number { get; set; }
        public bool IsActive { get; set; } = true;

    }
}