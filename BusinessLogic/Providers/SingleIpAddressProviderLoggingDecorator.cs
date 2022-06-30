using Providers;
using Serilog;
using Services.Exceptions;

namespace BusinessLogic.Providers
{
    public class SingleIpAddressProviderLoggingDecorator:ISingleIpAddressProvider
    {
        private readonly ISingleIpAddressProvider _singleIpAddressProvider;
        private readonly ILogger _logger = Log.Logger.ForContext<SingleIpAddressProviderLoggingDecorator>();

        public SingleIpAddressProviderLoggingDecorator(ISingleIpAddressProvider singleIpAddressProvider)
        {
            _singleIpAddressProvider = singleIpAddressProvider;            
        }

        public string GetIpAddress(string ipAddress)
        {
            try
            {
                return _singleIpAddressProvider.GetIpAddress(ipAddress);
            }
            catch (ServiceException e)
            {
                _logger.Error(e, "Ошибка при попытке получения IP-адреса");
                return string.Empty;
            }
        }
    }
}