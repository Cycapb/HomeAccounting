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
using WebUI.Core.Abstract.Helpers;
using WebUI.Core.Implementations;
using WebUI.Core.Implementations.Converters;
using WebUI.Core.Implementations.Helpers;
using WebUI.Core.Implementations.Providers;
using WebUI.Core.Infrastructure.Filters;

namespace WebUI.Core.Infrastructure
{
    public static class ServicesRegister
    {
        public static void RegisterAdditionalServices(IServiceCollection services)
        {
            services.AddScoped<CustomErrorAttribute>();
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
            services.AddTransient<IDebtService, DebtService>();
            services.AddTransient<IProductService, ProductService>();

            services.AddTransient<IPayingItemCreator, PayingItemCreator>();
            services.AddTransient<IPayingItemEditViewModelCreator, PayingItemEditViewModelCreator>();
            services.AddTransient<IPayingItemUpdater, PayingItemUpdater>();
            services.AddTransient<IMessageProvider, MessageProvider>();
            services.AddTransient<IReportHelper, ReportHelper>();
            services.AddTransient<ICategoryHelper, CategoryHelper>();

            services.AddTransient<IRepository<NotificationMailBox>, EntityRepositoryCore<NotificationMailBox, AccountingContextCore>>();
            services.AddTransient<IRepository<Category>, EntityRepositoryCore<Category, AccountingContextCore>>();
            services.AddTransient<IRepository<PayingItem>, EntityRepositoryCore<PayingItem, AccountingContextCore>>();
            services.AddTransient<IRepository<Account>, EntityRepositoryCore<Account, AccountingContextCore>>();
            services.AddTransient<IRepository<TypeOfFlow>, EntityRepositoryCore<TypeOfFlow, AccountingContextCore>>();
            services.AddTransient<IRepository<Debt>, EntityRepositoryCore<Debt, AccountingContextCore>>();
            services.AddTransient<IRepository<Product>, EntityRepositoryCore<Product, AccountingContextCore>>();

            services.AddTransient<IServiceTrigger<PayingItem>, PayingItemServiceTrigger>();
        }
    }
}
