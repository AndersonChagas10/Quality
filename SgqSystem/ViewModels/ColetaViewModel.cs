using DTO.DTO;
using System.Collections.Generic;

namespace SgqSystem.ViewModels
{
    public class ColetaViewModel : ColetaDTO
    {
        //public ColetaDTO resultado { get; set; }
        public List<ColetaDTO> listaResultado { get; set; }
      
    }
}