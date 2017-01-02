using DTO;
using DTO.DTO;
using DTO.DTO.Params;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SgqSystem.ViewModels
{
    public class FormularioParaRelatorioViewModel : DataCarrierFormulario
    {
        //private readonly IBaseApp<Dominio.Unit, UnitDTO> _Unit;
        //public FormularioParaRelatorioViewModel(IBaseApp<Dominio.Unit, UnitDTO> Unit
        //    )
        //{
        //    _Unit = Unit;


        //}

        #region Retorno

        public ResultSetRelatorioColeta result { get; set; }
        public ResultSetGetCorrectiveAction resultSetGetCorrectiveAction { get; set; }

        #endregion
        
        #region SelectListItem

        public IEnumerable<SelectListItem> level01DTOSelectList { get; set; }
        internal void SetLevel01SelectList(IEnumerable<Level01DTO> enumerable)
        {
            level01DTOSelectList = CriaSelectList(enumerable);
        }

        public IEnumerable<SelectListItem> level02DTOSelectList { get; set; }
        internal void Setlevel02SelectList(IEnumerable<Level02DTO> enumerable)
        {
            level02DTOSelectList = CriaSelectList(enumerable);
        }

        public IEnumerable<SelectListItem> level03DTOSelectList { get; set; }
        internal void SetLevel03SelectList(IEnumerable<Level03DTO> enumerable)
        {
            level02DTOSelectList = CriaSelectList(enumerable);

        }

        public IEnumerable<SelectListItem> UnitsSelectList { get; set; }
        internal void SetUnitsSelectList(IEnumerable<ParCompanyDTO> enumerable)
        {
            UnitsSelectList = CriaSelectList(enumerable);

        }

        public IEnumerable<SelectListItem> ShiftSelectList { get; set; }
        internal void SetShiftSelectList(/*IEnumerable<ShiftDTO> enumerable*/)
        {
            //MOCK tabela fora de padrão 29 09 2016 celsogea.
            ShiftSelectList = new List<SelectListItem>(){
                                                    new SelectListItem() {Text="Shift A", Value="1"},
                                                    new SelectListItem() {Text="Shift B", Value="2"},
                                                };
        }

        public IEnumerable<SelectListItem> PeriodSelectList { get; set; }
        internal void SetPeriodSelectList(IEnumerable<PeriodDTO> enumerable)
        {
            PeriodSelectList = CriaSelectList(enumerable);

        }

        public IEnumerable<SelectListItem> UserSelectList { get; set; }
        internal void SetUserSelectList(IEnumerable<UserDTO> enumerable)
        {
            UserSelectList = CriaSelectList(enumerable);

        }

        private List<SelectListItem> CriaSelectList<T>(IEnumerable<T> enumerable) 
        {
            List<SelectListItem> retorno = new List<SelectListItem>();
            var counter = 0;
            foreach (var i in enumerable)
            {

                var text = i.GetType().GetProperty("Name") ?? i.GetType().GetProperty("Description");
                var prop = i.GetType().GetProperty("Id");
                retorno.Insert(counter, new SelectListItem() { Text = text.GetValue(i).ToString(), Value = prop.GetValue(i).ToString() });
                counter++;
            }

            return retorno;
        }

        #endregion

        


    }
}