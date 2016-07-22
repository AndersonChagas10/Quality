using AutoMapper;
using DTO.DTO;
using DTO.Helpers;
using System.Collections.Generic;

namespace SgqSystem.ViewModels
{
    public class ColetaViewModel : ColetaDTO
    {
        //public ColetaDTO resultado { get; set; }
        public List<ColetaDTO> listaResultado { get; set; }
      
    }
}