using ADOFactory;
using Dominio;
using System;
using System.Linq;

namespace SgqServiceBusiness.Controllers.RH
{
    internal class ParLevel1Business
    {
        ParLevel1 parlevel1;

        internal ParLevel1 GetBy(int parLevel1_Id)
        {
            using (Factory factory = new Factory("DefaultConnection"))
            {
                parlevel1 = factory.SearchQuery<ParLevel1>($"Select * from ParLevel1 where id = {parLevel1_Id}").SingleOrDefault();
            }

            return parlevel1;
        }
    }
}