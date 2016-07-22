using System.Collections.Generic;

namespace DTO.DTO
{
    public class SyncDTO
    {
        public List<ColetaDTO> Coleta { get; set; }
        public List<ColetaDTO> Level1 { get; set; }
        public List<ColetaDTO> Level2 { get; set; }
        public List<ColetaDTO> Level3 { get; set; }
        public List<ColetaDTO> UserSgq { get; set; }
    }
}
