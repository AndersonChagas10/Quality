using DTO;
using DTO.DTO;
using DTO.DTO.Params;
using System.Collections.Generic;

namespace SgqService.ViewModels
{
    public partial class FormularioParaRelatorioViewModel : DataCarrierFormulario
    {

        public FormularioParaRelatorioViewModel()
        {

        }

        #region Retorno
        

        public ResultSetRelatorioColeta result { get; set; }
        public ResultSetGetCorrectiveAction resultSetGetCorrectiveAction { get; set; }

        #endregion

        #region SelectListItem

        public IEnumerable<KeyValuePair<string, string>> level01DTOSelectList { get; set; }
        internal void SetLevel01SelectList(IEnumerable<Level01DTO> enumerable)
        {
            level01DTOSelectList = CriaSelectList(enumerable);
        }

        public IEnumerable<KeyValuePair<string, string>> level02DTOSelectList { get; set; }
        internal void Setlevel02SelectList(IEnumerable<Level02DTO> enumerable)
        {
            level02DTOSelectList = CriaSelectList(enumerable);
        }

        public IEnumerable<KeyValuePair<string, string>> level03DTOSelectList { get; set; }
        internal void SetLevel03SelectList(IEnumerable<Level03DTO> enumerable)
        {
            level02DTOSelectList = CriaSelectList(enumerable);

        }

        public IEnumerable<KeyValuePair<string,string>> UnitsSelectList { get; set; }
        internal void SetUnitsSelectList(IEnumerable<ParCompanyDTO> enumerable)
        {
            UnitsSelectList = CriaSelectList(enumerable);

        }

        public IEnumerable<KeyValuePair<string, string>> ShiftSelectList { get; set; }
        internal void SetShiftSelectList(/*IEnumerable<ShiftDTO> enumerable*/)
        {
            //MOCK tabela fora de padrão 29 09 2016 celsogea.
            ShiftSelectList = new List<KeyValuePair<string, string>>(){
                                                    new KeyValuePair<string,string>("Shift A","1"),
                                                    new KeyValuePair<string,string>("Shift B","2"),
                                                };
        }

        public IEnumerable<KeyValuePair<string, string>> PeriodSelectList { get; set; }
        internal void SetPeriodSelectList(IEnumerable<PeriodDTO> enumerable)
        {
            PeriodSelectList = CriaSelectList(enumerable);

        }

        public IEnumerable<KeyValuePair<string, string>> UserSelectList { get; set; }
        internal void SetUserSelectList(IEnumerable<UserDTO> enumerable)
        {
            UserSelectList = CriaSelectList(enumerable);

        }

        private List<KeyValuePair<string, string>> CriaSelectList<T>(IEnumerable<T> enumerable)
        {
            List<KeyValuePair<string, string>> retorno = new List<KeyValuePair<string, string>>();
            var counter = 0;
            foreach (var i in enumerable)
            {
                var text = i.GetType().GetProperty("Name") ?? i.GetType().GetProperty("Description");
                var prop = i.GetType().GetProperty("Id");
                retorno.Insert(counter, new KeyValuePair<string, string>(text.GetValue(i).ToString(),prop.GetValue(i).ToString()));
                counter++;
            }

            return retorno;
        }

        #endregion

    }
}