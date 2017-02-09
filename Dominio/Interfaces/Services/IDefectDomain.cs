using DTO.DTO;
using DTO.DTO.Params;
using System;
using System.Collections.Generic;

namespace Dominio.Interfaces.Services
{
    public interface IDefectDomain
    {
        void MergeDefect(List<DefectDTO> defectDTO);
        List<DefectDTO> GetDefects(int ParCompany_Id);


    }
}
