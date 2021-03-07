﻿using Autofac;
using BussinessLogic.Providers;
using BussinessLogic.Services;
using BussinessLogic.Services.Triggers;
using DomainModels.EntityORM.Core.Infrastructure;
using DomainModels.Model;
using DomainModels.Repositories;
using Microsoft.AspNetCore.Http;
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
    public class WebUiCoreModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomErrorAttribute>().InstancePerLifetimeScope();
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

            builder.RegisterType<PayingItemCreator>().As<IPayingItemCreator>();
            builder.RegisterType<PayingItemEditViewModelCreator>().As<IPayingItemEditViewModelCreator>();
            builder.RegisterType<PayingItemUpdater>().As<IPayingItemUpdater>();
            builder.RegisterType<MessageProvider>().As<IMessageProvider>();
            builder.RegisterType<ReportHelper>().As<IReportHelper>();
            builder.RegisterType<CategoryHelper>().As<ICategoryHelper>();

            builder.RegisterType<EntityRepositoryCore<NotificationMailBox, AccountingContextCore>>().As<IRepository<NotificationMailBox>>();
            builder.RegisterType<EntityRepositoryCore<Category, AccountingContextCore>>().As<IRepository<Category>>();
            builder.RegisterType<EntityRepositoryCore<Account, AccountingContextCore>>().As<IRepository<Account>>();
            builder.RegisterType<EntityRepositoryCore<TypeOfFlow, AccountingContextCore>>().As<IRepository<TypeOfFlow>>();
            builder.RegisterType<EntityRepositoryCore<Debt, AccountingContextCore>>().As<IRepository<Debt>>();
            builder.RegisterType<EntityRepositoryCore<Product, AccountingContextCore>>().As<IRepository<Product>>();
            builder.RegisterType<EntityRepositoryCore<PayingItem, AccountingContextCore>>().As<IRepository<PayingItem>>();

            builder.RegisterType<PayingItemServiceTrigger>().As<IServiceTrigger<PayingItem>>();
        }
    }
}