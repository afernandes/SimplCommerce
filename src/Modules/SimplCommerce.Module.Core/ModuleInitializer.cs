﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SimplCommerce.Infrastructure.Caching;
using SimplCommerce.Infrastructure.Modules;
using SimplCommerce.Module.Core.Extensions;
using SimplCommerce.Module.Core.Models;
using SimplCommerce.Module.Core.Services;

namespace SimplCommerce.Module.Core
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStaticCacheManager, MemoryCacheManager>();

            serviceCollection.AddTransient<IEntityService, EntityService>();
            serviceCollection.AddTransient<IMediaService, MediaService>();
            serviceCollection.AddTransient<IThemeService, ThemeService>();
            serviceCollection.AddTransient<ITokenService, TokenService>();
            serviceCollection.AddTransient<IWidgetInstanceService, WidgetInstanceService>();
            serviceCollection.AddScoped<SignInManager<User>, SimplSignInManager<User>>();
            serviceCollection.AddScoped<IWorkContext, WorkContext>();
            serviceCollection.AddScoped<IWorkCurrency, WorkCurrency>();
            serviceCollection.AddScoped<ISmsSender, SmsSender>();
            serviceCollection.AddSingleton<SettingDefinitionProvider>();
            serviceCollection.AddScoped<ISettingService, SettingService>();

            serviceCollection.AddTransient<ICurrencyService, CurrencyService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
        }
    }
}
