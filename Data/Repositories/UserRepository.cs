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

            var result = db.Set<UserSgq>().FirstOrDefault(x => x.Name.ToLower().Equals(user.Name.ToLower()) && x.Password.Equals(user.Password));
            if (result == null)/*Verifica no caso de a senha estar descriptografada no DB e atualiza a mesma ,agora criptografada, no db.*/
            {
                var userByName = db.Set<UserSgq>().FirstOrDefault(x => x.Name.ToLower().Equals(user.Name.ToLower()));
                if (Guard.Criptografar3DES(userByName.Password).Equals(user.Password))
                {
                    result = userByName;
                    result.Password = Guard.Criptografar3DES(result.Password);
                    Salvar(result);
                }

            }

            return result;
        }

        public List<UserSgq> GetAllUser()
        {
            return GetAll().ToList();
        }
    }
}
