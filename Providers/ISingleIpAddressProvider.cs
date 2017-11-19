namespace Providers
{
    public interface ISingleIpAddressProvider
    {
        string GetIpAddress(string ipAddress);
    }
}