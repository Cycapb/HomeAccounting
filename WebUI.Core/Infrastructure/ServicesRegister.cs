using BussinessLogic.Providers;
using BussinessLogic.Services;
using DomainModels.EntityORM.Core.Infrastructure;
using DomainModels.Model;
using DomainModels.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Providers;
using Services;
using WebUI.Core.Abstract.Converters;
using WebUI.Core.Concrete.Converters;

namespace WebUI.Core.Infrastructure
{
    public static class ServicesRegister
    {
        public static void RegisterAdditionalServices(IServiceCollection services)
        {
            services.AddScoped<CustomErrorAttribute>();

            services.AddTransient<IMailboxService, MailboxService>();
            services.AddTransient<ICategoryService, CategoryService>();            
            services.AddTransient<IRouteDataConverter, RouteDataConverter>();
            services.AddTransient<IRepository<NotificationMailBox>, EntityRepositoryCore<NotificationMailBox, AccountingContextCore>>();
            services.AddTransient<IRepository<Category>, EntityRepositoryCore<Category, AccountingContextCore>>();
            services.AddTransient<ISingleIpAddressProvider, SingleIpAddressProvider>();
            services.AddTransient<IMultipleIpAddressProvider, MultipleIpAddressProvider>();
        }
    }
}
