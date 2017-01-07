using AutoMapper;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using DTO;
using DTO.DTO;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Dominio.Services
{
    public class UserDomain : IUserDomain
    {
        #region Parametros e construtores.

        private readonly IUserRepository _userRepo;
        private readonly IBaseRepository<ParCompanyXUserSgq> _baseParCompanyXUserSgq;
        private readonly IBaseRepository<ParCompany> _baseParCompany;

        private static string dominio = "global.corp.prod";

        public string falhaGeral { get { return "It was not possible retrieve any data."; } }

        /// <summary>
        /// Construtor para Inversion of Control.
        /// </summary>
        /// <param name="userRepo"> Repositório de Usuario, interface de comunicação com Data. </param>
        public UserDomain(IUserRepository userRepo
            , IBaseRepository<ParCompanyXUserSgq> baseParCompanyXUserSgq
            , IBaseRepository<ParCompany> baseParCompany)
        {
            _baseParCompany = baseParCompany;
            _baseParCompanyXUserSgq = baseParCompanyXUserSgq;
            _userRepo = userRepo;
        }

        #endregion

        #region AuthenticationLogin

        /// <summary>
        /// Verifica se existe Usuario e Senha Correspondentes no Banco de dados.
        /// </summary>
        /// <param name="name"> Nome do Usuário. </param>
        /// <param name="password"> Senha do Usuário. </param>
        /// <returns> Retorna o Usuário caso exista, caso não exista retorna exceção com uma mensagem</returns>
        public GenericReturn<UserDTO> AuthenticationLogin(UserDTO userDto)
        {
            try
            {

                UserSgq userByName;

                if (userDto.IsNull())
                    throw new ExceptionHelper("Username and Password are required.");

                try
                {

                    userDto.Password = DecryptStringAES(userDto.Password);
                    userDto.ValidaObjetoUserDTO(); //Valida Properties do objeto para gravar no banco.
                    userByName = _userRepo.GetByName(userDto.Name);

                }
                catch (Exception e)
                {
                    throw new Exception("Erro inesperado ao validar objeto e buscar user pelo nome SGq Global.", e);
                }

                if (GlobalConfig.Brasil)
                    LoginBraSil(userDto, userByName);

                if (GlobalConfig.Eua)
                {
                    if (userByName == null)
                        throw new ExceptionHelper("User not found, please verify Username and Password.");

                    AutenticaAdEUA(userDto);//Autenticação no AD JBS USA
                }

                try
                {
                    userByName = _userRepo.GetByName(userDto.Name);
                    userByName.Password = Guard.Criptografar3DES(userDto.Password);
                    _userRepo.Salvar(userByName);
                    return new GenericReturn<UserDTO>(Mapper.Map<UserSgq, UserDTO>(userByName));
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao tentar atualizar o usuario no post back da autenticação.", e);
                }

            }
            catch (Exception e)
            {
                new CreateLog(e);
                return new GenericReturn<UserDTO>(e, falhaGeral + e.Message);
            }


        }

        private void AutenticaAdEUA(UserDTO userDto)
        {
            //Autenticação no AD JBS USA
            if (!CheckUserInAD(dominio, userDto.Name, userDto.Password))
            {
                var user = Mapper.Map<UserDTO, UserSgq>(userDto);
                user.Password = Guard.Criptografar3DES(user.Password);
                var isUser = _userRepo.AuthenticationLogin(user);
                if (isUser.IsNull())
                    throw new ExceptionHelper("User not found, please verify Username and Password.");

                //userByName.Password = Guard.Criptografar3DES(userDto.Password);
                //_userRepo.Salvar(userByName);
                //return new GenericReturn<UserDTO>(Mapper.Map<UserSgq, UserDTO>(userByName));
            }
        }

        private void LoginBraSil(UserDTO userDto, UserSgq userByName)
        {
            try
            {
                if (userByName == null)//Não existe no nosso DB
                {
                    CriaUSerSgqPeloUserSgqBR(userDto);
                }

            }
            catch (Exception e)
            {
                throw new Exception("Erro ao criar o usuario Sgq Global a partir do ERP Sgq Brasil", e);
            }

            try
            {
                var user = Mapper.Map<UserDTO, UserSgq>(userDto);
                user.Password = Guard.Criptografar3DES(user.Password);
                var isUser = _userRepo.AuthenticationLogin(user);
                if (isUser.IsNull())
                    throw new ExceptionHelper("User not found, please verify Username and Password.");

                userDto.Id = isUser.Id;
                AtualizaRolesSgqBrPelosDadosDoErp(userDto);
            }
            catch (Exception e)
            {
                throw new Exception("Erro após criar usuario Sgq Global a partir do ERP Sgq Brasil", e);
            }

        }

        private void AtualizaRolesSgqBrPelosDadosDoErp(UserDTO userDto)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                Usuario usuarioSgqBr;
                try
                {
                    usuarioSgqBr = db.Usuario.AsNoTracking().FirstOrDefault(r => r.cSigla.ToLower() == userDto.Name.ToLower());
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao buscar dados do usuario do ERP JSB", e);
                }
                    
                if (usuarioSgqBr != null)
                {
                    IEnumerable<UsuarioPerfilEmpresa> usuarioPerfilEmpresaSgqBr;
                    IEnumerable<ParCompanyXUserSgq> rolesSgqGlobal;
                    IEnumerable<ParCompany> allCompanySgqGlobal;

                    try
                    {
                        usuarioPerfilEmpresaSgqBr = db.UsuarioPerfilEmpresa.Where(r => r.nCdUsuario == usuarioSgqBr.nCdUsuario);
                        rolesSgqGlobal = _baseParCompanyXUserSgq.GetAll().Where(r => r.UserSgq_Id == userDto.Id);
                        allCompanySgqGlobal = _baseParCompany.GetAll();
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Erro ao buscar dados de roles do ERP da JBS", e);
                    }

                    foreach (var upe in usuarioPerfilEmpresaSgqBr)
                    {
                        try
                        {
                            var perfilSgqBr = db.Perfil.FirstOrDefault(r => r.nCdPerfil == upe.nCdPerfil).nCdPerfil.ToString();
                            var parCompanySgqGlobal = allCompanySgqGlobal.FirstOrDefault(r => r.IntegrationId == upe.nCdEmpresa);
                            if (parCompanySgqGlobal != null)
                            {
                                if (rolesSgqGlobal.Any(r => r.ParCompany_Id == parCompanySgqGlobal.Id && r.UserSgq_Id == userDto.Id && r.Role == perfilSgqBr))/*Se existe no global e existe no ERP*/
                                {
                                    try
                                    {
                                        var atualizaRole = rolesSgqGlobal.FirstOrDefault(r => r.ParCompany_Id == parCompanySgqGlobal.Id && r.UserSgq_Id == userDto.Id && r.Role == perfilSgqBr);
                                        atualizaRole.Role = perfilSgqBr;
                                        _baseParCompanyXUserSgq.AddOrUpdate(atualizaRole);
                                    }
                                    catch (Exception e)
                                    {
                                        throw new Exception("Erro ao atualizar uma role existente no SGQ Global e ERP JBS", e);
                                    }
                                }
                                else if (!rolesSgqGlobal.Any(r => r.ParCompany_Id == parCompanySgqGlobal.Id && r.UserSgq_Id == userDto.Id))/*Se não existe no global*/
                                {
                                    try
                                    {
                                        var adicionaRoleGlobal = new ParCompanyXUserSgq()
                                        {
                                            ParCompany_Id = parCompanySgqGlobal.Id,
                                            UserSgq_Id = userDto.Id,
                                            Role = perfilSgqBr
                                        };
                                        _baseParCompanyXUserSgq.AddOrUpdate(adicionaRoleGlobal);
                                    }
                                    catch (Exception e)
                                    {
                                        throw new Exception("Erro ao criar uma nova Role para SGQ Global ", e);
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Erro no loop usuarioPerfilEmpresaSgqBr", e);
                        }
                    }

                    try
                    {
                        var existentesSomenteSgqGlobal = _baseParCompanyXUserSgq.GetAll();
                        var todosOsPerfisSgqBrAssociados = db.Perfil.Where(r => usuarioPerfilEmpresaSgqBr.Any(upe => upe.nCdPerfil == r.nCdPerfil));
                        if (todosOsPerfisSgqBrAssociados != null)
                        {
                            existentesSomenteSgqGlobal = existentesSomenteSgqGlobal.Where(r => todosOsPerfisSgqBrAssociados.Any(t => !(t.nCdPerfil.ToString() == r.Role)));

                            foreach (var removerPerfilSgqGlobal in existentesSomenteSgqGlobal)/*remove se existir no global e nao existir no br*/
                                _baseParCompanyXUserSgq.Remove(removerPerfilSgqGlobal);
                        }
                    }
                    catch (Exception e)
                    {

                        throw new Exception("Erro ao remover uma role existente no sgq global, e removida do ERP JBS", e);
                    }

                }

            }

        }

        private void CriaUSerSgqPeloUserSgqBR(UserDTO userDto)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                try
                {
                    var existenteNoDbAntigo = db.Usuario.FirstOrDefault(r => r.cSigla.ToLower() == userDto.Name.ToLower());
                    if (existenteNoDbAntigo != null)//Porem existe no DB antigo.
                    {
                        UserSgq newUserSgq;

                        try
                        {
                            newUserSgq = new UserSgq()
                            {
                                Name = existenteNoDbAntigo.cSigla.ToLower(),
                                FullName = existenteNoDbAntigo.cNmUsuario,
                                //Email = existenteNoDbAntigo.cEMail,
                                Password = Guard.Criptografar3DES(userDto.Password),
                            };
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Erro ao criar usuario Sgq Brasil", e);
                        }

                        try
                        {
                            _userRepo.Salvar(newUserSgq);//Slava e cria o ID
                            userDto.Id = newUserSgq.Id;
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Error ao salvar o usuario SGQ Brasil > new UserDomain SGQ Global", e);
                        }

                    }
                    else
                    {
                        throw new Exception("User not found, please verify Username and Password.");
                    }
                }
                catch (Exception e)
                {
                    new CreateLog(new Exception("Realizando Rollback em CriaUSerSgqPeloUserSgqBR", e));
                    throw e;
                }
            }
        }

        #endregion

        public GenericReturn<List<UserDTO>> GetAllUserValidationAd(UserDTO userDto)
        {
            try
            {

                AuthenticationLogin(userDto);

                var retorno = Mapper.Map<List<UserSgq>, List<UserDTO>>(_userRepo.GetAllUser());

                foreach (var i in retorno)
                {
                    if (!string.IsNullOrEmpty(i.Password))
                    {
                        i.Password = Guard.Descriptografar3DES(i.Password);
                        i.Password = EncryptStringAES(i.Password);
                    }
                }

                return new GenericReturn<List<UserDTO>>(retorno);
            }
            catch (Exception e)
            {
                return new GenericReturn<List<UserDTO>>(e, falhaGeral);
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

        #region Auxiliares REMOVER E PASSAR PARA CLASSE GUARD.

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

        public static string EncryptStringAES(string cipherText)
        {
            try
            {
                var keybytes = Encoding.UTF8.GetBytes("JDS438FDSSJHLWEQ");
                var iv = Encoding.UTF8.GetBytes("679FDM329IFD23HJ");

                byte[] encryptedbyte = EncryptStringToBytes(cipherText, keybytes, iv);
                string encrypted = Convert.ToBase64String(encryptedbyte);
                return encrypted;
            }
            catch (Exception)
            {
                return cipherText;
            }
        }

        public static string DecryptStringAES(string cipherText)
        {
            try
            {
                var keybytes = Encoding.UTF8.GetBytes("JDS438FDSSJHLWEQ");
                var iv = Encoding.UTF8.GetBytes("679FDM329IFD23HJ");

                var encrypted = Convert.FromBase64String(cipherText);
                var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
                return string.Format(decriptedFromJavascript);
            }
            catch (Exception)
            {
                return cipherText;
            }
        }

        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.  
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.  
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.  
            return encrypted;
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold  
            // the decrypted text.  
            string plaintext = null;

            // Create an RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings  
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.  
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream  
                                // and place them in a string.  
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }

        #endregion
    }

}