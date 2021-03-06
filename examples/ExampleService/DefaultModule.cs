﻿using ExampleBusiness;
using ExampleBusiness.CommandHandlers;
using ExampleBusiness.EventHandlers;
using ExampleCommon;
using ExampleCommon.Events;
using Vanguard.Framework.Core.DomainEvents;

namespace ExampleService
{
    using Autofac;
    using ExampleData;
    using ExampleData.Entities;
    using ExampleService.Extensions;
    using ExampleService.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Vanguard.Framework.Core.Cqrs;
    using Vanguard.Framework.Data.Repositories;

    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpContextAccessor>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CommandDispatcher>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<QueryDispatcher>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<EventDispatcher>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<Mapper>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterGeneric(typeof(Repository<>)).AsImplementedInterfaces();
            builder.RegisterGeneric(typeof(ReadRepository<>)).AsImplementedInterfaces();
            builder.RegisterType<ExampleContext>().As<DbContext>();

            var businessLayer = typeof(BusinessLayer).Assembly;
            builder.RegisterAssemblyTypes(businessLayer)
                .Where(type => type.Name.EndsWith("CommandHandler"))
                .AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(businessLayer)
                .Where(type => type.Name.EndsWith("QueryHandler"))
                .AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(businessLayer)
                .Where(type => type.Name.EndsWith("EventHandler"))
                .AsImplementedInterfaces();

            builder.RegisterCrudHandlers<CarModel, Car>();
        }
    }
}
