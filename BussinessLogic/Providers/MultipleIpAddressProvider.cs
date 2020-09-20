using Providers;
using System;
using System.Text;

namespace BussinessLogic.Providers
{
    public class MultipleIpAddressProvider : IMultipleIpAddressProvider
    {
        private readonly ISingleIpAddressProvider _singleIpAddressProvider;

        public MultipleIpAddressProvider(ISingleIpAddressProvider singleIpAddressProvider)
        {
            _singleIpAddressProvider = singleIpAddressProvider;
        }

        public string GetIpAddresses(string ipAdresses)
        {
            var outAdresses = new StringBuilder();
            var adresses = ipAdresses.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var ip in adresses)
            {
                if (outAdresses.Length == 0)
                {
                    outAdresses.Append(_singleIpAddressProvider.GetIpAddress(ip));
                }
                else
                {
                    outAdresses.Append("," + _singleIpAddressProvider.GetIpAddress(ip));
                }
            }

            return outAdresses.ToString();
        }
    }
}