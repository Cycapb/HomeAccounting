using System;
using System.Net;
using Providers;
using Services.Exceptions;

namespace BussinessLogic.Providers
{
    public class SingleIpAddressProvider : ISingleIpAddressProvider
    {
        private const string IpV6LoopBack = "::1";

        public string GetIpAddress(string ipAddress)
        {
            try
            {
                IPAddress ip;
                
                if (ipAddress.Contains(":") && ipAddress.IndexOf(':') == 4)
                {
                    ip = IPAddress.Parse(ipAddress);
                    return ip.ToString();
                }

                if (ipAddress == IpV6LoopBack)
                {
                    return IPAddress.Parse(ipAddress).ToString();
                }

                if(ipAddress.Contains(":"))
                {                    
                    var ipAddressWithoutPort = ipAddress.Substring(0, ipAddress.Length - ipAddress.Substring(ipAddress.IndexOf(':')).Length);
                    ip = IPAddress.Parse(ipAddressWithoutPort);
                    return ip.ToString();
                }

                ip = IPAddress.Parse(ipAddress);
                return ip.ToString();
            }
            catch (ArgumentNullException e)
            {
                throw new ServiceException($"Ошибка в аргументе {nameof(ipAddress)} при получении IP. Получено значение {ipAddress}", e);
            }
            catch (NullReferenceException e)
            {
                throw new ServiceException($"Ошибка в аргументе {nameof(ipAddress)} при получении IP. Получено значение {ipAddress}", e);
            }
            catch (FormatException e)
            {
                throw new ServiceException($"Неверный формат данных параметра {nameof(ipAddress)}. Получено значение {ipAddress}", e);
            }
            catch (Exception e)
            {
                throw new ServiceException($"Ошибка при получении IP из параметра {nameof(ipAddress)}. Получено значение {ipAddress}", e);
            }
        }
    }
}