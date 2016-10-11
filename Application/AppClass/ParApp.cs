using Application.Interface;
using System;
using DTO.DTO;
using Dominio.Interfaces.Services;

namespace Application.AppClass
{
    public class ParApp : IParApp
    {
        #region Construtor

        private IParamsDomain _paramsDomain;

        public ParApp(IParamsDomain paramsDomain)
        {
            _paramsDomain = paramsDomain;
        } 

        #endregion

        public ParamsDTO AddUpdateCompany()
        {
            throw new NotImplementedException();
        }

        public ParamsDTO AddUpdateParLevel1()
        {
           return _paramsDomain.AddUpdateParLevel1();
        }

        public ParamsDTO AddUpdateParLevel2()
        {
            throw new NotImplementedException();
        }

        public ParamsDTO AddUpdateParLevel3()
        {
            throw new NotImplementedException();
        }

        public ParamsDTO Integrate()
        {
            throw new NotImplementedException();
        }
    }
}
