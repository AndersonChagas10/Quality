using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqSystem.ViewModels
{
    public class SyncViewModel
    {
        public List<ColetaDTO> Coleta { get; set; }
        public List<Level1DTO> Level1 { get; set; }
        public List<Level2DTO> Level2 { get; set; }
        public List<Level3DTO> Level3 { get; set; }
        public List<UserDTO> UserSgq { get; set; }
    }
}