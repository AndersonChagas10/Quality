using DTO.BaseEntity;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace DTO.Helpers
{
    public static class Guard
    {

        #region CONVERT

        public static bool ConverteValor<T>(this object valor, out T resultado, T valorPadrao)
        {
            try
            {
                resultado = (T)Convert.ChangeType(valor, typeof(T));
                return true;
            }
            catch (ExceptionHelper)
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
            catch (ExceptionHelper)
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
            catch (ExceptionHelper)
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
            catch (ExceptionHelper)
            {
                return valorPadrao;
            }
        }

        public static bool VerifyStringNullValue(string verify)
        {
            return !(verify != null && (verify.Equals("null") || verify.Equals("undefined")));
        }

        public static DateTime AsDateTime(this object valor)
        {
            try
            {
                return Convert.ToDateTime(valor);
            }
            catch (ExceptionHelper)
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
            catch (ExceptionHelper)
            {
                return valorPadrao;
            }
        }

        public static void VerifyIfIsBool(bool isIAmBool, string propName)
        {
            if (isIAmBool.GetType() != typeof(bool))
                throw new Exception("The property: " + propName + " is not a bool.");

        }

        public static DateTime AsDateTime(this object valor, DateTime valorPadrao, IFormatProvider formato)
        {
            try
            {
                return Convert.ToDateTime(valor, formato);
            }
            catch (ExceptionHelper)
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
            catch (ExceptionHelper)
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
            catch (ExceptionHelper)
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


        /// <summary>
        /// Valida Data 
        /// </summary>
        /// <param name="data">Data a ser validada</param>
        /// <returns></returns>
        public static string dataValidar(string data)
        {
            //Verifica se a data é null.
            if (string.IsNullOrEmpty(data))
            {
                //Caso for Null, retorna Null.
                return null;
            }
            //Se a data não tiver o tamanho mínimo
            if (data.Length != 10)
            {
                //Informe que a data não foi preenchida corretamente.
                return "Data não foi preenchida corretamente";
            }
            try
            {
                //Converta a String  data em um DateTime.
                DateTime dataSelecionada = Convert.ToDateTime(data);
                //Retorna Null caso a conversão esteja correta.
                return null;
            }
            catch
            {
                //Retorna Data Inválida.
                return "Data " + data + " Invalida ";
            }
        }

        #endregion

        /// <summary>
        /// Não pode ser Zero.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="msg"></param>
        public static void forValueZero(int value, string msg)
        {
            if (value == 0)
                throw new ExceptionHelper(msg);
        }

        public static void forValueZeroPredefinedMessage(int value, string paramName)
        {
            if (value == 0)
                throw new ExceptionHelper(paramName + " Must be different of Zero.");
        }

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

        public static void ForNullOrEmpty(string value, string mensagemErro)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ExceptionHelper(mensagemErro);
            }
        }


        /// <summary>
        /// Não pode ser negativo, se alteração não pdoe ser Zero.
        /// Throw message "Invalid key for: paramName  in:  className"
        /// </summary>
        /// <param name="id"></param>
        /// <param name="paramName"></param>
        /// <param name="className"></param>
        /// <param name="isAlter"></param>
        public static void ForValidId(int id, string callerMethod)
        {
            if (!(id >= 0))
                throw new ExceptionHelper("Invalid primary key detected in: " + callerMethod);
        }

        /// <summary>
        ///  Se nulo utilizar a data atual.
        /// </summary>
        /// <param name="date"></param>
        public static void AutoFillDateWithDateNow(DateTime? date)
        {
            if (date.IsNull())
                date = DateTime.Now;
        }


        public static void ForValidFk(string propName, int id)
        {
            ForValidFk(id, propName + "Id Inválido!");
        }

        public static void ForValidFk(int id, string mensagemErro)
        {
            if (!(id > 0))
                throw new ExceptionHelper(mensagemErro);
        }

        /// <summary>
        /// Não pode ser negativo.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="propName"></param>
        public static void ForNegative(int number, string propName)
        {
            if (number < 0)
                throw new ExceptionHelper(propName + " Should be positive number.!");
        }

        public static void ForNegative(decimal number, string propName)
        {
            if (number < 0)
                throw new ExceptionHelper(propName + " Should be positive number.!");
        }

        public static void ForNegative(double number, string propName)
        {
            if (number < 0)
                throw new ExceptionHelper(propName + " Should be positive number.!");
        }

        public static void ForNegative(float number, string propName)
        {
            if (number < 0)
                throw new ExceptionHelper(propName + " Should be positive number.!");
        }

        public static void ForMaiorQuer(int number, int number2, string propName, string propName2)
        {
            if (number < number2)
                throw new ExceptionHelper(propName + " não pode menor que "+ propName2 + "!");
        }

        public static void ForMaiorQuer(decimal number, decimal number2, string propName, string propName2)
        {
            if (number < number2)
                throw new ExceptionHelper(propName + " não pode menor que " + propName2 + "!");
        }

        public static void ForMaiorQuer(double number, double number2, string propName, string propName2)
        {
            if (number < number2)
                throw new ExceptionHelper(propName + " não pode menor que " + propName2 + "!");
        }

        public static void ForMaiorQuer(float number, float number2, string propName, string propName2)
        {
            if (number < number2)
                throw new ExceptionHelper(propName + " não pode menor que " + propName2 + "!");

        }


        /// <summary>
        /// Valida string nula, string de injection Jquery, string como "undefined" no value. Como padrão remove todos os espaços no começo e fim da string, e espaços duplicados no meio da string.
        /// </summary>
        /// <param name="retorno"> Retorno. </param>
        /// <param name="value"> String a ser validada. </param>
        /// <param name="mensagem"> Mensagem de retorno opcional. </param>
        /// <param name="requerido"> Se o campo valor da property é obrigatório. </param>
        /// <param name="DuplicateWhiteEspace"> Remove espaços duplicados entre a string. </param>
        /// <param name="WhiteSpacesStart"> Remove espaços no inicio da string. </param>
        /// <param name="WhiteSpacesEnd"> Remove espaços no fim da string.</param>
        public static void NullOrEmptyValuesCheck(out string retorno, string propName, string value, string mensagem = null, bool requerido = false
                                                 ,bool DuplicateWhiteEspace = false, bool WhiteSpacesStart = false, bool WhiteSpacesEnd = false)
        {
            //Se o valor da STRING é NULL e for Obrigatório.
            if (string.IsNullOrEmpty(value) && requerido == true)
                throw new ExceptionHelper(mensagem ?? propName + " não pode ser vazia.");

            if (!string.IsNullOrEmpty(value))
            {
                //Converte a STRING para minuscula para facilitar a comparacao.
                string compareValue = value.ToLower();

                //Compara a STRING com os possíveis erros de parametros passados através do JAVASCRIPT.
                if (compareValue == "undefined" || compareValue == "null" || compareValue == "[object object]")
                {
                    throw new ExceptionHelper("Formato do campo " + propName + " inválido.");
                }
                
                //Caso a string contenha valor, trata espaços em branco.
                RemoveEspacosEmString(value, DuplicateWhiteEspace, WhiteSpacesStart, WhiteSpacesEnd);
            }
            else {
                //Caso valor em branco "" retorna nulo para definição do padrão Data Base.
                value = null;
            }
            retorno = value;

        }

        /// <summary>
        /// Trata espaços duplicados em uma string.
        /// </summary>
        /// <param name="value"> String a ser tratada. </param>
        /// <param name="DuplicateWhiteEspace"> Remove espaços duplicados entre a string. </param>
        /// <param name="WhiteSpacesStart"> Remove espaços no inicio da string. </param>
        /// <param name="WhiteSpacesEnd"> Remove espaços no fim da string.</param>
        /// <returns></returns>
        public static string RemoveEspacosEmString(string value, bool DuplicateWhiteEspace = false, bool WhiteSpacesStart = false, bool WhiteSpacesEnd = false)
        {
            //Se não permitir espaço em branco duplicado.
            if (DuplicateWhiteEspace == false)
            {
                //Enquanto houver espaço duplicado.
                while (value.Contains("  "))
                {
                    //Retira espaços duplicado.
                    value = value.Replace("  ", " ");
                }
            }

            //Se não permite espaços em branco no inicio e no fim.
            if (WhiteSpacesStart == false && WhiteSpacesEnd == false)
            {
                // Remove espaços do inicio e do fim.
                value = value.Trim();
            }
            else
            {
                //Se não aceita espaços no começo.
                if (WhiteSpacesStart == false)
                {
                    //Remove espaço inicial da STRING.
                    value = value.TrimStart();
                }
                //Se não aceita espaços no fim.
                if (WhiteSpacesEnd == false)
                {
                    //Remove o espaço no final da STRING.
                    value = value.TrimEnd();
                }
            }
            //Retorna o valor.
            return value;
        }

        /// <summary>
        /// Verifica se existe espaços em uma string, caso sim dispara uma exceção com uma mensagem.
        /// </summary>
        /// <param name="value"> String a ser verificada. </param>
        /// <param name="message"> Mensagem a ser devolvida como throw. </param>
        public static void VerificaEspacoString(string value, string message)
        {
            if (string.IsNullOrWhiteSpace(value) && value.Length > 0)
                throw new ExceptionHelper(message);

        }


    }
}