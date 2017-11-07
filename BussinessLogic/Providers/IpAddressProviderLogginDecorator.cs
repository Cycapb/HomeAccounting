using Loggers;
using Providers;
using Services.Exceptions;

namespace BussinessLogic.Providers
{
    public class IpAddressProviderLogginDecorator:IIpAddressProvider
    {
        private readonly IExceptionLogger _exceptionLogger;
        private readonly IIpAddressProvider _decoratedIpAddressProvider;

        public IpAddressProviderLogginDecorator(IIpAddressProvider ipAddressProvider, IExceptionLogger exceptionLogger)
        {
            _decoratedIpAddressProvider = ipAddressProvider;
            _exceptionLogger = exceptionLogger;
        }

        public string GetIpAddress(string ipAddress)
        {
            try
            {
                return _decoratedIpAddressProvider.GetIpAddress(ipAddress);
            }
            catch (ServiceException e)
            {
                _exceptionLogger.LogException(e);
                return string.Empty;
            }
        }
    }
}