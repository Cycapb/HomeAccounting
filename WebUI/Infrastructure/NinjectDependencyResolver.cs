using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BussinessLogic.Converters;
using BussinessLogic.Loggers;
using BussinessLogic.Providers;
using BussinessLogic.Services;
using Converters;
using DomainModels.Model;
using DomainModels.Repositories;
using WebUI.Abstract;
using WebUI.Concrete;
using WebUI.Helpers;
using Ninject;
using Services;
using DomainModels.EntityORM;
using Loggers;

namespace WebUI.Infrastructure
{
    public class NinjectDependencyResolver:IDependencyResolver
    {
        private readonly IKernel _kernel;        
        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            AddBindings();
        }

        private void AddBindings()
        {

            _kernel.Bind<IRepository<PayingItem>>().To<EntityRepository<PayingItem>>();
            _kernel.Bind<IRepository<Account>>().To<EntityRepository<Account>>();
            _kernel.Bind<IRepository<Category>>().To<EntityRepository<Category>>();
            _kernel.Bind<IRepository<TypeOfFlow>>().To<EntityRepository<TypeOfFlow>>();
            _kernel.Bind<IRepository<Product>>().To<EntityRepository<Product>>();
            _kernel.Bind<IRepository<PaiyngItemProduct>>().To<EntityRepository<PaiyngItemProduct>>();
            _kernel.Bind<IRepository<PlanItem>>().To<EntityRepository<PlanItem>>();
            _kernel.Bind<IRepository<Order>>().To<EntityRepository<Order>>();
            _kernel.Bind<IRepository<OrderDetail>>().To<EntityRepository<OrderDetail>>();
            _kernel.Bind<IReportMenu>().To<ReportMenu>();
            _kernel.Bind<IReporter>().To<EmailUserReporter>();
            _kernel.Bind<IPlanningHelper>().To<PlaningControllerHelper>();
            _kernel.Bind<IPayingItemHelper>().To<PayingItemHelper>();
            _kernel.Bind<IPayingItemProductHelper>().To<PayingItemProductHelper>();
            _kernel.Bind<IPayItemSubcategoriesHelper>().To<PayItemSubcategoriesHelper>();
            _kernel.Bind<IReportControllerHelper>().To<ReportControllerHelper>();
            _kernel.Bind<IReportModelCreator>().To<ReportModelCreator>();
            _kernel.Bind<IDbHelper>().To<DbHelper>();
            _kernel.Bind<IPagingInfoCreator>().To<PagingInfoCreator>();
            _kernel.Bind<IDebtService>().To<DebtService>();
            _kernel.Bind<IRepository<Debt>>().To<EntityRepository<Debt>>();
            _kernel.Bind<IPayingItemService>().To<PayingItemService>();
            _kernel.Bind<ICategoryService>().To<CategoryService>();
            _kernel.Bind<IAccountService>().To<AccountService>();
            _kernel.Bind<IPayingItemProductService>().To<PayingItemProductService>();
            _kernel.Bind<IProductService>().To<ProductService>();
            _kernel.Bind<IMailSettingsProvider>().To<AccountingNotificationMailBoxProvider>();
            _kernel.Bind<IEmailSender>().To<OrderSenderService>();
            _kernel.Bind<IOrderService>().To<OrderService>();
            _kernel.Bind<IOrderDetailService>().To<OrderDetailService>();
            _kernel.Bind<ITypeOfFlowService>().To<TypeOfFlowService>();
            _kernel.Bind<IPlanItemService>().To<PlanItemService>();
            _kernel.Bind<IRepository<NotificationMailBox>>().To<EntityRepository<NotificationMailBox>>();
            _kernel.Bind<IMailboxService>().To<MailboxService>();
            _kernel.Bind<ICategoryHelper>().To<CategoryHelper>();
            _kernel.Bind<IMessageProvider>().To<MessageProvider>();
            _kernel.Bind<IRouteDataConverter>().To<RouteDataConverter>();
            _kernel.Bind<IExceptionLogger>().To<NlogExceptionLogger>();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
           return _kernel.GetAll(serviceType);
        }
    }
}