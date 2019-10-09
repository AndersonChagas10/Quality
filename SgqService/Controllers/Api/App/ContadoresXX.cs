using ADOFactory;
using Dominio;
using DTO;
using DTO.Helpers;
using ServiceModel;
using SgqService.Controllers.Api;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;

namespace SgqService.Controllers.Api.App
{
    public class ContadoresXX
    {
        public SgqServiceBusiness.Api.App.ContadoresXX business;

        public ContadoresXX()
        {
            business = new SgqServiceBusiness.Api.App.ContadoresXX();
        }
        public List<DefeitosPorAmostra> GetContadoresXX(SgqDbDevEntities db, int level1Id,
            int ParCompany_Id)
        {
            return business.GetContadoresXX(db, level1Id, ParCompany_Id);
        }
    }
}