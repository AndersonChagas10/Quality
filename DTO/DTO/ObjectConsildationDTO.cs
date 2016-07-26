using System.Collections.Generic;

namespace DTO.DTO
{
    /// <summary>
    /// Listas : 
    /// level01ConsolidationDTO,
    /// level02ConsolidationDTO,
    /// level03ConsolidationDTO,
    /// dataCollectionDTO,
    /// dataCollectionResultDTO.
    /// </summary>
    public class ObjectConsildationDTO
    {
        public List<DataCollectionDTO> dataCollectionDTO { get; set; }
        public List<DataCollectionResultDTO> dataCollectionResultDTO { get; set; }
    }
}
