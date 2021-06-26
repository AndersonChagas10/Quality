﻿using Data.PlanoDeAcao;
using Data.PlanoDeAcao.Repositorio;
using Data.Repositories;
using DTO.Interfaces.Repositories;
using Ninject.Modules;
using Services.PlanoDeAcao;

namespace CrossCutting.IOC.Module
{
    public class DataNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IBaseRepository<>)).To(typeof(RepositoryBase<>));
            Bind(typeof(IBaseRepositoryNoLazyLoad<>)).To(typeof(RepositoryBaseNoLazyLoad<>));
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IParamsRepository>().To<ParamsRepository>();
            Bind<IParLevel3Repository>().To<ParLevel3Repository>();
            Bind<IAcaoRepository>().To<AcaoRepository>();
            Bind<IAcaoService>().To<AcaoService>();
            Bind<IAcompanhamentoAcaoRepository>().To<AcompanhamentoAcaoRepository>();
            Bind<IEvidenciaNaoConformeRepository>().To<EvidenciaNaoConformeRepository>();
            Bind<IEvidenciaNaoConformeService>().To<EvidenciaNaoConformeService>();
            Bind<IEvidenciaConcluidaRepository>().To<EvidenciaConcluidaRepository>();
            Bind<IEvidenciaConcluidaService>().To<EvidenciaConcluidaService>();
        }
    }
}
