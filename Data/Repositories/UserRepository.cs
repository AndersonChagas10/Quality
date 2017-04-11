using Dominio;
using Dominio.Interfaces.Repositories;
using DTO.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repositories
{
    public class UserRepository : RepositoryBase<UserSgq>, IUserRepository
    {
        
        public UserRepository(SgqDbDevEntities _db)
            : base(_db)
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

        public UserSgq GetByName(string Name)
        {
            //  return GetAll().FirstOrDefault(r => r.Name.ToLower().Equals(Name.ToLower()));
            return db.UserSgq.Include("ParCompanyXUserSgq").Include("UnitUser").FirstOrDefault(r => r.Name.Equals(Name));
        }

        public bool UserNameIsCadastrado(string Name, int id)
        {
            return db.UserSgq.Any(x => x.Id != id && x.Name == Name);
        }
        
        public void Salvar(UserSgq user)
        {
            AddOrUpdate(user);
            Commit();
        }

        public UserSgq AuthenticationLogin(UserSgq user)
        {
            var pass = Guard.EncryptStringAES(user.Password);
            var result = db.UserSgq.Include("ParCompanyXUserSgq").Include("UnitUser")
                .FirstOrDefault(x => x.Name.ToLower().Equals(user.Name.ToLower()) && x.Password.Equals(pass));

            if (result == null)/*Verifica no caso de a senha estar descriptografada no DB e atualiza a mesma ,agora criptografada, no db.*/
            {
                var userByPassDecripted = db.UserSgq.Include("ParCompanyXUserSgq").Include("UnitUser")
                    .FirstOrDefault(x => x.Name.ToLower().Equals(user.Name.ToLower()) && x.Password.Equals(user.Password));

                if (userByPassDecripted != null)
                {
                    userByPassDecripted.Password = pass;
                    Salvar(userByPassDecripted);
                    return db.UserSgq.Include("ParCompanyXUserSgq").Include("UnitUser")
                        .FirstOrDefault(x => x.Name.ToLower().Equals(user.Name.ToLower()) && x.Password.Equals(pass)); ;
                }
            }
            return result;
        }

        public List<UserSgq> GetAllUser(int unidadeId)
        {
            if (unidadeId > 0)
               return db.UserSgq.Include("ParCompanyXUserSgq").Include("UnitUser").Where(r => r.ParCompanyXUserSgq.Any(x => x.ParCompany_Id == unidadeId)).ToList();
            else
                return db.UserSgq.Include("ParCompanyXUserSgq").Include("UnitUser").ToList();
        }
    }
}
