using AutoMapper;
using NHibernate;

namespace PA.Domain.AutoMapper
{
    public static class AutoMapperNHibernateUtil
    {
        public static void CreateMap<TSource, TDestination>()
        {
            //Mapper.CreateMap<TSource, TDestination>().ForAllMembers(opt => opt.Condition(source => NHibernateUtil.IsInitialized(source.SourceValue)));
        }
    }
}
