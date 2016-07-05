using System;

namespace SgqSystem.ViewModels.BaseEntityViewModel
{
    public class EntityBaseViewModel
    {
        public int Id { get; set; } = 0;
        public DateTime AddDate { get; set; } = DateTime.Now;
        public DateTime? AlterDate { get; set; } = null;
    }
}
