using DTO.Interfaces.Services;
using DTO.Services;
using Ninject.Modules;

namespace CrossCutting.IOC.Module
{
    public class DomainNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IBaseDomain<,>)).To(typeof(BaseDomain<,>));
            Bind<IUserDomain>().To<UserDomain>();
            Bind<IParamsDomain>().To<ParamsDomain>();
            Bind<ICompanyDomain>().To<CompanyDomain>();
            Bind<IDefectDomain>().To<DefectDomain>();
        }
    }
}
