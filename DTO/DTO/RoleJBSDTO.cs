using DTO.BaseEntity;
using System;

namespace DTO.DTO
{
    public class RoleJBSDTO : EntityBase
    {
        public int ScreenComponent_Id { get; set; }
        public string Role { get; set; }
        public ScreenComponentDTO ScreenComponent { get; set; }
    }
}
