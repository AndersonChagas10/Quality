using System.Collections.Generic;

namespace DTO.DTO
{
    public class SyncDTO
    {
        public List<ColetaDTO> Coleta { get; set; }
        public List<Level1DTO> Level1 { get; set; }
        public List<Level2DTO> Level2 { get; set; }
        public List<Level3DTO> Level3 { get; set; }
        public List<UserDTO> UserSgq { get; set; }
        public List<CorrectiveActionDTO> CorrectiveAction { get; set; }
    }
}
