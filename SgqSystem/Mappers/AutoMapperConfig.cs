﻿using AutoMapper;

namespace SgqSystem.Mappers
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {

            Mapper.Initialize(x =>
            {
                x.AddProfile<DomainToViewModelMappingProfile>();
                x.AddProfile<CorrectiveActionMappingProfile>();
                x.AddProfile<UserMappingProfile>();
                x.AddProfile<ColetaMappingProfile>();
            });

        }

    }
}