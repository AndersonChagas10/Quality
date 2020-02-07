using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Pa_Acompanhamento
    {
        public int Id { get; set; }
        public DateTime? AddDate { get; set; }
        public DateTime? AlterDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Acao_Id { get; set; }
        public int? Order { get; set; }
        public int Status_Id { get; set; }
        public int Author_Id { get; set; }
    }
}
