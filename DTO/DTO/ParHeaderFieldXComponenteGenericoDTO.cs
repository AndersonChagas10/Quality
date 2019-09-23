using DTO.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTO.DTO
{
    public class ParHeaderFieldXComponenteGenericoDTO : EntityBase
    {
        public int ComponenteGenerico_Id { get; set; }
        public int ParHeaderField_Id { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public string ValueName { get; set; }
        public string TextName { get; set; }
    }
}