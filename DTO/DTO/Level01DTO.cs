﻿using DTO.BaseEntity;

namespace DTO.DTO
{
    public class Level01DTO : EntityBase
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public bool Active { get; set; }
    }
}
