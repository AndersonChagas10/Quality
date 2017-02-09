using DTO.BaseEntity;
using System;

namespace DTO.DTO
{
    public class ScreenComponentDTO : EntityBase
    {

        public int Id { get; set; }
        public string HashKey { get; set; }
        public string Component { get; set; }
        public Nullable<int> Type { get; set; }
    }
}
