using DTO.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO
{
    public class VTVerificacaoTipificacaoDTO : DataCollectionBase
    {
        public int Id { get; set; }
        public int Sequencial { get; set; }
        public byte Banda { get; set; }
        public System.DateTime DataHora { get; set; }
        public int UnidadeId { get; set; }
        public string Chave { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}
