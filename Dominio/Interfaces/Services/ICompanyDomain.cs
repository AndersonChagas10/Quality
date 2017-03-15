using DTO.DTO.Params;
using System.Collections.Generic;

namespace Dominio.Interfaces.Services
{
    public interface ICompanyDomain
    {

        ParCompanyDTO AddUpdateParCompany(ParCompanyDTO parCompanyDTO);
        ParStructureDTO AddUpdateParStructure(ParStructureDTO parStructureDTO);
        ParStructureGroupDTO AddUpdateParStructureGroup(ParStructureGroupDTO parStructureGroupDTO);
        ParCompanyXStructureDTO AddUpdateParCompanyXStructureDTO(ParCompanyXStructureDTO parCompanyXStructureDTO);

        void SaveParCompany(ParCompany parCompany);
        void SaveParCompanyXStructure(List<ParCompanyXStructure> listParCompanyXStructure, ParCompany parCompany);
        void SaveParCompanyCluster(List<ParCompanyCluster> listParCompanyCluster, ParCompany parCompany);
    }
}
