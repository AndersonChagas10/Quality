using System;

namespace Conformity.Domain.Core.Entities.PlanoDeAcao
{
    public class EvidenciaNaoConformidade
    {
        public int Id { get; set; }
        public int Acao_Id { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime? AlterDate { get; set; }
        public bool IsActive { get; set; }
    }
}
