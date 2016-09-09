using DTO.DTO;
using System.Collections.Generic;

namespace DTO
{
    public class ResultSetRelatorioColeta
    {
        public List<Level01DTO> listResultLevel01 { get; set; }
        public List<ConsolidationLevel01DTO> listConsolidationLevel01 { get; set; }
        public List<ConsolidationLevel02DTO> listConsolidationLevel02 { get; set; }
        public List<CollectionLevel02DTO> listCollectionLevel02DTO { get; set; }
        public List<CollectionLevel03DTO> listCollectionLevel03DTO { get; set; }
    }
}
