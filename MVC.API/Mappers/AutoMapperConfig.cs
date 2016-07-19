using AutoMapper;
using Dominio.Entities;
using MVC.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.API.Mappers
{
    public class AutoMapperConfig
    {
         public static void RegisterMappings()
        {
          
            Mapper.Initialize(x =>
            {
                //x.CreateMap<GenericReturn<T>, GenericReturnViewModel<T>>();
                x.AddProfile<DomainToViewModelMappingProfile>();
                x.AddProfile<ViewModelToDomainMappingProfile>();
            });

            
        }

    }
}