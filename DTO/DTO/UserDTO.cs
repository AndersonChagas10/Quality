using DTO.Entities.BaseEntity;

namespace DTO.DTO
{
    public class UserDTO : EntityBase
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
