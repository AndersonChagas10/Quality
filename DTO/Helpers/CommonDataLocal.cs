using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DTO.Helpers
{
    public static class CommonDataLocal
    {
        public static DictionaryEntry getResource(string value)
        {
            var resourceManager = (IDictionary<string, object>)Resources.Resource;

            foreach (var r in resourceManager)
            {
                if (r.Key.ToString() == value)
                    return new DictionaryEntry() { Key = r.Key, Value = r.Value };
            }

            return new DictionaryEntry();
        }
    }
}
