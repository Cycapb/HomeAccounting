using Loggers;
using Providers;
using Services.Exceptions;

namespace BussinessLogic.Providers
{
    public class SingleIpAddressProviderLoggingDecorator:ISingleIpAddressProvider
    {
        private readonly ISingleIpAddressProvider _singleIpAddressProvider;
        private readonly IExceptionLogger _exceptionLogger;

        public SingleIpAddressProviderLoggingDecorator(ISingleIpAddressProvider singleIpAddressProvider, IExceptionLogger exceptionLogger)
        {
            _singleIpAddressProvider = singleIpAddressProvider;
            _exceptionLogger = exceptionLogger;
        }

        public string GetIpAddress(string ipAddress)
        {
            try
            {
                return _singleIpAddressProvider.GetIpAddress(ipAddress);
            }
            catch (ServiceException e)
            {
                _exceptionLogger.LogException(e);
                return string.Empty;
            }
        }
    }
}