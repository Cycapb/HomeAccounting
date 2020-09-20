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
        /// <param name="ipAdresses">Список ip через запятую в виде строки</param>
        /// <returns>Список ip через запятую в виде строки</returns>
        string GetIpAddresses(string ipAdresses);
    }
}