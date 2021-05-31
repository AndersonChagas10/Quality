using ADOFactory;
using Dominio;
using System;
using System.Linq;

namespace SgqServiceBusiness.Controllers.RH
{
    internal class ParCargoBusiness
    {
        ParCargo parCargo;

        internal ParCargo GetBy(int parCargo_Id)
        {
            using (Factory factory = new Factory("DefaultConnection"))
            {
                parCargo = factory.SearchQuery<ParCargo>($"Select * from ParCargo where id = {parCargo_Id}").SingleOrDefault();
            }

            return parCargo;
        }
    }
}