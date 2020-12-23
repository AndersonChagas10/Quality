using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Enum
{
    public class Enums
    {
        public enum PDCAMenuItem
        {
            Plan = 1, Do, Check, Action
        }

        public enum ReportType
        {
            ItemMenu = 1, ReportXUserSgq, ComponenteGenerico
        }


        public static explicit operator int(Enums v)
        {
            throw new NotImplementedException();
        }
    }
}
