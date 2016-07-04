using Dominio.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Dominio.Helpers
{
    public static class ExtensoesComuns
    {

        #region CONVERT

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

        #endregion

        #region AS

        public static string AsString(this object valor)
        {
            try
            {
                return Convert.ToString(valor);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string AsString(this object valor, string valorPadrao = "")
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
            try
            {
                return Convert.ToDateTime(valor);
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
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

        #endregion

        #region IS

        public static bool IsNull(this object value)
        {
            try
            {
                if (value == null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsNotNull(this object value)
        {
            try
            {
                if (value != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region Data

        public static DateTime ConverteStringPateParaDateTime(string dataString)
        {
            return DateTime.ParseExact(dataString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        #endregion

        public static string RetornaInsersaoRegistro<T>(List<T> ids) where T : EntityBase
        {
            var acao = "inseridos";
            if (ids.Count > 0)
            {
                return ids.Count + " Registros " + acao + " com sucesso!";
            }
            else
                if(ids[0].Id == 0)
                    return "Registro inserido com sucesso!";
                else
                    return "Registro alterado com sucesso!";


        }
    }
}