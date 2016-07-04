using AutoMapper;
using Dominio.Entities;
using SgqSystem.ViewModels;

namespace SgqSystem.Mappers
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        //protected override void Configure()
        //{
        //    //CreateMap<DateTime, String>().ConvertUsing<StringFromDateTimeTypeConverter>();
        //}
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<GenericReturnViewModel<UserViewModel>, GenericReturn<User>>();
            CreateMap<UserViewModel, User>();
            CreateMap<ResultOldViewModel, ResultOld>()
                .ConstructUsing(src =>
                    new ResultOld()
                    {
                        Id = src.Id,
                        Id_Monitoramento = src.Id_Monitoramento,
                        Id_Operacao = src.Id_Operacao,
                        Id_Tarefa = src.Id_Tarefa
                        //NotConform = src.NotConform,
                        //Evaluate = src.Evaluate
                    }
                );
            //.IgnoreAllPropertiesWithAnInaccessibleSetter(); ;
        }
    }
}