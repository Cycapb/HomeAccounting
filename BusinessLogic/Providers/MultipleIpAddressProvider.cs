using Providers;
using System;
using System.Text;

namespace BusinessLogic.Providers
{
    public class MultipleIpAddressProvider : IMultipleIpAddressProvider
    {
        private readonly ISingleIpAddressProvider _singleIpAddressProvider;

        public MultipleIpAddressProvider(ISingleIpAddressProvider singleIpAddressProvider)
        {
            _singleIpAddressProvider = singleIpAddressProvider;
        }

        public string GetIpAddresses(string ipAddresses)
        {
            var outAddresses = new StringBuilder();
            var addresses = ipAddresses.Split(new [] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var ip in addresses)
            {
                if (outAddresses.Length == 0)
                {
                    outAddresses.Append(_singleIpAddressProvider.GetIpAddress(ip));
                }
                else
                {
                    outAddresses.Append("," + _singleIpAddressProvider.GetIpAddress(ip));
                }
            }

            return outAddresses.ToString();
        }
    }
}