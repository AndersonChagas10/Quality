using ADOFactory;
using Dominio;
using System;
using System.Linq;

namespace SgqServiceBusiness.Controllers.RH
{
    internal class ParLevel2Business
    {
        ParLevel2 parLevel2;

        internal ParLevel2 GetBy(int parLevel2_Id)
        {
            using (Factory factory = new Factory("DefaultConnection"))
            {
                parLevel2 = factory.SearchQuery<ParLevel2>($"Select * from ParLevel2 where id = {parLevel2_Id}").SingleOrDefault();
            }

            return parLevel2;
        }
    }
}