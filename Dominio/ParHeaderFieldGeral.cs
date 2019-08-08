using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    [Table("ParHeaderFieldGeral")]
    public class ParHeaderFieldGeral : BaseModel
    {
        public ParHeaderFieldGeral()
        {
            ParMultipleValuesGeral = new HashSet<ParMultipleValuesGeral>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int ParFieldType_Id { get; set; }
        public string Description { get; set; }
        public int ParLevelHeaderField_Id { get; set; }
        public int Generic_Id { get; set; }
        public bool LinkNumberEvaluation { get; set; }
        public bool IsActive { get; set; }
        public bool IsRequired { get; set; }
        public bool Duplicate { get; set; }

        [ForeignKey("ParFieldType_Id")]
        public virtual ParFieldType ParFieldType { get; set; }

        [ForeignKey("ParLevelHeaderField_Id")]
        public virtual ParLevelHeaderField ParLevelHeaderField { get; set; }

        public virtual ICollection<ParMultipleValuesGeral> ParMultipleValuesGeral { get; set; }
    }
}
