using AutoMapper;
using DTO.DTO;
using DTO.Helpers;
using System.Collections.Generic;

namespace SgqSystem.ViewModels
{
    public class ResultOldViewModel : ResultOldDTO
    {
        //public ResultOldDTO resultado { get; set; }
        public List<ResultOldDTO> listaResultado { get; set; }
      
    }
}