namespace Providers
{
    public interface IMultipleIpAddressProvider
    {
        string GetIpAddresses(string ipAdresses);
    }
}