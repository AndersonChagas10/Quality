using DTO.DTO.Params;

namespace Dominio.Interfaces.Services
{
    public interface ICompanyDomain
    {

        ParCompanyDTO AddUpdateParCompany(ParCompanyDTO parCompanyDTO);
        ParStructureDTO AddUpdateParStructure(ParStructureDTO parStructureDTO);
        ParStructureGroupDTO AddUpdateParStructureGroup(ParStructureGroupDTO parStructureGroupDTO);
        ParCompanyXStructureDTO AddUpdateParCompanyXStructureDTO(ParCompanyXStructureDTO parCompanyXStructureDTO);

    }
}
