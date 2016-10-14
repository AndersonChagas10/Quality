using DTO.BaseEntity;
using System;
using System.Collections.Generic;

namespace DTO.DTO
{
    public class Level02DTO : EntityBase
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public bool Active { get; set; }

        //public  List<ConsolidationLevel02DTO> ConsolidationLevel02 { get; set; }
       // public List<CollectionLevel02DTO> CollectionLevel02 { get; set; }
    }
}
