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
    public class ReportXUserSgq : BaseModel
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Relatorio")]
        public int ItemMenu_Id { get; set; }
        [Display(Name = "Elaborador")]
        public string Elaborador { get; set; }
        [Display(Name = "Aprovador")]
        public string Aprovador { get; set; }
        [Display(Name = "Indicador")]
        public int ParLevel1_Id { get; set; }
        [Display(Name = "Unidade")]
        public int? ParCompany_Id { get; set; }
        [Display(Name = "Ativo")]
        public bool IsActive { get; set; }

        [ForeignKey("ItemMenu_Id")]
        public ItemMenu ItemMenu { get; set; }

        [ForeignKey("ParLevel1_Id")]
        public ParLevel1 ParLevel1 { get; set; }

        [ForeignKey("ParCompany_Id")]
        public ParCompany ParCompany { get; set; }

        [Display(Name = "Codigo do Relatório")]
        public string CodigoRelatorio { get; set; }

    }
}
