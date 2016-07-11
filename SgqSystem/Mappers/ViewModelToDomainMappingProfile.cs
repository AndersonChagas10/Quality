using AutoMapper;
using Dominio.Entities;
using SgqSystem.ViewModels;

namespace SgqSystem.Mappers
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<GenericReturnViewModel<UserViewModel>, GenericReturn<User>>();

            CreateMap<UserViewModel, User>()
                .ConstructUsing(src =>
                    new User(name: src.Name, password: src.Password)
                );

            CreateMap<ResultOldViewModel, ResultOld>()
                .ConstructUsing(src =>
                    new ResultOld(id: src.Id, id_Tarefa: src.Id_Tarefa, id_Operacao: src.Id_Operacao, id_Monitoramento: src.Id_Monitoramento, evaluate: src.Evaluate, notConform: src.NotConform)
                );
        }
    }
}