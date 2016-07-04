using System;
using System.Collections.Generic;
using System.Globalization;

namespace SgqSystem.ViewModels
{
    public class GenericReturnViewModel<T>
    {
        public string MensagemErro { get; set; }
        public string MensagemSucesso { get; set; }
        public string MensagemAlerta { get; set; }
        public string MensagemExcecao { get; set; }
        public T Retorno { get; set; }
        public List<T> ListRetorno { get; set; }
        public bool ReturnisBool { get; set; }

        //private DateTime _dtvalueInicio, _dtvalueFim;

        //public string dataInicio { get; set; }

        //public string dataFim { get; set; }

        //public DateTime _dataInicio
        //{
        //    get
        //    {
        //        if (dataInicio != null)
        //        {
        //            this._dtvalueInicio = DateTime.ParseExact(dataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //            return _dtvalueInicio;
        //        }
        //        return DateTime.Now;
        //    }
        //}

        //public DateTime _dataFim
        //{
        //    get
        //    {
        //        if (dataFim != null)
        //        {
        //            DateTime.TryParse(dataFim, out _dtvalueFim);
        //            return _dtvalueFim;
        //        }
        //        return DateTime.Now;
        //    }
        //}

    }
}
