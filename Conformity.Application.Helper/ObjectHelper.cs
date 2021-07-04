using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conformity.Application.Util
{
    public class ObjectHelper
    {
        public static T Clone<T>(T obj)
        {
            var newObject = Activator.CreateInstance<T>();
            var fields = newObject.GetType().GetProperties();
            foreach (var field in fields)
            {
                var value = field.GetValue(obj);
                field.SetValue(newObject, value);
            }
            return newObject;
        }
    }
}
