
﻿using Application.Helpers;
 using Application.Interfaces;
 using Core.Interfaces;
 using FluentValidation;


using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyResolver;

public static class DependencyResolverService
{
    public static void RegisterApplicationLayer(IServiceCollection services)
    {

        services.AddScoped<IEventService, EventService>();
        services.AddScoped<ISlotAnswerService, SlotAnswerService>();       
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<EncryptionService>();

    }
}