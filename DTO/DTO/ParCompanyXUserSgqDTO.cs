using DTO.BaseEntity;
using DTO.DTO.Params;

namespace DTO.DTO
{
    public class ParCompanyXUserSgqDTO : EntityBase
    {
        public int UserSgq_Id { get; set; }
        public int ParCompany_Id { get; set; }
        public string Role { get; set; }

        public ParCompanyDTO ParCompany { get; set; }
    }
}
