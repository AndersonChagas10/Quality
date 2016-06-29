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
        }
    }
}