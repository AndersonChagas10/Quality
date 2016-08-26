using AutoMapper;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using System;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Dominio.Services
{
    public class UserDomain : IUserDomain
    {
        private readonly IUserRepository _userRepo;

        public string falhaGeral { get { return "It was not possible retrieve any data."; } }

        /// <summary>
        /// Construtor para Inversion of Control.
        /// </summary>
        /// <param name="userRepo"> Repositório de Usuario, interface de comunicação com Data. </param>
        public UserDomain(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        /// <summary>
        /// VErifica se existe Usuario e Senha Correspondentes no Banco de dados.
        /// </summary>
        /// <param name="name"> Nome do Usuário. </param>
        /// <param name="password"> Senha do Usuário. </param>
        /// <returns> Retorna o Usuário caso exista, caso não exista retorna exceção com uma mensagem</returns>
        public GenericReturn<UserDTO> AuthenticationLogin(UserDTO userDto)
        {
            try
            {
                //var t1 = CriptografarAES("eduardomaia");
                //var t2 = Criptografar3DES("eduardomaia");
                //var t3 = Encrypt("eduardomaia", true);
                //var t4 = Encrypt("eduardomaia", false);

                if (userDto.IsNull())
                    throw new ExceptionHelper("Username and Password are required.");

                userDto.ValidaObjetoUserDTO(); //Valida Properties do objeto para gravar no banco.

                var user = Mapper.Map<UserDTO, UserSgq>(userDto);

                //Demonstração Funcionando
                //  CheckUserInAD("servidoradteste.com", "Administrador", "1qaz!QAZ", "eduardo.maia");
                var boolResultadoAd = CheckUserInAD("servidoradteste.com", "celso.gea", "1qaz!QAZ");

                var isUser = _userRepo.AuthenticationLogin(user);

                if (!isUser.IsNotNull())
                    throw new ExceptionHelper("User not found, please verify Username and Password.");

                var retorno = Mapper.Map<UserSgq, UserDTO>(isUser);
                return new GenericReturn<UserDTO>(retorno);
            }
            catch (Exception e)
            {
                return new GenericReturn<UserDTO>(e, falhaGeral);
            }
        }

        public GenericReturn<UserDTO> GetByName(string username)
        {
            try
            {
                var queryResult = _userRepo.GetByName(username);
                return new GenericReturn<UserDTO>(Mapper.Map<UserSgq, UserDTO>(queryResult));
            }
            catch (Exception e)
            {
                return new GenericReturn<UserDTO>(e, "CAnnot get user by name.");
            }
        }

        public static bool CheckUserInAD(string domain, string username, string password, string userVerific)
        {
            try
            {
                using (var domainContext = new PrincipalContext(ContextType.Domain, domain, username, password))
                {
                    using (var user = new UserPrincipal(domainContext))
                    {
                        user.SamAccountName = userVerific;

                        using (var pS = new PrincipalSearcher())
                        {
                            pS.QueryFilter = user;

                            using (PrincipalSearchResult<Principal> results = pS.FindAll())
                            {
                                if (results != null && results.Count() > 0)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CheckUserInAD(string domain, string username, string password)
        {
            try
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain))
                {
                    var userValid = pc.ValidateCredentials(username, password);
                    return userValid;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }




        #region Testes

        #region Constantes para Criptografar

        /// <summary>     
        /// Representação de valor em base 64 (Chave Interna)    
        /// O Valor representa a transformação para base64 de     
        /// um conjunto de 32 caracteres (8 * 32 = 256bits)      
        /// </summary>     
        const string cryptoKey = "90A4F2C1DC40CE1F";

        /// <summary>     
        /// Vetor de bytes utilizados para a criptografia (Chave Externa)     
        /// </summary>     
        static byte[] bIV = { 0, 0, 0, 0, 0, 0, 0, 0 };
        //{ 0x50, 0x08, 0xF1, 0xDD, 0xDE, 0x3C, 0xF2, 0x18,
        //0x44, 0x74, 0x19, 0x2C, 0x53, 0x49, 0xAB, 0xBC };

        #endregion

        #region Criptografia 3DES

        public static string Criptografar3DES(string Message)
        {
            byte[] Results = null;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(cryptoKey));
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

        public static string Descriptografar3DES(string Message)
        {
            byte[] Results = null;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(cryptoKey));
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

        #endregion

        #region Criptografia AES

        /// <summary>     
        /// Metodo de criptografia de valor     
        /// </summary>     
        /// <param name="text">valor a ser criptografado</param>     
        /// <returns>valor criptografado</returns>
        public static string CriptografarAES(string text)
        {
            try
            {
                // Se a string não está vazia, executa a criptografia
                if (!string.IsNullOrEmpty(text))
                {
                    // Cria instancias de vetores de bytes com as chaves                
                    byte[] bKey = Convert.FromBase64String(cryptoKey);
                    byte[] bText = new UTF8Encoding().GetBytes(text);

                    // Instancia a classe de criptografia Rijndael
                    Rijndael rijndael = new RijndaelManaged();

                    // Define o tamanho da chave "256 = 8 * 32"                
                    // Lembre-se: chaves possíves:                
                    // 128 (16 caracteres), 192 (24 caracteres) e 256 (32 caracteres)                
                    rijndael.KeySize = 256;

                    // Cria o espaço de memória para guardar o valor criptografado:                
                    MemoryStream mStream = new MemoryStream();
                    // Instancia o encriptador                 
                    CryptoStream encryptor = new CryptoStream(
                        mStream,
                        rijndael.CreateEncryptor(),// rijndael.CreateEncryptor(bKey, bIV),
                        CryptoStreamMode.Write);

                    // Faz a escrita dos dados criptografados no espaço de memória
                    encryptor.Write(bText, 0, bText.Length);
                    // Despeja toda a memória.                
                    encryptor.FlushFinalBlock();
                    // Pega o vetor de bytes da memória e gera a string criptografada                
                    return Convert.ToBase64String(mStream.ToArray());
                }
                else
                {
                    // Se a string for vazia retorna nulo                
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Se algum erro ocorrer, dispara a exceção            
                throw new ApplicationException("Erro ao criptografar", ex);
            }
        }

        /// <summary>     
        /// Pega um valor previamente criptografado e retorna o valor inicial 
        /// </summary>     
        /// <param name="text">texto criptografado</param>     
        /// <returns>valor descriptografado</returns>     
        public static string DescriptografarAES(string text)
        {
            try
            {
                // Se a string não está vazia, executa a criptografia           
                if (!string.IsNullOrEmpty(text))
                {
                    // Cria instancias de vetores de bytes com as chaves                
                    byte[] bKey = Convert.FromBase64String(cryptoKey);
                    byte[] bText = Convert.FromBase64String(text);

                    // Instancia a classe de criptografia Rijndael                
                    Rijndael rijndael = new RijndaelManaged();

                    // Define o tamanho da chave "256 = 8 * 32"                
                    // Lembre-se: chaves possíves:                
                    // 128 (16 caracteres), 192 (24 caracteres) e 256 (32 caracteres)                
                    rijndael.KeySize = 256;

                    // Cria o espaço de memória para guardar o valor DEScriptografado:               
                    MemoryStream mStream = new MemoryStream();

                    // Instancia o Decriptador                 
                    CryptoStream decryptor = new CryptoStream(
                        mStream,
                        rijndael.CreateDecryptor(bKey, bIV),
                        CryptoStreamMode.Write);

                    // Faz a escrita dos dados criptografados no espaço de memória   
                    decryptor.Write(bText, 0, bText.Length);
                    // Despeja toda a memória.                
                    decryptor.FlushFinalBlock();
                    // Instancia a classe de codificação para que a string venha de forma correta         
                    UTF8Encoding utf8 = new UTF8Encoding();
                    // Com o vetor de bytes da memória, gera a string descritografada em UTF8       
                    return utf8.GetString(mStream.ToArray());
                }
                else
                {
                    // Se a string for vazia retorna nulo                
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Se algum erro ocorrer, dispara a exceção            
                throw new ApplicationException("Erro ao descriptografar", ex);
            }
        }

        #endregion

        private static String getHexStringFromArray(byte[] arr)
        {
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < arr.Length; i++)
            {
                sBuilder.Append(arr[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(cryptoKey.Substring(0, 16)));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(cryptoKey.Substring(0, 16));

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            tdes.IV = bIV;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            tdes.Clear();

            var aaa = getHexStringFromArray(resultArray);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        #endregion

        public bool AuthenticationLoginAd(UserDTO userDto)
        {
            return CheckUserInAD("global.corp.prod", userDto.Name, userDto.Password);
        }

    }


}
