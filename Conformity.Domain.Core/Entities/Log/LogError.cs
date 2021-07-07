using Conformity.Domain.Core.Enums.Log;
using Conformity.Domain.Core.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conformity.Domain.Core.Entities.Log
{
    [Table("LogError")]
    public class LogError
    {
        public int Id { get; set; }
        public string ErrorMessage { get; set; }
        public int Line { get; set; }
        public string Method { get; set; }
        public string Controller { get; set; }

        private string _object;

        public string Object
        {
            get { return _object?.Substring(0, _object.Length > 900 ? 900 : _object.Length); }
            set { _object = value; }
        }

        private string _stackTrace;

        public string StackTrace
        {
            get { return _stackTrace.Substring(0, _stackTrace.Length > 900 ? 900 : _stackTrace.Length); }
            set { _stackTrace = value; }
        }

        public DateTime AddDate { get; set; }
    }
}
