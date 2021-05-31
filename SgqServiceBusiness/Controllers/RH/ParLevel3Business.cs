using ADOFactory;
using Dominio;
using System;
using System.Linq;

namespace SgqServiceBusiness.Controllers.RH
{
    internal class ParLevel3Business
    {
        ParLevel3 parLevel3;

        internal ParLevel3 GetBy(int parLevel3_Id)
        {
            using (Factory factory = new Factory("DefaultConnection"))
            {
                parLevel3 = factory.SearchQuery<ParLevel3>($"Select * from ParLevel3 where id = {parLevel3_Id}").SingleOrDefault();
            }

            return parLevel3;
        }
    }
}