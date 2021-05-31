using ADOFactory;
using Dominio;
using System;
using System.Linq;

namespace SgqServiceBusiness.Controllers.RH
{
    internal class ParDepartmentBusiness
    {
        ParDepartment parDepartment;

        internal ParDepartment GetBy(int parDepartment_Id)
        {
            using (Factory factory = new Factory("DefaultConnection"))
            {
                parDepartment = factory.SearchQuery<ParDepartment>($"Select * from ParDepartment where id = {parDepartment_Id}").SingleOrDefault();
            }

            return parDepartment;
        }
    }
}