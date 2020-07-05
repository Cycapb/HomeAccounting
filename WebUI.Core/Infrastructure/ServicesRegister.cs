using BussinessLogic.Providers;
using BussinessLogic.Services;
using BussinessLogic.Services.Triggers;
using DomainModels.EntityORM.Core.Infrastructure;
using DomainModels.Model;
using DomainModels.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Providers;
using Services;
using Services.Triggers;
using WebUI.Core.Abstract;
using WebUI.Core.Abstract.Converters;
using WebUI.Core.Concrete.Converters;
using WebUI.Core.Concrete.Providers;
using WebUI.Core.Implementations;
using WebUI.Core.Infrastructure.Attributes;
using WebUI.Core.Infrastructure.Filters;

namespace WebUI.Core.Infrastructure
{
    public static class ServicesRegister
    {
        public static void RegisterAdditionalServices(IServiceCollection services)
        {
            services.AddScoped<CustomErrorAttribute>();
            services.AddScoped<IsUniqueAttribute>();
            services.AddScoped<UserHasAnyAccount>();
            services.AddScoped<UserHasAnyCategory>();
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IMailboxService, MailboxService>();
            services.AddTransient<ICategoryService, CategoryService>();            
            services.AddTransient<IRouteDataConverter, RouteDataConverter>();
            services.AddTransient<ISingleIpAddressProvider, SingleIpAddressProvider>();
            services.AddTransient<IMultipleIpAddressProvider, MultipleIpAddressProvider>();
            services.AddTransient<IPayingItemService, PayingItemService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ITypeOfFlowService, TypeOfFlowService>();

            services.AddTransient<IPayingItemCreator, PayingItemCreator>();
            services.AddTransient<IPayingItemEditViewModelCreator, PayingItemEditViewModelCreator>();
            services.AddTransient<IPayingItemUpdater, PayingItemUpdater>();
            services.AddTransient<IMessageProvider, MessageProvider>();
            services.AddTransient<IReportHelper, ReportHelper>();

            services.AddTransient<IRepository<NotificationMailBox>, EntityRepositoryCore<NotificationMailBox, AccountingContextCore>>();
            services.AddTransient<IRepository<Category>, EntityRepositoryCore<Category, AccountingContextCore>>();
            services.AddTransient<IRepository<Category>, EntityRepositoryCore<Category, AccountingContextCore>>();
            services.AddTransient<IRepository<PayingItem>, EntityRepositoryCore<PayingItem, AccountingContextCore>>();
            services.AddTransient<IRepository<Account>, EntityRepositoryCore<Account, AccountingContextCore>>();
            services.AddTransient<IRepository<TypeOfFlow>, EntityRepositoryCore<TypeOfFlow, AccountingContextCore>>();

            services.AddTransient<IServiceTrigger<PayingItem>, PayingItemServiceTrigger>();
        }
    }
}
