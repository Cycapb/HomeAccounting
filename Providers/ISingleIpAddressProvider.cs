namespace Providers
{
    /// <summary>
    /// Интерфейс поставщика одного ip-адреса
    /// </summary>
    public interface ISingleIpAddressProvider
    {
        /// <summary>
        /// Получает ip адрес из входной строки
        /// </summary>
        /// <param name="ipAddress">IP-адрес в виде строки</param>
        /// <returns>IP-адрес в виде строки</returns>
        string GetIpAddress(string ipAddress);
    }
}