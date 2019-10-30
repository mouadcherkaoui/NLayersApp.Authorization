using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using System.Reflection;
using NLayersApp.Persistence.Abstractions;
using NLayersApp.CQRS.Requests;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NLayersApp.CQRS.Handlers;

namespace NLayersApp.CQRS.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddMediatRHandlers(this IServiceCollection services, ITypesResolver typesResolver)
        {
            foreach (var type in typesResolver.RegisteredTypes)
            {
                var keyType = type.GetProperties().FirstOrDefault(p =>  p.GetCustomAttribute<KeyAttribute>() != null)?.PropertyType ?? typeof(object);

                var requestType = typeof(CreateEntityRequest<>).MakeGenericType(type);
                var returnType = type;

                var serviceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, returnType);
                var implementationType = typeof(CreateEntityHandler<,>).MakeGenericType(keyType, type);

                services.AddScoped(serviceType, implementationType);

                requestType = typeof(ReadEntityRequest<,>).MakeGenericType(keyType, type);
                returnType = type;

                serviceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, returnType);
                implementationType = typeof(ReadEntityHandler<,>).MakeGenericType(keyType, type);
                
                services.AddScoped(serviceType, implementationType);

                requestType = typeof(ReadEntitiesRequest<>).MakeGenericType(type);
                returnType = typeof(IEnumerable<>).MakeGenericType(type);

                serviceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, returnType);
                implementationType = typeof(ReadEntitiesHandler<>).MakeGenericType(type);

                services.AddScoped(serviceType, implementationType);

                requestType = typeof(UpdateEntityRequest<,>).MakeGenericType(keyType, type);
                returnType = type;

                serviceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, returnType);
                implementationType = typeof(UpdateEntityHandler<,>).MakeGenericType(keyType, type);

                services.AddScoped(serviceType, implementationType);

                requestType = typeof(DeleteEntityRequest<,>).MakeGenericType(keyType, type);
                returnType = typeof(bool);

                serviceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, returnType);
                implementationType = typeof(DeleteEntityHandler<,>).MakeGenericType(keyType, type);

                services.AddScoped(serviceType, implementationType);
            }

            services.AddMediatR(Assembly.GetEntryAssembly());
            return services;
        }
    }
}
