using DTO.DTO;
using DTO.DTO.Params;
using System;
using System.Collections.Generic;

namespace Dominio.Interfaces.Services
{
    public interface IDefectDomain
    {
        List<Defect> MergeDefect(List<DefectDTO> defectDTO);
    }
}
