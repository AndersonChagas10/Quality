using DTO.BaseEntity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;

namespace DTO.DTO.Params
{
    public class ParLevel3DTO : EntityBase
    {
        //[Required(ErrorMessage = "O Nome é obrigatório.")]
        //[MinLength(3, ErrorMessage = "O tamanho mínimo do Nome são 3 caracteres.")]
        //[MaxLength(300, ErrorMessage = "O tamanho máximo do Nome são 300 caracteres.")]
        public string Name { get; set; }

        //[Required(ErrorMessage = "A Descrição é obrigatória.")]
        //[MinLength(1, ErrorMessage = "O tamanho mínimo da Descrição deve ser 1 caracter.")]
        //[MaxLength(300, ErrorMessage = "O tamanho máximo da Descrição são 300 caracteres.")]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsPointLess { get; set; } = true;
        public bool hasVinculo { get; set; }
        public bool HasTakePhoto { get; set; } = false;
        public decimal pesoDoVinculo { get; set; }
        public virtual List<ParLevel3ValueDTO> ParLevel3Value { get; set; }
        public List<ParLevel3GroupDTO> listGroupsLevel2 { get; set; }
        public int? groupLevel2Selected;
        public Nullable<int> OrderColumn { get; set; }

        public List<ParRelapseDTO> listParRelapseDto { get; set; }
        public List<int> removeReincidencia { get; set; }
        public List<ParLevel3ValueDTO> listLevel3Value { get; set; }
        public List<ParLevel3Level2DTO> listLevel3Level2 { get; set; }
        public List<ParLevel3EvaluationSampleDTO> listParLevel3EvaluationSample { get; set; }
        public List<ParLevel3XDepartmentDTO> listParLevel3XDepartment { get; set; }

        public List<ParLevel3Value_OuterListDTO> ParLevel3Value_OuterList { get; set; }
        public IEnumerable<List<ParLevel3Value_OuterListDTO>> ParLevel3Value_OuterListGrouped { get; set; }
        public bool AllowNA { get; set; }
    }
}