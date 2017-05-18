using DTO.BaseEntity;
using System;

namespace DTO.DTO
{
    public class VTVerificacaoTipificacaoDTO : DataCollectionBase
    {
        public int Sequencial { get; set; }
        public byte Banda { get; set; }
        public System.DateTime DataHora { get; set; }
        public int UnidadeId { get; set; }
        public string Chave { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}
