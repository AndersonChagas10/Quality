using DTO.BaseEntity;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Security.Cryptography;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace DTO.Helpers
{
    public static class Guard
    {
        public static String LANGUAGE_PT_BR = "pt-BR";
        public static String LANGUAGE_EN_US = "en-US";
        public static String LANGUAGE_DEFAULT = "default";

        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }

        public static DateTime PrimeiroDiaMesAnterior(DateTime data)
        {
            if (data.Month.Equals(1))
            {
                return new DateTime(data.Year - 1, data.AddMonths(-1).Month, 1);
            }
            return new DateTime(data.Year, data.AddMonths(-1).Month, 1);
        }

        public static string CheckStringFullSimple(string name)
        {
            var nameValidado = "";
            Guard.CheckStringFull(out nameValidado, "Name", name, "Property Name is invalid", true);
            return nameValidado;
        }

        public static DateTime Next(this DateTime from, DayOfWeek dayOfWeek)
        {
            int start = (int)from.DayOfWeek;
            int target = (int)dayOfWeek;
            if (target <= start)
                target += 7;
            return from.AddDays(target - start);
        }

        public static int GetUsuarioLogado_Id(HttpContextBase filterContext)
        {
            var userId = 0;
            HttpCookie cookie = filterContext.Request.Cookies.Get("webControlCookie");
            if (cookie != null)
                if (!string.IsNullOrEmpty(cookie.Values["userId"]))
                    int.TryParse(cookie.Values["userId"].ToString(), out userId);

            return userId;
        }

        

        //public static int GetUsuarioLogado_Id(HttpContextBase filterContext)
        //{
        //    var userId = 0;
        //    HttpCookie cookie = filterContext.Request.Cookies.Get("webControlCookie");
        //    if (cookie != null)
        //        if (!string.IsNullOrEmpty(cookie.Values["userId"]))
        //            int.TryParse(cookie.Values["userId"].ToString(), out userId);

        //    return userId;
        //}

        //public static int GetUsuarioLogado_Id(HttpContextBase filterContext)
        //{
        //    var userId = 0;
        //    HttpCookie cookie = filterContext.Request.Cookies.Get("webControlCookie");
        //    if (cookie != null)
        //        if (!string.IsNullOrEmpty(cookie.Values["userId"]))
        //            int.TryParse(cookie.Values["userId"].ToString(), out userId);

        //    return userId;
        //}

        #region Campo Calculado

        public static decimal ConverteValorCalculado(string valorString)
        {
            decimal number;
            decimal retorno;
            if (Decimal.TryParse(valorString, out number))
            {
                retorno = number;
            }
            else
            {
                valorString = valorString.ToUpper().Replace(',', '.');
                double v1 = double.Parse(valorString.Split('X')[0], CultureInfo.InvariantCulture);
                double v2 = double.Parse(valorString.Split('^')[1]);
                var resultado = v1 * Math.Pow(10, v2);
                retorno = Convert.ToDecimal(resultado, CultureInfo.InvariantCulture);

            }

            return retorno;
        }

        public static void ParseDateToSql(string date, ref DateTime _dtvalue)
        {
            if (GlobalConfig.Brasil)
                DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _dtvalue);
            else if (GlobalConfig.Eua)
                DateTime.TryParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _dtvalue);
        }

        public static DateTime ParseDateToSqlV2(string date)
        {
            if (string.IsNullOrEmpty(date))
                return DateTime.Now;

            var _dtvalue = DateTime.Now;
            //if (GlobalConfig.Brasil)
                DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _dtvalue);
            //else if (GlobalConfig.Eua)
            if(_dtvalue == DateTime.MinValue)
                DateTime.TryParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _dtvalue);

            return _dtvalue;
        }
      
        public static string ConverteValorCalculado(decimal valorDecimal)
        {

            if (valorDecimal == 0)
            {
                return "0x10^0";
            }

            int dezElevado = 0;
            if (valorDecimal > 0)//SE POSITIVO
            {
                if (valorDecimal > 10)//é maior que 10?
                {
                    while (valorDecimal > 10)
                    {
                        valorDecimal = valorDecimal / 10;
                        dezElevado++;
                    }
                }
                else if (valorDecimal < 1)//Esta entre 1 e 0?
                {
                    while (valorDecimal < 1)
                    {
                        valorDecimal = valorDecimal * 10;
                        dezElevado--;
                    }
                }
            }
            else//SE NEGATIVO
            {
                if (valorDecimal < -10)//É menor que 10?
                {
                    while (valorDecimal < -10)
                    {
                        valorDecimal = valorDecimal / 10;
                        dezElevado++;
                    }
                }
                else if (valorDecimal > -1)//Esta entre -1 e 0?
                {
                    while (valorDecimal > -1)
                    {
                        valorDecimal = valorDecimal * 10;
                        dezElevado--;
                    }
                }
            }

            var resultado = valorDecimal.ToString("G29") + "x10^" + dezElevado.ToString();

            return resultado;
        }

        public static string MesangemModelError(string fieldName, bool isSelecionado)
        {
            var selecionado = isSelecionado ? "selecionado" : "preenchido";
            return "O campo \"" + fieldName + "\" precisa ser " + selecionado + ".";
        }

        #endregion

        #region CONVERT

        public static bool ConverteValor<T>(this object valor, out T resultado, T valorPadrao, string paramiterName)
        {
            try
            {
                resultado = (T)Convert.ChangeType(valor, typeof(T));
                return true;
            }
            catch (Exception e)
            {
                resultado = valorPadrao;
                throw new ExceptionHelper("Impossível converter o valor: " + valor.ToString() + " Para:" + typeof(T) + " Nome do parametro: " + paramiterName, e);
            }
        }

        public static T ConverteValor<T>(this object valor)
        {
            return (T)Convert.ChangeType(valor, typeof(T));
        }

        public static T ConverteValor<T>(this object valor, T valorPadrao, string paramiterName)
        {
            try
            {
                return (T)Convert.ChangeType(valor, typeof(T));
            }
            catch (Exception e)
            {
                throw new ExceptionHelper("Impossível converter o valor: " + valor.ToString() + " Para:" + typeof(T) + " Nome do parametro: " + paramiterName, e);
                //return valorPadrao;
            }
        }

        public static T ConverteValor<T>(this object valor, string paramiterName)
        {
            try
            {
                return (T)Convert.ChangeType(valor, typeof(T));
            }
            catch (Exception e)
            {
                throw new ExceptionHelper("Impossível converter o valor: " + valor.ToString() + " Para: " + typeof(T) + " Nome do parametro: " + paramiterName, e);
                //return valorPadrao;
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

        public static void CheckListNullOrEmpty<T>(List<T> result, string messsage)
        {
            if (result.IsNull())
                throw new ExceptionHelper(messsage);

            if (result.Count == 0)
                throw new ExceptionHelper(messsage);
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

        /// <summary>
        /// Verifica property , se ela for data default ou nula, coloca data atual.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        public static void verifyDate<T>(T obj, string property)
        {
            try
            {
                if (obj.GetType().GetProperty(property) != null)
                {
                    var date = (DateTime)obj.GetType().GetProperty(property).GetValue(obj, null);
                    if (date.IsNull())
                    {
                        obj.GetType().GetProperty(property).SetValue(obj, DateTime.Now);
                    }
                    else
                    {
                        if (date == DateTime.MinValue)
                            obj.GetType().GetProperty(property).SetValue(obj, DateTime.Now);
                    }
                }
            }
            catch (Exception)
            {
                obj.GetType().GetProperty(property).SetValue(obj, DateTime.Now);
            }
        }


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

        #region Criptografia 3DES


        /// <summary>     
        /// Representação de valor em base 64 (Chave Interna)    
        /// O Valor representa a transformação para base64 de     
        /// um conjunto de 32 caracteres (8 * 32 = 256bits)      
        /// </summary>     
        const string cryptoKey3DES = "90A4F2C1DC40CE1F";

        public static string Criptografar3DES(string Message)
        {
            try
            {
                byte[] Results = null;
                System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
                MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
                byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(cryptoKey3DES));
                TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
                TDESAlgorithm.Key = TDESKey;
                TDESAlgorithm.Mode = CipherMode.ECB;
                TDESAlgorithm.Padding = PaddingMode.PKCS7;
                byte[] DataToEncrypt = UTF8.GetBytes(Message);
                try
                {
                    ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                    Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
                }
                finally
                {
                    TDESAlgorithm.Clear();
                    HashProvider.Clear();
                }
                return Convert.ToBase64String(Results);
            }
            catch (Exception e)
            {
                new CreateLog(new Exception("Erro no metodo Guard.Criptografar3DES", e));
                return Message;
            }
        }

        public static string Descriptografar3DES(string Message)
        {
            try
            {
                byte[] Results = null;
                System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
                MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
                byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(cryptoKey3DES));
                TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
                TDESAlgorithm.Key = TDESKey;
                TDESAlgorithm.Mode = CipherMode.ECB;
                TDESAlgorithm.Padding = PaddingMode.PKCS7;
                byte[] DataToDecrypt = Convert.FromBase64String(Message);
                try
                {
                    ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                    Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
                }
                finally
                {
                    TDESAlgorithm.Clear();
                    HashProvider.Clear();
                }
                return UTF8.GetString(Results);
            }
            catch (Exception e)
            {
                new CreateLog(new Exception("Erro no metodo Guard.Descriptografar3DES", e));
                return Message;
            }
        }

        #endregion

        public static T DeserializaJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string ToJson(object value)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return JsonConvert.SerializeObject(value, Formatting.Indented, settings);
        }

        public static List<SelectListItem> CreateDropDownList<T>(IEnumerable<T> enumerable)
        {
            List<SelectListItem> retorno = new List<SelectListItem>();
            retorno.Insert(0, new SelectListItem() { Text = "Selecione...", Value = "-1" });
            var counter = 1;
            foreach (var i in enumerable)
            {
                var text = i.GetType().GetProperty("Name") ?? i.GetType().GetProperty("Description");
                var prop = i.GetType().GetProperty("Id");
                var defaultSelected = i.GetType().GetProperty("IsDefaultOption");
                var selectItem = new SelectListItem() { Text = text.GetValue(i).ToString(), Value = prop.GetValue(i).ToString() };
                if (defaultSelected != null && (bool)defaultSelected.GetValue(i))
                    selectItem.Selected = true;
                retorno.Insert(counter, selectItem);
                counter++;
            }

            return retorno;
        }

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
                if (ids[0].Id == 0)
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
                throw new ExceptionHelper(propName + " não pode menor que " + propName2 + "!");
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
        public static void CheckStringFull(out string retorno, string propName, string value, string mensagem = null, bool requerido = false
                                                 , bool DuplicateWhiteEspace = false, bool WhiteSpacesStart = false, bool WhiteSpacesEnd = false)
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
            else
            {
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

        public static bool InsideFrequency(DateTime currentDate, int Frequency_Id)
        {
            DateTime startDate = DateTime.Now.Date;
            DateTime endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 23, 59, 59).AddMonths(1).AddDays(-1);

            switch (Frequency_Id)
            {
                case 1: //Por Período
                    if (currentDate >= startDate && currentDate <= endDate)
                    {
                        return true;
                    }
                    break;
                case 2: //Por Turno
                    if (currentDate >= startDate && currentDate <= endDate)
                    {
                        return true;
                    }
                    break;
                case 3: //Diário
                    if (currentDate >= startDate && currentDate <= endDate)
                    {
                        return true;
                    }
                    break;
                case 4: //Semanal
                    startDate = startDate.AddDays(-(int)startDate.DayOfWeek);
                    endDate = startDate.AddDays(6);  //new DateTime(DateTime.Now.Year, DateTime.Now.Month, startDate.Day + 6, 23, 59, 59);
                    if (currentDate >= firstDayOfMonth && currentDate <= lastDayOfMonth)
                    {
                        return true;
                    }
                    break;
                case 5: //Quinzenal
                    if (currentDate.Day <= 15)
                    {
                        startDate = firstDayOfMonth;
                        endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 15, 23, 59, 59);
                    }
                    else
                    {
                        startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 16);
                        endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, lastDayOfMonth.Day, 23, 59, 59);
                    }
                    if (currentDate >= firstDayOfMonth && currentDate <= lastDayOfMonth)
                    {
                        return true;
                    }
                    break;
                case 6: //Mensal
                    if (currentDate >= firstDayOfMonth && currentDate <= lastDayOfMonth)
                    {
                        return true;
                    }
                    break;
                default:
                    throw new ExceptionHelper(Frequency_Id + " Invalid Frequency.");
            }
            return false;
        }

    }
}