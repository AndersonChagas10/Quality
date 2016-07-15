using AutoMapper;
using Dominio.Entities;
using DTO.DTO;
using DTO.Helpers;
using SgqSystem.ViewModels;

namespace SgqSystem.Mappers
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<GenericReturnViewModel<UserViewModel>, GenericReturn<UserSgq>>();

            CreateMap<GenericReturnViewModel<UserViewModel>, GenericReturn<UserDTO>>();

            CreateMap<UserViewModel, UserSgq>()
                .ConstructUsing(src =>
                    new UserSgq(name: src.Name, password: src.Password)
                );

            CreateMap<UserDTO, UserSgq>()
              .ConstructUsing(src =>
                  new UserSgq(name: src.Name, password: src.Password)
              );

            CreateMap<UserViewModel, UserDTO>();

            CreateMap<ResultOldViewModel, ResultOld>()
                .ConstructUsing(src =>
                    new ResultOld(id: src.Id, id_Tarefa: src.Id_Tarefa, id_Operacao: src.Id_Operacao, id_Monitoramento: src.Id_Monitoramento, evaluate: src.Evaluate, notConform: src.NotConform)
                );
        }
    }
}