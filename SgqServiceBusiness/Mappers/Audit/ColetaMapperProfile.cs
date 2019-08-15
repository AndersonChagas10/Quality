using AutoMapper;

namespace SgqSystem.Mappers
{
    public class ColetaMapperProfile : Profile
    {
        public ColetaMapperProfile()
        {

            //CreateMap<ColetaDTO, Coleta>();
            //CreateMap<Coleta, ColetaDTO>();

            ////Coleta para >>> ColetaViewModel, ColetaDTO

            //CreateMap<GenericReturn<Coleta>, GenericReturn<ColetaViewModel>>();
            //CreateMap<Coleta, ColetaViewModel>();
            //CreateMap<GenericReturn<Coleta>, GenericReturn<ColetaDTO>>();
            //CreateMap<Coleta, ColetaDTO>();

            ////ColetaViewModel para >>> Coleta, ColetaDTO

            //CreateMap<GenericReturn<ColetaViewModel>, GenericReturn<Coleta>>();
            //CreateMap<ColetaViewModel, Coleta>();
            //CreateMap<GenericReturn<ColetaViewModel>, GenericReturn<ColetaDTO>>();
            //CreateMap<ColetaViewModel, ColetaDTO>();

            ////ColetaDTO para >>> ColetaViewModel, Coleta

            //CreateMap<GenericReturn<ColetaDTO>, GenericReturn<ColetaViewModel>>();
            //CreateMap<GenericReturn<List<ColetaDTO>>, GenericReturn<List<ColetaViewModel>>>();
            //CreateMap<ColetaDTO, ColetaViewModel>();
            //CreateMap<GenericReturn<ColetaDTO>, GenericReturn<Coleta>>();
            //CreateMap<ColetaDTO, Coleta>();

            //CreateMap<ColetaViewModel, Coleta>();


        }

    }
}