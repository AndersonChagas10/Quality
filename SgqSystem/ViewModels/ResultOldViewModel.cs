﻿using SgqSystem.ViewModels.BaseEntityViewModel;

namespace SgqSystem.ViewModels
{
    public class ResultOldViewModel : DataCollectionBaseViewModel
    {
        public int Id_Tarefa { get; set; }
        public int Id_Operacao { get; set; }
        public int Id_Monitoramento { get; set; }
        public int numero1 { get; set; }
        public int numero2 { get; set; }

        /// <summary>
        /// Construtor para o Auto Mapper.
        /// </summary>
        public ResultOldViewModel()
        {

        }
        
    }
}