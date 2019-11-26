using DTO.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO
{
    public class ItemMenuDTO : EntityBase
    {
        public Nullable<int> ItemMenu_Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string Resource { get; set; }
        public bool IsActive { get; set; }

        public bool IsSubItem { get; set; }

        public int? PDCAMenuItem { get; set; }

        public ItemMenuDTO MenuPredecessor { get; set; }
        public List<ItemMenuDTO> ListaFilho { get; set; } = new List<ItemMenuDTO>();
 
    }
}
