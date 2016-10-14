using Dominio;
using Dominio.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repositories
{
    public class UserRepository : RepositoryBase<UserSgq>, IUserRepository
    {

        public UserRepository(SgqDbDevEntities _db)
            : base(_db)
        {
        }

        public UserSgq GetByName(string Name)
        {
            //  return GetAll().FirstOrDefault(r => r.Name.ToLower().Equals(Name.ToLower()));
            return GetAll().Where(r => r.Name.ToLower().Equals(Name.ToLower())).FirstOrDefault();
        }

        public bool UserNameIsCadastrado(string Name, int id)
        {
            return GetAll().Any(x => x.Id != id && x.Name == Name);
        }

        public void Salvar(UserSgq user)
        {
            AddOrUpdate(user);
            Commit();
        }

        public UserSgq AuthenticationLogin(UserSgq user)
        {

            var result = db.Set<UserSgq>().Where(x => x.Name.ToLower().Equals(user.Name.ToLower()) && x.Password.Equals(user.Password)).FirstOrDefault();
            // var result = db.Set<UserSgq>().FirstOrDefault(r => r.Name.ToLower().Equals(user.Name.ToLower()) && r.Password.Equals(user.Password));
            return result;
        }

        public List<UserSgq> GetAllUser()
        {
            return GetAll().ToList();
        }
    }
}
