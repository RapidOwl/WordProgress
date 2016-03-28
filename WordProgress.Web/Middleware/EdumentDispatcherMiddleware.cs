using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using WordProgress.Edument;

namespace WordProgress.Web.Middleware
{
    public static class EdumentExtensions
    {
        public static IServiceCollection AddEdumentDispatcher(this IServiceCollection services)
        {
            services.AddSingleton<IEventStore, InMemoryEventStore>();
            services.AddSingleton<IMessageDispatcher, MessageDispatcher>();

            return services;
        }

        public static IApplicationBuilder UseEdumentDispatcher(this IApplicationBuilder builder, IEnumerable<Type> typesToScan)
        {
            return builder.UseMiddleware<EdumentDispatcherMiddleware>(typesToScan);
        }
    }

    public class EdumentDispatcherMiddleware
    {
        private readonly RequestDelegate _next;

        public EdumentDispatcherMiddleware(RequestDelegate next, IMessageDispatcher messageDispatcher, IEnumerable<Type> typesToScan)
        {
            messageDispatcher.Setup(typesToScan);

            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next.Invoke(context);
        }
    }
}
