using ADOFactory;
using Dominio;
using System;
using System.Linq;

namespace SgqServiceBusiness.Controllers.RH
{
    internal class UserSgqBusiness
    {
        UserSgq userSgq;

        internal UserSgq GetBy(int id)
        {
            using (Factory factory = new Factory("DefaultConnection"))
            {
                userSgq = factory.SearchQuery<UserSgq>($"Select * from UserSgq where id = {id}").SingleOrDefault();
            }

            return userSgq;
        }
    }
}