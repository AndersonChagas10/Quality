using System.Collections.Generic;

namespace DTO.DTO
{
    /// <summary>
    /// Listas : 
    /// dataCollectionDTO,
    /// dataCollectionResultDTO.
    /// level03ConsolidationDTO
    /// </summary>
    public class ObjectConsildationDTO
    {
        public Level01ConsolidationDTO level01ConsolidationDTO { get; set; }

        public List<Level02ConsolidationDTO> level02ConsolidationDTO { get; set; }
        public List<DataCollectionDTO> dataCollectionDTO { get; set; }
        public List<Level03ConsolidationDTO> level03ConsolidationDTO { get; set; }
        public List<DataCollectionResultDTO> dataCollectionResultDTO { get; set; }
    }
}
