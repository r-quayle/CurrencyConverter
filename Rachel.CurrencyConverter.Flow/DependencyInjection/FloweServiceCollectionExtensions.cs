using Microsoft.Extensions.DependencyInjection;
using Rachel.CurrencyConverter.Flow.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rachel.CurrencyConverter.Flow
{
    public static class FloweServiceCollectionExtensions
    {
        public static IServiceCollection AddFlowServices(this IServiceCollection services)
        {
            services.AddTransient<ICurrencyConverter, CurrencyConverter>();
            return services;
        }
    }
}
