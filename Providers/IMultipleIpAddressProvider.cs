namespace Providers
{
    /// <summary>
    /// Интерфейс поставщика ip-адресов
    /// </summary>
    public interface IMultipleIpAddressProvider
    {
        /// <summary>
        /// Получает список ip адресов из входной строки
        /// </summary>
        /// <param name="ipAddresses">Список ip через запятую в виде строки</param>
        /// <returns>Список ip через запятую в виде строки</returns>
        string GetIpAddresses(string ipAddresses);
    }
}