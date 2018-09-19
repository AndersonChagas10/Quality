using Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    [Table("ReportXUserSgq")]
    public class ReportXUserSgq
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Data de Adição")]
        public DateTime AddDate { get; set; }
        [Display(Name = "Data de Alteração")]
        public DateTime? AlterDate { get; set; }
        [Display(Name = "Relatorio")]
        public int ItemMenu_Id { get; set; }
        [Display(Name = "Elaborador")]
        public int Elaborador_Id { get; set; }
        [Display(Name = "Editor")]
        public int Editor_Id { get; set; }
        [Display(Name = "Indicador")]
        public int ParLevel1_Id { get; set; }
        [Display(Name = "Unidade")]
        public int? ParCompany_Id { get; set; }
        [Display(Name = "Ativo")]
        public bool IsActive { get; set; }

        [ForeignKey("ItemMenu_Id")]
        public ItemMenu ItemMenu { get; set; }

        [ForeignKey("Elaborador_Id")]
        public UserSgq Elaborador { get; set; }

        [ForeignKey("Editor_Id")]
        public UserSgq Editor { get; set; }

        [ForeignKey("ParLevel1_Id")]
        public ParLevel1 ParLevel1 { get; set; }

        [ForeignKey("ParCompany_Id")]
        public ParCompany ParCompany { get; set; }

    }
}
