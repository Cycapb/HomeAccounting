using Autofac;
using BusinessLogic.Providers;
using BusinessLogic.Services;
using BussinnessLogic.Services;
using BussinnessLogic.Services.Triggers;
using DomainModels.EntityORM.Core.Infrastructure;
using DomainModels.Model;
using DomainModels.Repositories;
using Microsoft.AspNetCore.Http;
using Paginator.Abstract;
using Paginator.Concrete;
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

namespace WebUI.Core.Configuration
{
    public class WebUiCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomErrorFilter>().InstancePerLifetimeScope();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();
            builder.RegisterType<RouteDataConverter>().As<IRouteDataConverter>();

            builder.RegisterType<SingleIpAddressProvider>().As<ISingleIpAddressProvider>();
            builder.RegisterType<MultipleIpAddressProvider>().As<IMultipleIpAddressProvider>();

            builder.RegisterType<CreateCloseDebtService>().As<ICreateCloseDebtService>();
            builder.RegisterDecorator<CreateCloseDebtServicePayingItemDecorator, ICreateCloseDebtService>();
            builder.RegisterType<MailboxService>().As<IMailboxService>();
            builder.RegisterType<CategoryService>().As<ICategoryService>();
            builder.RegisterType<PayingItemService>().As<IPayingItemService>();
            builder.RegisterType<CategoryService>().As<ICategoryService>();
            builder.RegisterType<AccountService>().As<IAccountService>();
            builder.RegisterType<TypeOfFlowService>().As<ITypeOfFlowService>();
            builder.RegisterType<DebtService>().As<IDebtService>();
            builder.RegisterType<ProductService>().As<IProductService>();
            builder.RegisterType<OrderService>().As<IOrderService>();
            builder.RegisterType<EmailSenderService>().As<IEmailSender>();
            builder.RegisterType<AccountingNotificationMailBoxProvider>().As<IMailSettingsProvider>();

            builder.RegisterType<PayingItemCreator>().As<IPayingItemCreator>();
            builder.RegisterType<PayingItemEditViewModelCreator>().As<IPayingItemEditViewModelCreator>();
            builder.RegisterType<PayingItemUpdater>().As<IPayingItemUpdater>();
            builder.RegisterType<MessageProvider>().As<IMessageProvider>();
            builder.RegisterType<ReportHelper>().As<IReportHelper>();
            builder.RegisterType<CategoryHelper>().As<ICategoryHelper>();
            builder.RegisterType<PayItemSubcategoriesHelper>().As<IPayItemSubcategoriesHelper>();
            builder.RegisterType<ReportControllerHelper>().As<IReportControllerHelper>();
            builder.RegisterType<ReportModelCreator>().As<IReportModelCreator>();
            builder.RegisterType<AjaxPageCreator>().As<IPageCreator>();
            builder.RegisterType<PagingInfoCreator>().As<IPagingInfoCreator>();
            builder.RegisterType<Paginator.Concrete.Paginator>().As<IPaginator>();

            builder.RegisterType<EntityRepositoryCore<NotificationMailBox, AccountingContextCore>>().As<IRepository<NotificationMailBox>>();
            builder.RegisterType<EntityRepositoryCore<Category, AccountingContextCore>>().As<IRepository<Category>>();
            builder.RegisterType<EntityRepositoryCore<Account, AccountingContextCore>>().As<IRepository<Account>>();
            builder.RegisterType<EntityRepositoryCore<TypeOfFlow, AccountingContextCore>>().As<IRepository<TypeOfFlow>>();
            builder.RegisterType<EntityRepositoryCore<Debt, AccountingContextCore>>().As<IRepository<Debt>>();
            builder.RegisterType<EntityRepositoryCore<Product, AccountingContextCore>>().As<IRepository<Product>>();
            builder.RegisterType<EntityRepositoryCore<PayingItem, AccountingContextCore>>().As<IRepository<PayingItem>>();
            builder.RegisterType<EntityRepositoryCore<Order, AccountingContextCore>>().As<IRepository<Order>>();

            builder.RegisterType<PayingItemServiceTrigger>().As<IServiceTrigger<PayingItem>>();
        }
    }
}
