using System;

namespace PA.DTO.Ferramentas
{
    public static class ExtensoesComuns
    {

        public static bool IsNotNull(this object value)
        {
            if (value != null)
            {
                return true;
            }
            return false;
        }

        public static bool IsNull(this object value)
        {
            if (value == null)
            {
                return true;
            }
            return false;
        }

        public static bool IsNullOrEmptyOrMaxLength(this string value, int maxLength)
        {
            if (value.IsNull())
            {
                return true;
            }
            if (value.Length == 0)
            {
                return true;
            }
            if (value.Length > maxLength)
            {
                return true;
            }
            return false;
        }

        public static bool IsNotNullOrMaxLength(this string value, int maxLength)
        {
            if (value.IsNull())
            {
                return false;
            }
            if (value.Length > maxLength)
            {
                return true;
            }
            return false;
        }

        public static bool IsNotNullOrMinLength(this string value, int minLength)
        {
            if (value.IsNull())
            {
                return false;
            }

            if (value.Length < minLength)
            {
                return true;
            }
            return false;
        }

        public static bool IsNullOrEmptyOrMinLength(this string value, int minLength)
        {
            if (value.IsNull())
            {
                return true;
            }
            if (value.Length == 0)
            {
                return true;
            }
            if (value.Length < minLength)
            {
                return true;
            }
            return false;
        }

        public static T IfTernary<T>(this T valor, T valorPadrao)
        {
            try
            {
                return valor != null ? valor : valorPadrao;
            }
            catch (Exception)
            {
                return valorPadrao;
            }
        }

        public static bool ConverteValor<T>(this object valor, out T resultado, T valorPadrao)
        {
            try
            {
                resultado = (T)Convert.ChangeType(valor, typeof(T));
                return true;
            }
            catch (Exception)
            {
                resultado = valorPadrao;
                return false;
            }

        }

        public static T ConverteValor<T>(this object valor)
        {
            return (T)Convert.ChangeType(valor, typeof(T));
        }

        public static T ConverteValor<T>(this object valor, T valorPadrao)
        {
            try
            {
                return (T)Convert.ChangeType(valor, typeof(T));
            }
            catch (Exception)
            {
                return valorPadrao;
            }
        }

        public static string AsString(this object valor)
        {
            return Convert.ToString(valor);
        }

        public static string AsString(this object valor, string valorPadrao)
        {
            try
            {
                return Convert.ToString(valor);
            }
            catch (Exception)
            {
                return valorPadrao;
            }
        }

        public static DateTime AsDateTime(this object valor)
        {
            return Convert.ToDateTime(valor);
        }

        public static DateTime AsDateTime(this object valor, DateTime valorPadrao)
        {
            try
            {
                return Convert.ToDateTime(valor);
            }
            catch (Exception)
            {
                return valorPadrao;
            }
        }

        public static DateTime AsDateTime(this object valor, IFormatProvider formato)
        {
            return Convert.ToDateTime(valor, formato);
        }

        public static DateTime AsDateTime(this object valor, DateTime valorPadrao, IFormatProvider formato)
        {
            try
            {
                return Convert.ToDateTime(valor, formato);
            }
            catch (Exception)
            {
                return valorPadrao;
            }
        }

        public static int AsInt32(this object valor)
        {
            return Convert.ToInt32(valor);
        }

        public static int AsInt32(this object valor, int valorPadrao)
        {
            try
            {
                return Convert.ToInt32(valor);
            }
            catch (Exception)
            {
                return valorPadrao;
            }
        }

    }

    //public class Utilidades
    //{

    //    public List<DominioBaseDTO> GetAllEnumDominioBaseDTO<T>() where T : struct, IConvertible
    //    {
    //        var camposEnum = Enum.GetNames(typeof(T));
    //        List<DominioBaseDTO> list = new List<DominioBaseDTO>();

    //        foreach (var item in camposEnum)
    //        {
    //            T value = (T)Enum.Parse(typeof(T), item);
    //            string descricao;
    //            int valor = Convert.ToInt32(value);

    //            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
    //            DescriptionAttribute[] attributes =
    //             (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

    //            if (attributes != null &&
    //             attributes.Length > 0)
    //                descricao = attributes[0].Description;
    //            else
    //                descricao = value.ToString();

    //            list.Add(new DominioBaseDTO
    //            {
    //                Codigo = valor,
    //                Descricao = descricao
    //            });

    //        }

    //        return list;
    //    }

    //    public List<SelectListItem> GetAllEnumSelectListItem<T>() where T : struct, IConvertible
    //    {
    //        var camposEnum = Enum.GetNames(typeof(T));
    //        List<SelectListItem> list = new List<SelectListItem>();

    //        foreach (var item in camposEnum)
    //        {
    //            T value = (T)Enum.Parse(typeof(T), item);
    //            string descricao;
    //            string valor = value.ToString().Replace("Valor", "");

    //            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
    //            DescriptionAttribute[] attributes =
    //             (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

    //            if (attributes != null &&
    //             attributes.Length > 0)
    //                descricao = attributes[0].Description;
    //            else
    //                descricao = value.ToString();

    //            list.Add(new SelectListItem
    //            {
    //                Value = valor.ToString(),
    //                Text = descricao
    //            });

    //        }

    //        return list;
    //    }


    //    public SelectList PopulaDropDownList<T>(List<T> Items, string idValue, string textValue)
    //    {
    //        return new SelectList(Items, idValue, textValue);
    //    }

    //}
}
