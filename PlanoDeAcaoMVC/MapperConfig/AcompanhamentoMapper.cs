using AutoMapper;
using PlanoAcaoCore.Acao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlanoDeAcaoMVC.MapperConfig
{
    public class AcompanhamentoMapper : Profile
    {

        public AcompanhamentoMapper()
        {
            CreateMap<Pa_Acompanhamento, PlanoAcaoEF.Pa_Acompanhamento>();
            CreateMap<PlanoAcaoEF.Pa_Acompanhamento, Pa_Acompanhamento>();
        }

    }
}