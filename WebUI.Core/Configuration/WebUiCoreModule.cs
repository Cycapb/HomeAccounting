using Autofac;
using BussinessLogic.Providers;
using BussinessLogic.Services;
using Providers;
using Services;

namespace WebUI.Core.Configuration
{
    public class WebUiCoreModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CreateCloseDebtService>().As<ICreateCloseDebtService>();
            builder.RegisterDecorator<CreateCloseDebtServicePayingItemDecorator, ICreateCloseDebtService>();
            builder.RegisterType<SingleIpAddressProvider>().As<ISingleIpAddressProvider>();
            builder.RegisterType<MultipleIpAddressProvider>().As<IMultipleIpAddressProvider>();
        }
    }
}
