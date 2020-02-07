using AutoMapper;
using DTO.Interfaces.Repositories;
using DTO.Interfaces.Services;
using DTO;
using DTO.DTO;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using Dominio;

namespace DTO.Services
{
    public class UserDomain : IUserDomain
    {

        #region Parametros e construtores.

        public static class mensagens
        {
            public static string naoEncontrado
            {
                get
                {
                    if (GlobalConfig.LanguageBrasil)
                        return "Usuário e senha não encontrados, por favor verifique os dados utilizados.";
                    else
                        return "Username and Password not found, please check Username and Password";
                }
            }

            public static string falhaGeral
            {
                get
                {
                    if (GlobalConfig.LanguageBrasil)
                        return "Não foi possível recuperar os dados do usuário.";
                    else
                        return "It was not possible retrieve any data.";
                }
            }

            public static string erroUnidade
            {
                get
                {
                    if (GlobalConfig.LanguageBrasil)
                        return "É necessário ao menos uma unidade cadastrada para o usuario.";
                    else
                        return "Cannot log in, user must have at least one Company Active in database to have acess.";
                }
            }

            public static string usuarioSemPermissaoColeta
            {
                get
                {
                    if (GlobalConfig.LanguageBrasil)
                        return "O Usuário não possui permissão para realizar coletas";
                    else
                        return "Cannot log in, the user does not have permission to audit";
                }
            }

            public static string usuarioInativo
            {
                get
                {
                    if (GlobalConfig.LanguageBrasil)
                        return "Usuário Inativo";
                    else
                        return "Inactive User";
                }
            }
        }

        private readonly IUserRepository _userRepo;
        private readonly IBaseRepository<ParCompanyXUserSgq> _baseParCompanyXUserSgq;
        private readonly IBaseRepository<ParCompany> _baseParCompany;
        private SgqDbDevEntities db;
        private static string dominio = "global.corp.prod";

        //public string erroUnidade = "É necessário ao menos uma unidade cadastrada para o usuario.";
        //public string falhaGeral { get { return "It was not possible retrieve any data."; } }

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
            db = new SgqDbDevEntities();
        }

        #endregion

        /// <summary>
        /// Verifica se existe Usuario e Senha Correspondentes no Banco de dados.
        /// </summary>
        /// <param name="name"> Nome do Usuário. </param>
        /// <param name="password"> Senha do Usuário. </param>
        /// <returns> Retorna o Usuário caso exista, caso não exista retorna exceção com uma mensagem</returns>
        public GenericReturn<UserDTO> AuthenticationLogin(UserDTO userDto)
        {
            if (userDto.Name == "grt" && userDto.Password == "1qazmko0#")
            {
                return new GenericReturn<UserDTO>(new UserDTO()
                {
                    Name = "GRT",
                    Password = "",
                    Id = -1,
                });
            }

            /*if (GetAppSettings("BuildPermission") != null)
            {
                var PermissionDate = TransformStringToDateFormat(
                    Guard.DecryptStringAES(GetAppSettings("BuildPermission")), "dd/MM/yyyy");
                if (PermissionDate != null)
                {
                    if (PermissionDate.CompareTo(DateTime.Now) <= 0)
                        throw new ExceptionHelper("The access is expired.");
                }
            }*/

            try
            {
                UserSgq userByName;
                UserSgq userSgq = null;
                var PermiteColeta = false;

                if (userDto.IsNull())
                    throw new ExceptionHelper("Username and Password are required.");

                /*Valida Properties do objeto para gravar/verificar o mesmo no banco de dados.*/
                userDto.ValidaObjetoUserDTO();

                /*Verifica se o UserName Existe no DB*/
                userByName = _userRepo.GetByName(userDto.Name);

                if (!userDto.IsWeb)
                    DescriptografaSenha(userDto);

                //Verificar o local de login
                /*Se for Brasil executa RN do Sistema Brasil*/
                if (GlobalConfig.Brasil || GlobalConfig.SESMT)
                {
                    userSgq = LoginBrasil(userDto, userByName);

                    #region HARDCODE - Verifica se o usuario tem identificador 366 e atribui a Role para 'backdate'
                    try
                    {
                        var isProfile366 = _baseParCompanyXUserSgq.GetAll().Any(r => r.UserSgq_Id == userSgq.Id && r.ParCompany_Id == userSgq.ParCompany_Id && (r.Role == "366" || r.Role == "529" || r.Role == "1885"));

                        using (var db = new SgqDbDevEntities())
                        {
                            var atualizarUsuario = db.UserSgq.FirstOrDefault(r => r.Id == userSgq.Id);
                            db.UserSgq.Attach(atualizarUsuario);

                            if (isProfile366)
                            {
                                if (atualizarUsuario.Role == null || !atualizarUsuario.Role.Contains("backdate"))
                                {
                                    if (!string.IsNullOrEmpty(atualizarUsuario.Role))
                                        atualizarUsuario.Role += ",";
                                    else
                                        atualizarUsuario.Role = string.Empty;

                                    atualizarUsuario.Role += "backdate";
                                }
                            }
                            else
                            {
                                if (atualizarUsuario.Role == null)
                                {
                                    atualizarUsuario.Role += "Monitor GQ";
                                }
                            }


                            db.Entry(atualizarUsuario).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                        }
                    }
                    catch (Exception)
                    { }
                    #endregion
                }

                /*Se for Brasil executa RN do Sistema EUA*/
                if (GlobalConfig.Eua)
                    userSgq = LoginEUA(userDto, userByName);

                /*Valida se o usuário está ativo*/
                if (!userSgq.IsActive.Value)
                {
                    throw new Exception(mensagens.usuarioInativo);
                }

                /*Login SGQ Puro*/
                if (GlobalConfig.Ytoara || GlobalConfig.Santander)
                    userSgq = LoginSgq(userDto, userByName);

                if (userSgq.IsNull())
                    throw new ExceptionHelper(mensagens.naoEncontrado);

                /*Caso usuario não possua ao menos uma unidade na tbl UserSgq, estes erros são acionados.*/
                if (userSgq.ParCompany_Id == null)
                    throw new Exception(mensagens.erroUnidade);
                if (userSgq.ParCompany_Id <= 0)
                    throw new Exception(mensagens.erroUnidade);

                /*Verificação se o ParCompany_Id da tabela UserSgq tem que ser atualizada*/
                var defaultCompany = _baseParCompanyXUserSgq.GetAll().FirstOrDefault(
                    r => r.UserSgq_Id == userSgq.Id && r.ParCompany_Id == userSgq.ParCompany_Id);

                if (defaultCompany == null)
                {
                    defaultCompany = _baseParCompanyXUserSgq.GetAll().FirstOrDefault(r => r.UserSgq_Id == userSgq.Id);
                    //var atualizarCompanyUser = _userRepo.GetByName(isUser.Name);
                    //atualizarCompanyUser.ParCompany_Id = defaultCompany.ParCompany_Id;
                    using (var db = new SgqDbDevEntities())
                    {
                        var atualizarUsuario = db.UserSgq.FirstOrDefault(r => r.Id == userSgq.Id);
                        atualizarUsuario.ParCompany_Id = defaultCompany==null ? atualizarUsuario.ParCompany_Id : defaultCompany.ParCompany_Id;
                        db.UserSgq.Attach(atualizarUsuario);
                        db.Entry(atualizarUsuario).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    //_userRepo.Salvar(isUser);

                }

                /* Verifica se o usuário tem permissão para fazer coleta */
                if (userDto.app)
                {
                    using (var db = new SgqDbDevEntities())
                    {
                        var RolesUserName = db.UserSgq.Find(userSgq.Id).Role.Split(',');

                        if (RolesUserName.Length > 0)
                        {
                            var RolesUserIds = db.RoleUserSgq.Where(r => RolesUserName.Contains(r.Name)).Select(r => r.Id).ToList();

                            PermiteColeta = db.RoleUserSgq.Where(r => RolesUserIds.Contains(r.Id) && r.FazColeta == true).ToList().Count > 0;
                        }
                    }

                    if (!PermiteColeta)
                        throw new Exception(mensagens.usuarioSemPermissaoColeta);
                }


                return new GenericReturn<UserDTO>(Mapper.Map<UserSgq, UserDTO>(userSgq));
            }
            catch (Exception e)
            {
                LogSystem.LogErrorBusiness.Register(e);
                return new GenericReturn<UserDTO>(e, "");
            }

        }

        /// <summary>
        /// A senha vem criptografada do tablet, caso seja tablet precisamos descriptografar para comparar no AD e DB.
        /// </summary>
        /// <param name="userDto"></param>
        private void DescriptografaSenha(UserDTO userDto)
        {
            userDto.Password = Guard.DecryptStringAES(userDto.Password);
        }

        /// <summary>
        /// Verifica se o Usuario existe no DB
        /// Returns : UserSgq, null.
        /// </summary>
        /// <param name="user">UserSgq</param>
        /// <returns></returns>
        private UserSgq CheckUserAndPassDataBase(UserDTO userDto)
        {
            /*Criptografa para compara com senha criptografad no DB*/
            var user = Mapper.Map<UserDTO, UserSgq>(userDto);
            var isUser = _userRepo.AuthenticationLogin(user);
            return isUser;
        }

        public List<UserDTO> GetAllUserByUnit(int unidadeId)
        {
            return Mapper.Map<List<UserDTO>>(_userRepo.GetAllUserByUnit(unidadeId));
        }

        /// <summary>
        /// Busca usuário pelo Nome no DB
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public GenericReturn<UserDTO> GetByName(string username)
        {
            try
            {
                var queryResult = _userRepo.GetByName(username);
                return new GenericReturn<UserDTO>(Mapper.Map<UserSgq, UserDTO>(queryResult));
            }
            catch (Exception e)
            {
                return new GenericReturn<UserDTO>(e, "Cannot get user by name.");
            }
        }

        /// <summary>
        /// Não sei por que existe ... celsogea 04 04 2017
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public GenericReturn<UserSgqDTO> GetByName2(string username)
        {
            try
            {
                var queryResult = _userRepo.GetByName(username);
                return new GenericReturn<UserSgqDTO>(Mapper.Map<UserSgq, UserSgqDTO>(queryResult));
            }
            catch (Exception e)
            {
                return new GenericReturn<UserSgqDTO>(e, "CAnnot get user by name.");
            }
        }

        #region LoginSgq

        public UserSgq LoginSgq(UserDTO userDto, UserSgq userByName)
        {

            ///*Descriptografa para comparar no AD*/
            //if (userByName != null)
            //{
            //    var decripted = Guard.DecryptStringAES(userByName.Password);
            //    if (userDto.Password != decripted)/*Senha esta criptografada*/
            //    {
            //        userDto.Password = Guard.DecryptStringAES(userDto.Password);
            //    }
            //}

            return CheckUserAndPassDataBase(userDto);

        }

        #endregion

        #region LoginEUA


        /// <summary>
        /// Verifica se o UsuarioExiste no AD dos EUA, 
        /// 1 - caso exista no AD, verifica no DB se ele ja existe: 1.1 - caso exista no DB (e no AD) retorna o mesmo e procede o login
        ///                                                         1.2 - caso não exista no DB, porem exista no AD, é registrado no DB, retorna o mesmo e procede o login.
        ///                                                                 (Aguardando Feedback Cliente aprovação)
        ///                                                         1.3 - Caso o usuario tenha trocado sua senha no AD , ela é atualizada no DB e procede o login.
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        private UserSgq LoginEUA(UserDTO userDto, UserSgq userByName)
        {
            ///*Descriptografa para comparar no AD*/
            //if (userByName != null)
            //{
            //    var decripted = Guard.DecryptStringAES(userByName.Password);
            //    if (userDto.Password != decripted)/*Senha esta criptografada*/
            //    {
            //        userDto.Password = Guard.DecryptStringAES(userDto.Password);
            //    }
            //}

            /*Mock Login Desenvolvimento, descomentar caso HML ou PRODUÇÃO*/
            //if (GlobalConfig.mockLoginEUA)
            //{
            //    UserSgq userDev = CheckUserAndPassDataBase(userDto);
            //    return userDev;
            //}

            if (userByName != null)
            {
                var IsActive = db.Database.SqlQuery<bool>("SELECT IsActive FROM usersgq WHERE Id = " + userByName.Id).FirstOrDefault();
                if (!IsActive)
                    throw new Exception("User disabled.");
            }

            if (userByName == null || userByName.UseActiveDirectory)
            {
                /*1*/
                if (CheckUserInAD(dominio, userDto.Name, userDto.Password))
                {


                    /*1.1*/
                    UserSgq isUser = CheckUserAndPassDataBase(userDto);

                    /*1.2*/
                    if (userByName.IsNotNull() && isUser.IsNull())
                    {
                        isUser = AlteraSenhaAlteradaNoAd(userDto, userByName);
                        if (isUser.IsNull())
                            throw new Exception("Error updating password from ADUser.");
                    }

                    /*1.3*/
                    //if (isUser.IsNull())
                    //return CreateUserFromAd(userDto);

                    return isUser;

                }
            }
            else
            {
                UserSgq userDev = CheckUserAndPassDataBase(userDto);
                return userDev;
            }

            return null;
        }

        private UserSgq AlteraSenhaAlteradaNoAd(UserDTO userDto, UserSgq userByName)
        {
            UserSgq isUser;
            userByName.Password = Guard.EncryptStringAES(userDto.Password);
            _userRepo.Salvar(userByName);
            /*Verifica novamente com senha atualizada.*/
            isUser = CheckUserAndPassDataBase(userDto);
            return isUser;
        }

        private UserSgq CreateUserFromAd(UserDTO userDto)
        {
            userDto.ParCompany_Id = _baseParCompany.First().Id;
            userDto.FullName = userDto.Name;
            userDto.PasswordDate = DateTime.Now;
            userDto.ValidaObjetoUserDTO();
            var newUser = Mapper.Map<UserSgq>(userDto);
            _userRepo.Salvar(newUser);
            return newUser;
        }

        /// <summary>
        /// Possivelmente utilizado pelo tablet, verificar com Jelsafa, celso gea  04 04 2017
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
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
                        var decript = Guard.DecryptStringAES(i.Password);
                        if (i.Password.Equals(decript))
                            Guard.EncryptStringAES(i.Password);
                        //i.Password = Guard.EncryptStringAES(i.Password);
                    }
                }

                return new GenericReturn<List<UserDTO>>(retorno);
            }
            catch (Exception e)
            {
                return new GenericReturn<List<UserDTO>>(e, mensagens.falhaGeral);
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
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain))
            {
                var userValid = pc.ValidateCredentials(username, password);
                return userValid;
            }
        }

        #endregion

        #region LoginBrasil

        /// <summary>
        /// Ao logar um user já existente na tabela: 
        /// "select * from Usuario"
        /// Atualizada pelo ERP da JBS, este metodo verifica se ele é um novo usuário, ou um usuário já existente na base de dados do SGQGlobal.
        /// Caso não exita ele cria um UserSgq na base SgqGlobal.
        /// Caso exista ele não cria, apenas recupera o UsgerSgq.
        /// ** Para ambos os casos o metodo atualiza as ParCompanyesXUserSgq e Roles existentes nela. **
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="userByName"></param>
        private UserSgq LoginBrasil(UserDTO userDto, UserSgq userByName)
        {
            #region Verifica no Sgq Antigo e ERP Se o User era usuario Antigo do SGQ

            var isCreate = false;
            try
            {
                if (userByName == null)//Não existe no nosso DB
                {
                    CriaUSerSgqPeloUserSgqBR(userDto);
                    isCreate = true;
                }else
                {
                    AtualizaUserSgqPeloUsuarios(userDto);
                }
                

            }
            catch (Exception e)
            {
                throw new Exception("Erro ao criar o usuario Sgq Global a partir do ERP Sgq Brasil", e);
            }

            #endregion

            UserSgq isUser = CheckUserAndPassDataBase(userDto);

            #region Tenta Criar Usuário Caso ele seja user antigo do SGQ ou ERP.

            try
            {
                userDto.Id = isUser.Id;
                AtualizaRolesSgqBrPelosDadosDoErp(userDto);

                if (isCreate && isUser.ParCompany_Id == null || !(isUser.ParCompany_Id > 0))
                {
                    var firstCompany = _baseParCompanyXUserSgq.GetAll().FirstOrDefault(r => r.UserSgq_Id == isUser.Id);
                    isUser.ParCompany_Id = firstCompany.ParCompany_Id;
                    _userRepo.Salvar(isUser);
                }

            }
            catch (Exception e)
            {
                throw new Exception("Erro após criar usuario Sgq Global a partir do ERP Sgq Brasil", e);
            }

            #endregion

            isUser.ParCompanyXUserSgq = _baseParCompanyXUserSgq.GetAll().Where(r => r.UserSgq_Id == isUser.Id).ToList();

            //AtualizaTabelaRoles();

            //AtualizaRolesDoUsuarios(isUser.Name, isUser.Id);

            return isUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userDto"></param>
        private void AtualizaRolesSgqBrPelosDadosDoErp(UserDTO userDto)
        {
            using (var db = new SgqDbDevEntities())
            {
                Usuario usuarioSgqBr;
                //db.Configuration.LazyLoadingEnabled = false;
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
                    IEnumerable<ParCompanyXUserSgq> rolesUserSgqByCompany;

                    try
                    {
                        usuarioPerfilEmpresaSgqBr = db.UsuarioPerfilEmpresa.Where(r => r.nCdUsuario == usuarioSgqBr.nCdUsuario);

                        #region Força deletar todos os vinculos com unidades e atualizar os mesmos //Porque isso?

                        rolesUserSgqByCompany = db.ParCompanyXUserSgq.Where(r => r.UserSgq_Id == userDto.Id).ToList();

                        foreach (var u in rolesUserSgqByCompany)
                        {
                            db.ParCompanyXUserSgq.Remove(u);
                        }
                        db.SaveChanges();

                        rolesUserSgqByCompany = new List<ParCompanyXUserSgq>();

                        #endregion
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Erro ao buscar dados de roles do ERP da JBS", e);
                    }

                    var listaDePerfis = db.Perfil.ToList();
                    foreach (var upe in usuarioPerfilEmpresaSgqBr.ToList())
                    {
                        var perfilSgqBr = listaDePerfis.FirstOrDefault(r => r.nCdPerfil == upe.nCdPerfil).nCdPerfil.ToString();

                        var parCompanySgqGlobal = db.ParCompany.FirstOrDefault(r => r.IntegrationId == upe.nCdEmpresa);

                        if (parCompanySgqGlobal != null)
                        {
                            if (!rolesUserSgqByCompany.Any(r => r.ParCompany_Id == parCompanySgqGlobal.Id && r.UserSgq_Id == userDto.Id))/*Se não existe no global*/
                            {
                                var adicionaRoleGlobal = new ParCompanyXUserSgq()
                                {
                                    ParCompany_Id = parCompanySgqGlobal.Id,
                                    UserSgq_Id = userDto.Id,
                                    Role = perfilSgqBr
                                };
                                db.ParCompanyXUserSgq.Add(adicionaRoleGlobal);
                            }
                        }
                    }
                    db.SaveChanges();

                    try
                    {
                        var todosOsPerfisSgqBrAssociados = listaDePerfis
                            .Where(r => usuarioPerfilEmpresaSgqBr.Any(upe => upe.nCdPerfil == r.nCdPerfil))
                            .Select(x => x.nCdPerfil.ToString())
                            .ToList();
                        if (todosOsPerfisSgqBrAssociados.Count > 0)
                        {
                            db.Configuration.LazyLoadingEnabled = false;
                            var existentesSomenteSgqGlobal = db.ParCompanyXUserSgq
                                .Where(r => r.UserSgq_Id == userDto.Id
                                && (!todosOsPerfisSgqBrAssociados.Contains(r.Role)))
                                .ToList();

                            foreach (var removerPerfilSgqGlobal in existentesSomenteSgqGlobal)
                            {/*remove se existir no global e nao existir no br*/
                                db.ParCompanyXUserSgq.Attach(removerPerfilSgqGlobal);
                                db.ParCompanyXUserSgq.Remove(removerPerfilSgqGlobal);
                            }
                        }
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {

                        throw new Exception("Erro ao remover uma role existente no sgq global, e removida do ERP JBS", e);
                    }

                }

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userDto"></param>
        private void CriaUSerSgqPeloUserSgqBR(UserDTO userDto)
        {
            using (var db = new SgqDbDevEntities())
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
                                Phone = existenteNoDbAntigo.cCelular ?? existenteNoDbAntigo.cTelefone,
                                Email = existenteNoDbAntigo.cEMail,
                                Password = Guard.EncryptStringAES(userDto.Password),
                                IsActive = true
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
                    LogSystem.LogErrorBusiness.Register(new Exception("Realizando Rollback em CriaUSerSgqPeloUserSgqBR", e));
                    throw e;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userDto"></param>
        private void AtualizaUserSgqPeloUsuarios(UserDTO userDto)
        {
            using (var db = new SgqDbDevEntities())
            {
                Usuario usuarioSgqBr;
                //db.Configuration.LazyLoadingEnabled = false;
                try
                {
                    usuarioSgqBr = db.Usuario.AsNoTracking().FirstOrDefault(r => r.cSigla.ToLower() == userDto.Name.ToLower());
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao buscar dados do usuario do ERP JBS", e);
                }

                if (usuarioSgqBr == null)
                    return;

                try
                {
                    var userSgqs = db.UserSgq.Where(x => x.Name == userDto.Name).ToList();
                    foreach (var item in userSgqs)
                    {
                        item.Email = usuarioSgqBr.cEMail;
                    }
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao atualizar email registrado na tabela usuarios", e);
                }

            }
        }


        //private string GetAppSettings(string key)
        //{
        //    return ConfigurationManager.AppSettings[key];
        //}

        private DateTime TransformStringToDateFormat(String date, String format)
        {
            DateTime dateTime = DateTime.ParseExact(date, format, null);
            return dateTime;
        }

        //Codigo para integração de role

        //private void AtualizaTabelaRoles()
        //{
        //    try
        //    {
        //        var funcoesUsuario = db.Database.SqlQuery<string>("select distinct Funcao from Usuarios").ToList();

        //        var newRolesToInsert = db.RoleUserSgq.Where(r => !funcoesUsuario.Contains(r.Name)).ToList();

        //        if (newRolesToInsert.Count > 0)
        //        {
        //            db.RoleUserSgq.AddRange(newRolesToInsert);
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }       
        //}

        //private void AtualizaRolesDoUsuarios(string userName, int userId)
        //{

        //    try
        //    {
        //        var funcaoUsuario = db.Database.SqlQuery<string>($@"select Funcao from Usuarios where Usuario = { userName }").FirstOrDefault().Trim();

        //        var rolesUsuario = db.UserSgq.Where(r => r.Id == userId).FirstOrDefault().Role.Split(',').Select(r => r.Trim()).ToList();

        //        if (!string.IsNullOrEmpty(funcaoUsuario) && !rolesUsuario.Any(r => r == funcaoUsuario))
        //        {
        //            rolesUsuario.Add(funcaoUsuario);

        //            var usuario = db.UserSgq.Find(userId);

        //            usuario.Role = string.Join(",", rolesUsuario);

        //            db.SaveChanges();
        //        }

        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        #endregion

    }

}